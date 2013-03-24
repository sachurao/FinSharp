using System;
using StreamCipher.Common.Ioc.Impl;

namespace StreamCipher.Common.Logging
{
    /// <summary>
    /// Acts as entry point for all logging within the application.
    /// </summary>
    public static class Logger
    {
        private static ILoggingService _loggingService = StreamCipherServiceLocator.GetImplementationOf<ILoggingService>();
        
        public static void Debug(Object caller, string debugMessage)
        {
            _loggingService.Debug(caller.GetType(), debugMessage);
        }
        public static void Info(Object caller, string infoMessage)
        {
            _loggingService.Info(caller.GetType(), infoMessage);
        }
        public static void Warn(Object caller, string warnMessage)
        {
            _loggingService.Warn(caller.GetType(), warnMessage);
        }
        public static void Error(Object caller, string errorMessage, Exception exception = null)
        {
            _loggingService.Error(caller.GetType(), errorMessage, exception);
        }
        public static void Fatal(Object caller, string fatalMessage)
        {
            _loggingService.Fatal(caller.GetType(), fatalMessage);
        }
    }
}
