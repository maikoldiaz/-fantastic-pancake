@owner=jagudelos @ui @MVP2and3 @S15 @testsuite=61557 @testplan=61542
Feature: ApproveDeltaAdjustments
As a user of chain, I require to approve delta adjustments
to have the official approved balance of a node

@testcase=66801 @version=2 @BVT2
Scenario Outline: Verify Node approval button is enabled when the status is Deltas
	Given I am logged in as "<User>"
	And I have existing official node balance
	When I navigate to "Official deltas by node" page
	And I select "Deltas" from the "officialDeltaNodesGrid" "status" "dropdown" filter
	And I should see "officialDeltaNodesGrid" "viewReport" "0" "link" as enabled
	And I click on "officialDeltaNodesGrid" "viewReport" "0" "link"
	And I should see breadcrumb "Balance oficial por nodo"
	Then I should see "submitForApproval" "button" as enabled

	Examples:
		| User  |
		| admin |
		| chain |

@testcase=66802 @version=2 @BVT2
Scenario Outline: Verify Node approval button is disabled when the status is different from Deltas
	Given I am logged in as "<User>"
	And I have existing official node balance
	When I navigate to "Official deltas by node" page
	And I select "Aprobado" from the "officialDeltaNodesGrid" "status" "dropdown" filter
	And I should see "officialDeltaNodesGrid" "viewReport" "0" "link" as enabled
	And I click on "officialDeltaNodesGrid" "viewReport" "0" "link"
	And I should see breadcrumb "Balance oficial por nodo"
	Then I should see "submitForApproval" "button" as disabled

	Examples:
		| User  |
		| admin |
		| chain |

@testcase=66803 @bvt @version=2 
Scenario Outline: Verify Node approval when the node has no predecessors
	Given I am logged in as "<User>"
	And I have valid official Movements and Inventories with same identifier in the system
	And I have Calculate deltas by official adjustment
	When I navigate to "Official deltas by node" page
	And I filter NodeName in the grid which has "no predecessors"
	And I should see "officialDeltaNodesGrid" "viewReport" "0" "link" as enabled
	And I click on "officialDeltaNodesGrid" "viewReport" "0" "link"
	And I should see breadcrumb "Balance oficial por nodo"
	And I click on "submitForApproval" "button"
	Then I should be redirected to "Official deltas by node" page
	And I filter NodeName which is selected above in the grid which has "no predecessors"
	And I should see the state of the node as "Enviado a aprobación"

	Examples:
		| User  |
		| admin |
		| chain |

@testcase=66804 @bvt @version=2 @BVT2
Scenario: Verify Node approval when the node has predecessors and all predecessors are from another segment
	Given I am logged in as "admin"
	And I have valid official Movements and Inventories with same identifier in the system
	And I have Calculate deltas by official adjustment
	When I navigate to "Official deltas by node" page
	And I filter NodeName in the grid which has "no predecessors for same segment"
	And I should see "officialDeltaNodesGrid" "viewReport" "0" "link" as enabled
	And I click on "officialDeltaNodesGrid" "viewReport" "0" "link"
	And I should see breadcrumb "Balance oficial por nodo"
	And I click on "submitForApproval" "button"
	Then I should be redirected to "Official deltas by node" page
	And I should see the state of the node as "Enviado a aprobación"

@testcase=66805 @version=2 @manual
Scenario Outline: Verify Node approval when the node has predecessors and some predecessors are NOT from another segment
	Given I am logged in as "<User>"
	And I have valid official Movements and Inventories with same identifier in the system
	And I have Calculate deltas by official adjustment
	When I navigate to "Official deltas by node" page
	And I filter NodeName in the grid which has "lower predecessors status as Sent to Approval"
	And I Validate that all predecessor nodes of the same segment with lower order in the chain are in the “Sent to Approval" state
	And I should see "officialDeltaNodesGrid" "viewReport" "0" "link" as enabled
	And I click on "officialDeltaNodesGrid" "viewReport" "0" "link"
	And I should see breadcrumb "Balance oficial por nodo"
	And I click on "submitForApproval" "button"
	Then I should see the state of the node as "Enviado a aprobación"

	Examples:
		| User  |
		| admin |
		| chain |

