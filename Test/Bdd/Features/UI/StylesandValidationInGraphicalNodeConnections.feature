@sharedsteps=4013 @owner=jagudelos @ui @testsuite=35687 @testplan=35673 @parallel=false
Feature: StylesandValidationInGraphicalNodeConnections
As an application administrator
I need to graphically add styles and validate inappropriate connections to configure the network graphically

Background: Login
Given I am logged in as "admin"

@testcase=37394 @bvt @bvt1.5
Scenario: Verify the application cannot allow to make connections graphically from active to inactive node
And I have Inactive data configured for "Segmento" network
When I navigate to "Graphic Network Configuration" page
And I select "Segmento" from "Category"
And I select SegmentValue from "NodeFilter" "Element" "dropdown"
And I select Todos from Node dropdown
And I click on "NodeFilter" "ViewReport" "button"
And  I select the output of an "active" node and drag and drop it into the input of an "inactive" node
Then Represent the connection attempt graphically with graphic style Improper Connections
And Following message "No se pueden crear conexiones desde o hacia nodos inactivos" should be appear

@testcase=37395 @bvt @bvt1.5
Scenario: Verify the application cannot allow to make connections graphically from inactive to active node
And I have Inactive data configured for "Segmento" network
When I navigate to "Graphic Network Configuration" page
And I select "Segmento" from "Category"
And I select SegmentValue from "NodeFilter" "Element" "dropdown"
And I select Todos from Node dropdown
And I click on "NodeFilter" "ViewReport" "button"
And  I select the output of an "Inactive" node and drag and drop it into the input of an "Active" node
Then Represent the connection attempt graphically with graphic style Improper Connections
And Following message "No se pueden crear conexiones desde o hacia nodos inactivos" should be appear

@testcase=37396 @bvt @bvt1.5
Scenario Outline: Verify the system should not allow to create a duplicate connection graphically
And I have duplicate data configured for  "<Category_Options>"
When Create a connection between both the nodes
And I navigate to "Graphic Network Configuration" page
And I select "<Category_Options>" from "Category"
And I select SegmentValue from "NodeFilter" "Element" "dropdown"
And I select Todos from Node dropdown
And I click on "NodeFilter" "ViewReport" "button"
And Again create a duplicate connection between both the nodes
Then Represent the connection attempt graphically with graphic style Improper Connections
And Following message "Ya existe una conexión entre los nodos" should be appear

Examples:
| Category_Options |
| Segmento         |
| Sistema          |

@testcase=37397 @api @output=QueryAll(GetNodeConnection) @bvt @bvt1.5
Scenario: Create a connection between two nodes that is inactive state
And I want to create a "node-connection" in the system
When I provide inactive Node details while creating node connection
Then the response should fail with message "No se pueden crear conexiones desde o hacia nodos inactivos"

@testcase=37398 @bvt
Scenario Outline: Verify the application cannot allow to make connections graphically between 2 inactive Nodes
And I have Inactive data configured for "<Category_Options>" network
When I navigate to "Graphic Network Configuration" page
And I select "<Category_Options>" from "Category"
And I choose CategoryElement from "nodeFilter" "Element" "dropdown"
And I select Todos from Node dropdown
And I click on "NodeFilter" "ViewReport" "button"
And I select the output of an inactive node and drag and drop it into the input of an inactive node
Then Represent the connection attempt graphically with graphic style Improper Connections
And Following message "No se pueden crear conexiones desde o hacia nodos inactivos" should be appear

Examples:
| Category_Options |
| Segmento         |
| Sistema          |

@testcase=37399 @bvt
Scenario Outline: Verify the application cannot allow to make connections graphically between 2 inactive Nodes for which a connection already exists
And I have Inactive data configured for "<Category_Options>" network
When I update the record and connect both the node which is inactive
And I navigate to "Graphic Network Configuration" page
And I select "<Category_Options>" from "Category"
And I choose CategoryElement from "nodeFilter" "Element" "dropdown"
And I select Todos from Node dropdown
And I click on "NodeFilter" "ViewReport" "button"
And I select the output of an inactive node and drag and drop it into the input of an inactive node
Then Represent the connection attempt graphically with graphic style Improper Connections
And Following message "Ya existe una conexión entre los nodos" should be appear

Examples:
| Category_Options |
| Segmento         |
| Sistema          |

@testcase=37400 @bvt @bvt1.5
Scenario Outline: Verify the application cannot allow to make connections graphically between active to inactive node for which a connection already exists
And I have Inactive data configured for "<Category_Options>" network
When Create a connection between both the nodes
And I update the record and make second node as inactive
And I navigate to "Graphic Network Configuration" page
And I select "<Category_Options>" from "Category"
And I choose CategoryElement from "nodeFilter" "Element" "dropdown"
And I select Todos from Node dropdown
And I click on "NodeFilter" "ViewReport" "button"
And I select the output of an active node and drag and drop it into the input of an inactive node for which the connections already established
Then Represent the connection attempt graphically with graphic style Improper Connections
And Following message "Ya existe una conexión entre los nodos" should be appear

Examples:
| Category_Options |
| Segmento         |
| Sistema          |

@testcase=37401 @bvt
Scenario Outline: Verify the application cannot allow to make connections graphically between inactive to active node for which a connection already exists
And I have Inactive data configured for "<Category_Options>" network
When I navigate to "Graphic Network Configuration" page
And I select "<Category_Options>" from "Category"
And I choose CategoryElement from "nodeFilter" "Element" "dropdown"
And I select Todos from Node dropdown
And I click on "NodeFilter" "ViewReport" "button"
And I update the record and make first node as inactive
And I select the output of an inactive node and drag and drop it into the input of an active node for which the connections already established
Then Represent the connection attempt graphically with graphic style Improper Connections
And Following message "Ya existe una conexión entre los nodos" should be appear

Examples:
| Category_Options |
| Segmento         |
| Sistema          |
