using System;
using System.Collections.Concurrent;
using StreamCipher.Common.Logging;
using StreamCipher.Common.Utilities.Concurrency;

namespace StreamCipher.Common.Communication.Impl
{
    /// <summary>
    /// Facade over all communication actions.  Set up using CommunicationServiceBuilder and accessed via StreamCipherServiceLocator.
    /// </summary>
    public class DefaultCommunicationService:ICommunicationService
    {
        #region Member Variables

        private readonly ConcurrentDictionary<CommunicationMode, ICommunicationChannelPool> _channelPools;
        private bool _disposed = false;
        
        #endregion

        #region Init
        
        public DefaultCommunicationService()
        {
            _channelPools = new ConcurrentDictionary<CommunicationMode, ICommunicationChannelPool>();
        }
        
        #endregion

        #region ICommunicationService

        public ICommunicationServiceBuilder Build(CommunicationMode communicationMode, ICommunicationChannelFactory channelFactory)
        {
            return new DefaultCommunicationServiceBuilder(this, communicationMode, channelFactory);
        }

        public void Send(CommunicationMode communicationMode, IMessageDestination destination, IMessageWrapper message,
            Action<IIncomingMessage> responseHandler = null)
        {
            VerifyCommunicationModeExists(communicationMode);

            IMessageSenderChannel senderChannel = null;
            try
            {
                ICommunicationChannelPool channelPool = _channelPools[communicationMode];
                senderChannel = channelPool.GetSenderChannel();
                if (responseHandler != null)
                {
                    channelPool.ResponseProcessor.AddSpecificResponseHandler(message.CorrelationId, responseHandler);
                    senderChannel.Send(destination, message, channelPool.UniqueResponseDestination);
                }
                else
                {
                    senderChannel.Send(destination, message);
                }
            }
            finally
            {
                if (senderChannel != null) _channelPools[communicationMode].PutSenderChannel(senderChannel);
            }
        }

        
        public void Subscribe(CommunicationMode communicationMode, IMessageDestination topic, 
            Action<IIncomingMessage> incomingMessageHandler)
        {
            VerifyCommunicationModeExists(communicationMode);
            _channelPools[communicationMode].Subscribe(topic, incomingMessageHandler);
        }

        public void Unsubscribe(CommunicationMode communicationMode, IMessageDestination topic)
        {
            VerifyCommunicationModeExists(communicationMode);
            _channelPools[communicationMode].Unsubscribe(topic);
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
                    Logger.Info(this, "Disposing communication service.");
                    foreach (var pool in _channelPools.Values)
                    {
                        pool.Dispose();
                    }
                }
                _disposed = true;
            }
        }


        #endregion

        #region Private Methods

        private void VerifyCommunicationModeExists(CommunicationMode communicationMode)
        {
            if (!_channelPools.ContainsKey(communicationMode))
                throw new InvalidOperationException("Communication Service must be initialised first for this communication mode.");
        }

        #endregion

        class DefaultCommunicationServiceBuilder:ICommunicationServiceBuilder
        {
            private DefaultCommunicationService _svc;
            private CommunicationMode _mode;
            private ICommunicationChannelFactory _channelFactory;
            private ICommunicationServiceConfig _config;
            private Action<Exception> _defaultExceptionHandler;
            private AtomicBoolean _isInitialised = new AtomicBoolean(false);

            public DefaultCommunicationServiceBuilder(DefaultCommunicationService svc, CommunicationMode communicationMode,
                ICommunicationChannelFactory channelFactory)
            {
                _svc = svc;
                _mode = communicationMode;
                _channelFactory = channelFactory;
            }

            #region Implementation of ICommunicationServiceBuilder

            public ICommunicationServiceBuilder WithConfig(ICommunicationServiceConfig config)
            {
                if (_isInitialised.Value) throw new InvalidOperationException("This communication service builder has already been used once.");
                _config = config;
                return this;
            }

            public ICommunicationServiceBuilder WithDefaultExceptionHandler(Action<Exception> handleException)
            {
                if (_isInitialised.Value) throw new InvalidOperationException("This communication service builder has already been used once.");
                _defaultExceptionHandler = handleException;
                return this;
            }

            public void Now()
            {
                if (_isInitialised.CompareAndSet(false, true))
                {
                    //Initialising the channel pool for defined communication mode
                    var channelPool = new CommunicationChannelPool();
                    if (!_svc._channelPools.TryAdd(_mode, channelPool))
                        throw new InvalidOperationException("Communication mode has already been initialised.");
                    //Adding sender channels
                    for (int i = 0; i < _config.TotalSenderChannels; i++)
                    {
                        var senderChannel = _channelFactory.CreateMessageSenderChannel(_mode, _config,
                                                                                       _defaultExceptionHandler, i);
                        channelPool.AddSenderChannel(senderChannel);
                    }
                    //Adding receiver channels
                    for (int i = 0; i < _config.TotalReceiverChannels; i++)
                    {
                        var receiverChannel = _channelFactory.CreateMessageReceiverChannel(_mode, _config,
                                                                                           _defaultExceptionHandler, i);
                        channelPool.AddReceiverChannel(receiverChannel);
                    }

                    //Adding response topic
                    var responseDestination = new MessageDestination(_config.UniqueResponseTopic);
                    channelPool.Subscribe(responseDestination,
                            channelPool.ResponseProcessor.ProcessMessageReceived);
                    channelPool.UniqueResponseDestination = responseDestination;
                }
            }
            
            #endregion
        }


    }
}
