﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.Common.Interfaces.Communication
{
    public interface IMessageSenderChannel:ICommunicationChannel
    {
        void Send(IMessageDestination destination, IMessageWrapper message);
        IMessageWrapper SendRpc(IMessageDestination destination, IMessageWrapper message);
    }
}
