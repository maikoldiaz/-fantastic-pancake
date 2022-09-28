@sharedsteps=4013 @owner=jagudelos @ui @testplan=49466 @testsuite=49473 MVP2and3 @parallel=false
Feature: AdjustmentToExcelFileUpload
As TRUE system, I need some adjustments in process of an Excel file
upload to match the canonical MVP2

Background: Login
	Given I am logged in as "admin"
@parallel=false @testcase=52253 @BVT2
Scenario: Verify the newly added fields for Inventory when TRUE User uploads an excel with valid Inventory data
	Given I have "Excel" homologation data in the system
	And I update the excel for "TestData_UpdatedExcel" data for newly added fields
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestData_UpdatedExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	Then Verify that following "fields" should recorded into the Inventory Table
		| fields     |
		| TankName   |
		| BatchId    |
		| Version    |
		| SystemId   |
		| OperatorId |
	And Verify that "TipoAtributo" should be loaded succesfully into the Attribute for the given Inventory
@parallel=false @testcase=52254 @version=2
Scenario: Verify the datatype of IdEscenario field User uploads an excel with valid Movement/Inventory data
	Given I have "Excel" homologation data in the system
	And I update the excel for "TestData_UpdatedExcel" data with incorrect datatype
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestData_UpdatedExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	Then Excel upload should be failed
	And I click on "FileUploads" "ViewError" "link"
	And I click on "PendingTransactionErrors" "Detail" "Link"
	And "Una o más columnas IdEscenario presentan un tipo de dato incorrecto." message should be displayed in the exception page
@parallel=false @testcase=52255 @BVT2
Scenario: Verify the newly added fields for Movement when TRUE User uploads an excel with valid Movement data
	Given I have "Excel" homologation data in the system
	And I update the excel for "TestData_UpdatedExcel" data for newly added fields
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestData_UpdatedExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	Then Verify that following "fields" should recorded into the Movements Table
		| fields     |
		| BatchId    |
		| Version    |
		| SystemId   |
		| OperatorId |
	And Verify that "TipoAtributo" should be loaded succesfully into the Attribute for the given Movement
@parallel=false @testcase=52256
Scenario: Verify that error message should be displayed when uploads an excel with scenario id other than 1,2 and 3
	Given I have "Excel" homologation data in the system
	And I update the excel for "TestData_UpdatedExcel" data with different scenario id
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestData_UpdatedExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	Then Excel upload should be failed
	And I click on "FileUploads" "ViewError" "link"
	And I click on "PendingTransactionErrors" "Detail" "Link"
	And "El escenario suministrado no es válido." message should be displayed in the exception page
@parallel=false @testcase=52257
Scenario: Verify the changed in the fields for Inventory/Movements when TRUE User uploads an excel with valid data
	Given I have "Excel" homologation data in the system
	And I update the excel for "TestData_UpdatedExcel" data for changed in field
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestData_UpdatedExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	Then Verify that following "fields" should be used while uploading the excel and same should be reflected properly in TRUE Application
		| fields                |
		| UncertaintyPercentage |
		| ProductVolume         |
		| GrossStandardQuantity |
@parallel=false @testcase=52258 @BVT2
Scenario: Verify the excel upload will failed and exception will be triggered when user upload the file with previous/older column names
	Given I have "Excel" homologation data in the system
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "WithoutSourceColumn" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the error Message "La columna Sistema,IdEscenario,CantidadBruta,CantidadNeta,Incertidumbre,Version,Operador es requerida para el procesamiento del archivo."
@parallel=false @testcase=52259
Scenario: Verify the optional field in excel upload and it should upload successfully without giving any values into those columns
	Given I have "Excel" homologation data in the system
	And I update the excel for "TestData_UpdatedExcel_withOptional" data for optional fields
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestData_UpdatedExcel_withOptional" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then I wait till file upload to complete
	And the value in the following "fields" for Movements and Inventories should be NULL in TRUE
		| fields                   |
		| ProductType              |
		| SourceProductTypeId      |
		| DestinationProductId     |
		| DestinationProductTypeId |
		| Version                  |
@parallel=false @testcase=52260
Scenario: Verify that newly added fields are optional in excel upload and it should upload successfully without giving any values into those columns
	Given I have "Excel" homologation data in the system
	And I update the excel for "TestData_UpdatedExcel_withOptional" data for optional fields
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestData_UpdatedExcel_withOptional" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then I wait till file upload to complete
	And the value in the following "fields" for Movements and Inventories should be NULL in TRUE
		| fields     |
		| TankName   |
		| BatchId    |
		| SystemId   |
		| OperatorId |
@parallel=false @testcase=52261 @BVT2
Scenario: Verify the excel should be successfully uploaded if  the Movement Source is null/empty but movement Destination has value
	Given I have "Excel" homologation data in the system
	And I update the excel for "TestData_UpdatedExcel" data with "Movement Source" as NULL
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestData_UpdatedExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	Then it should be registered in the system with "Movement Source" as NULL
@parallel=false @testcase=52262
Scenario: Verify the excel should be successfully uploaded if the Movement Destination is null/empty but movement Source has value
	Given I have "Excel" homologation data in the system
	And I update the excel for "TestData_UpdatedExcel" data with "Movement Destination" as NULL
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestData_UpdatedExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	Then it should be registered in the system with "Movement Destination" as NULL
@parallel=false @testcase=52263 @BVT2
Scenario: Verify the Error should be displayed during excel uploads when Movement Source and Movement Destination is null
	Given I have "Excel" homologation data in the system
	And I update the excel for "TestData_UpdatedExcel" data with "Movement Source and Destination" as NULL
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestData_UpdatedExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	Then Excel upload should be failed
	And I click on "FileUploads" "ViewError" "link"
	And I click on "PendingTransactionErrors" "Detail" "Link"
	And "Es obligatorio reportar información del origen o del destino. (Ambas no pueden estar vacías)." message should be displayed in the exception page
@parallel=false @testcase=52264 @BVT2
Scenario: Verify that while doing excel upload SourceProductId should be used as a value if DestinationProductId is null and Movement Source has value
	Given I have "Excel" homologation data in the system
	And I update the excel for "TestData_UpdatedExcel" data with "DestinationProductId" as NULL
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestData_UpdatedExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	Then Verify that SourceProductId should be used as a value for Destination Product
@parallel=false @testcase=52265 @BVT2
Scenario: Verify that while doing excel upload a required field error for DestinationProductId must be raised if DestinationProductId is null and Movement Source has NOT value
	Given I have "Excel" homologation data in the system
	And I update the excel for "TestData_UpdatedExcel" data with "DestinationProductId with MovementSource" as NULL
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestData_UpdatedExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	Then Excel upload should be failed
	And I click on "FileUploads" "ViewError" "link"
	And I click on "PendingTransactionErrors" "Detail" "Link"
	And "El identificador del producto destino es obligatorio" message should be displayed in the exception page