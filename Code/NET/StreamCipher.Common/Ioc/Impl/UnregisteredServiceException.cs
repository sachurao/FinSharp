using System;

namespace StreamCipher.Common.Ioc.Impl
{
    [Serializable]
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
