using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using StreamCipher.Common.Components.Logging;
using StreamCipher.Common.Interfaces.Communication;
using StreamCipher.Common.Interfaces.DataInterchange;
using RabbitMQ.Client;

namespace StreamCipher.Common.ThirdParty.Communication.RabbitMQ
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

        public void Send(IMessageDestination destination, IMessageWrapper message)
        {
            Logger.Debug(this, String.Format("{0}: Sending message to {1}", this.ConnectionId, destination.Address));
            IBasicProperties basicProperties = _session.Use.CreateBasicProperties();
            _session.Use.BasicPublish(EXCHANGE_NAME, destination.Address,
                basicProperties, Formatter.Serialize(message.Content.ToString()));
            Logger.Debug(this, String.Format("{0}: Sent message to {1}", this.ConnectionId, destination.Address));
            
        }

        public IMessageWrapper SendRpc(IMessageDestination destination, IMessageWrapper message)
        {
            //IBasicProperties basicProperties = _session.CreateBasicProperties();
            //_session.(EXCHANGE_NAME, destination.Address,
            //    basicProperties, Formatter.Serialize(message.Payload.ToString()));
            throw new NotImplementedException();
        } 
        
        #endregion

        


    }
}
