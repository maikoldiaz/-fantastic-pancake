@owner=jagudelos @ui @testplan=6671 @testsuite=7832 @after=DeleteTestDataAsync()
Feature: ProcessExcelFilesToRecordMovementsAndInventories
In order to register a Movement or Inventory in the system
As a transport segment user
I want to process the Excel files

@testcase=7604 @version=2 @ignore
Scenario Outline: Register a new Movement or Inventory by uploading Excel
	Given I want to register a "Homologation" in the system
	When I am logged in as "admin"
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	When I select "<ActionType>" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "ValidExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then it should be registered in the system

	Examples:
		| ActionType |
		| Insert     |
		| Update     |
		| Delete     |