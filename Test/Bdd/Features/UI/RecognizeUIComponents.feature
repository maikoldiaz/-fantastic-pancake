@owner=jagudelos @ui @testsuite=14722 @testplan=14709
Feature: RecognizeUIComponents
In order to identify and recognize components
As a user
I need to quickly identify and recognize the components

@testcase=16577 @ui @manual
Scenario Outline: Validate Icon Color in all module
	When I navigate to "<Module>" page
	Then validate all the button color as "##002060"

	Examples:
		| Module                   |
		| node-connection          |
		| Categories               |
		| Node                     |
		| CategoryElements         |
		| ConfigureAttributesNodes |
		| Grouping Categories      |
		| UploadExcel              |
		| OperationalCutoff        |

@testcase=16578 @ui @manual
Scenario Outline: Validate Add, Search, Edit, View button in all module
	When I navigate to "<Module>" page
	Then validate "Add" button as "plus-square"
	And validate "Search" button as "search"
	And validate "Edit" button as "edit"
	And validate "View" button as "eye"

	Examples:
		| Module                   |
		| node-connection          |
		| Categories               |
		| Node                     |
		| CategoryElements         |
		| ConfigureAttributesNodes |
		| Grouping Categories      |
		| UploadExcel              |
		| OperationalCutoff        |

@testcase=16579 @ui @manual
Scenario: Upload Valid Excel to validate spinner and check-circle
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	When I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "<FileType>" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the Upload Id for file tracking
	And I should see "spinner" until the process complete
	And I should see "check-circle" when transaction "passed"

@testcase=16580 @ui @manual
Scenario: Upload InValid Excel to validate spinner and times-circle
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	When I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "<FileType>" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the Upload Id for file tracking
	And I should see "spinner" until the process complete
	And I should see "times-circle" when transaction "failed"