using System;

namespace StreamCipher.Common.Communication
{
    // Could be an individual endpoint destination or a broadcast topic
    public interface IMessageDestination
    {
        String Address { get; }
    }

}
