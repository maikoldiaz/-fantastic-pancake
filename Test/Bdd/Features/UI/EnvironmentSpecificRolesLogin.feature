@owner=jagudelos @ui @testplan=39221 @testsuite=39236 @parallel=false
Feature: EnvironmentSpecificRolesLogin
As a System Architect
I require that the roles be segregated by environment

@testcase=41401 @bvt @bvt1.5
Scenario: Verify login functionality with that particular environment specific role
Given I have any environment specific role defined
When I try to login with that defined role
Then the login should be successful

@testcase=41402 @bvt @bvt1.5
Scenario: Verify login functionality with other environment specific role
Given I have other environment specific role defined
When I try to login with other environment specific role
Then the login should not be successful
