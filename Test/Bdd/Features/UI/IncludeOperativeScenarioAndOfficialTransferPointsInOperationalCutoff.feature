@sharedsteps=16581 @owner=jagudelos @ui @testplan=55104 @testsuite=55113 @MVP2and3 @S14
Feature: IncludeOperativeScenarioAndOfficialTransferPointsInOperationalCutoff
In order to use movements of the operative scenario and official transfer points
As a Balance Segment Professional User
I need the process of operational cutoff to be modified

Background: Login
	Given I am logged in as "profesional"

@testcase=56855 @version=2 @BVT2
Scenario: Verify that system should mark as official point for the movements that have a note assigned in the transfer points step of the wizard
	And I have ownershipnodes created for node status having movements with transfer point
	And I have "Excel" homologation data in the system
	And I have "TestData_TransferPoint" excel having movements with transfer point
	And I upload the excel into the system
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "validateInitialInventory" "submit "button" as enabled
	And I click on "validateInitialInventory" "submit "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "ErrorsGrid" "Submit" "button" as enabled
	And I click on "ErrorsGrid" "Submit" "button"
	Then Verify that TransferPoint movements without a global identifier should be displayed in the grid
	And I select all pending records from grid
	And I click on "officialPointsGrid" "addNote" "button"
	And I enter "This is TransferPoint Movement" into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "officialPointsGrid" "submit" "button" as enabled
	And I click on "officialPointsGrid" "submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	And I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "ConfirmCutoff" "Submit" "button"
	And I wait till cutoff ticket processing to complete
	Then Verify that Note should be assigned to all the selected Transfer point movements from the grid
	And Verify that Ticket Number should be assigned to all the movements
	And It should mark as official points for all the movements

@testcase=56856 @version=2
Scenario: Verify that system should exclude the transfer point movements belong to the official scenario during the operational cutoff calculations
	And I have ownershipnodes created for node status having movements with transfer point
	And I have "Excel" homologation data in the system
	And I have "TestData_TransferPoint" excel having movements with transfer point
	And I upload the excel into the system
	And I have official transfer point movements belongs official scenario (scenario id two) and have a global identifier
	And Operational date equal to the date of the period day
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "validateInitialInventory" "submit "button" as enabled
	And I click on "validateInitialInventory" "submit "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "ErrorsGrid" "Submit" "button" as enabled
	And I click on "ErrorsGrid" "Submit" "button"
	Then the movements with global identifier should not be there on this list
	And I select all pending records from grid
	And I click on "officialPointsGrid" "addNote" "button"
	And I enter "This is TransferPoint Movement" into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "officialPointsGrid" "submit" "button" as enabled
	And I click on "officialPointsGrid" "submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	And I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "ConfirmCutoff" "Submit" "button"
	And I wait till cutoff ticket processing to complete
	Then Verify that the transfer point movements belong to the official scenario should not be taken into account in calculations of operational cutoff

@testcase=56857 @version=2
Scenario: Verify that system should consider the transfer point movements belong to the operative scenario during the operational cutoff calculations
	And I have ownershipnodes created for node status having movements with transfer point
	And I have "Excel" homologation data in the system
	And I have "TestData_TransferPoint" excel having movements with transfer point
	And I upload the excel into the system
	And I have official transfer point movements belongs operative scenario (scenario id one) and have a global identifier
	And  Operational date equal to the date of the period day
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "validateInitialInventory" "submit "button" as enabled
	And I click on "validateInitialInventory" "submit "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "ErrorsGrid" "Submit" "button" as enabled
	And I click on "ErrorsGrid" "Submit" "button"
	Then the movements with global identifier should not be there on this list
	And I select all pending records from grid
	And I click on "officialPointsGrid" "addNote" "button"
	And I enter "This is TransferPoint Movement" into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "officialPointsGrid" "submit" "button" as enabled
	And I click on "officialPointsGrid" "submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	And I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "ConfirmCutoff" "Submit" "button"
	And I wait till cutoff ticket processing to complete
	Then Verify that the system must assign the corresponding ticket number for all these official transfer point movements

