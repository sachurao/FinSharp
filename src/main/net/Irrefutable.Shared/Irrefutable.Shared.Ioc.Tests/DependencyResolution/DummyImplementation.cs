using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irrefutable.Shared.Ioc.Tests.DependencyResolution
{
    public class DummyImplementation:DummyBaseClass, IDummyInterface
    {
        public void DoSomething()
        {
            //Do something...
        }
    }
}
