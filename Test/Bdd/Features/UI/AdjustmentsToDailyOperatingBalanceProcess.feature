@sharedsteps=16581 @owner=jagudelos @ui @testplan=26817 @testsuite=26841
Feature: AdjustmentsToDailyOperatingBalanceProcess
As a Professional Segment Balance User, I require some adjustments
to the daily operating balance processes to the data will be consistent

Background: Login
Given I am logged in as "profesional"

@testcase=28187 @ignore @version=2
Scenario Outline: Verify empty or error in report indicators when division by zero with ownership report
	Given I have "<Category>" for the ownership Report
	When I navigate to "OperativeBalanceReport" page
	And I select "<Category>" from "Category"
	And I select "<Element>" from "Element"
	And I enter "NodeName" into "Node" "Name" "textbox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
	And I click on "ReportFilter" "ViewReport" "button"
	Then I should see KPI Indicators in the Report Home Page
	When reports that have indicators are consulted and the last indicator value is empty or zero
	Then I should see "Error" message or empty structure in the KPI indicator
	
	Examples:
		| Category | Element        |
		| Segmento | SegmentElement |
		| Sistema  | SystemElement  |

@testcase=28188 @version=2
Scenario Outline: Verify inventory details from the day before the start date of the period with ownership report
	Given I have "<Category>" for the ownership Report
	When I navigate to "OperativeBalanceReport" page
	And I select "<Category>" from "Category"
	And I select "<Element>" from "Element"
	And I enter "NodeName" into "Node" "Name" "textbox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
	And I click on "ReportFilter" "ViewReport" "button"
	When I click on "InventoryDetails" "tab"
	Then it should display inventory values for dates from the day before the start of the selected period to the end day of the selected period
	
	Examples:
		| Category | Element        |
		| Segmento | SegmentElement |
		| Sistema  | SystemElement  |

@testcase=28189 @version=3
Scenario Outline: Verify drillthrough functionality on Movement Quality details tab when product is selected on ownership report
	Given I have "<Category>" for the ownership Report
	When I navigate to "OperativeBalanceReport" page
	And I select "<Category>" from "Category"
	And I select "<Element>" from "Element"
	And I enter "NodeName" into "Node" "Name" "textbox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
	And I click on "ReportFilter" "ViewReport" "button"
	When I Right click on Product and Select "MovementQuality"
	Then movement quality details should be displayed for selected product
	
	Examples:
		| Category | Element        |
		| Segmento | SegmentElement |
		| Sistema  | SystemElement  |

@testcase=28190 @version=2
Scenario Outline: Verify drillthrough functionality on movement details tab when product is selected on ownership report
	Given I have "<Category>" for the ownership Report
	When I navigate to "OperativeBalanceReport" page
	And I select "<Category>" from "Category"
	And I select "<Element>" from "Element"
	And I enter "NodeName" into "Node" "Name" "textbox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
	And I click on "ReportFilter" "ViewReport" "button"
	When I Right click on Product and Select "MovementDetails"
	Then movement details should be displayed for selected product

	Examples:
		| Category | Element        |
		| Segmento | SegmentElement |
		| Sistema  | SystemElement  |

@testcase=28191 @version=2
Scenario Outline: Verify reverse the signs of KPI indicators as it is made with the values in the table with ownership report
	Given I have "<Category>" for the ownership Report
	When I navigate to "OperativeBalanceReport" page
	And I select "<Category>" from "Category"
	And I select "<Element>" from "Element"
	And I enter "NodeName" into "Node" "Name" "textbox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
	And I click on "ReportFilter" "ViewReport" "button"
	Then I should see KPI Indicators in the Report Home Page
	When reports that have indicators are consulted and indicators are for variables of Tolerance or Unidentified Losses
	Then it should reverse the signs of the indicator's value

	Examples:
		| Category | Element        |
		| Segmento | SegmentElement |
		| Sistema  | SystemElement  |

@testcase=28192 @ignore @version=2
Scenario Outline: Verify empty or error in report indicators when division by zero without ownership report
	Given I have "<Category>" for the without ownership Report
	When I navigate to "OperativeBalanceReport" page
	And I select "<Category>" from "Category"
	And I select "<Element>" from "Element"
	And I enter "NodeName" into "Node" "Name" "textbox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
	And I click on "ReportFilter" "ViewReport" "button"
	Then I should see KPI Indicators in the Report Home Page
	When reports that have indicators are consulted and the last indicator value is empty or zero
	Then I should see "Error" message or empty structure in the KPI indicator

	Examples:
		| Category | Element        |
		| Segmento | SegmentElement |
		| Sistema  | SystemElement  |

@testcase=28193 @version=2
Scenario Outline: Verify inventory details from the day before the start date of the period without ownership report
	Given I have "<Category>" for the without ownership Report
	When I navigate to "OperativeBalanceReport" page
	And I select "<Category>" from "Category"
	And I select "<Element>" from "Element"
	And I enter "NodeName" into "Node" "Name" "textbox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
	And I click on "ReportFilter" "ViewReport" "button"
	When I click on "InventoryDetails" "tab"
	Then it should display inventory values for dates from the day before the start of the selected period to the end day of the selected period
	
	Examples:
		| Category | Element        |
		| Segmento | SegmentElement |
		| Sistema  | SystemElement  |

@testcase=28194 @version=2
Scenario Outline: Verify reverse the signs of KPI indicators as it is made without  the values in the table without ownership report
	Given I have "<Category>" for the without ownership Report
	When I navigate to "OperativeBalanceReport" page
	And I select "<Category>" from "Category"
	And I select "<Element>" from "Element"
	And I enter "NodeName" into "Node" "Name" "textbox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
	And I click on "ReportFilter" "ViewReport" "button"
	Then I should see KPI Indicators in the Report Home Page
	When reports that have indicators are consulted and indicators are for variables of Tolerance or Unidentified Losses
	Then it should reverse the signs of the indicator's value

	Examples:
		| Category | Element        |
		| Segmento | SegmentElement |
		| Sistema  | SystemElement  |

@testcase=28195 @version=3
	Scenario Outline: Verify drillthrough functionality on MovementQuality details tab when product is selected without ownership report
	Given I have "<Category>" for the without ownership Report
	When I navigate to "OperativeBalanceReport" page
	And I select "<Category>" from "Category"
	And I select "<Element>" from "Element"
	And I enter "NodeName" into "Node" "Name" "textbox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
	And I click on "ReportFilter" "ViewReport" "button"
	When I Right click on Product and Select "MovementQuality"
	Then movement quality details should be displayed for selected product

	Examples:
		| Category | Element        |
		| Segmento | SegmentElement |
		| Sistema  | SystemElement  |

@testcase=28196 @version=2
Scenario Outline: Verify drillthrough functionality on movement details tab when product is selected without ownership report
	Given I have "<Category>" for the without ownership Report
	When I navigate to "OperativeBalanceReport" page
	And I select "<Category>" from "Category"
	And I select "<Element>" from "Element"
	And I enter "NodeName" into "Node" "Name" "textbox"
	And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
	And I click on "ReportFilter" "ViewReport" "button"
	When I Right click on Product and Select "MovementDetails"
	Then movement details should be displayed for selected product
	
	Examples:
		| Category | Element        |
		| Segmento | SegmentElement |
		| Sistema  | SystemElement  |