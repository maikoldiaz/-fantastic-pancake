@sharedsteps=16581 @owner=jagudelos @ui @testsuite=21113 @testplan=19772
Feature: ExcelFileGenerationWithLogisticInformationfeature
In order to load the SIV file
As a True system
I need to generate a file with the logistic information of the balance with ownership

Background: Login
Given I am logged in as "profesional"

@testcase=21204 @bvt @version=2
Scenario: Verify the structure of the file with ownership logistic information
	Given I have data with logistic center information
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I selected Segment from "Segment" "CreateLogistics" "combobox"
	And I select Owner on the Create file interface
	And I select Start date and End Date on Create file Interface
	And I click on "CreateLogistics" "Submit" "button"
	When I click on "Tickets" "Download" "link"
	Then it should contains columns and values as per the defined mapping

@testcase=21205
Scenario: Verify the message when file with ownership logistic information
	And I have data with no logistic center information
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I selected Segment from "Segment" "CreateLogistics" "combobox"
	And I select Owner on the Create file interface
	And I select Start date and End Date on Create file Interface
	And I click on "CreateLogistics" "Submit" "button"
	Then I should see the message "No hay información logística para los criterios dados" in the system

@testcase=21206
Scenario: Verify the logistic file static message
	And I have data with logistic center information
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I selected Segment from "Segment" "CreateLogistics" "combobox"
	And I select Owner on the Create file interface
	And I select Start date and End Date on Create file Interface
	And I click on "CreateLogistics" "Submit" "button"
	Then I should see message "Información operativa con propiedad para el segmento de transporte" in the excel

@testcase=21207
Scenario: Verify message when no homologation found between SAP and TRUE for Movement Type
	And I have data with with no homoloation between SAP and TRUE
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I selected Segment from "Segment" "CreateLogistics" "combobox"
	And I select Owner on the Create file interface
	And I select Start date and End Date on Create file Interface
	And I click on "CreateLogistics" "Submit" "button"
	Then I should see the message "No se encontró una homologación entre TRUE y SIV, por favor revise la homologación" in the system

@testcase=21208
Scenario: Verify message when invalid combination to SIV movement
	And I have data with with no homoloation between SAP and TRUE
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I selected Segment from "Segment" "CreateLogistics" "combobox"
	And I select Owner on the Create file interface
	And I select Start date and End Date on Create file Interface
	And I click on "CreateLogistics" "Submit" "button"
	Then I should see the message "La combinación de producto, centro logístico y almacén es invalida y no esta considerada para envió a SIV" in the system

@testcase=21209
Scenario: : Verify the Movement of type Translado are resolved as per the defined logic
	And I have data with logistic center information with Movement Type "Translado"
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I selected Segment from "Segment" "CreateLogistics" "combobox"
	And I select Owner on the Create file interface
	And I select Start date and End Date on Create file Interface
	And I click on "CreateLogistics" "Submit" "button"
	Then I should see Logistic Report for selected segment in the Logistic Report Generation page
	And I should see the value "Traslado Materia a Material" on the excel file for MovementTypeId

@testcase=21210
Scenario: : Verify that the Movement type of the type id is homologated based on TRUE to SIV homologation
	And I have data with logistic center information with Movement Type "Translado"
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I selected Segment from "Segment" "CreateLogistics" "combobox"
	And I select Owner on the Create file interface
	And I select Start date and End Date on Create file Interface
	And I click on "CreateLogistics" "Submit" "button"
	Then I should see Logistic Report for selected segment in the Logistic Report Generation page
	And I should see the homologated value on the excel file for MovementTypeId

@testcase=21211
Scenario: : Verify the file name of generated excel
	And I have data with logistic center information
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I selected Segment from "Segment" "CreateLogistics" "combobox"
	And I select Owner on the Create file interface
	And I select Start date and End Date on Create file Interface
	And I click on "CreateLogistics" "Submit" "button"
	Then I should see Logistic Report for selected segment in the Logistic Report Generation page
	And it should display the file name as "ReporteLogistico_[SegmentName]_[OwnerName]_[TicketID].xls"

@testcase=21212
Scenario: Verify the concatenation of values as per the source and destination storage locations
	And I have data with logistic center information
	When I navigate to "Logistic Report Generation" page
	And I click on "CreateLogistics" "button"
	Then I should see "CreateLogistics" "Create" "Interface"
	When I selected Segment from "Segment" "CreateLogistics" "combobox"
	And I select Owner on the Create file interface
	And I select Start date and End Date on Create file Interface
	And I click on "CreateLogistics" "Submit" "button"
	Then I should see Logistic Report for selected segment in the Logistic Report Generation page
	And I should see the concatenation of source and destination storage locations values