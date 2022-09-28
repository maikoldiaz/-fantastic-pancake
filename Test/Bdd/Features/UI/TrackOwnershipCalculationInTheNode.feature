@sharedsteps=16581 @owner=jagudelos @ui @testplan=14709 @testsuite=14718
Feature: TrackOwnershipCalculationInTheNode
In order to perform the Ownership Calculation
As a Balance Segment Professional User
I want a UI to track the ownership calculation for Nodes

Background: Login
Given I am logged in as "profesional"

@testcase=16592 @bvt @version=2
Scenario: Verify ownership calculation process was executed successfully or with errors when there are records in the last 40 days
Given I have Ownership Calculation for Nodes information from the last 40 days
When I navigate to "Volumetric Balance with ownership for node" page
Then I should see the information of executed "Ownership Calculation for the Nodes" in the grid

@testcase=21316 @manual
Scenario: Verify ownership calculation process was executed successfully or with errors when there are no records in the last 40 days
Given I have no information from the last 40 days
When I navigate to "Volumetric Balance with ownership for node" page
Then I should see error message "Sin registros"

@testcase=16593 @version=2
Scenario: Verify the functionality when the record is in Submitted state
When I navigate to "Volumetric Balance with ownership for node" page
And I have a record with "Submitted" state and I searched in "Ownership Calculation for the Nodes" Grid
Then verify that "OwnershipNodes" "ViewError" "link" is "disabled"
And verify that "OwnershipNodes" "ViewReport" "link" is "disabled"
And verify that "OwnershipNodes" "EditOwnership" "link" is "disabled"

@testcase=16594 @version=2
Scenario: Verify the functionality when the record is in Completed with error state
When I navigate to "Volumetric Balance with ownership for node" page
And I have a record with "Error" state and I searched in "Ownership Calculation for the Nodes" Grid
Then verify that "OwnershipNodes" "ViewError" "link" is "enabled"
And verify that "OwnershipNodes" "ViewReport" "link" is "disabled"
And verify that "OwnershipNodes" "EditOwnership" "link" is "disabled"

@testcase=16595 @bvt @version=2
Scenario: Verify the functionality when the record is in Completed state
When I navigate to "Volumetric Balance with ownership for node" page
And I have a record with "Completed" state and I searched in "Ownership Calculation for the Nodes" Grid
Then verify that "OwnershipNodes" "ViewError" "link" is "disabled"
And verify that "OwnershipNodes" "ViewReport" "link" is "enabled"
And verify that "OwnershipNodes" "EditOwnership" "link" is "enabled"

@testcase=16596 @version=2
Scenario: Verify the functionality for records of previous periods
Given I have Ownership Calculation for Nodes information from the last 40 days
When I navigate to "Volumetric Balance with ownership" page
And I have a top record with "Completed" state and I searched in "Ownership Calculation for the Segments" Grid
And I click on "error details" "icon"
Then I see "Balance per Node" header
When I have a record with "Completed" state and I searched in "Ownership Calculation for the Nodes" Grid
Then verify that "OwnershipNodes" "ViewError" "link" is "disabled"
And verify that "OwnershipNodes" "ViewReport" "link" is "enabled"
And verify that "OwnershipNodes" "EditOwnership" "link" is "enabled"
When I click on "to return" "link"
Then I see "Volumetric Balance with ownership" header
When I have a record of previous period in "Completed" state and I searched for it
And I click on "tickets" "viewSummary" "link"
Then I see "Balance per Node" header
When I have a record with "Completed" state and I searched in "Ownership Calculation for the Nodes" Grid
Then verify that "OwnershipNodes" "ViewError" "link" is "disabled"
And verify that "OwnershipNodes" "ViewReport" "link" is "enabled"
And verify that "OwnershipNodes" "EditOwnership" "link" is "disabled"

@testcase=16597 @version=2
Scenario Outline: Verify Filters functionality
Given I have Ownership Calculation for Nodes information from the last 40 days
When I navigate to "Volumetric Balance with ownership for node" page
And I provide the value for "OwnershipNodes" "<Field>" "<ControlType>" filter in "Ownership Calculation for the Nodes" Grid
Then I should see the information that matches the data entered for the "<Field>" in "Ownership Calculation for the Nodes" Grid

Examples:
| Field                | ControlType |
| TicketId             | textbox     |
| StartDate            | date        |
| EndDate              | date        |
| CreatedDate          | date        |
| CreatedBy            | textbox     |
| node_name            | textbox     |
| categoryElement_name | textbox     |
| Status               | combobox    |

@testcase=16598 @version=2
Scenario Outline: Verify Sorting functionality
Given I have Ownership Calculation for Nodes information from the last 40 days
When I navigate to "Volumetric Balance with ownership for node" page
And I click on the "<ColumnName>"
Then the results should be sorted based on "<ColumnName>" in "Ownership Calculation for the Nodes" Grid

Examples:
| ColumnName    |
| Ticket        |
| StartDate     |
| EndDate       |
| ExecutionDate |
| Username      |
| Node          |
| Segment       |
| Status        |

@testcase=16599 @version=2
Scenario: Verify Pagination functionality
Given I have Ownership Calculation for Nodes information from the last 40 days
When I navigate to "Volumetric Balance with ownership for node" page
And I navigate to second page in "Ownership Calculation for the Nodes" Grid
Then the records should be displayed accordingly in "Ownership Calculation for the Nodes" Grid
When I change the elements count per page to 50
Then the records count in "Ownership Calculation for the Nodes" Grid shown per page should also be 50
@testcase=21317 
Scenario: Verify the headers of Volumetric Balance with ownership for node page
When I navigate to "Volumetric Balance with ownership for node" page
Then I see "Volumetric Balance by Node" header