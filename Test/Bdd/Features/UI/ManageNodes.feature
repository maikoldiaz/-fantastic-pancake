@sharedsteps=16612 @Owner=jagudelos @ui @testplan=11317 @testsuite=11320
Feature: ManageNodes
In order to manage the transport nodes
As a TRUE Administrator
I want to User Interface to manage nodes

Background: Login
	Given I am authenticated as "admin"
	And I have "Node" in the system
	And I am logged in as "admin"

@testcase=12065 @bvt @version=2 @prodready
Scenario: Verify the Node management user Interface is displayed
	When I navigate to "CreateNodes" page
	And I click on "CreateNode" "button"
	Then I should see the "CreateNodeInterface"

@testcase=12066 @bvt @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Verify the Update Node user Interface is displayed
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	And I click on "Nodes" "Edit" "link"
	Then I should see the "UpdateNodeInterface"

@testcase=12067 @bvt @output=QueryAll(GetNodes) @version=4 @prodready
Scenario Outline: Verify the filtering functionality on Node management page
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	And I provided value for "Nodes" "<FieldName>" "name" "textbox"
	Then I should see the records filtered as per the search criteria

	Examples:
		| FieldName |
		| Segment   |
		| Name      |
		| NodeType  |
		| Operator  |

@testcase=12069 @version=5 @prodready
Scenario Outline: Verify the sorting functionality on Node management page
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	And I click on the "<ColumnName>" in Grid
	Then the results should be sorted based on "<ColumnName>" in "Nodes" Grid

	Examples:
		| ColumnName |
		| Segmento   |
		| Nombre     |
		| Tipo       |
		| Operador   |

@testcase=12070 @version=2 @prodready
Scenario: Verify the message on Node management page for the first time
	When I navigate to "CreateNodes" page
	Then I should see the "NodeInformation" interface
	And I should see message "Sin registros"

@testcase=12071 @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Verify that atleast one Segment selectd in the search filter
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see the results based on the selected filter

@testcase=12072 @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Verify the search filter functionality
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	When I select "SegmentElement" from "Segments" filter
	And I select Type from "NodeGridFilter" "NodeType" "combobox"
	And I select Operator from "NodeGridFilter" "Operator" "combobox"
	And I click on "NodeGridFilter" "Apply" "button"
	Then I should see the results based on the selected filter

@testcase=12073 @output=QueryAll(GetNodes) @version=3 @prodready
Scenario: Verify Pagination functionality
	Given I have "Nodes" in the system
	When I navigate to "CreateNodes" page
	And I click on "Search" "button"
	And I select a Segment from "NodeGridFilter" "Segment" "combobox"
	And I click on "NodeGridFilter" "Apply" "button"
	When I change the elements count per page to 50
	Then the records count in "Nodes" Grid shown per page should also be 50