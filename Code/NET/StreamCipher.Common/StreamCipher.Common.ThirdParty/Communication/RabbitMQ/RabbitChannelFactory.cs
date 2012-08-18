using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.Communication;
using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.ThirdParty.DataInterchange;
using StreamCipher.Common.Utilities.DataInterchange;

namespace StreamCipher.Common.ThirdParty.Communication.RabbitMQ
{
    public class RabbitChannelFactory:ICommunicationChannelFactory
    {
        private readonly FormatterFactory _dataInterchangeFormatterFactory = new FormatterFactory();

        public IMessageSenderChannel CreateMessageSenderChannel(CommunicationMode mode, ICommunicationServiceConfig config, Action<Exception> defaultExceptionHandler, int instanceNum)
        {
            IDataInterchangeFormatter formatter =
                _dataInterchangeFormatterFactory.CreateFormatter(mode.Format);
            return new RabbitMessageSenderChannel(formatter, config, instanceNum, defaultExceptionHandler);
        }

        public IMessageReceiverChannel CreateMessageReceiverChannel(CommunicationMode mode, ICommunicationServiceConfig config, Action<Exception> defaultExceptionHandler, int instanceNum)
        {
            IDataInterchangeFormatter formatter =
                _dataInterchangeFormatterFactory.CreateFormatter(mode.Format);

            return new RabbitMessageReceiverChannel(formatter, config, instanceNum, defaultExceptionHandler);
        }
    }
}
