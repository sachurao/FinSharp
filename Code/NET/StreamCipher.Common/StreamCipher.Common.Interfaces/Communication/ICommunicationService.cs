using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.ActivityControl;

namespace StreamCipher.Common.Interfaces.Communication
{
    /// <summary>
    /// Facade over all message-based communication actions.
    /// Allows for communication using multiple service buses.  
    /// Set up using CommunicationServiceBuilder and accessed via ServiceLocator.
    /// </summary>
    public interface ICommunicationService: IDisposable
    {
        ICommunicationServiceBuilder Build(CommunicationMode communicationMode, ICommunicationChannelFactory channelFactory);

        void Send(CommunicationMode communicationMode, IMessageDestination destination, IMessageWrapper message);
        IMessageWrapper SendRpc(CommunicationMode communicationMode, IMessageDestination destination, IMessageWrapper message);

        void Subscribe(CommunicationMode communicationMode, IMessageDestination topic, 
            Action<IIncomingMessage> incomingMessageHandler);
        void Unsubscribe(CommunicationMode communicationMode, IMessageDestination topic);        
    }
}
