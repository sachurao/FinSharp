using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StreamCipher.Common.Interfaces.ActivityControl;

namespace StreamCipher.Common.Pooling
{
    public interface IObjectPool<T>:IControlledComponent where T:class, IPoolableObject
    {
        T BorrowObject(int waitTimeInMilliseconds = Timeout.Infinite);
        void ReturnObject(T poolableObject);
        bool UsingObjectFromPool(Action<T> doSomething, int waitTimeInMilliseconds = Timeout.Infinite);

        void UsingObjectFromPool(IEnumerable<Action<T>> thingsToDo, int waitTimeInMilliseconds = Timeout.Infinite,
                                 CancellationToken cancellationToken = default(CancellationToken));

        Int32 Capacity { get; }
        Int32 Available { get; }

    }
}
