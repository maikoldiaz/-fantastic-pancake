@sharedsteps=4013 @owner=jagudelos @ui @testsuite=6701 @testplan=6671 @souryanewui
Feature: NodeAttributesConfiguration
In order to complete the balance
As an application administrator
I want to configure the Node Attributes

Background: Login
	Given I am logged in as "admin"

@testcase=7621 @ui @output=QueryAll(GetNodes) @prodready
Scenario: Querying Nodes with a combination of one filter
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "Search" "button"
	When I select "Segmento" from "Category" filter
	And I select "SegmentElement" from "Element" filter
	And I click on "nodeAttributes" "apply" "button" in filter
	Then I should see the list of "nodeAttribute" that meets the filter conditions

@testcase=7622 @ui @output=QueryAll(GetNodes) @prodready
Scenario: Querying Nodes with a combination of two filters with AND condition
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "Search" "button"
	When I select "Segmento" from "Category" filter
	And I select "SegmentElement" from "Element" filter
	And I click on "nodeAttributes" "and" "button"
	When I select "Operador" from "Category" filter
	And I select "OperatorElement" from "Element" filter
	And I click on "nodeAttributes" "apply" "button" in filter
	Then I should see the list of "nodeAttribute" that meets the filter conditions

@testcase=7623 @ui @output=QueryAll(GetNodes) @prodready
Scenario: Querying Nodes with a combination of two filters with OR condition
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "Search" "button"
	When I select "Segmento" from "Category" filter
	And I select "SegmentElement" from "Element" filter
	And I click on "nodeAttributes" "or" "button"
	When I select "Operador" from "Category" filter
	And I select "OperatorElement" from "Element" filter
	And I click on "nodeAttributes" "apply" "button" in filter
	Then I should see the list of "nodeAttribute" that meets the filter conditions

@testcase=7624 @bvt @ui @output=QueryAll(GetNodes) @prodready
Scenario: Querying Nodes with a combination of three filters with AND and OR condition
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "Search" "button"
	When I select "Segmento" from "Category" filter
	And I select "SegmentElement" from "Element" filter
	And I click on "nodeAttributes" "or" "button"
	When I select "Operador" from "Category" filter
	And I select "OperatorElement" from "Element" filter
	And I click on "nodeAttributes" "and" "button"
	When I select "Tipo de Nodo" from "Category" filter
	And I select "NodeTypeElement" from "Element" filter
	And I click on "nodeAttributes" "apply" "button" in filter
	Then I should see the list of "nodeAttribute" that meets the filter conditions

@testcase=7625 @ui @output=QueryAll(GetNodes) @prodready
Scenario: Querying Nodes with a combination of three filters with OR and AND condition
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "Search" "button"
	When I select "Segmento" from "Category" filter
	And I select "SegmentElement" from "Element" filter
	And I click on "nodeAttributes" "and" "button"
	When I select "Operador" from "Category" filter
	And I select "OperatorElement" from "Element" filter
	And I click on "nodeAttributes" "or" "button"
	When I select "Tipo de Nodo" from "Category" filter
	And I select "NodeTypeElement" from "Element" filter
	And I click on "nodeAttributes" "apply" "button" in filter
	Then I should see the list of "nodeAttribute" that meets the filter conditions

@testcase=7626 @ui @output=QueryAll(GetNodes) @prodready
Scenario: Querying Nodes with a combination of three filters with AND and AND condition
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "Search" "button"
	When I select "Segmento" from "Category" filter
	And I select "SegmentElement" from "Element" filter
	And I click on "nodeAttributes" "and" "button"
	When I select "Operador" from "Category" filter
	And I select "OperatorElement" from "Element" filter
	And I click on "nodeAttributes" "and" "button"
	When I select "Tipo de Nodo" from "Category" filter
	And I select "NodeTypeElement" from "Element" filter
	And I click on "nodeAttributes" "apply" "button" in filter
	Then I should see the list of "nodeAttribute" that meets the filter conditions

@testcase=7627 @ui @output=QueryAll(GetNodes) @prodready
Scenario: Querying Nodes with a combination of three filters with OR and OR condition
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "Search" "button"
	When I select "Segmento" from "Category" filter
	And I select "SegmentElement" from "Element" filter
	And I click on "nodeAttributes" "or" "button"
	When I select "Operador" from "Category" filter
	And I select "OperatorElement" from "Element" filter
	And I click on "nodeAttributes" "or" "button"
	When I select "Tipo de Nodo" from "Category" filter
	And I select "NodeTypeElement" from "Element" filter
	And I click on "nodeAttributes" "apply" "button" in filter
	Then I should see the list of "nodeAttribute" that meets the filter conditions

@testcase=7628 @bvt @ui @output=QueryAll(GetNodes) @prodready
Scenario: Verify the list of product associations relating to the node
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	When I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	Then I should see a "NodeName" belongs to "nodeAttribute" in the grid
	When I click on "nodeAttributes" "Edit" "link"
	Then I should see 1 product associations relating to that "node-products"

