using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Irrefutable.Shared.DependencyResolution;

namespace Irrefutable.Shared.Ioc.DependencyResolution
{
    /// <summary>
    /// Allows dependencies to be programmatically defined.
    /// Once dependencies are registered, it can then be used to instantiate <see cref="ServiceLocator"/>
    /// </summary>
    public class ProgrammaticDependencyManager:IDependencyManager
    {
        #region Member Variables

        private readonly object _syncRoot = new object();
        private ConcurrentDictionary<Type, Type> _dependencies = new ConcurrentDictionary<Type, Type>();
        private ConcurrentDictionary<Type, object> _services = new ConcurrentDictionary<Type, object>();

        #endregion

        #region Public Properties

        public IDictionary<Type, Type> Dependencies
        {
            get { return _dependencies; }
        }

        public IDictionary<Type, object> ResolvedServices
        {
            get { return _services; }
        }

        #endregion

        #region Implementation of IDependencyResolver

        public IDependencyManager Register<T1, T2>()
        {
            Type inter = typeof(T1);
            Type impl = typeof(T2);
            if (!inter.IsInterface)
                throw new ArgumentException("Generic parameter T1 should be an interface", "T1");
            if (!inter.IsAssignableFrom(impl))
                throw new ArgumentException("Instance of type T2 should be assignable to variable of type T1", "T2");
            lock (_syncRoot)
            {
                object instance;
                _services.TryRemove(inter, out instance);
                _dependencies.AddOrUpdate(inter, impl, (key, existingVal) => { return impl; });
            }
            return this;
        }

        public T Resolve<T>()
        {
            Type type = typeof(T);
            if (!_dependencies.ContainsKey(type))
                throw new UnregisteredServiceException(
                    String.Format("Could not find an implementation registered for the type {0}",
                    type.FullName));

            ///Using blocking synchronization to ensure
            ///caller class always receives instance of 
            ///dependency currently registered.
            
            lock (_syncRoot)
            {
                return (T)_services.GetOrAdd(type, (t) => { return Activator.CreateInstance(_dependencies[t]); });
            }
        } 
        
        #endregion

        public IDependencyManager Register<T1, T2>(T2 instance)
        {
            var retVal = Register<T1, T2>();
            _services.TryAdd(typeof(T1), instance);
            return retVal;
        }
    }
}
