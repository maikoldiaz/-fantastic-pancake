@sharedsteps=7539 @owner=jagudelos @api @testplan=61542 @testsuite=66734
Feature: BlockchainNewFeature
As an application administrator,
I want to check data in Blockchain

Background: Login
Given I am authenticated as "admin"

@testcase=66751 @output=QueryAll(GetNodes)
Scenario: Verify Node Data in Blockchain when it is created
Given I want to create a "Node" in the system
When I provide the required fields
And  a record should be "inserted" into "offchain.Node" Table
Then Node Data should be registered in Blockchain with status "CreatedNode"

@testcase=66752 @api @output=QueryAll(GetNodes)
Scenario: Verify Update Node with valid data in Blockchain
Given I have "Node" in the system
When I update a record with required fields
Then the response should succeed with message "Nodo actualizado con éxito"
And  a record should be "updated" into "offchain.Node" Table
And Node Data should be registered in Blockchain with status "UpdatedNode"

@testcase=66753 @api @bvt @output=QueryAll(GetNodeConnection)
Scenario: Create a connection between two nodes when origin and destination node identifiers are same and validate in Blockchain
Given I want to create a "node-connection" in the system
When I provide the required fields
Then the response should succeed with message "Conexión creada con éxito"
And Node Connection Data should be registered in Blockchain with status "CreatedNode"

@testcase=66754 @api @bvt @output=QueryAll(GetNodeConnection)
Scenario: Check Node Status in Blockchain when Balance is calculated
Given I have nodes in segment whose balance is calculated
When I check the nodes status
Then a record should be "updated" into "offchain.Node" Table
And Node Data should be registered in Blockchain with status "OperativeBalanceCalculated"

@testcase=66755 @api @bvt @output=QueryAll(GetNodeConnection)
Scenario: Check Node Status in Blockchain when Ownership is calculated
Given I have nodes in segment whose ownership is calculated
When I check the nodes status
Then a record should be "updated" into "offchain.Node" Table
And Node Data should be registered in Blockchain with status "OperativeBalanceCalculatedWithOwnership"

@testcase=66756
Scenario: Verify updated Movement Data in Blockchain
Given I have movement data
When I submit movement data
Then updated Movement details should be registered in Blockchain
And "Movement" Owner data should be stored in Blockchain

@testcase=66757
Scenario: Verify updated Inventory Data in Blockchain
Given I have inventory data
When I submit inventory data
Then updated Inventory details should be registered in Blockchain
And "Inventory"Owner data should be stored in Blockchain
@testcase=66758
Scenario: Verify updated Ownership Data of Movements in Blockchain
Given I have movement data
When I execute operational cutoff with "movement"
Then "Movement" Ownership data should be stored in Blockchain

@testcase=66759 
Scenario: Verify updated Ownership Data of Inventory in Blockchain
Given I have inventory data
When I execute operational cutoff with "inventory"
Then "Inventory" Ownership data should be stored in Blockchain


