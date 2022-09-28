@sharedsteps=4013 @owner=jagudelos @ui @testsuite=70801 @testplan=70526 @S16
Feature: CheckPriorityForNodeConnection
I need to be allowed to configure connections with priorities from 1 to 1,000,000
to ensure that each product on a connection has a different priority

Background: Login
Given I am logged in as "admin"

@testcase=72837 @output=QueryAll(GetNodeConnection) @BVT2
Scenario: Check Title of the Modal Window
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
And I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
And I should see a "SourceNodeName" belongs to "node-connection" in the grid
And I click on "connAttributes" "Edit" "link"
And I should see 1 product associations relating to that "node-connection-products"
And I click on edit priority link
Then I should see "Editar la prioridad del producto" as the title of modal winow

@testcase=72838 @output=QueryAll(GetNodeConnection) @BVT2
Scenario: Check Default Priority is 1
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
And I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
And I should see a "SourceNodeName" belongs to "node-connection" in the grid
And I click on "connAttributes" "Edit" "link"
And I should see 1 product associations relating to that "node-connection-products"
Then I should see the value of "Priority" as "1"

@testcase=72839 @output=QueryAll(GetNodeConnection) @BVT2
Scenario: Entry of valid value for Priority Connection of Product
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
And I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
And I should see a "SourceNodeName" belongs to "node-connection" in the grid
And I click on "connAttributes" "Edit" "link"
And I should see 1 product associations relating to that "node-connection-products"
And I click on edit priority link
And I should see "Edit product priority" interface
And I provide an integer value in range "1" to "1000000" for "decimal" "Priority" "textbox"
And I click on "connAttributes" "info" "submit" "button"
Then the changes should be updated in "Priority"

@testcase=72840 @output=QueryAll(GetNodeConnection) @BVT2
Scenario: Entry of invalid value for Priority Connection of Product
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
And I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
And I should see a "SourceNodeName" belongs to "node-connection" in the grid
And I click on "connAttributes" "Edit" "link"
And I should see 1 product associations relating to that "node-connection-products"
And I click on edit priority link
And I should see "Edit product priority" interface
When I provide text value different to a positive integer for "decimal" "Priority" "textbox"
Then I must see "blank" value in "decimal" "Priority" "textbox"
And I click on "connAttributes" "info" "submit" "button"
And I should see "Requerido" message in modal window

@testcase=72841 @output=QueryAll(GetNodeConnection) @BVT2
Scenario: Entry of value greater than 1,000,000 for Priority Connection of Product
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
And I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
And I should see a "SourceNodeName" belongs to "node-connection" in the grid
And I click on "connAttributes" "Edit" "link"
And I should see 1 product associations relating to that "node-connection-products"
And I click on edit priority link
And I should see "Edit product priority" interface
And I enter value greater than "1000000" for "decimal" "Priority" "textbox"
Then I should not see value greater than "1000000" in "decimal" "Priority" "textbox"
