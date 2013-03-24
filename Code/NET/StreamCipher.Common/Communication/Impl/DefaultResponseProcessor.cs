using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using StreamCipher.Common.Logging;

namespace StreamCipher.Common.Communication.Impl
{
    class DefaultResponseProcessor : IProcessResponses
    {
        private ConcurrentDictionary<string, Action<IIncomingMessage>> _responseHandlerDict =
            new ConcurrentDictionary<string, Action<IIncomingMessage>>();
        private Stopwatch _stopwatch = new Stopwatch();
        

        public void ProcessMessageReceived(IIncomingMessage incomingMessage)
        {
            //This will be done on the worker thread of the background queue.
            Logger.Info(this, String.Format("Received response on {0} from sender {1} with correlation id {2}",
                incomingMessage.Topic,
                incomingMessage.Sender,
                incomingMessage.CorrelationId)); 
            Action<IIncomingMessage> responseHandler;
            _responseHandlerDict.TryGetValue(incomingMessage.CorrelationId, out responseHandler);
            if (responseHandler != null)
            {
                _stopwatch.Start();
                responseHandler(incomingMessage);
                _stopwatch.Stop();
                Logger.Info(this, String.Format("Processed response on {0} from sender {1} with correlation id {2}.  Time since arrival = {3}. Processing time = {4}",
                    incomingMessage.Topic,
                    incomingMessage.Sender,
                    incomingMessage.CorrelationId,
                    (DateTime.UtcNow - incomingMessage.TimeOfArrival).ToString(),
                    _stopwatch.Elapsed.ToString()));
                _stopwatch.Reset();
                
            }
            else
            {
                //This should not happen.  You should not receive a message on the unique response topic
                //if it is not an expected response.  DO SOMETHING!!
                Logger.Warn(this, String.Format("Was not expecting a response for correlation id = {0}",
                    incomingMessage.CorrelationId));
            }
        }

        public void AddSpecificResponseHandler(string correlationId, Action<IIncomingMessage> handler)
        {
            Logger.Debug(this, String.Format("Adding response handler for correlation id = {0}",
                correlationId));
            _responseHandlerDict.AddOrUpdate(correlationId, handler, (c,h) => handler);
            Logger.Debug(this, String.Format("Added response handler for correlation id = {0}",
                correlationId));
            
        }
    }
}