using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.Shared.Interfaces.Interception;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension.Configuration;

namespace Irrefutable.Shared.ThirdParty.Ioc.Interception
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
