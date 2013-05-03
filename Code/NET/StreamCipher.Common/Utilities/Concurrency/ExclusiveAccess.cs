using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace StreamCipher.Common.Utilities.Concurrency
{
    public class ThreadSafe
    {
        private readonly Object _syncRoot = new object();
        
        public void Do(Action action)
        {
            lock(_syncRoot)
            {
                action();
            }
        }
    }

    public class ExclusiveAccess<T> where T: class
    {
        private readonly Object _syncRoot = new object();
        private T _thing;

        public ExclusiveAccess(T thing)
        {
            _thing = thing;
        }

        public T Use
        {
            get
            {
                lock(_syncRoot)
                {
                    return _thing;
                }        
            }
        }
    }
}
