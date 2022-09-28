@sharedsteps=1658 @owner=jagudelos @ui @testsuite=31118 @testplan=31102
Feature: ReportOfEventsAndContractsToViewInformationRegisteredInTrue
	As a Professional User of Segment Balance, 
	I need a report of events and contracts 
	to view the information registered in TRUE

Background: Login
Given I am logged in as "profesional"

@testcase=32795 @version=2
Scenario: Validate initial page load
	When I navigate to "Purchases Sales and Events PPA" page
	Then I should see Segment list with all "Automation" category elements
	And I should see Node control with "Todos" option
	And I should see "nodeFilter" "initialDate" "date" 
	And I should see "nodeFilter" "finalDate" "date" 

@testcase=32796 @version=2
Scenario: Validate node search
	When I navigate to "Purchases Sales and Events PPA" page
	And I select "Transporte" from "segment"
	And I enter "automation_" into node
	Then I should see all the node values of the selected segment
	
@testcase=32797 @version=2
Scenario: Validate mandatory fields
	When I navigate to "Purchases Sales and Events PPA" page
	And I click on "nodeFilter" "viewReport" "button"
	Then I should see the message on interface "Requerido"

@testcase=32798 @version=2
Scenario: Validate error when start date greater than end date
	When I navigate to "Purchases Sales and Events PPA" page
	And I select "Transporte" from "segment"
	And I enter "Todos" into node
	And I enter date "03/01/2020" into "nodeFilter" "initialDate" "date"
	And I enter date "02/02/2020" into "nodeFilter" "finalDate" "date"
	And I click on "nodeFilter" "viewReport" "button"
	Then I should see "La fecha inicial debe ser menor o igual a la fecha final" message

@testcase=32799 @manual
Scenario: View report by segment
	When I navigate to "Purchases Sales and Events PPA" page
	And I select "category element" from "segment"
	And I select "Todos" from "node"
	And I select Initial date 
	And I select final date
	Then I should see report with "Compras, Ventas y Eventos PPA" "Segment name" "From date" "To date" title
	And I should see "Buy and Sell" sheet 
	And I should see purchase and sale contracts with selected initial and final dates 
	And I should see data in ascending order by source node and initial date
	And I should see tiles with contract data

@testcase=32800 @manual
Scenario: View report by node
	When I navigate to "Purchases Sales and Events PPA" page
	And I select "category element" from "segment"
	And I select "node name" from "node"
	And I select Initial date 
	And I select final date
	Then I should see report with "Compras, Ventas y Eventos PPA" "Segment name" "Node name" "From date" "To date" title
	And I should see "Buy and Sell" sheet 
	And I should see purchase and sale contracts with selected initial and final dates 
	And I should see data in ascending order by source node and initial date
	And I should see tiles with contract data

@testcase=32801 @manual
Scenario: Validate data of PPA events sheet by segment
	When I navigate to "Purchases Sales and Events PPA" page
	And I select "category element" from "segment"
	And I select "Todos" from "node"
	And I select Initial date 
	And I select final date
	Then I should see report with "Compras, Ventas y Eventos PPA" "Segment name" "From date" "To date" title
	And I select "Eventos PPA" sheet
	And I should see events with selected initial and final dates 
	And I should see data in ascending order by source node and initial date
	And I should see tiles with event data
	
@testcase=32802 @manual
Scenario: Validate data of PPA events sheet by node
	When I navigate to "Purchases Sales and Events PPA" page
	And I select "category element" from "segment"
	And I select "node name" from "node"
	And I select Initial date 
	And I select final date
	Then I should see report with "Compras, Ventas y Eventos PPA" "Segment name" "Node name" "From date" "To date" title
	And I select "Eventos PPA" sheet
	And I should see events with selected initial and final dates 
	And I should see data in ascending order by source node and initial date
	And I should see tiles with event data

@testcase=32803 @manual
Scenario: Validate report with template data configured
	And I have template data configured
	When I navigate to "Purchases Sales and Events PPA" page
	And I select "category element" from "segment"
	And I select "Todos" from "node"
	And I select Initial date 
	And I select final date
	Then I should see template pages with data

@testcase=32804 @manual
Scenario: Validate report with no template data configured
	And I have no template data configured 
	When I navigate to "Purchases Sales and Events PPA" page
	And I select "category element" from "segment"
	And I select "Todos" from "node"
	And I select Initial date 
	And I select final date
	Then I should not see any template pages with data