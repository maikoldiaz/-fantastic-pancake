@sharedsteps=16581 @owner=jagudelos @ui @testplan=31102 @testsuite=31111
Feature: ValidationOfExceptionManagementGrid
As TRUE System Administrator,
I need the exception management UI grid to
display the failed messages instead of the errors

Background: Login
Given I am logged in as "admin"

@testcase=32808 @version = 1, @testcase =
Scenario: Validate messages registered in last 40 days
When I navigate to "Exceptions" page
Then I should see errors registered for last 40 days in the grid

@testcase=32809 @version = 1, @testcase =
Scenario: Validate Messages originated by loading Excel files
Given I have failed messages generated through excel files
When I navigate to "Exceptions" page
Then I should see "file name" in "file" column

@testcase=32810 @version = 1, @testcase =
Scenario: Validate Messages originated by SINOPER system
Given I have failed messages by Sinoper
When I navigate to "Exceptions" page
Then I should not see any data in "file" column

@testcase=32811 @version = 1, @testcase =
Scenario: Validate error details of message
When I navigate to "Exceptions" page
And I click on details of message
Then I should not see error message in error section

@testcase=32812 @version = 1, @testcase =
Scenario: Validate return to exception management page
Given I set filters to view error messages and navigate to exception management page
When I click on "return to list" option
Then I should see all the filters set by user

@testcase=32813 @version = 1, @testcase =
Scenario: Validate exception grid data
When I navigate to "Exceptions" page
Then I should see all the required columns in grid
