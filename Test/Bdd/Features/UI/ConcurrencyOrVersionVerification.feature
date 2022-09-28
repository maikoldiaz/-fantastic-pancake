@owner=jagudelos @ui @testplan=35673 @testsuite=35694
Feature: ConcurrencyOrVersionVerification
Checking version of an entity when two users
are working on the same entity

@testcase=37316
Scenario Outline: Verify error message when two users are working on the same entity
Given "User1" is logged in as "admin"
And "User1" have "<Entity>" in the system
When "User1" navigated to "<Pagename>" page
And "User1" clicked on edit "<Entity>" link
And "User1" see edit "<Entity>" interface
And "User2" is logged in as "admin" in different browser
And "User2" have "<Entity>" in the system
And "User2" navigated to "<Pagename>" page
And "User2" clicked on same edit "<Entity>" link
And "User2" see edit "<Entity>" interface
And "User1" edited and saved "<Entity>" information
And "User2" edited and saved "<Entity>" information
Then "User2" receives an error message

Examples:
| Entity                     | Pagename                       |
| Category                   | Categories                     |
| Category Element           | CategoryElement                |
| Node                       | Node                           |
| Node Attributes            | ConfigureAttributesNodes       |
| Group Node                 | ConfigureGroupNodes            |
| Node Connection Attributes | ConfigureAttributesConnections |
| Homologation               | Homologation                   |
| Transfer Points            | TransferPointsOperationalNodes |
| Movement Transformation    | TransformationSettings         |

@testcase=37317 
Scenario: Verify error message when two users are working on the same exception
Given "User1" is logged in as "admin"
And "User1" have "Exceptions" in the system
When "User1" navigated to “Exceptions” page
And "User2" is logged in as "admin" in different browser
And "User2" have "Exceptions" in the system
And "User2" navigated to “Exceptions” page
And "User1" deleted or retried an exception
And "User2" deleted or retried same exception
Then "User2" receives an error message
