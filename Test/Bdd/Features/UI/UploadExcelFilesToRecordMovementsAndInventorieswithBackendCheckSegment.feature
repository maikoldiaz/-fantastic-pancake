@sharedsteps=16581 @owner=jagudelos @ui @testplan=14709 @testsuite=14728
Feature: UploadExcelFilesToRecordMovementsAndInventorieswithBackendCheckSegment
In order to register a Movement or Inventory in the system
As a transport segment user
I want to upload the Excel files from UI with Segment

Background: Login
Given I am logged in as "profesional"

@testcase=16883 @bvt @version=3 @prodready
Scenario: Validate a new Movement or Inventory inserted in database by uploading Excel and selecting Segment
Given I want to register a "Homologation" in the system
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select segment from "FileUpload" "segment" "dropdown"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then it should be registered in the system

@testcase=25236 @bvt @prodready
Scenario: Validate a new Movement or Inventory updated in database by uploading Excel and selecting Segment
Given I have valid Movements and Inventory in the system
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select same segment from "FileUpload" "segment" "dropdown"
When I select "Update" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then it should be registered in the system

@testcase=25237 @bvt @prodready
Scenario: Validate a new Movement or Inventory deleted in database by uploading Excel and selecting Segment
Given I have valid Movements and Inventory in the system
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select same segment from "FileUpload" "segment" "dropdown"
When I select "Delete" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then it should be registered in the system

@testcase=16884
Scenario: Validate a new Movement or Inventory entry in database by uploading incorrect Excel file for Segment category
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select segment from "FileUpload" "segment" "dropdown"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "InvalidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then I should see error message "El segmento elegido en el sitio y los valores para segmento de los nodos origen y destino no coinciden"
