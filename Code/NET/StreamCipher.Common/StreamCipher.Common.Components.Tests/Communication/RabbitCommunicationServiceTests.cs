using StreamCipher.Common.Components.Communication;
using StreamCipher.Common.Components.Logging;
using StreamCipher.Common.Interfaces.Communication;
using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.ThirdParty.Communication.RabbitMQ;

namespace StreamCipher.Common.Components.Tests.Communication
{
    class RabbitCommunicationServiceTests
    {
        private ICommunicationService commSvc;
        private CommunicationMode mode;
        public RabbitCommunicationServiceTests()
        {

            mode = new CommunicationMode(ServiceBusType.RABBITMQ, DataInterchangeFormat.TEXT_UTF8);
            ICommunicationChannelFactory channelFactory =  new RabbitChannelFactory(); 
            ICommunicationServiceConfig config = new DefaultCommunicationServiceConfig("StreamCipherServiceBus");
            commSvc = new DefaultCommunicationService();
            commSvc.Initialise(mode, channelFactory)
                .WithConfig(config)
                .WithDefaultExceptionHandler((e)=>Logger.Error(this, "Failed", e))
                .Now();
            
        }

        public void TestSend()
        {
            commSvc.Send(mode, new MessageDestination("/something new/"), new MessageWrapper("Cook some food."));
        }
    }
}
