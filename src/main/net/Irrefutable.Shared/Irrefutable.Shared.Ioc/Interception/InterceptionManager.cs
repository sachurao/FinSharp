using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.Shared.Interfaces.Interception;

namespace Irrefutable.Shared.Ioc.Interception
{
    /// <summary>
    /// Acts as entry-point for instantiating interception.
    /// All interception should be configuration-based.
    /// All objects should be POCO, i.e. no library-specific attributes
    /// </summary>
    public static class InterceptionManager
    {
        public static void SetUp(IConfigureInterception configurator)
        {
            configurator.Configure();
        }
    }
}
