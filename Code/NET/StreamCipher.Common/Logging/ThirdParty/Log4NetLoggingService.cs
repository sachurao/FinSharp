using System;
using System.Collections.Generic;
using log4net;
using log4net.Config;
using log4net.Core;

namespace StreamCipher.Common.Logging.ThirdParty
{
    /// <summary>
    /// Implementing LoggingService using Log4Net
    /// </summary>
    public class Log4NetLoggingService:ILoggingService
    {
        #region Member Variables

        private IDictionary<LoggingLevel, Level> _logLevelMapping = new Dictionary<LoggingLevel, Level>();

        #endregion
        
        #region Init
        
        public Log4NetLoggingService()
        {
            //TODO:  Read log4net configuration from config file.
            BasicConfigurator.Configure();

            _logLevelMapping.Add(LoggingLevel.DEBUG, Level.Debug);
            _logLevelMapping.Add(LoggingLevel.INFO, Level.Info);
            _logLevelMapping.Add(LoggingLevel.WARN, Level.Warn);
            _logLevelMapping.Add(LoggingLevel.ERROR, Level.Error);
            _logLevelMapping.Add(LoggingLevel.FATAL, Level.Fatal);
        }

        #endregion
        
        #region ILoggingService
		
        public void Debug(Type callerType, string debugMessage)
        {
            ILog log = LogManager.GetLogger(callerType);
            if (log.IsDebugEnabled)
            {
                log.Debug(debugMessage);
            }
        }

        public void Info(Type callerType, string infoMessage)
        {
            ILog log = LogManager.GetLogger(callerType);
            if (log.IsInfoEnabled)
            {
                log.Info(infoMessage);
            }
        }

        public void Warn(Type callerType, string warnMessage)
        {
            ILog log = LogManager.GetLogger(callerType);
            if (log.IsWarnEnabled)
            {
                log.Warn(warnMessage);
            }
        }

        public void Error(Type callerType, string errorMessage, Exception exception = null)
        {
            ILog log = LogManager.GetLogger(callerType);
            if (log.IsErrorEnabled)
            {
                log.Error(errorMessage, exception);
            }
        }

        public void Fatal(Type callerType, string fatalMessage)
        {
            ILog log = LogManager.GetLogger(callerType);
            if (log.IsFatalEnabled)
            {
                log.Fatal(fatalMessage);
            }
        }

        public LoggingLevel LoggingLevel
        {
            set 
            {
                LogManager.GetRepository().Threshold = _logLevelMapping[value];
            }
        }

	    #endregion
    }
}
