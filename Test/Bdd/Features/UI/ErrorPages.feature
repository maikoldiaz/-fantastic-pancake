@owner=jagudelos @testplan=14709 @testsuite=14713 @ui
Feature: ErrorPages
As a user of TRUE,
I need to see a message that lets me know when pages are not available,
so I can properly be informed of any errors.

@testcase=16645 @ui
Scenario Outline: Verify TRUE user is able to see 403 Error Page if the user doesn't have access to resources
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
		| User      |
		| aprobador |

@testcase=16646 @ui
Scenario Outline: Verify TRUE user is able to see 401 Error page if the users credentials invalid
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
		| User  |
		| admin |

@testcase=16647 @ui
Scenario Outline: Verify TRUE User is able to see 404 Error page if the user navigates to a non-existent page
	Given I am logged in as "admin"
	When I hit the "<RandomPage>" URL directly
	Then I should see an Page Not Found error

	Examples:
		| RandomPage |
		| createuser |
		| node       |

@testcase=16648 @ui
Scenario: Verify TRUE user is able to see the 500 Error page If an error occurs
	Given I am logged in as "admin"
	When I hit the "InternalServerError" URL directly
	Then I should see Internal Error Page

@testcase=16649 @ui
Scenario: Verify TRUE user is able to see the ghost refresh page if a 500 Error occurs
	Given I am logged in as "admin"
	When I hit the "InternalServerError" URL directly
	Then I should see Internal Error Page
	And I should be able to click refresh

@testcase=16650 @ui
Scenario: Verify TRUE user is able to see the Contact EcoPetrol Support line if a 500 Error persists
	Given I am logged in as "admin"
	When I hit the "InternalServerError" URL directly
	Then I should see Internal Error Page
	And I should be able to click refresh
	And I should be able to See the support link