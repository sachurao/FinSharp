using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StreamCipher.Common.Communication;
using StreamCipher.Common.Communication.Impl;
using StreamCipher.Common.Communication.ThirdParty.RabbitMQ;
using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.Logging;

namespace StreamCipher.Common.IntegrationTests.Communication
{

    /*
    connections should be
Env\PeerName\UserName_MachineName_ProcessId_ConnectionType_InstanceNum

streamcipher.request.function
streamcipher.request.uniquePeerAddress
streamcipher.response.uniquePeerAddress (this is also known as uniqueResponseTopic)
streamcipher.broadcast.function

uniquePeerAddress = prefix_username_machinename_processid



1.  Each peer at least has one connection listening on uniqueResponseTopic on connecting to Rabbit MQ.

2.  Each peer when sending a message, if expecting a response,
	
	a.  Populates uniqueResponseTopic on the message.
	b.  Adds a message handler for the correlationId
	c.  Sends the request.

3.  When response is received, if message handler found for correlationId
	handleResponse()
		...
		finally, if saga is complete (no more responses expected), remove handler for correlationId


     */
    class CommunicatingPeer
    {
        public ICommunicationService CommSvc { get; private set; }
        public CommunicationMode CommunicationMode { get; private set; }
        private String _peerName;

        public CommunicatingPeer(String peerName)
        {
            _peerName = peerName;
        }
        
        
        public void ConnectToRabbitMQ(string customResponseTopic)
        {
            CommunicationMode = new CommunicationMode(ServiceBusType.RABBITMQ, DataInterchangeFormat.TEXT_UTF8);
            var config = new DefaultCommunicationServiceConfig("localhost", _peerName, 3,1,customResponseTopic);
            config.CustomProps.Add("VirtualHost", "TestEnv");
            config.CustomProps.Add("UserName", "finsharp");
            config.CustomProps.Add("Password", "streamcipher");

            CommSvc = new DefaultCommunicationService();
            CommSvc.Build(CommunicationMode, new RabbitChannelFactory())
                .WithConfig(config)
                .WithDefaultExceptionHandler((ex) => Logger.Error(CommSvc, "Messaging error", ex))
                .Now();
            /*
            
             *          var config = new DefaultCommunicationServiceConfig(new RabbitChannelFactory(),
             *          DataInterchangeFormat.TEXT_UTF8,
             *          "localhost", _peerName, 3,1,customResponseTopic);
             *          
                        config.CustomProps.Add("VirtualHost", "TestEnv");
                        config.CustomProps.Add("UserName", "finsharp");
                        config.CustomProps.Add("Password", "streamcipher");
             *          config.AddDefaultExceptionHandler((ex) => Logger.Error(CommSvc,"Messaging error", ex));

                        ICommunicationService2 commSvc = new DefaultCommunicationService(config);
             *          commSvc.Start();
            */
        }

        public void Disconnect()
        {
            CommSvc.Dispose();
        }

        
    }

    
    class CommunicationSynchronizer 
    {
        
        private readonly object _syncRoot = new object();

        public String ReplyTo { get; private set; }
        public String LastSentMessageTopic { get; private set; }
        public String LastSentMessage { get; private set; }
        public String LastSentMsgCorrelationId { get; private set; }
        public String ReceivedMessageTopic { get; private set; }
        public String ReceivedMessage { get; private set; }
        public String ReceivedMsgCorrelationId { get; private set; }
        public String ReceivedResponseTopic { get; private set; }
        public String ReceivedResponseMessage { get; private set; }
        public String ReceivedResponseCorrelationId { get; private set; }
        public bool HasMessageBeenReceived { get; private set; }
        public bool HasResponseBeenReceived { get; private set; }
        
        
        public void Send(CommunicatingPeer peer, string topic, string content, string correlationId="whatever",
            Action<IIncomingMessage> responseHandler = null)
        {
            lock (_syncRoot)
            {
                peer.CommSvc.Send(peer.CommunicationMode, 
                    new MessageDestination(topic), 
                    new MessageWrapper(content, correlationId),responseHandler);
                
                LastSentMessageTopic = topic;
                LastSentMessage = content;
                LastSentMsgCorrelationId = correlationId;
                
                Monitor.Wait(_syncRoot, 1000);
            }
        }
        
        public void ReceiveResponse(IIncomingMessage incomingMessage)
        {
            Logger.Debug(this, "Handling incoming response...");
            lock (_syncRoot)
            {
                ReceivedResponseCorrelationId = incomingMessage.CorrelationId;
                ReceivedResponseMessage = incomingMessage.Content.ToString();
                ReceivedResponseTopic = incomingMessage.Topic.Address;
                HasResponseBeenReceived = true;
                Monitor.Pulse(_syncRoot);
            } 
        }

        public void Receive(IIncomingMessage incomingMessage, CommunicatingPeer recipient)
        {
            Logger.Debug(this, "Handling incoming message...");
            lock (_syncRoot)
            {
                string incomingTopic = incomingMessage.Topic.ToString();
                HasMessageBeenReceived = true;
                ReceivedMessageTopic = incomingTopic;
                ReceivedMessage = incomingMessage.Content.ToString();
                ReceivedMsgCorrelationId = incomingMessage.CorrelationId;
                ReplyTo = incomingMessage.ReplyTo != null ? incomingMessage.ReplyTo.Address : String.Empty;
                Logger.Info(this, String.Format("Received message  on topic {0} from {1}",
                    incomingMessage.Topic.Address, incomingMessage.Sender));
                Monitor.Pulse(_syncRoot);
            }
        }

        public void Subscribe(CommunicatingPeer peer, string topic)
        {
            peer.CommSvc.Subscribe(peer.CommunicationMode, new MessageDestination(topic), 
                i => Receive(i, peer));
        }

        public void Unsubscribe(CommunicatingPeer peer, string topic)
        {
            peer.CommSvc.Unsubscribe(peer.CommunicationMode, new MessageDestination(topic));
        }

        public void Reset()
        {
            Logger.Info(this, "Resetting...");
            ReplyTo = String.Empty;
            LastSentMessageTopic = String.Empty;
            LastSentMessage = String.Empty;
            LastSentMsgCorrelationId = String.Empty;
            ReceivedMessageTopic = String.Empty;
            ReceivedMessage = String.Empty;
            ReceivedMsgCorrelationId = String.Empty;
            ReceivedResponseTopic = String.Empty;
            ReceivedResponseMessage = String.Empty;
            ReceivedResponseCorrelationId = String.Empty;
            HasMessageBeenReceived = false;
            HasResponseBeenReceived = false;

        }
    }
}
