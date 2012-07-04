using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.RemoteConnection;
using StreamCipher.Common.Interfaces.DataInterchange;

namespace StreamCipher.Common.Interfaces.Communication
{
    public interface ICommunicationChannel:IRemoteConnection
    {
        ServiceBusType MessagingPlatform { get; }
        DataInterchangeFormat Format { get; } 
    }
}
