using System;
using RabbitMQ.Client;
using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.Logging;

namespace StreamCipher.Common.Communication.ThirdParty.RabbitMQ
{
    public class RabbitMessageSenderChannel:BaseRabbitCommunicationChannel, IMessageSenderChannel
    {
        #region Init
        
        public RabbitMessageSenderChannel(IDataInterchangeFormatter formatter,
            ICommunicationServiceConfig config, int instanceNum,
            Action<Exception> defaultExceptionHandler) :
            base(formatter, config, "Sender_" + instanceNum.ToString(), defaultExceptionHandler)
        {
        }

        #endregion
        
        #region IMessageSenderChannel

        public void Send(IMessageDestination destination, IMessageWrapper message, 
            IMessageDestination responseDestination = null)
        {
            Logger.Debug(this, String.Format("{0}: Sending message to {1} with correlation id {2}", this.ConnectionId, destination.Address,
                message.CorrelationId));
            IBasicProperties basicProperties = _session.Use.CreateBasicProperties();
            basicProperties.AppId = this.ConnectionId;
            basicProperties.CorrelationId = message.CorrelationId;
            if (responseDestination != null)
                basicProperties.ReplyTo = responseDestination.Address;
            _session.Use.BasicPublish(EXCHANGE_NAME, destination.Address,
                basicProperties, Formatter.Serialize(message.Content.ToString()));
            Logger.Debug(this, String.Format("{0}: Sent message to {1} with correlation id {2}", this.ConnectionId, destination.Address,
                message.CorrelationId));
            
        }
        
        #endregion

        


    }
}
