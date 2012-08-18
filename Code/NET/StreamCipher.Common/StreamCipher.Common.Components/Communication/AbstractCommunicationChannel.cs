using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.Communication;
using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.Utilities.Concurrency;
using StreamCipher.Common.Components.Logging;

namespace StreamCipher.Common.Components.Communication
{
    public abstract class AbstractCommunicationChannel:ICommunicationChannel
    {
        #region Member Variables

        private readonly ServiceBusType _messagingPlatform;
        protected IDataInterchangeFormatter _formatter;
        protected ICommunicationServiceConfig _config;
        protected string _connectionId;
        protected Action<Exception> _defaultExceptionHandler;
        protected AtomicBoolean _isConnected = new AtomicBoolean(false);

        #endregion

        #region Init
        
        protected AbstractCommunicationChannel(ServiceBusType messagingPlatform,
            IDataInterchangeFormatter formatter,
            ICommunicationServiceConfig config,
            String connectionIdSuffix,
            Action<Exception> defaultExceptionHandler)
        {
            _messagingPlatform = messagingPlatform;
            _formatter = formatter;
            _config = config;
            _defaultExceptionHandler = defaultExceptionHandler;
            _connectionId = String.Format("{0}_{1}_{2}_{3}_{4}",
                config.ConnectionIdPrefix,
                System.Environment.UserName,
                System.Environment.MachineName,
                System.Diagnostics.Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture),
                connectionIdSuffix);
        } 

        #endregion
        
        #region ICommunicationChannel

        public ServiceBusType MessagingPlatform
        {
            get { return _messagingPlatform; }
        }

        public IDataInterchangeFormatter Formatter
        {
            get { return _formatter; }
        }

        #endregion
        
        #region IRemoteConnection

        public string ConnectionId
        {
            get { return _connectionId; }
        }

        public void Connect()
        {
            if (_isConnected.CompareAndSet(false, true))
            {
                ConnectCore();
            }
        }

        public void Disconnect()
        {
            if (_isConnected.CompareAndSet(false, true))
            {
                DisconnectCore();
            }
        }

        public bool IsConnected
        {
            get { return _isConnected.Value; }
        }

        #endregion
        
        #region Abstract Methods
        
        protected abstract void ConnectCore();
        
        protected abstract void DisconnectCore();

        #endregion
    }
}
