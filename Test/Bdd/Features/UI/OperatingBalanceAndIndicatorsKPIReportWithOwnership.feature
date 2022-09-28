@sharedsteps= @owner=jagudelos @ui @testplan=19772 @testsuite=19775
Feature:OperatingBalanceAndIndicatorsKPIReportWithOwnership
In order to visualize the operating balance and Indicators KPI with ownership and traceability of the rules applied
As a Query User
I want to visualize  the operating balance Indicators KPI

Background: Login
Given I am logged in as "consulta"
@testcase=21267
Scenario: Verify the subtitle of the report
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating" "Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
When  I choose a node other than the "Todos/All"
And Click on "View" "Report" "Button"
Then I should see the subtitle as the node name to the category and element as selected in the filter
@testcase=21268
Scenario: Verify the indicators section of the report
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating" "Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And click on withownership ownership checkbox
And Click on "View" "Report" "Button"
When i see the indicators section of the report
Then I should see the report should use as a period for last value calculation the period immediately preceding that of the report
@testcase=21269
Scenario: Verify the indicators section when there is no data
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating" "Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And Check the ownership checkbox
And Click on "View" "Report" "Button"
When there is no data exists for precedent period or the last period
Then I should see the report with empty indicators or Not Applicable tag
@testcase=21270
Scenario: Verify the indicators section when there is data
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating" "Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And Check the ownership checkbox
And Click on "View" "Report" "Button"
When the input parameters are filled, and the ownership calculation process has been executed
Then I should see the operating balance report with the new variables embedded
And I should see the KPI Indicators are Visualized
@testcase=21271
Scenario: Verify the Report sections
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating" "Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And Check the ownership checkbox
And Click on "View" "Report" "Button"
When the input parameters are filled, and the ownership calculation process has been executed
Then I should see the Header Section
And I should see the Indicators Section
@testcase=21272
Scenario:Verify the Header Section of the report
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating" "Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And Check the ownership checkbox
And Click on "View" "Report" "Button"
Then I should see the Header Section
And I should see ECOPETROL standard color for the section
And I should see the ECOPETROL logo on the left
And I should see the report Title on the centre
Then I should see the word "Confidencial"
And I should see the update date in the format dd/mm/yyyy hh: mm: ss am/pm
@testcase=21273
Scenario: Verify the Title of the Report in the Header Section
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating" "Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And Check the ownership checkbox
And Click on "View" "Report" "Button"
When I see the Header Section
Then I should see the title as "BALANCE VOLUMÉTRICO CON PROPIEDAD"
@testcase=21274
Scenario: Verify the SubTitle of the Report in the Header Section
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating" "Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And Check the ownership checkbox
And Click on "View" "Report" "Button"
When I see the Header Section
Then I should see the title as "BALANCE VOLUMÉTRICO CON PROPIEDAD"
And I should see the subtitle as "CATEGORY NAME  ELEMENT NAME NODE NAME" in upper case
And I should see the Start date to End date of that element formatted "dd-mmm-YY"
@testcase=21275
Scenario: Verify The report if the input parameter for the node as Todos
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating" "Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And Check the ownership checkbox
And Click on "View" "Report" "Button"
When I give the node as Todos and view the header section
Then I should see the subtitle of the report as "CATEGORY NAME  ELEMENT NAME"
@testcase=21276
Scenario: Verify the Indicators displayed for the balance report
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating" "Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And Check the ownership checkbox
And Click on "View" "Report" "Button"
When I see the variables of indicators section
Then I should see the "Perdidas identificadas" section and the values
And I should see the "Tolerancias" and its corresponding value
And I should see the "INTERFASES" and its corresponding value
Then I should see the "Perdidas No identificadas" and its corresponding value
@testcase=21277
Scenario: Validate NodeName not concatenate to Category and Element
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating" "Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then validate "BalanceOperativeReport" is displayed
And validate selected "NodeName" not concatenate to "Category" and "Element"
And validate selected "StartDate", "EndDate" is displayed
@testcase=21278
Scenario: Validate KPI indicators value are calculated and displayed
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating" "Balance Report" "link"
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
@testcase=21279 
Scenario: Validate blank is displayed in all indicators if no data for the last value
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating" "Balance Report" "link"
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















