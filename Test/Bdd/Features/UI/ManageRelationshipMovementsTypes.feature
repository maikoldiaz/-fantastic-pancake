@sharedsteps=7539 @owner=jagudelos @ui @MVP2and3 @testsuite=49478 @testplan=49466
Feature: ManageRelationshipMovementsTypes
In order to handle the Relationship Movements
As an application administrator
I want to manage Relationship Movements Types

@testcase=52387 @bvt
Scenario: Verify Relationship Movements Types menu accessible to the role of Administrator and present under Administration menu
	Given I am logged in as "admin"
	When I navigate to "Relationship Movements Types" page
	And I should see breadcrumb "Relación tipos de movimientos"

@testcase=52388
Scenario Outline: Verify Relationship Movements Types menu not accessible Where other users try accessing the menu
	Given I am logged in as "<User>"
	Then I should not see "Relationship Movements Types" tab

	Examples:
		| User        |
		| aprobador   |
		| programador |
		| consulta    |
		| profesional |

@testcase=52389 @BVT2
Scenario Outline: Verify Relationship Movements Types menu not accessible When the other Users try accessing the URL directly
	Given I am logged in as "<User>"
	When I hit the "Relationship Movements Types" URL directly
	Then I should see an unauthorized error page

	Examples:
		| User        |
		| aprobador   |
		| programador |
		| consulta    |
		| profesional |

@testcase=52390 @bvt @version=1
Scenario: Verify grid columns and sorted by Creation Date
	Given I am logged in as "admin"
	When I navigate to "Relationship Movements Types" page
	Then I verify all columns "tipo de movimiento","tipo de anulación","origen","destino","prod. origen","prod. destino","estado" are present in Grid
	And I verify the Relationship Movements Types list sorted by creation date in decending order

@testcase=52391 @bvt @version=1
Scenario Outline: Verify Filters functionality
	Given I am logged in as "admin"
	When I navigate to "Relationship Movements Types" page
	And I provide value for "<Field>" in the Grid for filter
	Then I should see the filtered data in the grid for "<Field>"

	Examples:
		| Field             |
		| Movements Type    | 
		| Cancellation type | 

@testcase=52392 @bvt @version=1
Scenario Outline: Verify Sorting functionality on Relationship Movement Types grid
	Given I am logged in as "admin"
	When I navigate to "Relationship Movements Types" page
	And I click on the "<ColumnName>"
	Then I verify the results should be sorted according to "<ColumnName>"

	Examples:
		| ColumnName        |
		| Movements Type    |
		| Cancellation type |
		| Source            |

@testcase=52393 @bvt @BVT2
Scenario: Verify Create Relationship button shown on the page
	Given I am logged in as "admin"
	When I navigate to "Relationship Movements Types" page
	Then I should see "crear relación" button

@testcase=52394
Scenario: Verify Pagination functionality on Relationship Movements Types grid
	Given I am logged in as "admin"
	When I navigate to "Relationship Movements Types" page
	And I change the elements count per page to 50
	Then I verify the records count shown per page as 50

@testcase=52395 @manual
Scenario: Verify grid message when no records found into the grid
	Given I am logged in as "admin"
	When I navigate to "Relationship Movements Types" page
	And I verify no records present in the grid
	Then I should see error message "Sin registros"