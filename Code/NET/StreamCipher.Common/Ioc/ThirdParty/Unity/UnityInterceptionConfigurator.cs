using Microsoft.Practices.Unity;

namespace StreamCipher.Common.Ioc.ThirdParty.Unity
{
    public class UnityInterceptionConfigurator:IConfigureInterception
    {
        private IUnityContainer _container;
        public UnityInterceptionConfigurator(IUnityContainer container)
        {
            _container = container;
        }

        public void Configure()
        {
            //Nothing to do... assuming the container is already configured to have interception extension.
        }
    }
}
