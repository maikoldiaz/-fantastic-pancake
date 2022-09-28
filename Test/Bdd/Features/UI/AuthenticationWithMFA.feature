@owner=jagudelos @ui @testsuite=15267 @testplan=14709
Feature: MultifactorAuthentication
As a user with MFA Account
I want to Login to System

@testcase=16656 @ui @manual
Scenario: Validate the Multifactor authentication for Application
	Given I have User configured for Multifactor authentication
	When I navigate to "Login" page
	Then I should be asked for Authentication Code