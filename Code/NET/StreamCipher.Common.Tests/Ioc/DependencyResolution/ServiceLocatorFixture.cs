using System;
using NUnit.Framework;
using FluentAssertions;
using StreamCipher.Common.Ioc;
using StreamCipher.Common.Ioc.Impl;

namespace StreamCipher.Common.Tests.Ioc.DependencyResolution
{
    [TestFixture]
    class ServiceLocatorFixture
    {
        private IDependencyManager _resolver;
        [SetUp]
        public void SetUp()
        {
            StreamCipherServiceLocator.ClearServices();
            _resolver = new ProgrammaticDependencyManager();
            _resolver.Register<IDummyInterface, DummyImplementation>();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetImplementationOf_BeforeInitialising_ThrowsException_Test()
        {
            StreamCipherServiceLocator.GetImplementationOf<IDummyInterface>();
        }

        [Test]
        public void GetImplementationOf_AfterInitialisation_ReturnsService_Test()
        {
            StreamCipherServiceLocator.Initialise(_resolver);
            StreamCipherServiceLocator.GetImplementationOf<IDummyInterface>().Should().BeOfType<DummyImplementation>();
        }
                

    }
}
