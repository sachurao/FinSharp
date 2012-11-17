using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.Common.Interfaces.Communication
{
    public interface IMessageReceiverChannel:ICommunicationChannel
    {
        void Subscribe(IMessageDestination topicOrQueue, Action<IIncomingMessage> incomingMessageHandler);
        void Unsubscribe(IMessageDestination topicOrQueue);
        void UnsubscribeAll();
        IEnumerable<IMessageDestination> Subscriptions { get; }
    }
}
