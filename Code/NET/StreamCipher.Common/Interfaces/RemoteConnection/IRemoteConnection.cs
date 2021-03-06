﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.Common.Interfaces.RemoteConnection
{
    public interface IRemoteConnection:ICanConnect,ICanDisconnect
    {
        String ConnectionId { get; }
        bool IsConnected { get; }
    }
}
