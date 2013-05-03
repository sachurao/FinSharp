using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamCipher.Common.Pooling;

namespace StreamCipher.Common.Communication
{
    interface IMessageSenderChannelPool : IObjectPool<IMessageSenderChannel>
    {
    }
}
