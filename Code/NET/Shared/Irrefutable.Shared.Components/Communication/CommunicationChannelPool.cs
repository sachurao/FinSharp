using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.Shared.Interfaces.Communication;
using System.Collections.Concurrent;
using Irrefutable.Shared.Utilities.ObjectType;
using System.Threading;
using Irrefutable.Shared.Utilities;

namespace Irrefutable.Shared.Components.Communication
{
    class CommunicationChannelPool:ICommunicationChannelPool
    {

        #region Member Variables
        
        private BlockingCollection<IMessageSenderChannel> _messageSenderChannels;
        private BlockingCollection<IMessageReceiverChannel> _messageReceiverChannels;
        private bool _disposed = false;

        #endregion

        #region Init

        public CommunicationChannelPool()
        {
            _messageSenderChannels = new BlockingCollection<IMessageSenderChannel>(
                new ConcurrentStack<IMessageSenderChannel>());
            _messageReceiverChannels = new BlockingCollection<IMessageReceiverChannel>(
                new ConcurrentQueue<IMessageReceiverChannel>());
        }
        
        #endregion

        #region ICommunicationChannelPool

        public void AddSenderChannel(IMessageSenderChannel senderChannel)
        {
            _messageSenderChannels.Add(senderChannel);
            _messageSenderChannels.CompleteAdding();
        }

        public void AddReceiverChannel(IMessageReceiverChannel receiverChannel)
        {
            _messageReceiverChannels.Add(receiverChannel);
            _messageReceiverChannels.CompleteAdding();
        }

        public IMessageSenderChannel GetSenderChannel()
        {
            var senderChannel = _messageSenderChannels.Take();
            LazyInitializer.EnsureInitialized<IMessageSenderChannel>(ref senderChannel,
                () =>
                {
                    senderChannel.Connect();
                    return senderChannel;
                });
            return senderChannel;
        }

        public void PutSenderChannel(IMessageSenderChannel senderChannel)
        {
            _messageSenderChannels.Add(senderChannel);
        }

        public void Subscribe(IMessageDestination topic, IMessageHandler messageHandler)
        {
            IMessageReceiverChannel receiverChannel = null;
            try
            {
                receiverChannel = GetReceiverChannel();
                receiverChannel.Subscribe(topic, messageHandler);
            }
            finally
            {
                if (receiverChannel != null) PutReceiverChannel(receiverChannel);
            }
        }

        public void Unsubscribe(IMessageDestination topic)
        {
                _messageReceiverChannels.Where(
                channel => channel.Subscriptions.Any(
                msgDest => msgDest.Address.Equals(topic.Address)))
                .Do(r => r.Unsubscribe(topic));
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _messageReceiverChannels.Do(receiver => receiver.Disconnect());
                    _messageReceiverChannels = null;
                    
                    _messageSenderChannels.Do(sender => sender.Disconnect());
                    _messageSenderChannels = null;
                }
                _disposed = true;
            }
        }
        
        #endregion

        #region Private Methods

        private IMessageReceiverChannel GetReceiverChannel()
        {
            var receiverChannel = _messageReceiverChannels.Take();
            LazyInitializer.EnsureInitialized<IMessageReceiverChannel>(ref receiverChannel,
                () =>
                {
                    receiverChannel.Connect();
                    return receiverChannel;
                });
            return receiverChannel;
        }

        private void PutReceiverChannel(IMessageReceiverChannel receiverChannel)
        {
            _messageReceiverChannels.Add(receiverChannel);
        }

        #endregion
    }
}
