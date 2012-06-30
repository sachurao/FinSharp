using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irrefutable.Shared.Ioc.DependencyResolution
{
    class UnregisteredServiceException : Exception
    {
        public UnregisteredServiceException()
            : base()
        {
        }

        public UnregisteredServiceException(string message)
            : base(message)
        {
        }

        public UnregisteredServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
