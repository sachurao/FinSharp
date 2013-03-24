using System;

namespace StreamCipher.Common.Communication
{
    //hello
    public interface IProcessBroadcasts:IProcessAnyMessage
    {
        event Action<IIncomingMessage> OnMessageReceived;
    }
}
