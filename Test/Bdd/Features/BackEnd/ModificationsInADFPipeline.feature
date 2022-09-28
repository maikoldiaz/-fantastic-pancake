@owner=jagudelos @api @S14 @MVP2and3 @testsuite=55125 @testplan=55104
Feature: ModificationsInADFPipeline
As a TRUE System it is necessary to make modifications to the pipelines ADFs for the loading
of historical records of the operational movements with and without ownership due to the new
loading strategy for MVP 1.5

@testcase=56801 @bvt @priority=1 @BVT2
Scenario Outline: Verify that the historical information of movements from '2016-06-01' to '2020-05-31' date should be loaded
	Given I have historical data to create analytical models
	And I have deleted the data from Analytics Table stored previously
	When I upload the "<table>" into the blob
	And I initiate the data load process with load type as 0
	Then Modified Records from "2016-06-01" to "2020-05-31" date should be loaded into "<table>"

	Examples:
		| table                           |
		| OperativeMovementsWithOwnership |
		| OperativeMovements              |

@testcase=56802 @bvt @priority=0 @BVT2
Scenario Outline: Validated that the historical information of movements from '2016-06-01' to '2020-05-31' date should not be loaded
	Given I have historical data to create analytical models
	And I have deleted the data from Analytics Table stored previously
	When I upload the "<table>" into the blob
	And I initiate the data load process with load type as 1
	Then Modified Records from "2016-06-01" to "2020-05-31" date should not be loaded into "<table>"
	And Modified Records other than "2016-06-01" to "2020-05-31" date should be stored into "<table>"

	Examples:
		| table                           |
		| OperativeMovementsWithOwnership |
		| OperativeMovements              |