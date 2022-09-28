@sharedsteps=16581 @owner=jagudelos @ui @testsuite=39242 @testplan=39221 @parallel=false
Feature: StartDateOfNodeToBelessThanNodeCreationDate
As a Balance Segment Professional User,
I need the start date of a node in a group to be less than the node creation date
to allow storing operational data from previous days.

@testcase=41489 @output=QueryAll(GetNodes) @version=2
Scenario: Validate start date of a node is less than N months of the current date
	Given I am logged in as "Admin"
	When I create Node in the system
	And I get start date from node tag
	Then start date of node type group is "N" months less than current date
	| N |
	| 6 |
	And start date of node in segment group is "N" months less than current date
	| N |
	| 6 |
	And start date of node in operator group is "N" months less than current date
	| N |
	| 6 |

@testcase=41490 @version=2 @bvt1.5
Scenario: Validate excel inventory registration with inventory date 2 days less than current date
	Given I am logged in as "profesional"
	And I want to register a "Homologation" in the system
	When I navigate to "FileUpload" page
	And I update the excel with "TestDataCutOff_singleInventory" data
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestDataCutOff_singleInventory" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then it should be processed 

@testcase=41491 @version=2 @bvt1.5
Scenario: Validate sinoper inventory registration with inventory date less than current date
	Given I am authenticated as "admin"
	And I want to register an "Inventory" in the system
	And I set the date value in the xml to 2 days less than current date
	Then it should be registered

@testcase=41492 @version=2 @bvt1.5
Scenario: Register Sap inventory with inventory date less than current date
	Given I am authenticated as "admin"
	And I have data to process "Inventories" in system
	When I set inventory date as 2 days less than current date
	And I register "Inventories" in system
	Then response should be successful

@testcase=41493 @version=2 @bvt1.5
Scenario: Validate excel movement registration with operative date less than current date
	Given I am logged in as "profesional"
	And I want to register a "Homologation" in the system
	When I navigate to "FileUpload" page
	And I update the excel with "TestDataCutOff_singleMovement" data
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "TestDataCutOff_singleMovement" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then it should be processed 

@testcase=41494 @version=2
Scenario: Validate sinoper movement registration with operative date less than current date
	Given I am authenticated as "admin"
	And I set the date value for movement in the xml to 2 days less than current date
	When the "EventType" field is equal to "Insert"
	Then it should be registered

@testcase=41495 @version=2 @bvt1.5
Scenario: Validate SAP Movement with operative date less than current date
	Given I am logged in as "profesional"
	And I have data to process "Movements" in system
	When I have 1 movement
	And I set operative date to 2 days less than current date
	And I register "Movements" in system
	Then response should be successful

@testcase=41496 @version=2
Scenario: Validate inventory registration failed when inventory date less than current date
	Given I am logged in as "profesional"
	And I want to register a "Homologation" in the system
	When I navigate to "FileUpload" page
	And I update the excel with "SingleInventory_DateLessThanNodeDate" data
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "SingleInventory_DateLessThanNodeDate" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then it should not be registered in the system

@testcase=41497 @version=2
Scenario: Validate movement registration failed when operative date less than current date
	Given I am logged in as "profesional"
	And I want to register a "Homologation" in the system
	When I navigate to "FileUpload" page
	And I update the excel with "SingleMovement_DateLessThanNodeDate" data
	And I click on "FileUpload" "button"
	And I select segment from "FileUpload" "segment" "dropdown"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "SingleMovement_DateLessThanNodeDate" file from directory
	And I click on "uploadFile" "Submit" "button"
	Then it should not be registered in the system