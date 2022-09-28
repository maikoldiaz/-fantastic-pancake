@sharedsteps=4013 @owner=jagudelos @ui @testsuite=19791 @testplan=19772
Feature: ConfigureOwnershipRulesInConnectionsAsPerAnalyticalModel
As a True Administrator
I need UI to Configure the Ownership Rules according to the results from the Analytical Model

@testcase=21179 @ui @output=QueryAll(GetNodeConnection) @version=2
Scenario: List of models according to master data
Given I am logged in as "admin"
And I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "connAttributes" in the grid
When I click on "connAttributes" "Edit" "link"
When I click on "connAttributes" "controlLimit" "edit" "link" of a combination not having value
Then I should see "connAttributes" "controlLimit" "interface"
When I click on "connAttributes" "isTransfer" "checkbox" on the UI
Then verify the list of models listed in "OwnershipStrategy" dropdown are as per master data

@testcase=21180 @bvt @ui @output=QueryAll(GetNodeConnection) @version=2
Scenario: Entry of an Analytical model
Given I am logged in as "admin"
And I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "connAttributes" in the grid
When I click on "connAttributes" "Edit" "link"
When I click on "connAttributes" "controlLimit" "edit" "link" of a combination not having value
Then I should see "connAttributes" "controlLimit" "interface"
When I click on "connAttributes" "isTransfer" "checkbox" on the UI
And I click on "connAttributes" "controlLimit" "submit" "button"
Then I should see the message "Requerido"
When I select "PROPHET" from "OwnershipStrategy"
And I click on "connAttributes" "controlLimit" "submit" "button"
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "connAttributes" in the grid
And the changes should be updated in "OwnershipStrategy" column

@testcase=21181 @ui @output=QueryAll(GetNodeConnection) @version=2
Scenario: Edit of an Analytical model
Given I am logged in as "admin"
And I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "connAttributes" in the grid
When I click on "connAttributes" "Edit" "link"
When I click on "connAttributes" "controlLimit" "edit" "link" of a combination not having value
Then I should see "connAttributes" "controlLimit" "interface"
When I click on "connAttributes" "isTransfer" "checkbox" on the UI
When I select "PROPHET" from "OwnershipStrategy"
And I click on "connAttributes" "controlLimit" "submit" "button"
When I click on "connAttributes" "controlLimit" "edit" "link" of a combination having value
Then I should see "connAttributes" "controlLimit" "interface"
When I click on "connAttributes" "isTransfer" "checkbox" on the UI
Then validate that "connAttributes" "algorithmId" "dropdown" is "disabled"
When I click on "connAttributes" "isTransfer" "checkbox" on the UI
When I select "XR_BOOST" from "OwnershipStrategy"
And I click on "connAttributes" "controlLimit" "submit" "button"
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "connAttributes" in the grid
And the changes should be updated in "OwnershipStrategy" column

@testcase=21182 @bvt @ui @output=QueryAll(GetNodeConnection) @version=2
Scenario: Unchecking a Connection as a transfer point
Given I am logged in as "admin"
And I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "connAttributes" in the grid
When I click on "connAttributes" "Edit" "link"
When I click on "connAttributes" "controlLimit" "edit" "link" of a combination not having value
Then I should see "connAttributes" "controlLimit" "interface"
When I click on "connAttributes" "isTransfer" "checkbox" on the UI
When I select "PROPHET" from "OwnershipStrategy"
And I click on "connAttributes" "controlLimit" "submit" "button"
When I click on "connAttributes" "controlLimit" "edit" "link" of a combination having value
Then I should see "connAttributes" "controlLimit" "interface"
When I click on "connAttributes" "isTransfer" "checkbox" on the UI
And I click on "connAttributes" "controlLimit" "submit" "button"
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "connAttributes" in the grid
Then saved "OwnershipStrategy" value should be disassociated from the connection
And verify that "connAttributes" "checkbox" "checkbox" should be "unchecked"

@testcase=21183 @ui @output=QueryAll(GetNodeConnection) @version=2
Scenario: Remove deprecated ownership function
Given I am logged in as "admin"
And I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
When I enter valid "SourceNodeName" into "connAttributes" "SourceNode" "Name" "textbox"
Then I should see a "SourceNodeName" belongs to "connAttributes" in the grid
When I click on "connAttributes" "Edit" "link"
Then I should not see current ownership function in the grid
And I should not be able to set or edit new ownership function in the grid

@testcase=25142 @api @output=QueryAll(GetNodeConnection)
Scenario: Create Node Connection with isTransfer as true and without algorithm Id
Given I am authenticated as "admin"
And I want to create a "node-connection" in the system
When I provide "isTransfer" value as "true" and without "algorithmId" for connection
Then the response should fail with message "Algoritmo no definido para el punto de transferencia"

@testcase=25143 @api @output=QueryAll(GetNodeConnection) @version=2
Scenario: Create Node Connection with isTransfer as false
Given I am authenticated as "admin"
And I want to create a "node-connection" in the system
When I provide "isTransfer" value as "false" and without "algorithmId" for connection
Then the response should succeed with message "Conexión creada con éxito"