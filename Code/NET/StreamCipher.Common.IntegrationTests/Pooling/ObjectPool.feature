Feature: Pooling of objects that represent expensive resources
	In order to build n-tier applications
	As a application developer
	I want to be able to pool expensive objects in the application

@objectpool
Scenario: Borrowing an object when the pool is not empty
	Given I have created a pool with a capacity of 10 and 3 available on startup 
	And I have activated the pool
	When I borrow an object
	Then I get an object
	And The total number of items that can still be borrowed from the pool is 9
	And The total objects created by the factory equals 3

@objectpool
Scenario: Borrowing an object when the pool is empty
	Given I have created a pool with a capacity of 10 and 0 available on startup 
	And I have activated the pool
	When I borrow an object
	Then I get an object
	And The total number of items that can still be borrowed from the pool is 9
	And The total objects created by the factory equals 1


