using System;

namespace StreamCipher.Common.Communication
{
    public interface IIncomingMessage:IMessageWrapper
    {
        string Sender { get; }
        IMessageDestination Topic { get; }
        IMessageDestination ReplyTo { get; }
        DateTime TimeOfArrival { get; }

    }
}
