﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.Common.Ioc.Tests.DependencyResolution
{
    public class DummyImpl2:IDummyInterface
    {
        public void DoSomething()
        {
            throw new NotImplementedException();
        }
    }
}
