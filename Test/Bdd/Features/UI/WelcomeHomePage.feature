@owner=jagudelos @ui @testsuite=70799 @testplan=70526 @S16
Feature: WelcomeHomePage
When a user authenticates to the app and has a role assigned with
permissions to any of the menu options, then the app welcome page should be displayed:
@testcase=72851 @final
Scenario Outline: Verify that authorized user are directed to welcome page
Given I am logged in as "<User>"
Then I should see welcome page
Examples:
| User             |
| admin            |
| aprobador        |
| profesional      |
| consulta         |
| audit            |
| programador      |

@testcase=72852 @manual
Scenario Outline: Verify that Unauthorized user are directed to Error page
Given I am logged in as user with no authorized role
Then I should not see welcome page
