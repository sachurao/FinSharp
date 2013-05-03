using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StreamCipher.Common.Pooling;
using StreamCipher.Common.Utilities.ObjectType;

namespace StreamCipher.Common.IntegrationTests.Pooling
{
    class DummyPoolableObjectFactory : IPoolableObjectFactory<DummyPoolableObject>
    {
        private int _totalCreated;

        public DummyPoolableObject Create()
        {
            Interlocked.Increment(ref _totalCreated);
            return new DummyPoolableObject();
        }

        public void Retire(DummyPoolableObject item)
        {
            item.Retire();
        }

        public int TotalCreated
        {
            get { return Thread.VolatileRead(ref _totalCreated); }
        }

    }
}
