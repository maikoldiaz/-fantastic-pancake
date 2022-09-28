@sharedsteps=21171 @owner=jagudelos @security @testsuite=20471 @testplan=20468 @manual
Feature: Web server Fingerprinting
Headers disclosing information about server are not present in response sent from the servers

Background: Login
Given I am logged in as "admin"
@testcase=21172 
Scenario Outline: Validate the headers disclosing information about the server are removed in response from the server
Given I am on "<Tab>" view
When I send request to the server
Then I should receive response with the following headers removed
| Header       |
| Server       |
| X-Powered-By |

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
