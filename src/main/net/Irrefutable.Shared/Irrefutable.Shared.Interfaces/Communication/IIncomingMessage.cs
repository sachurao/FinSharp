using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irrefutable.Shared.Interfaces.Communication
{
    public interface IIncomingMessage:IMessageWrapper
    {
        IMessageDestination Sender { get; }
        IMessageDestination Topic { get; }
    }
}
