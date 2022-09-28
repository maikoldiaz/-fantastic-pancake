@sharedsteps=21151 @owner=jagudelos @security @testsuite=20471 @testplan=20468 @manual
Feature: Cross Site Scripting
Validating that the application is not vulnerable to XSS attacks

Background: Login
Given I am logged in as "admin"
@testcase=21152 
Scenario Outline: Validate that the application is not vulnerable to cross site scripting
Given I am on "<Tab>" view
When I upload any script using input to the application
Then the application should output encode the provided script before rendering

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
