using System;
using StreamCipher.Common.Communication.Impl;
using StreamCipher.Common.DataInterchange;
using StreamCipher.Common.Interfaces.DataInterchange;

namespace StreamCipher.Common.Communication.ThirdParty.RabbitMQ
{
    public class RabbitChannelFactory:ICommunicationChannelFactory
    {
        private readonly FormatterFactory _dataInterchangeFormatterFactory = new FormatterFactory();
        private readonly IncomingMessageManager _incomingMessageManager;
  
        public RabbitChannelFactory()
        {
            _incomingMessageManager = new IncomingMessageManager();

        }

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

            return new RabbitMessageReceiverChannel(formatter, config, instanceNum, defaultExceptionHandler, _incomingMessageManager);
        }
    }
}
