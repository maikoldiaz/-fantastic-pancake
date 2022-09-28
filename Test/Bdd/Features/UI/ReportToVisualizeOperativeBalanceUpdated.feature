@sharedsteps=4013 @owner=jagudelos @ui @testsuite=26840 @testplan=26817
Feature: ReportToVisualizeOperativeBalanceUpdated
As a query user,
I need the report generation page to be updated to include in the filters the operational report without operational cutoff

Background: Login
Given I am logged in as "admin"
@testcase=28256
Scenario: Verify "Con propiedad" option is selected by default in "Reporte balance operativo con o sin propiedad" page
When I navigate to "OperativeBalanceReport" page
Then I validate "Con propiedad" radio button is selected by default
@testcase=28257
Scenario: Verify the end date for the "Reporte operativo" option
When I navigate to "OperativeBalanceReport" page
And I select the radio button "Reporte operativo"
Then I validate the end date should only be enabled until the current date minus one day
@testcase=28258
Scenario Outline: Verify the end date for the "Con corte operativo" option
When I navigate to "OperativeBalanceReport" page
And I select "<Selection>" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Todos" from "Node"
And I select the radio button "Con corte operativo"
Then I validate the end date should only be enabled until the date of the last operational cutoff executed for the selected "<Selection">
Examples:
| Selection |
| Sistema   |
| Segmento  |
| Nodo      |
@testcase=28259
Scenario Outline: Verify the end date for the "Con propiedad" option
When I navigate to "OperativeBalanceReport" page
And I select "<Selection>" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Todos" from "Node"
And I select the radio button "Con propiedad"
Then I validate the end date should only be enabled until the date of the last ownership calculation process executed for the selected "<Selection">
Examples:
| Selection |
| Sistema   |
| Segmento  |
| Nodo      |
@testcase=28260 version = 2
Scenario: Verify the error message when the initial date is greater than the final date
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" as "Greater than Current Date"
And enter "EndDate" as "Current Date"
And I click on "View" "Report" "button"
Then I validate the error message "La fecha inicial debe ser menor o igual a la fecha final"
@testcase=28261
Scenario: Verify the error message when the range between the start and end date is greater or equal than 45 days
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" as "CurrentDate-45 days"
And enter "EndDate" as "CurrentDate"
And I click on "View" "Report" "button"
Then I validate the error message "El rango de días elegidos debe ser menor a 45 días"

@testcase=28262 @bvt
Scenario: Verify the user is able to view the operational report by node
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And I select the radio button "Reporte operativo"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I view the report of the operational balance by node
@testcase=28263
Scenario: Verify the user is able to view the operational report by system
When I navigate to "OperativeBalanceReport" page
And I select "Sistema" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Todos" from "Node"
And I select the radio button "Reporte operativo"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I view the report of the operational balance by system
@testcase=28264
Scenario: Verify the user is able to view the operational report by segment
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Todos" from "Node"
And I select the radio button "Reporte operativo"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I view the report of the operational balance by segment
@testcase=28265
Scenario: Verify the user is able to view the operational balance without ownership report by node
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And I select the radio button "Con corte operativo"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I view the report of the operational balance without ownership by node
@testcase=28266
Scenario: Verify the user is able to view the operational balance without ownership report by system
When I navigate to "OperativeBalanceReport" page
And I select "Sistema" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Todos" from "Node"
And I select the radio button "Con corte operativo"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I view the report of the operational balance without ownership by system
@testcase=28267
Scenario: Verify the user is able to view the operational balance without ownership report by segment
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Todos" from "Node"
And I select the radio button "Con corte operativo"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I view the report of the operational balance without ownership by segment
@testcase=28268
Scenario: Verify the user is able to view the operational balance with ownership report by node
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And I select the radio button "Con propiedad"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I view the report of the operational balance with ownership by node
@testcase=28269
Scenario: Verify the user is able to view the operational balance with ownership report by system
When I navigate to "OperativeBalanceReport" page
And I select "Sistema" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Todos" from "Node"
And I select the radio button "Con propiedad"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I view the report of the operational balance without ownership by system
@testcase=28270 
Scenario: Verify the user is able to view the operational balance with ownership report by segment
When I navigate to "OperativeBalanceReport" page
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Todos" from "Node"
And I select the radio button "Con propiedad"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
Then I view the report of the operational balance without ownership by segment
