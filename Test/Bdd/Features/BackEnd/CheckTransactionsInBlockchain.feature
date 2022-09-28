@owner=jagudelos @backend @blockchain @testplan=8481 @testsuite=9744
Feature: CheckTransactionsInBlockchain
As a User I need to check if
Movement and Inventory data
is registered in Blockchain

@testcase=12007
Scenario: Verify Movement Data in Blockchain
	Given I have movement data
	When I submit movement data
Then Calculated Movement details should be registered in Blockchain

@testcase=12008
Scenario: Verify Inventory Data in Blockchain
	Given I have inventory data
	When I submit inventory data
	Then Inventory details should be registered in Blockchain