using System;
using System.Collections.Concurrent;
using StreamCipher.Common.Async;
using StreamCipher.Common.Logging;

namespace StreamCipher.Common.Communication.Impl
{
    /// <summary>
    /// This works closely with implementations of <see cref="IMessageReceiverChannel"/>
    /// and provides all the infrastructure necessary to manage incoming messages.
    /// It has a shared background queue that all receiver channels for a communication mode can
    /// drop items into.
    /// It also tracks message handlers that it invokes for each received message in the background queue.
    /// 
    /// </summary>
    public class IncomingMessageManager : IDisposable
    {
        #region Member Variables

        private readonly ConcurrentDictionary<IMessageDestination, IProcessAnyMessage> _knownMessageProcessors;
        private readonly BackgroundQueue<IIncomingMessage> _incomingMessageQueue;
        private bool _disposed = false;

        #endregion

        #region ctor

        public IncomingMessageManager()
        {
            _incomingMessageQueue = new BackgroundQueue<IIncomingMessage>(ProcessIncomingMessage);
            _knownMessageProcessors = new ConcurrentDictionary<IMessageDestination, IProcessAnyMessage>();
            _incomingMessageQueue.Start();
        }

        #endregion
        
        #region Public Properties

        public ConcurrentDictionary<IMessageDestination, IProcessAnyMessage> KnownMessageProcessors
        {
            get { return _knownMessageProcessors; }
        }

        public BackgroundQueue<IIncomingMessage> IncomingMessageQueue
        {
            get { return _incomingMessageQueue; }
        }

        #endregion

        #region Private Methods

        private void ProcessIncomingMessage(IIncomingMessage next)
        {
            Logger.Debug(this, "Finding message processor for this topic.");
            if (!_knownMessageProcessors.ContainsKey(next.Topic))
            {
                Logger.Warn(this,
                            String.Format(
                                "Received message on topic {0} but could not find any registered handlers.  Ignoring this message.",
                                next.Topic));
                return;
            }
            var messageProcessor = _knownMessageProcessors[next.Topic];
            ProcessIncomingMessageCore(next, messageProcessor);
        }

        #endregion

        #region Protected Methods

        protected virtual void ProcessIncomingMessageCore(IIncomingMessage next,
                                                          IProcessAnyMessage messageProcessor)
        {
            messageProcessor.ProcessMessageReceived(next);
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
                    Logger.Info(this, "Disposing Incoming Message Manager.");
                    _incomingMessageQueue.Shutdown();
                    _knownMessageProcessors.Clear();
                }
                _disposed = true;
            }
        }


        #endregion

        

    }
}