@testcase=56858 @version=2
Scenario: Verify that system should consider the official transfer point movements have insert and update event during the operational cutoff calculations
	And I have ownershipnodes created for node status having movements with transfer point
	And I have "Excel" homologation data in the system
	And I have "TestData_TransferPoint" excel having movements with transfer point
	And I upload the excel into the system
	And  Operational date equal to the date of the period day
	Given I have official transfer point movements have insert and update event records belongs operative scenario (scenario id one) and have a global identifier
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "validateInitialInventory" "submit "button" as enabled
	And I click on "validateInitialInventory" "submit "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "ErrorsGrid" "Submit" "button" as enabled
	And I click on "ErrorsGrid" "Submit" "button"
	And I select all pending records from grid
	And I click on "officialPointsGrid" "addNote" "button"
	And I enter "This is TransferPoint Movement" into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "officialPointsGrid" "submit" "button" as enabled
	And I click on "officialPointsGrid" "submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	And I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "ConfirmCutoff" "Submit" "button"
	And I wait till cutoff ticket processing to complete
	Then Verify that the system must assign the corresponding ticket number for all these official transfer point have insert and update event records movements

@testcase=56859 @version=2
Scenario: Verify that system should exclude the official transfer point movements with update event but do not have global identifier during the operational cutoff calculations
	And I have ownershipnodes created for node status having movements with transfer point
	And I have "Excel" homologation data in the system
	And I have "TestData_TransferPoint" excel having movements with transfer point
	And I upload the excel into the system
	And Operational date equal to the date of the period day
	Given I have official transfer point movements have update event records belongs operative scenario (scenario id one) but do not have global identifier
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "validateInitialInventory" "submit "button" as enabled
	And I click on "validateInitialInventory" "submit "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "ErrorsGrid" "Submit" "button" as enabled
	And I click on "ErrorsGrid" "Submit" "button"
	And I select all pending records from grid
	And I click on "officialPointsGrid" "addNote" "button"
	And I enter "This is TransferPoint Movement" into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "officialPointsGrid" "submit" "button" as enabled
	And I click on "officialPointsGrid" "submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	And I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "ConfirmCutoff" "Submit" "button"
	And I wait till cutoff ticket processing to complete
	Then Verify that these movements records that do not have a global identifier must be without a ticket number

@testcase=56861 @version=2 @BVT2
Scenario: Verify that system should not consider the movements and inventories belong to the official scenario during the operational cutoff calculations
	And I have ownershipnodes created for node status having movements with transfer point
	And I have "Excel" homologation data in the system
	And I have "TestData_TransferPoint" excel having movements with transfer point
	And I upload the excel into the system
	Given I have movements of the official scenario belongs to the selected segment with an operational date equal to the date of the period day
	And I have inventories of the official scenario belongs to the selected segment with an inventory date of previous day and the last day of the period
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "validateInitialInventory" "submit "button" as enabled
	And I click on "validateInitialInventory" "submit "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "ErrorsGrid" "Submit" "button" as enabled
	And I click on "ErrorsGrid" "Submit" "button"
	Then the movements with global identifier should not be there on this list
	And I select all pending records from grid
	And I click on "officialPointsGrid" "addNote" "button"
	And I enter "This is TransferPoint Movement" into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "officialPointsGrid" "submit" "button" as enabled
	And I click on "officialPointsGrid" "submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	And I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "ConfirmCutoff" "Submit" "button"
	And I wait till cutoff ticket processing to complete
	Then Verify that the system should not be taken into account in the calculations of the operational cutoff for the official scenario movements and inventories

@testcase=56862
Scenario: Verify that system should consider the movements and inventories belong to the operative scenario during the operational cutoff calculations
	And I have ownershipnodes created for node status having movements with transfer point
	And I have "Excel" homologation data in the system
	And I have "TestData_TransferPoint" excel having movements with transfer point
	And I upload the excel into the system
	Given I have movements of the operative scenario, which are NOT transfer points belongs to the selected segment, with an operational date equal to the date of the period day
	And I have inventories of the operative  scenario belongs to the selected segment, with an inventory date of previous day and the last day of the period
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "validateInitialInventory" "submit "button" as enabled
	And I click on "validateInitialInventory" "submit "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "ErrorsGrid" "Submit" "button" as enabled
	And I click on "ErrorsGrid" "Submit" "button"
	Then the movements with global identifier should not be there on this list
	And I select all pending records from grid
	And I click on "officialPointsGrid" "addNote" "button"
	And I enter "This is TransferPoint Movement" into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "officialPointsGrid" "submit" "button" as enabled
	And I click on "officialPointsGrid" "submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	And I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "ConfirmCutoff" "Submit" "button"
	And I wait till cutoff ticket processing to complete
	Then Verify that the system must take into account these movements and inventories and assign them the corresponding ticket number

