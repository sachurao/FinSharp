using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StreamCipher.Common.Interfaces.ActivityControl;

namespace StreamCipher.Common.Communication
{
    /// <summary>
    /// Facade over all message-based communication actions.
    /// Allows for communication using multiple service buses.  
    /// Set up using CommunicationServiceBuilder and accessed via StreamCipherServiceLocator.
    /// </summary>
    public interface ICommunicationService:IControlledComponent
    {
        void Send(IMessageDestination topicOrQueue, IMessageWrapper message,
            Action<IIncomingMessage> responseHandler = null);
        Task<IIncomingMessage> SendAsync(IMessageDestination topicOrQueue, IMessageWrapper message);
        void Subscribe(IMessageDestination topicOrQueue, Action<IIncomingMessage> incomingMessageHandler);
        void Unsubscribe(IMessageDestination topicOrQueue);
    }
}
