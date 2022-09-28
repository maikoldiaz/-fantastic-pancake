@sharedsteps=16534 @owner=jagudelos @ui @testsuite=26838 @testplan=26817
Feature: OperationalReportByNodeWithoutOperationalCutoffToVerifyMovementsAndInventories
In order to view operational report without operational cutoff
As a query user, I want to view the detail of the operational report by
node without operational cutoff to verify the data of the movements and inventories

Background: Login
Given I am logged in as "consulta"

@testcase=28219 @ui
Scenario Outline: Validate node movement detail of a product without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I Right click on Product and Select "MovementDetails"
Then validate "source or destination node" of movement should belong to selected "node"
And validate "source or destination product" of movement should belong to selected "product"
And validate "operational date" of movement should be within selected date range
And source system of movements should not be equal to TRUE
And movements must be displayed ascendingly by operational date
Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=28220 @ui
Scenario Outline: Validate node inventory detail of a product without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I Right click on Product and Select "InventoryDetails"
Then validate "node" of "inventory" should belong to selected "node"
And validate "product" of "inventory" should belong to selected "product"
And validate "operational date" of "inventory" should be greater than or equal to the initial date of the period minus one day and less than or equal to the end date of the period
And inventories must be displayed ascendingly by inventory date
Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=28221 @ui
Scenario Outline: Validate node movement quality detail of a product without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I Right click on Product and Select "MovementQuality"
Then validate "source or destination node" of movement should belong to selected "node"
And validate "source or destination product" of movement should belong to selected "product"
And validate "operational date" of movement should be within selected date range
And source system of movements should not be equal to TRUE
And movements must be displayed ascendingly by operational date
Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=28222 @ui
Scenario Outline: Validate node inventory quality detail of a product without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I Right click on Product and Select "InventoryQuality"
Then validate "node" of "inventory" should belong to selected "node"
And validate "product" of "inventory" should belong to selected "product"
And validate "operational date" of "inventory" should be greater than or equal to the initial date of the period minus one day and less than or equal to the end date of the period
And inventories must be displayed ascendingly by inventory date
Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=28223 @ui
Scenario Outline: Validate node movement detail without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I click on "MovementDetails" "tab"
Then validate "source or destination node" of movement should belong to selected "node"
And validate "operational date" of movement should be within selected date range
And source system of movements should not be equal to TRUE
And movements must be displayed ascendingly by operational date
Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=28224 @ui
Scenario Outline: Validate node inventory detail without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I click on "InventoryDetails" "tab"
Then validate "node" of "inventory" should belong to selected "node"
And validate "operational date" of "inventory" should be greater than or equal to the initial date of the period minus one day and less than or equal to the end date of the period
And inventories must be displayed ascendingly by inventory date
Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=28225 @ui
Scenario Outline: Validate node movement quality detail without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I click on "MovementQuality" "tab"
Then validate "source or destination node" of movement should belong to selected "node"
And validate "operational date" of movement should be within selected date range
And source system of movements should not be equal to TRUE
And movements must be displayed ascendingly by operational date
Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |

@testcase=28226 @ui
Scenario Outline: Validate node inventory quality detail without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I click on "InventoryQuality" "tab"
Then validate "node" of "inventory" should belong to selected "node"
And validate "operational date" of "inventory" should be greater than or equal to the initial date of the period minus one day and less than or equal to the end date of the period
And inventories must be displayed ascendingly by inventory date
Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |


@testcase=28227 @ui
Scenario Outline: Verify export to excel functionality without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "NodeName" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
And I click on "Export to Excel" "link"
Then report should be exported to excel with proper columns and data
When I click on "InventoryDetails" "tab"
Then report should be exported to excel with proper columns and data
When I click on "MovementDetails" "tab"
Then report should be exported to excel with proper columns and data
When I click on "InventoryQuality" "tab"
Then report should be exported to excel with proper columns and data
When I click on "MovementQuality" "tab"
Then report should be exported to excel with proper columns and data
Examples:
| Category | Element        |
| Segmento | SegmentElement |
| Sistema  | SystemElement  |
