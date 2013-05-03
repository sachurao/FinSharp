using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamCipher.Common.Pooling;

namespace StreamCipher.Common.Communication
{
    interface IMessageReceiverChannelPool : 
        IObjectPool<IMessageReceiverChannel>
    {
        IMessageDestination UniqueResponseDestination { get; }
        IProcessResponses ResponseProcessor { get; }
        IProcessBroadcasts BroadcastProcessor { get; }


    }
}
