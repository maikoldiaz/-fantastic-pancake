@owner=jagudelos @ui @testsuite=55116 @testplan=55104 @MVP2and3 @miss
Feature: OperationalCutoffToIncludeOfficialTransferPointValidationStep
In order to verify official transfer points
As a professional user
I need operational cutoff steps to be modified

@testcase=56874
Scenario: Validate transfer point movements displayed in the official transfer points having no GlobalMovementId
Given I am logged in as "profesional"
Given I have transfer point movements without update or delete events and without having GlobalMovementId
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the StartDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And validate that "validateInitialInventory" "submit "button" as enabled
And I click on "validateInitialInventory" "submit "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
Then validate that "VerifyOfficialPoints" "link" wizard is "active"
And validate transfer point movements without GlobalMovementId are displayed in "OfficialPoints" grid
And validate "Debe confirmar los puntos oficiales" is displayed in "OfficialPointsGrid" "InfoMessage" "message"
And validate that "OfficialPointsGrid" "AddNote" "button" as disabled
And I select all transfer point in the grid
And validate that "OfficialPointsGrid" "AddNote" "button" as enabled
And validate that "officialPointsGrid" "submit" "button" as disabled

@testcase=56875
Scenario: Validate transfer point movements displayed in the official transfer points having update events and no GlobalMovementId
Given I am logged in as "profesional"
Given I have transfer point movements with update events and without having GlobalMovementId
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the StartDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And validate that "validateInitialInventory" "submit "button" as enabled
And I click on "validateInitialInventory" "submit "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
Then validate that "VerifyOfficialPoints" "link" wizard is "active"
And validate transfer point movements last updated record without GlobalMovementId are displayed in "OfficialPoints" grid
And validate "Debe confirmar los puntos oficiales" is displayed in "OfficialPointsGrid" "InfoMessage" "message"
And validate that "OfficialPointsGrid" "AddNote" "button" as disabled
And I select all transfer point in the grid
And validate that "OfficialPointsGrid" "AddNote" "button" as enabled
And validate that "officialPointsGrid" "submit" "button" as disabled

@testcase=56876
Scenario: Validate transfer point movements displayed in the official transfer points having delete events and no GlobalMovementId
Given I am logged in as "profesional"
Given I have transfer point movements with delete events and without having GlobalMovementId
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the StartDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And validate that "validateInitialInventory" "submit "button" as enabled
And I click on "validateInitialInventory" "submit "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
Then validate that "VerifyOfficialPoints" "link" wizard is "active"
And I should see message "Sin registros"
And validate that "OfficialPointInfoMessage" is not displayed
And validate that "OfficialPointsGrid" "AddNote" "button" as disabled
And validate that "officialPointsGrid" "submit" "button" as enabled

@testcase=56877 @BVT2
Scenario: Validate transfer point movements displayed in the official transfer points having update events and insert event has GlobalMovementId
Given I am logged in as "profesional"
Given I have transfer point movements with update events and insert event having GlobalMovementId
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the StartDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And validate that "validateInitialInventory" "submit "button" as enabled
And I click on "validateInitialInventory" "submit "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
Then validate that "VerifyOfficialPoints" "link" wizard is "active"
And I should see message "Sin registros"
And validate that "OfficialPointInfoMessage" is not displayed
And validate that "OfficialPointsGrid" "AddNote" "button" as disabled
And validate that "officialPointsGrid" "submit" "button" as enabled

@testcase=56878 @BVT2
Scenario: Validate transfer point movements displayed in the official transfer points having update events and update event has GlobalMovementId
Given I am logged in as "profesional"
Given I have transfer point movements with update events and update event having GlobalMovementId
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the StartDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And validate that "validateInitialInventory" "submit "button" as enabled
And I click on "validateInitialInventory" "submit "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
Then validate that "VerifyOfficialPoints" "link" wizard is "active"
And I should see message "Sin registros"
And validate that "OfficialPointInfoMessage" is not displayed
And validate that "OfficialPointsGrid" "AddNote" "button" as disabled
And validate that "officialPointsGrid" "submit" "button" as enabled

@testcase=56879 @BVT2
Scenario: Validate add note in the official transfer points
Given I am logged in as "profesional"
Given I have transfer point movements without update or delete events and without having GlobalMovementId
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the StartDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And validate that "validateInitialInventory" "submit "button" as enabled
And I click on "validateInitialInventory" "submit "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
Then validate that "VerifyOfficialPoints" "link" wizard is "active"
And validate transfer point movements without GlobalMovementId are displayed in "OfficialPoints" grid
And validate "Debe confirmar los puntos oficiales" is displayed in "OfficialPointsGrid" "InfoMessage" "message"
And validate that "officialPointsGrid" "submit" "button" as disabled
And validate that "OfficialPointsGrid" "AddNote" "button" as disabled
And I select all transfer point in the grid
And validate that "OfficialPointsGrid" "AddNote" "button" as enabled
And I click on "OfficialPointsGrid" "AddNote" "button"
And I should see "Modal" "CommonAddComment" "container"
And I click on "AddComment" "submit" "button"
And I should see error message "Requerido"
And I click on "AddComment" "cancel" "button"
And validate transfer point movements without GlobalMovementId are displayed in "OfficialPoints" grid
And I unselected all transfer point in the grid
And validate "Debe confirmar los puntos oficiales" is displayed in "OfficialPointsGrid" "InfoMessage" "message"
And validate that "officialPointsGrid" "submit" "button" as disabled
And validate that "OfficialPointsGrid" "AddNote" "button" as disabled
And I select all transfer point in the grid
And validate that "OfficialPointsGrid" "AddNote" "button" as enabled
And I click on "OfficialPointsGrid" "AddNote" "button"
And I should see "Modal" "CommonAddComment" "container"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
And validate that "OfficialPointsGrid" "AddNote" "button" as disabled
And I should see message "Sin registros"
And validate that "OfficialPointInfoMessage" is not displayed
And validate that "officialPointsGrid" "submit" "button" as enabled

