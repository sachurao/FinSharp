Feature: Pooling of objects that represent expensive resources
	In order to build n-tier applications
	As a application developer
	I want to be able to pool expensive objects in the application

@objectpool
Scenario: Borrowing an object when the pool is not activated
	Given I have created a pool with a capacity of 10 and 1 available on startup
	Then Trying to borrow an object throws an InvalidOperationException

@objectpool
Scenario: : Returning an object when the pool after the pool is deactivated
	Given I have created a pool with a capacity of 10 and 1 available on startup
	And I have activated the pool
	When I borrow an object
	And The pool is deactivated
	Then Trying to return an object throws an InvalidOperationException

@objectpool
Scenario: Borrowing an object when the pool is not empty
	Given I have created a pool with a capacity of 10 and 3 available on startup 
	And I have activated the pool
	When I borrow an object
	Then I get an object
	And The total number of items that can still be borrowed from the pool is 9
	And The total objects created by the factory equals 3
	And The total items readily available equals 2

@objectpool
Scenario: Borrowing an object when the pool is empty
	Given I have created a pool with a capacity of 10 and 0 available on startup 
	And I have activated the pool
	When I borrow an object
	Then I get an object
	And The total number of items that can still be borrowed from the pool is 9
	And The total objects created by the factory equals 1
	And The total items readily available equals 0

@objectpool
Scenario:  Returning an object created by the pool
	Given I have created a pool with a capacity of 10 and 10 available on startup 
	And I have activated the pool
	When I borrow an object
	And I return the borrowed object
	Then The total number of items that can still be borrowed from the pool is 10
	And The total objects created by the factory equals 10
	And The total items readily available equals 10

@objectpool
Scenario:  Returning an object not created by the pool
	Given I have created a pool with a capacity of 10 and 9 available on startup 
	And I have activated the pool
	When I create a poolable item
	Then Trying to return an object throws an InvalidOperationException

@objectpool
Scenario: Some of the objects in the pool become invalid and have to validate before borrow
	Given I have created a pool with a capacity of 10 and 3 available on startup
	And I have activated the pool
	When 2 objects in the pool have become invalid
	And I borrow an object
	Then The total number of items that can still be borrowed from the pool is 9

@objectpool
Scenario: Some of the objects in the pool become invalid but do not have to validate before borrow
	Given I have created a pool with a capacity of 10 and 3 available on startup
	And I have activated the pool
	When 2 objects in the pool have become invalid
	And I borrow an object
	Then The total number of items that can still be borrowed from the pool is 9

