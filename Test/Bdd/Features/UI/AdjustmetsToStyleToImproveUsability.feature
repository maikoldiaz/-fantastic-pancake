@sharedsteps=16581 @owner=jagudelos @ui @testplan=26817 @testsuite=28414
Feature: AdjustmentsToStyleGuideToImproveUsability
As a user of the TRUE application, I require adjustments to the style guide to improve usability

Background: Login
	Given I am logged in as "profesional"

@testcase=29516 @manual
Scenario Outline: Verify for all the grid columns that allow to resize the width, an indicator is shown using which the column can be resized
	When I navigate to <Module> page
	Then validate for the grid columns which can be resized, an indicator is displayed
	And validate that we can resize the grid columns using the indicator
	Examples:
		| Module                                                |
		| Categories                                            |
		| CategoryElements                                      |
		| Nodes                                                 |
		| Configure Node Attributes                             |
		| Configure Group Attributes                            |
		| Configure Connection Attributes                       |
		| Homologations                                         |
		| Exceptions                                            |
		| load of movements and inventories                     |
		| load other records                                    |
		| operational cut                                       |
		| volumetric balance with property by node              |
		| volumetric balance with ownership by pipeline segment |
		| logistic report generation                            |
		| load other records                                    |

@testcase=29517 @manual @version=2
Scenario Outline: Verify when user clicks on submenu of a different menu, all other open menus are closed automatically
	When I navigate to <Already Opened> menu
	And I navigate to <New> menu
	Then the <Already Opened> menu is closed
	Examples:
		| Already Opened         | New                    |
		| categoría              | corte operativo        |
		| categoría              | cargue otros registros |
		| corte operativo        | categoría              |
		| corte operativo        | cargue otros registros |
		| cargue otros registros | categoría              |
		| cargue otros registros | corte operativo        |

@testcase=29518 @manual
Scenario: Verify that the text "Actualizado A (date)" and "Confidencial" are shown in the top right corner for the reports
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I select "Node" from "Node"
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then validate "BalanceOperativeReport" is displayed
	And verify that the text "Actualizado A" and "Confidencial" are shown in the top right corner for the reports
	And verify that the current date is displayed after "Actualizado A"

@testcase=29519 @manual
Scenario: Verify that the entire text in report title is in capital letters and the column headers in the report are center aligned
	Given I have "Operative Balance" in the system
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I select "Node" from "Node"
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then validate "BalanceOperativeReport" is displayed
	And verify that the entire text in report title is in Capital letters
	And verify that the column headers in the report are center aligned

@testcase=29520 @manual
Scenario Outline: Verify that the numbers are displayed as per the regional format in the reports
	Given I have "Operative Balance" in the system
	And I have set the system locale to <System Locale>
	When I navigate to "OperativeBalanceReport" page
	And I select "Segmento" from "Category"
	And I select "CategorySegment" from "CategoryElement"
	And I select "Node" from "Node"
	And enter "StartDate" into "StartDate" "date"
	And enter "EndDate" into "EndDate" "date"
	And I click on "View" "Report" "button"
	Then validate "BalanceOperativeReport" is displayed
	And verify that the format of the numbers is displayed as per the selected <System Locale>
	Examples:
		| System Locale |
		| English       |
		| German        |
		| Spanish       |

@testcase=29521 @manual
Scenario Outline: Verify that the numbers are displayed as per the regional format in all the pages
	Given I have set the system locale to <System Locale>
	When I have navigated to any of the pages in the UI which have numbers displayed
	Then verify that the format of the numbers is displayed as per the selected <System Locale>
	Examples:
		| System Locale |
		| English       |
		| German        |
		| Spanish       |

@testcase=29522 @manual
Scenario Outline: Verify that the autocomplete feature is disabled on all the calendar controls in the UI
	When I navigate to "<Module>" page
	Then validate the date picker field should always be a dropdown to select the month and year and day
	And validate that the auto complete feature is disabled in the calendar controls
	Examples:
		| Module                   |
		| Categories               |
		| CategoryElements         |
		| Grouping Categories      |
		| UploadExcel              |
		| OperationalCutoff        |
		| BalanceOperationalReport |

@testcase=29523 @manual
Scenario: Verify that the autocomplete feature is disabled on all the textbox controls in the UI
	When I navigate to any page that has textbox control or a visual control that requires typing data
	Then verify that the autocomplete feature is disabled on these controls