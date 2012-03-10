﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irrefutable.Shared.Interfaces.Communication
{
    public interface ICommunicationServiceConfig
    {
        String ConnectionIdPrefix { get; }
        int TotalSenderChannels { get; }
        int TotalReceiverChannels { get; }
        IDictionary<String, String> CustomProps { get; }
    }
}