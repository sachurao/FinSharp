using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.Common.Interfaces.Collections
{
    /// <summary>
    /// Represents a pool of objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectPool<T>
    {
        T Borrow();
        void Return(T item);
    }
}
