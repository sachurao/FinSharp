using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Util;
using StreamCipher.Common.Components.Communication;
using StreamCipher.Common.Components.Logging;
using StreamCipher.Common.Interfaces.Communication;
using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.Utilities.Concurrency;

namespace StreamCipher.Common.ThirdParty.Communication.RabbitMQ
{
    public class RabbitMessageReceiverChannel: BaseRabbitCommunicationChannel, IMessageReceiverChannel
    {
        //private static SharedQueue _sharedQueue = new SharedQueue();
        private readonly ThreadSafe _threadSafeExecutor = new ThreadSafe();
        private readonly IncomingMessageManager _incomingMessageManager;
        private readonly IDictionary<IMessageDestination, string> _consumerDict; 

        #region Init
        
        public RabbitMessageReceiverChannel(IDataInterchangeFormatter formatter,
            ICommunicationServiceConfig config, int instanceNum,
            Action<Exception> defaultExceptionHandler, IncomingMessageManager incomingMessageManager) :
            base(formatter, config, "Receiver_" + instanceNum.ToString(), defaultExceptionHandler)
        {
            
            _consumerDict = new Dictionary<IMessageDestination, string>();
            _incomingMessageManager = incomingMessageManager;
        }

        #endregion

        #region Implementation of IMessageReceiverChannel

        public void Subscribe(IMessageDestination topicOrQueue, Action<IIncomingMessage> incomingMessageHandler)
        {
            _threadSafeExecutor.Do(() => SubscribeCore(topicOrQueue, incomingMessageHandler));
        }

        public void Unsubscribe(IMessageDestination topicOrQueue)
        {
            _threadSafeExecutor.Do(() => UnsubscribeCore(topicOrQueue));
        }

        public void UnsubscribeAll()
        {
            _threadSafeExecutor.Do(() =>
                               {
                                   foreach (var subscribedTopic in _incomingMessageManager.SubscribedTopics.Keys)
                                   {
                                       UnsubscribeCore(subscribedTopic);
                                   }
                               });
        }

        public IEnumerable<IMessageDestination> Subscriptions
        {
            get { return _incomingMessageManager.SubscribedTopics.Keys; }
        }

        #endregion

        protected void SubscribeCore(IMessageDestination messageDestination, Action<IIncomingMessage> incomingMessageHandler)
        {
            Logger.Debug(this, String.Format("{0}: Subscribing to topic {1}", this.ConnectionId, messageDestination.Address));
            String topic = messageDestination.Address;
            if (!_incomingMessageManager.SubscribedTopics.Keys.Contains(messageDestination))
            {
                _session.Use.QueueDeclare(topic, false, false, false, null);
                _session.Use.QueueBind(topic, EXCHANGE_NAME, topic);
                
            }
            var messageProcessor = _incomingMessageManager.SubscribedTopics.GetOrAdd(
                messageDestination, new DefaultMessageProcessor());
            messageProcessor.OnMessageReceived += incomingMessageHandler;
            
            var consumer = new RabbitMessageConsumer(
                evt =>
                    {
                        try
                        {
                            Logger.Debug(this, "Received new message...");
                            string sender = evt.BasicProperties.ReplyTo;
                            string routingKey = evt.RoutingKey;
                            string correlationId = evt.BasicProperties.CorrelationId;
                            var content = _formatter.Deserialize(evt.Body);
                            var incomingMessage = new IncomingMessage(content, correlationId,
                                new MessageDestination(sender),
                                new MessageDestination(routingKey));
                            _incomingMessageManager.IncomingMessageQueue.Add(incomingMessage);
                        }
                        finally
                        {
                            //Take it off the message queue
                            _session.Use.BasicAck(evt.DeliveryTag, false);                            
                        }
                        
                    });

            var consumerTag = _session.Use.BasicConsume(topic, false, consumer);
            _consumerDict.Add(messageDestination, consumerTag);
            Logger.Debug(this, String.Format("{0}: Subscribed to topic {1}", this.ConnectionId, messageDestination.Address));
        }

        protected void UnsubscribeCore(IMessageDestination messageDestination)
        {
            if (_consumerDict.ContainsKey(messageDestination))
            {
                var consumerTag = _consumerDict[messageDestination];
                if (!String.IsNullOrEmpty(consumerTag))
                    _session.Use.BasicCancel(consumerTag);
                _consumerDict.Remove(messageDestination);
            }
        }

        


    }
}
