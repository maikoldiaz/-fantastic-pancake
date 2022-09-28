@sharedsteps=7539 @owner=jagudelos @ui @S14 @MVP2and3 @testsuite=55121 @testplan=55104
Feature: EditRelationshipMovementsTypes
In order to handle the Relationship Movements
As an application administrator
I want to Edit Relationship Movements Types

Background: Login
Given I am logged in as "admin"
@testcase=57694
Scenario: Verify data populated in Relationship Movements Types interface when click on Edit relationship button
When I navigate to "Relationship Movements Types" page
And I search for existing Movement type record using filter option
And I click on "annulations" "edit" "link" of any record
Then I should see "Edit relationship of movement types" interface
And Each list must show selected the element corresponding to the relationship that is being edited
And Active control should show the corresponding status to the record being edited
And validate that "source" "movement" "dropdown" is "disabled"
And "Cancellation type" drop down should be loaded with active movement types and not selected item in the movement type list
And I remove values from below fields to check all options present in the dropdown
And "Source" drop down should be loaded with their corresponding values
And "Destination" drop down should be loaded with their corresponding values
And "Source product" drop down should be loaded with their corresponding values
And "Destination product" drop down should be loaded with their corresponding values
@testcase=57695
Scenario: Verify filter information in lists
When I navigate to "Relationship Movements Types" page
And I click on "annulations" "edit" "link" of any record
Then I verify filter the content of the list "Cancellation type"
And I verify filter the content of the list "Source"
And I verify filter the content of the list "Destination"
And I verify filter the content of the list "Source product"
And I verify filter the content of the list "Destination product"

@testcase=57696 @bvt @BVT2
Scenario Outline: Update Relationship Movements Types without mandatory fields
When I navigate to "Relationship Movements Types" page
And I click on "annulations" "edit" "link" of any record
Then I should see "Edit relationship of movement types" interface
When I remove value from "<field>" dropdown
And I click on "annulation" "submit" "button"
And I should see error message "Requerido"

Examples:
| field               |
| Cancellation type   |
| Source              |
| Destination         |
| Source product      |
| Destination product |
@testcase=57697
Scenario: Update Relationship Movements Types when select already exists cancellation type
When I navigate to "Relationship Movements Types" page
And I click on "annulations" "edit" "link" of any record
And I select "existing cancellation type" from "Cancellation type" list
And I click on "annulation" "submit" "button"
Then I should see message as "El tipo de anulación seleccionado ya se encuentra asignado al tipo de movimiento" for "Movements type" assigned
@testcase=57698
Scenario: Verify Destination list not displayed None item when None item selected in the source list
When I navigate to "Relationship Movements Types" page
And I click on "annulations" "edit" "link" of any record
And I remove value from "Destination" dropdown
And I select "none" from "Source" list
Then "Destination" list should not contain "none" option
@testcase=57699
Scenario Outline: Verify Destination list displayed all options when user selects other item excepts None in the source list
When I navigate to "Relationship Movements Types" page
And I click on "annulations" "edit" "link" of any record
And I select "<Item>" from "Source" list
Then "Destination" list should contain all options

Examples:
| Item        |
| source      |
| destination |
@testcase=57700
Scenario: Verify Source list not displayed None item when None item selected in the Destination list
When I navigate to "Relationship Movements Types" page
And I click on "annulations" "edit" "link" of any record
And I remove value from "Source" dropdown
And I select "none" from "Destination" list
Then "Source" list should not contain "none" option
@testcase=57701
Scenario Outline: Verify Source list displayed all options when user selects other item excepts None in the Destination list
When I navigate to "Relationship Movements Types" page
And I click on "annulations" "edit" "link" of any record
And I select "<Item>" from "Destination" list
Then "Source" list should contain all options

Examples:
| Item        |
| source      |
| destination |
@testcase=57702
Scenario: Verify Destination product list not displayed None item when None item selected in the Source product list
When I navigate to "Relationship Movements Types" page
And I click on "annulations" "edit" "link" of any record
And I remove value from "Destination product" dropdown
And I select "none" from "Source product" list
Then "Destination product" list should not contain "none" option
@testcase=57703
Scenario Outline: Verify Destination product list displayed all options when user selects other item excepts None in the Source product list
When I navigate to "Relationship Movements Types" page
And I click on "annulations" "edit" "link" of any record
And I select "<Item>" from "Source product" list
Then "Destination product" list should contain all options

Examples:
| Item        |
| source      |
| destination |
@testcase=57704
Scenario: Verify Source product list not displayed None item when None item selected in the Destination product list
When I navigate to "Relationship Movements Types" page
And I click on "annulations" "edit" "link" of any record
And I remove value from "Source product" dropdown
And I select "none" from "Destination product" list
Then "Source product" list should not contain "None" option
@testcase=57705
Scenario Outline: Verify Source product list displayed all options when user selects other item excepts None in the Destination product list
When I navigate to "Relationship Movements Types" page
And I click on "annulations" "edit" "link" of any record
And I select "<Item>" from "Destination product" list
Then "Source product" list should contain all options

Examples:
| Item        |
| source      |
| destination |

@testcase=57706 @bvt @BVT2
Scenario: Update Relationship Movements Types with valid data
When I navigate to "Relationship Movements Types" page
And I click on "annulations" "edit" "link" of any record
And I select valid cancellation type from Cancellation type list
And the cancellation type has not been assigned to another movement type
And I click on "annulation" "submit" "button"
Then I should see breadcrumb "Relación tipos de movimientos"
And I should see the updated data of the relationship in the audit log
@testcase=57707 
Scenario: Verify Cancel functionality for edit Relationship Movements Types Interface
When I navigate to "Relationship Movements Types" page
And I click on "annulations" "edit" "link" of any record
Then I should see "Edit relationship of movement types" interface
And I click on "annulation" "cancel" "button"
And the popup should be closed
