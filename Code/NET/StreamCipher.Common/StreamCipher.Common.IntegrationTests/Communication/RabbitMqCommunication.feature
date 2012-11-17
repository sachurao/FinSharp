Feature: RabbitMqCommunication
	In order to be able to build n-tier apps
	As a developer
	I want to be able to communicate between processes

@Integration
Scenario: One-way communication
	Given A server starts up and subscribes to messages on a test topic on Rabbit MQ
	And A client starts up and can send messages on Rabbit MQ
	When Client sends a message on the test topic
	Then Server should receive the message

@Integration
Scenario:  Non-blocking request-response communication
	Given A server starts up and subscribes to messages on a test topic on Rabbit MQ
	And A client starts up and can send messages on Rabbit MQ
	When Client sends a request
	Then Server should receive the request
	And Client should not wait for response
	When Server sends a response
	Then Client should receive a response

@Integration
Scenario:  Blocking request-response communication
	Given A server starts up and subscribes to messages on a test topic on Rabbit MQ
	And A client starts up and can send messages on Rabbit MQ
	When Client sends a request
	Then Server should receive the request
	And Client should wait for response
	When Server sends a response
	Then Client should receive a response

@Integration
Scenario:  Broadcast notification
	Given A server starts up
	And Multiple clients start up
	And All receivers are subscribed to a particular topic
	When Sender sends a message on that particular topic
	Then All receivers should receive that message

  
