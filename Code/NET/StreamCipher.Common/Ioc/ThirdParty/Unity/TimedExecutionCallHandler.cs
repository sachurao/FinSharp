using System;
using System.Diagnostics;
using Microsoft.Practices.Unity.InterceptionExtension;
using StreamCipher.Common.Ioc.Impl;
using StreamCipher.Common.Logging;

namespace StreamCipher.Common.Ioc.ThirdParty.Unity
{
    public class TimedExecutionCallHandler : ICallHandler
    {
        private int _order = 0;
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            ILoggingService loggingservice = StreamCipherServiceLocator.GetImplementationOf<ILoggingService>();
            Type targetType = input.Target.GetType();
            loggingservice.Info(targetType, String.Format(
                "Executing method {0}", 
                input.MethodBase.Name));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = getNext()(input, getNext);
            stopwatch.Stop();
            loggingservice.Info(targetType, String.Format(
                "Completed execution of method {0}.  Time taken = {2} ", 
                input.MethodBase.Name, targetType, stopwatch.Elapsed));
            
            return result;
        }

        public int Order
        {
            get
            {
                return _order;
            }
            set
            {
                _order = value;
            }
        }
    }
}
