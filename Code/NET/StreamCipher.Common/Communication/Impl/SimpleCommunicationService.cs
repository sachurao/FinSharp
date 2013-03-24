using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamCipher.Common.Async;

namespace StreamCipher.Common.Communication.Impl
{
    public class SimpleCommunicationService: ControlledComponent, ICommunicationService2
    {


        #region Init

        public SimpleCommunicationService(SimpleCommunicationServiceConfig config,
                                          string name = "SimpleCommunicationService",
                                          Int32 instanceNum = 1) : base(name, instanceNum)
        {

        }

        #endregion


        

        #region ICommunicationService2

        

        public void Send(IMessageDestination topicOrQueue, IMessageWrapper message,
                         Action<IIncomingMessage> responseHandler = null)
        {
            throw new NotImplementedException();
        }

        public Task<IIncomingMessage> SendAsync(IMessageDestination topicOrQueue, IMessageWrapper message)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(IMessageDestination topicOrQueue, Action<IIncomingMessage> incomingMessageHandler)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(IMessageDestination topicOrQueue)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
