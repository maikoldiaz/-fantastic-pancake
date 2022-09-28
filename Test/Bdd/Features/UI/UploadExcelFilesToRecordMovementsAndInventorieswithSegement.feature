@sharedsteps=16581 @owner=jagudelos @ui @testplan=14709 @testsuite=14728
Feature: UploadExcelFilesToRecordMovementsAndInventoriesWithSegement
In order to register a Movement or Inventory in the system
As a transport segment user
I want to upload the Excel files from UI with Segment

Background: Login
	Given I am logged in as "profesional"

@testcase=16886 @bvt @version=2 @ignore
Scenario Outline: Register a new Movement or Inventory by uploading Excel and selecting Segment
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	When I select any segment from "FileUpload" "segment" "dropdown"
	When I select "<ActionType>" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "ValidExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then it should be registered in the system

	Examples:
		| ActionType |
		| Insert     |
		| Update     |
		| Delete     |

@testcase=16887 @prodready @version=2
Scenario Outline: Verify validations by uploading differnt types of files and selecting Segment
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	When I select any segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "<FileType>" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see error message "<Message>"

	Examples:
		| FileType            | Message                                                                            |
		| InvalidFormat       | Tipo de archivo no válido, debe ser archivo EXCEL.                                 |
		| WithoutRecords      | El archivo debe contener al menos una hoja obligatoria (movimientos o inventarios) |
		| WithoutSourceColumn | La columna IdInventario es requerida para el procesamiento del archivo.            |

@testcase=16888 @prodready @version=2
Scenario: Query file with data for Segment category that does not exist in the system
	When I navigate to "FileUpload" page
	And I click on "Search" "button"
	And I select date from "InitialDate" "calendar"
	When I click on "fileUploadGridFilter" "submit "button"
	Then I should see message "Sin registrosCargar nuevo"

@testcase=16889 @prodready @version=2 @bvt
Scenario: Verify Process Id generation for file operations​ with Segment category
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	When I select any segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	When I select "ValidExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the Process Id for file tracking

@testcase=16890 @prodready @version=2 @bvt
Scenario Outline: Verify file operations using different actions with Segment category
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	When I select any segment from "FileUpload" "segment" "dropdown"
	And I select "<Action>" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "<FileType>" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the Upload Id for file tracking

	Examples:
		| Action | FileType   |
		| Insert | ValidExcel |
		| Update | ValidExcel |
		| Delete | ValidExcel |

@testcase=16891 @version=2
Scenario: Verify file operations using Re-inject action for Segment category
	When I navigate to "FileUpload" page
	And I click on "Search" "button"
	And I select date from "InitialDate" "calendar"
	And I select date from "FinalDate" "calendar"
	And I select value from "User" "dropdown"
	When I click on "ApplyFilters" "button"
	And I click on "ReInject" "link"
	And I select "Valid" file from directory
	Then it should record the operations in the system
	And it should associate the new process Id with old process Id

@testcase=16892 @prodready @version=2
Scenario: Process files list by filters for Segment category
	When I navigate to "FileUpload" page
	And I click on "Search" "button"
	And I select date from "InitialDate" "calendar"
	And I select date from "FinalDate" "calendar"
	And I select value from "User" "dropdown"
	When I click on "ApplyFilters" "button"
	Then I should see the results as per the filer criteria

@testcase=16893
Scenario: Select the date which is greater than 6 months from search date filter for Segment category
	When I navigate to "FileUpload" page
	And I click on "Search" "button"
	And I select date which is greater than 6 months from "InitialDate" "calendar"
	And I select date which is greater than 6 months from "FinalDate" "calendar"
	Then I should not be able to select the date which is greater then 6 months

@testcase=16894
Scenario: Verify the Initial view of processed files list with Segment
	When I navigate to "FileUpload" page
	When the page is loaded into the UI
	Then I should see the list of files uploaded in the last two days with Segment

@testcase=16895 @prodready @version=2 @bvt
Scenario: Verify the Download files functionality for Segment category
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	When I select any segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "ValidExcel" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then I should see the Upload Id for file tracking
	When I click on download link
	Then I should be able to download the files successfully

@testcase=16896 @prodready @version=2
Scenario: Verify the displayed columns on File Upload page with Segment category
	When I navigate to "FileUpload" page
	When the page is loaded into the UI
	Then I should see the "Columns" on the page
		| Columns              |
		| Identificador        |
		| Fecha de creación    |
		| Segmento             |
		| Archivo              |
		| Acción               |
		| Usuario              |
		| Estado               |
		| Registros procesados |
		| Errores              |

@testcase=16897 @manual
Scenario: Verify the file sorting in descending order on File Upload page with Segment category
	When I navigate to "FileUpload" page
	When the page is loaded into the UI
	Then I should see the files sorted by uploaded date in descending order with Segment

@testcase=16898 @manual
Scenario: Verify the file sorting in ascending order on File Upload page with Segment category
	When I navigate to "FileUpload" page
	When the page is loaded into the UI
	And I click on sort arror
	Then I should see the files sorted by uploaded date in ascending order with Segment