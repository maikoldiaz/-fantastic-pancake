@sharedsteps=4013 @owner=jagudelos @ui @testsuite=31116 @testplan=31102 @parallel=false
Feature: ManageNetworkConfiguration
In order to manage network configuration
As an application administrator
I need to see the current network configuration

Background: Login
Given I am logged in as "admin"

@testcase=32769 @bvt @version=2 @bvt1.5
Scenario: Verify the graphical representation of a segment network
Given I have data configured for "Segmento" network
When I navigate to "Graphic Network Configuration" page
And I select "Segmento" from "Category"
And I select SegmentValue from "NodeFilter" "Element" "dropdown"
And I select Todos from Node dropdown
And I click on "NodeFilter" "ViewReport" "button"
Then I should see the active nodes that belong to the "segment" on the current date
And I should see the inactive nodes that belong to the "segment" on the current date
And I should see the active connections between nodes
And I should see the inactive connections between nodes
And I should see the ordered network so that the nodes and their connections can be visualized

@testcase=32770 @bvt @version=2
Scenario: Verify the graphical representation of a system network
Given I have data configured for "Sistema" network
When I navigate to "Graphic Network Configuration" page
And I select "Sistema" from "Category"
And I select SegmentValue from "NodeFilter" "Element" "dropdown"
And I select Todos from Node dropdown
And I click on "NodeFilter" "ViewReport" "button"
Then I should see the active nodes that belong to the "system" on the current date
And I should see the inactive nodes that belong to the "system" on the current date
And I should see the active connections between nodes
And I should see the inactive connections between nodes
And I should see the ordered network so that the nodes and their connections can be visualized

@testcase=32771 @bvt @version=2
Scenario: Verify the graphical representation of a node
Given I have data configured for "Segmento" network
When I navigate to "Graphic Network Configuration" page
And I select "Segmento" from "Category"
And I select SegmentValue from "NodeFilter" "Element" "dropdown"
And I select any "Active" node from node dropdown
And I click on "NodeFilter" "ViewReport" "button"
Then I should see the selected node

@testcase=32772 @bvt @version=2 @bvt1.5
Scenario Outline: Verify the graphical representation of an active node
Given I have data configured for "<Entity>" network
When I navigate to "Graphic Network Configuration" page
And I select "<Entity>" from "Category"
And I select SegmentValue from "NodeFilter" "Element" "dropdown"
And I select Todos from Node dropdown
And I click on "NodeFilter" "ViewReport" "button"
Then I should see all the specified details about the "Active" node
And I should see the node according to the graphic standard defined for the "Active" nodes
And I should see the node with the color defined for the segment to which the "Inactive" node belongs
And I should see the node with an icon that represents the node type

Examples:
| Entity   |
| Segmento |
| Sistema  |

@testcase=37363 @bvt @bvt1.5
Scenario: Verify the graphical representation of an active node when the chain of supply is from node
Given I have data configured for "Segmento" network
When I navigate to "Graphic Network Configuration" page
And I select "Segmento" from "Category"
And I select SegmentValue from "NodeFilter" "Element" "dropdown"
And I select any "Active" node from node dropdown
And I click on "NodeFilter" "ViewReport" "button"
Then I should see all the specified details about the "Active" node
And I should see the node according to the graphic standard defined for the "Active" nodes
And I should see the node with the color defined for the segment to which the "Active" node belongs
And I should see the node with an icon that represents the node type

@testcase=32773 @bvt @version=2
Scenario Outline: Verify the graphical representation of an inactive node
Given I have data configured for "<Entity>" network
When I navigate to "Graphic Network Configuration" page
And I select "<Entity>" from "Category"
And I select SegmentValue from "NodeFilter" "Element" "dropdown"
And I select Todos from Node dropdown
And I click on "NodeFilter" "ViewReport" "button"
Then I should see all the specified details about the "Inactive" node
And I should see the node according to the graphic standard defined for the "Inactive" nodes
And I should see the node with the color defined for the segment to which the "Active" node belongs
And I should see the node with locked icon

Examples:
| Entity   |
| Segmento |
| Sistema  |

@testcase=37364 @bvt
Scenario: Verify the graphical representation of an inactive node when the chain of supply is from node
Given I have data configured for "Segmento" network
When I navigate to "Graphic Network Configuration" page
And I select "Segmento" from "Category"
And I select SegmentValue from "NodeFilter" "Element" "dropdown"
And I select any "Inactive" node from node dropdown
And I click on "NodeFilter" "ViewReport" "button"
Then I should see all the specified details about the "Inactive" node
And I should see the node according to the graphic standard defined for the "Inactive" nodes
And I should see the node with the color defined for the segment to which the "Inactive" node belongs
And I should see the node with locked icon

@testcase=32774 @bvt @version=2
Scenario Outline: Verify the graphical representation of an active connection
Given I have data configured for "<Entity>" network
When I navigate to "Graphic Network Configuration" page
And I select "<Entity>" from "Category"
And I select SegmentValue from "NodeFilter" "Element" "dropdown"
And I select Todos from Node dropdown
And I click on "NodeFilter" "ViewReport" "button"
Then I should see the connections according to the graphic standard defined for the "active" connections

Examples:
| Entity   |
| Segmento |
| Sistema  |

@testcase=32775 @bvt @version=2
Scenario Outline: Verify the graphical representation of an inactive connection
Given I have data configured for "<Entity>" network
When I navigate to "Graphic Network Configuration" page
And I select "<Entity>" from "Category"
And I select SegmentValue from "NodeFilter" "Element" "dropdown"
And I select Todos from Node dropdown
And I click on "NodeFilter" "ViewReport" "button"
Then I should see the connections according to the graphic standard defined for the "inactive" connections

Examples:
| Entity   |
| Segmento |
| Sistema  |

@testcase=32776 @bvt @version=2 @bvt1.5
Scenario Outline: Verify the graphical representation of a connection which is marked as transfer point
Given I have data configured for "<Entity>" network
When I navigate to "Graphic Network Configuration" page
And I select "<Entity>" from "Category"
And I select SegmentValue from "NodeFilter" "Element" "dropdown"
And I select Todos from Node dropdown
And I click on "NodeFilter" "ViewReport" "button"
Then I should see the connections according to the graphic standard defined for the "transfer" connections

Examples:
| Entity   |
| Segmento |
| Sistema  |
