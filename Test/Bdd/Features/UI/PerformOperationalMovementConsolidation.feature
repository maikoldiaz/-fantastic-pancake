@sharedsteps=4013 @owner=jagudelos @ui @testplan=61542 @testsuite=61548 @S15 @MVP2and3
Feature: PerformOperationalMovementConsolidation
As TRUE system, I need to perform operational movement
consolidation to execute the official deltas calculation

Background: Login
Given I am logged in as "admin"

@testcase=66885 @version=2 @parallel=false @independent
Scenario: 01 Verify SON segment with operative movements and no data consolidation for a period of dates
	Given that the TRUE system is processing the operative movements consolidation
	When I have SON segment that has operative movements with an operational date within the dates of a period
	And movements have an ownership ticket
	And the movements with a source movement identifier are of different types than the cancellation types configured in the relationships between movement types
	And the segment has neither consolidated movements for the date period nor consolidated inventories with a date equal to the end date of the period
	Then consolidate the net quantity and gross quantity of the movements grouping the information by source node, destination node, source product, destination product and movement type
	And consolidate the ownership quantity of the movements by source node, destination node, source product, destination product, movement type and owner
	And the ownership of the movements must be obtained from the ownership information returned by FICO
	And join the information from the previous two points using source node, destination node, source product, destination product and movement type
	And store the consolidated movements with "<field>"
	| field                                                          |
	| start date                                                     |
	| end date                                                       |
	| movement type identifier                                       |
	| source node identifier                                         |
	| destination node identifier                                    |
	| source product identifier                                      |
	| destination product identifier                                 |
	| net quantity                                                   |
	| gross quantity                                                 |
	| unit identifier                                                |
	| owner ID                                                       |
	| ownership quantity                                             |
	| ownership percentage with respect to consolidated net quantity |
	| scenario identifier                                            |
	| segment identifier                                             |
	| source                                                         |
	| execution date-time of the consolidation process               |
	
@testcase=66886 @version=2 @parallel=false @independent
Scenario: 07 Verify SON segment with cancellation type movements that have a source movement identifier
	When there are operative movements with a source movement identifier
	And the movement type is equal to one of the cancellation types configured in the relationships between movement types
	And the movement type is different to one of the cancellation types configured in the relationships between movement types
	And there are operative movements without a source movement identifier
	And movements have an operational date within the dates of a period
	And the movements belong to a SON segment
	And that the TRUE system is processing the operative movements consolidation for all movement types
	And movements have an ownership ticket
	And the segment has neither consolidated movements for the date period nor consolidated inventories with a date equal to the end date of the period
	Then consolidate the operative movements with a source movement identifier and the movement type is equal to one of the cancellation types configured in the relationships between movement types
	And get information from the source movement identifiers
	And the quantity of the source movements subtract the value of one of its cancellation movements
	And with the ownership percentages of each source movement (ownership returned by FICO) calculate the ownership quantity of the new movements
	And consolidate the net quantity and gross quantity of the calculated movements grouping the information by source node, destination node, source product, destination product and movement type
	And consolidate the ownership quantity of the calculated movements by source node, destination node, source product, destination product, movement type and owner
	And join the information from the previous two points using source node, destination node, source product, destination product and movement type
	And add the consolidation of these movements to consolidate list with operative movements with a source movement identifier and the movement type is different to one of the cancellation types configured in the relationships between movement types
	And add the consolidation of these movements to consolidate list with operative movements without a source movement identifier
	And movements to consolidate list should contain consolidated information of the net quantity and gross quantity grouping the information by source node, destination node, source product, destination product and movement type
	And consolidated information of the ownership quantity of the movements by source node, destination node, source product, destination product, movement type and owner
	And the ownership of the movements must be obtained from the ownership information returned by FICO
	And join the information from the previous two points using source node, destination node, source product, destination product and movement type
	And store the consolidated movements with "<field>"
	| field                                                          |
	| start date                                                     |
	| end date                                                       |
	| movement type identifier                                       |
	| source node identifier                                         |
	| destination node identifier                                    |
	| source product identifier                                      |
	| destination product identifier                                 |
	| net quantity                                                   |
	| gross quantity                                                 |
	| unit identifier                                                |
	| owner ID                                                       |
	| ownership quantity                                             |
	| ownership percentage with respect to consolidated net quantity |
	| scenario identifier                                            |
	| segment identifier                                             |
	| source                                                         |
	| execution date-time of the consolidation process               |

