using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using StreamCipher.Common.Interfaces.Collections;

namespace StreamCipher.Common.Utilities.Concurrency
{
    public class ObjectPool<T>:IObjectPool<T>
    {
        private ConcurrentBag<T> _bagOfItems;
        private Func<T> _objectCreator;
        private int _totalInitialised=0;
        private int _capacity;


        public ObjectPool(Func<T> objectCreator) : this(objectCreator, Int32.MaxValue)
        {
            
        }

        public ObjectPool(Func<T> objectCreator, int boundedCapacity)
        {
            _objectCreator = objectCreator;
            _bagOfItems = new ConcurrentBag<T>();
            _capacity = boundedCapacity;
        }

        #region Implementation of IObjectPool<T>

        public T Borrow()
        {
            T retVal;
            if (_bagOfItems.TryTake(out retVal)) return retVal;
            return _objectCreator();
        }

        public void Return(T item)
        {
            _bagOfItems.Add(item);
        }

        #endregion
    }
}
