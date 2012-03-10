using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.Shared.Interfaces.Communication;
using Irrefutable.Shared.Interfaces.DataInterchange;
using Irrefutable.Shared.Ioc.DependencyResolution;

namespace Irrefutable.Shared.Components.Communication
{
    public class CommunicationServiceBuilder : ICommunicationServiceBuilder
    {
        #region Public Properties
        
        public CommunicationMode CommunicationMode { get; private set; }
        public ICommunicationServiceConfig Config { get; private set; }
        public IDataStructureFormatter Formatter { get; private set; }
        public Action<Exception> DefaultExceptionHandler { get; private set; }

        #endregion
        
        #region Init

        private CommunicationServiceBuilder(CommunicationMode communicationType)
        {
            CommunicationMode = communicationType;
            Config = new DefaultCommunicationServiceConfig();
        }

        public static ICommunicationServiceBuilder Initialise(CommunicationMode communicationType)
        {
            return new CommunicationServiceBuilder(communicationType);
        }

        #endregion
        
        #region CommunicationServiceBuilder

        public ICommunicationServiceBuilder WithConfig(ICommunicationServiceConfig config)
        {
            Config = config;
            return this;
        }

        public ICommunicationServiceBuilder WithDefaultExceptionHandler(Action<Exception> handleException)
        {
            DefaultExceptionHandler = handleException;
            return this;
        }

        public void Build()
        {
            var communicationSvc = ServiceLocator.GetImplementationOf<ICommunicationService>();
            for (int i = 0; i < Config.TotalSenderChannels; i++)
            {
                communicationSvc.AddSenderChannel(CommunicationMode, 
                    Factory.CreateCommunicationChannel<IMessageSenderChannel>(
                        CommunicationMode, Config, DefaultExceptionHandler, i));
            }

            for (int i = 0; i < Config.TotalReceiverChannels; i++)
            {
                communicationSvc.AddReceiverChannel(CommunicationMode, 
                    Factory.CreateCommunicationChannel<IMessageReceiverChannel>(
                        CommunicationMode, Config, DefaultExceptionHandler, i));
            }
        }

        #endregion
        
    }
}
