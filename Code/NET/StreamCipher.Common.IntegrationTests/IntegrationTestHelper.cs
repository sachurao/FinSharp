using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamCipher.Common.Ioc.Impl;
using StreamCipher.Common.Logging;

namespace StreamCipher.Common.IntegrationTests
{
    static class IntegrationTestHelper
    {
        public static void SetUpFixture()
        {
            StreamCipherServiceLocator.ClearServices();
            var loggingService = new ConsoleLoggingService();
            loggingService.LoggingLevel = LoggingLevel.DEBUG;
            var dependencyManager = new ProgrammaticDependencyManager();
            dependencyManager.Register<ILoggingService, ConsoleLoggingService>(loggingService);
            StreamCipherServiceLocator.Initialise(dependencyManager);
        }
    }
}
