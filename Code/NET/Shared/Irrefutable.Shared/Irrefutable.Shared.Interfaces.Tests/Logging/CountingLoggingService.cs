using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.Shared.Interfaces.Logging;

namespace Irrefutable.Shared.Interfaces.Tests.Logging
{
    /// <summary>
    /// Dummy logging service that counts logging calls
    /// </summary>
    public class CountingLoggingService:ILoggingService
    {
        public int CalledDebug { get; set; }
        public int CalledInfo { get; set; }
        public int CalledWarn { get; set; }
        public int CalledError { get; set; }
        public int CalledFatal { get; set; }

        public void Debug(Type callerType, string debugMessage)
        {
            CalledDebug++;
        }

        public void Info(Type callerType, string infoMessage)
        {
            CalledInfo++;
        }

        public void Warn(Type callerType, string warnMessage)
        {
            CalledWarn++;
        }

        public void Error(Type callerType, string errorMessage, Exception exception = null)
        {
            CalledError++;
        }

        public void Fatal(Type callerType, string fatalMessage)
        {
            CalledFatal++;
        }


        public LoggingLevel LoggingLevel
        {
            set 
            { 
                //Do nothing... 
            }
        }
    }
}
