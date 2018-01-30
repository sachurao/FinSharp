using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using StreamCipher.Common.Communication.ThirdParty.RabbitMQ;
using StreamCipher.Common.Logging;

namespace StreamCipher.Common.Communication.ThirdParty.RabbitMQ
{
    public class PoolableRabbitSenderChannel : BaseRabbitCommunicationChannel, IMessageSenderChannel
    {
        #region Init

        public PoolableRabbitSenderChannel(ICommunicationServiceConfig config,
                                           int instanceNum)
            : base(config, "Sender_" + instanceNum.ToString())
        {
        }

        #endregion

        #region IMessageSenderChannel

        public void Send(IMessageDestination destination, IMessageWrapper message,
            IMessageDestination responseDestination = null)
        {
            Logger.Debug(this, String.Format("{0}: Just sending message to {1} with correlation id {2}", 
                this.ConnectionId, destination.Address,
                message.CorrelationId));
            IBasicProperties basicProperties = _session.UseToRetrieve(s=>s.CreateBasicProperties());
            basicProperties.AppId = this.ConnectionId;
            basicProperties.CorrelationId = message.CorrelationId;
            if (responseDestination != null)
                basicProperties.ReplyTo = responseDestination.Address;
            _session.Do(s=>s.BasicPublish(EXCHANGE_NAME, destination.Address,
                basicProperties, Formatter.Serialize(message.Content)));
            Logger.Debug(this, String.Format("{0}: Just sent message to {1} with correlation id {2}", this.ConnectionId, destination.Address,
                message.CorrelationId));

        }



        #endregion



    }
}
