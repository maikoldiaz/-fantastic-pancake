@owner=jagudelos @ui @testplan=11317 @testsuite=11324 @version=1
Feature: BlockChain Data and Offchain Database
As a User I need to check that
Movement and Inventory Data is
registered in Blockchain and Offchain Database

@testcase=12017
Scenario: Check Movement Excel Data Insertion with valid Movement Id
	Given I have Excel and True system setup for Homologation
	When I upload valid Movement Excel with Operation Type as Insert
	Then the Movement should be registered in Blockchain as well as Offchain Database

@testcase=12018
Scenario: Check Movement Excel Data Insertion with invalid Movement Id
	Given I have Excel and True system setup for Homologation
	When I upload valid Movement Excel having existing Movement ID with Operation Type as Insert
	Then the error should be registered in Pending Transactions Table

@testcase=12019
Scenario: Check Movement Excel Data Update with valid Movement Id
	Given I have Excel and True system setup for Homologation
	When I upload valid Movement Excel  with Operation Type as Update
	Then the existing entry should be negated and new Movement should be registered in Blockchain as well as Offchain Database

@testcase=12020
Scenario: Check Movement  Excel Data Delete with valid Movement Id
	Given I have Excel and True system setup for Homologation
	When I upload valid Movement Excel  with Operation Type as Delete
	Then the existing entry should be negated in Blockchain as well as Offchain Database

@testcase=12021
Scenario: Check Inventory Excel Data Insertion with valid Inventory Id
	Given I have Excel and True system setup for Homologation
	When I upload valid Inventory Excel with Operation Type as Insert
	Then the Inventory should be registered in Blockchain as well as Offchain Database

@testcase=12022
Scenario: Check Inventory Excel Data Insertion with invalid Inventory Id
	Given I have Excel and True system setup for Homologation
	When I upload valid Inventory Excel having existing Inventory ID with Operation Type as Insert
	Then the error should be registered in Pending Transactions Table

@testcase=12023
Scenario: Check Inventory Excel Data Update with valid Inventory Id
	Given I have Excel and True system setup for Homologation
	When I upload valid Inventory Excel  with Operation Type as Update
	Then the existing entry should be negated and new Inventory should be registered in Blockchain as well as Offchain Database

@testcase=12024
Scenario: Check Inventory Excel Data Delete with valid Inventory Id
	Given I have Excel and True system setup for Homologation
	When I upload valid Inventory Excel  with Operation Type as Delete
	Then the existing entry should be negated in Blockchain as well as Offchain Database

@testcase=12025
Scenario: Check Movement Sinoper Data Insertion with valid Movement Id
	Given I have Sinoper and True system setup for Homologation
	When I upload valid Movement Sinoper with Operation Type as Insert
	Then the Movement should be registered in Blockchain as well as Offchain Database

@testcase=12026
Scenario: Check Movement Sinoper Data Insertion with invalid Movement Id
	Given I have Sinoper and True system setup for Homologation
	When I upload valid Movement Sinoper having existing Movement ID with Operation Type as Insert
	Then the error should be registered in Pending Transactions Table

@testcase=12027
Scenario: Check Movement Sinoper Data Update with valid Movement Id
	Given I have Sinoper and True system setup for Homologation
	When I upload valid Movement Sinoper  with Operation Type as Update
	Then the existing entry should be negated and new Movement should be registered in Blockchain as well as Offchain Database

@testcase=12028
Scenario: Check Movement Sinoper Data Delete with valid Movement Id
	Given I have Sinoper and True system setup for Homologation
	When I upload valid Movement Sinoper  with Operation Type as Delete
	Then the existing entry should be negated in Blockchain as well as Offchain Database

@testcase=12029
Scenario: Check Inventory Sinoper Data Insertion with valid Inventory Id
	Given I have Sinoper and True system setup for Homologation
	When I upload valid Inventory Sinoper with Operation Type as Insert
	Then the Inventory should be registered in Blockchain as well as Offchain Database

@testcase=12030
Scenario: Check Inventory Sinoper Data Insertion with invalid Inventory Id
	Given I have Sinoper and True system setup for Homologation
	When I upload valid Inventory Sinoper having existing Inventory ID with Operation Type as Insert
	Then the error should be registered in Pending Transactions Table

@testcase=12031
Scenario: Check Inventory Sinoper Data Update with valid Inventory Id
	Given I have Sinoper and True system setup for Homologation
	When I upload valid Inventory Sinoper  with Operation Type as Update
	Then the existing entry should be negated and new Inventory should be registered in Blockchain as well as Offchain Database

@testcase=12032
Scenario: Check Inventory Sinoper Data Delete with valid Inventory Id
	Given I have Sinoper and True system setup for Homologation
	When I upload valid Inventory Sinoper  with Operation Type as Delete
	Then the existing entry should be negated in Blockchain as well as Offchain Database