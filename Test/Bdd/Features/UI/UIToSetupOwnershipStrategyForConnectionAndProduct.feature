@sharedsteps=16581 @owner=jagudelos @ui @testplan=35673 @testsuite=35690
Feature: UIToSetupOwnershipStrategyForConnectionAndProduct
As an Administrator user, I need an UI to set up Ownership Strategy by Connection/Product to send in the FICO ownership calculation process

Background: Login
Given I am logged in as "admin"
@testcase=37479
Scenario: Verify that in the connection configuration page we can see the ownership strategy column and the edit button
Given I have a valid connection in the system
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
When I click on "connAttributes" "Edit" "link"
Then I should see 1 product associations relating to that "node-connection-products"
And I should see the column for editing ownership strategy "Estragia de propriedad"
And verify that the icon for "Modelo analítico" is updated
And verify that the tiles are properly arranged as per the new design
And I should see the button to edit the ownership strategy
And verify the column name Estragia de propriedad is displayed properly
When I click on "connectionProducts" "editRules" "link" of a combination not having value
Then I should see "Edit Property Information" interface
@testcase=37480
Scenario: Verify that the Edit Connection Strategy is displayed on clicking the edit button
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
When I click on "connAttributes" "Edit" "link"
Then I should see 1 product associations relating to that "node-connection-products"
When I click on edit ownership strategy button
Then I should see "Edit ownership strategy" interface
@testcase=37481
Scenario: Verify the user interface for Edit Connection Strategy window
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
When I click on "connAttributes" "Edit" "link"
Then I should see 1 product associations relating to that "node-connection-products"
When I click on edit ownership strategy button
Then I should see "Edit ownership strategy" interface
And verify that we can see the title "Editar estrategia de propiedad"
And verify that we can see the current strategy tag "Estrategia de propiedad actual"
And verify that we can see the new strategy tag "Nueva estrategia de propiedad"
@testcase=37482
Scenario: Verify the current strategy tag is empty for a product without any strategy
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
When I click on "connAttributes" "Edit" "link"
Then I should see 1 product associations relating to that "node-connection-products"
When I click on edit ownership strategy button
Then I should see "Edit ownership strategy" interface
And verify that we can current strategy tag is empty for a product without any strategy
@testcase=37483
Scenario: Verify the new strategy selection is mandatory
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
When I click on "connAttributes" "Edit" "link"
Then I should see 1 product associations relating to that "node-connection-products"
When I click on edit ownership strategy button
Then I should see "Edit ownership strategy" interface
And verify that we can see all the possible rules defined in Rules Master section under the New strategy dropdown
When the user clicks on save button
Then verify that a message is shown indicating that the new strategy field is mandatory
@testcase=37484
Scenario: Verify the when user clicks on Save the new strategy is successfully displayed in the UI
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
When I click on "connAttributes" "Edit" "link"
Then I should see 1 product associations relating to that "node-connection-products"
When I click on edit ownership strategy button
Then I should see "Edit ownership strategy" interface
And verify that we can see all the possible rules defined in Rules Master section under the New strategy dropdown
And the user selects a new strategy from the list of available strategies
When the user clicks on save button
Then verify that the new strategy is displayed in the UI
When I click on edit ownership strategy button
Then I should see the current strategy updated
@testcase=37485
Scenario: Verify the when user clicks on Cancel the new strategy is not displayed in the UI
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
When I click on "connAttributes" "Edit" "link"
Then I should see 1 product associations relating to that "node-connection-products"
When I click on edit ownership strategy button
Then I should see "Edit ownership strategy" interface
And verify that we can see all the possible rules defined in Rules Master section under the New strategy dropdown
And the user selects a new strategy from the list of available strategies
When the user clicks on Cancel button
Then verify that the new strategy is not displayed in the UI
@testcase=37486
Scenario: Verify the current ownership strategy is not part of the options displayed under new ownership strategy
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
When I click on "connAttributes" "Edit" "link"
Then I should see 1 product associations relating to that "node-connection-products"
When I click on edit ownership strategy button
Then I should see "Edit ownership strategy" interface
And verify that we can see all the possible rules defined in Rules Master section under the New strategy dropdown
And verify that the current ownership strategy is not part of the new strategy rules displayed
@testcase=37487
Scenario: Verify the new strategy is not displayed in the UI when there is an error during the processing
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
When I click on "connAttributes" "Edit" "link"
Then I should see 1 product associations relating to that "node-connection-products"
When I click on edit ownership strategy button
Then I should see "Edit ownership strategy" interface
And verify that we can see all the possible rules defined in Rules Master section under the New strategy dropdown
And the user selects a new strategy from the list of available strategies
When the user clicks on save button
Then verify that the new strategy is not displayed in the UI if there is an error during the processing
@testcase=37488 
Scenario: Verify that in the connection configuration page we can sort the ownership strategy column
Given I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "node-connection" in the grid
When I click on "connAttributes" "Edit" "link"
Then I should see product associations relating to that "node-connection-products"
When the user tries to perform sorting for the ownership strategy column
Then the user can verify that sorting is working properly on the column


