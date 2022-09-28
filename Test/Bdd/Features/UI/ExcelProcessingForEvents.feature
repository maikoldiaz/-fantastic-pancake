@sharedsteps=4013 @owner=jagudelos @ui @testplan=19772 @testsuite=19786
Feature: ExcelProcessingForEvents
In order to register the information of planning, programming and collaboration agreements
As a True user
I want to upload the Excel files from UI

Background: Login
	Given I am logged in as "admin"

@testcase=21214 @bvt @prodready
Scenario: Upload event excel with action type Insert
	And I have "Event" homologation data in the system
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	Then I should see upload new file interface
	When I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "<FileType>" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "Event" "Inserted" in the system

@testcase=21215 @bvt @prodready
Scenario: Upload event excel with action type Update
	And I have "Event" information in the system
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	Then I should see upload new file interface
	When I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Update" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "UpdateExcel" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "Event" "Updated" in the system

@testcase=21216 @bvt @prodready
Scenario: Upload event excel with action type Delete
	And I have "Event" information in the system
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	Then I should see upload new file interface
	When I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Delete" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "<FileType>" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "Event" "Deleted" in the system

@testcase=21217 @prodready @version=2
Scenario: Verify the message when active events already exists in the system
	And I have "Event" information in the system
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	Then I should see upload new file interface
	When I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "<FileType>" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see "El evento ya existe" in PendingTransactions for "Event"

@testcase=21218 @prodready @version=2
Scenario Outline: Verify the message when no active events exists in the system
	And I have "Event" information in the system
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	Then I should see upload new file interface
	When I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "<Action>" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "<FileType>" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see "El evento no existe" in PendingTransactions for "Event"

	Examples:
		| Action |
		| Update |
		| Delete |

@testcase=21219 @prodready @version=2
Scenario: Verify the message when active events period already exists in the system
	And I have "Event" information in the system
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	Then I should see upload new file interface
	When I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "PeriodOverlap" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see "Ya existe un evento que contiene el periodo" in PendingTransactions for "Event"

@testcase=21220 @manual
Scenario Outline: Verify the validations messages registered in the system
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	Then I should see upload new file interface
	When I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "InvalidExcel" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "<Message> in the system

	Examples:
		| Message                                                 |
		| El evento de propiedad es obligatorio                   |
		| El nodo origen es obligatorio                           |
		| Identificador del nodo origen no encontrado             |
		| El nodo destino es obligatorio                          |
		| Identificador del nodo destino no encontrado            |
		| El producto origen es obligatorio                       |
		| Identificador del producto origen no encontrado         |
		| El producto destino es obligatorio                      |
		| Identificador del producto destino no encontrado        |
		| La fecha inicial es obligatoria                         |
		| La fecha final es obligatoria                           |
		| La fecha final debe ser mayor a la fecha inicial        |
		| La fecha final debe ser mayor o igual a la fecha actual |
		| El propietario es obligatorio                           |
		| Identificador del propietario no encontrado             |
		| El valor del evento es obligatorio                      |
		| La unidad es obligatoria                                |
		| Identificador de la unidad no encontrado                |