@owner=jagudelos @backend  @testplan=8481 @testsuite=8488
Feature: NewOperationalReport
As a query user
I need all balance variables to be included in the operational report
to view the full balance

@testcase=9247 @Manual
Scenario: View Interface in Operational Report for a TicketId
	Given I have TicketId for which Calculation is complete
	When I open the Operational Report for the TicketId
	Then the report should contain Operating Balance with Interfaces

@testcase=9248 @Manual
Scenario: View Tolerance in Operational Report for a TicketId
	Given I have TicketId for which Calculation is complete
	When I open the Operational Report for the TicketId
	Then the report should contain Operating Balance with Tolerance

@testcase=9249 @Manual
Scenario: View Not Identified Losses in Operational Report for a TicketId
	Given I have TicketId for which Calculation is complete
	When I open the Operational Report for the TicketId
	Then the report should contain Operating Balance with Not Identified Losses