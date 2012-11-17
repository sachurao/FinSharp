using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Components.Tests.Async;
using System.Threading;
//using StreamCipher.Common.Components.Tests.Communication;
using StreamCipher.Common.Ioc.DependencyResolution;
using StreamCipher.Common.DependencyResolution;
using StreamCipher.Common.Interfaces.Logging;

namespace StreamCipher.Common.Components.Tests
{
    public class IntegrationTestHarness
    {
        public static void Main(string[] args)
        {
            ConsoleLoggingService loggingService = new ConsoleLoggingService();
            loggingService.LoggingLevel = LoggingLevel.DEBUG;
            ProgrammaticDependencyManager dependencyManager = new ProgrammaticDependencyManager();
            dependencyManager.Register<ILoggingService, ConsoleLoggingService>(loggingService);
            ServiceLocator.Initialise(dependencyManager);
            
            /*

            BackgroundQueue_Sequential_StopAbruptly test = new BackgroundQueue_Sequential_StopAbruptly();
            test.Run(args);

            BackgroundQueue_Sequential_EmptyQueueBeforeShutdown test2 = new BackgroundQueue_Sequential_EmptyQueueBeforeShutdown();
            test2.Run(args);
            
            BackgroundQueue_Sequential_WaitWhenEmpty test3 = new BackgroundQueue_Sequential_WaitWhenEmpty();
            test3.Run(args);
            
            BackgroundQueue_AddingWhenStopped_ThrowsException test4 = new BackgroundQueue_AddingWhenStopped_ThrowsException();
            test4.Run(args);
            
            BackgroundQueue_WaitForNextItemUntilStopped test5= new BackgroundQueue_WaitForNextItemUntilStopped();
            test5.Run(args);
            
            */

            //RabbitCommunicationServiceTests tc = new RabbitCommunicationServiceTests();

        }
    }
}
