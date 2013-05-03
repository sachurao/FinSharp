using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamCipher.Common.Pooling;

namespace StreamCipher.Common.Communication.Impl
{
    public abstract class AbstractPoolableChannelFactory<T> : IPoolableObjectFactory<T> where T:ICommunicationChannel
    {
         #region Member Variables

        protected ICommunicationServiceConfig _config;
        protected int _instanceNum = 1;

        #endregion

        #region Init

        protected AbstractPoolableChannelFactory(ICommunicationServiceConfig config)
        {
            _config = config;
        }

        #endregion
        
        #region IPoolableObjectFactory

        public T Create()
        {
            var retVal = CreateCore();
            retVal.Connect();
            return retVal;
        }

        public void Retire(T item)
        {
            if (item.IsConnected)
                item.Disconnect();
        }

        #endregion

        protected abstract T CreateCore();

    }
}
