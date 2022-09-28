@owner=jagudelos @testsuite=49480 @testplan=49466 @MVP2and3 @parallel=false
Feature: BatchIdAndTankFromAllSources
In order to ensure that the same data is received from all sources
As a professional user
I need the batch and tank identifier to be included in the Excel file upload and in the inventory web API

@testcase=52266 @ui 
Scenario: Register an inventory without TankName and BatchId through Excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel with "BatchIdTankNameValidation" data
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "TankName" and "BatchId" is updated as "Null"

@testcase=52267 @ui
Scenario: Register an inventory with both TankName and BatchId through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidation" with both "TankName" and "BatchId"
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "TankName" and 'BatchId' is updated

@testcase=52268 @ui
Scenario: Register an inventory with only TankName through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidation" with only "TankName"
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "TankName" is inserted
And validate "BatchId" is inserted as "Null"

@testcase=52269 @ui
Scenario: Register an inventory with only BatchId through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidation" with only "BatchId"
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "BatchId" is inserted
And validate "TankName" is inserted as "Null"

@testcase=52270 @ui
Scenario Outline: Validate TankName and BatchId length through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidation" with "<Field>" exceeds <Length> characters
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
And it must be stored in a Pendingtransactions repository with validation "<ErrorMessage>"

Examples:
| Field    | Length | ErrorMessage                                                   |
| TankName | 20     | El Tanque puede contener máximo 20 caracteres                  |
| BatchId  | 25     | El identificador del batch puede contener máximo 25 caracteres |

@testcase=52271 @ui 
Scenario: Register an inventory with two same product but different TankName through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidationWithTwoSameProduct" but different "TankName"
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidationWithTwoSameProduct" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered with 2 products in the system
And validate 2 same products with different "TankName"

@testcase=52272 @ui
Scenario: Register an inventory with two same product but different BatchId through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidationWithTwoSameProduct" but different "BatchId"
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidationWithTwoSameProduct" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered with 2 products in the system
And validate 2 same products with different "BatchId"

@testcase=52273 @ui
Scenario: Register an inventory with two different products but same TankName through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidationWithTwoDifferentProducts" but same "TankName"
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidationWithTwoDifferentProducts" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered with 2 products in the system
And validate 2 different products with same "TankName"

@testcase=52274 @ui 
Scenario: Register an inventory with two different products but same BatchId through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidationWithTwoDifferentProducts" but same "BatchId"
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidationWithTwoDifferentProducts" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered with 2 products in the system
And validate 2 different products with same "BatchId"

@testcase=52275 @ui
Scenario: Register an inventory with two different products but same TankName and BatchId through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidationWithTwoDifferentProducts" but same 'TankName' and 'BatchId'
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidationWithTwoDifferentProducts" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered with 2 products in the system
And validate 2 different products with same 'TankName' and 'BatchId'

@testcase=52276 @ui 
Scenario: Register an inventory with same BatchId for attributes and owners through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidation" with same "BatchId" for inventory, attributes, owners
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "attributes" and "owners" details are inserted

@testcase=52277 @ui
Scenario: Register an inventory with same BatchId for attributes, but without BatchId for owners through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidation" with same "BatchId" for inventory, attributes but without owners
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "Attribute" details is inserted
And validate "Owner" details is not inserted

@testcase=52278 @ui
Scenario: Register an inventory with same BatchId for owners, but without BatchId for attributes through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidation" with same "BatchId" for inventory, owners but without attributes
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "Owner" details is inserted
And validate "Attribute" details is not inserted

@testcase=52279 @ui 
Scenario: Register an inventory with BatchId, but without BatchId for attributes and owners through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidation" with same "BatchId" for inventory but without attributes and owners
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "Attribute", "Owner" details are not inserted

@testcase=52280 @ui 
Scenario: Register an inventory with same TankName for attributes and owners through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidation" with same "TankName" for inventory, attributes, owners
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "attributes" and "owners" details are inserted

@testcase=52281 @ui
Scenario: Register an inventory with same TankName for attributes, but TankName BatchId for owners through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidation" with same "TankName" for inventory, attributes but without owners
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "Attribute" details is inserted
And validate "Owner" details is not inserted

@testcase=52282 @ui
Scenario: Register an inventory with same TankName for owners, but without TankName for attributes through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidation" with same "TankName" for inventory, owners but without attributes
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "Owner" details is inserted
And validate "Attribute" details is not inserted

@testcase=52283 @ui
Scenario: Register an inventory with TankName, but without TankName for attributes and owners through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidation" with same "TankName" for inventory but without attributes and owners
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "Attribute", "Owner" details are not inserted

