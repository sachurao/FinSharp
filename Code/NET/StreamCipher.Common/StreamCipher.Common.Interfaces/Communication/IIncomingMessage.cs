using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.Common.Interfaces.Communication
{
    public interface IIncomingMessage:IMessageWrapper
    {
        IMessageDestination Sender { get; }
        IMessageDestination Topic { get; }
    }
}
