using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irrefutable.Shared.Interfaces.Communication
{
    public interface ICommunicationChannelPool:IDisposable
    {

        void AddSenderChannel(IMessageSenderChannel senderChannel);
        void AddReceiverChannel(IMessageReceiverChannel receiverChannel);
        IMessageSenderChannel GetSenderChannel();
        void PutSenderChannel(IMessageSenderChannel senderChannel);
        void Subscribe(IMessageDestination topic, IMessageHandler messageHandler);
        void Unsubscribe(IMessageDestination topic);
    }
}
