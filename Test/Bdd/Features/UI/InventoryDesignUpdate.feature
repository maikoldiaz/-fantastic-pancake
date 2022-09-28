@owner=jagudelos @testsuite=39224 @testplan=39221 @parallel=false
Feature: InventoryDesignUpdate
In order to store an inventory by product
As a TRUE system
I need to update the inventory design

@testcase=41425 @backend @version=2
Scenario: Register an inventory with many products by SINOPER
	Given I am authenticated as "admin"
	And I want to register an "Inventory_MultipleProduct" in the system
	When it meets "all" input validations with multiple products
	And the "EventType" field is equal to "Insert"
	And it should be registered
	Then validate inventory is registered with "many" products

@testcase=41426 @ui @version=2 @bvt1.5
Scenario: Register an inventory by Excel
	Given I want to register a "Homologation" in the system
	And I update data in "InventoryWithManyProducts"
	When I am logged in as "admin"
	And I navigate to "FileUpload" page
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "Segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "InventoryWithManyProducts" file from directory
	And I click on "uploadFile" "Submit" "button"
	And I wait till file upload to complete
	Then validate inventory is registered with "many" products

@testcase=41427 @api @version=2 @bvt1.5
Scenario: Register an inventory by SAP PO
	Given I am authenticated as "admin"
	And I have data to process "Inventories" in system
	When I have 1 inventory with "many" products
	And I register "Inventories" in system
	Then response should be successful
	And validate inventory is registered with "many" products

@testcase=41428 @ui @bvt1.5 @version=2
Scenario: Validate the unbalance in operational cutoff
	Given I want to calculate the daywise "OperationalBalance" in the system
	When I navigate to "Operational Cutoff" page
	And I click on "NewCut" "button"
	And I choose CategoryElement from "InitTicket" "Segment" "combobox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
	And I click on "InitTicket" "submit" "button"
	And validate that "validateInitialInventory" "submit "button" as enabled
	And I click on "validateInitialInventory" "submit "button"
	And I select all pending records from grid
	And I click on "ErrorsGrid" "AddNote" "button"
	And I enter valid value into "AddComment" "Comment" "textbox"
	And I click on "AddComment" "Submit" "button"
	And validate that "ErrorsGrid" "Submit" "button" as enabled
	And I click on "ErrorsGrid" "Submit" "button"
	And I click on "officialPointsGrid" "submit" "button"
	Then validate the unbalances are calculated in the grid

@testcase=41429 @ui @manual
Scenario:Validate the operational report without cutoff
	Given I have "Segment" for the Operational Report
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I enter "NodeName" into "Node" "Name" "textbox"
	And I click on "WithoutCutoff " "RadioButton"
	And I select the StartDate lessthan "2" days from CurrentDate on "Report" DatePicker
	And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
	And I click on "ReportFilter" "ViewReport" "button"
	And I should see "OperationalReport" "Screen"
	And validate "initial inventories" calculated on first day of the period
	And validate "final inventories" calculated on last day of the period
	And validate all the products are displayed
	And I click on "InventoryDetails" "tab"
	And validate "inventory details" are displayed as expected
	And I click on "InventoryQualityDetails" "tab"
	Then validate "inventory Quality details" are displayed as expected

@testcase=41430 @ui @manual
Scenario:Validate the operational report without ownership
	Given I have operating balance without ownership calculated for system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I enter "NodeName" into "Node" "Name" "textbox"
	And I click on "OperatinalReportWithoutOwnership " "RadioButton"
	And I select the StartDate lessthan "2" days from CurrentDate on "Report" DatePicker
	And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
	And I click on "ReportFilter" "ViewReport" "button"
	And I should see "OperationalReport" "Screen"
	And validate "initial inventories" calculated on first day of the period
	And validate "final inventories" calculated on last day of the period
	And validate all the products are displayed
	And I click on "InventoryDetails" "tab"
	And validate "inventory details" are displayed as expected
	And I click on "InventoryQualityDetails" "tab"
	Then validate "inventory Quality details" are displayed as expected

@testcase=41431 @ui @manual
Scenario:Validate the operational report with ownership
	Given I have "ownershipnodes" created
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I enter "NodeName" into "Node" "Name" "textbox"
	And I click on "OperatinalReportWithOwnership " "RadioButton"
	And I select the StartDate lessthan "2" days from CurrentDate on "Report" DatePicker
	And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
	And I click on "ReportFilter" "ViewReport" "button"
	And I should see "OperationalReport" "Screen"
	And validate "initial inventories" calculated on first day of the period
	And validate "final inventories" calculated on last day of the period
	And validate all the products are displayed
	And I click on "InventoryDetails" "tab"
	And validate "inventory details" are displayed as expected
	And I click on "InventoryQualityDetails" "tab"
	Then validate "inventory Quality details" are displayed as expected