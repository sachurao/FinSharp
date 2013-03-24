using System;

namespace StreamCipher.Common.Communication
{
    public interface ICommunicationChannelPool:IDisposable
    {

        void AddSenderChannel(IMessageSenderChannel senderChannel);
        void AddReceiverChannel(IMessageReceiverChannel receiverChannel);
        void ActivatePool();

        IMessageSenderChannel GetSenderChannel();
        void PutSenderChannel(IMessageSenderChannel senderChannel);

        IMessageDestination UniqueResponseDestination { get; set; }
        IProcessResponses ResponseProcessor { get; }

        void Subscribe(IMessageDestination topic, Action<IIncomingMessage> incomingMessageHandler);
        void Unsubscribe(IMessageDestination topic);

    }
}
