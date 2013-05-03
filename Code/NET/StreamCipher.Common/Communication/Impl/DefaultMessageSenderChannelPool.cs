using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamCipher.Common.Pooling;

namespace StreamCipher.Common.Communication.Impl
{
    class DefaultMessageSenderChannelPool : DefaultObjectPool<IMessageSenderChannel>,
        IMessageSenderChannelPool
    {
        public DefaultMessageSenderChannelPool(
            ObjectPoolConfig<IMessageSenderChannel> config) 
            : base(config)
        {
        }
    }
}
