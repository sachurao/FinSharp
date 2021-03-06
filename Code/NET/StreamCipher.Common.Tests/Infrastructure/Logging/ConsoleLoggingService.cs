﻿using System;
using System.Threading;
using StreamCipher.Common.Logging;

namespace StreamCipher.Common.Tests.Infrastructure.Logging
{
    /// <summary>
    /// Streams output to console.
    /// </summary>
    public class ConsoleLoggingService:ILoggingService
    {
        private volatile LoggingLevel _logLevel = LoggingLevel.INFO;

        #region ILoggingService
        public void Debug(Type callerType, string debugMessage)
        {
            if (_logLevel <= LoggingLevel.DEBUG) Log(callerType.Name, "DEBUG", debugMessage);
        }

        public void Info(Type callerType, string infoMessage)
        {
            if (_logLevel <= LoggingLevel.INFO) Log(callerType.Name, "INFO", infoMessage);
        }

        public void Warn(Type callerType, string warnMessage)
        {
            if (_logLevel <= LoggingLevel.WARN) Log(callerType.Name, "WARN", warnMessage);
        }

        public void Error(Type callerType, string errorMessage, Exception exception = null)
        {
            if (_logLevel <= LoggingLevel.ERROR)
            {
                var msg = errorMessage + "\n" + 
                    (exception == null ? String.Empty : exception.Message + "\n" + exception.StackTrace);
                Log(callerType.Name, "ERROR", msg);
            }
        }

        public void Fatal(Type callerType, string fatalMessage)
        {
            if (_logLevel <= LoggingLevel.FATAL) Log(callerType.Name, "FATAL", fatalMessage);
        }

        public LoggingLevel LoggingLevel
        {
            set { _logLevel = value; }
        }

        #endregion

        private void Log (String typeName, String messageLevel, String message)
        {
            Console.WriteLine(String.Format("{0} {1} [{2}] {3}: {4}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"), messageLevel, Thread.CurrentThread.ManagedThreadId, typeName, message));
        }

    }
}
