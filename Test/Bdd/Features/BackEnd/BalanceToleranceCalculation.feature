@owner=jagudelos @backend @testplan=8481 @testsuite=8484
Feature: BalanceToleranceCalculation
	As a Client Application
	I need to Calculate Balance Tolerance
	to complete the operational balance

@testcase=9885 @manual
Scenario: Verify Balance Tolerance Calculation with the given uncertainity values
	Given I want to calculate the "BalanceTolerance" in the system
	When I receive the input data
	Then I should be able to calculate the Balance Tolerance
	And I should be able to calculate the tolerance value for each product of the balance
	And I should be able to register for each product a movement with its tolerance
	And I should be able to see exception if an exception occurs during the calculation or registration process

@testcase=9886 @manual
Scenario: Verify Balance Tolerance Calculation with the default uncertainity values
	Given I want to calculate the "BalanceTolerance" in the system
	When I receive the input data
	Then I should be able to calculate the Balance Tolerance
	And I should be able to calculate the tolerance value for each product of the balance
	And I should be able to register for each product a movement with its tolerance
	And I should be able to see exception if an exception occurs during the calculation or registration process