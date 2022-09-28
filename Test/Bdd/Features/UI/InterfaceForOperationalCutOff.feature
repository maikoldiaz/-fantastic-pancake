@sharedsteps=7539 @owner=jagudelos @ui @testsuite=8483 @testplan=8481
Feature: InterfaceForOperationalBalance
In order to perform operation cutoff of volumetric information
As an application administrator
I want to perform interface for operational Cutoff summary

Background: Login
	Given I am authenticated as "admin"

@testcase=9897 @ui @manual
Scenario: Validate interface calculation for movements
	Given I have verified pending "Movements" in the system
	And I have verified pending "Movements"in the system
	Then calculate the desbalance for the segment related to the node
	And verify the interface is calculated successfully