@testcase=52284 @ui @BVT2
Scenario Outline: Update And Delete an inventory without TankName and BatchId through Excel
Given I have "InventoryWithoutTankNameBatchId" in the system through Excel
When I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "<Action>" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "TankName" and "BatchId" is updated as "Null"

Examples:
| Action |
| Update |
| Delete |

@testcase=52285 @ui
Scenario Outline: Update And Delete an inventory with both TankName and BatchId through Excel
Given I have "InventoryWithBothTankNameBatchId" in the system through Excel
When I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "<Action>" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "TankName" and 'BatchId' is updated

Examples:
| Action |
| Update |
| Delete |

@testcase=52286 @ui
Scenario Outline: Update And Delete an inventory with only TankName or BatchId through Excel
Given I have "<Entity>" in the system through Excel
When I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "<Action>" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "<Field1>" is inserted
And validate "<Field2>" is inserted as "Null"

Examples:
| Entity                    | Action | Field1   | Field2   |
| InventoryWithOnlyTankName | Update | TankName | BatchId  |
| InventoryWithOnlyTankName | Delete | TankName | BatchId  |
| InventoryWithOnlyBatchId  | Update | BatchId  | TankName |
| InventoryWithOnlyBatchId  | Delete | BatchId  | TankName |

@testcase=52287 @ui
Scenario Outline: Update And Delete an inventory with TankName or BatchId greater than the expected limit through Excel
And I am logged in as "profesional"
And I update the excel "BatchIdTankNameValidation" with "<Field>" exceeds <Length> characters
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "<Action>" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then it must be stored in a Pendingtransactions repository with validation "<ErrorMessage>"

Examples:
| Field    | Action | Length | ErrorMessage                                                   |
| TankName | Update | 20     | El Tanque puede contener máximo 20 caracteres                  |
| TankName | Delete | 20     | El Tanque puede contener máximo 20 caracteres                  |
| BatchId  | Update | 25     | El identificador del batch puede contener máximo 25 caracteres |
| BatchId  | Delete | 25     | El identificador del batch puede contener máximo 25 caracteres |

@testcase=52288 @ui
Scenario Outline: Update and Delete an inventory with two different products but same TankName and BatchId through excel
Given I have "InventoryWithTwoDifferentProductsButSameTankNameBatchId" in the system through Excel
When I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "<Action>" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidationWithTwoDifferentProducts" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered with 2 products in the system
And validate 2 different products with same 'TankName' and 'BatchId'

Examples:
| Action |
| Update |
| Delete |

@testcase=52289 @ui
Scenario:  Update an inventory volume, attributes and owners with BatchId through Excel
Given I have "InventoryWithOnlyBatchId" in the system through Excel
And I update the excel "BatchIdTankNameValidation" with only "ProductVolume"
When I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Update" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "ProductVolume" is updated

@testcase=52290 @ui
Scenario:  Update an inventory volume with BatchId, but without BatchId in attributes and owners through Excel
Given I have "InventoryWithOnlyBatchId" in the system through Excel
And I update the excel "BatchIdTankNameValidation" with only "ProductVolume" but without "BatchId" in attributes and owners
When I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Update" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system
And validate "ProductVolume" is updated

@testcase=52291 @ui
Scenario: Validate multiple action Insert, Delete and then Update for the same inventory through excel
Given I have "InventoryWithoutTankNameBatchId" in the system through Excel
When I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Delete" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
And "Inventory" should be registered in the system
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Update" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
And it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a ajustar no existe"

@testcase=52292 @ui
Scenario: Validate multiple action Update, Insert, Update and then Delete for the same inventory through excel
Given I have "Excel" homologation data in the system
And I am logged in as "profesional"
And I update the excel with "BatchIdTankNameValidation" data
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Update" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
And it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a ajustar no existe"
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
And "Inventory" should be registered in the system
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Update" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
And "Inventory" should be registered in the system
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Delete" from FileUpload dropdown
And I click on "Browse" to upload
And I select "BatchIdTankNameValidation" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then "Inventory" should be registered in the system

@testcase=52293 @api
Scenario: Register an inventory without BatchId through SAP PO
Given I am authenticated as "profesional"
And I have data to process "Inventories" in system
When I have 1 inventory without "BatchId"
And I register "Inventories" in system
Then response should be successful
And validate "BatchId" is inserted as "Null"

