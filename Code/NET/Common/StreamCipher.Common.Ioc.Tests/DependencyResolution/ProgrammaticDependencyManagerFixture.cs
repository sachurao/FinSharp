using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using StreamCipher.Common.Ioc.DependencyResolution;

namespace StreamCipher.Common.Ioc.Tests.DependencyResolution
{
    [TestFixture]
    public class ProgrammaticDependencyManagerFixture
    {
        ProgrammaticDependencyManager _resolver;
        [SetUp]
        public void SetUp()
        {
            _resolver = new ProgrammaticDependencyManager();
        }

        [Test]
        public void Dependencies_WhenInstantiated_ShouldReturnEmptyDictionary_Test()
        {
            //Assert.IsNotNull(_resolver.Dependencies);
            //Assert.That(!_resolver.Dependencies.Any());
            _resolver.Dependencies.Should().NotBeNull().And.BeEmpty();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Register_WhenType1IsNotInterface_ThrowsException_Test()
        {
            _resolver.Register<DummyBaseClass, DummyImplementation>();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Register_WhenType2IsNotImplementation_ThrowsException_Test()
        {
            _resolver.Register<IDummyInterface, DummyNonImplementation>();
        }

        [Test]
        public void Dependencies_WhenValidDependencyIsRegistered_ContainsEntry_Test()
        {
            _resolver.Register<IDummyInterface, DummyImplementation>();
            _resolver.Dependencies.Should().NotBeEmpty().And.ContainKey(typeof(IDummyInterface));
        }

        [Test]
        public void ResolvedServices_WhenValidDependencyIsRegistered_DoesNotContainEntry_Test()
        {
            _resolver.Register<IDummyInterface, DummyImplementation>();
            _resolver.ResolvedServices.Should().NotContainKey(typeof(IDummyInterface));
        }

        [Test]
        public void Resolve_WhenValidDependencyIsRegistered_ReturnsInstanceOfImplementation_Test()
        {
            _resolver.Register<IDummyInterface, DummyImplementation>();
            var service = _resolver.Resolve<IDummyInterface>();
            service.Should().NotBeNull().And.BeAssignableTo<IDummyInterface>();
        }

        [Test]
        public void ResolvedServices_WhenDependencyIsResolved_ContainsEntry_Test()
        {
            _resolver.Register<IDummyInterface, DummyImplementation>();
            _resolver.Resolve<IDummyInterface>();
            _resolver.ResolvedServices.Should().ContainKey(typeof(IDummyInterface));
        }

        [Test]
        public void Dependencies_WhenDependencyIsOverWritten_ContainsNewEntry_Test()
        {
            _resolver.Register<IDummyInterface, DummyImplementation>();
            _resolver.Register<IDummyInterface, DummyImpl2>();
            _resolver.Dependencies.Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.Contain(typeof(IDummyInterface), typeof(DummyImpl2));
        }

        [Test]
        public void Dependencies_WhenDependencyIsOverWritten_RemovesPreviouslyResolvedService_Test()
        {
            _resolver.Register<IDummyInterface, DummyImplementation>();
            _resolver.Register<IDummyInterface, DummyImpl2>();
            _resolver.ResolvedServices.Should().NotContainKey(typeof(IDummyInterface));
        }

    }
}
