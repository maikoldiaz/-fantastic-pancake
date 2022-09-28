@sharedsteps=4013 @owner=jagudelos @ui @testsuite=6702 @testplan=6671
Feature: ManageConnectionsAttributes
In order to complete the balance
As an application administrator
I want to configure the Connection Attributes

Background: Login
	Given I am logged in as "admin"

@testcase=7565 @ui @output=QueryAll(GetNodeConnection) @bvt
Scenario: Query Connections with a combination of one filter
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	And I click on "Search" "button"
	When I select "Segmento" from "Category" filter
	And I select "SegmentElement" from "Element" filter
	And I click on "connAttributes" "apply" "button" in filter
	Then I should see the list of "node-connection" that meets the filter conditions

@testcase=7566 @ui @output=QueryAll(GetNodeConnection)
Scenario: Query Connections with a combination of two filters with AND condition
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	And I click on "Search" "button"
	When I select "Segmento" from "Category" filter
	And I select "SegmentElement" from "Element" filter
	And I click on "connAttributes" "and" "button" condition in filter
	When I select "Operador" from "Category" filter
	And I select "OperatorElement" from "Element" filter
	And I click on "connAttributes" "apply" "button" in filter
	Then I should see the list of "node-connection" that meets the filter conditions

@testcase=7567 @ui @output=QueryAll(GetNodeConnection)
Scenario: Query Connections with a combination of two filters with OR condition
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	And I click on "Search" "button"
	When I select "Segmento" from "Category" filter
	And I select "SegmentElement" from "Element" filter
	And I click on "connAttributes" "or" "button" condition in filter
	When I select "Operador" from "Category" filter
	And I select "OperatorElement" from "Element" filter
	And I click on "connAttributes" "apply" "button" in filter
	Then I should see the list of "node-connection" that meets the filter conditions

@testcase=7568 @ui @output=QueryAll(GetNodeConnection)
Scenario: Query Connections with a combination of three filters with AND condition
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	And I click on "Search" "button"
	When I select "Segmento" from "Category" filter
	And I select "SegmentElement" from "Element" filter
	And I click on "connAttributes" "and" "button" condition in filter
	When I select "Operador" from "Category" filter
	And I select "OperatorElement" from "Element" filter
	And I click on "connAttributes" "and" "button" condition in filter
	When I select "Tipo de Nodo" from "Category" filter
	And I select "NodeTypeElement" from "Element" filter
	And I click on "connAttributes" "apply" "button" in filter
	Then I should see the list of "node-connection" that meets the filter conditions

@testcase=7569 @ui @output=QueryAll(GetNodeConnection)
Scenario: Query Connections with a combination of three filters with OR condition
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	And I click on "Search" "button"
	When I select "Segmento" from "Category" filter
	And I select "SegmentElement" from "Element" filter
	And I click on "connAttributes" "or" "button" condition in filter
	When I select "Operador" from "Category" filter
	And I select "OperatorElement" from "Element" filter
	And I click on "connAttributes" "or" "button" condition in filter
	When I select "Tipo de Nodo" from "Category" filter
	And I select "NodeTypeElement" from "Element" filter
	And I click on "connAttributes" "apply" "button" in filter
	Then I should see the list of "node-connection" that meets the filter conditions

@testcase=7570 @ui @bvt @output=QueryAll(GetNodeConnection)
Scenario: Query Connections with a combination of three filters with AND and OR condition
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	And I click on "Search" "button"
	When I select "Segmento" from "Category" filter
	And I select "SegmentElement" from "Element" filter
	And I click on "connAttributes" "or" "button" condition in filter
	When I select "Operador" from "Category" filter
	And I select "OperatorElement" from "Element" filter
	And I click on "connAttributes" "and" "button" condition in filter
	When I select "Tipo de Nodo" from "Category" filter
	And I select "NodeTypeElement" from "Element" filter
	And I click on "connAttributes" "apply" "button" in filter
	Then I should see the list of "node-connection" that meets the filter conditions

