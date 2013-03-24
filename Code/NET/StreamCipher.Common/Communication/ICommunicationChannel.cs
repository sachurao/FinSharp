using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.Interfaces.RemoteConnection;

namespace StreamCipher.Common.Communication
{
    public interface ICommunicationChannel:IRemoteConnection
    {
        ServiceBusType MessagingPlatform { get; }
        IDataInterchangeFormatter Formatter { get; }
    }
}
