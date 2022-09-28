@sharedsteps=4013 @owner=jagudelos @ui @testsuite=39234 @testplan=39221 @parallel=false
Feature: SelfNodeConnection
As TRUE System Administrator, I need to create a connection between the same node from the graphical network configuration to complete all types of connections

Background: Login
Given I am logged in as "admin"

@testcase=41807 @bvt @bvt1.5
Scenario: Verify that clicking on accept we can create a connection between the same node
Given I have data configured for "segment" network configuration
When I navigate to "Graphic Network Configuration" page
And I select any "Segment" from "NodeFilter" "Category" "combobox"
And I choose CategoryElement from "NodeFilter" "Element" "combobox"
And I enter "Todos" into Node Texbox
And I click on "NodeFilter" "ViewReport" "button"
And I select the output of an active node and drag and drop it into the input of the same active node
And I see confirmation dialog with accept and cancel options
And I click on "Confirm" "Accept" "button"
Then I should see a node creation created for the same node is "present"
@testcase=41808
Scenario: Verify that clicking on cancel we do not create a connection between the same node
Given I have data configured for "segment" network configuration
When I navigate to "Graphic Network Configuration" page
And I select any "Segment" from "NodeFilter" "Category" "combobox"
And I choose CategoryElement from "NodeFilter" "Element" "combobox"
And I enter "Todos" into Node Texbox
And I click on "NodeFilter" "ViewReport" "button"
And I select the output of an active node and drag and drop it into the input of the same active node
And I see confirmation dialog with accept and cancel options
And I click on "Confirm" "Cancel" "button"
Then I should see a node creation created for the same node is "absent"

@testcase=41809 @manual
Scenario: Verify that clicking on accept if any error occurs node connection is not created
Given I have data configured for "segment" network configuration
When I navigate to "Graphic Network Configuration" page
And I select any "Segment" from "NodeFilter" "Category" "combobox"
And I choose CategoryElement from "NodeFilter" "Element" "combobox"
And I enter "Todos" into Node Texbox
And I click on "NodeFilter" "ViewReport" "button"
And I select the output of an active node and drag and drop it into the input of the same active node
And I see confirmation dialog with accept and cancel options
And I disable the internet connection by pression on F12, goto Network and make connection offline
And I click on "Accept" "button"
Then I should see a node creation created for the same node is "absent"
@testcase=41810
Scenario: Verify that we cannot create a duplicate self node connection
Given I have data configured for "segment" network configuration
When I navigate to "Graphic Network Configuration" page
And I select any "Segment" from "NodeFilter" "Category" "combobox"
And I choose CategoryElement from "NodeFilter" "Element" "combobox"
And I enter "Todos" into Node Texbox
And I click on "NodeFilter" "ViewReport" "button"
And I select the output of an active node and drag and drop it into the input of the same active node
And I see confirmation dialog with accept and cancel options
And I click on "Confirm" "Accept" "button"
Then I should see a node creation created for the same node is "present"
When I select the output of an active node and drag and drop it into the input of the same active node
And I see this message "Ya existe una conexión entre los nodos" on "confirm" "message" "container"
@testcase=41811  @bvt1.5
Scenario: Verify that we cannot create a self connection for an inactive node
Given I have data configured for "segment" network configuration
When I navigate to "Graphic Network Configuration" page
And I select any "Segment" from "NodeFilter" "Category" "combobox"
And I choose CategoryElement from "NodeFilter" "Element" "combobox"
And I enter "Todos" into Node Texbox
And I click on "NodeFilter" "ViewReport" "button"
And I make the node inactive
And I select the output of an active node and drag and drop it into the input of the same active node
And I see confirmation dialog with accept and cancel options
And I click on "Confirm" "Accept" "button"
And I see this message "No se pueden crear conexiones desde o hacia nodos inactivos" on "confirm" "message" "container"
