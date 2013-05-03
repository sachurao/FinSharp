using System;
using System.Globalization;
using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.Utilities.Concurrency;

namespace StreamCipher.Common.Communication.Impl
{
    public abstract class AbstractCommunicationChannel:ICommunicationChannel
    {
        #region Member Variables

        protected ICommunicationServiceConfig _config;
        protected readonly string _connectionId;
        protected AtomicBoolean _hasConnectedOnce = new AtomicBoolean(false);

        #endregion

        #region Init
        
        protected AbstractCommunicationChannel(ICommunicationServiceConfig config, String connectionIdSuffix)
        {
            _config = config;
            _connectionId = String.Format("{0}/{1}_{2}_{3}_{4}",
                config.ConnectionIdPrefix,
                System.Environment.UserName,
                System.Environment.MachineName,
                System.Diagnostics.Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture),
                connectionIdSuffix);
        }

        #endregion
        
        #region ICommunicationChannel

        public IDataInterchangeFormatter Formatter
        {
            get { return _config.Formatter; }
        }

        public bool IsValidToUse
        {
            get { return this.IsConnected; }
        }

        #endregion
        
        #region IRemoteConnection

        public string ConnectionId
        {
            get { return _connectionId; }
        }

        public void Connect()
        {
            if (_hasConnectedOnce.CompareAndSet(false, true))
            {
                ConnectCore();
            }
        }

        public void Disconnect()
        {
            if (_hasConnectedOnce.CompareAndSet(true, false))
            {
                DisconnectCore();
            }
        }

        public bool IsConnected
        {
            get { return _hasConnectedOnce.Value && IsConnectedCore(); }
        }

        #endregion

        public override bool Equals(object obj)
        {
            var other = obj as AbstractCommunicationChannel;
            if (other == null) return false;
            return _connectionId == other.ConnectionId;
        }

        public override int GetHashCode()
        {
            return _connectionId.GetHashCode();
        }

        #region Abstract Methods
        
        protected abstract void ConnectCore();
        
        protected abstract void DisconnectCore();

        protected abstract bool IsConnectedCore();

        #endregion
    }
}
