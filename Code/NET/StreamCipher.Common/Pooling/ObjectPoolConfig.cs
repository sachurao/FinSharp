using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamCipher.Common.Pooling
{
    public class ObjectPoolConfig<T> where T:IPoolableObject
    {
        #region Member Variables

        #endregion

        #region Init

        private ObjectPoolConfig()
        {
        }

        public class Builder
        {
            private bool _validateBeforeBorrow = true;
            private int _capacity = Int32.MaxValue;
            private int _maximumObjectsActiveOnStartup = 0; //completely lazy by default
            private IPoolableObjectFactory<T> _factory;
            private IEqualityComparer<T> _equalityComparer; 

            public bool SetValidateBeforeBorrow
            {
                set { _validateBeforeBorrow = value; }
            }

            public int SetCapacity
            {
                set { _capacity = value; }
            }

            public int SetMaximumObjectsActiveOnStartup
            {
                set { _maximumObjectsActiveOnStartup = value; }
            }

            public IPoolableObjectFactory<T> Factory
            {
                set { _factory = value; }
            }

            public IEqualityComparer<T> EqualityComparer
            {
                set { _equalityComparer = value; }
            }
            
            public ObjectPoolConfig<T> Build()
            {
                if (_factory == null) throw new ArgumentNullException("Factory", "Need a valid factory for the pool to be able to create objects");
                if (_capacity <= 0) throw new ArgumentOutOfRangeException("Capacity has to be greater than zero.", "Capacity");
                if (_maximumObjectsActiveOnStartup > _capacity) throw new ArgumentOutOfRangeException("Max objects available on startup cannot be more than pool capacity");

                var retVal = new ObjectPoolConfig<T>()
                    {
                        PoolableObjectFactory = _factory,
                        ValidateBeforeBorrow = _validateBeforeBorrow,
                        Capacity = _capacity,
                        MaximumObjectsActiveOnStartup = _maximumObjectsActiveOnStartup,
                        EqualityComparer = _equalityComparer
                    };
                
                return retVal;
            }

        }

        #endregion

        #region Public Properties

        public bool ValidateBeforeBorrow { get; private set; }

        public int Capacity { get; private set; }

        public int MaximumObjectsActiveOnStartup { get; private set; }

        public IPoolableObjectFactory<T> PoolableObjectFactory { get; private set; }

        public IEqualityComparer<T> EqualityComparer { get; private set; } 

        #endregion

        

    }
}
