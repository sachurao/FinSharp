using System;
using System.Collections.Concurrent;
using System.Linq;
using StreamCipher.Common.Logging;
using StreamCipher.Common.Utilities;
using StreamCipher.Common.Utilities.Concurrency;

namespace StreamCipher.Common.Communication.Impl
{
    //TODO: Refactor CommunicationChannelPool to a generic object pool.
    class CommunicationChannelPool:ICommunicationChannelPool
    {
        
        #region Member Variables
        
        private BlockingCollection<IMessageSenderChannel> _messageSenderChannels;
        private BlockingCollection<IMessageReceiverChannel> _messageReceiverChannels;
        private IProcessResponses _defaultResponseProcessor; 
        private AtomicBoolean _isInitialised = new AtomicBoolean(false);
        private bool _disposed = false;

        #endregion

        #region Init

        public CommunicationChannelPool()
        {
            _messageSenderChannels = new BlockingCollection<IMessageSenderChannel>(
                new ConcurrentQueue<IMessageSenderChannel>());
            _messageReceiverChannels = new BlockingCollection<IMessageReceiverChannel>(
                new ConcurrentQueue<IMessageReceiverChannel>());
            _defaultResponseProcessor = new DefaultResponseProcessor();
        }
        
        #endregion

        #region ICommunicationChannelPool

        public void AddSenderChannel(IMessageSenderChannel senderChannel)
        {
            if (_isInitialised.Value) throw new InvalidOperationException("Channel pool is already active.");
            _messageSenderChannels.Add(senderChannel);
            //_messageSenderChannels.CompleteAdding();
        }

        public void AddReceiverChannel(IMessageReceiverChannel receiverChannel)
        {
            if (_isInitialised.Value) throw new InvalidOperationException("Channel pool is already active.");
            _messageReceiverChannels.Add(receiverChannel);
            //_messageReceiverChannels.CompleteAdding();
        }

        public void ActivatePool()
        {
            if (_isInitialised.CompareAndSet(false, true))
            {
                //_messageSenderChannels.CompleteAdding();
                //_messageReceiverChannels.CompleteAdding();
            }
        }

        public IMessageSenderChannel GetSenderChannel()
        {
            var senderChannel = _messageSenderChannels.Take();
            
            //Only one thread has access to this instance of IMessageSenderChannel from here on...
            if (!senderChannel.IsConnected) 
                senderChannel.Connect();
            return senderChannel;
        }

        public void PutSenderChannel(IMessageSenderChannel senderChannel)
        {
            _messageSenderChannels.Add(senderChannel);
        }

        public IMessageDestination UniqueResponseDestination { get; set; }

        public IProcessResponses ResponseProcessor
        {
            get { return _defaultResponseProcessor; }
        }

        public void Subscribe(IMessageDestination topic, Action<IIncomingMessage> incomingMessageHandler)
        {
            IMessageReceiverChannel receiverChannel = null;
            try
            {
                receiverChannel = GetReceiverChannel();
                receiverChannel.Subscribe(topic, incomingMessageHandler);
            }
            finally
            {
                if (receiverChannel != null) PutReceiverChannel(receiverChannel);
            }
        }

        public void Unsubscribe(IMessageDestination topic)
        {
            //TODO: Have to make Unsubscribe method thread safe.
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
                    Logger.Info(this, "Disconnecting receiver channels...");
                    _messageReceiverChannels.Do(receiver => receiver.Disconnect());
                    _messageReceiverChannels = null;

                    Logger.Info(this, "Disconnecting sender channels...");
                    _messageSenderChannels.Do(sender => sender.Disconnect());
                    _messageSenderChannels = null;
                    Logger.Info(this, "Disconnected.");
                    
                }
                _disposed = true;
            }
        }
        
        #endregion

        #region Private Methods

        private IMessageReceiverChannel GetReceiverChannel()
        {
            var receiverChannel = _messageReceiverChannels.Take();

            //Only one thread has access to this instance of receiver channel from here on...
            if (!receiverChannel.IsConnected)
                receiverChannel.Connect();
            return receiverChannel;
        }

        private void PutReceiverChannel(IMessageReceiverChannel receiverChannel)
        {
            _messageReceiverChannels.Add(receiverChannel);
        }

        #endregion
    }
}
