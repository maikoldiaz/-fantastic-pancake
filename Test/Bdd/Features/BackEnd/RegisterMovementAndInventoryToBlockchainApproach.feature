@backend  @testsuite=26828 @testplan=26817 @owner=jagudelos
Feature: RegisterMovementAndInventoryToBlockchainApproach
As blockchain Architect I need to change the approach
for storing the data in order to improve the
data consistency and reliability

@testcase=29909 @bvt
Scenario: Verify when a TRUE user Register a new Movement with valid data
Given I want to register a "Movements" in the system with valid data
When Movement registered successfully
Then Registered Record must be stored in database with "DataWrittenToBlockchain" is flagged as "Pending"
And Send a Message to servicebus Queue indicating that ready to register in block chain
And data should be registered at blockchain

@testcase=29910 @bvt
Scenario: Verify when a TRUE user registers new Movement with data consistency
Given I want to register a "Movements" in the system with valid data
When I have Movements in the system with this node connection as source or destination
Then Registered Record must be stored in database with "DataWrittenToBlockchain" is flagged as "Pending"
And data should be registered at blockchain
And I Perform update and we must capture the update or creation event from blockchain
And update the register at the data base

@testcase=29911 @bvt
Scenario: Verify when a TRUE user Register a new Inventory with valid data
Given I want to register a "Inventory" in the system with valid data
When Inventory registered successfully
Then Registered Record must be stored in database with "DataWrittenToBlockchain" is flagged as "Pending"
And Send a Message to servicebus Queue indicating that ready to register in block chain
And data should be registered at blockchain

@testcase=29912 @bvt
Scenario: Verify when a TRUE user registers new Inventory with data consistency
Given I want to register a "Inventory" in the system with valid data
When I have Inventory in the system
Then Registered Record must be stored in database with "DataWrittenToBlockchain" is flagged as "Pending"
And data should be registered at blockchain
And I Perform update and we must capture the update or creation event from blockchain
And update the register at the data base