@testcase=56863 @version=2
Scenario: Verify that True must calculate the unbalances of the nodes for Transfer point movements with a note assigned during the operational cutoff calculations
	And I have ownershipnodes created for node status having movements with transfer point
	And I have "Excel" homologation data in the system
	And I have "TestData_TransferPoint" excel having movements with transfer point
	And I upload the excel into the system
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "validateInitialInventory" "submit "button" as enabled
	And I click on "validateInitialInventory" "submit "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "ErrorsGrid" "Submit" "button" as enabled
	And I click on "ErrorsGrid" "Submit" "button"
	And I select all pending records from grid
	And I click on "officialPointsGrid" "addNote" "button"
	And I enter "This is TransferPoint Movement" into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "officialPointsGrid" "submit" "button" as enabled
	And I click on "officialPointsGrid" "submit" "button"
	Then Verify that the system must calculate the unbalances of the nodes for the above mentioned selected movements

Scenario: Verify that True must calculate the unbalances of the nodes for the Movements which are marked as official points and have a global identifier during the operational cutoff calculations
	And I have ownershipnodes created for node status having movements with transfer point
	And I have "Excel" homologation data in the system
	And I have "TestData_TransferPoint" excel having movements with transfer point
	And I upload the excel into the system
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "validateInitialInventory" "submit "button" as enabled
	And I click on "validateInitialInventory" "submit "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "ErrorsGrid" "Submit" "button" as enabled
	And I click on "ErrorsGrid" "Submit" "button"
	And I select all pending records from grid
	And I click on "officialPointsGrid" "addNote" "button"
	And I enter "This is TransferPoint Movement" into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "officialPointsGrid" "submit" "button" as enabled
	And I click on "officialPointsGrid" "submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	And I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "ConfirmCutoff" "Submit" "button"
	And I wait till cutoff ticket processing to complete
	Then Verify that the system must calculate the unbalances of the nodes for the above mentioned selected movements and stored in the Databases

Scenario: Verify that True must calculate the unbalances of the nodes for the backup Movements which are belong to same segment and have a global identifier during the operational cutoff calculations
	And I have ownershipnodes created for node status having movements with transfer point
	And I have "Excel" homologation data in the system
	And I have "TestData_TransferPoint" excel having movements with transfer point
	And I upload the excel into the system
	And Backing movement of these  movements should belongs to the selected segment.
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "validateInitialInventory" "submit "button" as enabled
	And I click on "validateInitialInventory" "submit "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "ErrorsGrid" "Submit" "button" as enabled
	And I click on "ErrorsGrid" "Submit" "button"
	And I select all pending records from grid
	And I click on "officialPointsGrid" "addNote" "button"
	And I enter "This is TransferPoint Movement" into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "officialPointsGrid" "submit" "button" as enabled
	And I click on "officialPointsGrid" "submit" "button"
	And I select all unbalances in the grid
	And I click on "consistencyCheck" "AddNote" "button"
	And I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "unbalancesGrid" "submit" "button"
	And I click on "ConfirmCutoff" "Submit" "button"
	And I wait till cutoff ticket processing to complete
	Then Verify that the system must calculate the unbalances of the nodes for the above mentioned selected movements and stored in the Databases

@testcase=56864 @version=2 @BVT2
Scenario: Verify that True must calculate the unbalances of the nodes for the operative scenario movement and inventories during the operational cutoff calculations
	And I have ownershipnodes created for node status having movements with transfer point
	And I have "Excel" homologation data in the system
	And I have "TestData_TransferPoint" excel having movements with transfer point
	And I upload the excel into the system
	Given I have movements of the operative scenario, which are NOT transfer points belongs to the selected segment, with an operational date equal to the date of the period day
	And I have inventories of the operative  scenario belongs to the selected segment, with an inventory date of previous day and the last day of the period
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "validateInitialInventory" "submit "button" as enabled
	And I click on "validateInitialInventory" "submit "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	When I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "ErrorsGrid" "Submit" "button" as enabled
	And I click on "ErrorsGrid" "Submit" "button"
	And I select all pending records from grid
	And I click on "officialPointsGrid" "addNote" "button"
	And I enter "This is TransferPoint Movement" into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "officialPointsGrid" "submit" "button" as enabled
	And I click on "officialPointsGrid" "submit" "button"
	Then Verify that the system must calculate the unbalances of the nodes for the above mentioned selected movements