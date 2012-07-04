﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.Common.Interfaces.Communication
{
    public interface IMessageWrapper
    {
        String CorrelationId { get; }
        Object Content { get; }
    }
}
