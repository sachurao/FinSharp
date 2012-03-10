using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.Shared.Interfaces.DataInterchange;
using Irrefutable.Shared.Interfaces.ActivityControl;
using Irrefutable.Shared.Interfaces.RemoteConnection;

namespace Irrefutable.Shared.Interfaces.Communication
{
        public interface ICommunicationServiceBuilder
    {
        ICommunicationServiceBuilder WithConfig(ICommunicationServiceConfig config);
        ICommunicationServiceBuilder WithDefaultExceptionHandler(Action<Exception> handleException);

        void Build();
    }
}
