@owner=jagudelos @ui @testsuite=26832 @testplan=26817
Feature: ReportToVisualizeResultOfAllAnalyticalModels
As a Query User,
I need to visualize the Power BI report
with the result of all analytical models.

@testcase=29586 @backend @database @version = 1
Scenario: Validate results of trained models in model evaluation table
Given I have to validate the records in analytical models
When I connect to Analytics model evaluation table
Then I validate records in model evaluation table

@testcase=29587 @ui @version= 1
Scenario: Validate power bi report with results of all analytical models
Given I have to validate the power BI report of all analytical models
When I view report
Then I view updated power bi report with all analytical models
