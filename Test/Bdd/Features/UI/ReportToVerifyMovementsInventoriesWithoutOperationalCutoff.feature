@owner=jagudelos @ui @testsuite=26839 @testplan=26817
Feature: ReportToVerifyMovementsInventoriesWithoutOperationalCutoff
In order to view operational report without operational cutoff
As a query user
I want to view the detail of the operational report by system or segment without operational cutoff to verify the data of the movements and inventories

@testcase=28242 @ui
Scenario Outline: Validate segment and system movement detail of a product without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "<Node>" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I Right click on Product and Select "MovementDetails"
Then validate "source or destination node" of movement should belong to "Segment" selected
And validate "source or destination product" of movement should belong to "product" selected
And validate "operational date" of movement should be within date range selected

Examples:
| Category | Element        | Node     |
| Segmento | SegmentElement | NodeName |
| Sistema  | SystemElement  | NodeName |
| Segmento | SegmentElement | Todos    |
| Sistema  | SystemElement  | Todos    |

@testcase=28243 @ui
Scenario Outline: Validate segment and system inventory detail of a product without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "<Node>" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I Right click on Product and Select "InventoryDetails"
Then validate "node" of "inventory" should belong to "Segment" selected
And validate "product" of "inventory" should belong to "product" selected
And validate "operational date" of "inventory" should be within date range selected

Examples:
| Category | Element        | Node     |
| Segmento | SegmentElement | NodeName |
| Sistema  | SystemElement  | NodeName |
| Segmento | SegmentElement | Todos    |
| Sistema  | SystemElement  | Todos    |

@testcase=28244 @ui
Scenario Outline: Validate segment and system movement quality detail of a product without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "<Node>" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I Right click on Product and Select "MovementQuality"
Then validate "source or destination node" of movement should belong to "Segment" selected
And validate "source or destination product" of movement should belong to "product" selected
And validate "operational date" of movement should be within date range selected

Examples:
| Category | Element        | Node     |
| Segmento | SegmentElement | NodeName |
| Sistema  | SystemElement  | NodeName |
| Segmento | SegmentElement | Todos    |
| Sistema  | SystemElement  | Todos    |

@testcase=28245 @ui
Scenario Outline: Validate segment and system inventory quality detail of a product without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "<Node>" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I Right click on Product and Select "InventoryQuality"
Then validate "node" of "inventory" should belong to "Segment" selected
And validate "product" of "inventory" should belong to "product" selected
And validate "operational date" of "inventory" should be within date range selected

Examples:
| Category | Element        | Node     |
| Segmento | SegmentElement | NodeName |
| Sistema  | SystemElement  | NodeName |
| Segmento | SegmentElement | Todos    |
| Sistema  | SystemElement  | Todos    |

@testcase=28246 @ui
Scenario Outline: Validate segment and system movement detail without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "<Node>" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I click on "MovementDetails" "tab"
Then validate "source or destination node" of movement should belong to "Segment" selected
And validate "operational date" of movement should be within date range selected

Examples:
| Category | Element        | Node     |
| Segmento | SegmentElement | NodeName |
| Sistema  | SystemElement  | NodeName |
| Segmento | SegmentElement | Todos    |
| Sistema  | SystemElement  | Todos    |

@testcase=28247 @ui
Scenario Outline: Validate segment and system inventory detail without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "<Node>" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I click on "InventoryDetails" "tab"
Then validate "node" of "inventory" should belong to "Segment" selected
And validate "operational date" of "inventory" should be within date range selected

Examples:
| Category | Element        | Node     |
| Segmento | SegmentElement | NodeName |
| Sistema  | SystemElement  | NodeName |
| Segmento | SegmentElement | Todos    |
| Sistema  | SystemElement  | Todos    |

@testcase=28248 @ui
Scenario Outline: Validate segment and system movement quality detail without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "<Node>" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I click on "MovementQuality" "tab"
Then validate "source or destination node" of movement should belong to "Segment" selected
And validate "operational date" of movement should be within date range selected

Examples:
| Category | Element        | Node     |
| Segmento | SegmentElement | NodeName |
| Sistema  | SystemElement  | NodeName |
| Segmento | SegmentElement | Todos    |
| Sistema  | SystemElement  | Todos    |

@testcase=28249 @ui
Scenario Outline: Validate segment and system inventory quality detail without operational cutoff
Given I have "<Category>" for the Operational Report
When I navigate to "OperativeBalanceReport" page
And I select "<Category>" from "Category"
And I select "<Element>" from "Element"
And I enter "<Node>" into "Node" "Name" "textbox"
And I select the FinalDate lessthan "4" days from CurrentDate on "Report" DatePicker
And I click on "ReportFilter" "ViewReport" "button"
When I click on "InventoryQuality" "tab"
Then validate "node" of "inventory" should belong to "Segment" selected
And validate "operational date" of "inventory" should be within date range selected

Examples:
| Category | Element        | Node     |
| Segmento | SegmentElement | NodeName |
| Sistema  | SystemElement  | NodeName |
| Segmento | SegmentElement | Todos    |
| Sistema  | SystemElement  | Todos    |
