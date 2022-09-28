@sharedsteps=21155 @owner=jagudelos @security @testsuite=20471 @testplan=20468 @manual
Feature: File upload validations
Verify the File uploaded are not malicious

Background: Login
Given I am logged in as "admin"
@testcase=21156
Scenario Outline: Check the extension and size of the file uploaded within the white-listed extension types
Given I am on "<Tab>" view
When I upload a file with an extension within the white listed extension types
Then I should be allowed to upload the file

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
@testcase=21157 
Scenario Outline: Check the extension and size of the file uploaded outside the white-listed extension types
Given I am on "<Tab>" view
When I upload a file with an extension outside the white listed extension types
Then I should not be allowed to upload the file

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
