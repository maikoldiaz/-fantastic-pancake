@sharedsteps=7539 @owner=jagudelos @ui @MVP2and3 @testsuite=49477 @testplan=49466
Feature: CreateRelationshipMovementsTypes
In order to handle the Relationship Movements
As an application administrator
I want to create Relationship Movements Types

Background: Login
	Given I am logged in as "admin"

@testcase=52317 @version=2
Scenario: Verify data populated in Relationship Movements Types interface when click on Create relationship button
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	Then I should see "Create a new relationship of movement types" interface
	And "Movements type" drop down should be loaded with their corresponding values
	And "Cancellation type" drop down should be loaded with their corresponding values
	And "Source" drop down should be loaded with their corresponding values
	And "Destination" drop down should be loaded with their corresponding values
	And "Source product" drop down should be loaded with their corresponding values
	And "Destination product" drop down should be loaded with their corresponding values
	And I should see the default value for "Movements type" dropdown is "Seleccionar"
	And I should see the default value for "Cancellation type" dropdown is "Seleccionar"
	And I should see the default value for "Source" dropdown is "Seleccionar"
	And I should see the default value for "Destination" dropdown is "Seleccionar"
	And I should see the default value for "Source product" dropdown is "Seleccionar"
	And I should see the default value for "Destination product" dropdown is "Seleccionar"
	And I should see the Active control "True" by default

@testcase=52318 @version=1
Scenario: Verify filter the content of the list
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	Then I verify filter the content of the list "Movements type"
	And I verify filter the content of the list "Cancellation type"
	And I verify filter the content of the list "Source"
	And I verify filter the content of the list "Destination"
	And I verify filter the content of the list "Source product"
	And I verify filter the content of the list "Destination product"

@testcase=52319 @bvt @version=1
Scenario: Create Relationship Movements Types without mandatory fields
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	Then I should see "Create a new relationship of movement types" interface
	And I click on "annulation" "submit" "button"
	Then I should see error message "Requerido"

@testcase=52320 @version=1
Scenario: Verify cancellation type list content when the user selects a movement type
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	Then I should see "Create a new relationship of movement types" interface
	And I select existing Movement Type from Movements type dropdown then "cancellation type" list should not contain the above selected movement type

@testcase=52321 @version=2
Scenario: Create Relationship Movements Types when select already exists cancellation type
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	And I select "Movements type" from "Movements type" list
	And I select "existing cancellation type" from "Cancellation type" list
	And I select "source" from "Source" list
	And I select "destination" from "Destination" list
	And I select "source" from "Source product" list
	And I select "destination" from "Destination product" list
	And I click on "annulation" "submit" "button"
	Then I should see message as "El tipo de anulación seleccionado ya se encuentra asignado al tipo de movimiento" for "Movements type" assigned

@testcase=52322 @version=2 @BVT2
Scenario: Create Relationship Movements Types when select already exists movement type
	Given I create Movements and its Annulation Type
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	And I select "existing movement type" from "Movements type" list
	And I select "cancellation type" from "Cancellation type" list
	And I select "source" from "Source" list
	And I select "destination" from "Destination" list
	And I select "source" from "Source product" list
	And I select "destination" from "Destination product" list
	And I click on "annulation" "submit" "button"
	Then I should see message as "El tipo de movimiento seleccionado ya tiene asignada la anulación" for "Cancellation type" assigned

@testcase=52323 @version=2
Scenario: Verify Destination list not displayed None item when None item selected in the source list
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	And I select "none" from "Source" list
	Then "Destination" list should not contain "none" option

@testcase=52324 @version=1
Scenario Outline: Verify Destination list displayed all options when user selects other item excepts None in the source list
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	And I select "<Item>" from "Source" list
	Then "Destination" list should contain all options

	Examples:
		| Item        |
		| source      |
		| destination |

@testcase=52325 @version=2
Scenario: Verify Source list not displayed None item when None item selected in the Destination list
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	And I select "none" from "Destination" list
	Then "Source" list should not contain "none" option

@testcase=52326 @version=2
Scenario Outline: Verify Source list displayed all options when user selects other item excepts None in the Destination list
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	And I select "<Item>" from "Destination" list
	Then "Source" list should contain all options

	Examples:
		| Item        |
		| source      |
		| destination |

@testcase=52327 @version=2
Scenario: Verify Destination product list not displayed None item when None item selected in the Source product list
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	And I select "none" from "Source product" list
	Then "Destination product" list should not contain "none" option

@testcase=52328 @version=2
Scenario Outline: Verify Destination product list displayed all options when user selects other item excepts None in the Source product list
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	And I select "<Item>" from "Source product" list
	Then "Destination product" list should contain all options

	Examples:
		| Item        |
		| source      |
		| destination |

@testcase=52329 @version=2
Scenario: Verify Source product list not displayed None item when None item selected in the Destination product list
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	And I select "none" from "Destination product" list
	Then "Source product" list should not contain "None" option

@testcase=52330 @version=2
Scenario Outline: Verify Source product list displayed all options when user selects other item excepts None in the Destination product list
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	And I select "<Item>" from "Destination product" list
	Then "Source product" list should contain all options

	Examples:
		| Item        |
		| source      |
		| destination |

@testcase=52331 @bvt @version=1 @BVT2
Scenario: Create Relationship Movements Types with valid data
	Given I create Movements and its Annulation Type
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	And I select "Movements type" from "Movements type" list
	And I select "cancellation type" from "Cancellation type" list
	And I select "source" from "Source" list
	And I select "destination" from "Destination" list
	And I select "source product" from "Source product" list
	And I select "destination product" from "Destination product" list
	And I click on "annulation" "submit" "button"
	Then I should see breadcrumb "Relación tipos de movimientos"

@testcase=52332 @version=1 @BVT2
Scenario: Verify movement type list content when the user selects a cancellation type
	When I navigate to "Relationship Movements Types" page
	And I click on "createRelation" "button"
	Then I should see "Create a new relationship of movement types" interface
	And I select existing Cancellation Type from Cancellation type dropdown then "Movements Type" list should not contain the above selected cancellation type