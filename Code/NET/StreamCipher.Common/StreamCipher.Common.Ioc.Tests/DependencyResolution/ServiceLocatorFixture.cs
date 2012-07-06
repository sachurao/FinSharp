using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using StreamCipher.Common.DependencyResolution;
using StreamCipher.Common.Ioc.DependencyResolution;

namespace StreamCipher.Common.Ioc.Tests.DependencyResolution
{
    [TestFixture]
    class ServiceLocatorFixture
    {
        private IDependencyManager _resolver;
        [SetUp]
        public void SetUp()
        {
            ServiceLocator.ClearServices();
            _resolver = new ProgrammaticDependencyManager();
            _resolver.Register<IDummyInterface, DummyImplementation>();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetImplementationOf_BeforeInitialising_ThrowsException_Test()
        {
            ServiceLocator.GetImplementationOf<IDummyInterface>();
        }

        [Test]
        public void GetImplementationOf_AfterInitialisation_ReturnsService_Test()
        {
            ServiceLocator.Initialise(_resolver);
            ServiceLocator.GetImplementationOf<IDummyInterface>().Should().BeOfType<DummyImplementation>();
        }
                

    }
}
