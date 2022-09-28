@owner=jagudelos @ui @testsuite=39228 @testplan=39221
Feature: ReportToVisualizeTicketNodesAndStatus
As a query user,
I require a report to
visualize the ticket nodes and their status

@testcase=41474
Scenario Outline: Validate Node status report menu
Given I am logged in as "<Role>"
Then I navigate to "Node States" page
Examples:
| Role        |
| profesional |
| admin       |
| consulta    |

@testcase=41475
Scenario: Validate error when difference between start date and end date is greater than 40 days
Given I am logged in as "Admin"
When I navigate to "Node States" page
And I select segment with ownership from "nodeFilter" "element" "dropdown"
And I enter end date into "nodeFilter" "finalDate" "date"
And I enter start date less than 40 days into "nodeFilter" "initialDate" "date"
And I click on "nodeFilter" "viewReport" "button"
Then I should see "El rango de días elegidos debe ser menor a 40 días" message

@testcase=41476
Scenario: Validate error when start date is greater than end date
Given I am logged in as "Admin"
And I have ownership for node status
When I navigate to "Node States" page
And I select segment with ownership from "nodeFilter" "element" "dropdown"
And I enter start date in "nodeFilter" "initialDate" "date"
And I enter end date less than start date into "nodeFilter" "finalDate" "date"
And I click on "nodeFilter" "viewReport" "button"
Then I should see "La fecha inicial debe ser menor o igual a la fecha final" message

@testcase=41477
Scenario: Validate error when end date is greater than current date
Given I am logged in as "Admin"
When I navigate to "Node States" page
And I select segment with ownership from "nodeFilter" "element" "dropdown"
And I enter start date in "nodeFilter" "initialDate" "date"
And I enter end date more than current date  into "nodeFilter" "finalDate" "date"
And I click on "nodeFilter" "viewReport" "button"
Then I should see "La fecha final debe ser menor a la fecha actual" message

@testcase=41478
Scenario: Validate mandatory fields
Given I am logged in as "Admin"
When I navigate to "Node States" page
And I click on "nodeFilter" "viewReport" "button"
Then I should see the message on interface "Obligatorio"

@testcase=41479 @manual
Scenario: Validate sheets of template
Given I am logged in as "consulta"
When I navigate to "Node States" page
And I select "segment category" from "segment"
And I enter date "start date" into "nodeFilter" "initialDate" "date"
And I enter date "end date" into "nodeFilter" "finalDate" "date"
And I click on "nodeFilter" "viewReport" "button"
And the "Node Status report" has the template data configured
And the parametric information for the template is obtained from configuration table
And pbix file is downloaded for "Node Status Report"
And it is opened in power bi desktop
Then I see "Cover Page" with all the data from confuguration table
And I see "Confidentiality Agreement Page" with data from confuguration table
And I see "Report Page" with data from configuration table
And I see "Log Page" with data from configuration table

@testcase=41480 @manual
Scenario: Validate report header
Given I am logged in as "consulta"
When I navigate to "Node States" page
And I select "segment category" from "segment"
And I enter date "start date" into "nodeFilter" "initialDate" "date"
And I enter date "end date" into "nodeFilter" "finalDate" "date"
And I click on "nodeFilter" "viewReport" "button"
Then I see report title as "Aprobaciones del balance con propiedad por nodo"

@testcase=41481 @manual
Scenario: Validate approval level
Given I am logged in as "consulta"
When I navigate to "Node States" page
And I select "segment category" from "segment"
And I enter date "start date" into "nodeFilter" "initialDate" "date"
And I enter date "end date" into "nodeFilter" "finalDate" "date"
And I click on "nodeFilter" "viewReport" "button"
Then I see report title as "Aprobaciones del balance con propiedad por nodo"
And I see graph with title "NIVEL DE APROBACIÓN" showing percentage of nodes in approved state

@testcase=41482 @manual
Scenario: Validate case without timely management
Given I am logged in as "consulta"
When I navigate to "Node States" page
And I select "segment category" from "segment"
And I enter date "start date" into "nodeFilter" "initialDate" "date"
And I enter date "end date" into "nodeFilter" "finalDate" "date"
And I click on "nodeFilter" "viewReport" "button"
Then I see report title as "Aprobaciones del balance con propiedad por nodo"
And I see graph with title "CASOS SIN GESTIÓN OPORTUNA" showing percentage of nodes that are not approved
And I see percentage between n+1 to n+2 days
And I see percentage between n+3 to n+4 days
And I see percentage for more than n+5 days

@testcase=41483 @manual
Scenario: Validate global review status
Given I am logged in as "consulta"
When I navigate to "Node States" page
And I select "segment category" from "segment"
And I enter date "start date" into "nodeFilter" "initialDate" "date"
And I enter date "end date" into "nodeFilter" "finalDate" "date"
And I click on "nodeFilter" "viewReport" "button"
Then I see report title as "Aprobaciones del balance con propiedad por nodo"
And I see graph with title "ESTADO GLOBAL DE REVISIONES" showing percentage of nodes grouped by states
And I see percentage of approved nodes
And I see percentage and state of nodes with color associated with the state

@testcase=41484 @manual
Scenario: Validate current status per node
Given I am logged in as "consulta"
When I navigate to "Node States" page
And I select "segment category" from "segment"
And I enter date "start date" into "nodeFilter" "initialDate" "date"
And I enter date "end date" into "nodeFilter" "finalDate" "date"
And I click on "nodeFilter" "viewReport" "button"
Then I see report title as "Aprobaciones del balance con propiedad por nodo"
And I see graph with title "ESTADO ACTUAL POR NODO" displaying names of all nodes in ascending order
And I see current state of each node under operational date

@testcase=41485 @manual
Scenario: Validate sub filters
Given I am logged in as "consulta"
When I navigate to "Node States" page
And I select "segment category" from "segment"
And I enter date "start date" into "nodeFilter" "initialDate" "date"
And I enter date "end date" into "nodeFilter" "finalDate" "date"
And I click on "nodeFilter" "viewReport" "button"
Then I see report title as "Aprobaciones del balance con propiedad por nodo"
And I see filter with title "FECHA" to select operating dates range
And I see filter with title "SISTEMA" to select one or more systems

@testcase=41486 @manual
Scenario: Validate details page
Given I am logged in as "consulta"
When I navigate to "Node States" page
And I select "segment category" from "segment"
And I enter date "start date" into "nodeFilter" "initialDate" "date"
And I enter date "end date" into "nodeFilter" "finalDate" "date"
And I click on "nodeFilter" "viewReport" "button"
Then I see report title as "Aprobaciones del balance con propiedad por nodo"
And I click on "View detailed table" "link"
And I see "<Columns>" with detailed information
| Columns                |
| Fecha inicio operativa |
| Fecha fin operativa    |
| Segmento               |
| Sistema                |
| Nodo                   |
| Estado nodo            |
| Fecha cambio estado    |
| Aprobador              |
| Comentario             |
And I see column value as empty if value is null

@testcase=41487 @manual
Scenario: Validate link between pages
Given I am logged in as "consulta"
When I navigate to "Node States" page
And I select "segment category" from "segment"
And I enter date "start date" into "nodeFilter" "initialDate" "date"
And I enter date "end date" into "nodeFilter" "finalDate" "date"
And I click on "nodeFilter" "viewReport" "button"
Then I see report title as "Aprobaciones del balance con propiedad por nodo"
And I click on "View detailed table" "link"
And I see details page
And I click on "Status revisions" "link"
And I see node review page