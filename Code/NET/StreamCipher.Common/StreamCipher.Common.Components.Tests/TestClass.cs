using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Components.Communication;
using StreamCipher.Common.Components.Logging;
using StreamCipher.Common.Interfaces.Communication;
using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.Ioc.DependencyResolution;

namespace StreamCipher.Common.Components.Tests
{
    class TestClass
    {
        public TestClass()
        {

            CommunicationMode mode = new CommunicationMode(ServiceBusType.RABBITMQ, DataInterchangeFormat.TEXT_UTF8);
            ICommunicationChannelFactory channelFactory = null; //new RabbitChannelFactory(); 
            ICommunicationServiceConfig config = new DefaultCommunicationServiceConfig("StreamCipherServiceBus");
            ICommunicationService commSvc = new DefaultCommunicationService();
            commSvc.Initialise(mode, channelFactory)
                .WithConfig(config)
                .WithDefaultExceptionHandler((e)=>Logger.Error(this, "Failed", e))
                .Now();
            
            commSvc.Send(mode, new MessageDestination("/something new/"), new MessageWrapper("Cook some food."));
        }
    }
}
