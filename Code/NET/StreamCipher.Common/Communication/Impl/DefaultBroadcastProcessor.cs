using System;
using System.Diagnostics;
using StreamCipher.Common.Logging;

namespace StreamCipher.Common.Communication.Impl
{
    public class DefaultBroadcastProcessor:IProcessBroadcasts
    {
        private Stopwatch _stopwatch = new Stopwatch();
        private readonly object _syncRoot = new object();
        private Action<IIncomingMessage> _messageHandlers;

        #region Implementation of IProcessAnyMessage

        public event Action<IIncomingMessage> OnMessageReceived
        {
            add
            {
                lock (_syncRoot)
                {
                    _messageHandlers += value;
                }
            }
            remove
            {
                lock (_syncRoot)
                {
                    _messageHandlers -= value;
                }
            }
        }

        public void ProcessMessageReceived(IIncomingMessage incomingMessage)
        {
            Logger.Info(this, String.Format("Received command on {0} from sender {1} using correlation id {2}",
                incomingMessage.Topic,
                incomingMessage.Sender,
                incomingMessage.CorrelationId));

            Action<IIncomingMessage> allHandlers = _messageHandlers;
            if (allHandlers != null)
            {
                _stopwatch.Start();
                HandleMessageReceived(incomingMessage, allHandlers);
                _stopwatch.Stop();

                Logger.Info(this,
                            String.Format(
                                "Executed command on {0} from sender {1} using correlation id {2}.  Time since arrival = {3}. Processing time = {4}",
                                incomingMessage.Topic,
                                incomingMessage.Sender,
                                incomingMessage.CorrelationId,
                                (DateTime.UtcNow - incomingMessage.TimeOfArrival).ToString(),    
                                _stopwatch.Elapsed.ToString()));
                _stopwatch.Reset();
            }
        }

        #endregion

        
        protected virtual void HandleMessageReceived(IIncomingMessage incomingMessage, Action<IIncomingMessage> messageHandlers)
        {
            //Delegate[] handlers = messageHandlers.GetInvocationList();
            //int totalHandlers = handlers.Count();
            //Logger.Debug(this, String.Format("Found {0} message handlers subscribing to this topic.", totalHandlers));
            
            messageHandlers(incomingMessage);
            
            //Logger.Debug(this, String.Format("Completed invoking {0} message handlers for this message.", totalHandlers));

            /*for (int i = 0; i < totalHandlers; i++)
            {
                handlers[i].DynamicInvoke(incomingMessage);
                Logger.Debug(this, String.Format("Completed invoking message handler: {0}.  {1} more to go...", i + 1,
                                                 totalHandlers - (i + 1)));
            }*/
            

        }
    }
}
