@owner=jagudelos @ui @testsuite=11330 @testplan=11317
Feature: KPIofOperativeBalanceReport
In order to visualize operative balance
As a user
I want to view the report of the operative balance

@testcase=12060 @ui @manual
Scenario: Validate selected NodeName concatenate to Category and Element
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I select "NodeName" from "Node"
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then validate "BalanceOperativeReport" is displayed
	And validate selected "NodeName" concatenate to "Category" and "Element"
	And validate selected "StartDate", "EndDate" is displayed

@testcase=12061 @ui @manual
Scenario: Validate NodeName not concatenate to Category and Element
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I select "Todos" from "Node"
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then validate "BalanceOperativeReport" is displayed
	And validate selected "NodeName" not concatenate to "Category" and "Element"
	And validate selected "StartDate", "EndDate" is displayed

@testcase=12062 @ui @manual
Scenario: Validate KPI indicators value are calculated and displayed
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I select "Node" from "Node"
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then validate "BalanceOperativeReport" is displayed
	And validate "IdentifiedLoss" calculated values are displayed
	And validate "Tolerance" calculated values are displayed
	And validate "Interface" calculated values are displayed
	And validate "UnidentifiedLoss" calculated values are displayed
	Then validate "RedDelta" for a decrease
	And validate "GreenDelta" for a increase

@testcase=12063 @ui @manual
Scenario: Validate blank is displayed in all indicators if no data for the last value
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I select "Node" from "Node"
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then validate "BalanceOperativeReport" is displayed
	And validate "Blank" is displayed in "IdentifiedLoss"
	And validate "Blank" is displayed in "Tolerance"
	And validate "Blank" is displayed in "Interface"
	And validate "Blank" is displayed in "UnidentifiedLoss"