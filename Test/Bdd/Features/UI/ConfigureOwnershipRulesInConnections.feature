@sharedsteps=4013 @owner=jagudelos @ui  @testsuite=8493 @testplan=8481
Feature: ConfigureOwnershipRulesInConnections
In order to calculate the Ownership of Hydrocarbons
As a True AdministratorconnectionAttributes
I need UI to Configure Ownership rules in Connections per Product

Background: Login
	Given I am logged in as "admin"

@testcase=9892 @bvt @output=QueryAll(GetNodeConnection) @version=5 @ignore
Scenario: Entry or Edit of New Property Function rule
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
	Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
	When I click on "connAttributes" "Edit" "link"
	Then I should see 1 product associations relating to that "node-connection-products"
	When I click on "connectionProducts" "editRules" "link" of a combination not having value
	Then I should see "Edit Property Information" interface
	When I click on "connAttributes" "info" "submit" "button"
	Then I should see the message "Requerido"
	When I select "S - Por Entradas" from "New Property Function"
	And I click on "connAttributes" "info" "submit" "button"
	Then the changes should be updated in "New Property Function"
	When I click on "connectionProducts" "editRules" "link" of a combination having value
	And I select "S - Por Disponible Mensual" from "New Property Function"
	And I click on "connAttributes" "info" "submit" "button"
	Then the changes should be updated in "New Property Function"

@testcase=9893 @bvt @output=QueryAll(GetNodeConnection) @version=4 @prodready
Scenario: Entry or Edit of Priority Connection of Product
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
	Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
	When I click on "connAttributes" "Edit" "link"
	Then I should see 1 product associations relating to that "node-connection-products"
	When I click on "connectionProducts" "editRules" "link" of a combination not having value
	Then I should see "Edit Property Information" interface
	When I provide integer value for "connAttributes" "Priority" "textbox"
	And I click on "connAttributes" "info" "submit" "button"
	Then the changes should be updated in "Priority"
	When I click on "connectionProducts" "editRules" "link" of a combination having value
	And I provide integer value for "connAttributes" "Priority" "textbox"
	And I click on "connAttributes" "info" "submit" "button"
	Then the changes should be updated in "Priority"

@testcase=9894 @output=QueryAll(GetNodeConnection) @version=4 @prodready
Scenario: Entry of invalid value for Priority Connection of Product
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
	Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
	When I click on "connAttributes" "Edit" "link"
	Then I should see 1 product associations relating to that "node-connection-products"
	When I click on "connectionProducts" "editRules" "link" of a combination not having value
	Then I should see "Edit Property Information" interface
	#When I select "S - Por Entradas" from "New Property Function"
	When I provide invalid value for "connAttributes" "Priority" "textbox"
	And I click on "connAttributes" "info" "submit" "button"
	Then I should see the error message "El valor debe ser superior o igual a 1"

@testcase=9895 @output=QueryAll(GetNodeConnection) @version=4 @prodready
Scenario: Edit of invalid value for Priority Connection of Product
	Given I have "node-connection" in the system
	When I navigate to "ConfigureAttributesConnections" page
	When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
	Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
	When I click on "connAttributes" "Edit" "link"
	Then I should see 1 product associations relating to that "node-connection-products"
	When I click on "connectionProducts" "editRules" "link" of a combination not having value
	Then I should see "Edit Property Information" interface
	#When I select "S - Por Entradas" from "New Property Function"
	When I provide integer value for "connAttributes" "Priority" "textbox"
	And I click on "connAttributes" "info" "submit" "button"
	When I click on "connectionProducts" "editRules" "link" of a combination having value
	When I provide invalid value for "connAttributes" "Priority" "textbox"
	And I click on "connAttributes" "info" "submit" "button"
	Then I should see the error message "El valor debe ser superior o igual a 1"