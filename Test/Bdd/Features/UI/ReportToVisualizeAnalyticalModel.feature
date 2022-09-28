@owner=jagudelos @testsuite=19790 @testplan=19772 @ui
Feature: ReportToVisualizeAnalyticalModel
As a TRUE System I need to display
the Power BI report of the results of
the trained analytical model

@testcase=21402 @bvt @manual
Scenario: Verify TRUE User is able to access the power bi report of the results of analytical model
Given I have historical data in the system
And I have trained the analytical model with the historical data
When  I open the power bi reports for analytical model
Then The reports should open

@testcase=21403 @bvt @manual
Scenario Outline: Verify TRUE User is able to see the pages in the reports
Given I have historical data in the system
And I have trained the analytical model with the historical data
When I open the power bi reports for analytical model
Then I should see the "<pages>"
Examples:
| pages                          |
| Forecast volumen sin propiedad |
| Forecast porcentaje propiedad  |
