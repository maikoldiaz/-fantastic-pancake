@sharedsteps=4013 @Owner=jagudelos @ui @testplan=6671 @testsuite=7850
Feature: UploadExcelFilesToRecordMovementsAndInventories
In order to register a Movement or Inventory in the system
As a transport segment user
I want to upload the Excel files from UI

Background: Login
	Given I am logged in as "admin"

@testcase=7636 @ui @ignore
Scenario Outline: Verify validations by uploading differnt types of files
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	When I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "<FileType>" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see error message "<Message>"

	Examples:
		| FileType            | Message                                                                            |
		| InvalidFormat       | Tipo de archivo no válido, debe ser archivo EXCEL.                                 |
		| WithoutRecords      | El archivo debe contener al menos una hoja obligatoria (movimientos o inventarios) |
		| WithoutSourceColumn | La columna IdInventario es requerida para el procesamiento del archivo.            |

@testcase=7637 @ui @ignore
Scenario: Query file with data that does not exist in the system
	When I navigate to "FileUpload" page
	And I click on "Search" "button"
	And I select date from "InitialDate" "calendar"
	When I click on "fileUploadGridFilter" "submit "button"
	Then I should see message "No se encuentra datos para los criterios dados."

@testcase=7638 @ui @version=2 @ignore
Scenario: Verify Process Id generation for file operations​
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	When I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "ValidExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the Process Id for file tracking

@testcase=7639 @ui @bvt @ignore
Scenario Outline: Verify file operations using different actions
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	When I select "<Action>" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "<FileType>" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the Process Id for file tracking

	Examples:
		| Action | FileType   |
		| Insert | ValidExcel |
		| Update | ValidExcel |
		| Delete | ValidExcel |

@testcase=7640 @ui @ignore
Scenario: Verify file operations using Re-inject action
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select date from "InitialDate" "calendar"
	And I select date from "FinalDate" "calendar"
	And I select value from "User" "dropdown"
	When I click on "ApplyFilters" "button"
	And I click on "ReInject" "link"
	And I select "Valid" file from directory
	Then it should record the operations in the system
	And it should associate the new process Id with old process Id

@testcase=7641 @ui @ignore
Scenario: Process files list by filters
	When I navigate to "FileUpload" page
	And I click on "Search" "button"
	And I select date from "InitialDate" "calendar"
	And I select date from "FinalDate" "calendar"
	And I select value from "User" "dropdown"
	When I click on "ApplyFilters" "button"
	Then I should see the results as per the filer criteria

@testcase=7642 @ui @ignore
Scenario: Select the date which is greater than 6 months from search date filter
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select date which is greater than 6 months from "InitialDate" "calendar"
	And I select date which is greater than 6 months from "FinalDate" "calendar"
	Then I should not be able to select the date which is greater then 6 months

@testcase=7643 @ui @ignore
Scenario: Verify the Initial view of processed files list
	When I navigate to "FileUpload" page
	When the page is loaded into the UI
	Then I should see the list of files uploaded in the last two days

@testcase=7644 @ui @ignore
Scenario: Verify the Download files functionality
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	When I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "ValidExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the Process Id for file tracking
	When I click on download link
	Then I should be able to download the files successfully

@testcase=7645 @ui @version=2 @ignore
Scenario Outline: Verify the displayed columns on File Upload page
	When I navigate to "FileUpload" page
	When the page is loaded into the UI
	Then I should see the "<Columns>" on File Upload page

	Examples:
		| Columns               |
		| Identificador         |
		| Fecha                 |
		| Archivo               |
		| Acción                |
		| Usuario               |
		| Estado                |
		| Registros  procesados |
		| Errores               |

@testcase=7646 @ui @ignore
Scenario: Verify the file sorting on File Upload page
	When I navigate to "FileUpload" page
	When the page is loaded into the UI
	Then I should see the files sorted by uploaded date in descending order