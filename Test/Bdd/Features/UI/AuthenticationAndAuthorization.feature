@owner=jagudelos @ui @testplan=11317 @testsuite=11325
Feature: AuthenticationAndAuthorization
In order to handle the application for different users
I want to implement Authorization for ui

@testcase=12012 @bvt @version=2
Scenario: Verify Admin User Authorization
	Given I am logged in as "administrador"
	When I click on Administration tab
	Then I should see "Category" tab
	And I should see "Category elements" tab
	And I should see "Configure group nodes" tab
	And I should see "Configure attributes nodes" tab
	And I should see "Configure connections attributes" tab
	And I should see "Nodes" tab
	When I click on conveyor balance with property menu
	Then I should see "FileUpload" tab
	And I should see "Operational Cutoff" tab

@testcase=12013 @version=2
Scenario: Verify Professional Segment Balances User Authorization
	Given I am logged in as "profesional"
	When I click on Administration tab
	Then I should not see "Category" tab
	And I should not see "Category elements" tab
	And I should not see "Configure group nodes" tab
	And I should not see "Configure attributes Nodes" tab
	And I should not see "Configure connections attributes" tab
	And I should see "Nodes" tab
	When I click on conveyor balance with property menu
	Then I should see "FileUpload" tab
	And I should see "Operational Cutoff" tab

@testcase=12014
Scenario: Verify Professional Segment Balances User Authorization when directly accessing the unauthorized pages URL
	Given I am logged in as "profesional"
	When I hit the "Category" URL directly
	Then I should see an unauthorized page
	When I hit the "Category elements" URL directly
	Then I should see an unauthorized page
	When I hit the "Configure group nodes" URL directly
	Then I should see an unauthorized page
	When I hit the "Configure attributes nodes" URL directly
	Then I should see an unauthorized page
	When I hit the "Configure connections attributes" URL directly
	Then I should see an unauthorized page

@testcase=12015 @version=2
Scenario Outline: Verify Other Users Authorization
	Given I am logged in as "<User>"
	Then I should not see "Administration" tab
	Then I should not see "Category" tab
	And I should not see "Category elements" tab
	And I should not see "Configure group nodes" tab
	And I should not see "Configure attributes nodes" tab
	And I should not see "Configure connections attributes" tab
	And I should not see "Nodes" tab
	And I should not see "Conveyor balance with property" tab
	And I should not see "FileUpload" tab
	And I should not see "Operational Cutoff" tab

	Examples:
		| User        |
		| aprobador   |
		| programador |
		| consulta    |

@testcase=12016 @version=2
Scenario Outline: Verify Other Users Authorization when directly accessing the unauthorized pages URL
	Given I am logged in as "<User>"
	When I hit the "Category" URL directly
	Then I should see an unauthorized page
	When I hit the "Category elements" URL directly
	Then I should see an unauthorized page
	When I hit the "Configure group nodes" URL directly
	Then I should see an unauthorized page
	When I hit the "Configure attributes nodes" URL directly
	Then I should see an unauthorized page
	When I hit the "Configure connections attributes" URL directly
	Then I should see an unauthorized page
	When I hit the "Nodes" URL directly
	Then I should see an unauthorized page
	When I hit the "FileUpload" URL directly
	Then I should see an unauthorized page
	When I hit the "Operational Cutoff" URL directly
	Then I should see an unauthorized page

Examples:
| User        |
| aprobador   |
| programador |
| consulta    |