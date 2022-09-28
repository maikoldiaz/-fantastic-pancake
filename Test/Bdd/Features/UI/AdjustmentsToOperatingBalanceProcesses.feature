@sharedsteps=29511 @ui @owner=jagudelos @testplan=26817 @testsuite=26833 @parallel=false
Feature: AdjustmentsToOperatingBalanceProcesses
As a Professional Segment Balance User, I require
some adjustments to the daily operating balance processes
to the data will be consistent

Background: Login
	Given I am logged in as "admin"

@testcase=29512 @bvt @ui @version=2 @bvt1.5
Scenario: I upload an excel with movements and inventories with final date less than start date
	Given I update data in "MovementValidation"
	When I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "MovementValidation" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	Then it must be stored in a Pendingtransactions repository with validation "La fecha final de la operación debe ser mayor o igual a la fecha inicial."

@testcase=29513 @bvt @ui @output=QueryAll(GetNodes) @version=3 @bvt1.5
Scenario: As a TRUE user i should see an error message when i try to create node tags with final date less than start date
	Given I create Nodes in the system
	When I navigate to "Grouping Categories" page
	And I click on "Search" "button"
	And I select any "Node Type" from "NodeTags" "category" "dropdown"
	And I select any "Value" from "NodeTags" "element" "dropdown"
	And I click on "NodeTags" "apply" "button"
	And I select required Nodes from "Nodes" "checkbox"
	And I select any "ChangeValue" from "GroupingTitle" "dropdown"
	And I select new "CategoryElement" from dropdown
	And I select Current Date from "NodeTag" "operationDate" "datepicker"
	And I click on "NodeTag" "save" "button"
	Then I should see the error message "La fecha final de la asociación debe ser mayor que la fecha inicial."

@testcase=29514 @ui @output=QueryAll(GetNodes) @priority=2 @version=3
Scenario: As a TRUE user I should see an error message when I try to update a node with an association to a new segment before the end date of its previous segment
	Given I create Nodes in the system
	When I navigate to "Grouping Categories" page
	And I click on "Search" "button"
	And I select any "Node Type" from "NodeTags" "category" "dropdown"
	And I select any "Value" from "NodeTags" "element" "dropdown"
	And I click on "NodeTags" "apply" "button"
	And I select required Nodes from "Nodes" "checkbox"
	And I select any "ChangeValue" from "GroupingTitle" "dropdown"
	And I select Current Date from "NodeTag" "operationDate" "datepicker"
	And I click on "NodeTag" "save" "button"
	Then I should see the error message "La fecha final de la asociación debe ser mayor que la fecha inicial."

@testcase=30332 @version=3 @ui
Scenario: I upload to SINOPER with Movements with final date less than start date
	Given I want to register an "Movements" in the system
	When I receive invalid "EndTime" in "Movements" XML
	And the "EventType" field is equal to "Insert"
	Then it should be registered

@testcase=37315
Scenario: I upload to SINOPER with Inventories with final date less than start date
	Given I want to register an "Inventories" in the system
	When I receive invalid "EndTime" in "Inventories" XML
	And the "EventType" field is equal to "Insert"
	Then it should be registered