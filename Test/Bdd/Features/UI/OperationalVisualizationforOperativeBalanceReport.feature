@owner=jagudelos @ui @testsuite=11331 @testplan=11317
Feature: OperationalVisualizationforOperativeBalanceReport
In order to visualize operative balance
As a user
I want to view the report of the operative balance

@testcase=12077 @ui @manual
Scenario: Validate the operational visualization table
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I select "Node" from "Node"
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then validate "BalanceOperativeReport" is displayed
	And validate "OperationalVisualization" for Products is displayed
	And validate "Products" are displayed with calculated values
	When I right click on any product
	Then validate table visualization of that movements details
	When I right click on any product
	Then validate table visualization of that movements details
	And validate all the values in movements table
	When I right click on any movement
	Then validate table visualization of that Quality Attributes details
	And validate all the values in Quality Attributes table