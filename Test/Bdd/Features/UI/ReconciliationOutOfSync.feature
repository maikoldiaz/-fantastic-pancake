@owner=jagudelos @testplan=39221 @testsuite=39239 @parallel=false
Feature: ReconciliationOutOfSync
In order to avoid out of sync between database and block chain
As an Operations-user
I want to have a mechanism to reconcile

@testcase=41776 @backend
Scenario: Register a movement with valid data through SINOPER
	Given I want to register an "Movements" in the system
	When it meets "all" input validations
	And the "EventType" field is equal to "Insert"
	Then it should be registered

@testcase=41777 @backend
Scenario: Update a movement through SINOPER
	Given I want to adjust an "Movements"
	And I provide "EventType" field is equal to "Update"
	Then record a new "Movements" with negative values for the "ProductVolume"
	And it should be registered

@testcase=41778 @backend
Scenario: Delete a movement through SINOPER
	Given I want to cancel an "Movements"
	And I provide "EventType" field is equal to "Delete"
	Then record a new "Movements" with negative values for the "ProductVolume"
	And it should be registered

@testcase=41779 @backend
Scenario: Register a movement with existing MovementId through SINOPER
	Given I have "Movements" in the system through SINOPER
	When I register with same data
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del movimiento ya existe en el sistema​"

@testcase=41780 @backend
Scenario: Update a movement with MovementId does not exist through SINOPER
	Given I want to adjust an "Movements"
	When the 'MovementIdentifier' does not exist and "EventType" field is equal to "Update"
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del movimiento a ajustar no existe"

@testcase=41781 @backend
Scenario: Delete a movement with MovementId does not exist through SINOPER
	Given I want to adjust an "Movements"
	When the 'MovementIdentifier' does not exist and "EventType" field is equal to "Delete"
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del movimiento a anular no existe"

@testcase=41782 @backend
Scenario: Register an inventory with valid data through SINOPER
	Given I want to register an "Inventory" in the system
	When it meets "all" input validations
	And the "EventType" field is equal to "Insert"
	Then it should be registered

@testcase=41783 @backend
Scenario: Update an inventory through SINOPER
	Given I want to adjust an "Inventory"
	And I provide "EventType" field is equal to "Update"
	Then record a new "Inventory" with negative values for the "ProductVolume"
	And it should be registered

@testcase=41784 @backend
Scenario: Delete an inventory through SINOPER
	Given I want to cancel an "Inventory"
	And I provide "EventType" field is equal to "Delete"
	Then record a new "Inventory" with negative values for the "ProductVolume"
	And it should be registered

@testcase=41785 @backend
Scenario: Register an inventory with existing InventoryId through SINOPER
	Given I have "Inventory" in the system through SINOPER
	When I register with same data
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario ya existe en el sistema​"

@testcase=41786 @backend
Scenario: Update an inventory with InventoryId does not exist through SINOPER
	Given I want to cancel an "Inventory"
	When the 'InventoryIdentifier' does not exist and "EventType" field is equal to "Update"
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a ajustar no existe"

@testcase=41787 @backend
Scenario: Delete an inventory with InventoryId does not exist through SINOPER
	Given I want to cancel an "Inventory"
	When the 'InventoryIdentifier' does not exist and "EventType" field is equal to "Delete"
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a anular no existe"

@testcase=41788 @api @version=2 @bvt1.5
Scenario: Register a movement with valid data through SAP PO
	Given I am authenticated as "admin"
	And I have data to process "Movements" in system
	When I have 1 movement
	And I register "Movements" in system
	Then response should be successful

@testcase=41789 @api @version=2 @bvt1.5
Scenario: Register a movement with existing MovementId through SAP PO
	Given I am authenticated as "admin"
	And I have "Movements" in the system for SAP PO
	When I register "Movements" with existing MovementId
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del movimiento ya existe en el sistema​"

@testcase=41790 @version=2 @bvt1.5
Scenario: Register an inventory with valid data through SAP PO
	Given I am authenticated as "admin"
	And I have data to process "Inventories" in system
	When I have 1 inventory
	And I register "Inventories" in system
	Then response should be successful

@testcase=41791 @api @version=2 @bvt1.5
Scenario: Register an inventory with existing InventoryId through SAP PO
	Given I am authenticated as "admin"
	And I have "Inventories" in the system for SAP PO
	When I register "Inventories" with existing InventoryId
	Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario ya existe en el sistema"

@testcase=41792 @ui @version=2 @bvt1.5
Scenario Outline: Register a movement and an inventory with valid data through Excel
	Given I have "Excel" homologation data in the system
	And I am logged in as "admin"
	And I update data in "<FileType>"
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "<FileType>" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	Then "<Entity>" should be registered in the system

	Examples:
		| FileType       | Entity    |
		| MovementExcel  | Movement  |
		| InventoryExcel | Inventory |

@testcase=41793 @ui @version=2 @bvt1.5
Scenario Outline: Update And Delete a movement and an inventory through Excel
	Given I have "<Entity>" in the system through Excel
	When I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "<ActionType>" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "<FileType>" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	Then "<Entity>" should be registered in the system

	Examples:
		| Entity    | FileType       | ActionType |
		| Movement  | MovementExcel  | Update     |
		| Inventory | InventoryExcel | Update     |
		| Movement  | MovementExcel  | Delete     |
		| Inventory | InventoryExcel | Delete     |

@testcase=41794 @ui @version=2 @bvt1.5
Scenario: Register an event with valid data
	Given I have "Event" homologation data in the system
	And I am logged in as "admin"
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

