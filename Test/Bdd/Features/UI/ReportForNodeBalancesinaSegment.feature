@sharedsteps=72649 @owner=jagudelos @testplan=70526 @testsuite=70812 @manual
Feature: ReportForNodeBalancesinaSegment
As a True Query User,
I need a new sheet to be included in the operative report with ownership to have an overview of the node balances in a segment or system

Background: Login
Given I am logged in as "admin"
@testcase=72650
Scenario: Verify Operational balance with ownership by System for all nodes.
Given I have ownership calculation data generated in the system
When I navigate to "Operating balance with ownership" page
And I select "Category" System  from "Category"
And I select "Element" from "Element"
And I select "Todos" from "Node"
And I click on "NodeFilter" "ViewReport" "button"
And I should see the report with Nodes belonging to System for the given date period
Then I click on Resumen nodos tab
And I should see the message at the top 'Para ver el detalle haga clic sobre el nombre del nodo'
And I should see balance calculated for the given period
And I should see the Resumen nodos/Summary Nodes'
And I see the columns Nodo, Propoetario, Inv. inicial,Salidas, Pérdidas identificadas, Interfases, Tolerancia, Pérdidas no identificadas, Inv. final, Control on Resumen nodos page
@testcase=72651
Scenario: Verify Operational balance with ownership by Segment for all nodes.
Given I have ownership calculation data generated in the system
When I navigate to "Operating balance with ownership" page
And I select "Category" Segment  from "Category"
And I select "Element" from "Element"
And I select "Todos" from "Node"
And I click on "NodeFilter" "ViewReport" "button"
And I should see the report with Nodes belonging to Segment for the given date period
Then I click on Resumen nodos tab
And I should see the message at the top 'Para ver el detalle haga clic sobre el nombre del nodo'
And I should see balance calculated for the given period
And I should see the Resumen nodos/Summary Nodes'
And I see the columns Nodo, Propoetario, Inv. inicial,Salidas, Pérdidas identificadas, Interfases, Tolerancia, Pérdidas no identificadas, Inv. final, Control on Resumen nodos page
@testcase=72652
Scenario: Verify Operational balance with ownership by system for a single node
Given I have ownership calculation data generated in the system
When I navigate to "Operating balance with ownership" page
And I select "Category" system  from "Category"
And I select "Element" from "Element"
And I select "Node" from "Node"
And I click on "NodeFilter" "ViewReport" "button"
And I should see the report with Nodes belonging to System for the given date period
Then validate Resume Nodo tab is not visible
@testcase=72653
Scenario: Verify Operational balance with ownership by Segment for a single node
Given I have ownership calculation data generated in the system
When I navigate to "Operating balance with ownership" page
And I select "Category" Segment  from "Category"
And I select "Element" from "Element"
And I select "Node" from "Node"
And I click on "NodeFilter" "ViewReport" "button"
And I should see the report with Nodes belonging to Segment for the given date period
Then validate Resume Nodo tab is not visible
@testcase=72654
Scenario: Verify Operational balance with ownership report by segment and all its nodes when selected filters do not have data
Given I have no ownership calculation data generated in the system
When I navigate to "Operating balance with ownership" page
And I select "Category" segment  from "Category"
And I select "Element" from "Element"
And I select "Todos" from "Node"
And I click on "NodeFilter" "ViewReport" "button"
Then node balance table should not have any data
@testcase=72655
Scenario: Verify Operational balance with ownership report by System and all its nodes when selected filters do not have data
Given I have no ownership calculation data generated in the system
When I navigate to "Operating balance with ownership" page
And I select "Category" system  from "Category"
And I select "Element" from "Element"
And I select "Todos" from "Node"
And I click on "NodeFilter" "ViewReport" "button"
Then node balance table should not have any data
@testcase=72656 
Scenario: Verify new browser tab is opened on clicking on Resumen nodos/Summary Nodes Node
Given I have ownership calculation data generated in the system
When I navigate to "Operating balance with ownership" page
And I select "Category" system  from "Category"
And I select "Element" from "Element"
And I select "Todos" from "Node"
And I click on "NodeFilter" "ViewReport" "button"
Then I click on the Resumen nodo tab
And I Click on any of the Node link
And the report should get opened in new browser tab
And operational balance with ownership report of the selected node, in the selected date period is displayed
