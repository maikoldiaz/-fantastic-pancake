@sharedsteps=21153 @owner=jagudelos @security @testsuite=20471 @testplan=20468 @manual
Feature: Error Handling
Exceptions thrown should not disclose debugging or stack-trace information

Background: Login
Given I am logged in as "admin"
@testcase=21154 
Scenario Outline: Validate that all the exceptions are handled properly
Given I am on "<Tab>" view
When I perform any action that causes the application to throw any exception
Then I should not see the stack-trace of the exception

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
