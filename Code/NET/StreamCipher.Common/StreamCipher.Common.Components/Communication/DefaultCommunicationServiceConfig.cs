using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.Communication;
using System.Diagnostics;

namespace StreamCipher.Common.Components.Communication
{
    class DefaultCommunicationServiceConfig : ICommunicationServiceConfig
    {
        #region Member Variables

        private String _connectionIdPrefix;
        private int _totalSenderChannels;
        private int _totalReceiverChannels;
        private IDictionary<String, String> _customProps = new Dictionary<String, String>();
        
        #endregion

        #region Init
        
        public DefaultCommunicationServiceConfig(int totalSenderChannels, int totalReceiverChannels, String connectionIdPrefix)
        {
            _totalSenderChannels = totalSenderChannels;
            _totalReceiverChannels = totalReceiverChannels;
            _connectionIdPrefix = connectionIdPrefix;
        }

        public DefaultCommunicationServiceConfig(): this(3, 1, System.AppDomain.CurrentDomain.FriendlyName)
        { 
        }
        #endregion

        #region ICommunicationServiceConfig

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
