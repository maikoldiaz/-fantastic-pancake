@sharedsteps=4013 @owner=jagudelos @ui @testsuite=35686 @testplan=35673 @parallel=false
Feature: NetworkGraphStylesAndEvents
In order to improve the visualization of nodes and connections
As an application administrator
I need some styles and events to be added to the network graph

Background: Login
	Given I am logged in as "admin"

@testcase=37373 @bvt @version=2 @bvt1.5
Scenario Outline: Verify zoom in functionality
	And I have data configured for "<Entity>" network
	When I navigate to "Graphic Network Configuration" page
	And I select "<Category>" from "Category"
	And I select SegmentValue from "NodeFilter" "Element" "dropdown"
	And I select "<NodeName>" value from "NodeFilter" "Node" "combobox"
	And I click on "NodeFilter" "ViewReport" "button"
	And I click on "ZoomIn" "button"
	Then the system should perform the "ZoomIn" action on the network graph

	Examples:
		| Entity   | Category | NodeName |
		| Segmento | Segmento | Todos    |
		| Sistema  | Sistema  | Todos    |
		| Nodo     | Segmento | NodeName |

@testcase=37374 @bvt @version=2 @bvt1.5
Scenario Outline: Verify zoom out functionality
	And I have data configured for "<Entity>" network
	When I navigate to "Graphic Network Configuration" page
	And I select "<Category>" from "Category"
	And I select SegmentValue from "NodeFilter" "Element" "dropdown"
	And I select "<NodeName>" value from "NodeFilter" "Node" "combobox"
	And I click on "NodeFilter" "ViewReport" "button"
	And I click on "ZoomOut" "button"
	Then the system should perform the "ZoomOut" action on the network graph

	Examples:
		| Entity   | Category | NodeName |
		| Segmento | Segmento | Todos    |
		| Sistema  | Sistema  | Todos    |
		| Nodo     | Segmento | NodeName |

@testcase=37375 @bvt @version=2 @bvt1.5
Scenario Outline: Verify reset zoom functionality
	And I have data configured for "<Entity>" network
	When I navigate to "Graphic Network Configuration" page
	And I select "<Category>" from "Category"
	And I select SegmentValue from "NodeFilter" "Element" "dropdown"
	And I select "<NodeName>" value from "NodeFilter" "Node" "combobox"
	And I click on "NodeFilter" "ViewReport" "button"
	And I click on "ZoomIn" "button"
	And I click on "ResetZoom" "button"
	Then the system should perform the "ResetZoom" action on the network graph

	Examples:
		| Entity   | Category | NodeName |
		| Segmento | Segmento | Todos    |
		| Sistema  | Sistema  | Todos    |
		| Nodo     | Segmento | NodeName |

@testcase=37376 @bvt @version=2
Scenario Outline: Order nodes and connections in network graph
	And I have data configured for "<Entity>" network
	When I navigate to "Graphic Network Configuration" page
	And I select "<Entity>" from "Category"
	And I select SegmentValue from "NodeFilter" "Element" "dropdown"
	And I select "Todos" value from "NodeFilter" "Node" "combobox"
	And I click on "NodeFilter" "ViewReport" "button"
	And I click on "Sort" "button"
	Then the system should order the nodes and connections with the default functionality

	Examples:
		| Entity   |
		| Segmento |
		| Sistema  |

@testcase=37377 @bvt @version=2 @bvt1.5
Scenario Outline: Toggle background grid in network graph
	And I have data configured for "<Entity>" network
	When I navigate to "Graphic Network Configuration" page
	And I select "<Category>" from "Category"
	And I select SegmentValue from "NodeFilter" "Element" "dropdown"
	And I select "<NodeName>" value from "NodeFilter" "Node" "combobox"
	And I click on "NodeFilter" "ViewReport" "button"
	And I click on "ToggleBackgroundGrid" "button"
	Then the system should perform the corresponding action

	Examples:
		| Entity   | Category | NodeName |
		| Segmento | Segmento | Todos    |
		| Sistema  | Sistema  | Todos    |
		| Nodo     | Segmento | NodeName |

@testcase=37378 @bvt @version=2 @bvt1.5
Scenario Outline: Verify option to move nodes
	And I have data configured for "<Entity>" network
	When I navigate to "Graphic Network Configuration" page
	And I select "<Category>" from "Category"
	And I select SegmentValue from "NodeFilter" "Element" "dropdown"
	And I select "<NodeName>" value from "NodeFilter" "Node" "combobox"
	And I click on "NodeFilter" "ViewReport" "button"
	And I move a node with the click and drag action over any area of the node
	Then the node should move within the grid

	Examples:
		| Entity   | Category | NodeName |
		| Segmento | Segmento | Todos    |
		| Sistema  | Sistema  | Todos    |
		| Nodo     | Segmento | NodeName |

@testcase=37379 @bvt @version=2 @bvt1.5
Scenario Outline: Verify node hover functionality
	And I have data configured for "<Entity>" network
	When I navigate to "Graphic Network Configuration" page
	And I select "<Category>" from "Category"
	And I select SegmentValue from "NodeFilter" "Element" "dropdown"
	And I select "<NodeName>" value from "NodeFilter" "Node" "combobox"
	And I click on "NodeFilter" "ViewReport" "button"
	And I hovers over a node
	Then I should see the node according to the style defined for "Hovered Node"

	Examples:
		| Entity   | Category | NodeName |
		| Segmento | Segmento | Todos    |
		| Sistema  | Sistema  | Todos    |
		| Nodo     | Segmento | NodeName |

@testcase=37380 @bvt @version=2 @bvt1.5
Scenario Outline: Verify node select functionality
	And I have data configured for "<Entity>" network
	When I navigate to "Graphic Network Configuration" page
	And I select "<Category>" from "Category"
	And I select SegmentValue from "NodeFilter" "Element" "dropdown"
	And I select "<NodeName>" value from "NodeFilter" "Node" "combobox"
	And I click on "NodeFilter" "ViewReport" "button"
	And I select a node
	Then I should see the node according to the style defined for "Selected Node"
	And I should see the input and output active connections of the node according to the style defined for "Selected Active Connections"

	Examples:
		| Entity   | Category | NodeName |
		| Segmento | Segmento | Todos    |
		| Sistema  | Sistema  | Todos    |
		| Nodo     | Segmento | NodeName |

@testcase=37381 @bvt @version=2 @bvt1.5
Scenario Outline: Verify hovered active connection functionality
	And I have data configured for "<Entity>" network
	When I navigate to "Graphic Network Configuration" page
	And I select "<Entity>" from "Category"
	And I select SegmentValue from "NodeFilter" "Element" "dropdown"
	And I select "Todos" value from "NodeFilter" "Node" "combobox"
	And I click on "NodeFilter" "ViewReport" "button"
	And I hovers over an active connection
	Then I should see the connection according to the style defined for "Hovered Active Connection"

	Examples:
		| Entity   |
		| Segmento |
		| Sistema  |

@testcase=37382 @bvt @version=2 @bvt1.5
Scenario Outline: Verify selected active connection functionality
	And I have data configured for "<Entity>" network
	When I navigate to "Graphic Network Configuration" page
	And I select "<Entity>" from "Category"
	And I select SegmentValue from "NodeFilter" "Element" "dropdown"
	And I select "Todos" value from "NodeFilter" "Node" "combobox"
	And I click on "NodeFilter" "ViewReport" "button"
	And I select an active connection
	Then I should see the connection according to the style defined for "Selected Active Connection"

	Examples:
		| Entity   |
		| Segmento |
		| Sistema  |