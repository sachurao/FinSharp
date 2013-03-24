using System;
using NUnit.Framework;
using StreamCipher.Common.Ioc.Impl;
using StreamCipher.Common.Logging;
using TechTalk.SpecFlow;

namespace StreamCipher.Common.IntegrationTests.Communication
{
    [Binding]
    public class CommunicationViaRabbitMQSteps
    {
        private CommunicatingPeer _peerOne;
        private CommunicatingPeer _peerTwo;
        private CommunicationSynchronizer _synchronizer;
        private Random randomGenerator = new Random();
        private readonly string _testTopic = "streamcipher.topic.testcomms";
        private readonly string _messageContent = "abracadabra";
        private readonly string _correlationId = "testCorrelationId";
        private readonly string _responseContent = "crashbangwallop.boom";
        private readonly string _responseTopic;

        public CommunicationViaRabbitMQSteps()
        {
            IntegrationTestHelper.SetUpFixture();
            
            _correlationId = _correlationId + randomGenerator.Next().ToString();
            _messageContent = _messageContent + randomGenerator.Next().ToString();

            _peerOne = new CommunicatingPeer("PeerOne");
            _peerTwo = new CommunicatingPeer("PeerTwo");
            _synchronizer = new CommunicationSynchronizer();

        }

        [BeforeScenario("communication")]
        public void BeforeTestScenario()
        {
            _synchronizer.Reset();
        }

        [AfterScenario("communication")]
        public void AfterScenario()
        {
            _peerOne.Disconnect();
            _peerTwo.Disconnect();
        }



        [Given(@"The test process connects to Rabbit MQ as Peer One")]
        public void GivenTheTestProcessConnectsToRabbitMQAsPeerOne()
        {
            _peerOne.ConnectToRabbitMQ(_responseTopic);
        }
        
        [Given(@"The test process connects to Rabbit MQ as Peer Two")]
        public void GivenTheTestProcessConnectsToRabbitMQAsPeerTwo()
        {
            _peerTwo.ConnectToRabbitMQ(_responseTopic);
        }
        
        [Given(@"Peer Two subscribes on a test topic")]
        public void GivenPeerTwoSubscribesOnATestTopic()
        {
            _synchronizer.Subscribe(_peerTwo, _testTopic);
        }
        
        [Given(@"Peer One sends a message on the test topic")]
        [When(@"Peer One sends a message on the test topic")]
        public void PeerOneSendsAMessageOnTheTestTopic()
        {
            _synchronizer.Send(_peerOne,_testTopic,_messageContent, _correlationId);
        }

        [When(@"Peer One sends a message on the test topic expecting a response")]
        public void WhenPeerOneSendsAMessageOnTheTestTopicExpectingAResponse()
        {
            _synchronizer.Send(_peerOne, _testTopic, _messageContent, _correlationId,
                _synchronizer.ReceiveResponse);
            //Assert.IsNotNullOrEmpty(_synchronizer.ReplyTo);
        }

        
        [When(@"Peer Two unsubscribes to the test topic")]
        public void WhenPeerTwoUnsubscribesToTheTestTopic()
        {
            _synchronizer.Unsubscribe(_peerTwo, _testTopic);
            _synchronizer.Reset();
        }

        [Given(@"Peer Two receives the message")]
        [When(@"Peer Two receives the message")]
        [Then(@"Peer Two receives the message")]
        public void PeerTwoReceivesTheMessage()
        {
            Assert.IsTrue(_synchronizer.ReceivedMessage == _messageContent
               && _synchronizer.ReceivedMsgCorrelationId == _correlationId);
        }

        [Then(@"Peer Two sends a response")]
        public void ThenPeerTwoSendsAResponse()
        {
            _synchronizer.Send(_peerTwo,_synchronizer.ReplyTo,_responseContent, _correlationId);
        }

        [Then(@"Peer One receives the response with the same correlation id on the response topic")]
        public void ThenPeerOneReceivesTheResponseWithTheSameCorrelationIdOnTheResponseTopic()
        {
            Assert.IsTrue(_synchronizer.ReceivedResponseMessage == _responseContent
                && _synchronizer.ReceivedResponseCorrelationId == _correlationId);
        }     
        
        [Then(@"Peer Two does not receive the message")]
        public void ThenPeerTwoDoesNotReceiveTheMessage()
        {
            Assert.False(_synchronizer.ReceivedMessage == _messageContent);
        }
    }
}
