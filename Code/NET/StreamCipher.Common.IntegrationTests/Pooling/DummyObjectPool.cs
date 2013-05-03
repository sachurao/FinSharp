using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamCipher.Common.Pooling;

namespace StreamCipher.Common.IntegrationTests.Pooling
{
    class DummyObjectPool : DefaultObjectPool<DummyPoolableObject>
    {
        public DummyObjectPool(ObjectPoolConfig<DummyPoolableObject> config) : base(config)
        {
        }

        public void Invalidate(int totalItems)
        {
            int i = 0;
            var enumerator = _poolImpl.GetEnumerator();
            while (i < totalItems && enumerator.MoveNext())
            {
                enumerator.Current.Invalidate();
            }
        }

        public Int32 TotalReadilyAvailable { get { return _poolImpl.Count; } }
    }
}