@testcase=56880
Scenario: Validate movement are marked as official and notes added in the step transfer point when user executes cutoff
Given I am logged in as "profesional"
Given I have transfer point movements without update or delete events and without having GlobalMovementId
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the StartDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And validate that "validateInitialInventory" "submit "button" as enabled
And I click on "validateInitialInventory" "submit "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
Then validate that "VerifyOfficialPoints" "link" wizard is "active"
And validate transfer point movements without GlobalMovementId are displayed in "OfficialPoints" grid
And validate "Debe confirmar los puntos oficiales" is displayed in "OfficialPointsGrid" "InfoMessage" "message"
And validate that "officialPointsGrid" "submit" "button" as disabled
And validate that "OfficialPointsGrid" "AddNote" "button" as disabled
And I select all transfer point in the grid
And validate that "OfficialPointsGrid" "AddNote" "button" as enabled
And I click on "OfficialPointsGrid" "AddNote" "button"
And I should see "Modal" "CommonAddComment" "container"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
And validate that "OfficialPointsGrid" "AddNote" "button" as disabled
And I should see message "Sin registros"
And validate that "OfficialPointInfoMessage" is not displayed
And validate that "officialPointsGrid" "submit" "button" as enabled
And I click on "officialPointsGrid" "submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
And I click on "unbalancesGrid" "submit" "button"
And I click on "ConfirmCutoff" "Submit" "button"
And I wait till cutoff ticket processing to complete
And validate movement are marked as official and notes added

@testcase=56881 @BVT2
Scenario: Validate user clicks on cancel button in transfer points step
Given I am logged in as "profesional"
Given I have transfer point movements without update or delete events and without having GlobalMovementId
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the StartDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And validate that "validateInitialInventory" "submit "button" as enabled
And I click on "validateInitialInventory" "submit "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
And validate that "VerifyOfficialPoints" "link" wizard is "active"
And validate transfer point movements without GlobalMovementId are displayed in "OfficialPoints" grid
Then I click on "officialPointsGrid" "Cancel" "button"
And validate that "Start" "link" wizard is "active"
And I should see the default value for "Segment" dropdown is "Seleccionar"
And validate that "initTicket" "initialDate" "date" is selected with current date minus one
And validate that "initTicket" "finalDate" "date" is empty
@testcase=56882 
Scenario: Validate transfer point movement error reported by SAP PO
Given I am logged in as "profesional"
Given I have transfer point movements without update or delete events and without having GlobalMovementId
And I have transfer point movement with error reported by SAP PO
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the StartDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And validate that "validateInitialInventory" "submit "button" as enabled
And I click on "validateInitialInventory" "submit "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
And validate that "VerifyOfficialPoints" "link" wizard is "active"
Then validate transfer point movements without GlobalMovementId are displayed in "OfficialPoints" grid
And Validate transfer point movement with error reported by SAP PO are displayed in "OfficialPoints" grid
And validate that error icon is enabled for movement with error reported by SAP PO in "OfficialPoints" grid
And validate that error icon is disabled for transfer point movements without error reported by SAP PO and without GlobalMovementId in "OfficialPoints" grid
And I click on error icon movement with error reported by SAP PO in "OfficialPoints" grid
And I should see "Modal" "ShowOfficialPointsError" "container"
And validate message "Error al conciliar el punto de transferencia" in "Modal" "Title" "header"
And validate the selected "MovementId" in "OfficialPointsCommentMessage" "movementIdValue" "label"
And validate the selected "MovementType" in "OfficialPointsCommentMessage" "movementTypeName" "label"
And validate the selected "OperationalDate" in "OfficialPointsCommentMessage" "operationalDateValue" "label"
And validate the selected "ErrorMessage" in "OfficialPointsCommentMessage" "ErrorMessage" "label"

@testcase=56883
Scenario: Validate transfer point movements displayed in the official transfer points having multiple events and any of event has GlobalMovementId
Given I am logged in as "profesional"
Given I have transfer point movements with multiple events and any event having GlobalMovementId
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the StartDate lessthan "3" days from CurrentDate on "Cutoff" DatePicker
And I select the EndDate lessthan "1" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And validate that "validateInitialInventory" "submit "button" as enabled
And I click on "validateInitialInventory" "submit "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
And validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
Then validate that "VerifyOfficialPoints" "link" wizard is "active"
And validate transfer point movements without GlobalMovementId are displayed in "OfficialPoints" grid
And validate transfer point movement with GlobalMovementId for any of event is not displayed in "OfficialPoints" grid
And validate "Debe confirmar los puntos oficiales" is displayed in "OfficialPointsGrid" "InfoMessage" "message"
And validate that "OfficialPointsGrid" "AddNote" "button" as disabled
And I select all transfer point in the grid
And validate that "OfficialPointsGrid" "AddNote" "button" as enabled
And validate that "officialPointsGrid" "submit" "button" as disabled
