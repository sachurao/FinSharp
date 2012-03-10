using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using Irrefutable.Shared.Interfaces.Logging;
using Irrefutable.Shared.Interfaces.Command;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity;
using Irrefutable.Shared.ThirdParty.Ioc;
using Irrefutable.Shared.Interfaces.Tests.Logging;
using Irrefutable.Shared.Ioc.DependencyResolution;

namespace Irrefutable.Shared.ThirdParty.Tests.Ioc
{
    [TestFixture]
    public class UnityInterceptionFixture
    {
        private IUnityContainer _container;
        [SetUp]
        public void SetUp()
        {
            var dependencyManager = new UnityDependencyManager();
            dependencyManager.Register<ILoggingService, CountingLoggingService>();
            ServiceLocator.ClearServices();
            ServiceLocator.Initialise(dependencyManager);
            _container = dependencyManager.Container;
        }

        [Test]
        public void Interception_WhenConfiguredMethodCalled_IsIntercepted_Test()
        {
            //Arrange
            ICommand command = ServiceLocator.GetImplementationOf<ICommand>();
            
            //Act
            command.Execute();

            //Assert
            ((CountingLoggingService)ServiceLocator.GetImplementationOf<ILoggingService>()).CalledInfo.Should().Be(2); 
        }


    }
}
