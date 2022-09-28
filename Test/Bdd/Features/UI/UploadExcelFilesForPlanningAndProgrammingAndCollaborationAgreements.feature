@sharedsteps=16581 @owner=jagudelos @ui @testplan=19772 @testsuite=19784
Feature: UploadExcelFilesForPlanningAndProgrammingAndCollaborationAgreements
In order to register planning, programming and collaboration agreements
As a Professional Segment Balance User,
I want an UI to upload an Excel file with planning information,
programming and collaboration agreements

Background: Login
Given I am logged in as "profesional"

@testcase=21320 @ui
Scenario: Verify processed files in Planning, Programming And Collaboration Agreements for last 30 days
Given I have processed files information from the last 30 days
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
Then I should see the information of processed files "Planning, Programming And Collaboration Agreements" in the grid

@testcase=21321 @ui @manual @version=2
Scenario: Verify Planning, Programming And Collaboration Agreements Grid when files are not uploaded in the last 30 days
Given I have no information from the last 30 days
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
Then I should see error message "Sin registros"

@testcase=21322 @ui @bvt
Scenario Outline: Verify validations by uploading differnt types of files for Planning, Programming And Collaboration Agreements
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
And I click on "LoadNew" "button"
Then I should see upload new file interface
When I select "Planning, Programming and Agreements" from FileType dropdown
And I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload in planning, programming and collaboration agreements page
And I select "<FileType>" file from planning, programming and collaboration agreements directory
And I click on "uploadFile" "Submit" "button"
Then I should see error message "<Message>"

Examples:
| FileType            | Message                                                           |
| InvalidFormat       | Tipo de archivo no válido, debe ser archivo EXCEL.                |
| WithoutRecords      | El archivo debe tener por lo menos un registro                    |
| WithoutSourceColumn | La columna Unidad es requerida para el procesamiento del archivo. |


@testcase=21323 @ui
Scenario Outline: Verify file operations using different actions for Planning, Programming And Collaboration Agreements
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
And I click on "LoadNew" "button"
Then I should see upload new file interface
When I select "Planning, Programming and Agreements" from FileType dropdown
And I select "<Action>" from FileUpload dropdown
And I click on "Browse" to upload in planning, programming and collaboration agreements page
And I select "<FileType>" file from planning, programming and collaboration agreements directory
And I click on "uploadFile" "Submit" "button"
Then I should see the uploaded file in the Grid

Examples:
| Action | FileType   |
| Insert | ValidExcel |
| Update | ValidExcel |
| Delete | ValidExcel |


@testcase=21324 @ui
Scenario: Verify the Download files functionality for Planning, Programming And Collaboration Agreements
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
And I have a record with "Completed" state and I searched in "Planning, Programming And Collaboration Agreements" Grid
Then verify that "fileUploads" "Download" "link" is "enabled"
And verify that "fileUploads" "ViewError" "link" is "disabled"
When I click on "fileUploads" "Download" "link"
Then I should be able to download file successfully

@testcase=21325 @ui @version=2
Scenario: Verify the functionality when the generated file is in Processing state
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
And I have a record with "Processing" state and I searched in "Planning, Programming And Collaboration Agreements" Grid
Then verify that "fileUploads" "Download" "link" is "enabled"

@testcase=21326 @ui
Scenario: Verify the functionality when the uploaded file is in Completed with error state
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
And I have a record with "Error" state and I searched in "Planning, Programming And Collaboration Agreements" Grid
Then verify that "fileUploads" "Download" "link" is "enabled"
And verify that "fileUploads" "ViewError" "link" is "enabled"
When I click on "fileUploads" "ViewError" "link"
Then I should see "Control exceptions" header

@testcase=21327 @ui
Scenario: Verify the search filter functionality
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
And I click on "Search" "button"
Then I should see Search Interface
When I select start date on Search Interface
And I select end date greater than start date on Search Interface
And I click on "fileUploadGridFilter" "submit "button"
Then I should see the results based on the selected filter on Search Interface

@testcase=41512 
Scenario: Verify the search filter functionality when user selected date range more than 6 months
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
And I click on "Search" "button"
Then I should see Search Interface
When I select start date on Search Interface less than 6 months from current date
And I select end date greater than start date on Search Interface
And I click on "fileUploadGridFilter" "submit "button"
Then I should see the results based on the selected filter on Search Interface

@testcase=21328 @ui @version=2
Scenario: Verify Cancel functionality for File Upload Interface
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
And I click on "LoadNew" "button"
Then I should see upload new file interface
When I click on "uploadFile" "cancel" "button"
Then I should see breadcrumb "Cargue Otros Registros"

@testcase=21329 @ui
Scenario: Verify the mandatory fields on Search Interface
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
And I click on "Search" "button"
Then I should see Search Interface
When I do not enter values for Mandatory fields on Search Interface
And I click on "fileUploadGridFilter" "submit "button"
Then I should see the message on interface "Requerido"


@testcase=21330 @ui
Scenario: Verify the restart functionlaity on Search Interface
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
And I click on "Search" "button"
Then I should see Search Interface
When I enter values for Mandatory fields on Search Interface
And I click on "UploadFileFilter" "reset" "button"
Then I should see entered values in the fields are cleared

@testcase=21331 @ui @version=2
Scenario: Verify Cancel functionality for Search Interface
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
And I click on "Search" "button"
Then I should see Search Interface
When I click on "UploadFileFilter" "cancel" "button"
Then I should see breadcrumb "Cargue Otros Registros"

@testcase=21332 @ui
Scenario Outline: Verify the displayed columns on File Upload page for Planning, Programming And Collaboration Agreements
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
And the page is loaded into the UI
Then I should see the "<Columns>" on Events File Upload page

Examples:
| Columns              |
| Fecha                |
| Archivo              |
| Acción               |
| Usuario              |
| Estado               |
| Tipo                 |
| Registros procesados |

@testcase=21333 @ui
Scenario Outline: Verify Filters functionality
Given I have processed files information from the last 30 days
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
And the page is loaded into the UI
And I provide the value for "<Field>" "<ControlType>" filter in "Planning, Programming And Collaboration Agreements" Grid
Then I should see the information that matches the data entered for the "<Field>" in "Planning, Programming And Collaboration Agreements" Grid and sorted by descending date

Examples:
| Field                | ControlType |
| Fecha                | date        |
| Archivo              | textbox     |
| Acción               | combobox    |
| Usuario              | textbox     |
| Estado               | combobox    |
| Tipo                 | combobox    |
| Registros procesados | textbox     |

@testcase=21334
Scenario Outline: Verify Sorting functionality
Given I have processed files information from the last 30 days
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
And the page is loaded into the UI
And I click on the "<ColumnName>"
Then the results should be sorted based on "<ColumnName>" in "Planning, Programming And Collaboration Agreements" Grid

Examples:
| Columns           |
| Date              |
| Archive           |
| Action            |
| Username          |
| Status            |
| Type              |
| Records Processed |


@testcase=21335
Scenario: Verify Pagination functionality
Given I have processed files information from the last 30 days
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
And the page is loaded into the UI
And I navigate to second page in "Planning, Programming And Collaboration Agreements" Grid
Then the records should be displayed accordingly in "Planning, Programming And Collaboration Agreements" Grid
When I change the elements count per page to 50
Then the records count in "Planning, Programming And Collaboration Agreements" Grid shown per page should also be 50

@testcase=21336
Scenario: Verify the breadcrumb of Planning, Programming And Collaboration Agreements page
When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
Then I should see breadcrumb "Cargue Otros Registros"
