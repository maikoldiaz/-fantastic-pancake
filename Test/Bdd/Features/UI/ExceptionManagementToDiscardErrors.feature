@sharedsteps=4013 @owner=jagudelos @ui @testplan=19772 @testsuite=19789
Feature: ExceptionManagementToDiscardErrors
In order to discard the errors
As an application administrator
I need an exception management UI

Background: Login
Given I am logged in as "admin"

@testcase=21222 @bvt
Scenario: Discard an exceptions group
Given I am having exceptions in Exceptions page
When I navigate to "Exceptions" page
And I select multiple exceptions
When I click on "discardException" "button"
Then I should see "Add Note" interface
When I click on "AddComment" "submit" "button"
Then I should see error message "Requerido"
When I provide value for "AddComment" "comment" "textbox" that exceeds "1000" characters
Then I should see error message "Máximo 1000 caracteres"
When I provide valid value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
Then given note should be saved for 2 exception
And discarded 2 exceptions should not be displayed in the grid

@bvt
Scenario: Discard an exception
Given I am having exceptions in Exceptions page
When I navigate to "Exceptions" page
And I get the total exceptions
Then I should see the tooltip "Discard" text for "exceptions" "discard" "link"
When I click on "pendingTransactionErrors" "discardException" "link"
Then I should see "Add Note" interface
When I click on "AddComment" "submit" "button"
Then I should see error message "Requerido"
When I provide value for "AddComment" "comment" "textbox" that exceeds "1000" characters
Then I should see error message "Máximo 1000 caracteres"
When I provide valid value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
Then given note should be saved for 1 exception
And discarded 1 exceptions should not be displayed in the grid

@testcase=21224 @bvt
Scenario: Discard an exception from the detail page
Given I am having exceptions in Exceptions page
When I navigate to "Exceptions" page
And I get the total exceptions
When I click on "pendingTransactionErrors" "detail" "link"
When I click on "discardException" "button"
Then I should see "Add Note" interface
When I click on "AddComment" "submit" "button"
Then I should see error message "Requerido"
When I provide value for "AddComment" "comment" "textbox" that exceeds "1000" characters
Then I should see error message "Máximo 1000 caracteres"
When I click on "AddComment" "cancel" "button"
Then the modal window should be closed
When I click on "discardException" "button"
When I provide valid value for "AddComment" "comment" "textbox"
And I click on "AddComment" "submit" "button"
Then given note should be saved for 1 exception
And discarded 1 exceptions should not be displayed in the grid
