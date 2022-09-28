@sharedsteps=21169 @owner=jagudelos @security @testsuite=20471 @testplan=20468 @manual
Feature: SQL injection
validating that the application is not vulnerable to SQL injection

Background: Login
Given I am logged in as "admin"
@testcase=21170 
Scenario Outline: Validate if the application doesn't have any SQL injection
Given I am on "<Tab>" view
And performing an action which further runs an SQL query in the backend
When I provide invalid parameters for the actions
Then the backend query should run only after validating the input

Examples:
| Tab                              |
| Category                         |
| Category elements                |
| Configure group nodes            |
| Configure attributes nodes       |
| Configure connections attributes |
| Nodes                            |
| Conveyor balance with property   |
| FileUpload                       |
| Operational Cutoff               |
