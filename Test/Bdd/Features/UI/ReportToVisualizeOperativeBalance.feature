@owner=jagudelos @ui @testsuite=11333 @testplan=11317
Feature: ReportToVisualizeOperativeBalance
In order to visualize operative balance
As a user
I want to view the report of the operative balance

@testcase=12078 @ui
Scenario: Validate operative balance report is displayed for the selected Category
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I select "Node" from "Node"
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then validate "BalanceOperativeReport" is displayed
	And validate selected "Segment", "Node", "StartDate", "EndDate" is displayed

@testcase=12079 @ui
Scenario: Validate mandatory field for category
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "CategorySegment" from "CategoryElement"
	And I select "Node" from "Node"
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then I should see error message "CONTROLS_REQUIREDVALIDATION"

@testcase=12080 @ui
Scenario: Validate mandatory field for element
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "Node" from "Node"
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then I should see error message "CONTROLS_REQUIREDVALIDATION"

@testcase=12081 @ui
Scenario: Validate mandatory field for node
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then I should see error message "CONTROLS_REQUIREDVALIDATION"

@testcase=12082 @ui
Scenario: Validate mandatory field for StartDate
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I select "Node" from "Node"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then I should see error message "CONTROLS_REQUIREDVALIDATION"

@testcase=12083 @ui
Scenario: Validate mandatory field for EndDate
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I select "Node" from "Node"
	And enter "StartDate" into "StartDate" "date"
	And I click on "View" "Report" "button"
	Then I should see error message "CONTROLS_REQUIREDVALIDATION"

@testcase=12084 @ui
Scenario: Validate the error when initial date is greater than the final date
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I select "Node" from "Node"
	And enter "StartDate" into "StartDate" "date" greater than "EndDate"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then I should see error message "DATES_INCONSISTENT"

@testcase=12085 @ui
Scenario: Validate the error when end date is equal to current date
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I select "Node" from "Node"
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date" equal to currentDate
	And I click on "View" "Report" "button"
	Then I should see error message "ENDDATE_BEFORENOWVALIDATION"

@testcase=12086 @ui
Scenario: Validate the error when range of days chosen is greater than 45 days
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I select "Node" from "Node"
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then I should see error message "MAXIMUM_REPORTSDAYS_RANGEVALIDATION"