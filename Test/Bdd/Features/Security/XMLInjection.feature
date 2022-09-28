@sharedsteps=21173 @owner=jagudelos @security @testsuite=20471 @testplan=20468 @manual
Feature: XML Injection
Verify that the application is not vulnerable to XML injection

Background: Login
Given I am logged in as "admin"
@testcase=21174 
Scenario Outline: Validate that the application is not vulnerable to XML Injection
Given I am on "<Tab>" view and sending input using application/XML format
When I upload a malicious XML content
Then the application should read the XML safely

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
