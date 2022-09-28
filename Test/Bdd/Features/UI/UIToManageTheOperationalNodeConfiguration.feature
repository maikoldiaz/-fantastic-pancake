@sharedsteps=7539 @testplan=26817 @testsuite=28148 @owner=jagudelos
Feature: UIToManageTheOperationalNodeConfiguration
As an Administrator user, I require an UI
to manage the operational node configuration
of the analytical model.

Background: Login
Given I am logged in as "admin"

@testcase=29589 @version=1 @ui @manual
Scenario: Verify that TRUE administrator require to manage elements in the category
When I navigate to "TransferPointsOperationalnodes" page
And I click on " CreateRelationship" button
Then I should see "CreateRelationship" Page
And I should see and select any of active Category from "TransferPoints" dropdown

@testcase=29590 @version=1 @ui
Scenario: Verify that TRUE administrator require to configure Valid settings only for TRUE transfer points
When I navigate to "TransferPointsOperationalnodes" page
And I click on " CreateRelationship" button
Then I should see "CreateRelationship" Page
When I should select any Category from "TransferPoints" dropdown
And I should select Movement Type from "MovementType" dropdown
And I should see Source Node from "SourceNode" dropdown contains list of source nodes that belong to a connection marked as a transfer point
And I should see Destination Node from "DestinationNode" dropdown List of destination nodes for connections whose source is the value selected in Source Node and are marked as transfer points

@testcase=29591 @version=1 @ui @manual
Scenario: Verify that TRUE administrator able to create transfer relationship with valid details
When I navigate to "TransferPointsOperationalnodes" page
And I click on " CreateRelationship" button
Then I should see "CreateRelationship" Page
When I should select any Category from "TransferPoints" dropdown
And I should select Movement Type from "MovementType" dropdown
And I should select Source Node from "SourceNode" dropdown
And I provide value for  Source Node Type field
And I should select Destination Node from "DestinationNode" dropdown
And I provide value for  Destination Node Type field
And I should select Source Product from "SourceProduct" dropdown
And I should select Product Type Origin from "ProductTypeOrigin" dropdown
And I provide value for  Camp field
And I provide value for  Water camp field
And I provide value for  Correlated Cases field
When I click on " Save" button
Then I should see created Transfer relation information in grid

@testcase=29592 @version=1 @ui @manual
Scenario Outline: Verify that TRUE administrator able to create transfer relationship with values exceeding maximum limit
When I navigate to "TransferPointsOperationalnodes" page
And I click on " CreateRelationship" button
Then I should see "CreateRelationship" Page
When I should select any Category from "TransferPoints" dropdown
And I should select Movement Type from "MovementType" dropdown
And I should select Source Node from "SourceNode" dropdown
And I provide value for  Source Node Type field
And I should select Destination Node from "DestinationNode" dropdown
And I provide value for  Destination Node Type field
And I should select Source Product from "SourceProduct" dropdown
And I should select Product Type Origin from "ProductTypeOrigin" dropdown
And I provide value for  Camp field
And I provide value for  Water camp field
And I provide value for  Correlated Cases field
When I provide value for "<FieldName>" that exceeds "<Limit>" characters
Then I should see error message "<Message>"

Examples:
| FieldName        | Limit | Message                                           |
| Camp             | 200   | The field can contain a maximum of 200 characters |
| Water camp       | 200   | The field can contain a maximum of 200 characters |
| Correlated Cases | 200   | The field can contain a maximum of 200 characters |

@testcase=29593 @version=1 @ui @manual
Scenario: Verify that TRUE administrator able to view transfer relationships records in grid with required information
When I navigate to "TransferPointsOperationalnodes" page
And I should create "TransferRelationship"
Then I should see "CreateRelationship" Page
And I should see fields Transfer Point,Type of Movement,Source Node,Source Node Type,Destination Node,Destination Node Type,Source Product,Source Product Type in grid
And I should see Edit Icon for all rows with always enabled state
And I should see Delete Icon for all rows with always enabled state

@testcase=29594 @version=1 @ui @manual
Scenario: Verify that TRUE administrator able to create a transfer relationships with existing details
When I navigate to "TransferPointsOperationalnodes" page
And I should create "TransferRelationship"
And I click on "CreateRelationship" button
When I Provided Existing details for "TransferRelationship"
And  I click on "Save" button
Then I should see error message "A transfer relationship with the values entered already exists"

@testcase=29595 @version=2 @ui @manual
Scenario: Verify that TRUE administrator grid information if There are No transfer relationship configuration rows
When I navigate to "TransferPointsOperationalnodes" page
Then I should see the Message "Sin registros" if there are No transfer relationship configuration available

@testcase=29596 @version=1 @ui @manual
Scenario: Verify that TRUE administrator can Edit a transfer relationship
When I navigate to "TransferPointsOperationalnodes" page
And I should create "TransferRelationship"
Then I should see  created transfer relationship in grid
When I click on "EditActionIcon" button to eidt transfer relationship
Then I should see "updateTransferPointOperational" modal
And I should see All fields are disabled except camp,Water camp,Correlated Cases fields
And I provide updated values to camp,Water camp,Correlated Cases fields without exceeding maximum limit of text
When I click on "Save" button
Then I should see transfer relationship information updated and to be displayed in the grid with updated details.

@testcase=29597 @version=1 @ui @manual
Scenario Outline: Verify that TRUE administrator can Edit a transfer relationship with values exceeding maximum limit
When I navigate to "TransferPointsOperationalnodes" page
And I should create "TransferRelationship"
Then I should see  created transfer relationship in grid
When I click on "EditActionIcon" button to eidt transfer relationship
Then I should see "updateTransferPointOperational" modal
And I should see All fields are disabled except camp,Water camp,Correlated Cases fields
When I provide value for "<FieldName>" that exceeds "<Limit>" characters
Then I should see error message "<Message>"

Examples:
| FieldName        | Limit | Message                                           |
| Camp             | 200   | The field can contain a maximum of 200 characters |
| Water camp       | 200   | The field can contain a maximum of 200 characters |
| Correlated Cases | 200   | The field can contain a maximum of 200 characters |

@testcase=29598 @version=1 @ui @manual
Scenario: Verify that TRUE administrator can Delete a transfer relationship
When I navigate to "TransferPointsOperationalnodes" page
And I should create "TransferRelationship"
Then I should see  created transfer relationship in grid
When I click on "DeleteActionIcon" button to Delete transfer relationship
Then I should see "DeleteTransferPointOperational" modal
And I should see All fields are disabled except Notes field
And I provide Value to "Notes" field
When I click on "Accept" button
Then I should see transfer relationship information deleted and not displayed in the grid.

@testcase=29599 @version=1 @ui @manual
Scenario: Verify that TRUE administrator can Delete a transfer relationship with values exceeding maximum limit
When I navigate to "TransferPointsOperationalnodes" page
And I should create "TransferRelationship"
Then I should see  created transfer relationship in grid
When I click on "DeleteActionIcon" button to Delete transfer relationship
Then I should see "DeleteTransferPointOperational" modal
And I should see All fields are disabled except Notes field
When I provide value for "Notes" that exceeds "1000" characters
Then I should see error message "The field can contain a maximum of 1000 characters"
When I click on "Accept" button
And I should see transfer relationship information not deleted