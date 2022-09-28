@sharedsteps=21167 @owner=jagudelos @security @testsuite=20471 @testplan=20468 @manual
Feature: Security header
Security Headers are present in response sent from the servers

Background: Login
Given I am logged in as "admin"
@testcase=21168 
Scenario Outline: Validate the presence of security headers in response from the server
Given I am on "<Tab>" view
When I send request to the server
Then I should receive response with the following headers
| Header                    | value             |
| Content-Security-Policy   | script-src 'self' |
| X-XSS-Protection          | 1; mode=block     |
| Strict-Transport-Security | max-age=31536000; |
| X-Frame-Options           | deny              |
| X-Content-Type-Options    | nosniff           |

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