@testcase=7629 @ui @output=QueryAll(GetNodes)
Scenario: Entry or Edit Uncertainty variable
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	When I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	Then I should see a "NodeName" belongs to "nodeAttribute" in the grid
	When I click on "nodeAttributes" "Edit" "link"
	Then I should see 1 product associations relating to that "node-products"
	When I click on "NodeProducts" "Edit" "link" of a combination having or not having value
	Then I should see "NodeProducts" "uncertainty" "interface"
	When I clear value into "NodeProducts" "uncertainty" "textbox"
	Then I should see error message "Requerido"
	When I enter valid value into "NodeProducts" "uncertainty" "textbox"
	And I click on "NodeProducts" "uncertainty" "submit" "button"
	Then the new value should be updated in "NodeProducts" "UncertaintyPercentage" "Label"

@testcase=7630 @ui @output=QueryAll(GetNodes) @prodready
Scenario: Entry or Edit Limit of Control variable
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	When I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	Then I should see a "NodeName" belongs to "nodeAttribute" in the grid
	When I click on "nodeAttributes" "Edit" "link" of a combination having or not having value
	Then I should see 1 product associations relating to that "node-products"
	When I click on "nodeAttributes" "controlLimit" "edit" "link"
	Then I should see "nodeAttributes" "controlLimit" "interface"
	When I clear value into "nodeAttributes" "controlLimit" "textbox"
	Then I should see error message "Requerido"
	When I enter valid value into "nodeAttributes" "controlLimit" "textbox"
	When I enter valid value into "nodeAttributes" "AcceptableBalance" "textbox"
	And I click on "nodeAttributes" "controlLimit" "submit" "button"
	Then the new value should be updated in "nodeAttributes" "controlLimit" "Label"

@testcase=7631 @ui @output=QueryAll(GetNodes) @prodready
Scenario: Entry or Edit Acceptable Balance Percentage variable
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	When I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	Then I should see a "NodeName" belongs to "nodeAttribute" in the grid
	When I click on "nodeAttributes" "Edit" "link" of a combination having or not having value
	Then I should see 1 product associations relating to that "node-products"
	When I click on "nodeAttributes" "AcceptableBalance" "edit" "link"
	Then I should see "nodeAttributes" "controlLimit" "interface"
	When I clear value into "nodeAttributes" "AcceptableBalance" "textbox"
	Then I should see error message "Requerido"
	When I enter valid value into "nodeAttributes" "AcceptableBalance" "textbox"
	When I enter valid value into "nodeAttributes" "controlLimit" "textbox"
	And I click on "nodeAttributes" "controlLimit" "submit" "button"
	Then the new value should be updated in "nodeAttributes" "AcceptableBalance" "Label"

@testcase=7632 @bvt @ui @output=QueryAll(GetNodes) @prodready
Scenario: Entry or Edit Ownership variable
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	When I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	Then I should see a "NodeName" belongs to "nodeAttribute" in the grid
	When I click on "nodeAttributes" "Edit" "link"
	Then I should see 1 product associations relating to that "node-products"
	When I click on "NodeProducts" "ownership" "edit" "link" of a combination having or not having value
	Then I should see "NodeProducts" "owners" "select" "container"
	When I click on "NodeProducts" "owners" "source" "moveAll" "button"
	When I click on "NodeProducts" "owners" "select" "Next" "button"
	Then I should see "NodeProducts" "ownership" "interface"
	When I enter values for all the "NodeProducts" associated owners so that the sum of them is equal to 200
	Then I should see error message "La sumatoria de los valores de propiedad debe ser 100%"
	When I enter values for all the "NodeProducts" associated owners so that the sum of them is equal to 80
	Then I should see error message "La sumatoria de los valores de propiedad debe ser 100%"
	When I enter values for all the "NodeProducts" associated owners so that the sum of them is equal to 100
	And I click on "NodeProducts" "ownership" "submit" "button"

@testcase=7633 @ui @output=QueryAll(GetNodes)
Scenario Outline: Verify Filters functionality
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	When I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	Then I should see a "NodeName" belongs to "nodeAttribute" in the grid
	When I click on "nodeAttributes" "Edit" "link"
	Then I should see 1 product associations relating to that "node-products"
	When I click on "nodeAttributes" "controlLimit" "edit" "link"
	Then I should see "nodeAttributes" "controlLimit" "interface"
	When I enter valid value into "nodeAttributes" "controlLimit" "textbox"
	When I enter valid value into "nodeAttributes" "AcceptableBalance" "textbox"
	And I click on "nodeAttributes" "controlLimit" "submit" "button"
	Then I navigate to "ConfigureAttributesNodes"
	When I provide "<FieldValue>" for "nodeAttributes" "<Field>" "textbox" filter
	Then I should see a "<FieldValue>" belongs to "nodeAttribute" in the grid

	Examples:
		| Field                       | FieldValue                  |
		| Name                        | NodeName                    |
		| ControlLimit                | ControlLimit                |
		| AcceptableBalancePercentage | AcceptableBalancePercentage |

@testcase=7634 @ui @manual
Scenario Outline: Verify Filters functionality when no records found
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	When I provide "<Field>" value for "nodeAttributes" "<Field>" "textbox" filter that doesn't matches with any record
	Then I should see message "No hay nodos"

	Examples:
		| Field             |
		| nodeId            |
		| name              |
		| controlLimit      |
		| acceptableBalance |