@testcase=7571 @ui @bvt @output=QueryAll(GetNodeConnection)
Scenario: Verify the list of product associations relating to the connections
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
	Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
	When I click on "connAttributes" "Edit" "link"
	Then I should see 1 product associations relating to that "node-connection-products"

@testcase=7572 @ui @bvt @output=QueryAll(GetNodeConnection)
Scenario: Entry or Edit Uncertainty variable
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
	Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
	When I click on "connAttributes" "Edit" "link"
	Then I should see 1 product associations relating to that "node-connection-products"
	When I click on "connectionProducts" "Edit" "link" of a combination having or not having value
	Then I should see "connectionProducts" "uncertainty" "interface"
	When I clear value into "connectionProducts" "uncertainty" "textbox"
	Then I should see error message "Requerido"
	When I enter valid value into "connectionProducts" "uncertainty" "textbox"
	And I click on "connectionProducts" "uncertainty" "submit" "button"
	Then the new value should be updated in "connectionProducts" "UncertaintyPercentage" "Label"

@testcase=7573 @ui @bvt @output=QueryAll(GetNodeConnection)
Scenario: Entry or Edit Limit of Control variable
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
	Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
	When I click on "connAttributes" "Edit" "link" of a combination having or not having value
	Then I should see 1 product associations relating to that "node-connection-products"
	When I click on "connAttributes" "controlLimit" "edit" "link"
	Then I should see "connAttributes" "controlLimit" "interface"
	When I clear value into "connAttributes" "controlLimit" "textbox"
	Then I should see error message "Requerido"
	When I enter valid value into "connAttributes" "controlLimit" "textbox"
	And I click on "connAttributes" "controlLimit" "submit" "button"
	Then the new value should be updated in "connAttributes" "controlLimit" "Label"

@testcase=7574 @ui @bvt @bvt @output=QueryAll(GetNodeConnection)
Scenario: Entry or Edit Ownership variable
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
	Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
	When I click on "connAttributes" "Edit" "link"
	Then I should see 1 product associations relating to that "node-connection-products"
	When I click on "connectionProducts" "ownership" "edit" "link" of a combination having or not having value
	Then I should see "connectionProducts" "ownership" "pie" "container"
	When I click on "connectionProducts" "ownership" "pie" "submit" "button"
	Then I should see "connectionProducts" "owners" "select" "container"
	When I click on "connectionProducts" "owners" "source" "moveAll" "button"
	When I click on "connectionProducts" "owners" "select" "Next" "button"
	Then I should see "connectionProducts" "ownership" "interface"
	When I enter values for all the "ConnectionProducts" associated owners so that the sum of them is equal to 200
	Then I should see error message "La sumatoria de los valores de propiedad debe ser 100%"
	When I enter values for all the "ConnectionProducts" associated owners so that the sum of them is equal to 80
	Then I should see error message "La sumatoria de los valores de propiedad debe ser 100%"
	When I enter values for all the "ConnectionProducts" associated owners so that the sum of them is equal to 100
	And I click on "connectionProducts" "ownership" "submit" "button"

@testcase=7575 @ui @output=QueryAll(GetNodeConnection)
Scenario Outline: Verify Filters functionality
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	When I provide "<FieldValue>" for "connAttributes" "<Field>" "Name" "textbox" filter
	Then I should see a "<FieldValue>" belongs to "node-connection" in the grid

	Examples:
		| Field           | FieldValue          |
		| SourceNode      | SourceNodeName      |
		| DestinationNode | DestinationNodeName |

@testcase=8655 @ui @output=QueryAll(GetNodeConnection)
Scenario: Verify control limit filter functionality
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	When I provide "ControlLimit" for "connAttributes" "ControlLimit" "textbox" filter
	Then I should see a "ControlLimit" belongs to "node-connection" in the grid

@testcase=7576 @ui @manual
Scenario Outline: Verify Filters functionality when no records found
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	When I provide "<Field>" value for "connAttributes" "<Field>" "textbox" filter that doesn't matches with any record
	Then I should see message "No hay conexión de nodos"

	Examples:
		| Field               |
		| sourceNodeName      |
		| destinationNodeName |
		| controlLimit        |