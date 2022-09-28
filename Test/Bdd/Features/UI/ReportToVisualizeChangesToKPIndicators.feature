@ui @owner=jagudelos @testsuite=39237 @testplan=39221
Feature: ReportToVisualizeChangesToKPIndicators
As a professional segment balance user, I need to adjust
balance reports with and without ownership in the display
section of the KPI

@testcase=41469 @bvt
Scenario Outline: Verify as a TRUE User decrement in KPI when past value is greater than current value
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
When I click on "InitTicket" "submit" "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
When I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
Then validate that "ErrorsGrid" "Submit" "button" as enabled
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
When I click on "unbalancesGrid" "submit" "button"
When I click on "ConfirmCutoff" "Submit" "button"
When I wait till cutoff ticket processing to complete
When I generate report for the data
Then I view the report
And I verify the KPI Indicators for "value" should display the decrement value with a green arrow symbol

Examples:
| value               |
| LOSSES IDENTIFIED   |
| INTERFACE           |
| TOLERANCE           |
| UNIDENTIFIED LOSSES |

@testcase=41470 @bvt
Scenario Outline: Verify as a TRUE User increment in KPI when past value is less than the current value
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
When I click on "InitTicket" "submit" "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
When I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
Then validate that "ErrorsGrid" "Submit" "button" as enabled
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
When I click on "unbalancesGrid" "submit" "button"
When I click on "ConfirmCutoff" "Submit" "button"
When I wait till cutoff ticket processing to complete
When I generate report for the data
Then I view the report
And I verify the KPI Indicators for "value" should display the increment with a red arrow symbol

Examples:
| value               |
| LOSSES IDENTIFIED   |
| INTERFACE           |
| TOLERANCE           |
| UNIDENTIFIED LOSSES |

@testcase=41471 @bvt
Scenario Outline: Verify as a TRUE User equality in KPI when past value is equal to current value
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
When I click on "InitTicket" "submit" "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
When I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
Then validate that "ErrorsGrid" "Submit" "button" as enabled
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
When I click on "unbalancesGrid" "submit" "button"
When I click on "ConfirmCutoff" "Submit" "button"
When I wait till cutoff ticket processing to complete
When I generate report for the data
Then I view the report
And I verify the KPI Indicators for "value"  should display the equality with the yellow equality symbol

Examples:
| value               |
| LOSSES IDENTIFIED   |
| INTERFACE           |
| TOLERANCE           |
| UNIDENTIFIED LOSSES |

@testcase=41472 @bvt
Scenario Outline: Verify as a TRUE User Error in KPI when past value or current value is empty
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
When I click on "InitTicket" "submit" "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
When I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
Then validate that "ErrorsGrid" "Submit" "button" as enabled
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
When I click on "unbalancesGrid" "submit" "button"
When I click on "ConfirmCutoff" "Submit" "button"
When I wait till cutoff ticket processing to complete
When I generate report for the data
Then I view the report
And I verify the KPI Indicators for "value" should display the word ERROR

Examples:
| value               |
| LOSSES IDENTIFIED   |
| INTERFACE           |
| TOLERANCE           |
| UNIDENTIFIED LOSSES |

@testcase=41473 @bvt
Scenario Outline: Verify as a TRUE User for large numbers no formatting is done for KPI indicators
Given I have "ownershipnodes" created
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Cutoff" DatePicker
When I click on "InitTicket" "submit" "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
When I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
Then validate that "ErrorsGrid" "Submit" "button" as enabled
When I click on "ErrorsGrid" "Submit" "button"
And I select all unbalances in the grid
And I click on "consistencyCheck" "AddNote" "button"
When I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "submit" "button"
When I click on "unbalancesGrid" "submit" "button"
When I click on "ConfirmCutoff" "Submit" "button"
When I wait till cutoff ticket processing to complete
When I generate report for the data
Then I view the report
And I verify the KPI Indicators for "value" should display without any special format

Examples:
| value               |
| LOSSES IDENTIFIED   |
| INTERFACE           |
| TOLERANCE           |
| UNIDENTIFIED LOSSES |
