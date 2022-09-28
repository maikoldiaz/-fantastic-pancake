@sharedsteps=4013 @owner=jagudelos @ui @testplan=49466 @testsuite=49468 @MVP2and3 @S13
Feature: NewValidationsInExcelUpload
As a Balance Segment Professional User
I need new validations in the processing of official movements and inventories
to allow the registration of these in the application.

Background: Login
	Given I am logged in as "admin"

@parallel=false @testcase=52792 @BVT2
Scenario: Register a new official movement through excel
And I have "Excel" homologation data in the system
And I have 1 movement with scenarioId attribute as 2
When I update the "TestData_Official" excel
And I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then Verify that movement Id from excel upload should be hashed and stored in MovementId column in Movement table

@parallel=false @testcase=52793
Scenario:Register through excel an official movement that already exists in TRUE
Given I have valid official Movements with same identifier in the system
And I have 1 movement with scenarioId attribute as 2
When I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then Excel upload should be failed
And I click on "FileUploads" "ViewError" "link"
And I clicked on "PendingTransactionErrors" "Detail" "Link"  for "Movements"
And "El identificador del movimiento ya existe en el sistema" message should be displayed in the exception page

@parallel=false @testcase=52794
Scenario: Update through excel an official movement that does not exist in TRUE
And I have "Excel" homologation data in the system
And I have 1 movement with scenarioId attribute as 2
When I update the "TestData_Official" excel
And I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Update" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then Excel upload should be failed
And I click on "FileUploads" "ViewError" "link" 
And I clicked on "PendingTransactionErrors" "Detail" "Link"  for "Movements"
And "El identificador del movimiento a ajustar no existe" message should be displayed in the exception page

@parallel=false @testcase=52795 @BVT2
Scenario:Update through excel an official movement that already exists in TRUE
Given I have valid official Movements with same identifier in the system
And I have 1 movement with scenarioId attribute as 2
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Update" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then Official movement should be stored into the system
And Existing movement should be stored in the system with negative volume
And Identifier of the negative movement and the movement to be stored must be the same identifier of the original movement

@parallel=false @testcase=52796
Scenario: Delete through excel an official movement that does not exist in TRUE
And I have "Excel" homologation data in the system
And I have 1 movement with scenarioId attribute as 2
When I update the "TestData_Official" excel
And I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Delete" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then Excel upload should be failed
And I click on "FileUploads" "ViewError" "link"
And I clicked on "PendingTransactionErrors" "Detail" "Link"  for "Movements"
And "El identificador del movimiento a anular no existe" message should be displayed in the exception page

@parallel=false @testcase=52797 @BVT2
Scenario: Delete through excel an official movement that already exists in TRUE
Given I have valid official Movements with same identifier in the system
And I have 1 movement with scenarioId attribute as 2
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Delete" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then Existing movement should be stored in the system with negative volume
And Identifier of the negative movement and the movement to be stored must be the same identifier of the original movement

@parallel=false @testcase=52798
Scenario:Register through excel an official inventory that already exists in TRUE
Given I have valid official Inventory with same identifier in the system
And I have 1 inventory with scenarioId attribute as 2
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then Excel upload should be failed
And I click on "FileUploads" "ViewError" "link"
And I clicked on "PendingTransactionErrors" "Detail" "Link"  for "Inventory"
And "El identificador del inventario ya existe en el sistema" message should be displayed in the exception page

@parallel=false @testcase=52799 @BVT2
Scenario: Register through excel a new official inventory
And I have "Excel" homologation data in the system
And I have 1 inventory with scenarioId attribute as 2
When I update the "TestData_Official" excel
And I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then Verify that inventory Id from excel upload should be hashed and stored in Inventoryid and InventoryProductUniqueId column in InventoryProduct table

@parallel=false @testcase=52800
Scenario: Update through excel an official inventory that does not exist in TRUE
And I have "Excel" homologation data in the system
And I have 1 inventory with scenarioId attribute as 2
When I update the "TestData_Official" excel
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Update" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then Excel upload should be failed
And I click on "FileUploads" "ViewError" "link"
And I clicked on "PendingTransactionErrors" "Detail" "Link"  for "Inventory"
And "El identificador del inventario a ajustar no existe" message should be displayed in the exception page

@parallel=false @testcase=52801 @BVT2
Scenario: Update through excel an official inventory that already exists in TRUE
Given I have valid official Inventory with same identifier in the system
And I have 1 inventory with scenarioId attribute as 2
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Update" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then Official inventory should be stored into the system
And Existing inventory should be stored in the system with negative volume

@parallel=false @testcase=52802
Scenario: Delete through excel an official inventory that that already exists in TRUE
And I have "Excel" homologation data in the system
And I have 1 inventory with scenarioId attribute as 2
When I update the "TestData_Official" excel
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Delete" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then Excel upload should be failed
And I click on "FileUploads" "ViewError" "link"
And I clicked on "PendingTransactionErrors" "Detail" "Link"  for "Inventory"
And "El identificador del inventario a anular no existe" message should be displayed in the exception page

@parallel=false @testcase=52803 @BVT2
Scenario:Delete through excel an official inventory that does not exist in TRUE
Given I have valid official Inventory with same identifier in the system
And I have 1 inventory with scenarioId attribute as 2
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Delete" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then Existing inventory should be stored in the system with negative volume

@parallel=false @testcase=52804 @api
Scenario:  Verify that record should be store in pending transactions table when official movement is already registered through Web API
Given I have "Movements" in the application
When I have 1 movement with event type is "INSERT" and scenarioId attribute as 2
And I register "Movements" in system
Then it must be stored in a Pendingtransactions repository with validation "El identificador del movimiento ya existe en el sistema"

