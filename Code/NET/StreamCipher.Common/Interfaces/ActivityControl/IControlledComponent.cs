using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.Common.Interfaces.ActivityControl
{
    public enum ActivationStatus
    {
        ACTIVE = 0,
        INACTIVE = 1
    }

    /// <summary>
    /// The interface needs no explanation.  The whole is indeed sum of its parts.
    /// </summary>
    public interface IControlledComponent :ICanStart,ICanShutdown
    {
        ActivationStatus Status { get; }
    }
}