@testcase=66806 @version=2 @manual
Scenario Outline: Verify Node approval when the node has predecessors and lower-order segment in the chain of different segment status is not "Sent to Approval"
	Given I am logged in as "<User>"
	And I have valid official Movements and Inventories with same identifier in the system
	And I have Calculate deltas by official adjustment
	When I navigate to "Official deltas by node" page
	And I filter NodeName in the grid which has "lower predecessors from different Segment"
	And I should see "officialDeltaNodesGrid" "viewReport" "0" "link" as enabled
	And I click on "officialDeltaNodesGrid" "viewReport" "0" "link"
	And I should see breadcrumb "Balance oficial por nodo"
	And I click on "submitForApproval" "button"
	Then I should see the message "El nodo no puede ser enviado a aprobación porque en la cadena existen nodos que deben ser aprobados primero"

	Examples:
		| User  |
		| admin |
		| chain |

@testcase=66807 @version=2 @manual
Scenario Outline: Verify Node approval when the node has predecessors and all predecessors are of the same segment
	Given I am logged in as "<User>"
	And I have valid official Movements and Inventories with same identifier in the system
	And I have Calculate deltas by official adjustment
	When I navigate to "Official deltas by node" page
	And I filter NodeName in the grid which has "lower predecessors status as Sent to Approval"
	And I Validate that all predecessor nodes of the same segment with lower order in the chain are in the “Sent to Approval" state
	And I should see "officialDeltaNodesGrid" "viewReport" "0" "link" as enabled
	And I click on "officialDeltaNodesGrid" "viewReport" "0" "link"
	And I should see breadcrumb "Balance oficial por nodo"
	And I click on "submitForApproval" "button"
	Then I should see the state of the node as "Enviado a aprobación"

	Examples:
		| User  |
		| admin |
		| chain |

@testcase=66808 @version=2 @manual
Scenario Outline: Verify Node approval when the node has predecessors and all lower-order segment in the chain of same segment status is not "Sent to Approval"
	Given I am logged in as "<User>"
	And I have valid official Movements and Inventories with same identifier in the system
	And I have Calculate deltas by official adjustment
	When I navigate to "Official deltas by node" page
	And I filter NodeName in the grid which has "lower predecessors status as not Sent to Approval"
	And I should see "officialDeltaNodesGrid" "viewReport" "0" "link" as enabled
	And I click on "officialDeltaNodesGrid" "viewReport" "0" "link"
	And I should see breadcrumb "Balance oficial por nodo"
	And I click on "submitForApproval" "button"
	Then I should see the message "El nodo no puede ser enviado a aprobación porque en la cadena existen nodos que deben ser aprobados primero"

	Examples:
		| User  |
		| admin |
		| chain |

@testcase=66809 @version=2
Scenario Outline: Verify Back to list button when the page is loaded from the node details list for a segment
	Given I am logged in as "<User>"
	When I navigate to "Official deltas by node" page
	And I select "Deltas" from the "officialDeltaNodesGrid" "status" "dropdown" filter
	And I should see "officialDeltaNodesGrid" "viewReport" "0" "link" as enabled
	And I click on "officialDeltaNodesGrid" "viewReport" "0" "link"
	And I should see breadcrumb "Balance oficial por nodo"
	And I should see "returnDeltaNodeListing" "button" as enabled

	Examples:
		| User  |
		| admin |
		| chain |

@testcase=66810 @version=2
Scenario Outline: Verify Back to list button when the page is loaded directly from the official node balance menu option directly
	Given I am logged in as "<User>"
	When I navigate to "Balance oficial por nodo" page
	And I select "CategoryElement" from the "nodeFilter" "element" "dropdown"
	And I choose node from "node" "combobox" which state is "Sent to Approval"
	And I Select period from "nodeFilter" "periods"
	And I click on "nodeFilter" "viewReport" "button"
	Then I should see breadcrumb "Balance oficial por nodo"
	And I should see "returnDeltaNodeListing" "button" is hidden
	And I should see "submitForApproval" "button" is hidden

	Examples:
		| User  |
		| admin |
		| chain |

@testcase=66811 @version=2
Scenario Outline: Verify Node approval button is disabled when the record is already sent for approval
	Given I am logged in as "<User>"
	And I have existing official node balance
	When I navigate to "Official deltas by node" page
	And I select "Enviado a aprobación" from the "officialDeltaNodesGrid" "status" "dropdown" filter
	And I should see "officialDeltaNodesGrid" "viewReport" "0" "link" as enabled
	And I click on "officialDeltaNodesGrid" "viewReport" "0" "link"
	And I should see breadcrumb "Balance oficial por nodo"
	Then I should see "submitForApproval" "button" as disabled

	Examples:
		| User  |
		| admin |
		| chain |