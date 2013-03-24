using System;
using NUnit.Framework;
using StreamCipher.Common.Pooling;
using TechTalk.SpecFlow;

namespace StreamCipher.Common.IntegrationTests.Pooling
{
    [Binding]
    public class ObjectPoolSteps
    {
        private DummyPoolableObjectFactory _factory;
        private IObjectPool<DummyPoolableObject> _objectPool;
        private IPoolableObject _borrowedObject = null;

        public ObjectPoolSteps()
        {
            IntegrationTestHelper.SetUpFixture();
        }

        [Given(@"I have created a pool with a capacity of (.*) and (.*) available on startup")]
        public void GivenIHaveCreatedAPoolWithACapacityOfAndAvailableOnStartup(int p0, int p1)
        {
            _factory = new DummyPoolableObjectFactory();
            var config = new ObjectPoolConfig<DummyPoolableObject>.Builder
            {
                Factory = _factory,
                SetCapacity = p0,
                SetMaximumObjectsActiveOnStartup = p1,
                SetValidateBeforeBorrow = true
            }
            .Build();

            _objectPool = new DefaultObjectPool<DummyPoolableObject>(config);
            
        }
        
        [Given(@"I have activated the pool")]
        public void GivenIHaveActivatedThePool()
        {
            _objectPool.Start();
        }
        
        [When(@"I borrow an object")]
        public void WhenIBorrowAnObject()
        {
            _borrowedObject = _objectPool.BorrowObject();
        }

        
        [Then(@"I get an object")]
        public void ThenIGetAnObject()
        {
            Assert.IsNotNull(_borrowedObject);
        }

        [Then(@"The total number of items that can still be borrowed from the pool is (.*)")]
        public void ThenTheTotalNumberOfItemsThatCanStillBeBorrowedFromThePoolIs(int p0)
        {
            Assert.AreEqual(p0, _objectPool.Available);
        }

        [Then(@"The total objects created by the factory equals (.*)")]
        public void ThenTheTotalObjectsCreatedByTheFactoryEquals(int p0)
        {
            Assert.AreEqual(p0, _factory.TotalCreated);
        }

        public void AddingRandomMethod()
        {
            //This does nothing...
        }

    }
}
