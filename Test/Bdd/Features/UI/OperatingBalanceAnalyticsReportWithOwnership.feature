@sharedsteps= @owner=jagudelos @ui @testplan=19772 @testsuite=19776
Feature:OperatingBalanceAnalyticsReportWithOwnership
In order to visualize the operating balance Analytics with ownership and traceability of the rules applied
As a Query User
I want to visualize  the operating balance Analytics

Background: Login
Given I am logged in as "consulta"
@testcase=21261
Scenario: Verify the Operational Balance report with Ownership
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And Check the "ownership" "checkbox"
And click on the "view" "report" "Button"
Then I should see the reports with the corresponding data
Then I should see the Analytics section of the corresponding data in the report
@testcase=21262
Scenario: Verify the Analytics Section in the report
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And Check the "ownership" "checkbox"
And click on the "view" "report" "Button"
When I see the Analytics section
Then I should see the "Diagrama Pérdidas No Identificadas" chart
And I should see the "Movimientos" chart
Then I should see the "Inventario Final" chart
@testcase=21263
Scenario: Verify the Pérdidas No Identificadas diagram in the report
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And Check the "ownership" "checkbox"
And click on the "view" "report" "Button"
And I see the Analytics section
When I see the Pérdidas No Identificadas chart
Then I should see the net volume of the unidentified losses by owner for the period including the units of measure and the percentage
@testcase=21264
Scenario: Verify the Movimientos chart and section in the report
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And Check the "ownership" "checkbox"
And click on the "view" "report" "Button"
And I see the Analytics section
When I see the Movimientos section in the report
Then I should see the percentage of each volumetric balance variable for the period by owner
Then I should see the followed List of variables
| List of variables         |
| Inventario Inicial        |
| Entradas                  |
| Salidas                   |
| Pérdidas Identificadas    |
| Tolerancia                |
| Interfases                |
| Pérdidas No Identificadas |
| Inventario Final          |
| Control                   |
@testcase=21265 
Scenario: Verify the Inventario Final chart and section in the report
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And Check the "ownership" "checkbox"
And click on the "view" "report" "Button"
And I see the Analytics section
When I see the Inventario Final section in the report
Then I should see the percentage of ownership distribution for each product of the final inventories for the period




