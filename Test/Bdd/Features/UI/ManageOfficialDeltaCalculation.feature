@owner=jagudelos @ui @MVP2and3 @S15 @testsuite=61549 @testplan=61542
Feature: ManageOfficialDeltaCalculation
As a user of chain, I need a UI to track the
official delta calculation in the nodes

@testcase=66840
Scenario Outline: Verify Official deltas by node menu accessible to the role of Administrator/Chain and present under Supply chain management menu
	Given I am logged in as "<User>"
	When I navigate to "Official deltas by node" page
	Then I should see breadcrumb "Deltas oficiales por nodo"

	Examples:
		| User  |
		| admin |
		| chain |

@testcase=66841
Scenario Outline: Verify Official deltas by node menu not accessible Where other users try accessing the menu
	Given I am logged in as "<User>"
	Then I should not see "Official deltas by node" tab

	Examples:
		| User        |
		| aprobador   |
		| programador |
		| consulta    |
		| profesional |

@testcase=66842
Scenario Outline: Verify Official deltas by node menu not accessible When the other Users try accessing the URL directly
	Given I am logged in as "<User>"
	When I hit the "Official deltas by node" URL directly
	Then I should see an unauthorized error page

	Examples:
		| User        |
		| aprobador   |
		| programador |
		| consulta    |
		| profesional |

@testcase=66843 @bvt @version=2 @BVT2
Scenario: Verify grid columns and sorted by node in ascending order
	Given I am logged in as "admin"
	When I navigate to "Official deltas by node" page
	Then I verify all columns "Tiquete","Fecha inicial","Fecha final","Fecha ejecución","Usuario","Nodo","Segmento" are present in Grid
	And I should see the Official deltas by node list sorted by node in ascending order
	And I should see the records on the grid for last 90 days
	And the records count in "Official deltas by node" Grid shown per page should also be 50

@testcase=66844 @version=2
Scenario: Verify grid records filtered by segment ticket and sorted by node in ascending order
	Given I am logged in as "admin"
	When I navigate to "Calculation of deltas by official adjustment" page
	And I select "ticketId" for "tickets" "ticketId" "textbox" filter
	And I click on "tickets" "viewSummary" link in the grid
	Then I should see breadcrumb "Deltas oficiales por nodo"
	And I should see the Official deltas by node list for filtered segment ticket and sorted by node in ascending order

@testcase=66845 @bvt @version=2 @BVT2
Scenario: Verify failed status rows in Official deltas by node page
	Given I am logged in as "admin"
	When I navigate to "Official deltas by node" page
	And I select "Fallido" from the "officialDeltaNodesGrid" "status" "dropdown" filter
	Then I should see "officialDeltaNodesGrid" "viewErrorCaps" "0" "link" as enabled
	And I should see "officialDeltaNodesGrid" "viewReport" "0" "link" button as disabled

@testcase=66846 @bvt @version=2 @BVT2
Scenario: Verify other than failed status rows in Official deltas by node page
	Given I am logged in as "admin"
	When I navigate to "Official deltas by node" page
	And I select "Deltas" from the "officialDeltaNodesGrid" "status" "dropdown" filter
	Then validate that "officialDeltaNodesGrid" "viewErrorCaps" "0" "link" is "disabled"
	And I should see "officialDeltaNodesGrid" "viewReport" "0" "link" as enabled
	And I click on "officialDeltaNodesGrid" "viewReport" "0" "link"
	And I should see breadcrumb "Balance oficial por nodo"

@testcase=66847 @manual
Scenario Outline: Verify approval and rejection statuses Manually
	Given I am logged in as "admin"
	When I navigate to "Official deltas by node" page
	And I update the Officialdeltas Table and set the status of last record as "<status>"
	Then I should see status and icon corresponding to the manual modification in the grid

	Examples:
		| status   |
		| Approved |
		| Rejected |

@testcase=66848 @version=2
Scenario: Verify Back to list button when the page open from Calculation of deltas by official adjustment
	Given I am logged in as "admin"
	When I navigate to "Calculation of deltas by official adjustment" page
	And I select "Deltas" from the "tickets" "state" "dropdown" filter
	And I click on "tickets" "viewSummary" link in the grid
	Then I should see breadcrumb "Deltas oficiales por nodo"
	And I should see "returnListing" "button" as enabled

@testcase=66849 @version=2
Scenario: Verify Back to list button when the page open directly from Official deltas by node menu
	Given I am logged in as "admin"
	When I navigate to "Official deltas by node" page
	Then I should see breadcrumb "Deltas oficiales por nodo"
	And I should see "returnListing" "button" is hide

@testcase=66850 @manual
Scenario: Verify grid message when no records found into the grid
	Given I am logged in as "admin"
	When I navigate to "Official deltas by node" page
	And I verify no records present in the grid
	Then I should see error message "Sin registros""