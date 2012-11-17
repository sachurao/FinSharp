Feature: RabbitMQ
	In order to be able to build n-tier apps
	As a developer
	I want to be able to communicate between processes

@Integration
Scenario: Send an async command
	Given I have connected as a sender to the message broker
	And I have connected as a receiver to the message broker
	When I send a message
	Then I should receive the message

@Integration
Scenario: Communicate using request-response pattern
	Given I have connected a sender and a receiver to the message broker
	When I send a request
	Then I should receive the request 
	And On responding to the original request
	Then Original sender should receive the response



