namespace StreamCipher.Common.Communication
{
    public interface IProcessAnyMessage
    {
        void ProcessMessageReceived(IIncomingMessage incomingMessage);
    }
}
