@sharedsteps=16581 @owner=jagudelos @ui @testplan=19772 @testsuite=19781
Feature: GenerateOwnershipBalanceFileProcessedBySIVSystem
In order to Generate Ownership Balance File to be Processed by SIV System
As a Professional Segment Balance User
I want an UI to generate the ownership balance file
to be processed by the SIV system

Background: Login
	Given I am logged in as "profesional"
	
@testcase=21233 @ui
Scenario: Verify generated Ownership balance files for the segments in the last 40 days
	Given I have generated Ownership balance files from the last 40 days
	When I navigate to "Logistic Report Generation" page
	Then I should see the information of "Logistic Report Generation" in the grid
	
@testcase=21234 @ui @manual @version=2
Scenario: Verify Logistic Report Generation page when there are no files generated in the last 40 days
	Given I did not have generated Ownership balance files from the last 40 days
	When I navigate to "Logistic Report Generation" page
	Then I should see error message "Sin registros"
	
@testcase=21235 @ui @bvt @version=3 @ignore
Scenario: Verify Ownership balance file is generated in Logistic Report Generation page
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I selected Segment from "CreateLogistics" "Segment" "combobox"
	And I select Owner on the Create file interface
	And I select Start date and End Date on Create file Interface
	And I click on "CreateLogistics" "Submit" "button"
	Then I should see Logistic Report for selected segment in the "Logistic Report Generation" in the grid

@testcase=21236 @ui @version=2
Scenario: Verify the functionality when the generated file is in Processing state
	Given I have generated Ownership balance files from the last 40 days
	When I navigate to "Logistic Report Generation" page
	And I have a record with "Processing" state and I searched in "Logistic Report Generation" Grid
	Then verify that "Tickets" "Download" "link" is "disabled"
	And verify that "Tickets" "ViewSummary" "link" is "disabled"

@testcase=21237 @ui @version=2
Scenario: Verify the Download file functionality in Logistic Report Generation page
	Given I have generated Ownership balance files from the last 40 days
	When I navigate to "Logistic Report Generation" page
	And I have a record with "Completed" state and I searched in "Logistic Report Generation" Grid
	Then verify that "Tickets" "Download" "link" is "enabled"
	And verify that "Tickets" "ViewSummary" "link" is "disabled"
	When I click on "Tickets" "Download" "link"
	Then I should be able to download the generated report file successfully

@testcase=21238 @ui @version=2
Scenario: Verify the functionality when the generated file is in Completed with error state
	Given I have generated Ownership balance files from the last 40 days
	When I navigate to "Logistic Report Generation" page
	And I have a record with "Error" state and I searched in "Logistic Report Generation" Grid
	Then verify that "Tickets" "Download" "link" is "disabled"
	And verify that "Tickets" "ViewError" "link" is "enabled"
	When I click on "Tickets" "ViewError" "link"
	Then I should see a modal window with the error information in "Logistic Report Generation" page

@testcase=21239 @ui @version=3 @ignore
Scenario Outline: Verify validations while generation of a file in Logistic Report Generation page
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I select Segment from "<Field1>" combobox
	And I select "<Field2>" on the Create file interface
	And I select "<Field3>" and "<Field4>" on Create file Interface where Ownership is not calculated for selected Segment
	And I click on "CreateLogistics" "Submit" "button"
	Then I should see the message on interface "<Message>"
	
	Examples:
	| Field1            | Field2           | Field3                        | Field4                                       | Message                                                                                                    |
	| NotSelectedSement | NotSelectedOwner | NotSelectedStartDate          | NotSelectedEndDate                           | Requerido                                                                                                  |
	| ValidSegment      | ValidOwner       | StartDateGreaterThanFinalDate | ValidEndDate                                 | La fecha inicial debe ser antes que la fecha final, y la fecha final debe ser posterior a la fecha inicial |
	| ValidSegment      | ValidOwner       | ValidStartDate                | EndDateGreaterThanOrEqualtoCurrentDate       | La fecha final debe ser menor a la fecha actual                                                            |
	| ValidSegment      | ValidOwner       | ValidStartDate                | RangeBetweenStartAndEndDateGreaterThan60Days | El rango de días elegidos debe ser menor a 60 días.                                                        |


