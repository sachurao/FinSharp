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

        public T Get
        {
            get
            {
                lock(_syncRoot)
                {
                    return _thing;
                }        
            }
        }

        public void Do(Action<T> doSomethingWithIt)
        {
            lock(_syncRoot)
            {
                doSomethingWithIt(_thing);
            }
        }

        public T2 UseToRetrieve<T2>(Func<T,T2> func)
        {
            lock (_syncRoot)
            {
                return func(_thing);
            }
        }


    }
}
