using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.Shared.Interfaces.RemoteConnection;
using Irrefutable.Shared.Interfaces.DataInterchange;

namespace Irrefutable.Shared.Interfaces.Communication
{
    public interface ICommunicationChannel:IRemoteConnection
    {
        ServiceBusType MessagingPlatform { get; }
        DataInterchangeFormat Format { get; } 
    }
}
