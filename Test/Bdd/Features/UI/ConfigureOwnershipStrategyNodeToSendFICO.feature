@sharedsteps=4013 @owner=jagudelos @ui @testsuite=35688 @testplan=35673 @parallel=false
Feature: ConfigureOwnershipStrategyNodeToSendFICO
In order to Process ownership calculation for FICO
As an administrator user
I need an UI to configure ownership strategy by Node

Background: Login
	Given I am logged in as "admin"

@testcase=37320 @version=2
Scenario: Validate the node level strategy visualization
	And I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	And I should see a "NodeName" belongs to "nodeAttribute" in the grid
	And validate the "Ownership strategy" column is displayed in "nodeAttribute" grid
	And I click on "nodeAttributes" "Edit" "link"
	Then I should see "NodeAttributes" "NodeOwnershipRule" "Edit" "Link"

@testcase=37321 @version=2 @bvt @bvt1.5
Scenario: Add Strategy to the node level
	And I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	And I should see a "NodeName" belongs to "nodeAttribute" in the grid
	And validate the "Ownership strategy" column is displayed in "nodeAttribute" grid
	And I click on "nodeAttributes" "Edit" "link"
	And I click on "NodeAttributes" "NodeOwnershipRule" "Edit" "Link"
	And I should see "Modal" "NodeAttributeBulkUpdate" "container"
	And I should see "BulkUpdate" "Rules" "Label" is empty
	And I click on "NodeAttributes" "Functions" "Submit" "button"
	And I should see error message "Requerido"
	And I click on "NodeAttributes" "Functions" "Cancel" "button"
	And I should not see "Modal" "NodeAttributeBulkUpdate" "container"
	And I should see "NodeAttributes" "NodeOwnershipRule" "Label" is empty
	And I click on "NodeAttributes" "NodeOwnershipRule" "Edit" "Link"
	And I should see "Modal" "NodeAttributeBulkUpdate" "container"
	And I select "Strategy" from "New Ownership Strategy" dropdown
	And I click on "NodeAttributes" "Functions" "Submit" "button"
	Then the "Strategy" should be updated in "NodeAttributes" "NodeOwnershipRule" "Label"

@testcase=37322 @version=2 @bvt @bvt1.5
Scenario: Update Strategy to the node level
	And I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	And I should see a "NodeName" belongs to "nodeAttribute" in the grid
	And validate the "Ownership strategy" column is displayed in "nodeAttribute" grid
	And I click on "nodeAttributes" "Edit" "link"
	And I click on "NodeAttributes" "NodeOwnershipRule" "Edit" "Link"
	And I should see "Modal" "NodeAttributeBulkUpdate" "container"
	And I select "Strategy" from "New Ownership Strategy"
	And I click on "NodeAttributes" "Functions" "Submit" "button"
	And the "Strategy" should be updated in "NodeAttributes" "NodeOwnershipRule" "Label"
	And I click on "NodeAttributes" "NodeOwnershipRule" "Edit" "Link"
	And I should see "Modal" "NodeAttributeBulkUpdate" "container"
	And validate "Strategy" in "BulkUpdate" "Rules" "Label"
	And validate "Strategy" is not shown in "New Ownership Strategy" option
	And I select "New Strategy" from "New Ownership Strategy"
	And I click on "NodeAttributes" "Functions" "Submit" "button"
	Then the "New Strategy" should be updated in "NodeAttributes" "NodeOwnershipRule" "Label"

@testcase=37323 @version=2 @bvt @bvt1.5
Scenario: Add Strategy to the product level
	And I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	And I should see a "NodeName" belongs to "nodeAttribute" in the grid
	And validate the "Ownership strategy" column is displayed in "nodeAttribute" grid
	And I click on "nodeAttributes" "Edit" "link"
	And I click on "NodeProducts" "EditRules" "Link"
	And I should see "Modal" "NodeAttributeProductBulkUpdate" "container"
	And I should see "BulkUpdate" "Rules" "Label" is empty
	And I click on "NodeAttributes" "Functions" "Submit" "button"
	And I should see error message "Requerido"
	And I click on "NodeAttributes" "Functions" "Cancel" "button"
	And I should not see "Modal" "NodeAttributeProductBulkUpdate" "container"
	And validate "Node" "StrategyConfiguration" "Label" is empty
	And I click on "NodeProducts" "EditRules" "Link"
	And I should see "Modal" "NodeAttributeProductBulkUpdate" "container"
	And I select "Strategy" from "New" "Ownership" "Strategy"
	And I click on "NodeAttributes" "Functions" "Submit" "button"
	Then the "Strategy" should be updated in "NodeAttributes" "NodeOwnershipRule" "Label"

@testcase=373204 @version=2 @bvt @bvt1.5
Scenario: Update Strategy to the product level
	And I have "Nodes" in the system
	When I navigate to "ConfigureAttributesNodes" page
	And I enter valid "NodeName" into "nodeAttributes" "Name" "textbox"
	And I should see a "NodeName" belongs to "nodeAttribute" in the grid
	And validate the "Ownership strategy" column is displayed in "nodeAttribute" grid
	And I click on "nodeAttributes" "Edit" "link"
	And I click on "NodeProducts" "EditRules" "Link"
	And I should see "Modal" "NodeAttributeProductBulkUpdate" "container"
	And I select "Strategy" from "New Ownership Strategy"
	And I click on "NodeAttributes" "Functions" "Submit" "button"
	And the "Strategy" should be updated in "NodeAttributes" "NodeOwnershipRule" "Label"
	And I click on "NodeProducts" "EditRules" "Link"
	And I should see "Modal" "NodeAttributeProductBulkUpdate" "container"
	And validate "Strategy" in "BulkUpdate" "Rules" "Label"
	And validate "Strategy" is not shown in "New Ownership Strategy" option
	And I select "New Strategy" from "New Ownership Strategy"
	And I click on "NodeAttributes" "Functions" "Submit" "button"
	Then the "New Strategy" should be updated in "NodeAttributes" "NodeOwnershipRule" "Label"