@testcase=41795 @ui @version=2 @bvt1.5
Scenario Outline: Update and Delete an event
	Given I am logged in as "admin"
	And I have "ValidEvent" information in the system
	And I update data in "<File>"
	When I click on "LoadNew" "button"
	Then I should see upload new file interface
	When I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "<Action>" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "<File>" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "Event" "<EventType>" in the system

	Examples:
		| Action | EventType | File             |
		| Update | Updated   | UpdateEventExcel |
		| Delete | Deleted   | DeleteEventExcel |

@testcase=41796 @ui @version=2
Scenario: Register an event already exist in the system
	Given I am logged in as "admin"
	And I have "ValidEvent" information in the system
	When I click on "LoadNew" "button"
	And I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "InsertEventExcel" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see "El evento ya existe" in PendingTransactions for "Event"

@testcase=41797 @ui @version=2
Scenario Outline: Update and Delete an event does not exist
	Given I have "Event" homologation data in the system
	And I am logged in as "admin"
	And I update data in "InsertEventExcel"
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	And I should see upload new file interface
	And I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "<Action>" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "InsertEventExcel" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see "El evento no existe" in PendingTransactions for "Event"

	Examples:
		| Action |
		| Update |
		| Delete |

@testcase=41798 @ui @version=2 @bvt1.5
Scenario: Register a contract with valid data
	Given I have "SalesAndPurchase" homologation data in the system
	And I am logged in as "admin"
	And I update data in "InsertContractExcel"
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "InsertContractExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "Contract" "Inserted" in the system

@testcase=41799 @ui @version=2 @bvt1.5
Scenario Outline: Update and Delete a contract with valid data
	Given I am logged in as "admin"
	And I have "Contract" datas in the system
	And I update data in "<File>"
	When I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "<Action>" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "<File>" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "Contract" "<EventType>" in the system

	Examples:
		| Action | EventType | File                |
		| Update | Updated   | UpdateContractExcel |
		| Delete | Deleted   | DeleteContractExcel |

@testcase=41800 @ui @version=2
Scenario: Register a contract already exist
	Given I am logged in as "admin"
	And I have "Contract" datas in the system
	When I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "InsertContractExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I should see "Ya existe un contrato que contiene el periodo" in PendingTransactions for "Contract"

@testcase=41801 @ui @version=2
Scenario Outline: Update and Delete a contract does not exist
	Given I have "SalesAndPurchase" homologation data in the system
	And I am logged in as "admin"
	And I update data in "InsertContractExcel"
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "<Action>" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "InsertContractExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I should see "El contrato no existe" in PendingTransactions for "Contract"

	Examples:
		| Action |
		| Update |
		| Delete |

@testcase=41802 @manual
Scenario Outline: Validate Movement, Inventory and Ownership registered in blockchain after first retry
	Given I have blockchain disabled
	When I have registered "<Entity>" in the system
	And validate it registered in database
	And validate blockchain status set to 0
	And validate retry count is set to 0
	And blockchain should be enabled
	And validate retry count is changed to 1 after 30 minutes
	Then validate blockchain status set to 1

	Examples:
		| Entity    |
		| Movement  |
		| Inventory |
		| Ownership |

@testcase=41803 @manual
Scenario Outline: Validate Movement, Inventory and Ownership registered in blockchain after second retry
	Given I have blockchain disabled
	When I have registered "<Entity>" in the system
	And validate it registered in database
	And validate blockchain status set to 0
	And validate retry count is set to 0
	And validate retry count is changed to 1 after 30 minutes
	And validate blockchain status set to 0
	And I should get the blockchain failures in the "<Entity>" collection
	And blockchain should be enabled
	And validate retry count is changed to 2 after 60 minutes
	Then validate blockchain status set to 1

	Examples:
		| Entity    |
		| Movement  |
		| Inventory |
		| Ownership |

@testcase=41804 @manual
Scenario Outline: Validate Movement, Inventory and Ownership not registered in blockchain are moved to critical after fourth retry
	Given I have blockchain disabled
	When I have registered "<Entity>" in the system
	And validate it registered in database
	And validate blockchain status set to 0
	And validate retry count is set to 0
	And validate retry count is changed to 1 after 30 minutes
	And validate blockchain status set to 0
	And I should get the blockchain failures in the "<Entity>" collection
	And validate retry count is changed to 2 after 60 minutes
	And validate blockchain status set to 0
	And I should get the blockchain failures in the "<Entity>" collection
	And validate retry count is changed to 3 after 90 minutes
	And validate blockchain status set to 0
	And I should get the blockchain failures in the "<Entity>" collection
	And validate retry count is changed to 4 after 120 minutes
	And validate blockchain status set to 0
	Then I should get the critical blockchain failures in the "<Entity>" collection

	Examples:
		| Entity    |
		| Movement  |
		| Inventory |
		| Ownership |

@testcase=41805 @manual
Scenario Outline: Validate critical Movement, Inventory and Ownership are registered in blockchain after posted the failures to retry
	Given I have blockchain disabled
	When I have registered "<Entity>" in the system
	And validate it registered in database
	And validate blockchain status set to 0
	And validate retry count is set to 0
	And validate retry count is changed to 1 after 30 minutes
	And validate blockchain status set to 0
	And I should get the blockchain failures in the "<Entity>" collection
	And validate retry count is changed to 2 after 60 minutes
	And validate blockchain status set to 0
	And I should get the blockchain failures in the "<Entity>" collection
	And validate retry count is changed to 3 after 90 minutes
	And validate blockchain status set to 0
	And I should get the blockchain failures in the "<Entity>" collection
	And validate retry count is changed to 4 after 120 minutes
	And validate blockchain status set to 0
	And I should get the critical blockchain failures in the "<Entity>" collection
	And I should post the failure "<Entity>" collection
	And blockchain should be enabled
	And validate retry count is set to 0
	Then validate blockchain status set to 1

	Examples:
		| Entity    |
		| Movement  |
		| Inventory |
		| Ownership |