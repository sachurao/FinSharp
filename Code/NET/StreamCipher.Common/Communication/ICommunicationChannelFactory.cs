using System;

namespace StreamCipher.Common.Communication
{
    /// <summary>
    /// Creates formatters and communication channels.
    /// </summary>
    public interface ICommunicationChannelFactory
    {
        IMessageSenderChannel CreateMessageSenderChannel(CommunicationMode mode, ICommunicationServiceConfig config, Action<Exception> defaultExceptionHandler, int instanceNum);
        IMessageReceiverChannel CreateMessageReceiverChannel(CommunicationMode mode, ICommunicationServiceConfig config, Action<Exception> defaultExceptionHandler, int instanceNum);

    }
}
