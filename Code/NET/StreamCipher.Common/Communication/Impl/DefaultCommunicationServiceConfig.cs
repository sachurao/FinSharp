using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamCipher.Common.Interfaces.DataInterchange;

namespace StreamCipher.Common.Communication.Impl
{
    public class DefaultCommunicationServiceConfig : ICommunicationServiceConfig
    {
        #region Init

        private DefaultCommunicationServiceConfig()
        {
            
        }

        public class Builder
        {
            public string SetServiceBusAddress { private get; set; }
            public string SetConnectionIdPrefix { private get; set; }
            public string SetUniqueResponseTopic { private get; set; }
            public int SetTotalSenderChannels { private get; set; }
            public int SetTotalReceiverChannels { private get; set; }
            private IDictionary<String, String> CustomProps { get; set; } 
            public IDataInterchangeFormatter SetFormatter { private get; set; }
            public Action<Exception> SetDefaultExceptionHandler { private get; set; }

            public Builder()
            {
                SetConnectionIdPrefix = "StreamCipher";
                SetTotalSenderChannels = 3;
                SetTotalReceiverChannels = 1;
                CustomProps = new Dictionary<string, string>();

            }

            public IDictionary<String, String> ImportCustomProps
            {
                set
                {
                    CustomProps = new Dictionary<string, string>(value);
                }
            }

            public DefaultCommunicationServiceConfig Build()
            {
                var retVal =  new DefaultCommunicationServiceConfig()
                    {
                        ServiceBusAddress = SetServiceBusAddress,
                        ConnectionIdPrefix = SetConnectionIdPrefix,
                        UniqueResponseTopic = SetUniqueResponseTopic,
                        TotalSenderChannels = SetTotalSenderChannels,
                        TotalReceiverChannels = SetTotalReceiverChannels,
                        CustomProps = CustomProps,
                        Formatter = SetFormatter,
                        DefaultExceptionHandler = SetDefaultExceptionHandler
                    };
                return retVal;
            }
            
        }

        #endregion

        #region ICommunicationServiceConfig

        public string ServiceBusAddress { get; private set; }

        public string ConnectionIdPrefix { get; private set; }

        public string UniqueResponseTopic { get; private set; }

        public int TotalSenderChannels { get; private set; }

        public int TotalReceiverChannels { get; private set; }

        public IDictionary<string, string> CustomProps { get; private set; }

        public IDataInterchangeFormatter Formatter { get; private set; }
        
        public Action<Exception> DefaultExceptionHandler { get; private set; }

        #endregion

    }
}
