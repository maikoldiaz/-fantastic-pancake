@sharedsteps=4013 @owner=jagudelos @testplan=14709 @testsuite=14714
Feature: ProcessExcelFile
In order to handle the calculations
As a TRUE system
I want to process Excel file with the results of the ownership calculations

Background: Login
	Given I am logged in as "admin"

@testcase=16496 @manual
Scenario: File names with invalid format
	Given I have a new file exists in the ownership response file path
	When the file name does not meet the naming standard defined for failed or successful response files​
	Then the system should not process the file

@testcase=16497 @manual
Scenario: Invalid structure in the error file
	Given I have a new error file in the ownership response file path
	When the file does not meet the defined structure
	Then the error must be stored in the general error log of the application

@testcase=16498 @manual
Scenario: Invalid structure in the successful response file
	Given I have a new file with successful response in the ownership response file path
	When the file does not meet the defined structure
	Then the error must be stored in the general error log of the application

@testcase=16499 @manual
Scenario: Process error response file
	When I have a new error file in the ownership response file path
	Then the errors in the "Inventory" sheet should be recorded one by one in a table that allows to identify errors of a ticket
	And the errors in the "Movement" sheet should be recorded one by one in a table that allows to identify errors of a ticket
	And status of the ticket should be updated to "Completed with error" in segments ownership calculation table
	And status of all the nodes related to the ticket should be updated to "Completed with error" in nodes ownership calculation table
	And nodes in the submitted state which correspond to the received ticket number should be deleted in nodes ownership calculation table

@testcase=16500 @bvt
Scenario: Process file with ownership calculation results
	And I have ownership calculation data generated in the system
	When I have a new file with successful response in the ownership response file path
	Then the records in the "Inventory" sheet should be saved one by one in the inventory ownership table
	And the records in the "Movement" sheet should be saved one by one in the movement ownership table
	And status of the ticket should be updated to "Completed" in segments ownership calculation table
	And status of all the nodes related to the ticket should be updated to "Completed" in nodes ownership calculation table