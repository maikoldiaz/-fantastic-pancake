@sharedsteps=33843 @owner=jagudelos @ui @testsuite=31107 @testplan=31102
Feature: RetryExceptionsToRegisterPendingTransactions
As TRUE System Administrator,
I need to retry exceptions
to register pending transactions

Background: Login
	Given I am logged in as "admin"

@testcase=33844
Scenario: Validate non retryable message
	Given I have technical exceptions in grid
	When I navigate to "Exceptions" page
	Then I see retry option is disabled

@testcase=33845
Scenario: Retry multiple records with technical exceptions
	Given I have technical exceptions in grid
	When I navigate to "Exceptions" page
	And I select multiple grid records
	And I click on "retry" "button"
	Then I see "it is not possible to retry technical exceptions" message

@testcase=33846
Scenario: Retry multiple messages successfully
	Given I have exceptions in grid
	When I navigate to "Exceptions" page
	And I select multiple grid records
	And I click on "retry" "button"
	Then I should be able to retry the registration of selected messages
	And I should not see selected messages in exceptions grid

@testcase=33847
Scenario: Retry single message successfully using grid retry option
	Given I have exceptions in grid
	When I navigate to "Exceptions" page
	And I click on "retry" option
	Then I should be able to retry the registration of selected messages
	And I should not see selected messages in exceptions grid

@testcase=33848
Scenario: Validate errors after retrying multiple messages
	Given I have exceptions in grid
	When I navigate to "Exceptions" page
	And I select multiple grid records
	And I click on "retry" option
	Then I should be able to retry the registration of selected messages
	And I should not see selected messages in exceptions grid
	And I have errors after message processing
	Then I see message in the exception grid

@testcase=33849
Scenario: Validate errors after retrying single message
	Given I have exceptions in grid
	When I navigate to "Exceptions" page
	And I click on "retry" option
	Then I should be able to retry the registration of selected messages
	And I should not see selected messages in exceptions grid
	And I have errors after message processing
	Then I see message in the exception grid

@testcase=33850
Scenario: Validate message detail with technical exception
	Given I have technical exceptions in grid
	When I navigate to "Exceptions" page
	Then I see retry option is disabled

@testcase=33851
Scenario: Validate message detail with business errors
	Given I have technical exceptions in grid
	When I navigate to "Exceptions" page
	Then I see retry option is enabled

@testcase=33852
Scenario: Retry a message from details page
	Given I have exceptions in grid
	When I navigate to "Exceptions" page
	And I navigate to details page of a message
	And I click on "retry" button
	Then I should navigate back to exception grid page
	And I should not see the selected message record in grid
	And I have errors after message processing
	Then I see message in the exception grid