@testcase=52294 @api @BVT2
Scenario: Register an inventory with BatchId through SAP PO
Given I am authenticated as "profesional"
And I have data to process "Inventories" in system
When I have 1 inventory with "BatchId"
And I register "Inventories" in system
Then response should be successful
And validate "BatchId" is inserted

@testcase=52295 @api @BVT2
Scenario: Validate BatchId length through SAP PO
Given I am authenticated as "profesional"
And I have data to process "Inventories" in system
When I have 1 inventory with "BatchId" that exceeds 25 characters
And I register "Inventories" in system
Then the response should fail with message "El identificador del batch (BatchId) puede contener máximo 25 caracteres"

@testcase=52296 @api @BVT2
Scenario: Register an inventory with two same product but different BatchId through SAP PO
Given I am authenticated as "profesional"
And I have data to process "Inventories" in system
When I have 1 inventory with 2 same product but different "BatchId"
And I register "Inventories" in system
Then response should be successful
And validate 2 same products with different "BatchId"

@testcase=52297 @api @BVT2
Scenario: Register an inventory with two different products and BatchId through SAP PO
Given I am authenticated as "profesional"
And I have data to process "Inventories" in system
When I have 1 inventory with 2 different products and "BatchId"
And I register "Inventories" in system
Then response should be successful
And validate 2 different products and "BatchId"

@testcase=52298 @api @BVT2
Scenario: Register an inventory with two different products but same BatchId through SAP PO
Given I am authenticated as "profesional"
And I have data to process "Inventories" in system
When I have 1 inventory with 2 different products but same "BatchId"
And I register "Inventories" in system
Then response should be successful
And validate 2 different products with same "BatchId"

@testcase=52299 @api @BVT2
Scenario: Register an inventory with same product and BatchId already exist through SAP PO
Given I am authenticated as "profesional"
And I have data to process "Inventories" in system
And I have 1 inventory with "BatchId" in the system
When I register "Inventories" with exiting InventoryId and BatchId
Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario ya existe en el sistema​"

@testcase=52300 @api @BVT2
Scenario Outline: Update and Delete an inventory without BatchId through SAP PO
Given I am authenticated as "profesional"
And I have data to process "Inventories" in system
And I have 1 inventory without "BatchId" in the system
When I update "EventType" as "<Action>" with existing 'InvetoryId'
And I register "Inventories" in system
Then response should be successful
And "Inventory" should be registered in the system

Examples:
| Action |
| Update |
| Delete |

@testcase=52301 @api @BVT2
Scenario Outline: Update and Delete an inventory with BatchId through SAP PO
Given I am authenticated as "profesional"
And I have data to process "Inventories" in system
And I have 1 inventory with "BatchId" in the system
When I update "EventType" as "<Action>" with existing 'BatchId'
And I register "Inventories" in system
Then response should be successful
And "Inventory" should be registered in the system

Examples:
| Action |
| Update |
| Delete |

@testcase=52302 @api
Scenario Outline: Update and Delete an inventory with BatchId exceeds limit through SAP PO
Given I am authenticated as "profesional"
And I have data to process "Inventories" in system
When I update "EventType" as "<Action>" with "BatchId" exceeds 25 characters
And I register "Inventories" in system
Then the response should fail with message "El identificador del batch (BatchId) puede contener máximo 25 caracteres"

Examples:
| Action |
| Update |
| Delete |

@testcase=52303 @api @BVT2
Scenario: Register an inventory without BatchId through SAP PO by homologated json
Given I am authenticated as "profesional"
And I have "shouldHomologate" flag set as 'false'
And I have data to process "Inventories" in system
When I have 1 inventory without "BatchId"
And I register "Inventories" in system
Then response should be successful
And validate "BatchId" is inserted as "Null"

@testcase=52304 @api @BVT2
Scenario: Register an inventory with BatchId through SAP PO by homologated json
Given I am authenticated as "profesional"
And I have "shouldHomologate" flag set as 'false'
And I have data to process "Inventories" in system
When I have 1 inventory with "BatchId"
And I register "Inventories" in system
Then response should be successful
And validate "BatchId" is inserted

@testcase=52305 @api
Scenario: Validate BatchId length through SAP PO by homologated json
Given I am authenticated as "profesional"
And I have "shouldHomologate" flag set as 'false'
And I have data to process "Inventories" in system
When I have 1 inventory with "BatchId" that exceeds 25 characters
And I register "Inventories" in system
Then the response should fail with message "El identificador del batch (BatchId) puede contener máximo 25 caracteres"

