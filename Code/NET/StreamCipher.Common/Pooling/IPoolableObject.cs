using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamCipher.Common.Pooling
{
    public interface IPoolableObject
    {
        bool IsValidToUse { get; }
    }
}
