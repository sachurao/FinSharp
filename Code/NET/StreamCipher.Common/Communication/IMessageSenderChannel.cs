namespace StreamCipher.Common.Communication
{
    public interface IMessageSenderChannel:ICommunicationChannel
    {
        void Send(IMessageDestination destination, IMessageWrapper message,
            IMessageDestination responseDestination = null);
    }
}
