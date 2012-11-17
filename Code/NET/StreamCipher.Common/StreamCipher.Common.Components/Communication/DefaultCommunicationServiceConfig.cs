using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.Communication;
using System.Diagnostics;

namespace StreamCipher.Common.Components.Communication
{
    public class DefaultCommunicationServiceConfig : ICommunicationServiceConfig
    {
        #region Member Variables

        private String _connectionIdPrefix;
        private String _serviceBusAddress;
        private int _totalSenderChannels;
        private int _totalReceiverChannels;
        private IDictionary<String, String> _customProps = new Dictionary<String, String>();
        
        #endregion

        #region Init
        
        public DefaultCommunicationServiceConfig(String serviceBusAddress, 
            int totalSenderChannels, int totalReceiverChannels, String connectionIdPrefix)
        {
            _serviceBusAddress = serviceBusAddress;
            _totalSenderChannels = totalSenderChannels;
            _totalReceiverChannels = totalReceiverChannels;
            _connectionIdPrefix = connectionIdPrefix;
        }

        public DefaultCommunicationServiceConfig(String serviceBusAddress): this(serviceBusAddress, 
            3, 1, "StreamCipher")
        { 
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


        
        #endregion

        
    }
}
