using System;

namespace StreamCipher.Common.Logging
{
    public interface ILoggingService
    {
        void Debug(Type callerType, string debugMessage);
        void Info(Type callerType, string infoMessage);
        void Warn(Type callerType, string warnMessage);
        void Error(Type callerType, string errorMessage, Exception exception = null);
        void Fatal(Type callerType, string fatalMessage);

        LoggingLevel LoggingLevel { set; }
    }

    public enum LoggingLevel
    {
        DEBUG = 0,
        INFO = 1,
        WARN = 2,
        ERROR = 3,
        FATAL = 4
    }
}