@testcase=66887 @version=2 @parallel=false @independent
Scenario: 02 Verify SON segment with operative movements and consolidated movements for a period of dates
	When SON segment already has consolidated movements for a date period or inventories with a date equal to the end date of the period
	And that the TRUE system is processing the operative movements
	Then do not run the process of operative movements consolidation

@testcase=66888 @version=2 @parallel=false @independent
Scenario: 03 Verify segment is not SON with operative movements and no data consolidation for a period of dates
	Given that the TRUE system is processing the operative movements consolidation for not SON segment
	When segment is not SON has operative movements with an operational date within the dates of a period
	And movements have owners reported from the source systems
	And the segment has neither consolidated movements for the date period nor consolidated inventories with a date equal to the end date of the period
	Then consolidate the net quantity and gross quantity of the movements grouping the information by source node, destination node, source product, destination product and movement type
	And consolidate the ownership quantity of the movements by source node, destination node, source product, destination product, movement type and owner
	And the ownership of the movements must be obtained from the owners information reported by the source systems
	And if the value of the owner was reported in percentage then the quantity must be calculated first
	And join the information from the previous two points using source node, destination node, source product, destination product and movement type
	And store the consolidated movements with "<field>"
	| field                                                          |
	| start date                                                     |
	| end date                                                       |
	| movement type identifier                                       |
	| source node identifier                                         |
	| destination node identifier                                    |
	| source product identifier                                      |
	| destination product identifier                                 |
	| net quantity                                                   |
	| gross quantity                                                 |
	| unit identifier                                                |
	| owner ID                                                       |
	| ownership quantity                                             |
	| ownership percentage with respect to consolidated net quantity |
	| scenario identifier                                            |
	| segment identifier                                             |
	| source                                                         |
	| execution date-time of the consolidation process               |

@testcase=66889 @parallel=false @independent @version=2
Scenario: 04 Verify segment is not SON with operative movements and consolidated movements for a period of dates
	When segment is not SON already has consolidated movements for a date period or inventories with a date equal to the end date of the period
	And that the TRUE system is processing the operative movements for not SON segment
	Then do not run the process of operative movements consolidation

@testcase=66890 @version=2 @parallel=false @independent
Scenario: 05 Verify segment is not SON has movements without owners
	Given that the TRUE system is processing the operative movements consolidation for not son segment has movements without owners
	When segment is not SON has operative movements with an operational date within the dates of a period
	And movements do not have owners reported from the source systems
	And the segment has neither consolidated movements for the date period nor consolidated inventories with a date equal to the end date of the period
	Then movements without owners should not be taken into account in the consolidation process

@testcase=66891 @version=2 @parallel=false @independent @backend @chaos
Scenario: 06 Verify technical exception occured during consolidation of movements
	Given I am authenticated as "admin"
	And that the TRUE system is processing the operative movements consolidation data
	When a technical exception occurs during the movements consolidation process of a segment
	Then update the segment ticket to failed
	And store the message "Se presentó un error técnico inesperado en la consolidación de movimientos e inventarios del escenario operativo. Por favor ejecute nuevamente el proceso o comuníquese con la mesa de ayuda."
	And store the exception in Application Insights

@testcase=66892 @manual
Scenario: Verify multiple segments with operative movements and no data consolidation for a period of dates
Given that the TRUE system is processing the operative movements consolidation
When I have multiple segments that has operative movements with an operational date within the dates of a period
And movements have an ownership ticket
And the movements with a source movement identifier are of different types than the cancellation types configured in the relationships between movement types
And segment has neither consolidated movements for the date period nor consolidated inventories with a date equal to the end date of the period
Then consolidate the net quantity and gross quantity of the movements grouping the information by source node, destination node, source product, destination product and movement type
And consolidate the ownership quantity of the movements by source node, destination node, source product, destination product, movement type and owner
And the ownership of the movements must be obtained from the ownership information returned by FICO
And join the information from the previous two points using source node, destination node, source product, destination product and movement type
And store the consolidated movements with "<field>"
| field                                                          |
| start date                                                     |
| end date                                                       |
| movement type identifier                                       |
| source node identifier                                         |
| destination node identifier                                    |
| source product identifier                                      |
| destination product identifier                                 |
| net quantity                                                   |
| gross quantity                                                 |
| unit identifier                                                |
| owner ID                                                       |
| ownership quantity                                             |
| ownership percentage with respect to consolidated net quantity |
| scenario identifier                                            |
| segment identifier                                             |
| source                                                         |
| execution date-time of the consolidation process               |