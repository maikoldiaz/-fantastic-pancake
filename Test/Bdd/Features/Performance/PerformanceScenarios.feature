@sharedsteps=21135 @owner=jagudelos @perf @testsuite=20470 @testplan=20468 @manual
Feature: Performance Scenarios
As a user I want the application to perform as per the defined SLAs

Background: Login
Given I am Logged in as "admin"
@testcase=21136
Scenario Outline: Measure home page response time
Given I have user registered in the system
When I simulate <UserCount> concurrent users for login
Then I should see the home page with set of API call respond within <SLA> seconds

Examples:
| UserCount | SLA |
| 50        | 7   |
| 100       | 7   |
@testcase=21137
Scenario Outline: Measure File upload processing time
Given I have elements, nodes, connection
And homologation in system
When I upload file with <InvCount> inventory and <MovCount> movements for single user simulation
Then I should see the processing completes within <SLA> minutes
And resource utilization in App service and DB should be less than 30 %

Examples:
| InvCount | MovCount | SLA |
| 175      | 95       | 3   |
| 700      | 375      | 3   |
@testcase=21138
Scenario Outline: Measure response time for node creation
Given I have elements in system
When I simulate 1<UserCount> concurrent users for node creation with <AssociationCount> element associations
Then I should see the API completes within <SLA> milliseconds

Examples:
| UserCount | AssociationCount | SLA |
| 10        | 4                | 400 |
@testcase=21139
Scenario Outline: Measure response time for cut-off process
Given I have file processed with <InvCount> inventory and <MovCount> movements
When I create the cut-off process
Then I should see that each processing from client to server completes within <SLA> seconds

Examples:
| InvCount | MovCount | SLA |
| 175      | 95       | 7   |
| 700      | 375      | 7   |
@testcase=21140
Scenario Outline: Measure response time for Report view
Given I have created ticket in the system
And Sync up completes in analysis service
When I simulate <UserCount> concurrent users for viewing report of files processed with <InvCount> inventory and <MovCount> movements
Then I should see the report call respond within <SLA> seconds

Examples:
| InvCount | MovCount | UserCount | SLA |
| 175      | 95       | 50        | 7   |
| 700      | 375      | 50        | 7   |
| 175      | 95       | 100       | 7   |
| 700      | 375      | 100       | 7   |
@testcase=21141 
Scenario: Measure response size of pages
Given I am on any page
When The content of the page is fully loaded
Then I should see the response size of the pages are within 500 Kb
