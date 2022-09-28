@sharedsteps=52139 @api @owner=jagudelos @testplan=26817 @testsuite=26829
Feature: RegisterNodeStatusToBlockchain
As blockchain Architect I need to change
how the system is storing the supply chain
nodes at blockchain in order to track all the
changes related with these nodes

Background: Login
Given I am authenticated as "admin"

@testcase=28178 @bvt @version=2 @prodready1.5
Scenario: Verfiy when a TRUE User creates a node
Given I have "nodes" in the system
When I verify if Node record is created in db
Then I verify if the status for the node has been updated to "createdNode"

@testcase=28179 @bvt @ui @verion=2
Scenario: Verify when a TRUE user performs cutoff with nodes
Given I have operational cutoff data generated in the system
When I verify if Node record is created in db
Then I verify if the status for the node has been updated to "operativeBalanceCalculated"

@testcase=28180 @bvt @version=2
Scenario: Verify when a true user updates a node
Given I have "Nodes" in the system
When I update the order of nodes in a segment
Then I verify if the status for the node has been updated to "updatedNode"

@testcase=28181 @bvt @version=2
Scenario: Verify when a TRUE user performs ownership calculation for movements with a node in the system
Given I have ownership calculation data generated in the system
When I verify if Node record is created in db
Then I verify if the status for the node has been updated to "operativeBalanceCalculatedWithOwnership"


@testcase=28182 @bvt @version=2
Scenario: Verfiy when a TRUE User creates a node connection
Given I have "node connections" in the system
Then I verify if node connection record is created in blockchain register

@testcase=28183 @bvt @ignore @version=2
Scenario: Verify when a TRUE user performs cutoff with node connection-connections
Given I have "node connections" in the system
When I have Movements in the system with this node connection as source or destination
And I Perform operational cutoff for them
Then I verify if the status for the node connection has been updated to "OperativeBalanceCalculated" in blockchain register

@testcase=28184 @bvt @ignore @version=2
Scenario: Verify when a true user updates a node connection
Given I have "node connections" in the system
When I update the order of node connection-connections in a segment
Then I verify the status of the node connection in the blockchain register is 'updated'

@testcase=28185 @bvt @ignore @version=2
Scenario: Verify when a TRUE user performs ownership calculation for movements with a node connection in the system
Given I have "node connections" in the system
When I have Movements in the system with this node connection as source or destination
And I Perform ownership calculation
Then I verify if the status for the node connection has been updated to "OperativeBalanceCalculatedWithOwnership" in blockchain register


