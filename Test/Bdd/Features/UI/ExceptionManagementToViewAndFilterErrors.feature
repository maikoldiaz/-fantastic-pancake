@sharedsteps=4013 @owner=jagudelos @ui @testplan=19772 @testsuite=19793
Feature: ExceptionManagementToViewAndFilterErrors
In order to view the errors
As an application administrator
I need an exception management UI to view and filter the errors

Background: Login
Given I am logged in as "admin"

@testcase=21226 @bvt
Scenario: Verify exceptions registered in the last 40 days
Given I am having exceptions in Exceptions page
When I navigate to "Exceptions" page
Then I should see the information of executed "Exceptions" in the grid

@testcase=21227 @manual
Scenario: Verify exceptions information when there are no records in the last 40 days
Given I have no information from the last 40 days
When I navigate to "Exceptions" page
Then I should see error message "Sin registros"

@testcase=21228 @bvt
Scenario: Verify Exception details page
	Given I am having exceptions in Exceptions page
	When I navigate to "Exceptions" page
	#Then I should see the tooltip "Detail" text for "exceptions" "detail" "link"
	When I search for movements related exceptions
	When I click on "pendingTransactionErrors" "detail" "link"
	Then I should see a page with the exception detail
	When the fields do not have a registered value
	Then only the title should be displayed

@testcase=21229
Scenario Outline: Verify Filters functionality
Given I am having exceptions in Exceptions page
When I navigate to "Exceptions" page
And I provide the value for "<Field>" "<ControlType>" filter in "Exceptions" Grid
Then I should see the information that matches the data entered for the "<Field>" in "Exceptions" Grid

Examples:
| Field        | ControlType |
| Error        | textbox     |
| OriginSystem | textbox     |
| Process      | textbox     |
| DateCreated  | date        |
@testcase=21230
Scenario Outline: Verify Sorting functionality
Given I am having exceptions in Exceptions page
When I navigate to "Exceptions" page
And I click on the "<ColumnName>"
Then the results should be sorted based on "<ColumnName>" in "Exceptions" Grid

Examples:
| Columns      |
| Error        |
| OriginSystem |
| Process      |
| DateCreated  |
@testcase=21231 
Scenario: Verify Pagination functionality
Given I am having exceptions in Exceptions page
When I navigate to "Exceptions" page
And I navigate to second page in "Exceptions" Grid
Then the records should be displayed accordingly in "Exceptions" Grid
When I change the elements count per page to 50
Then the records count in "Exceptions" Grid shown per page should also be 50
