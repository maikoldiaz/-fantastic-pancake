@owner=jagudelos @ui @testplan=8481 @testsuite=8490
Feature: VolumetricCutoffCheckConsistency
In order to perform the volumetric cutoff information
As a transport segment user
I want to check consistency

@testcase=9906 @version=5 @prodready
Scenario: Verify when there are no unbalances greater than acceptable balance percentage
	Given I am logged in as "admin"
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
	Then validate that "ErrorsGrid" "Submit" "button" is "enabled"
	When I click on "ErrorsGrid" "Submit" "button"
	And I have no unbalances greater than acceptable balance percentage
	Then I should see message "Sin registros"
	And validate that "unbalancesGrid" "submit" "button" as enabled

@testcase=9907 @bvt @version=5 @prodready
Scenario: Verify when there are unbalances greater than acceptable balance percentage
	Given I want to calculate the "OperationalBalance" in the system
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
	Then validate that "ErrorsGrid" "Submit" "button" as enabled
	When I click on "ErrorsGrid" "Submit" "button"
	Then I should see unbalances of the nodes that exceed the "Acceptable Balance Percentage" value configured for each node
	And I should see all the column data related to it
	Then validate that "unbalancesGrid" "submit" "button" as disabled

@testcase=9908 @version=5 @prodready
Scenario: Verify manage unbalances without entering the note
	Given I want to calculate the "OperationalBalance" in the system
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
	Then validate that "ErrorsGrid" "Submit" "button" as enabled
	When I click on "ErrorsGrid" "Submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	Then I should see "Note for unbalances" interface
	When I click on "AddComment" "submit" "button"
	Then I should see error message "La nota es requerida"

@testcase=9909 @version=5 @prodready
Scenario: Verify manage unbalances with note exceeding maximum limit
	Given I am having pending records in Operational Cutoff page
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
	Then validate that "ErrorsGrid" "Submit" "button" as enabled
	When I click on "ErrorsGrid" "Submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	Then I should see "Note for unbalances" interface
	When I enter morethan 1000 characters into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "submit" "button"
	Then I should see error message "La nota puede contener máximo 1000 caracteres"

@testcase=9910 @bvt @version=4
Scenario: Verify manage unbalances
	Given I am having pending records in Operational Cutoff page
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	Then I should see "Start" "link"
	When I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "2" days from CurrentDate on "Cutoff" DatePicker
	When I click on "InitTicket" "submit" "button"
	And I select all pending records from grid
	And I click on "Notes" "button"
	Then I should see "Add Note Functions" interface
	When I enter valid value into "Agregar Nota" "textbox"
	And I click on "Save" "button"
	Then I should see "ErrorsGrid" "Submit" "button" as enabled
	When I click on "ErrorsGrid" "Submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	Then I should see "Note for unbalances" interface
	When I provide value for "AddComment" "comment" "textbox"
	And I click on "AddComment" "submit" "button"
	Then I should see the unbalances updated with the information added in the note
	And the list should be refreshed so that the managed unbalances are no longer displayed