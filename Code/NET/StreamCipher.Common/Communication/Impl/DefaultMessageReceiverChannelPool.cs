using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamCipher.Common.Pooling;

namespace StreamCipher.Common.Communication.Impl
{
    class DefaultMessageReceiverChannelPool : DefaultObjectPool<IMessageReceiverChannel>,
        IMessageReceiverChannelPool
    {
        private IMessageDestination _uniqueResponseDestination;
        private IProcessResponses _responseProcessor;
        private IProcessBroadcasts _broadcastProcessor;

        public DefaultMessageReceiverChannelPool(
            ObjectPoolConfig<IMessageReceiverChannel> config,
            String uniqueResponseTopic) : base(config)
        {
            _uniqueResponseDestination = new MessageDestination(uniqueResponseTopic);
            _responseProcessor = new DefaultResponseProcessor();
            //_broadcastProcessor = new DefaultBroadcastProcessor();
        }

        public IMessageDestination UniqueResponseDestination
        {
            get { return _uniqueResponseDestination; }
        }

        public IProcessResponses ResponseProcessor
        {
            get { return _responseProcessor; }
        }

        public IProcessBroadcasts BroadcastProcessor
        {
            get { return _broadcastProcessor; }
        }
    }
}
