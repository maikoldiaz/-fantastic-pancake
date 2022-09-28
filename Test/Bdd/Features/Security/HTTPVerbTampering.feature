@sharedsteps=21158 @owner=jagudelos @security @testsuite=20471 @testplan=20468 @manual
Feature: HTTP Verb Tampering
Validate that the controller actions are only accessible through allowed HTTP methods

Background: Login
Given I am logged in as "admin"
@testcase=21159 
Scenario Outline: Validate that the controller actions are only accessible through allowed HTTP methods
Given I am on "<Tab>" view
When I try accessing a HTTP method with any other HTTP method
| HttpMethod |
| Get        |
| Post       |
| Put        |
| Delete     |
Then I should see a message "Method not allowed"

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
