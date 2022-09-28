@sharedsteps=4013 @owner=jagudelos @testsuite=55107 @testplan=55104 @ui
Feature: DeltasOperations
As a admin, I need to send FICO the movements and inventories pending to calculate the operating deltas

Background: Login
Given I am logged in as "admin"
@testcase=57943
Scenario: User confirms and generate a ticket for operational deltas calculation.
Given I have ownership calculation data generated in the system
When user have a request contains the "segment" and the "start" and "enddates" of the period.
And The request contains pending "movements or inventories"
Then user confirms the execution of the operational delta calculation
Then Application should generate a ticket for the delta calculation and store require "data"
Then Application should return to the "deltas calculation" page
And  status of the ticket should be as "processing"

@testcase=57944
Scenario: verify user receives a request to run the operational delta calculation for the movement types of pending movements
are NOT assigned a cancellation type
Given I have ownership calculation data generated in the system
When user have a pending "movements" with the relationship between movements types as "inactive"
And generate pending movements which are NOT assigned a "cancellation" type
And user confirms the execution of the operational delta calculation
Then Application should return to the "deltas calculation" page
And  status of the ticket should be as "failed"
And Application should store the "Movement transaction identifier" "Delta calculation ticket number" and "El tipo de movimiento [movement type name] no tiene configurado un tipo de anulación."
And The delta calculation execution process must be "finalized"

@testcase=57945
Scenario: verify user is processing an operational delta calculation ticket for original movements with a global identifier
Given I have ownership calculation data generated in the system
When movement have a global identifier and ownership ticket
And movement has other event types without "ownership ticket"
And generate the movement belongs to the segment selected in the operational delta wizard
And movement has an operational date between the dates of the period selected in the operational delta wizard
Then movement should NOT be included in the list of original movements to send to FICO

@testcase=57946
Scenario: Verify application is processing an operational delta calculation ticket for original movements where the last event type is a delete.
Given I have ownership calculation data generated in the system
When movement have an ownership ticket without a global identifier with last event type is a "delete"
And movement has other event types without "ownership ticket"
And the movement belongs to the segment selected in the operational delta wizard
And movement has an operational date between the dates of the period selected in the operational delta wizard
Then movement should NOT be included in the list of original movements to send to FICO

@testcase=57947
Scenario: verify the application is processing an operational delta calculation ticket for original movements where the last event type is an insert or update.
Given I have ownership calculation data generated in the system
When movement have an ownership ticket without a global identifier with last event type is an "insert or update"
And movement has other event types without "ownership ticket"
And Have the movement belongs to the segment selected in the operational delta wizard
And movement has an operational date between the dates of the period selected in the operational delta wizard
Then the last event type of the movement with ownership ticket must be included in the list of original movement to send to FICO

@testcase=57948
Scenario: Verify the application is processing an operational delta calculation ticket for Updated movements with a global identifier.
Given I have ownership calculation data generated in the system
When updated movement have an ownership ticket with a global identifier
And movement has other event types without "ownership ticket"
And Have the movement belongs to the segment selected in the operational delta wizard
And movement has an operational date between the dates of the period selected in the operational delta wizard
Then verify movement should NOT be included in the list of updated movements to send to FICO

@testcase=57949
Scenario: Verify the application is processing an operational delta calculation ticket for updated movements without a global identifier.
Given I have ownership calculation data generated in the system
When movement have an ownership ticket without a global identifier with last event type is an "insert or update or delete"
And movement has other event types without "ownership ticket"
And movement has an operational date between the dates of the period selected in the operational delta wizard
And the movement belongs to the segment selected in the operational delta wizard
Then verify the last event type of the movement without ownership ticket must be included in the list of updated movements to send to FICO

@testcase=57950
Scenario: Verify application is processing an operational delta calculation ticket for an original inventories where the last event type is a delete.
Given I have ownership calculation data generated in the system
When inventory have an ownership ticket with last event type is a "delete"
And inventory has other event types without "ownership ticket"
And the inventory belongs to the segment selected in the operational delta wizard
And inventory has an inventory date between the dates of the period selected in the operational delta wizard
Then verify the inventory should NOT be included in the list of original inventories to send to FICO

@testcase=57951
Scenario: Verify application is processing an operational delta calculation ticket for a original inventories.
Given I have ownership calculation data generated in the system
When inventory have an ownership ticket with last event type is an "insert or update"
And inventory has other event types without "ownership ticket"
And the inventory belongs to the segment selected in the operational delta wizard
And inventory has an operational date between the dates of the period selected in the operational delta wizard
Then verify the last event type of the inventory with ownership ticket must be included in the list of original inventories to send to FICO

@testcase=57952 
Scenario: Verify application is processing an operational delta calculation ticket for a Updated inventories
Given I have ownership calculation data generated in the system
When inventory have an ownership ticket with last event type is an "insert or update or delete"
And inventory has other event types without "ownership ticket"
And the inventory belongs to the segment selected in the operational delta wizard
And inventory has an operational date between the dates of the period selected in the operational delta wizard
Then verify the last event type of the inventory without ownership ticket must be included in the list of updated inventories to send to FICO
