@sharedsteps=4013 @owner=jagudelos @ui @testplan=14709 @testsuite=14719
Feature: ManageOwnershipRulesThroughCategory
In order to Manage Ownership Rules
As a TRUE System Administrator
I need to manage ownership rules through a category
to configure them at the node-product level

Background: Login
	Given I am logged in as "admin"

@testcase=16543 @ui @output=QueryAll(GetNodes)
Scenario: Verify Ownership Rules category should be shown in the dropdown while creation of Category Element
	Given I have "Nodes" in the system
	When I navigate to "Category Elements" page
	And I click on "Create Element" "button"
	Then I should see "Create Element" interface
	When I click on "Category" "combobox"
	Then I should be able to select "Ownership Rules" from Category combobox

@testcase=16544 @bvt @ui @output=QueryAll(GetNodes)
Scenario: Verify edit ownership strategy should display the list of rules of the Ownership Rules Category
	Given I have "Nodes" in the system
	When I navigate to "Category Elements" page
	And I click on "Create Element" "button"
	Then I should see "Create Element" interface
	When I select "Ownership Rules" from Category combobox
	And I provide value for "element" "name" "textbox"
	And I provide value for "element" "description" "textarea"
	And I click on "element" "submit" "button"
	Then the "category element" should be saved and showed in the list
	When I navigate to "ConfigureAttributesNodes" page
	And I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	Then I should see a "NodeName" belongs to "nodeAttributes" in the grid
	When I click on "nodeAttributes" "edit" "link"
	Then I should see 1 product associations relating to that "node-products"
	And I should see empty value for ownership strategy in the grid
	When I click on "nodeProducts" "editRules" "link"
	Then I should see "Edit Property Strategy" interface
	When I select created OwnershipStrategy element from "nodeAttributes" "to" "combobox"
	And I click on "nodeAttributes" "functions" "submit" "button"
	Then I should see selected OwnershipStrategy element in the Grid

@testcase=16545 @bvt @ui @output=QueryAll(GetNodes)
Scenario: Verify Edit ownership strategy rule on the product
	Given I have "Nodes" in the system
	When I navigate to "Category Elements" page
	And I click on "Create Element" "button"
	Then I should see "Create Element" interface
	When I select "Ownership Rules" from Category combobox
	And I provide value for "element" "name" "textbox"
	And I provide value for "element" "description" "textarea"
	And I click on "element" "submit" "button"
	Then the "category element" should be saved and showed in the list
	When I navigate to "ConfigureAttributesNodes" page
	And I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	Then I should see a "NodeName" belongs to "nodeAttributes" in the grid
	When I click on "nodeAttributes" "edit" "link"
	Then I should see 1 product associations relating to that "node-products"
	And I should see empty value for ownership strategy in the grid
	When I click on "nodeProducts" "editRules" "link"
	Then I should see "Edit Property Strategy" interface
	When I select created OwnershipStrategy element from "nodeAttributes" "to" "combobox"
	And I click on "nodeAttributes" "functions" "submit" "button"
	Then I should see selected OwnershipStrategy element in the Grid
	When I click on "nodeProducts" "editRules" "link"
	Then I should see "Edit Property Strategy" interface
	And I should see selected OwnershipStrategy element under actual OwnershipStrategy label
	When I select OwnershipStrategy element from "nodeAttributes" "to" "combobox"
	And I click on "nodeAttributes" "functions" "submit" "button"
	Then I should see selected OwnershipStrategy element in the Grid

@testcase=16546 @ui @output=QueryAll(GetNodes)
Scenario: Verify title for the only rule column in the grid	and Edit Interface
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	Then I should see a "NodeName" belongs to "nodeAttributes" in the grid
	When I click on "nodeAttributes" "edit" "link"
	Then I should see 1 product associations relating to that "node-products"
	And I should see title for the only rule column as "Ownership strategy"
	When I click on "nodeProducts" "editRules" "link"
	Then I should see "Edit Property Strategy" interface

@testcase=16547 @ui @output=QueryAll(GetNodes)
Scenario: Validate warning message is shown when admin tried to save without selecting strategy
	Given I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	When I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	Then I should see a "NodeName" belongs to "nodeAttributes" in the grid
	When I click on "nodeAttributes" "Edit" "link"
	Then I should see 1 product associations relating to that "node-products"
	When I click on "nodeProducts" "editRules" "link"
	Then I should see "Edit Property Strategy" interface
	When I click on "nodeAttributes" "functions" "submit" "button"
	Then I should see the message on interface "Requerido"