@testcase=21240 @ui @version=3 @ignore
Scenario: Verify validations while generation of a file in Logistic Report Generation page where Ownership is not calculated for selected Segment
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I select Segment where Ownership is not calculated for it
	Then Start Date and End Date on Create file Interface should be "disabled"
	And I should see error message "No se encontró cálculo de propiedad para el segmento. Por favor realice el cálculo de propiedad primero."

@testcase=21241 @ui @version=3 @ignore
Scenario: Verify Cancel functionality for Create file Interface
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I click on "CreateLogistics" "Cancel" "button"
	Then I should see breadcrumb "Generación reporte logístico"

@testcase=21242 @ui @version=2
Scenario Outline: Verify the displayed columns on Logistic Report Generation page
	When I navigate to "Logistic Report Generation" page
	And the page is loaded into the UI
	Then I should see the "<Columns>" on Logistic Report page
	
	Examples:
	| Columns         |
	| Segmento        |
	| Propietario     |
	| Fecha Inicial   |
	| Fecha Final     |
	| Fecha Ejecución |
	| Usuario         |
	| Estado          |


@testcase=21243 @ui @version=2
Scenario Outline: Verify Filters functionality
	Given I have generated Ownership balance files from the last 40 days
	When I navigate to "Logistic Report Generation" page
	And the page is loaded into the UI
	And I provide the value for "tickets" "<Field>" "<ControlType>" filter in "Logistic Report Generation" Grid
	Then I should see the information that matches the data entered for the "<Field>" in "Logistic Report Generation" Grid
	
	Examples:
	| Field       | ControlType |
	| Segment     | textbox     |
	| OwnerName   | textbox     |
	| StartDate   | date        |
	| EndDate     | date        |
	| CreatedDate | date        |
	| CreatedBy   | textbox     |
	| Status      | combobox    |

@testcase=21244 @ui @version=2
Scenario Outline: Verify Sorting functionality
	Given I have generated Ownership balance files from the last 40 days
	When I navigate to "Logistic Report Generation" page
	And the page is loaded into the UI
	And I click on the "<ColumnName>"
	Then the results should be sorted based on "<ColumnName>" in "Logistic Report Generation" Grid
	
	Examples:
	| ColumnName    |
	| Segment       |
	| Owner         |
	| StartDate     |
	| EndDate       |
	| ExecutionDate |
	| Username      |
	| Status        |

@testcase=21245
Scenario: Verify Pagination functionality
	Given I have generated Ownership balance files from the last 40 days
	When I navigate to "Logistic Report Generation" page
	And the page is loaded into the UI
	And I navigate to second page in "Logistic Report Generation" Grid
	Then the records should be displayed accordingly in "Logistic Report Generation" Grid
	When I change the elements count per page to 50
	Then the records count in "Logistic Report Generation" Grid shown per page should also be 50

@testcase=21246 @version=2
Scenario: Verify the breadcrumb of Ownership balance files for the segments page
	When I navigate to "Logistic Report Generation" page
	Then I should see breadcrumb "Generación reporte logístico"

@testcase=21247 @manual @version=2
Scenario: Verify the Range of Start and End Date must be parametric per system and can be configured in the Database
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I selected Segment from "CreateLogistics" "Segment" "combobox"
	And I select Owner on the Create file interface
	Then range of Start Date and End Date should be same as value configured in the Database

@testcase=25182 @ui @version=3 @ignore
Scenario: Verify Ownership balance file can be generated more than once for same segment with same date range
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I selected Segment from "CreateLogistics" "Segment" "combobox"
	And I select Owner on the Create file interface
	And I select Start date and End Date on Create file Interface
	And I click on "CreateLogistics" "Submit" "button"
	Then I should see Logistic Report for selected segment in the "Logistic Report Generation" in the grid
	When I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I selected Segment from "CreateLogistics" "Segment" "combobox"
	And I select Owner on the Create file interface
	And I select Start date and End Date on Create file Interface
	And I click on "CreateLogistics" "Submit" "button"
	Then I should see Logistic Report for selected segment in the "Logistic Report Generation" in the grid