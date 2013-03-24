using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace StreamCipher.Common.Ioc.ThirdParty.Unity
{
    /// <summary>
    /// Uses Unity IoC container to resolve dependencies.
    /// </summary>
    public class UnityDependencyManager:IDependencyManager
    {
        private IUnityContainer _container = new UnityContainer();
        
        public UnityDependencyManager()
        {
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(_container, "mainContainer");
        }

        public IDependencyManager Register<T1, T2>()
        {
            //_container.RegisterType(typeof(T1), typeof(T2), new InjectionConstructor());
            _container.RegisterType(typeof(T1), typeof(T2), new ContainerControlledLifetimeManager());
            return this;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>() ;
        }

        public IUnityContainer Container
        {
            get { return _container; }
        }
    }
}
