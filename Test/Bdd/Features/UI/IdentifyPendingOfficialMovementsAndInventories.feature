@sharedsteps=4013 @ui @owner=jagudelos  @testplan=61542 @testsuite=61554 @S15 @MVP2and3
Feature: IdentifyPendingOfficialMovementsAndInventories
As a TRUE system, I need to identify for each period and segment the pending official movements and inventories,
which will be used to compare with the operational consolidated data and generate the corresponding deltas.

Background: Login
Given I am logged in as "admin"
@testcase=66830
Scenario: Verify segment without pending data
Given I have data generated in the system for processing an official delta calculation request
When the segment has no pending movements and inventories to execute the official deltas calculation
Then the segment ticket should be updated to failed state with error message "El segmento no tiene información oficial pendiente en el período de fechas."
@testcase=66831
Scenario: Verify pending movement of a segment
Given I have data generated in the system for processing an official delta calculation request
When all the conditions are met for pending movement of a segment
Then include the last version of the movement and its owners in the list of official movements identified to send to FICO
And main information details should be added to the list of pending movements along with owners information
@testcase=66832
Scenario: Verify pending inventory of a segment
Given I have data generated in the system for processing an official delta calculation request
When all the conditions are met for pending inventory of a segment
Then include the last version of the inventory and its owners in the list of official inventories identified to send to FICO
And main information details should be added to the list of pending inventories along with owners information
@testcase=66833
Scenario Outline: Verify excluded movement
Given I have data generated in the system for processing an official delta calculation request
When a movement belongs to a segment in the date period and the movement meets "<Condition>"
Then exclude the movement from the list of identified movements to send to FICO

Examples:
| Condition                                                                                                                                                                                        |
| The last event type of the movement is a delete                                                                                                                                                  |
| The last event type of the movement is an insert or an update but it has an official delta ticket                                                                                                |
| The creation date of the movement is less than the execution date of the last official delta ticket of the segment in the date period                                                            |
| The source or destination node of the movement is in state approved or submitted for approval or rejected in the date period                                                                     |
| The source and destination node of the movement belong to the segment on the current date but the lowest order node does not belong to the system that reported the movement on the current date |
| The source node of the movement belongs to a different segment on the current date, and the destination node does not belong to the system that reported the movement on the current date        |
| The destination node of the movement belongs to a different segment on the current date, and the source node does not belong to the system that reported the movement on the current date        |
@testcase=66834
Scenario Outline: Verify excluded inventory
Given I have data generated in the system for processing an official delta calculation request
When an inventory in the segment has an inventory date equal to the start or end date of the period and the inventory meets "<Condition>"
Then exclude the inventory from the list of identified inventories to send to FICO

Examples:
| Condition                                                                                                                       |
| The last event type of inventory is a delete                                                                                    |
| The last event type of inventory is an insert or update but it has official delta ticket                                        |
| The inventory creation date is less than the execution date of the last official delta ticket of the segment in the date period |
| The inventory node is in state approved or submitted for approval or rejected in the date period                                |
@testcase=66835
Scenario: Verify pending nodes
Given I have data generated in the system for processing an official delta calculation request
When there are official movements and inventories identified to be sent to FICO
Then a unique list of nodes should be formed from source and destination nodes of the movements and nodes of the inventories
And nodes that belong to a different segment on the current date should be excluded from the node list
And the pending nodes should be registered with processing status and official delta ticket of the segment should be assigned
@testcase=66836
Scenario: Remove deltas from pending nodes
Given I have data generated in the system for processing an official delta calculation request
When there are pending nodes and the delta calculation process ends successfully
Then remove the deltas from the pending nodes
And delete pending nodes from the previous tickets for the same period
@testcase=66837 
Scenario: Verify technical exception scenario
Given I have data generated in the system for processing an official delta calculation request
When a technical exception occurs during the process of identifying official movements and inventories of the segment
Then the status of the segment ticket should be updated as failed with error "Se presentó un error técnico inesperado durante el proceso de identificación de información oficial. Por favor ejecute nuevamente el proceso o comuníquese con la mesa de ayuda."
And the exception should be registered in application insights
