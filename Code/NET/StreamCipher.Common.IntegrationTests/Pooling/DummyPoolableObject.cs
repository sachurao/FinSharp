using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StreamCipher.Common.Pooling;
using StreamCipher.Common.Utilities.Concurrency;

namespace StreamCipher.Common.IntegrationTests.Pooling
{
    class DummyPoolableObject:IPoolableObject
    {
        private AtomicBoolean _isValidToUse;
        private AtomicBoolean _isRetired;

        public DummyPoolableObject(bool isValidToUse = true, bool isRetired = false)
        {
            _isValidToUse = new AtomicBoolean(isValidToUse);
            _isRetired = new AtomicBoolean(isRetired);
        }

        public bool IsValidToUse
        {
            get { return _isValidToUse.Value; }
        }

        public bool IsRetired
        {
            get { return _isRetired.Value; }
        }

        public void Retire()
        {
            _isRetired.CompareAndSet(false, true);
        }

    }
}
