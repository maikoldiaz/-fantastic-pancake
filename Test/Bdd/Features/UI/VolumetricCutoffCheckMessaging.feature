@sharedsteps=4013 @Owner=jagudelos @ui @testplan=8481 @testsuite=8486
Feature: VolumetricCutoffCheckMessaging
In order to perform the volumetric cutoff information
As a transport segement user
I want check messaging

Background: Login
	Given I am logged in as "admin"

@testcase=9912 @manual
Scenario: Validate Balance Standard Uncertanity validation
	When I navigate to "Operational Cutoff" page
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And "Percentage Standard uncertainty” parameter" doesn't have any setting in the system
	When I click on "InitTicket" "submit" "button"
	Then I should see the message "TRUE no tiene la configuración del parámetro % Incertidumbre estándar"

@testcase=9913 @manual
Scenario: Validate Balance Control Limit validation
	When I navigate to "Operational Cutoff" page
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And "Balance Control Limit” parameter" doesn't have any setting in the system
	When I click on "InitTicket" "submit" "button"
	Then I should see the message "TRUE no tiene la configuración del parámetro Límite de Control"

@testcase=9914 @manual
Scenario: Validate Acceptable Balance Percentage validation
	When I navigate to "Operational Cutoff" page
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And "Acceptable Balance Percentage” parameter" doesn't have any setting in the system
	When I click on "InitTicket" "submit" "button"
	Then I should see the message "TRUE no tiene la configuración del parámetro Porcentaje de Balance Aceptable"

@testcase=9915 @version=2 @prodready
Scenario: Validate following button functionality
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose any CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on "Cutoff" DatePicker
	Then I should see next button as enabled

@testcase=9916 @version=2 @prodready
Scenario: Validate Segment Mandatory validation
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	When I click on "InitTicket" "Segment" "combobox" and didn't choosen category element
	Then I should see the message "El segmento es obligatorio"

@testcase=9917 @bvt @version=4 @prodready
Scenario: Validate message when there are no pending transacction records for movements and inventories
	And I have pending transactions in the system
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on "Cutoff" DatePicker
	When I click on "InitTicket" "submit" "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	Then I should see "Add Note Functions" interface
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	Then I should see the message "Sin registros" when there are no pending records
	Then validate that "ErrorsGrid" "Submit" "button" as enabled

@testcase=9918
Scenario: Verify the pending movements are displayed based on the selected Initial and final dates of operational cutoff
	When I navigate to "Operational Cutoff" page
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on "Cutoff" DatePicker
	When I click on "InitTicket" "submit" "button"
	Then I should see the pending movements are displayed based on the selected Initial and final dates of operational cutoff

@testcase=9919
Scenario: Verify the pending inventories are displayed one day before the Initial or Final date of operational cutoff
	When I navigate to "Operational Cutoff" page
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on "Cutoff" DatePicker
	When I click on "InitTicket" "submit" "button"
	Then I should see the pending inventories are displayed one day before the Initial or Final date of operational cutoff

@testcase=9920
Scenario: Verify the pending records are grouped based on the source system
	When I navigate to "Operational Cutoff" page
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on "Cutoff" DatePicker
	When I click on "InitTicket" "submit" "button"
	Then I should see the pending records are grouped based on the source system

@testcase=9921
Scenario: Verify the pending records sorting
	When I navigate to "Operational Cutoff" page
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on "Cutoff" DatePicker
	When I click on "InitTicket" "submit" "button"
	Then I should see the pending records sorted by volume from highest to lowest

@testcase=9922
Scenario Outline: Verify the pending movements information should be displayed corresponds to actual records
	When I navigate to "Operational Cutoff" page
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on "Cutoff" DatePicker
	When I click on "InitTicket" "submit" "button"
	Then I should see the pending records "<Fields>" mappeed according to the actual records

	Examples:
		| Field               |
		| Source System       |
		| Type                |
		| Movement Type       |
		| Source Node         |
		| Destination Node    |
		| Source Product      |
		| Destination Product |
		| Net Volumen         |
		| Unit                |
		| Start Date          |
		| End Date            |
		| Exception           |

@testcase=9923
Scenario Outline: Verify the pending inventories information should be displayed corresponds to actual records
	When I navigate to "Operational Cutoff" page
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on "Cutoff" DatePicker
	When I click on "InitTicket" "submit" "button"
	Then I should see the pending records "<Fields>" mappeed according to the actual records

	Examples:
		| Field          |
		| Source System  |
		| Type           |
		| Movement Type  |
		| Node           |
		| Product        |
		| Net Volumen    |
		| Unit           |
		| Inventory Date |
		| Exception      |

@testcase=9924 @version=2 @prodready
Scenario: Verify the defualt behaviour of Check consistyency button
	And I have pending transactions in the system
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on "Cutoff" DatePicker
	When I click on "InitTicket" "submit" "button"
	Then validate that "ErrorsGrid" "Submit" "button" as disabled

@testcase=9925 @version=3 @prodready
Scenario: Validate the note madatory message on Add Note Functions Interface
	And I have pending transactions in the system
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on "Cutoff" DatePicker
	When I click on "InitTicket" "submit" "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	Then I should see "Add Note Functions" interface
	When I don't enter valid value into "Add Note" "textbox"
	And I click on "AddComment" "Submit" "button"
	Then I should see the message "La nota es requerida"

@testcase=9926 @version=3 @prodready
Scenario: Enter more than 1000 characters on Add notes text box
	And I have pending transactions in the system
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on "Cutoff" DatePicker
	When I click on "InitTicket" "submit" "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	Then I should see "Add Note Functions" interface
	When I enter morethan 1000 characters into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	Then I should see the message "La nota puede contener máximo 1000 caracteres​​"

@testcase=9927 @version=3 @prodready
Scenario: Enter valid notes for pending transactions
	And I have pending transactions in the system
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on "Cutoff" DatePicker
	When I click on "InitTicket" "submit" "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	Then I should see "Add Note Functions" interface
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	Then I shouldn't see any pending transactions on grid