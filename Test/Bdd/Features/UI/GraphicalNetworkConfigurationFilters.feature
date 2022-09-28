@sharedsteps=4013 @owner=jagudelos @ui @testplan=31102 @testsuite=31114 @parallel=false
Feature: GraphicalNetworkConfigurationFilters
In order to view the graphical configuration process of the network
As an administrator
I need a UI with segment, system and node filters

Background: Login
Given I am logged in as "admin"

@testcase=41403 @version=2
Scenario: Verify the list of fields available in the Graphic Network Configuration Menu Item
When I navigate to "Graphic Network Configuration" page
Then I should see the "category" dropdown
And I should see the "element" dropdown
And I should see the Node textbox
And I  see the "Show" label on "nodeFilter" "viewReport" "button"

@testcase=33874 @version=2
Scenario Outline: Verify that when user select category system must update the Element list with the options that belong to the selected category
And I have data configured for "<Category_Options>"
When I navigate to "Graphic Network Configuration" page
And I enter "<Category_Options>" in the Category dropdown
Then I should see the values in Element according to the selected Category_Options

Examples:
| Category_Options |
| Segment          |
| System           |

@testcase=33875 @version=2
Scenario: Verify that user should see the node list that match with the value typed and that belong to the element selected in the “Element” list
And I have data configured for "Segment"
When I navigate to "Graphic Network Configuration" page
And I enter "Segment" in the Category dropdown
And I select category element value from dropdown
And I enter "NodeName" into Node Texbox
And I click on "nodeFilter" "viewReport" "button"

@testcase=33876 @version=2 @bvt1.5
Scenario: Verify that system must display the Required message below each field if clicked on show button without entering any value
When I navigate to "Graphic Network Configuration" page
And I click on "nodeFilter" "viewReport" "button"
Then I should see the "Requerido" Message below each field

@testcase=33877 @version=2
Scenario Outline: Verify that system should display the nodes and connections of the selected segment or system if Todos is Enter
And I have data configured for "<Category_Options>"
When I navigate to "Graphic Network Configuration" page
And I enter "<Category_Options>" in the Category dropdown
And I select category element value from dropdown
And I enter "Todos" into Node Texbox
And I click on "nodeFilter" "viewReport" "button"
Then I should see the nodes and Connections

Examples:
| Category_Options |
| Segment          |
| System           |

@testcase=41404 @version=2 @bvt1.5
Scenario: Verify that system should display the nodes in "Graphic Network Configuration" Page
And I have data configured for "Segment"
When I navigate to "Graphic Network Configuration" page
And I enter "Segment" in the Category dropdown
And I select category element value from dropdown
And I enter "NodeName" into Node Texbox
And I click on "nodeFilter" "viewReport" "button"
Then I should see the selected node in the graphical network page
