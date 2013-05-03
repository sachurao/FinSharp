using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamCipher.Common.Pooling
{
    public interface IPoolableObjectFactory<T> where T:IPoolableObject
    {
        T Create();
        void Retire(T item);
    }
}
