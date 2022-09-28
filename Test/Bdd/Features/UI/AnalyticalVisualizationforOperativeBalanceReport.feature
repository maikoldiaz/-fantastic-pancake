@owner=jagudelos @ui @testsuite=11332 @testplan=11317
Feature: AnalyticalVisualizationforOperativeBalanceReport
In order to visualize operative balance
As a user
I want to view the report of the operative balance

@testcase=12011 @ui @manual
Scenario: Validate the analytical visualization of operative balance report
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I select "Node" from "Node"
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then validate "BalanceOperativeReport" is displayed
	And validate "UnidentifiedLosses" chart is displayed
	And validate net volume of the identified losses by product is displayed in "UnidentifiedLosses" chart
	Then validate "Movement" chart is displayed
	And validate percentage of each volumetric balance variable is displayed in "Movement" chart
	Then validate "FinalInventory" chart is displayed
	And validate composition for each product of the final inventories is displayed in "FinalInventory" chart