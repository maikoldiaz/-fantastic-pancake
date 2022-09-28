@sharedsteps=4013 @owner=jagudelos @ui @testplan=35673 @testsuite=35676 @parallel=false
Feature: AdjustmentsToEventsAndContractExcelUpload
As TRUE system, I need some adjustments in process of an Excel file with
purchases and sales contracts and Events to register them in the system

Background: Login
	Given I am logged in as "admin"

@testcase=37297 @version=2 @bvt1.5
Scenario: Verify when TRUE User uploads an excel with valid SalesAndPurchase data
	Given I have "SalesAndPurchase" homologation data in the system
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "ValidExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "Contracts" "Inserted" in the system

@testcase=37298 @version=2
Scenario Outline: Verify when TRUE user uploads SalesAndPurchase excel with missing owner field
	Given I have "SalesAndPurchase" homologation data in the system
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "FileUpload" "button"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "WithoutOnwerExcel" file from purchase sales directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see "<Message>" in PendingTransactions for "Event"

	Examples:
		| Message                         |
		| El propietario 1 es obligatorio |
		| El propietario 2 es obligatorio |

@testcase=37299 @version=2
Scenario Outline: Verify when TRUE user uploads SalesAndPurchase contract excel without ownerid field
	Given I have "SalesAndPurchase" homologation data in the system
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "FileUpload" "button"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "ValidExcel" file from purchase sales directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see "<Message>" in PendingTransactions

	Examples:
		| Message                                       |
		| Identificador del propietario 1 no encontrado |
		| Identificador del propietario 2 no encontrado |

@testcase=37300 @version=3
Scenario: Verify when TRUE user uploads a SalesAndPurchase contract excel which has already been processed
	Given I have "SalesAndPurchase" homologation data in the system
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "ValidExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I should see "El contracto ya existe" in PendingTransactions for "Event"

@testcase=37301 @version=2 @bvt1.5
Scenario: Verify when TRUE user uploads an SalesAndPurchase contract excel to update existing contracts
	Given I have "SalesAndPurchase" homologation data in the system
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Update" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "UpdateExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "Contracts" "Updated" in the system

@testcase=37302 @version=3
Scenario: : Verify when TRUE user uploads an excel to update existing SalesAndPurchase contracts with no previous data
	Given I have "SalesAndPurchase" homologation data in the system
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Update" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "UpdateExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I should see "El contrato no existe" in PendingTransactions for "Event"

@testcase=37303 @verion=2 @bvt1.5
Scenario: Verify TRUE user is able to delete existing SalesAndPurchase contracts from system
	Given I have "Contract" datas in the system
	And I update data in "DeleteContractExcel"
	When I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Delete" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "DeleteContractExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "Contract" "Deleted" in the system

@testcase=37304 @version=3
Scenario: Verify the error message when TRUE user tries to delete SalesAndPurchase contract not in the system
	Given I have "SalesAndPurchase" homologation data in the system
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Delete" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "ValidExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I should see "El contrato no existe" in PendingTransactions for "Event"

@testcase=37305 @version=3
Scenario: Verify the message when active SalesAndPurchase contracts period already exists in the system
	Given I have "SalesAndPurchase" homologation data in the system
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Delete" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "PeriodOverlop" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I should see "Ya existe un contrato que contiene el periodo" in PendingTransactions for "Event"

@testcase=37306 @version=2 @bvt1.5
Scenario: Upload event excel with action type Insert
	Given I have "Event" homologation data in the system
	And I update data in "InsertEventExcel"
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	Then I should see upload new file interface
	When I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "InsertEventExcel" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "Event" "Inserted" in the system

@testcase=37307 @version=2 @bvt1.5
Scenario: Upload event excel with action type Update
	Given I have "ValidEvent" information in the system
	And I update data in "UpdateEventExcel"
	When I click on "LoadNew" "button"
	Then I should see upload new file interface
	When I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Update" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "UpdateEventExcel" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "Event" "Updated" in the system

@testcase=37308 @version=2 @bvt1.5
Scenario: Upload event excel with action type Delete
	Given I have "ValidEvent" information in the system
	And I update data in "DeleteEventExcel"
	When I click on "LoadNew" "button"
	Then I should see upload new file interface
	When I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Delete" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "DeleteEventExcel" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "Event" "Deleted" in the system

@testcase=37309 @version=3
Scenario: Verify the message when active events already exists in the system
	Given I have "Event" homologation data in the system
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	And I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "ValidExcel" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see "El evento ya existe" in PendingTransactions for "Event"

@testcase=37310 @version=3
Scenario Outline: Verify the message when no active events exists in the system
	Given I have "Event" homologation data in the system
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	And I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "<Action>" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "ValidExcel" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see "El evento no existe" in PendingTransactions for "Event"

	Examples:
		| Action |
		| Update |
		| Delete |

@testcase=37311 @version=3
Scenario: Verify the message when active events period already exists in the system
	Given I have "Event" homologation data in the system
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	And I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "PeriodOverlap" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see "Ya existe un evento que contiene el periodo" in PendingTransactions for "Event"

@testcase=37312 @version=3
Scenario: Scenario Outline: Verify when TRUE user uploads an events excel with missing owner field
	Given I have "Event" homologation data in the system
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	And I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Delete" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "WithoutRecords" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see "Identificador del propietario 2 no encontrado" in PendingTransactions for "Event"

@testcase=37313 @version=3
Scenario: Scenario Outline: Verify when TRUE user uploads an events excel without ownerid field
	Given I have "Event" homologation data in the system
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	And I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Delete" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "WithoutRecords" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see "El propietario 2 es obligatorio" in PendingTransactions for "Event"