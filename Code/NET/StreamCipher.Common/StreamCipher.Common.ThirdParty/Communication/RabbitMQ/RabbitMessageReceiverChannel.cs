using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.Communication;
using StreamCipher.Common.Interfaces.DataInterchange;

namespace StreamCipher.Common.ThirdParty.Communication.RabbitMQ
{
    public class RabbitMessageReceiverChannel: BaseRabbitCommunicationChannel, IMessageReceiverChannel
    {
        #region Init
        
        public RabbitMessageReceiverChannel(IDataInterchangeFormatter formatter,
            ICommunicationServiceConfig config, int instanceNum,
            Action<Exception> defaultExceptionHandler) :
            base(formatter, config, "Receiver_" + instanceNum.ToString(), defaultExceptionHandler)
        {
        }

        #endregion

        #region Implementation of IMessageReceiverChannel

        public void Subscribe(IMessageDestination topicOrQueue, IMessageHandler messageHandler)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(IMessageDestination topicOrQueue)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMessageDestination> Subscriptions
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
