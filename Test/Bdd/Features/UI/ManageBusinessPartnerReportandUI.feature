@sharedsteps=72617 @owner=jagudelos @testplan=70526 @testsuite=70810 @manual
Feature: ManageBusinessPartnerReportandUI
As a True User,
I want to manage the Business Partner Structure

Background: Login
Given I am logged in as "admin"
@testcase=72618
Scenario: Validate initial data load of DataPartners.xls in the environment
Given I have DataloadPipeline
And I check the PartnerOwnerMapping table in DB
Then table should have columns GrandOwner and OwnerPartner columns have data populated from DataPartners.xls
@testcase=72619
Scenario: Verify Daily SIV File Processing Modification
Given I have data with segment with official Delta calculated
When I navigate to "Logistic Report Generation" page
And I click on "CreateLogistics" "button"
And I should see "CreateLogistics" "Create" "Interface"
And I selected Segment from "Segment" "CreateLogistics" "combobox"
And I select Owner on the Create file interface
And I search for movements the owner partners associated with the great owner
And I select Start date and End Date on Create file Interface
And I click on "CreateLogistics" "Submit" "button"
Then I should see Logistic Report for selected segment in the Logistic Report Generation page
And I should see additional level of grouping in the official balance detail table by node to group the grand owner
@testcase=72620
Scenario: Verify Official siv file processing modification​
Given I have data with segment with official Delta calculated
When I navigate to "Logistic Report Generation" page
And I click on "CreateLogistics" "button"
And I should see "CreateLogistics" "Create" "Interface"
And I search for movements the owner partners associated with the great owner chosen
And I selected Segment from "Segment" "CreateLogistics" "combobox"
And I select Owner on the Create file interface
And I select Start date and End Date on Create file Interface
And I click on "CreateLogistics" "Submit" "button"
Then I should see Logistic Report for selected segment in the Logistic Report Generation page
And I should see additional level of grouping in the official balance detail table by node to group the grand owner
@testcase=72621
Scenario: Verify Modification Of Official Balance By Node Report where SourceSystem is ManualMovOficial
Given I have data with segment with ManualMovOficial Delta calculated
When I navigate to "Official balance per node" page
And I click on "segmentoDropdown" "segmento" "dropdown"
And i should see that only active Segmento values
And I click on "nodoDD" "Nodo" "dropdown"
And I should see that only active Nodo values
And I click on "Ano" "dropdown"
And I should see that last 5 years values are displayed in descending order
And I should see current year selected
And I Click on "Perido" "Periodo" "dropdown"
And I should see that periods from selected year are displayed in descending order
And I click on "Ver Reporte" "button"
Then I should see successful report Generation
And I should see additional level of grouping in the official balance detail table by node to group the grand owner
And I should see the selected node from the earlier steps on Logistic Report Generation page
And I should see the report in chosen Period
@testcase=72622
Scenario: Verify Modification Of Official Balance By Node Report where OfficialDeltaMessageTypeId is OfficalMovementDelta
Given I have data with segment with official Delta calculated
When I navigate to "Official balance per node" page
And I click on "segmentoDropdown" "segmento" "dropdown"
And i should see that only active Segmento values
And I click on "nodoDD" "Nodo" "dropdown"
And I should see that only active Nodo values
And I click on "Ano" "dropdown"
And I should see that last 5 years values are displayed in descending order
And I should see current year selected
And I Click on "Perido" "Periodo" "dropdown"
And I should see that periods from selected year are displayed in descending order
And I click on "Ver Reporte" "button"
Then I should see successful report Generation
And I should see additional level of grouping in the official balance detail table by node to group the grand owner
And I should see the selected node from the earlier steps on Logistic Report Generation page
And I should see the report in chosen Period
@testcase=72623 
Scenario: Verify Modification Of Official Balance By Node Report where OfficialDeltaMessageTypeId is ConsolidatedMovementDelta
Given I have data with segment with ManualMovOficial Delta calculated
When I navigate to "Official balance per node" page
And I click on "segmentoDropdown" "segmento" "dropdown"
And i should see that only active Segmento values
And I click on "nodoDD" "Nodo" "dropdown"
And I should see that only active Nodo values
And I click on "Ano" "dropdown"
And I should see that last 5 years values are displayed in descending order
And I should see current year selected
And I Click on "Perido" "Periodo" "dropdown"
And I should see that periods from selected year are displayed in descending order
And I click on "Ver Reporte" "button"
Then I should see successful report Generation
And I should see additional level of grouping in the official balance detail table by node to group the grand owner
And I should see the selected node from the earlier steps on Logistic Report Generation page
And I should see the report in chosen Period
