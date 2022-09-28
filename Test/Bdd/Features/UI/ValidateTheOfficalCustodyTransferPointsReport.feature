@owner=jagudelos @ui @testsuite=61547 @testplan=61542 @S15
Feature: ValidateTheOfficalCustodyTransferPointsReport
In the TRUE system
As an Administrator or Consulta user
I need to be able to view the Official custody transfer points report
@testcase=66962
Scenario Outline: Verify that no user other than Admin or Consulta has access to the Official custody transfer points page
Given I am logged in as "<User>"
Then I should not see "Official custody transfer points" page
Examples:
| User        |
| audit       |
| aprobador   |
| profesional |
| programador |
@testcase=66963
Scenario Outline: Verify breadcrumb for Official custody transfer points page
Given I am logged in as "<User>"
When I expand the 'Gestión cadena de suministro' menu
Then I validate that 'Puntos oficiales de transferencia de custodia' option is visible
When I navigate to "Official custody transfer points" page
Then I should see breadcrumb "Puntos oficiales de transferencia de custodia"
Examples:
| User     |
| admin    |
| consulta |
@testcase=66964
Scenario Outline: Verify that the report with title 'Puntos oficiales de transferencia de custodia' is embedded when user navigates to the Official custody transfer points page
Given I am logged in as "<User>"
When I navigate to "Official custody transfer points" page
Then I validate that the report with title 'Puntos oficiales de transferencia de custodia' is displayed
Examples:
| User     |
| admin    |
| consulta |
@testcase=66965
Scenario Outline: Validate the template of the report, its tabs and the configured data
Given I am logged in as "<User>"
When I navigate to "Official custody transfer points" page
And I download the report
Then I validate that the report has the following tabs
| Report Tabs                 |
| Portada                     |
| Acuerdo de confidencialidad |
| Puntos Oficiales            |
| Bitacora                    |
And I validate that the content inside each tab is as per the configured data
Examples:
| User     |
| admin    |
| consulta |
@testcase=66966
Scenario Outline: Validate the filters displayed on the Official custody transfer points page
Given I am logged in as "<User>"
When I navigate to "Official custody transfer points" page
Then I validate that the following filters are displayed on the left side of reports page
| Filter Names        |
| Sistema origen      |
| Sistema Destino     |
| Nodo Origen         |
| Nodo Destino        |
| Sistema Oficial     |
Examples:
| User     |
| admin    |
| consulta |
@testcase=66967
Scenario Outline: Validate the functionality of Source system filter
Given I am logged in as "<User>"
When I navigate to "Official custody transfer points" page
And the user searches and selects required values from the Sistema origen dropdown
Then I validate that the report table shows resultant data based on the filters selection
Examples:
| User     |
| admin    |
| consulta |
@testcase=66968
Scenario Outline: Validate the functionality of Destination system filter
Given I am logged in as "<User>"
When I navigate to "Official custody transfer points" page
And the user searches and selects required values from the Sistema Destino dropdown
Then I validate that the report table shows resultant data based on the filters selection
Examples:
| User     |
| admin    |
| consulta |
@testcase=66969
Scenario Outline: Validate the functionality of Source Node filter
Given I am logged in as "<User>"
When I navigate to "Official custody transfer points" page
And the user searches and selects required values from the Nodo Origen dropdown
Then I validate that the report table shows resultant data based on the filters selection
Examples:
| User     |
| admin    |
| consulta |
@testcase=66970
Scenario Outline: Validate the functionality of Destination Node filter
Given I am logged in as "<User>"
When I navigate to "Official custody transfer points" page
And the user searches and selects required values from the Nodo Destino dropdown
Then I validate that the report table shows resultant data based on the filters selection
Examples:
| User     |
| admin    |
| consulta |
@testcase=66971
Scenario Outline: Validate the functionality of Official System filter
Given I am logged in as "<User>"
When I navigate to "Official custody transfer points" page
And the user searches and selects required values from the Sistema Oficial dropdown
Then I validate that the report table shows resultant data based on the filters selection
Examples:
| User     |
| admin    |
| consulta |
@testcase=66972
Scenario Outline: Validate the report with a combination of filters
Given I am logged in as "<User>"
When I navigate to "Official custody transfer points" page
And the user uses more than one or all the filters on the report page
Then I validate that the report table shows resultant data based on the filters selection
Examples:
| User     |
| admin    |
| consulta |
@testcase=66973
Scenario Outline: Validate the functionality when any entity doesn't has an entity name
Given I am logged in as "<User>"
And there are records in the Db with no entity name
When I navigate to "Official custody transfer points" page
Then I validate that such entities must display thier respective entity ids
Examples:
| User     |
| admin    |
| consulta |
@testcase=66974
Scenario Outline: Validate the details on the report for Source Point section
Given I am logged in as "<User>"
When I navigate to "Official custody transfer points" page
Then I validate that a label 'Punto origen' is displayed as a header on the the left part of the report table
And I validate the left-down arrow depicting 'Punto Origen'
And I validate that the following columns are displayed with respective color on the left part of the report table
| Column names       |
| Sistema origen     |
| Tipo de movimiento |
| Producto           |
| Nodo origen        |
| Nodo destino       |
Examples:
| User     |
| admin    |
| consulta |
@testcase=66975
Scenario Outline: Validate the details on the report for Destination Point section
Given I am logged in as "<User>"
When I navigate to "Official custody transfer points" page
Then I validate that a label 'Punto destino' is displayed as a header on the the right part of the report table
And I validate the right-down arrow depicting 'Punto Destino'
And I validate that the following columns are displayed with respective color on the left part of the report table
| Column names       |
| Sistema destino    |
| Tipo de movimiento |
| Producto           |
| Nodo origen        |
| Nodo destino       |
Examples:
| User     |
| admin    |
| consulta |
@testcase=66976 
Scenario Outline: Validate the Official System column on the report page
Given I am logged in as "<User>"
When I navigate to "Official custody transfer points" page
Then I validate that a column 'Sistema oficial' is displayed on the the right most part of the report table
Examples:
| User     |
| admin    |
| consulta |
