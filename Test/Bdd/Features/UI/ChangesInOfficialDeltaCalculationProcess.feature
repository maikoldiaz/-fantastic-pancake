@sharedsteps=4013 @ui @owner=jagudelos @testplan=70526 @testsuite=70796
Feature: ChangesInOfficialDeltaCalculationProcess
As TRUE system, I need to execute the official deltas calculation
with reopened and rejected nodes to complete the delta calculation
with new versions of the official balance

Background: Login
Given I am logged in as "admin"
@testcase=71694
Scenario: Verify the processing of Official Delta Calculation Request when node status is Deltas or Rejected and has not been previously approved.
Given I have data segment to process the official Delta Calculation
When A node in the date period is already in the node list of official delta tickets
And The node status is Deltas or Rejected and has not been previously approved
And The node has official movements or inventories identified to send to FICO that belong to this segment in the date period
Then Verify that System should register the node in the state historical table
And Verify that system should store the node, its status, its ticket number, and the status date
And Verify that system should register the node in the ticket node table with "processing" status and the official delta ticket of the segment
And System should delete movements originated by inventory deltas where the source or destination node is equal to the node in Deltas or Rejected state and the operational date is equal to the end date of the period.
And System should delete movements originated by manual inventory deltas where the source or destination node is equal to the node in Deltas or Rejected state and the operational date is equal to the end date of the period.
And system should delete movements originated by inventory deltas where the source or destination node is equal to the node in Deltas or Rejected state and the operational date is equal to the start date of the period minus one day, and the created date is greater than the approval date of the node in the previous period.
And System should delete movements originated by manual inventory deltas where the source or destination node is equal to the node in Deltas or Rejected state and the operational date is equal to the start date of the period minus one day, and the created date is greater than the approval date of the node in the previous period.
And System should delete movements originated by movement deltas where the source or destination node is equal to the node in Deltas or Rejected state and the start and end dates are equal to the start and end dates of the period.
And System should delete movements originated by manual movement deltas where the source or destination node is equal to the node in Deltas or Rejected state and the start and end dates are equal to the start and end dates of the period.
And System should delete the node from previous tickets from the same period.
@testcase=71695
Scenario: Verify the processing of Official Delta Calculation Request when node status is Deltas or Rejected and has been previously approved.
Given I have data segment to process the official Delta Calculation
When A node in the date period is already in the node list of official delta tickets
And The node status is Deltas or Rejected and has been previously approved
And The node has official movements or inventories identified to send to FICO that belong to this segment in the date period
Then Verify that System should register the node in the state historical table
And Verify that system should store the node, its status, its ticket number, and the status date
And Verify that system should update the node ticket by the new official delta ticket of the segment and update the status of the nodes to processing.
And System should delete movements originated by inventory deltas where the source or destination node is equal to the node in Deltas or Rejected state and the operational date is equal to the start date of the period minus one day or equal to the end date of the period, and the created date is greater than the approval date of the node.
And System should delete movements originated by manual inventory deltas where the source or destination node is equal to the node in Deltas or Rejected state and the operational date is equal to the start date of the period minus one day or equal to the end date of the period, and the created date is greater than the approval date of the node.
And System should delete movements originated by movement deltas where the source or destination node is equal to the node in Deltas or Rejected state and the start and end dates are equal to the start and end dates of the period, and the created date is greater than the approval date of the node.
And System should delete movements originated by manual movement deltas where the source or destination node is equal to the node in Deltas or Rejected state and the start and end dates are equal to the start and end dates of the period, and the created date is greater than the approval date of the node.
@testcase=71696
Scenario: Verify the processing of Official Delta Calculation Request when node status is Reopened.
Given I have data segment to process the official Delta Calculation
When A node in the date period is already in the node list of official delta tickets
And The node status is Reopened
And The node has official movements or inventories identified to send to FICO that belong to this segment in the date period
Then Verify that System should register the node in the state historical table
And Verify that system should store the node, its status, its ticket number, and the status date
And Verify that system should update the node ticket by the new official delta ticket of the segment and update the status of the nodes to processing
@testcase=71697 
Scenario: Verify the processing of Official Delta Calculation Request when node status is Rejected or Reopened states and the nodes have official entries and inventories identified to send to FICO.
Given I have data segment to process the official Delta Calculation
And The node status is Rejected or Reopened states and the nodes have official movements and inventories identified to send to FICO.
When The node has official movements or inventories identified to send to FICO that belong to this segment in the date period
Then Verify that  "balanceOficialInventarios" collection contains the official inventories of the nodes in the Reopened or Rejected state
And Verify that If there are inventories with a date equal to the start date of the period, then send FICO the inventory date minus one day, otherwise send the date unchanged.
And Verify that "balanceOficialMovimientos" collection contains the movements of the nodes in the Reopened or Rejected state.
