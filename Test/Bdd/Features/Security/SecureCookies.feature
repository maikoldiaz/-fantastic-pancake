@sharedsteps=21165 @owner=jagudelos @security @testsuite=20471 @testplan=20468 @manual
Feature: Secure Cookies
Ensure that the cookies set by the application have the recommended properties set

Background: Login
Given I am logged in as "admin"
@testcase=21166 
Scenario Outline: Check the properties of the cookies set by the application
Given I am on "<Tab>" view
When I look at the properties of the Cookies set by the application
Then I should see the following
| key      | value |
| HttpOnly | true  |
| Secure   | true  |

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
