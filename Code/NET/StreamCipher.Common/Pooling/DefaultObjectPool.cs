using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StreamCipher.Common.Async;
using StreamCipher.Common.Interfaces.ActivityControl;
using StreamCipher.Common.Logging;
using StreamCipher.Common.Utilities.ObjectType;

namespace StreamCipher.Common.Pooling
{
    public class DefaultObjectPool<T> : ControlledComponent, IObjectPool<T> where T:class, IPoolableObject
    {
        #region Member Variables

        protected readonly ObjectPoolConfig<T> _config;
        protected readonly ConcurrentBag<T> _poolImpl;
        private readonly SemaphoreSlim _availabilityGate;
        private readonly HashSet<T> _itemsCreatedByPool; 

        #endregion
        
        #region Init

        public DefaultObjectPool(ObjectPoolConfig<T> config) : this("DefaultObjectPool - " + typeof (T).Name, config)
        {

        }

        public DefaultObjectPool(string name, ObjectPoolConfig<T> config) : this(name, 1, config)
        {
        }

        public DefaultObjectPool(string name, int instanceNum, ObjectPoolConfig<T> config)
            : base(name, instanceNum)
        {
            _config = config;
            _poolImpl = new ConcurrentBag<T>();
            _availabilityGate = new SemaphoreSlim(_config.Capacity, _config.Capacity);
            if (_config.EqualityComparer != null) _itemsCreatedByPool = new HashSet<T>(_config.EqualityComparer);
            else _itemsCreatedByPool = new HashSet<T>();

        }

        #endregion

        #region IObjectPool

        public T BorrowObject(int waitTimeInMilliseconds = Timeout.Infinite)
        {
            if (this.Status != ActivationStatus.ACTIVE) throw new InvalidOperationException("Please start the pool first.");
            T retVal = default(T);
            
            //Wait until allowed to retrieve object from pool
            if (_availabilityGate.Wait(waitTimeInMilliseconds))
            {

                //When you get something out of the pool...
                while (_poolImpl.TryTake(out retVal))
                {
                    //If you have to validate before borrowing and it is not valid.
                    if (_config.ValidateBeforeBorrow && !retVal.IsValidToUse)
                    {
                        Logger.Debug(this, "Retiring an object retrieved from the pool because it is not valid to use anymore.");
                        _config.PoolableObjectFactory.Retire(retVal);
                        retVal = default(T);
                        continue;
                    }
                    return retVal;
                }

                //No more objects readily available in the pool, so create one
                //We're still within limits because we've entered the semaphore.
                Logger.Debug(this, "Creating a new instance of the Poolable Object.");
                retVal = _config.PoolableObjectFactory.Create();
                
                // A newly created object would be expected to be valid.
                // If it is not, then throw an exception, because there is no way 
                // of recovering from the intrinsic problem in the creator and the expectation
                // of a borrowed object being valid.
                if (_config.ValidateBeforeBorrow && !retVal.IsValidToUse)
                {
                    throw new Exception(
                        "Poolable object factory is invalid because the objects it creates are invalid at the very outset.");
                }

                _itemsCreatedByPool.Add(retVal);

            }
            return retVal;
        }

        public void ReturnObject(T poolableObject)
        {
            if (this.Status != ActivationStatus.ACTIVE) throw new InvalidOperationException("Please start the pool first.");
            if (!_itemsCreatedByPool.Contains(poolableObject)) throw new InvalidOperationException("Cannot return an item that wasn't originally borrowed from the pool.");          
            try
            {
                _availabilityGate.Release();
                _poolImpl.Add(poolableObject);
            }
            catch (SemaphoreFullException)
            {
                Logger.Warn(this, "Pool is already full.  Hence retiring the object that was returned.");
                _config.PoolableObjectFactory.Retire(poolableObject);
            }

        }

        public bool UsingObjectFromPool(Action<T> doSomething, int waitTimeInMilliseconds = Timeout.Infinite)
        {
            if (this.Status != ActivationStatus.ACTIVE) throw new InvalidOperationException("Please start the pool first.");
            T borrowedItem = BorrowObject(waitTimeInMilliseconds);
            if (!TypeUtils.IsDefault(borrowedItem))
            {
                try
                {
                    Logger.Debug(this, "Executing action using object retrieved from the pool.");
                    doSomething(borrowedItem);
                    Logger.Debug(this, "Executed action using object retrieved from the pool.");
                    return true;
                }
                finally
                {
                    ReturnObject(borrowedItem);
                }
            }
            return false;
        }

        public void UsingObjectFromPool(IEnumerable<Action<T>> thingsToDo, 
            int waitTimeInMilliseconds = Timeout.Infinite,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.Status != ActivationStatus.ACTIVE) throw new InvalidOperationException("Please start the pool first.");
            T borrowedItem = BorrowObject(waitTimeInMilliseconds);
            if (!TypeUtils.IsDefault(borrowedItem))
            {
                try
                {
                    foreach (var doSomething in thingsToDo)
                    {
                        Logger.Debug(this, "Executing action using object retrieved from the pool.");
                        doSomething(borrowedItem);
                        Logger.Debug(this, "Executed action using object retrieved from the pool.");
                        if (!TypeUtils.IsDefault(cancellationToken) && cancellationToken.IsCancellationRequested)
                        {
                            Logger.Debug(this, "Discontinuing execution because cancellation has been requested.");
                            break;
                        }
                    }
                }
                finally
                {
                    ReturnObject(borrowedItem);
                }
            }
        }



        public int Capacity
        {
            get { return _config.Capacity; }
        }

        public int Available
        {
            get { return _availabilityGate.CurrentCount; }
        }

        #endregion

        #region ControlledComponent

        protected override void StartCore()
        {
            //Load up the pool...
            for (int i = 0; i < _config.MaximumObjectsActiveOnStartup; i++)
            {
                var t = _config.PoolableObjectFactory.Create();
                if (!TypeUtils.IsDefault(t))
                {
                    _poolImpl.Add(t);
                    _itemsCreatedByPool.Add(t);
                }
            }
            base.StartCore();
        }

        protected override void ShutdownCore()
        {
            T emptyingOut;
            while (_poolImpl.TryTake(out emptyingOut))
            {
                _config.PoolableObjectFactory.Retire(emptyingOut);
            }
            _itemsCreatedByPool.Clear();
        }

        #endregion
    }
}
