@owner=jagudelos @ui @testsuite=49479 @testplan=49466 @MVP2and3
Feature: FilterPageForSettingsAuditReport
In order to generate the settings audit report SOX
As an auditor user
I require a filter page
@testcase=52340
Scenario Outline: Verify report page should present for administrator and auditor role under Administración menu
Given I am logged in as "<User>"
Then I should or should not see "Audit Configuration" page based on user "<User>"

Examples:
| User  |
| admin |
| audit |
@testcase=52341
Scenario Outline: Verify report page should not present for user other than administrator and auditor role under Administración menu
Given I am logged in as "<User>"
Then I should or should not see "Audit Configuration" page based on user "<User>"

Examples:
| User        |
| aprobador   |
| profesional |
| programador |
| consulta    |
@testcase=52342 @version=2
Scenario: Verify breadcrumb for Audit Configuration page
Given I am logged in as "audit"
When I navigate to "Audit Configuration" page
Then I should see breadcrumb "Auditoría de configuraciones"
@testcase=52343 @version=2
Scenario: Verify start date and end date fields on initial page load of Audit Configuration page
Given I am logged in as "audit"
When I navigate to "Audit Configuration" page
Then I validate that the date in "nodeFilter" "initialDate" "date" should be enabled until one day before the current date
And I validate that the date in "nodeFilter" "finalDate" "date" should be enabled until one day before the current date
@testcase=52344 @version=2
Scenario: Verify mandatory fields on initial page load of Audit Configuration page
Given I am logged in as "audit"
When I navigate to "Audit Configuration" page
And I click on "nodeFilter" "Submit" "button"
Then I should see error message "Requerido" below each field on "Audit Configuration" page
@testcase=52345 @version=2
Scenario: Verify error message when user selects initial date greater than final date of Audit Configuration page
Given I am logged in as "audit"
When I navigate to "Audit Configuration" page
And I select the date in "nodeFilter" "initialDate" "date" based on below criteria
| key           | value               |
| dateSelection | initial             |
| daysLessThen  | 2                   |
| page          | Audit Configuration |
And I select the date in "nodeFilter" "finalDate" "date" based on below criteria
| key           | value               |
| dateSelection | final               |
| daysLessThen  | 4                   |
| page          | Audit Configuration |
And I click on "nodeFilter" "Submit" "button"
Then I should see the message "La fecha inicial debe ser menor o igual a la fecha final" in the Page
@testcase=52346 @version=2
Scenario: Verify error message when user selects number of valid days between the initial and final date is greater than the value configured in the valid days parameter of Audit Configuration page
Given I am logged in as "audit"
When I navigate to "Audit Configuration" page
And I select the date in "nodeFilter" "initialDate" "date" based on below criteria
| key           | value               |
| dateSelection | initial             |
| daysLessThen  | 70                  |
| page          | Audit Configuration |
And I select the date in "nodeFilter" "finalDate" "date" based on below criteria
| key           | value               |
| dateSelection | final               |
| daysLessThen  | 4                   |
| page          | Audit Configuration |
And I click on "nodeFilter" "viewReport" "button"
Then I should see the message "El rango de días elegidos debe ser menor a 62 días" in the Page
@testcase=52347 @version=2
Scenario: Verify Audit Configuration report is displayed after selecting valid days
Given I am logged in as "audit"
When I navigate to "Audit Configuration" page
And I select the date in "nodeFilter" "initialDate" "date" based on below criteria
| key           | value               |
| dateSelection | initial             |
| daysLessThen  | 4                   |
| page          | Audit Configuration |
And I select the date in "nodeFilter" "finalDate" "date" based on below criteria
| key           | value               |
| dateSelection | final               |
| daysLessThen  | 3                   |
| page          | Audit Configuration |
And I click on "nodeFilter" "Submit" "button"
Then I should see "backToSettingsAudit" "button"
@testcase=52348 @version=2 @BVT2
Scenario: Verify Change date range link in the Audit Configuration report page
Given I am logged in as "audit"
When I navigate to "Audit Configuration" page
And I select the date in "nodeFilter" "initialDate" "date" based on below criteria
| key           | value               |
| dateSelection | initial             |
| daysLessThen  | 10                  |
| page          | Audit Configuration |
And I select the date in "nodeFilter" "finalDate" "date" based on below criteria
| key           | value               |
| dateSelection | final               |
| daysLessThen  | 10                  |
| page          | Audit Configuration |
And I click on "nodeFilter" "Submit" "button"
Then I should see "backToSettingsAudit" "button"
Then I should see "backToSettingsAudit" "button" as enabled
And I click on "backToSettingsAudit" "button"
Then I should see "nodeFilter" "Submit" "button"
And validate previously selected "initial" date in "nodeFilter" "initialDate" "date" on the filters page
And validate previously selected "final" date in "nodeFilter" "finalDate" "date" on the filters page
