@owner=jagudelos @security @testsuite=20471 @testplan=20468 @manual
Feature: MultifactorAuthentication
As a user with MFA Account I should be asked for the MFA auth-code when I Login to System
@testcase=21164 
Scenario: Validate the Multi-factor authentication for Application
Given The user is configured for Multi-factor authentication
When I navigate to "Login" page
Then I should be asked for Authentication Code