@testcase=52306 @api @BVT2
Scenario: Register an inventory with two same product but different BatchId through SAP PO by homologated json
Given I am authenticated as "profesional"
And I have "shouldHomologate" flag set as 'false'
And I have data to process "Inventories" in system
When I have 1 inventory with 2 same product but different "BatchId"
And I register "Inventories" in system
Then response should be successful
And validate 2 same products with different "BatchId"

@testcase=52307 @api
Scenario: Register an inventory with two different products and BatchId through SAP PO by homologated json
Given I am authenticated as "profesional"
And I have "shouldHomologate" flag set as 'false'
And I have data to process "Inventories" in system
When I have 1 inventory with 2 different products and "BatchId"
And I register "Inventories" in system
Then response should be successful
And validate 2 different products and "BatchId"

@testcase=52308 @api
Scenario: Register an inventory with two different products but same BatchId through SAP PO by homologated json
Given I am authenticated as "profesional"
And I have "shouldHomologate" flag set as 'false'
And I have data to process "Inventories" in system
When I have 1 inventory with 2 different products but same "BatchId"
And I register "Inventories" in system
Then response should be successful
And validate 2 different products with same "BatchId"

@testcase=52309 @api
Scenario: Register an inventory with same product and BatchId already exist through SAP PO by homologated json
Given I am authenticated as "profesional"
And I have "shouldHomologate" flag set as 'false'
And I have data to process "Inventories" in system
And I have 1 inventory with "BatchId" in the system
When I register "Inventories" with exiting InventoryId and BatchId
Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario ya existe en el sistema​"

@testcase=52310 @api @BVT2
Scenario Outline: Update and Delete an inventory without BatchId through SAP PO by homologated json
Given I am authenticated as "profesional"
And I have "shouldHomologate" flag set as 'false'
And I have data to process "Inventories" in system
And I have 1 inventory without "BatchId" in the system
When I update "EventType" as "<Action>" with existing 'InvetoryId'
And I register "Inventories" in system
Then response should be successful
And "Inventory" should be registered in the system

Examples:
| Action |
| Update |
| Delete |

@testcase=52311 @api @BVT2
Scenario Outline: Update and Delete an inventory with BatchId through SAP PO by homologated json
Given I am authenticated as "profesional"
And I have "shouldHomologate" flag set as 'false'
And I have data to process "Inventories" in system
And I have 1 inventory with "BatchId" in the system
When I update "EventType" as "<Action>" with existing 'BatchId'
And I register "Inventories" in system
Then response should be successful
And "Inventory" should be registered in the system

Examples:
| Action |
| Update |
| Delete |

@testcase=52312 @api @BVT2
Scenario Outline: Update and Delete an inventory with BatchId exceeds limit through SAP PO by homologated json
Given I am authenticated as "profesional"
And I have "shouldHomologate" flag set as 'false'
And I have data to process "Inventories" in system
When I update "EventType" as "<Action>" with "BatchId" exceeds 25 characters
And I register "Inventories" in system
Then the response should fail with message "El identificador del batch (BatchId) puede contener máximo 25 caracteres"

Examples:
| Action |
| Update |
| Delete |

@testcase=52313 @api
Scenario: Validate multiple action Insert, Delete and then Update for the same inventory through SAP PO
Given I am authenticated as "profesional"
And I have data to process "Inventories" in system
When I have 1 inventory with "BatchId"
And I register "Inventories" in system
And response should be successful
And validate "Inventory" should be "Insert" in the system
And I update "EventType" as "Delete" with existing 'BatchId'
And I register "Inventories" in system
And response should be successful
And validate "Inventory" should be "Delete" in the system
And I update "EventType" as "Update" with existing 'BatchId'
And I register "Inventories" in system
And response should be successful
Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a ajustar no existe"

@testcase=52314 @api
Scenario: Validate multiple action Update, Insert, Update and then Delete for the same inventory through SAP PO
Given I am authenticated as "profesional"
And I have data to process "Inventories" in system
When I have 1 inventory with "BatchId"
And I update "EventType" as "Update"
And I register "Inventories" in system
And response should be successful
And it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a ajustar no existe"
And I update "EventType" as "Insert" with existing 'BatchId'
And I register "Inventories" in system
And response should be successful
And validate "Inventory" should be "Insert" in the system
And I update "EventType" as "Update" with existing 'BatchId'
And I register "Inventories" in system
And response should be successful
And validate "Inventory" should be "Update" in the system
And I update "EventType" as "Delete" with existing 'BatchId'
And I register "Inventories" in system
And response should be successful
Then validate "Inventory" should be "Delete" in the system
