using System;
using System.Collections.Generic;
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
        private readonly DataInterchangeFormat _format;
        protected IDataStructureFormatter _formatter;
        protected ICommunicationServiceConfig _config;
        protected string _connectionId;
        protected Action<Exception> _defaultExceptionHandler;
        protected AtomicBoolean _isConnected = new AtomicBoolean(false);

        #endregion

        #region Init
        
        protected AbstractCommunicationChannel(ServiceBusType messagingPlatform,
            DataInterchangeFormat format, 
            ICommunicationServiceConfig config,
            String connectionIdSuffix,
            Action<Exception> defaultExceptionHandler)
        {
            _messagingPlatform = messagingPlatform;
            _format = format;
            _config = config;
            _defaultExceptionHandler = defaultExceptionHandler;
            _connectionId = String.Format("{0}_{1}_{2}_{3}_{4}",
                config.ConnectionIdPrefix,
                System.Environment.UserName,
                System.Environment.MachineName,
                System.Diagnostics.Process.GetCurrentProcess().Id.ToString(),
                connectionIdSuffix);
        } 

        #endregion
        
        #region ICommunicationChannel

        public DataInterchangeFormat Format 
        { 
            get { return _format; } 
        }

        public ServiceBusType MessagingPlatform
        {
            get { return _messagingPlatform; }
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
