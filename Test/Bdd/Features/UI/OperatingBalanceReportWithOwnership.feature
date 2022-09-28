@sharedsteps= @owner=jagudelos @ui @testplan=19772 @testsuite=19794
Feature:OperatingBalanceReportWithOwnership
In order to visualize the operating balance with ownership and traceability of the rules applied
As a Query User
I want to visualize  the operating balance

Background: Login
Given I am logged in as "consulta"
@testcase=21288
Scenario: Verify the report file pages
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
When I open the report file
Then the report page of the report file should be displayed
And the Cover Page Confidentiality Agreement Page and Log page should be hidden
@testcase=21289
Scenario: Verify the first page name of the report
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
When I open the report file
Then the first page of the report file should be named and displayed as "Portada"
@testcase=21290
Scenario: Verify the first page sections of the report
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
When I open the report file
Then the first page of the report file should contain the Title section
Then I should see the Confidentiality agreement section
And I should see the  Information section of the area responsible for the report
And I should see the Update frequency section
Then I should see the Legend section
@testcase=21291
Scenario: Verify the Title section of the report
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
When the first page of the report file is opened and see the Title section
Then I should see EcoPetrol standard color for the section
Then I should see EcoPetrol logo on the left
And I should see report title should on the center
And I should see The word Confidencial on the right
And I should see The refresh rate legend icon on the right
Then I should see the update date in the format dd / mm / yyyy hh: mm: ss am / pm
@testcase=21292
Scenario: Verify the confidentiality agreement section of the report
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
And I open the report file
When the first page of the reportis opened and see confidentiality agreement section
Then the confidentiality icon should be displayed and should have a link to the confidentiality agreement page

@testcase=21293
Scenario: Verify the  information  section of the report
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
And I open the report file
When the first page of the report file is opened and see information  section
Then Ishould see the name of the area that generates the report
Then I should see the  name of the person responsible for the published information

@testcase=21294
Scenario: Verify the Update frequency section of the report
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
And I open the report file
When the first page of the report file is opened and see Update frequency section
Then  I should see the frequency of updating the information according to the legend

@testcase=21295
Scenario: Verify the Update Legend section section of the report
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
And I open the report file
When the first page of the report file is opened and see Legend section section
Then I should see the icons that represent the frequency of updating the information

@testcase=21296
Scenario: Verify the second page name of the report
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
When I open the report file
Then the second page of the report file should be named and displayed as "Acuerdo de confidencialidad"

@testcase=21297
Scenario: Verify the second page sections of the report
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
When I open the second page of the report file
Then I should see the page divided into two sections
And I should see one section as Title section
Then I should see the second section as Information confidentiality agreement
@testcase=21298
Scenario: Verify the Title section of the second page of report file
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
And I open the report file
When the second page of the report file is opened and see the Title section
Then I should see EcoPetrol standard color for the section
Then I should see EcoPetrol logo on the left
And I should see report title should on the center
And I should see The word Confidencial on the right
And I should see The refresh rate legend icon on the right
Then I should see the update word "Actualizado a " and date in the format dd / mm / yyyy hh: mm: ss am / pm
Then I should see icon according to the figure on the right

@testcase=21299
Scenario: Verify the Information confidentiality agreement section of the second page of report file
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
And I open the report file
When the second page of the report file is opened and see the Information confidentiality agreement section
Then I should see the same text indicated in the figure
And I should see the word "de este Datamart XXXXXXXXXX" to that of “de la solución TRUE”

@testcase=21300
Scenario: Verify the third page name of the report
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
When I open the report file
Then the third page of the report file should be named and displayed as "Balance"

@testcase=21301
Scenario: Verify the Title section of the third page of report file
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
And I open the report file
When the third page of the report file is opened and see the Title section
Then I should see EcoPetrol standard color for the section
Then I should see EcoPetrol logo on the left
And I should see report title should on the center
And I should see The word Confidencial on the right
And I should see The refresh rate legend icon on the right
Then I should see the update word "Actualizado a " and date in the format dd / mm / yyyy hh: mm: ss am / pm
Then I should see the word Confidencial

@testcase=21302
Scenario: Verify the fourth page name of the report
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
When I open the report file
Then the fourth page of the report file should be named and displayed as "Bitacora"

@testcase=21303
Scenario: Verify the fourth page sections of the report
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
And I open the report file
When I open the fourth page of the report file
Then I should see the page divided into two sections
And I should see one section as Title section
Then I should see the second section as Report version information
@testcase=21304
Scenario: Verify the Title section of the fourth page of report file
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
And I open the report file
When the fourth page of the report file is opened and see the Title section
Then I should see EcoPetrol standard color for the section
Then I should see EcoPetrol logo on the left
And I should see report title should on the center
And I should see The word Confidencial on the right
And I should see The refresh rate legend icon on the right
Then I should see the update word "Actualizado a " and date in the format dd / mm / yyyy hh: mm: ss am / pm
Then I should see the word Confidencial
@testcase=21305 
Scenario: Verify the Report version information section of the fourth page of report file
Given I have "Operative Balance" in the system
When I navigate to "Conveyor balance with property" "page"
And I click on "Operating Balance Report" "link"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
And I open the report file
When the fourth page of the report file is opened and see the Report version information section
Then I should see version of the report
And I should see the update date of the report in the format dd / mm / yyyy hh: mm: ss am / pm
Then I should see the name of the person responsible for the change
