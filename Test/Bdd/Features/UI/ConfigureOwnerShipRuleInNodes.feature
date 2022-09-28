@sharedsteps=4013 @owner=jagudelos @ui @testsuite=8492 @testplan=8481 @ThursRun
Feature: ConfigureOwnershipRuleInNodes
In order to calculate the Ownership of Hydrocarbons
As a True Administrator
I need UI to Configure Ownership rule in Nodes per Product

Background: Login
	Given I am logged in as "admin"

@testcase=9889 @ui @output=QueryAll(GetNodes) @ignore
Scenario: Verify Entry and Edit the rules on the Product
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	Then I should see a "NodeName" belongs to "node" in the grid
	When I click on "nodeAttributes" "edit" "link"
	Then I should see 1 product associations relating to that "node-products"
	And I should see empty value in the grid
		| Rule               |
		| IdentifiedLosses   |
		| UnIdentifiedLosses |
		| FinalInventory     |
		| OutputTolerance    |
	When I click on "nodeProducts" "editRules" "link"
	Then I should see "Edit Property Functions" interface
	When I select Rule from Respective dropdowns
		| Rule    | Dropdown                |
		| il_Rule | Identified LossesRule   |
		| ul_Rule | UnIdentified LossesRule |
		| to_Rule | Outputs Tolerance Rule  |
		| if_Rule | Final Inventory Rule    |
	And I click on "nodeAttributes" "functions" "submit" "button"
	Then I should see selected values in the grid
		| Selected           | Expected                |
		| IdentifiedLosses   | Identified LossesRule   |
		| UnIdentifiedLosses | UnIdentified LossesRule |
		| FinalInventory     | Final Inventory Rule    |
		| OutputTolerance    | Outputs Tolerance Rule  |
	When I click on "nodeProducts" "editRules" "link"
	Then I should see "Edit Property Functions" interface
	When I select Rule from Respective dropdowns
		| Rule     | Dropdown                |
		| il_Rule1 | Identified LossesRule   |
		| ul_Rule1 | UnIdentified LossesRule |
		| to_Rule1 | Outputs Tolerance Rule  |
		| if_Rule1 | Final Inventory Rule    |
	And I click on "nodeAttributes" "functions" "submit" "button"
	Then I should see selected values in the grid
		| Selected           | Expected                |
		| IdentifiedLosses   | Identified LossesRule   |
		| UnIdentifiedLosses | UnIdentified LossesRule |
		| FinalInventory     | Final Inventory Rule    |
		| OutputTolerance    | Outputs Tolerance Rule  |

@testcase=9890 @ui @output=QueryAll(GetNodes) @ignore
Scenario: Validate warning messages are shown when admin tried to save without selecting rules
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	When I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	Then I should see a "NodeName" belongs to "node" in the grid
	When I click on "nodeAttributes" "Edit" "link"
	Then I should see 1 product associations relating to that "node-products"
	When I click on "nodeProducts" "editRules" "link"
	Then I should see "Edit Property Functions" interface
	When I click on "nodeAttributes" "functions" "submit" "button"
	Then I should see the message on interface "Requerido"