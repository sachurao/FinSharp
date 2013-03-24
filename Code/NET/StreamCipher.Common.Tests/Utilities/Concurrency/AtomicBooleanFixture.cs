using System;
using FluentAssertions;
using NUnit.Framework;
using StreamCipher.Common.Utilities.Concurrency;
using System.Threading;
using System.Threading.Tasks;

namespace StreamCipher.Common.Tests.Utilities.Concurrency
{
    [TestFixture]
    public class AtomicBooleanFixture
    {
        private AtomicBoolean _atomicBoolean;

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void CompareAndSet_ManyConcurrentThreads_OnlyOneSucceeds_Test()
        {
            _atomicBoolean = new AtomicBoolean(false);
            int totalSucceedingThreads = 0;

            Parallel.For(1, 11, (state) =>
            {
                System.Console.WriteLine(String.Format("Executing CompareAndSet on thread {0}", 
                    Thread.CurrentThread.ManagedThreadId));
                if (_atomicBoolean.CompareAndSet(false, true))
                {
                    System.Console.WriteLine(String.Format("Thread {0} was successful in calling CompareAndSet.",
                        Thread.CurrentThread.ManagedThreadId));
                    Interlocked.Increment(ref totalSucceedingThreads);
                }
            });

            //Assert
            _atomicBoolean.Value.Should().BeTrue();
            totalSucceedingThreads.Should().Be(1);
        }

        [Test]
        public void SetValue_ManyConcurrentThreads_OnlyOneSucceeds_Test()
        {
            _atomicBoolean = new AtomicBoolean(false);
            int totalSucceedingThreads = 0;

            Parallel.For(1, 11, (state) =>
            {
                System.Console.WriteLine(String.Format("Executing SetValue on thread {0}",
                    Thread.CurrentThread.ManagedThreadId));
                if (!_atomicBoolean.SetValue(true))
                {
                    System.Console.WriteLine(String.Format("Thread {0} was successful in calling SetValue.",
                        Thread.CurrentThread.ManagedThreadId));
                    Interlocked.Increment(ref totalSucceedingThreads);
                }
            });

            //Assert
            _atomicBoolean.Value.Should().BeTrue();
            totalSucceedingThreads.Should().Be(1);
        }
    }
}
