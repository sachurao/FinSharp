using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.DataInterchange;

namespace StreamCipher.Common.Interfaces.Communication
{
    public static class Constants
    {
    }

    public enum ServiceBusType
    {
        RABBITMQ = 0,
        CUSTOM = 1
    }
}
