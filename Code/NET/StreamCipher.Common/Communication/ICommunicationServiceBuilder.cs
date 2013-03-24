using System;

namespace StreamCipher.Common.Communication
{
    public interface ICommunicationServiceBuilder
    {
        ICommunicationServiceBuilder WithConfig(ICommunicationServiceConfig config);
        ICommunicationServiceBuilder WithDefaultExceptionHandler(Action<Exception> handleException);
        void Now();
    }
}
