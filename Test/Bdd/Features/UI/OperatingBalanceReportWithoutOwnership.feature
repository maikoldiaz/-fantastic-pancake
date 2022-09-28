@sharedsteps= @owner=jagudelos @ui @testplan=14709 @testsuite=14730
Feature:OperatingBalanceReportWithoutOwnership
In order to visualize the operating balance without ownership and traceability of the rules applied
As a Query User
I want to visualize  the operating balance

Background: Login
	Given I am logged in as "consulta"

@testcase=16558 @manual
Scenario: Verify the report file pages
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	When I open the report file
	Then the report page of the report file should be displayed
	And the Cover Page Confidentiality Agreement Page and Log page should be hidden

@testcase=16559 @manual
Scenario: Verify the first page name of the report
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	When I open the report file
	Then the first page of the report file should be named and displayed as "Portada"

@testcase=16560 @manual
Scenario: Verify the first page sections of the report
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	When I open the report file
	Then the first page of the report file should contain the Title section
	Then I should see the Confidentiality agreement section
	And I should see the  Information section of the area responsible for the report
	And I should see the Update frequency section
	Then I should see the Legend section

@testcase=16561 @manual
Scenario: Verify the Title section of the report
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	When the first page of the report file is opened and see the Title section
	Then I should see EcoPetrol standard color for the section
	Then I should see EcoPetrol logo on the left
	And I should see report title should on the center
	And I should see The word Confidencial on the right
	And I should see The refresh rate legend icon on the right
	Then I should see the update date in the format dd / mm / yyyy hh: mm: ss am / pm

@testcase=16562 @manual
Scenario: Verify the confidentiality agreement section of the report
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	When the first page of the reportis opened and see confidentiality agreement section
	Then the confidentiality icon should be displayed and should have a link to the confidentiality agreement page

@testcase=16563 @Manual
Scenario: Verify the  information  section of the report
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	When the first page of the report file is opened and see information  section
	Then Ishould see the name of the area that generates the report
	Then I should see the  name of the person responsible for the published information

@testcase=16564 @manual
Scenario: Verify the Update frequency section of the report
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	When the first page of the report file is opened and see Update frequency section
	Then  I should see the frequency of updating the information according to the legend

@testcase=16565 @manual
Scenario: Verify the Update Legend section section of the report
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	When the first page of the report file is opened and see Legend section section
	Then I should see the icons that represent the frequency of updating the information

@testcase=16566 @manual
Scenario: Verify the second page name of the report
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	When I open the report file
	Then the second page of the report file should be named and displayed as "Acuerdo de confidencialidad"

@testcase=16567 @manual
Scenario: Verify the second page sections of the report
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	When I open the second page of the report file
	Then I should see the page divided into two sections
	And I should see one section as Title section
	Then I should see the second section as Information confidentiality agreement

@testcase=16568 @manual
Scenario: Verify the Title section of the second page of report file
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	When the second page of the report file is opened and see the Title section
	Then I should see EcoPetrol standard color for the section
	Then I should see EcoPetrol logo on the left
	And I should see report title should on the center
	And I should see The word Confidencial on the right
	And I should see The refresh rate legend icon on the right
	Then I should see the update word "Actualizado a " and date in the format dd / mm / yyyy hh: mm: ss am / pm
	Then I should see icon according to the figure on the right

@testcase=16569 @manual
Scenario: Verify the Information confidentiality agreement section of the second page of report file
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	When the second page of the report file is opened and see the Information confidentiality agreement section
	Then I should see the same text indicated in the figure
	And I should see the word "de este Datamart XXXXXXXXXX" to that of “de la solución TRUE”

@testcase=16570 @manual
Scenario: Verify the third page name of the report
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	When I open the report file
	Then the third page of the report file should be named and displayed as "Balance"

@testcase=16571 @manual
Scenario: Verify the Title section of the third page of report file
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	When the third page of the report file is opened and see the Title section
	Then I should see EcoPetrol standard color for the section
	Then I should see EcoPetrol logo on the left
	And I should see report title should on the center
	And I should see The word Confidencial on the right
	And I should see The refresh rate legend icon on the right
	Then I should see the update word "Actualizado a " and date in the format dd / mm / yyyy hh: mm: ss am / pm
	Then I should see the word Confidencial

@testcase=16572 @manual
Scenario: Verify the fourth page name of the report
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	When I open the report file
	Then the fourth page of the report file should be named and displayed as "Bitacora"

@testcase=16573 @manual
Scenario: Verify the fourth page sections of the report
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	When I open the fourth page of the report file
	Then I should see the page divided into two sections
	And I should see one section as Title section
	Then I should see the second section as Report version information

@testcase=16574 @manual
Scenario: Verify the Title section of the fourth page of report file
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	When the fourth page of the report file is opened and see the Title section
	Then I should see EcoPetrol standard color for the section
	Then I should see EcoPetrol logo on the left
	And I should see report title should on the center
	And I should see The word Confidencial on the right
	And I should see The refresh rate legend icon on the right
	Then I should see the update word "Actualizado a " and date in the format dd / mm / yyyy hh: mm: ss am / pm
	Then I should see the word Confidencial

@testcase=16575 @manual
Scenario: Verify the Report version information section of the fourth page of report file
	Given that I have operating balance without ownership and traceability of the rules applied
	When I navigate to "Balance transportadores con propiedad" page
	And I click on the Balance transportadores con propiedad link
	And I click on corte operativo link
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View Report"
	And I open the report file
	When the fourth page of the report file is opened and see the Report version information section
	Then I should see version of the report
	And I should see the update date of the report in the format dd / mm / yyyy hh: mm: ss am / pm
	Then I should see the name of the person responsible for the change