using System;

namespace StreamCipher.Common.Tests.Ioc.DependencyResolution
{
    public class DummyImpl2:IDummyInterface
    {
        public void DoSomething()
        {
            throw new NotImplementedException();
        }
    }
}
