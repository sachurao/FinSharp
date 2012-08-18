using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.DataInterchange;

namespace StreamCipher.Common.Interfaces.Communication
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
