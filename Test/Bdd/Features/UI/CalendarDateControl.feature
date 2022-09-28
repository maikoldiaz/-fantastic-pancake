@sharedsteps=4013 @owner=jagudelos @testsuite=14724 @testplan=14709
Feature: CalendarDateControl
In order to input dates into the system
As a user
I need to have a calendar component

@testcase=16501 @ui @manual
Scenario Outline: Validate the Date picker component in all modules
	When I navigate to "<Module>" page
	Then validate the date picker field should always be a dropdown to select the month and year and day
	And validate blue calendar icon is displayed inside date textbox

	Examples:
		| Module                   |
		| Categories               |
		| CategoryElements         |
		| Grouping Categories      |
		| UploadExcel              |
		| OperationalCutoff        |
		| BalanceOperationalReport |