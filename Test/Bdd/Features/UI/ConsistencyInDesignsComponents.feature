@sharedsteps=4013 @owner=jagudelos @ui @testsuite=14723 @testplan=14709
Feature: ConsistencyInDesignsComponents
In order to make consistency in all Designs and Componenets
As a user
I need to see consistency in all designs and components

@testcase=16504 @ui @manual
Scenario: Validate Modal window of Uncertainity in connection attributes
	When I navigate to "ConfigureAttributesConnections" page
	When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
	Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
	When I click on "connAttributes" "Edit" "link"
	Then I should see 4 product associations relating to that "node-connection-products"
	When I click on "connectionProducts" "Edit" "link" of a combination having or not having value
	Then I should see "connectionProducts" "uncertainty" "interface"
	And validate "modal window" displayed as per style guide
	And validtate footer of the modal should have a padding of 51px

@testcase=16505 @ui @manual
Scenario: Validate Modal window of limit of control variable in connection attributes
	When I navigate to "ConfigureAttributesConnections" page
	When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
	Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
	When I click on "connAttributes" "Edit" "link" of a combination having or not having value
	Then I should see 4 product associations relating to that "node-connection-products"
	When I click on "connAttributes" "controlLimit" "edit" "link"
	Then I should see "connAttributes" "controlLimit" "interface"
	And validate "modal window" displayed as per style guide
	And validtate footer of the modal should have a padding of 51px

@testcase=16506 @ui @manual
Scenario: Validate Modal window of Uncertainity in node attributes
	When I navigate to "ConfigureAttributesNodes" page
	When I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	Then I should see a "NodeName" belongs to "node" in the grid
	When I click on "nodeAttributes" "Edit" "link"
	Then I should see 1 product associations relating to that "node-products"
	When I click on "NodeProducts" "Edit" "link" of a combination having or not having value
	Then I should see "NodeProducts" "uncertainty" "interface"
	And validate "modal window" displayed as per style guide
	And validtate footer of the modal should have a padding of 51px

@testcase=16507 @ui @manual
Scenario: Validate Modal window of limit of control variable in node attributes
	When I navigate to "ConfigureAttributesNodes" page
	When I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	Then I should see a "NodeName" belongs to "node" in the grid
	When I click on "nodeAttributes" "Edit" "link" of a combination having or not having value
	Then I should see 1 product associations relating to that "node-products"
	When I click on "nodeAttributes" "controlLimit" "edit" "link"
	Then I should see "nodeAttributes" "controlLimit" "interface"
	And validate "modal window" displayed as per style guide
	And validtate footer of the modal should have a padding of 51px

@testcase=16508 @ui @manual @version=2
Scenario: Validate Modal window of Edit Transport Category
	When I navigate to "Category" page
	And I search for existing "Category" record using filter
	And I click on "categories" "edit" "link" of any record
	Then I should see "Edit Category" interface
	And validate "modal window" displayed as per style guide
	And validtate footer of the modal should have a padding of 51px

@testcase=16509 @ui @manual @version=2
Scenario: Validate Modal window of Edit Category Element
	When I navigate to "Category Elements" page
	And I search for existing "CategoryElement" record using filter
	And I click on "elements" "edit" "link" of any record
	Then I should see "Edit Element" interface
	And validate "modal window" displayed as per style guide
	And validtate footer of the modal should have a padding of 51px

@testcase=16510 @ui @manual
Scenario Outline: Validate table grid in all module
	When I navigate to "<Module>" page
	Then validate font size displayed in 14px
	And validtate checkbox vertically aligned to the center in table header
	When I hover over the data with ellipses in grid
	Then I should see a tooltip containing the data in its entirety

	Examples:
		| Module                   |
		| node-connection          |
		| Categories               |
		| Node                     |
		| CategoryElements         |
		| ConfigureAttributesNodes |
		| Grouping Categories      |
		| UploadExcel              |

@testcase=16511 @ui @manual
Scenario: Validate Modals with graph
	When I navigate to "ConfigureAttributesConnections" page
	When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
	Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
	When I click on "connAttributes" "Edit" "link"
	Then I should see 4 product associations relating to that "node-connection-products"
	When I click on "connectionProducts" "ownership" "edit" "link" of a combination having or not having value
	Then I should see "connectionProducts" "ownership" "pie" "container"
	And graph should be centered within the modal
	When I click on "connectionProducts" "ownership" "pie" "submit" "button"
	Then I should see "connectionProducts" "owners" "select" "container"
	And validate "dualselectbox" displayed as per style guide