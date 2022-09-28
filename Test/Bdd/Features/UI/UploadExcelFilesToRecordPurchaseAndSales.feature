@sharedsteps=16581 @Owner=jagudelos @ui @testplan=14709 @testsuite=14732
Feature: UploadExcelFilesToRecordPurchaseAndSales
In order to register a Pirchase or Sales in the system
As a transport segment user
I want to upload the Excel files from UI

Background: Login
	Given I am logged in as "profesional"

@testcase=16615 @ui
Scenario Outline: Verify validations by uploading differnt types of files for Purchase and Sales
	When I browse to "FileUploadForSalesAndPurchases" page
	When I should see breadcrumb "Subir archivo de compras/ventas"
	And I click on "FileUpload" "button"
	When I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload in purchase and sales page
	And I select "<FileType>" file from purchase sales directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see error message "<Message>"

	Examples:
		| FileType            | Message                                                           |
		| InvalidFormat       | Tipo de archivo no válido, debe ser archivo EXCEL.                |
		| WithoutRecords      | El archivo debe tener por lo menos un registr                     |
		| WithoutSourceColumn | La columna Unidad es requerida para el procesamiento del archivo. |

@testcase=16616 @ui
Scenario: Query file with data that does not exist in the system for Purchase and Sales
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "Search" "button"
	And I select date from "InitialDate" "calendar"
	When I click on "fileUploadGridFilter" "submit "button"
	Then I should see message "No se encuentra datos para los criterios dados."

@testcase=16617 @ui
Scenario: Verify Process Id generation for file operations​ for Purchase and Sales
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "FileUpload" "button"
	When I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "ValidExcel" file from purchase sales directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the Process Id for file tracking

@testcase=16618 @ui
Scenario Outline: Verify file operations using different actions for Purchase and Sales
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "FileUpload" "button"
	When I select "<Action>" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "<FileType>" file from purchase sales directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the Process Id for file tracking

	Examples:
		| Action | FileType   |
		| Insert | ValidExcel |
		| Update | ValidExcel |
		| Delete | ValidExcel |

@testcase=16619 @ui
Scenario: Verify the Download files functionality for Purchase and Sales
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "FileUpload" "button"
	When I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "ValidExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the Process Id for file tracking
	When I click on download link
	Then I should be able to download the files successfully

@testcase=16620 @ui
Scenario Outline: Verify the displayed columns on File Upload page for Purchase and Sales
	When I navigate to "FileUploadForSalesAndPurchases" page
	When the page is loaded into the UI
	Then I should see the "<Columns>" on File Upload page

	Examples:
		| Columns               |
		| Identificador         |
		| Fecha                 |
		| Archivo               |
		| Acción                |
		| created               |
		| Estado                |
		| Registros  procesados |
		| Errores               |

@testcase=16621 @ui
Scenario: Verify the file sorting on File Upload page for Purchase and Sales
	When I navigate to "FileUploadForSalesAndPurchases" page
	When the page is loaded into the UI
	Then I should see the files sorted by uploaded date in descending order