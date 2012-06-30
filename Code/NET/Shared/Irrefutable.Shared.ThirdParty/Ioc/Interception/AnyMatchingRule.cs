using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Irrefutable.Shared.ThirdParty.Ioc.Interception
{
    public class AnyMatchingRule:IMatchingRule
    {

        public bool Matches(System.Reflection.MethodBase member)
        {
            return true;
        }
    }
}
