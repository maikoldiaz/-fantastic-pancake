@sharedsteps=4013 @owner=jagudelos @ui @testsuite=61559 @testplan=61542 @S15
Feature: ValidateCreationOfTicketsAndTriggeringOfConsolidationProcess
When the TRUE system is processing an official delta calculation request
And when the system receives a period of dates and a list with one or more segments
Then tickets must be created for segments and consolidation process must be initiated for operative movements and operative inventories

Background: Login
Given I am logged in as "admin"
@testcase=66952
Scenario: Validate the generation of ticket numbers, their details and intiation of consolidating process for movement segments during official delta calculation
Given I have ownership calculation data generated in the system for official delta
And I navigate to "Calculation of deltas by official adjustment" page
When I click on "newDeltasCalculation" "button"
And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
And select a time period from the Processing Period dropdown
And I click on "initOfficialDeltaTicket" "submit" "button"
And I save the values of Period start and end dates from UI
And I click on "validateOfficialDeltaTicket" "submit" "button"
And I click on "confirmOfficialDeltaTicket" "submit" "button"
Then I wait for Official Delta Calculation process to complete
And validate the following values for each chosen segment
| ColumnNamesInSpanish |
| Tiquete              |
| Fecha Inicial        |
| Fecha Final          |
| Fecha Ejecucion      |
| Usuario              |
| Estado               |
Then I validate that the segment ticket numbers are associated to the corresponding nodes
@testcase=66953
Scenario: Validate the generation of ticket numbers, their details and intiation of consolidating process for inventory segments during official delta calculation
Given I have ownership calculation data generated in the system for official delta
And I navigate to "Calculation of deltas by official adjustment" page
When I click on "newDeltasCalculation" "button"
And I select segment from "initOfficialDeltaTicket" "segment" "dropdown"
And select a time period from the Processing Period dropdown
And I click on "initOfficialDeltaTicket" "submit" "button"
And I save the values of Period start and end dates from UI
And I click on "validateOfficialDeltaTicket" "submit" "button"
And I click on "confirmOfficialDeltaTicket" "submit" "button"
Then I wait for Official Delta Calculation process to complete
And validate the following values for each chosen segment
| ColumnNamesInSpanish |
| Tiquete              |
| Fecha Inicial        |
| Fecha Final          |
| Fecha Ejecucion      |
| Usuario              |
| Estado               |
Then I validate that the segment ticket numbers are associated to the corresponding nodes
@testcase=66954 
Scenario: Validate the generation of ticket numbers, their details and intiation of consolidating process for movement and inventory segments during official delta calculation
Given that the TRUE system is processing the operative inventories consolidation
	When I have SON segment that has operative inventories with an inventory date is equal to the end date of the period
	And inventories have an ownership ticket
	And segment has neither consolidated movements for the date period nor consolidated inventories with a date equal to the end date of the period
	Then consolidate the net quantity and gross quantity of the inventories grouping the information by node and product but not to take into account the owners
	And consolidate the ownership quantity of the inventories by node, product and owner
	And the ownership of the movements must be obtained from the ownership information returned by FICO
	And join the information from the previous two points using node and product
	And store the consolidated inventories with "<field>"
		| field                                                          |
		| inventory date                                                 |
		| node identifier                                                |
		| product identifier                                             |
		| net quantity                                                   |
		| gross quantity                                                 |
		| unit identifier                                                |
		| owner ID                                                       |
		| ownership volume                                               |
		| ownership percentage with respect to consolidated net quantity |
		| scenario identifier                                            |
		| segment identifier                                             |
		| source                                                         |
		| execution date-time of the consolidation process               |
