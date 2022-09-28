@sharedsteps=7539 @owner=jagudelos @ui @testsuite=8495 @testplan=8481
Feature: UnidentifiedLossesForOperationalBalance
In order to perform operation cutoff of volumetric information
As an application administrator
I want to perform unidentified losses for operational Cutoff summary

Background: Login
	Given I am authenticated as "admin"

@testcase=9904 @ui @manual
Scenario: Validate Unidentified Losses calculation for movements
	Given I have verified pending "Movements" in the system
	And I have verified pending "Movements"in the system
	Then calculate the desbalance for the segment related to the node
	And verify the unidentified loss is calculated successfully