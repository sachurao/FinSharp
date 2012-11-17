using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Components.Async;
using StreamCipher.Common.Components.Logging;
using StreamCipher.Common.Interfaces.Communication;
using StreamCipher.Common.Interfaces.DataInterchange;

namespace StreamCipher.Common.Components.Communication
{
    /// <summary>
    /// This works closely with implementations of <see cref="IMessageReceiverChannel"/>
    /// and provides all the infrastructure necessary to manage incoming messages.
    /// It has a shared background queue that all receiver channels for a communication mode can
    /// drop items into.
    /// It also tracks message handlers that it invokes for each received message in the background queue.
    /// </summary>
    public class IncomingMessageManager
    {
        #region Member Variables

        private readonly ConcurrentDictionary<IMessageDestination, IMessageProcessor> _subscribedTopics;
        private readonly BackgroundQueue<IIncomingMessage> _incomingMessageQueue;

        #endregion

        #region ctor

        public IncomingMessageManager()
        {
            _incomingMessageQueue = new BackgroundQueue<IIncomingMessage>(ProcessIncomingMessage);
            _subscribedTopics = new ConcurrentDictionary<IMessageDestination, IMessageProcessor>();
            _incomingMessageQueue.Start();
        }

        #endregion
        
        #region Public Properties

        public ConcurrentDictionary<IMessageDestination, IMessageProcessor> SubscribedTopics
        {
            get { return _subscribedTopics; }
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
            if (!_subscribedTopics.ContainsKey(next.Topic))
            {
                Logger.Warn(this,
                            String.Format(
                                "Received message on topic {0} but could not find any registered handlers.  Ignoring this message.",
                                next.Topic));
                return;
            }
            var messageProcessor = _subscribedTopics[next.Topic];
            ProcessIncomingMessageCore(next, messageProcessor);
        }

        #endregion

        #region Protected Methods

        protected virtual void ProcessIncomingMessageCore(IIncomingMessage next,
                                                          IMessageProcessor messageProcessor)
        {
            messageProcessor.RaiseMessageReceived(next);
        }

        #endregion


    }
}
