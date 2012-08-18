using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.Interfaces.ActivityControl;
using StreamCipher.Common.Interfaces.RemoteConnection;

namespace StreamCipher.Common.Interfaces.Communication
{
    public interface ICommunicationServiceBuilder
    {
        ICommunicationServiceBuilder WithConfig(ICommunicationServiceConfig config);
        ICommunicationServiceBuilder WithDefaultExceptionHandler(Action<Exception> handleException);
        void Now();
    }
}
