﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irrefutable.Shared.Interfaces.ActivityControl
{
    public enum ActivationStatus
    {
        ACTIVE = 0,
        INACTIVE = 1
    }

    /// <summary>
    /// The interface needs no explanation.  The whole is indeed sum of its parts.
    /// </summary>
    public interface IControlledActivity :ICanStart,ICanShutdown
    {
        ActivationStatus Status { get; }
    }
}