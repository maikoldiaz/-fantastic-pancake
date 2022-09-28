@owner=jagudelos @testplan=70526 @testsuite=70809 @ui @S16 @MVP2and3
Feature: PendingAprrovalUIForOfficialBalencesOfNodesByPeriod

@testcase=71730 @version=2
Scenario Outline:  Verify that admin and aprobador users should be able to see the Approval of the official balance by node page
	Given I am logged in as "<user>"
	Then I navigate to "Supply Chain Management" tab
	Then I should see "Approval of the official balance by node" tab

	Examples:
		| user      |
		| admin     |
		| aprobador |

@testcase=71731 @version=2 @BVT2
Scenario Outline: Verify that other than admin and aprobador users should not be able to see the Approval of the official balance by node page
	Given I am logged in as "<User>"
	Then I navigate to "Supply Chain Management" tab
	Then I should not see "Approval of the official balance by node" tab

	Examples:
		| User        |
		| Cadena      |
		| programador |
		| Profesional |
		| Consulta    |
		| Auditor     |

@testcase=71732 @version=2 @BVT2
Scenario: Verify Microsoft flow for official balance approval is embedded
	Given I am logged in as "admin"
	When I have delta node with official balance and Level 1 approver is configured
	And I navigate to "Official deltas by node" page
	And I select "Deltas" from the "officialDeltaNodesGrid" "status" "dropdown" filter
	Then I select node from the grid
	And I click on "officialDeltaNodesGrid" "viewReport" "0" "link"
	And I should see breadcrumb "Balance oficial por nodo"
	Then I click on "submitForApproval" "button"
	And I navigate to "Approval of the official balance by node" page
	And I wait for "20" seconds for the process to end
	When I open the approval flow of the respective node

@testcase=71733 @version=2
Scenario: Verify error message on send for approval if Level 1 approver is not configured in the configuration table for Official delta node
	Given I am logged in as "admin"
	And I have delta node with official balance and Level 1 approver is not configured
	When I navigate to "Official deltas by node" page
	And I select "Deltas" from the "officialDeltaNodesGrid" "status" "dropdown" filter
	Then I select node from the grid
	And I click on "officialDeltaNodesGrid" "viewReport" "0" "link"
	And I should see breadcrumb "Balance oficial por nodo"
	Then I click on "submitForApproval" "button"
	Then I should see "Los nodos requieren tener configurado un aprobador de primer nivel. Por favor, antes de continuar, defina el correo de ese usuario para este nodo." error message

@testcase=71734 @version=2
Scenario: Verify the Microsoft flow is invoked and change in node status (Approved) when official balance approval flow is initiated
	Given I am logged in as "admin"
	When I have delta node with official balance and Level 1 approver is configured
	And I navigate to "Official deltas by node" page
	And I select "Deltas" from the "officialDeltaNodesGrid" "status" "dropdown" filter
	Then I select node from the grid
	And I click on "officialDeltaNodesGrid" "viewReport" "0" "link"
	And I should see breadcrumb "Balance oficial por nodo"
	Then I click on "submitForApproval" "button"
	And I navigate to "Approval of the official balance by node" page
	And I wait for "20" seconds for the process to end
	When I open the approval flow of the respective node
	And I click on "Reporte balance oficial por nodo" link and validate report is opened in new tab
	Then I confirm approve or reject the request
	Then I validate node status is "Aprobado"

@testcase=71735 @version=2
Scenario: Verify the Microsoft flow is invoked and change in node status (Rejected) when official balance approval flow is initiated
	Given I am logged in as "admin"
	When I have delta node with official balance and Level 1 approver is configured
	And I navigate to "Official deltas by node" page
	And I select "Deltas" from the "officialDeltaNodesGrid" "status" "dropdown" filter
	Then I select node from the grid
	And I click on "officialDeltaNodesGrid" "viewReport" "0" "link"
	Then I click on "submitForApproval" "button"
	And I navigate to "Approval of the official balance by node" page
	And I wait for "10" seconds for the process to end
	When I "reject" the respective request
	Then I validate node status is "Rechazado"