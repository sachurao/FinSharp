using System;
using System.Collections.Generic;

namespace StreamCipher.Common.Communication
{
    public interface IMessageReceiverChannel:ICommunicationChannel
    {
        void Subscribe(IMessageDestination topicOrQueue, Action<IIncomingMessage> incomingMessageHandler);
        void Unsubscribe(IMessageDestination topicOrQueue);
        void UnsubscribeAll();
        IEnumerable<IMessageDestination> Subscriptions { get; }
    }
}
