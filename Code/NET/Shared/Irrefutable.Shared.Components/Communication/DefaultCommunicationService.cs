using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.Shared.Interfaces.Communication;
using Irrefutable.Shared.Interfaces.ActivityControl;
using System.Threading;
using Irrefutable.Shared.Utilities.Concurrency;
using System.Collections.Concurrent;

namespace Irrefutable.Shared.Components.Communication
{
    /// <summary>
    /// Facade over all communication actions.  Set up using CommunicationServiceBuilder and accessed via ServiceLocator.
    /// </summary>
    public class DefaultCommunicationService:ICommunicationService
    {
        #region Member Variables

        private ConcurrentDictionary<CommunicationMode, ICommunicationChannelPool> _channelPools;
        private bool _disposed = false;
        
        #endregion

        #region Init
        
        public DefaultCommunicationService()
        {
            _channelPools = new ConcurrentDictionary<CommunicationMode, ICommunicationChannelPool>();
        } 
        
        #endregion

        #region ICommunicationService

        public void AddSenderChannel(CommunicationMode communicationType, IMessageSenderChannel senderChannel)
        {
           var channelPool =  _channelPools.AddOrUpdate(communicationType,
                new CommunicationChannelPool(),
                (ct, ccp) => { return _channelPools[ct]; });
           channelPool.AddSenderChannel(senderChannel);
                
        }

        public void AddReceiverChannel(CommunicationMode communicationType, IMessageReceiverChannel receiverChannel)
        {
            var channelPool = _channelPools.AddOrUpdate(communicationType,
                new CommunicationChannelPool(),
                (ct, ccp) => { return _channelPools[ct]; });
            channelPool.AddReceiverChannel(receiverChannel);
        }

        public void Send(CommunicationMode communicationType, IMessageDestination destination, IMessageWrapper message)
        {
            IMessageSenderChannel senderChannel = null;
            try
            {
                senderChannel = _channelPools[communicationType].GetSenderChannel();
                senderChannel.Send(destination, message);
            }
            finally
            {
                if (senderChannel != null) _channelPools[communicationType].PutSenderChannel(senderChannel);
            }
        }

        public IMessageWrapper SendRpc(CommunicationMode communicationType, IMessageDestination destination, IMessageWrapper message)
        {
            IMessageSenderChannel senderChannel = null;
            try
            {
                senderChannel = _channelPools[communicationType].GetSenderChannel();
                return senderChannel.SendRpc(destination, message);
            }
            finally
            {
                if (senderChannel != null) _channelPools[communicationType].PutSenderChannel(senderChannel);
            }
        }

        public void Subscribe(CommunicationMode communicationType, IMessageDestination topic, IMessageHandler messageHandler)
        {
            _channelPools[communicationType].Subscribe(topic, messageHandler);
        }

        public void Unsubscribe(CommunicationMode communicationType, IMessageDestination topic)
        {
            _channelPools[communicationType].Unsubscribe(topic);
        }        

        #endregion

        #region IDisposable


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var pool in _channelPools.Values)
                    {
                        pool.Dispose();
                    }
                }
                _disposed = true;
            }
        }


        #endregion

    }
}
