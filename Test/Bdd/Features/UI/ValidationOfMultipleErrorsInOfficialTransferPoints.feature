@owner=jagudelos @ui @testsuite=74721 @testplan=74716 @MVP2and3
Feature: ValidationOfMultipleErrorsInOfficialTransferPoints
In Order to display multiple errors in the verification of official transfer points
As a professional segment balance user
I need the operational cut to be modified

@manual
Scenario: Validate Web API Client should be modified which includes the service contract changes for the Mult
	Given I am logged in as "profesional"
	And I have transfer point movements without update or delete events and without having GlobalMovementId
	When I have transfer point movement with error reported by SAP PO
	Then Verify that system should process the service response and it should interpret the error property as an array of errors based on the contract
	And Verify that system should store the error reported by SAP PO for future purpose.

@bvt
Scenario: Validate that system should display a list of errors instead of a single transfer point movement error reported by SAP PO
	And I have ownershipnodes created for node status having movements with transfer point
	And I have "Excel" homologation data in the system
	And I have "TestData_TransferPoint" excel having movements with transfer point
	And I upload the excel into the system
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
	And validate that error icon is disabled for transfer point movements without error reported by SAP PO
	And I click on error icon movement with error reported by SAP PO in "OfficialPoints" grid
	And I should see "Modal" "ShowOfficialPointsError" "container"
	And validate message "Error al conciliar el punto de transferencia" in "Modal" "Title" "header"
	And validate the selected "MovementId" in "OfficialPointsCommentMessage" "movementIdValue" "label"
	And validate the selected "MovementType" in "OfficialPointsCommentMessage" "movementTypeName" "label"
	And validate the selected "OperationalDate" in "OfficialPointsCommentMessage" "operationalDateValue" "label"
	And validate the selected "ErrorMessage" in "OfficialPointsCommentMessage" "ErrorMessage" "label"
	And Validate that a label with the text "Total Records:" and the value of the number of errors for the movement should be displayed
	And Validate that A table or grid will present the with error code,description of error sent by SAP sorted by error code in ascending order
	And Validate that system should display Display a list of errors when the movement has associated errors

Scenario: Validate that system should display errors when technical exceptions error reported by SAP PO
	And I have ownershipnodes created for node status having movements with transfer point
	And I have "Excel" homologation data in the system
	And I have "TestData_TransferPoint" excel having movements with transfer point
	And I upload the excel into the system
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
	And validate that error icon is disabled for transfer point movements without error reported by SAP PO
	And I click on error icon movement with error reported by SAP PO in "OfficialPoints" grid
	And I should see the "Se presentó un error técnico inesperado en el llamado al servicio de puntos de transferencia." error message.