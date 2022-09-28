@owner=jagudelos @ui @testsuite=31120 @testplan=31102 @parallel=false
Feature: DataToBeFilteredOnTheExceptionManagementPage
As a Balance Segment Professional User, I need data to be
filtered on the exception management page to display only
messages from a specific process

@testcase=33789 @version=2
Scenario: Verify exception management page to display only messages for specific file when user clicked on view error link of failed record in movement and inventory upload page
	Given I have "ownershipnodes" created
	When I click on "FileUploads" "ViewError" "Link"
	And records should be displayed for selected file
	And I should see "Return" "button"

@testcase=33790 @bvt @version=2 @bvt1.5
Scenario: Verify exception management page to display only messages for specific file when user clicked on view error link of failed record in contract and events upload page
	Given I am logged in as "profesional"
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	Then I should see upload new file interface
	When I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "InvalidExcel" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	And I click on "File Uploads" "View Error" "Link"
	And records should be displayed for selected file
	And I should see "Return to list" "button"

@testcase=33791 @bvt @version=2 @bvt1.5
Scenario: Verify return to list button functionality when user navigated from movement and inventory upload page
	Given I am logged in as "profesional"
	And I have "ownershipnodes" created
	When I click on "FileUploads" "ViewError" "Link"
	Then I click on "Return" "button"
	And I should see the "FileUpload" page

@testcase=33792 @bvt @version=2 @bvt1.5
Scenario: Verify return to list button functionality when user navigated from contract and events upload page
	Given I am logged in as "profesional"
	When I navigate to "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page
	And I click on "LoadNew" "button"
	Then I should see upload new file interface
	When I select "Planning, Programming and Agreements" from FileType dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload in planning, programming and collaboration agreements page
	And I select "InvalidExcel" file from planning, programming and collaboration agreements directory
	And I click on "uploadFile" "Submit" "button"
	And I click on "FileUploads" "ViewError" "Link"
	And I click on "Return" "button"
	Then I should see the "FileUploadForPlanningAndProgrammingAndCollaborationAgreements" page

@testcase=33793 @version=2 @priority=2
Scenario: Verify exceptions management page when logged in as Administrator role
	Given I am logged in as "administrador"
	When I navigate to "Exceptions" page
	Then "Return to list" "button" should not be displayed
	And I should see the errors information in the "Exceptions" grid for last 40 days

@testcase=33794 @version=2 @priority=2
Scenario: Verify exceptions management page when logged in as Profesional role
	Given I am logged in as "profesional"
	When I navigate to "Exceptions" page
	Then "Return to list" "button" should not be displayed
	And I should see the errors information in the "Exceptions" grid for last 40 days