using System.Threading;
using StreamCipher.Common.Communication.Impl;
using StreamCipher.Common.Pooling;

namespace StreamCipher.Common.Communication.ThirdParty.RabbitMQ
{
    public class PoolableRabbitSenderChannelFactory : AbstractPoolableChannelFactory<IMessageSenderChannel>
    {
        public PoolableRabbitSenderChannelFactory(ICommunicationServiceConfig config) : base(config)
        {
        }

        protected override IMessageSenderChannel CreateCore()
        {
            var retVal = new PoolableRabbitSenderChannel(_config, Interlocked.Increment(ref _instanceNum));
            return retVal;
        }
    }
}
