@owner=jagudelos @backend @testplan=35673 @testsuite=35696 @parallel=false
Feature: ChangesinAnalyticalModel
As a TRUE system, I need to make changes in the Analytical Processes
to access the ModelEvaluation table directly from the Power BI Report
and modify the ADFs pipelines of historical load.
@testcase=38064 @bvt1.5
Scenario Outline: Verify that while loading the historical movement tables it must be validated that Modified Records from '2016-06-01' to '2019-07-31' date should NOT be loaded
Given I have historical data to create analytical models
When I upload the "<table>" into the blob
And I initiate the data load process
Then Modified Records from "2016-06-01" to "2019-07-31" date should NOT be loaded into "<table>"

Examples:
| table                           |
| OperativeMovementsWithOwnership |
| OperativeMovements              |
@testcase=38065 @bvt1.5
Scenario Outline: Verify that while loading the historical movement tables it must be validated that Modified Records other than '2016-06-01' to '2019-07-31' date should be loaded
Given I have historical data to create analytical models
When I upload the "<table>" into the blob
And I initiate the data load process
Then Modified Records other than "2016-06-01" to "2019-07-31" date should be loaded into "<table>"

Examples:
| table                           |
| OperativeMovementsWithOwnership |
| OperativeMovements              |

@testcase=38066 @manual
Scenario: Verify that within the PowerBI Report the data comes directly from the ModelEvaluation table(Insertion of a record)
Given I have data in "ModelEvaluation" "table"
When I insert a record in the "ModelEvaluation" "table"
And I open the power bi reports for analytical model
Then The data should be directly from the ModelEvaluation in power bi reports
And the inserted record should be present in the power bi report

@testcase=38067 @manual
Scenario: Verify that within the PowerBI Report the data comes directly from the ModelEvaluation table(Deletion of a record)
Given I have data in "ModelEvaluation" "table"
When I delete a record in the "ModelEvaluation" "table"
And I open the power bi reports for analytical model
Then The data should be directly from the ModelEvaluation in power bi reports
And The deleted record should not present in the power bi report
