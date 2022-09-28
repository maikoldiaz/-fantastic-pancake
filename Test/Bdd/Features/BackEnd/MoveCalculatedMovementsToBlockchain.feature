@owner=jagudelos @backend @testplan=14709 @testsuite=14720
Feature: MoveCalculatedMovementsToBlockchain
	As a User I need to check if
	calculated Movements are
	registered in Blockchain

@testcase=16488
Scenario: Verify Calculated Movements Data are registered in Blockchain
	When I perform calculation for OperationalBalance in the system
	Then Calculated Movement details should be registered in Blockchain