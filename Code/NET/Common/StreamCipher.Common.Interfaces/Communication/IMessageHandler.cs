﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.Common.Interfaces.Communication
{
    public interface IMessageHandler
    {
        void HandleIncomingMessage(IIncomingMessage incomingMessage);
    }
}
