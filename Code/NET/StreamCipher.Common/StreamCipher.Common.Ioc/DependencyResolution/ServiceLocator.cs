using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using StreamCipher.Common.Utilities.Concurrency;
using StreamCipher.Common.DependencyResolution;

namespace StreamCipher.Common.Ioc.DependencyResolution
{
    /// <summary>
    /// Provides single-point access to all the services available within the application.
    /// Acts as a facade hiding all details of dependency injection.
    /// </summary>
    public static class ServiceLocator
    {

        #region Member Variables

        private static IDependencyManager _dependencyManager;
        private static AtomicBoolean _isInitialised = new AtomicBoolean(false);
        
        #endregion

        #region Public Methods

        public static void Initialise(IDependencyManager resolver)
        {
            if (_isInitialised.CompareAndSet(false, true))
            {
                _dependencyManager = resolver;
            }
            else throw new InvalidOperationException("ServiceLocator has already been initialised.  Please call ClearServices method.");
        }

        public static T GetImplementationOf<T>()
        {
            if (!_isInitialised.Value) throw new InvalidOperationException("ServiceLocator has not been initialised.  Please call Initialise method first.");
            return _dependencyManager.Resolve<T>();
        }

        public static void ClearServices()
        {
            if (_isInitialised.CompareAndSet(true, false))
                _dependencyManager = null;
        }
        
        #endregion
                
    }
}
