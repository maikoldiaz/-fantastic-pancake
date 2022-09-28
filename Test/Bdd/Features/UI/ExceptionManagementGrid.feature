@sharedsteps=16581 @owner=jagudelos @ui @testplan=31102 @testsuite=31111 @parallel=false
Feature: ExceptionManagementGrid 
As TRUE System Administrator, 
I need the exception management UI grid to 
display the failed messages instead of the errors

Background: Login
Given I am logged in as "admin"

@testcase=32759, @version=2 @bvt1.5
Scenario: Validate messages registered in last 40 days
	And I have failed messages generated through excel files
	When I navigate to "Exceptions" page
	Then I should see the executed Exceptions in the grid

@testcase=32760, @version=2 @bvt1.5
Scenario: Validate messages originated by loading Excel files
	And I have failed messages generated through excel files 
	When I navigate to "Exceptions" page 
	Then I should see the ExcelFileName in the grid

@testcase=32761, @version=2
Scenario: Validate messages originated by SINOPER system
	And I have failed messages by Sinoper 
	When I navigate to "Exceptions" page 
	Then I should not see any data in file column

@testcase=32762, @version=2 @bvt1.5
Scenario: Validate error details of message
	And I have failed messages generated through excel files
	When I navigate to "Exceptions" page 
	And I click on "pendingTransactionErrors" "detail" "link"
	Then I should see error message in error section

@testcase=32763, @version=2 @bvt1.5
Scenario: Validate return to exception management page
	And I have failed messages generated through excel files
	When I navigate to "Exceptions" page
	And I filter on "Undefined" "MessageId" "textbox" with message id
	And I click on "pendingTransactionErrors" "detail" "link"
	And I click on "return" "button"
	Then I should see the exceptions that matches the filter

@testcase=32764, @version=2  @bvt1.5
Scenario: Validate exception grid data
	When I navigate to "Exceptions" page 
	Then I should see the "Columns" on the page
| Columns           |
| Mensaje           |
| Sistema origen    |
| Proceso           |
| Archivo           |
| Fecha de creación |