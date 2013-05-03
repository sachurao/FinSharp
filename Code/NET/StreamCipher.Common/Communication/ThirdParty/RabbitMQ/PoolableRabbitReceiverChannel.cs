using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamCipher.Common.Communication.Impl;
using StreamCipher.Common.Communication.ThirdParty.RabbitMQ;
using StreamCipher.Common.Logging;
using StreamCipher.Common.Utilities.Concurrency;

namespace StreamCipher.Common.Communication.ThirdParty.RabbitMQ
{
    public class PoolableRabbitReceiverChannel: BaseRabbitCommunicationChannel, IMessageReceiverChannel
    {
        //private static SharedQueue _sharedQueue = new SharedQueue();
        private readonly ThreadSafe _threadSafeExecutor = new ThreadSafe();
        private readonly IncomingMessageManager _incomingMessageManager;
        private readonly IDictionary<IMessageDestination, string> _consumerDict; 
        
        #region Init

        public PoolableRabbitReceiverChannel(ICommunicationServiceConfig config,
                                           int instanceNum)
            : base(config, "Receiver_" + instanceNum.ToString())
        {
            _incomingMessageManager = new IncomingMessageManager();
            _consumerDict = new Dictionary<IMessageDestination, string>();
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
                                   foreach (var subscribedTopic in _incomingMessageManager.KnownMessageProcessors.Keys)
                                   {
                                       UnsubscribeCore(subscribedTopic);
                                   }
                               });
        }

        public IEnumerable<IMessageDestination> Subscriptions
        {
            get { return _incomingMessageManager.KnownMessageProcessors.Keys; }
        }

        #endregion

        protected void SubscribeCore(IMessageDestination messageDestination, Action<IIncomingMessage> incomingMessageHandler)
        {
            Logger.Debug(this, String.Format("{0}: Subscribing to topic {1}", this.ConnectionId, messageDestination.Address));
            String topic = messageDestination.Address;
            if (!_incomingMessageManager.KnownMessageProcessors.Keys.Contains(messageDestination))
            {
                _session.Use.QueueDeclare(topic, false, false, false, null);
                _session.Use.QueueBind(topic, EXCHANGE_NAME, topic);
                
            }


            var messageProcessor = new DefaultBroadcastProcessor();
            _incomingMessageManager.KnownMessageProcessors.GetOrAdd(
                 messageDestination, messageProcessor);
            messageProcessor.OnMessageReceived += incomingMessageHandler;
            
            var consumer = new RabbitMessageConsumer(
                evt =>
                    {
                        try
                        {
                            Logger.Debug(this, "Received new message...");
                            string sender = evt.BasicProperties.AppId;
                            string routingKey = evt.RoutingKey;
                            string correlationId = evt.BasicProperties.CorrelationId;
                            string responseTopic = evt.BasicProperties.ReplyTo;
                            //IMessage
                            var content = _config.Formatter.Deserialize(evt.Body);
                            var incomingMessage = new IncomingMessage(content, correlationId,
                                sender,
                                new MessageDestination(routingKey),
                                new MessageDestination(responseTopic));
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

        protected override void DisconnectCore()
        {
            _incomingMessageManager.Dispose();
            base.DisconnectCore();
        }


    }
}
