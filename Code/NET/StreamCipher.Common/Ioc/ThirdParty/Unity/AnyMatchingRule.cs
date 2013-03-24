using Microsoft.Practices.Unity.InterceptionExtension;

namespace StreamCipher.Common.Ioc.ThirdParty.Unity
{
    public class AnyMatchingRule:IMatchingRule
    {

        public bool Matches(System.Reflection.MethodBase member)
        {
            return true;
        }
    }
}
