using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using Irrefutable.Shared.ThirdParty.Ioc;
using Irrefutable.Shared.Ioc.Tests.DependencyResolution;

namespace Irrefutable.Shared.ThirdParty.Tests.Ioc
{
    [TestFixture]
    public class UnityDependencyManagerFixture
    {
        UnityDependencyManager _unityManager;

        [SetUp]
        public void SetUp()
        {
            _unityManager = new UnityDependencyManager();
        }

        [Test]
        public void Resolve_ValidDependencyDefinedInConfig_ReturnsResolvedService_Test()
        {
            _unityManager.Resolve<IDummyInterface>().Should().NotBeNull().And.BeOfType<DummyImplementation>();
        }

        [Test]
        public void Resolve_WhenDependencyIsOverWritten_ReturnsNewService_Test()
        {
            _unityManager.Register<IDummyInterface, DummyImpl2>();
            _unityManager.Resolve<IDummyInterface>().Should().NotBeNull().And.BeOfType<DummyImpl2>();
        }

    }
}
