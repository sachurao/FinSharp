using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.Interfaces.RemoteConnection;
using StreamCipher.Common.Pooling;

namespace StreamCipher.Common.Communication
{
    public interface ICommunicationChannel : IRemoteConnection, IPoolableObject
    {
        IDataInterchangeFormatter Formatter { get; }

    }
}
