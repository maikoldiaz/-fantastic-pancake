@sharedsteps= @owner=jagudelos @ui @testplan=19772 @testsuite=19785
Feature:OperatingBalanceReportWithAndWithoutOwnership
In order to visualize the operating balance With or without ownership and traceability of the rules applied
As a Query User
I want to visualize  the operating balance

Background: Login
Given I am logged in as "consulta"
@testcase=21281
Scenario: Verify the reports without ownership
Given that I have operating balance without ownership and traceability of the rules applied
When I navigate to "Operating Balance Report" "page"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
And I click on "View" "Report" "button"
When the input parameters are filled, and the ownership check is deselected
Then I should see the report of the balance without ownership the new variables embedded in the site
Then I should see the message "BALANCE OPERATIVO"
@testcase=21282
Scenario: Verify the reports with ownership
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Operating Balance Report" "page"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
When the input parameters are filled, and the ownership check is selected
And I click on "View" "Report" "button"
And I click on "with" "ownership" "checkbox"
Then I should see the report of the balance with ownership the new variables embedded in the site
Then I should see message "BALANCE VOLUMÉTRICO CON PROPIEDAD"
@testcase=21283
Scenario: Verify the reports with ownership and if there is no data
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Operating Balance Report" "page"
And I select "Segmento" from "Category"
And I select "CategorySegment" from "CategoryElement"
And I select "Node" from "Node"
And enter "StartDate" into "StartDate" "date"
And enter "EndDate" into "EndDate" "date"
When input parameters are filled, and the ownership check is selected and there is not data
And I click on "View" "Report" "button"
Then I should see the report of the balance with ownership in an empty report structure

@testcase=21284
Scenario: Verify there is ownership checkbox placed
Given that I have operating balance with ownership and traceability of the rules applied
When I navigate to "Operating Balance Report" "page"
When I click on Reporte Balance operativo link
Then I should see a checkbox is added to the current parameters indicating to the system when a report with ownership should be queried

@testcase=21285 @manual
Scenario: Calculate the Operational Balance with Start Date greater than the End Date
Given I want to calculate the "OperationalBalance" in the system
When I receive "StartDate" greater than the "EndDate"
Then the result should fail with message "DATES_INCONSISTENT"


@testcase=21286 @manual
Scenario: Calculate the Operational Balance with End Date greater than or equal to the Current Date
Given I want to calculate the "OperationalBalance" in the system
When I receive "EndDate" greater than or equal to the "CurrentDate"
Then the result should fail with message "ENDDATE_BEFORENOWVALIDATION"
