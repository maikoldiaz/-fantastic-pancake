@sharedsteps=21142 @owner=jagudelos @security @testsuite=20471 @testplan=20468 @manual
Feature: Anti-Forgery Token
POST/PUT/DELETE calls to the server should have a XSRF token which should be used to prevent CSRF attacks

Background: Login
Given I am logged in as "admin"
@testcase=21143
Scenario Outline: Validate response in the presence of valid XSRF tokens in POST/PUT/DELETE requests
Given I am on "<Tab>" view and send POST/PUT/DELETE request to the server
When I send the request with actual XSRF token
Then I should receive the appropriate response as per the action

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
@testcase=21144
Scenario Outline: Validate response in the absence of XSRF tokens in POST/PUT/DELETE requests
Given I am on "<Tab>" view and send POST/PUT/DELETE request to the server
When I send the request without XSRF token
Then I should get a response back with status code 400 and phrase "Bad request"

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
@testcase=21145 
Scenario Outline: Validate response in case of tampered XSRF tokens in POST/PUT/DELETE requests
Given I am on "<Tab>" view and send POST/PUT/DELETE request to the server
When I send the request with tampered XSRF token
Then I should get a response back with status code 400 and phrase "Bad request"

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
