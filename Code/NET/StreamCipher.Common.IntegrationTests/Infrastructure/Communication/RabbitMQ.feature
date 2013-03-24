Feature: Communication using RabbitMQ
	In order to build n-tier applications
	As an application developer
	I want processes to be able to communicate using RabbitMQ


@communication
Scenario: One-way communication
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen

@communication 
Scenario: Non-blocking request-response communication

@communication
Scenario: Blocking two-way communication
	
