using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StreamCipher.Common.Communication.Impl;
using StreamCipher.Common.Pooling;

namespace StreamCipher.Common.Communication.ThirdParty.RabbitMQ
{
    public class PoolableRabbitReceiverChannelFactory : AbstractPoolableChannelFactory<IMessageReceiverChannel>
    {
        public PoolableRabbitReceiverChannelFactory(ICommunicationServiceConfig config) : base(config)
        {
        }

        protected override IMessageReceiverChannel CreateCore()
        {
            return new PoolableRabbitReceiverChannel(_config, Interlocked.Increment(ref _instanceNum));
        }
    }
}
