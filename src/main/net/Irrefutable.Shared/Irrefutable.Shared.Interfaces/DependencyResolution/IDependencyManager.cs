﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irrefutable.Shared.DependencyResolution
{
    public interface IDependencyManager
    {
        IDependencyManager Register<T1, T2>();
        T Resolve<T>();
    }
}