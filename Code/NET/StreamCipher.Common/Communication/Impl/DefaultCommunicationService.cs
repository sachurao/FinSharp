using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamCipher.Common.Async;
using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.Pooling;
using StreamCipher.Common.Utilities;

namespace StreamCipher.Common.Communication.Impl
{
    public class DefaultCommunicationService: ControlledComponent, ICommunicationService
    {
        #region Member Variables

        private readonly IMessageSenderChannelPool _senderChannelPool;
        private readonly IMessageReceiverChannelPool _receiverChannelPool; 

        #endregion

        #region Init

        public DefaultCommunicationService(ICommunicationServiceConfig config,
            IPoolableObjectFactory<IMessageSenderChannel> senderChannelFactory,
            IPoolableObjectFactory<IMessageReceiverChannel> receiverChannelFactory, 
            string name = "DefaultCommunicationService",
            Int32 instanceNum = 1) : base(name, instanceNum)
        {
            var senderChannelPoolConfig = new ObjectPoolConfig<IMessageSenderChannel>.Builder()
                {
                    SetValidateBeforeBorrow = true,
                    SetCapacity = config.TotalSenderChannels,
                    SetMaximumObjectsActiveOnStartup = config.TotalSenderChannels,
                    Factory = senderChannelFactory
                }.Build();
            _senderChannelPool = new DefaultMessageSenderChannelPool(senderChannelPoolConfig);

            var receiverChannelPoolConfig = new ObjectPoolConfig<IMessageReceiverChannel>.Builder()
            {
                SetCapacity = config.TotalReceiverChannels,
                SetMaximumObjectsActiveOnStartup = config.TotalReceiverChannels,
                SetValidateBeforeBorrow = true,
                Factory = receiverChannelFactory
            }.Build();
            _receiverChannelPool = new DefaultMessageReceiverChannelPool(
                receiverChannelPoolConfig, config.UniqueResponseTopic);
        }

        #endregion

        #region ICommunicationService

        public void Send(IMessageDestination topicOrQueue, IMessageWrapper message,
                         Action<IIncomingMessage> responseHandler = null)
        {
            _senderChannelPool.UsingObjectFromPool(
            senderChannel =>
            {
                if (responseHandler != null)
                {
                    _receiverChannelPool.ResponseProcessor.
                        AddSpecificResponseHandler(message.CorrelationId, responseHandler);
                    senderChannel.Send(topicOrQueue, message, 
                        _receiverChannelPool.UniqueResponseDestination);
                }
                else
                {
                    senderChannel.Send(topicOrQueue, message);
                }
            });
        }

        public Task<IIncomingMessage> SendAsync(IMessageDestination topicOrQueue, 
            IMessageWrapper message)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(IMessageDestination topicOrQueue, Action<IIncomingMessage> incomingMessageHandler)
        {
            _receiverChannelPool.UsingObjectFromPool(c => c.Subscribe(
                topicOrQueue, incomingMessageHandler));
        }

        public void Unsubscribe(IMessageDestination topicOrQueue)
        {
            bool unsubscribed = false;
            while (!unsubscribed)
            {
                _receiverChannelPool.UsingObjectFromPool(
                    rc =>
                        {
                            if (rc.Subscriptions
                                  .Any(msgDest => msgDest.Address.Equals(topicOrQueue.Address)))
                            {
                                rc.Unsubscribe(topicOrQueue);
                                unsubscribed = true;
                            }
                        });
            }
        }

        #endregion

        #region ControlledComponent

        protected override void StartCore()
        {

            _senderChannelPool.Start();
            _receiverChannelPool.Start();
            _receiverChannelPool.UsingObjectFromPool(
                rc => rc.Subscribe(_receiverChannelPool.UniqueResponseDestination,
                    _receiverChannelPool.ResponseProcessor.ProcessMessageReceived));
            base.StartCore();
        }

        protected override void ShutdownCore()
        {
            _senderChannelPool.Shutdown();
            _receiverChannelPool.Shutdown();
            base.ShutdownCore();
        }

        #endregion

    }
}
