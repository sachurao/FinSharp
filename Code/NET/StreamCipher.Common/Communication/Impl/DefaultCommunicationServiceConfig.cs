using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StreamCipher.Common.Communication.Impl
{
    public class DefaultCommunicationServiceConfig : ICommunicationServiceConfig
    {
        #region Member Variables

        private String _connectionIdPrefix;
        private String _serviceBusAddress;
        private String _uniqueResponseTopic;
        private int _totalSenderChannels;
        private int _totalReceiverChannels;
        private IDictionary<String, String> _customProps = new Dictionary<String, String>();
        
        #endregion

        #region Init
        
        public DefaultCommunicationServiceConfig(string serviceBusAddress = "localhost", 
            string connectionIdPrefix = "StreamCipher",
            int totalSenderChannels = 3, 
            int totalReceiverChannels = 1,
            string uniqueResponseTopic = null)
        {
            _serviceBusAddress = serviceBusAddress;
            _totalSenderChannels = totalSenderChannels;
            _totalReceiverChannels = totalReceiverChannels;
            _connectionIdPrefix = connectionIdPrefix;
            _uniqueResponseTopic = !String.IsNullOrEmpty(uniqueResponseTopic)
                                       ? uniqueResponseTopic
                                       : String.Format("streamcipher.response.{0}_{1}_{2}",
                                                       connectionIdPrefix,
                                                       Environment.MachineName,
                                                       Process.GetCurrentProcess().Id);
        }

        #endregion

        #region ICommunicationServiceConfig

        public string ServiceBusAddress
        {
            get { return _serviceBusAddress; }
        }

        public string ConnectionIdPrefix
        {
            get { return _connectionIdPrefix; }
        }

        public int TotalSenderChannels
        {
            get { return _totalSenderChannels; }
        }

        public int TotalReceiverChannels
        {
            get { return _totalReceiverChannels; }
        }

        public IDictionary<String, String> CustomProps
        {
            get { return _customProps; }
        }

        public string UniqueResponseTopic
        {
            get { return _uniqueResponseTopic; }
        }

        #endregion

        
    }
}