@parallel=false @testcase=52805 @api
Scenario: Verify that official Movement should be registered If the movement does not exist
Given I have data to process "Movements" in system
When I have 1 movement with scenarioId attribute as 2
And I register "Movements" in system
Then response should be successful
And I have Official information data
And 1 movement should be registered in system

@parallel=false @testcase=52806 @api
Scenario: Verify that official movement should be updated when it meets all validations and EventType field is equal to UPDATE
Given I have "Movements" in the application
When I have 1 movement with event type is "UPDATE" and scenarioId attribute as 2
And I register "Movements" in system
Then response should be successful
And I have Official information data
And movement should be updated in system

@parallel=false @testcase=52807 @api
Scenario: Verify record should be store in pending transactions table when official movement does not exists in the system with UPDATE event
When I process "Movements" request with event type is "UPDATE"
And "movement" does not exists in the system
Then it must be stored in a Pendingtransactions repository with validation "El identificador del movimiento a ajustar no existe"

@parallel=false @testcase=52808 @api
Scenario: Verify movemennt should be deleted when it meets all validations and EventType field is equal to DELETE
Given I have "Movements" in the application
When I have 1 movement with event type is "DELETE" and scenarioId attribute as 2
And I register "Movements" in system
Then response should be successful
And I have Official information data
And movement should be deleted in system

@parallel=false @testcase=52809 @api
Scenario: Verify record should be store in pending transactions table when official movement does not exists in the system with DELETE event
When I process "Movements" request with event type is "DELETE"
And "movement" does not exists in the system
Then it must be stored in a Pendingtransactions repository with validation "El identificador del movimiento a anular no existe"

@parallel=false @testcase=52810 @api
Scenario:  Verify that record should be store in pending transactions table when official inventory is already registered through Web API
Given I have "Inventories" in the application
When I have 1 inventory with event type is "INSERT" and scenarioId attribute as 2
And I register "Inventories" in system
Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario ya existe en el sistema"

@parallel=false @testcase=52811 @api
Scenario: Verify that official inventory should be registered If the inventory does not exist
Given I have data to process "Inventories" in system
When I have 1 inventory with scenarioId attribute as 2
And I register "Inventories" in system
Then response should be successful
And I have Official information data
And 1 inventory should be registered in system

@parallel=false @testcase=52812 @api
Scenario: Verify that official inventory should be updated when it meets all validations and EventType field is equal to UPDATE
Given I have "Inventories" in the application
When I have 1 inventory with event type is "UPDATE" and scenarioId attribute as 2
And I register "Inventories" in system
Then response should be successful
And I have Official information data
And inventory should be updated in system

@parallel=false @testcase=52813 @api
Scenario: Verify record should be store in pending transactions table when official inventory does not exists in the system with UPDATE event
When I process "Inventories" request with event is "UPDATE"
And "inventory" does not exists in the system
Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a ajustar no existe"

@parallel=false @testcase=52814 @api
Scenario: Verify inventory should be deleted when it meets all validations and EventType field is equal to DELETE
Given I have "Inventories" in the application
When I have 1 inventory with event type is "DELETE" and scenarioId attribute as 2
And I register "Inventories" in system
Then response should be successful
And I have Official information data
And inventory should be deleted in system

@parallel=false @testcase=52815 @api
Scenario: Verify record should be store in pending transactions table when official inventory does not exists in the system with DELETE event
When I process "Inventories" request with event is "DELETE"
And "inventory" does not exists in the system
Then it must be stored in a Pendingtransactions repository with validation "El identificador del inventario a anular no existe"

@parallel=false @testcase=52816
Scenario: Verify that record should be store in pending transactions table with error when official movement belongs to the date of the current month.
Given I have "Excel" homologation data in the system
And I have 1 movement with scenarioId attribute as 2
And I update the "TestData_Official" excel for official movements with operational date of the current month
When I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then Excel upload should be failed
And I click on "FileUploads" "ViewError" "link"
And I clicked on "PendingTransactionErrors" "Detail" "Link"  for "Movements"
And "No es posible registrar un movimiento oficial con fecha del mes actual" message should be displayed in the exception page

@parallel=false @testcase=52817
Scenario: Verify that record should be store in pending transactions table with error when official inventory belongs to the date of the current month.
Given I have "Excel" homologation data in the system
And I have 1 inventory with scenarioId attribute as 2
And I update the "TestData_Official" excel for official inventory with opearional date of the current month
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
And I wait till file upload to complete
Then Excel upload should be failed
And I click on "FileUploads" "ViewError" "link"
And I clicked on "PendingTransactionErrors" "Detail" "Link"  for "Inventory"
And "No es posible registrar un inventario oficial con fecha del mes actual" message should be displayed in the exception page

@parallel=false @testcase=52818
Scenario: Verify that True should register movement into the system when operational date of official movements is of months prior to the current one
Given I have "Excel" homologation data in the system
And I have 1 movement with scenarioId attribute as 2
And I update the "TestData_Official" excel for official movement with operational date prior the current month
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
Then it should be registered in the system

@parallel=false @testcase=52819
Scenario: Verify that True should register inventory into the system when operational date of official inventory is of months prior to the current one
Given I have "Excel" homologation data in the system
And I have 1 inventory with scenarioId attribute as 2
And I update the "TestData_Official" excel for official inventory with operational date prior the current month
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
And I select segment from "FileUpload" "segment" "dropdown"
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "TestData_Official" file from directory
And I click on "uploadFile" "Submit" "button"
Then it should be registered in the system