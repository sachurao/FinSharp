using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.Common.Interfaces.Communication
{
    // Could be an individual endpoint destination or a broadcast topic
    public interface IMessageDestination
    {
        String Address { get; }
    }
}
