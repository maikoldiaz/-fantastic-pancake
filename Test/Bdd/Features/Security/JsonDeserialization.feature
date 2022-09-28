@sharedsteps=21160 @owner=jagudelos @security @testsuite=20471 @testplan=20468 @manual
Feature: JSON deserialization
User should not be able to change the object to which JSON data gets deserialized

Background: Login
Given I am logged in as "admin"
@testcase=21161 
Scenario Outline: Validate that JSON data is deserialized to specific objects only
Given I am on "<Tab>" view
When I send HTTP request in JSON format
Then I should not be able to specify the object into which it will be deserialized

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
