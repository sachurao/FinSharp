using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using StreamCipher.Common.Components.Communication;
using StreamCipher.Common.Components.Logging;
using StreamCipher.Common.Components.Tests;
using StreamCipher.Common.Interfaces.Communication;
using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.Interfaces.Logging;
using StreamCipher.Common.Ioc.DependencyResolution;
using StreamCipher.Common.ThirdParty.Communication.RabbitMQ;
using TechTalk.SpecFlow;

namespace StreamCipher.Common.IntegrationTests.Communication
{
    [Binding]
    public class RabbitMQSteps
    {
        private ICommunicationService _commSvc;
        private CommunicationMode _communicationMode;
        private CommandSynchronizer _synchronizer;

        public RabbitMQSteps()
        {
            //Initialise the app

            ConsoleLoggingService loggingService = new ConsoleLoggingService();
            loggingService.LoggingLevel = LoggingLevel.DEBUG;
            ProgrammaticDependencyManager dependencyManager = new ProgrammaticDependencyManager();
            dependencyManager.Register<ILoggingService, ConsoleLoggingService>(loggingService);
            ServiceLocator.Initialise(dependencyManager);

            _communicationMode = new CommunicationMode(ServiceBusType.RABBITMQ, DataInterchangeFormat.TEXT_UTF8);
            var config = new DefaultCommunicationServiceConfig("localhost");
            config.CustomProps.Add("VirtualHost", "TestEnv");
            config.CustomProps.Add("UserName", "finsharp");
            config.CustomProps.Add("Password", "streamcipher");

            _commSvc = new DefaultCommunicationService();
            _commSvc.Build(_communicationMode, new RabbitChannelFactory())
                .WithConfig(config)
                .WithDefaultExceptionHandler((ex) => Logger.Error(_commSvc, "Messaging error", ex))
                .Now();

            _synchronizer = new CommandSynchronizer();

        }

        [Given(@"I have connected as a sender to the message broker")]
        public void GivenIHaveConnectedAsASenderToTheMessageBroker()
        {
            //ScenarioContext.Current.Pending();
        }
        
        [Given(@"I have connected as a receiver to the message broker")]
        public void GivenIHaveConnectedAsAReceiverToTheMessageBroker()
        {
            //ScenarioContext.Current.Pending();
            _commSvc.Subscribe(_communicationMode, new MessageDestination(CommandSynchronizer.TEST_MESSAGE_TOPIC),
                _synchronizer.HandleIncomingMessage);
        }
        
        [When(@"I send a message")]
        public void WhenISendAMessage()
        {
            //ScenarioContext.Current.Pending();
            _synchronizer.Send(_commSvc, _communicationMode);
        }
        
        [Then(@"I should receive the message")]
        public void ThenIShouldReceiveTheMessage()
        {
            bool received = _synchronizer.HasMessageBeenReceived;
            Assert.IsTrue(_synchronizer.HasMessageBeenReceived);

            Assert.AreEqual(CommandSynchronizer.TEST_MESSAGE_CONTENT, _synchronizer.ReceivedMessage);
            
        }
    }

    class CommandSynchronizer :DefaultMessageProcessor
    {
        public const string TEST_MESSAGE_TOPIC = "test.message.topic";
        public const string TEST_MESSAGE_CONTENT = "random message";

        private readonly object _syncRoot = new object();
        public bool HasMessageBeenReceived { get; private set; }
        public String ReceivedMessageTopic { get; private set; }
        public String ReceivedMessage { get; private set; }

        public void Send (ICommunicationService commSvc, CommunicationMode communicationMode)
        {
            lock (_syncRoot)
            {
                commSvc.Send(communicationMode, new MessageDestination(TEST_MESSAGE_TOPIC), new MessageWrapper(TEST_MESSAGE_CONTENT));
                Monitor.Wait(_syncRoot, 1000);
            }
        }

        public void HandleIncomingMessage(IIncomingMessage incomingMessage)
        {
            Logger.Debug(this, "Handling incoming message...");
            lock (_syncRoot)
            {
                HasMessageBeenReceived = true;
                ReceivedMessageTopic = incomingMessage.Topic.ToString();
                ReceivedMessage = incomingMessage.Content.ToString();
                Logger.Info(this, String.Format("Received message  on topic {0} from {1}", 
                    incomingMessage.Topic.Address, incomingMessage.Sender.Address));
                Monitor.Pulse(_syncRoot);
            }
        }
    }
}
