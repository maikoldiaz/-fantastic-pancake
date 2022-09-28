@sharedsteps=4013 @owner=jagudelos @ui  @testsuite=35683 @testplan=35673
Feature: DiscoverAndHideConnectionsUI
As TRUE System Administrator, I need to discover and hide connections between nodes from a node to progressively visualize the network
or hide parts of the network

Background: Login
Given I am logged in as "admin"

@testcase=37349 @bvt
Scenario: Verify the input connections for the node are displayed/hidden when the user clicks on Input connections button of a specific node under Segment Category
Given I have data configured for "segment" network
When I navigate to "GraphicConfigurationChain" page
And I click on Segment dropdown from "NodeFilterCategory" dropdown
And I choose CategoryElement from "NodeFilter" "Element" "combobox"
And I send the name of the node to Node textbox
And I click on "NodeFilter" "ViewReport" "button"
And the user clicks on input connections button for the node
Then verify that all connections, nodes and branches which are part of the input connection to the selected node are hidden
And verify that all the input and output nodes for the displayed node are shown correctly
When the user clicks on input connections button for the node
Then verify that all connections, nodes and branches which are part of the input connection to the selected node are displayed
And verify that all the input and output nodes for the displayed node are shown correctly

@testcase=37350 @bvt
Scenario: Verify the output connections for the node are displayed/hidden when the user clicks on Output connections button of a node under Segment Category
Given I have data configured for "segment" network
When I navigate to "GraphicConfigurationChain" page
And I click on Segment dropdown from "NodeFilterCategory" dropdown
And I choose CategoryElement from "NodeFilter" "Element" "combobox"
And I send the name of the node to Node textbox
And I click on "NodeFilter" "ViewReport" "button"
And the user clicks on output connections button for the node
Then verify that all connections, nodes and branches which are part of the input connection to the selected node are hidden
And verify that all the input and output nodes for the displayed node are shown correctly
When the user clicks on output connections button for the node
Then verify that all connections, nodes and branches which are part of the input connection to the selected node are displayed
And verify that all the input and output nodes for the displayed node are shown correctly
@testcase=37351
Scenario: Verify the input connections for the node are displayed/hidden when the user clicks on Input connections button of all nodes under System Category
Given I have data configured for "system" network
When I navigate to "GraphicConfigurationChain" page
And I click on System dropdown from "NodeFilterCategory" dropdown
And I choose CategoryElement from "NodeFilter" "Element" "combobox"
And I send the text "Todos" to Node textbox
And I click on "NodeFilter" "ViewReport" "button"
And the user clicks on input connections button for the node
Then verify that all connections, nodes and branches which are part of the input connection to the selected node are hidden
When the user clicks on input connections button for the node
Then verify that all connections, nodes and branches which are part of the input connection to the selected node are displayed
@testcase=37352
Scenario: Verify the output connections for the node are displayed/hidden when the user clicks on Output connections button of all nodes under System Category
Given I have data configured for "system" network
When I navigate to "GraphicConfigurationChain" page
And I click on System dropdown from "NodeFilterCategory" dropdown
And I choose CategoryElement from "NodeFilter" "Element" "combobox"
And I send the text "Todos" to Node textbox
And I click on "NodeFilter" "ViewReport" "button"
And the user clicks on output connections button for the node
Then verify that all connections, nodes and branches which are part of the input connection to the selected node are hidden
When the user clicks on output connections button for the node
Then verify that all connections, nodes and branches which are part of the input connection to the selected node are displayed
@testcase=37353
Scenario: Verify the displayed connection count for input nodes is updated when the user clicks on input connections button for a node
Given I have data configured for "segment" network
When I navigate to "GraphicConfigurationChain" page
And I click on Segment dropdown from "NodeFilterCategory" dropdown
And I choose CategoryElement from "NodeFilter" "Element" "combobox"
And I send the text "Todos" to Node textbox
And I click on "NodeFilter" "ViewReport" "button"
And the user creates a new connection in the backend
And the user clicks on input connections button for the node
Then the displayed connection count should be updated for the input nodes and the relevant branches displayed
And the user deletes a connection in the backend
When the user clicks on input connections button for the node
Then the displayed connection count should be updated for the input nodes and the relevant branches displayed
@testcase=37354
Scenario: Verify the displayed connection count for output nodes is updated when the user clicks on output connections button for a node
Given I have data configured for "segment" network
When I navigate to "GraphicConfigurationChain" page
And I click on Segment dropdown from "NodeFilterCategory" dropdown
And I choose CategoryElement from "NodeFilter" "Element" "combobox"
And I send the text "Todos" to Node textbox
And I click on "NodeFilter" "ViewReport" "button"
And the user creates a new connection in the backend
And the user clicks on output connections button for the node
Then the displayed connection count should be updated for the output nodes and the relevant branches displayed
And the user deletes a connection in the backend
When the user clicks on output connections button for the node
Then the displayed connection count should be updated for the output nodes and the relevant branches displayed
@testcase=37355
Scenario: Verify that a proper error message is displayed when the user is unsuccessful in fetching input connnections for a node
Given I have data configured for "segment" network
When I navigate to "GraphicConfigurationChain" page
And I click on Segment dropdown from "NodeFilterCategory" dropdown
And I choose CategoryElement from "NodeFilter" "Element" "combobox"
And I send the text "Todos" to Node textbox
And I click on "NodeFilter" "ViewReport" "button"
And there is an error issue with the connection to the database
And the user clicks on input connections button for the node
Then verify that a proper error message is displayed asking the user to try later “Se presentó un error inesperado y no fue posible obtener la información.Por favor, intente de nuevo más tarde.”
@testcase=37356
Scenario: Verify that a proper error message is displayed when we are unsuccessful in fetching output connnections for a node
Given I have data configured for "segment" network
When I navigate to "GraphicConfigurationChain" page
And I click on Segment dropdown from "NodeFilterCategory" dropdown
And I choose CategoryElement from "NodeFilter" "Element" "combobox"
And I send the text "Todos" to Node textbox
And I click on "NodeFilter" "ViewReport" "button"
And there is an error issue with the connection to the database
When the user clicks on output connections button for the node
Then verify that a proper error message is displayed asking the user to try later “Se presentó un error inesperado y no fue posible obtener la información.Por favor, intente de nuevo más tarde.”
@testcase=37357 
Scenario: Verify that node connections are displayed properly for nodes across segments
Given I have data configured for "segment" network
When I navigate to "GraphicConfigurationChain" page
And I click on Segment dropdown from "NodeFilterCategory" dropdown
And I choose CategoryElement from "NodeFilter" "Element" "combobox"
And I send the name of the node to Node textbox
And I click on "NodeFilter" "ViewReport" "button"
And the user clicks on input connections button for the node
Then verify that all the nodes for the input connection are shown correctly irrespective of the segment
And the user clicks on output connections button for the node
Then verify that all the nodes for the output connection are shown correctly irrespective of the segment
