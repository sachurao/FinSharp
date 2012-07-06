using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using StreamCipher.Common.Interfaces.Logging;
using StreamCipher.Common.Interfaces.Command;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity;
using StreamCipher.Common.ThirdParty.Ioc;
using StreamCipher.Common.Interfaces.Tests.Logging;
using StreamCipher.Common.Ioc.DependencyResolution;

namespace StreamCipher.Common.ThirdParty.Tests.Ioc
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
