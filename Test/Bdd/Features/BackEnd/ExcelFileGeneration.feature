@sharedsteps=4013 @owner=jagudelos @backend @database @testplan=14709 @testsuite=14733
Feature: ExcelFileGeneration
In order to perform the Ownership calculation
As a TRUE system
I need to generate an Excel file with the operational and configuration information

Background: Login
	Given I am logged in as "admin"

@testcase=16478 @ignore
Scenario Outline: Mandatory Fields Validation
	Given I want to calculate the ownership of inventories and movements of a segment
	When I receive the request to generate the Excel file with the data and configurations without "<Field>"
	Then the result should fail with message "<Message>"

	Examples:
		| Field     | Message                                |
		| Segment   | OWNERSHIP_SEGMENT_REQUIREDVALIDATION   |
		| StartDate | OWNERSHIP_STARTDATE_REQUIREDVALIDATION |
		| EndDate   | OWNERSHIP_ENDDATE_REQUIREDVALIDATION   |
		| UserName  | OWNERSHIP_USERNAME_REQUIREDVALIDATION  |

@testcase=16479 @bvt @manual
Scenario: Excel File Generation
	Given I want to calculate the ownership of inventories and movements of a segment
	When I receive the request to generate the Excel file with the data and configurations
	Then ticket number should be generated
	And segment information should be saved
	And node information should be saved
	And excel file should be generated with the information required to calculate the ownership
	And excel file should be copied to a path where the user can obtain the file

@testcase=16480 @manual
Scenario: Validate Excel File Name and Structure
	Given I want to calculate the ownership of inventories and movements of a segment
	When I receive the request to generate the Excel file with the data and configurations
	Then ticket number should be generated
	And file name should be validated
	And file structure should be validated