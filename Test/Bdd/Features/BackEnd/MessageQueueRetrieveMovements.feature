@owner=jagudelos @testplan=4938 @testsuite=5402 @backend @servicebus
Feature: MessageQueueRetrieveMovements
	In order to receive the movements from SINOPER
	As a TRUE system, I want to connect to an Enterprise MQ

@backend @bvt @testcase=5878
Scenario Outline: Validate data mapping for each Movement Type
	Given the system is processing a valid message from MQ
	When it is transforming the original message to the canonical
	Then the system must apply the defined mapping per each "<MovementType>"

	Examples:
		| MovementType |
		| Movements    |

@backend @testcase=5879
Scenario: Verify retry strategy in case of Transient Failure
	Given the system is processing a message from MQ
	When process fails due to "TransientFailure"
	Then the system must log the failure
	And implement a retry strategy

@testcase=5880 @backend
Scenario: Verify retry strategy when the system reaches maximum number of defined retries
	Given the system is retrying a message processing
	When it reaches the "MaximumNumberOfRetries"
	Then the system must log the failure
	And it must release the message to be processed later

@testcase=5881 @backend
Scenario: Verify message processing in case of Inconsistent Message
	Given the system is processing a message from MQ
	When process fails due to "InconsistentMessage"
	Then the system must log the failure
	And it must release the message to be processed later

@testcase=5882 @backend
Scenario: Verify message processing when Similar Failures occurs
	Given the system is processing a message from MQ
	When process fails due to "TransientFailure"
	And the system detects many "SimilarFailures" recently
	Then the system must log the failure
	And implement a circuit breaker strategy