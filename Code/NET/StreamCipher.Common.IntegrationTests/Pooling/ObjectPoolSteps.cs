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
        private DummyObjectPool _objectPool;
        private IPoolableObject _borrowedObject = null;
        private DummyPoolableObject _externallyCreatedObject;

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

            _objectPool = new DummyObjectPool(config);
            
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

        [When(@"(.*) objects in the pool have become invalid")]
        public void WhenObjectsInThePoolHaveBecomeInvalid(int p0)
        {
            _objectPool.Invalidate(p0);
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

        [Then(@"The total items readily available equals (.*)")]
        public void ThenTheTotalItemsReadilyAvailableEquals(int p0)
        {
            Assert.AreEqual(p0, _objectPool.TotalReadilyAvailable);
        }


        [When(@"I return an object")]
        public void WhenIReturnAnObject()
        {
            _externallyCreatedObject= new DummyPoolableObject(true);
            _objectPool.ReturnObject(_externallyCreatedObject);
        }

        [Then(@"The object is retired")]
        public void ThenTheObjectIsRetired()
        {
            Assert.IsTrue(_externallyCreatedObject.IsRetired);
        }

    }
}
