@sharedsteps=21162 @owner=jagudelos @security @testsuite=20471 @testplan=20468 @manual
Feature: Logout
Logout functionality should terminate the associated session

Background: Login
Given I am logged in as "admin"
@testcase=21163 
Scenario Outline: Validate that logout functionality terminates the associated session
Given I have logged out of the application
When I try to access <Tab>" view
Then I should receive a 404 unauthorized error
And I should be redirected to sign-in page

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
