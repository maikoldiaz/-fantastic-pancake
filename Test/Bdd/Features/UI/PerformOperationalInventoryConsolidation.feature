@sharedsteps=4013 @owner=jagudelos @testplan=61542 @testsuite=61552 @ui @S15 @MVP2and3 
Feature: PerformOperationalInventoryConsolidation

Background: Login
Given I am logged in as "admin"

@testcase=66876 @version=2 @parallel=false @independent
Scenario: Verify SON segment with operative inventories and no data consolidation where the inventory date is equal to the end date of the period
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

@testcase=66877 @version=2 @parallel=false @independent
Scenario: Verify SON segment with operative inventories and no data consolidation but segment has a consolidation process executed for another period. where the inventory date is equal to the end date of the period
	Given that the TRUE system is processing the operative inventories consolidation for which segment is already having consolidation for another period
	When I have SON segment that has operative inventories with an inventory date is equal to the end date of the period
	And inventories have an ownership ticket
	And segment has neither consolidated movements for the date period nor consolidated inventories with a date equal to the end date of the period
	And segment has already consolidation process executed for another period
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

@testcase=66878 @manual @version=2
Scenario: Verify SON segment with operative inventories and no data consolidation where the inventory date is equal to the start date of the period minus one day.
	Given that the TRUE system is processing the operative inventories consolidation
	When I have SON segment that has operative inventories with an inventory date is equal to the end date of the period
	And inventories have an ownership ticket
	And segment has neither consolidated movements for the date period nor consolidated inventories with a date equal to the end date of the period
	And segment does NOT have a consolidation process executed for another period
	Then consolidate the net quantity and gross quantity of the inventories grouping the information by node and product but not to take into account the owners
	And consolidate the ownership quantity of the inventories by node, product and owner
	And the ownership of the movements must be obtained from the ownership information returned by FICO
	And join the information from the previous two points using node and product
	And store the consolidated inventories with "<field>"
		| field                                                          |
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

@testcase=66879 @version=2 @parallel=false @independent
Scenario: Verify SON segment with consolidated inventories for a date period or inventories with a date equal to the end date of the period.
	When SON segment already has consolidated inventories for a date period or inventories with a date equal to the end date of the period
	And that the TRUE system is processing the operative inventories
	Then do not run the process of operative inventories consolidation

@testcase=66880 @version=2 @parallel=false @independent
Scenario: Verify inventories with owners should undergo consolidation process.
	Given that the TRUE system is processing the operative inventories consolidation for not SON segment
	When I have not SON segment that has operative inventories with an inventory date is equal to the end date of the period
	And inventories have owners reported from the source systems
	And the segment has neither consolidated movements for the date period nor consolidated inventories with a date equal to the end date of the period
	Then consolidate the net quantity and gross quantity of the inventories grouping the information by node and product but not to take into account the owners
	And consolidate the ownership quantity of the inventories by node, product and owner
	And the ownership of the movements must be obtained from the ownership information returned by FICO
	And join the information from the previous two points using node and product
	And store the consolidated inventories with "<field>"
		| field                                                          |
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

@testcase=66881 @version=2 @parallel=false @independent
Scenario: Verify not SON segment with consolidated inventories for a date period or inventories with a date equal to the end date of the period.
	When segment is not SON already has consolidated movements for a date period or inventories with a date equal to the end date of the period
	And that the TRUE system is processing the operative inventories for not SON segment
	Then do not run the process of operative inventories consolidation

@testcase=66882 @version=2 @parallel=false @independent
Scenario: Verify inventories without owners should not be taken into account in the consolidation process.
	Given that the TRUE system is processing the operative inventories consolidation for not son segment has movements without owners
	When segment is not SON has operative inventories with an operational date within the dates of a period
	And inventories do not have owners reported from the source systems
	And the segment has neither consolidated movements for the date period nor consolidated inventories with a date equal to the end date of the period
	Then movements without owners should not be taken into account in the consolidation process

@testcase=66883 @version=2 @parallel=false @independent
Scenario: Verify technical exception occurred during consolidation of inventories
	Given I am authenticated as "admin"
	And that the TRUE system is processing the operative inventories consolidation data
	When a technical exception occurs during the inventories consolidation process of a segment
	Then update the segment ticket to failed
	And store the message "Se presentó un error técnico inesperado en la consolidación de movimientos e inventarios del escenario operativo. Por favor ejecute nuevamente el proceso o comuníquese con la mesa de ayuda."
	And store the exception in Application Insights