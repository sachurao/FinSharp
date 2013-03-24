using System;
using StreamCipher.Common.Utilities.Concurrency;

namespace StreamCipher.Common.Ioc.Impl
{
    /// <summary>
    /// Provides single-point access to all the services available within the application.
    /// Acts as a facade hiding all details of dependency injection.
    /// </summary>
    public static class StreamCipherServiceLocator
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
            else throw new InvalidOperationException("StreamCipherServiceLocator has already been initialised.  Please call ClearServices method.");
        }

        public static T GetImplementationOf<T>()
        {
            if (!_isInitialised.Value) throw new InvalidOperationException("StreamCipherServiceLocator has not been initialised.  Please call Initialise method first.");
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
