using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.Common.Interfaces.Communication
{
    public interface ICommunicationChannelPool:IDisposable
    {

        void AddSenderChannel(IMessageSenderChannel senderChannel);
        void AddReceiverChannel(IMessageReceiverChannel receiverChannel);
        void ActivatePool();

        IMessageSenderChannel GetSenderChannel();
        void PutSenderChannel(IMessageSenderChannel senderChannel);
        void Subscribe(IMessageDestination topic, Action<IIncomingMessage> incomingMessageHandler);
        void Unsubscribe(IMessageDestination topic);

    }
}
