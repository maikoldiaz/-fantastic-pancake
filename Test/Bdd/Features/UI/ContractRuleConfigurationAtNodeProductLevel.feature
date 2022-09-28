@sharedsteps=16659 @owner=jagudelos @ui @testsuite=39232 @testplan=39221 @parallel=false
Feature: ContractRuleConfigurationAtNodeProductLevel
As a TRUE system administrator,
I require a contract rule to be configured at Node-Product level
to complement the ownership calculation criteria

Background: Login
	Given I am logged in as "admin"

@testcase=41380 @output=QueryAll(GetNodes) @version=2 @bvt1.5
Scenario: Validate changes in node configuration list without configuration
	And I have "Node" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I enter node name into "nodeAttributes" "Name" "textbox"
	And I click on "nodeAttributes" "edit" "link"
	And validate the "Contractual configuration" column is displayed in "node-products" grid
	And I have node product without configuration strategy
	Then I should not see any value in "Contractual configuration" column

@testcase=41381 @output=QueryAll(GetNodes) @version=2 @bvt1.5
Scenario: Validate changes in node configuration list with multiple configuration
	And I have "Node" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I enter node name into "nodeAttributes" "Name" "textbox"
	And I click on "nodeAttributes" "edit" "link"
	And validate the "Contractual configuration" column is displayed in "node-products" grid
	And I click on "nodeProducts" "editVariables" "link"
	And I select mutiple variables "PI" and "INTERFASE" from "nodeproductVariables" "dropdown"
	And I select ownershipstrategy from "nodeAttributes" "to" "dropdown" 
	And I click on "nodeAttributes" "functions" "submit" "button"
	Then I should see "PI" and "INTERFASE" as values in "Contractual configuration" column separated by semicolon 

@testcase=41382 @version=2
Scenario: Validate shared editing buttons
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "nodeAttributes" "edit" "link"
	And validate the "Contractual configuration" column is displayed in "node-products" grid
	And I click on "nodeProducts" "editVariables" "link"
	Then I should see "nodeOwnershipRules" "functions" "interface"
	And I click on "nodeOwnershipRules" "functions" "cancel" "button" 
	And I click on "nodeProducts" "editRules" "link" 
	And I should see "nodeOwnershipRules" "functions" "interface"

@testcase=41383 @bvt @output=QueryAll(GetNodes) @version=2 @bvt1.5
Scenario: Validate successful update of selected variable
	And I have "Node" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I enter node name into "nodeAttributes" "Name" "textbox"
	And I click on "nodeAttributes" "edit" "link"
	And validate the "Contractual configuration" column is displayed in "node-products" grid
	And I click on "nodeProducts" "editVariables" "link"
	Then I should see "nodeOwnershipRules" "functions" "interface"
	And I should see "nodeAttributes" "functions" "submit" "button"
	And I should see "nodeOwnershipRules" "functions" "cancel" "button" 
	And I select mutiple variables "PI" and "PNI" from "nodeproductVariables" "dropdown"
	And I select ownershipstrategy from "nodeAttributes" "to" "dropdown"
	And I click on "nodeAttributes" "functions" "submit" "button"
	And I should see "PI" and "PNI" as values in "Contractual configuration" column separated by semicolon 
	And I should see product is updated with the selected variable

@testcase=41384 @version=2
Scenario: Validate cancel button of select contractual configuration
	When I navigate to "ConfigureAttributesNodes" page
	And I click on "nodeAttributes" "edit" "link"
	And validate the "Contractual configuration" column is displayed in "node-products" grid
	And I click on "nodeProducts" "editVariables" "link"
	Then I should see "nodeOwnershipRules" "functions" "interface"
	And I should see "nodeAttributes" "functions" "submit" "button"
	And I should see "nodeOwnershipRules" "functions" "cancel" "button"
	And I click on "nodeOwnershipRules" "functions" "cancel" "button"
	And I should see "nodeProducts" "grid"

@testcase=41385 @output=QueryAll(GetNodes) @version=2 @bvt1.5
Scenario: Validate List of contractual variables
	And I have "Node" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I enter node name into "nodeAttributes" "Name" "textbox"
	And I click on "nodeAttributes" "edit" "link"	
	And I click on "nodeProducts" "editVariables" "link"
	Then I should see "nodeOwnershipRules" "functions" "interface"
	And I see below "Variables" list from "Variable" dropdown in "nodeproductVariables" "dropdown"
	| Variables  |
	| PI         |
	| PNI        |
	| TOLERANCIA |
	| INTERFASE  |
	| INVENTARIO |

@testcase=41386 @bvt @output=QueryAll(GetNodes) @version=2 @bvt1.5
Scenario: Validate existing values in edit view
	And I have "Node" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I enter node name into "nodeAttributes" "Name" "textbox"
	And I click on "nodeAttributes" "edit" "link"
	And I click on "nodeProducts" "editVariables" "link"
	And I select mutiple variables "PI" and "PNI" from "nodeproductVariables" "dropdown"
	And I select ownershipstrategy from "nodeAttributes" "to" "dropdown" 
	And I click on "nodeAttributes" "functions" "submit" "button"
	And I click on "nodeProducts" "editVariables" "link"
	Then I should see existing values "PI" "PNI" in "nodeproductVariables" "dropdown"

@testcase=41387 @bvt @output=QueryAll(GetNodes) @version=2 @bvt1.5
Scenario: Validate invalid combination for new ownership strategy
	And I have "Node" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I enter node name into "nodeAttributes" "Name" "textbox"
	And I click on "nodeAttributes" "edit" "link"
	And I click on "nodeProducts" "editVariables" "link"
	Then I should see "nodeOwnershipRules" "functions" "interface"
	And I select ownership strategy as "Inventario Final" from "nodeAttributes" "to" "dropdown" 
	And I should not see value of "Variable" as "INVENTARIO" in "nodeproductVariables" "dropdown"

@testcase=41388 @output=QueryAll(GetNodes) @version=2
Scenario: Validate invalid combination for existing ownership strategy
	And I have "Node" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I enter node name into "nodeAttributes" "Name" "textbox"
	And I click on "nodeAttributes" "edit" "link"
	And I click on "nodeProducts" "editRules" "link"
	And I select ownership strategy as "Inventario Final" from "nodeAttributes" "to" "dropdown"
	And I click on "nodeAttributes" "functions" "submit" "button"
	And I click on "nodeProducts" "editVariables" "link"
	Then I should see "nodeOwnershipRules" "functions" "interface"
	And I should not see value of "Variable" as "INVENTARIO" in "nodeproductVariables" "dropdown"