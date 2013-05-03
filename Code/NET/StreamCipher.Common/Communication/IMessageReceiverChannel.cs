using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamCipher.Common.Communication
{
    public interface IMessageReceiverChannel : ICommunicationChannel
    {
        void Subscribe(IMessageDestination topicOrQueue, Action<IIncomingMessage> incomingMessageHandler);
        void Unsubscribe(IMessageDestination topicOrQueue);
        void UnsubscribeAll();
        IEnumerable<IMessageDestination> Subscriptions { get; }
    }
}
