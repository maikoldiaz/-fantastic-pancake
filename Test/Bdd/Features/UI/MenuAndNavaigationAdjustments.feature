@owner=jagudelos @ui @testsuite=35685 @testplan=35673 @parallel=false
Feature: MenuAndNavaigationAdjustments
In order to navigation and access the Menu
As a User
I want UI in the website
@testcase=37365 @bvt1.5
Scenario: Verify the navigation links
Given I am logged in as "admin"
When I click on "Menu" toggler
Then I should see the below "Menu" Options
| Menu                              |
| Administration                    |
| Balance of pipelines and stations |
| Pipeline and station reports      |
| Supply Chain Management           |
@testcase=37366 @bvt1.5
Scenario Outline: Verify transformation settings option under administration link
Given I am logged in as "admin"
When I click on "Menu" toggler
And I click on "Administration" link
Then I should see the "TransformSettings" option

Examples:
| User        |
| Admin       |
| Profesional |
@testcase=37367 @bvt1.5
Scenario: Verify the sub menu options under administration link
Given I am logged in as "admin"
When I click on "Menu" toggler
And I click on "Administration" link
Then I should see the below "SubMenu" Options
| SubMenu                                              |
| Category                                             |
| Category elements                                    |
| Nodes                                                |
| Configure attributes nodes                           |
| Configure connections attributes                     |
| Configure group nodes                                |
| Graphic Network Configuration                        |
| Homologation Configuration                           |
| TransformSettings                                    |
| Exception Management                                 |
| Configuration of transfer points - operational nodes |
| Configuration of transfer points - logistic nodes    |
| Approval Flow Settings                               |
@testcase=37368 @bvt1.5
Scenario Outline: Verify the sub menu options under balance of pipelines and stations link
Given I am logged in as "<User>"
When I click on "Menu" toggler
And I click on "Balance of pipelines and stations" link
Then I should see the below "SubMenu" Options
| SubMenu                                   |
| Load movements and inventories            |
| Load shopping, sales and PPA events       |
| Execution of the operational cut          |
| Property determination by segment         |
| Operating balance with ownership per node |

Examples:
| User        |
| Admin       |
| Profesional |
@testcase=37369
Scenario Outline: Verify approval of balance with ownership by node option under balance of pipelines and stations link
Given I am logged in as "<User>"
When I click on "Menu" toggler
And I click on "Balance of pipelines and stations" link
Then I should see the "Approval of balance with ownership by node" option

Examples:
| User      |
| Admin     |
| Aprobador |
@testcase=37370 @bvt1.5
Scenario Outline: Verify the sub menu options under pipeline and station reports link
Given I am logged in as "<User>"
When I click on "Menu" toggler
And I click on "Pipeline and station reports" link
Then I should see the below "SubMenu" Options
| SubMenu                                          |
| Configuration of purchases, sales and events PPA |
| Predictive analytical models                     |
| Operating balance with or without property       |
| Control letter                                   |
| Logistic movements and inventories               |

Examples:
| User        |
| Admin       |
| Profesional |
| Consulta    |
@testcase=37371 
Scenario Outline: Verify load movements and inventories option under supply chain management  link
Given I am logged in as "<User>"
When I click on "Menu" toggler
And I click on "Supply Chain Management" link
Then I should see the "Load movements and inventories" option

Examples:
| User        |
| Admin       |
| Profesional |
