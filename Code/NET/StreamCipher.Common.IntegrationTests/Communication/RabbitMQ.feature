Feature: Communication via RabbitMQ
	In order to build n-tier applications
	As an application developer
	I want to be able to make two processes communicate via RabbitMQ messaging

@communication
Scenario: One-way communication
	Given The test process connects to Rabbit MQ as Peer One
	And The test process connects to Rabbit MQ as Peer Two
	And Peer Two subscribes on a test topic
	When Peer One sends a message on the test topic
	Then Peer Two receives the message

@communication
Scenario: Two-way communication
	Given The test process connects to Rabbit MQ as Peer One
	And The test process connects to Rabbit MQ as Peer Two
	And Peer Two subscribes on a test topic
	When Peer One sends a message on the test topic expecting a response
	And Peer Two receives the message
	Then Peer Two sends a response
	And Peer One receives the response with the same correlation id on the response topic
	
@communication
Scenario: Unsubscribing on a topic
	Given The test process connects to Rabbit MQ as Peer One
	And The test process connects to Rabbit MQ as Peer Two
	And Peer Two subscribes on a test topic
	When Peer One sends a message on the test topic
	And Peer Two receives the message
	And Peer Two unsubscribes to the test topic
	And Peer One sends a message on the test topic
	Then Peer Two does not receive the message


