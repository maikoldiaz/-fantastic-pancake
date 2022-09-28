@owner=jagudelos @backend @database @testplan=19772 @testsuite=19783
Feature: CreateAndPublishAnalyticalModels
As a TRUE system, I need to create and publish
the analytical models of ownership calculation for
points of transfer


@testcase=21418 @bvt
Scenario: Verify if the Historical operational movements data are loaded into the blob
Given I want to create and publish analytical models of property calculation for transfer points
When I want to review the historical files of operational movements
Then I verify if "OperativeMovements" historical data is loaded onto the storage blob

@testcase=21419 @bvt
Scenario: Verify if the Historical operational movements with ownership data is loaded into the blob
Given I want to create and publish analytical models of property calculation for transfer points
When I want to review the historical files of operational movements
Then I verify if "OperativeMovementswithOwnership" historical data is loaded onto the storage blob

@testcase=21420 @bvt
Scenario: Verify if the Operative node relationship file is loaded into the blob
Given I want to create and publish analytical models of property calculation for transfer points
When I want to review the historical files of operational movements
Then I verify if "OperativeNodeRelationship" historical data is loaded onto the storage blob

@testcase=21421 @bvt
Scenario: Verify if the operative node relationship with owenership file is loaded into the blob
Given I want to create and publish analytical models of property calculation for transfer points
When I want to review the historical files of operational movements
Then I verify if "OperativeNodeRelationshipWithOwnership" historical data is loaded onto the storage blob

@testcase=21422 @bvt
Scenario Outline: Verify if the data loaded is present in its respective tables
Given I have historical data to create analytical models
When I upload the "<table>" into the blob
Then I verify if data is present in the "<table>"
Examples: 
| data                                 | table                                  |
| Operational Movements                | OperativeMovements                     |
| Operational Movements with ownership | OperativeMovementsWithOwnership        |
| Operational Nodes                    | OperativeNodeRelationship              |
| Operational Nodes with Ownership     | OperativeNodeRelationshipWithOwnership |

@testcase=21423 @bvt
Scenario Outline: Verify if the table for data is present
Given I have historical data to create analytical models
When I want to load data for creation and publishing of analytical model I need sql table to store the data
Then I verify if the "<tables>" are present
Examples:
| tables                                 |
| OperativeMovements                     |
| OperativeMovementswithOwnership        |
| OperativeNodeRelationship              |
| OperativeNodeRelationshipwithOwnership |

@testcase=21424
Scenario: Verify the values stored in the destination table for Operational Movements
Given I have tables to store in the database
When I upload a csv file to the "OperativeMovements" container
And I initiate the data load process
Then I verify if the data that is loaded is stored into appdb data tables
And I also verify if the data is a direct copy of data that is loaded from csv

@testcase=21425
Scenario: Verify the values stored in the destination table for Operational Movements with Ownership
Given I have tables to store in the database
When I upload a csv file to the "OperativeMovementswithOwnership" container
And I initiate the data load process
Then I verify if the data that is loaded is stored into appdb data tables
And I also verify if the data is a direct copy of data that is loaded from csv


@testcase=21426
Scenario: Verify the values stored in the destination table for Operative Node Relationship
Given I have tables to store in the database
When I upload a csv file to the "OperativeNodeRelationship" container
And I initiate the data load process
Then I verify if the data that is loaded is stored into appdb data tables
And I also verify if the data is a direct copy of data that is loaded from csv


@testcase=21427 
Scenario: Verify the values stored in the destination table for Operative Node Relationship with Ownership
Given I have tables to store in the database
When I upload a csv file to the "OperativeNodeRelationshipwithOwnership" container
And I initiate the data load process
Then I verify if the data that is loaded is stored into appdb data tables
And I also verify if the data is a direct copy of data that is loaded from csv


