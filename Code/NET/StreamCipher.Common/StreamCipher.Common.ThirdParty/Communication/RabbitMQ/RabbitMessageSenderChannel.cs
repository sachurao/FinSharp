using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.Communication;
using StreamCipher.Common.Components.Communication;
using StreamCipher.Common.Interfaces.DataInterchange;
using RabbitMQ.Client;

namespace StreamCipher.Common.ThirdParty.Communication.RabbitMQ
{
    public class RabbitMessageSenderChannel:BaseRabbitCommunicationChannel, IMessageSenderChannel
    {

        #region Init
        
        public RabbitMessageSenderChannel(DataInterchangeFormat format,
            ICommunicationServiceConfig config, int instanceNum,
            Action<Exception> defaultExceptionHandler) :
            base(format, config, "Sender_" + instanceNum.ToString(), defaultExceptionHandler)
        {
        }

        #endregion
        
        #region IMessageSenderChannel

        public void Send(IMessageDestination destination, IMessageWrapper message)
        {
            IBasicProperties basicProperties = _session.CreateBasicProperties();
            _session.BasicPublish(EXCHANGE_NAME, destination.Address,
                basicProperties, Encoding.UTF8.GetBytes(message.Content.ToString()));
        }

        public IMessageWrapper SendRpc(IMessageDestination destination, IMessageWrapper message)
        {
            throw new NotImplementedException();
        } 
        
        #endregion
    }
}
