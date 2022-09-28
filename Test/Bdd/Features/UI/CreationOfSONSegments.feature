@owner=jagudelos @ui @testsuite=55119 @testplan=55104 @S14 @MVP2and3
Feature: CreationOfSONSegments
In order to allow active segments to run the cutoff, ownership calculation, and operational deltas processes
As an Administrator user
I need to configure SON segments

@testcase=57672
Scenario Outline: Verify that no user other than Admin has access to the SON Segment configuration page
Given I am logged in as "<User>"
Then I should or should not see "Segment configuration in TRUE as SON" page based on user "<User>"
Examples:
| User        |
| admin       |
| audit       |
| aprobador   |
| profesional |
| programador |
| consulta    |
@testcase=57673
Scenario: Verify breadcrumb for SON Segment configuration page
Given I am logged in as "admin"
When I navigate to "Segment configuration in TRUE as SON" page
Then I should see breadcrumb "Configuración de segmentos en TRUE como SON"
@testcase=57674 @BVT2
Scenario: Verify Chain and SON segments are displayed in alphabetical order on SON Segment configuration page
Given I have "2" active "chain" segments available
And I have "2" active "son" segments available
And I am logged in as "admin"
When I navigate to "Segment configuration in TRUE as SON" page
Then I validate that only expected segments are listed inside the "chain" segments list and that they are in alphabetical order
And I validate that only expected segments are listed inside the "son" segments list and that they are in alphabetical order
When I click on "segments" "target" "moveAll" "button"
Then I should see "segments" "submit" "button" as disabled
@testcase=57675 @BVT2
Scenario: Verify the user is able to move a segment from Chain segments list to SON segments list
Given I have "2" active "chain" segments available
And I am logged in as "admin"
When I navigate to "Segment configuration in TRUE as SON" page
And I move "1" segments from "chain-son" list by searching in "segments" "source" "search" "textbox" and by clicking on "segments" "source" "move" "button"
Then I validate that "1" moved segments are "not available" in "chain" segments list by searching in "segments" "source" "search" "textbox"
And I validate that "1" moved segments are "available" in "son" segments list by searching in "segments" "target" "search" "textbox"
@testcase=57676
Scenario: Verify the user is able to move a segment from SON segments list to Chain segments list
Given I have "2" active "son" segments available
And I am logged in as "admin"
When I navigate to "Segment configuration in TRUE as SON" page
And I move "1" segments from "son-chain" list by searching in "segments" "target" "search" "textbox" and by clicking on "segments" "target" "move" "button"
Then I validate that "1" moved segments are "available" in "chain" segments list by searching in "segments" "source" "search" "textbox"
And I validate that "1" moved segments are "not available" in "son" segments list by searching in "segments" "target" "search" "textbox"
@testcase=57677
Scenario: Verify the user is able to move all Chain segments to SON segments list
Given I have "1" active "chain" segments available
And I am logged in as "admin"
When I navigate to "Segment configuration in TRUE as SON" page
And I click on "segments" "source" "moveAll" "button"
Then I validate that "1" moved segments are "not available" in "chain" segments list by searching in "segments" "source" "search" "textbox"
And I validate that "1" moved segments are "available" in "son" segments list by searching in "segments" "target" "search" "textbox"
@testcase=57678
Scenario: Verify the user is able to move all active SON segments to Chain segments list
Given I have "1" active "son" segments available
And I am logged in as "admin"
When I navigate to "Segment configuration in TRUE as SON" page
And I click on "segments" "target" "moveAll" "button"
Then I validate that "1" moved segments are "available" in "chain" segments list by searching in "segments" "source" "search" "textbox"
And I validate that "1" moved segments are "not available" in "son" segments list by searching in "segments" "target" "search" "textbox"
And I should see "segments" "submit" "button" as disabled
@testcase=57679
Scenario: Verify the filter functionality in Chain and SON segments lists
Given I have "2" active "chain" segments available
And I have "2" active "son" segments available
And I am logged in as "admin"
When I navigate to "Segment configuration in TRUE as SON" page
Then I validate the filter functionality of "partial" text for "chain" segments by searching in "segments" "source" "search" "textbox"
And I validate the filter functionality of "complete" text for "chain" segments by searching in "segments" "source" "search" "textbox"
And I validate the filter functionality of "partial" text for "son" segments by searching in "segments" "target" "search" "textbox"
And I validate the filter functionality of "complete" text for "son" segments by searching in "segments" "target" "search" "textbox"
@testcase=57680 @BVT2
Scenario: Verify that the Save button is disabled when there are no active SON segments on the SON Segment configuration page
Given I am logged in as "admin"
And I have "2" active "son" segments available
When I navigate to "Segment configuration in TRUE as SON" page
And I click on "segments" "target" "moveAll" "button"
Then I should see "segments" "submit" "button" as disabled

@testcase=57681 @manual
Scenario: Verify the SON configuration failure error message
Given I am logged in as "admin"
And I have "1" active "chain" segments available
When I navigate to "Segment configuration in TRUE as SON" page
And I move "1" segments from "chain-son" list by searching in "segments" "source" "search" "textbox" and by clicking on "segments" "source" "move" "button"
And I put the browser in offline mode using the Network tab from element inspection section
And I click on "segments" "submit" "button"
Then I should see error message "Se presentó un error inesperado durante el proceso de guardar la configuración de los segmentos SON. Por favor intente de nuevo."
@testcase=57682
Scenario: Verify that the user is successfully able to configure a Chain segment as a SON segment
Given I am logged in as "admin"
And I have "3" active "chain" segments available
When I navigate to "Segment configuration in TRUE as SON" page
And I move "2" segments from "chain-son" list by searching in "segments" "source" "search" "textbox" and by clicking on "segments" "source" "move" "button"
And I click on "segments" "submit" "button"
Then I validate that "2" configured segments are updated as "son segment" in the Db
@testcase=57683 @BVT2
Scenario: Verify that the user is successfully able to configure a SON segment as a Chain segment
Given I am logged in as "admin"
And I have "3" active "son" segments available
When I navigate to "Segment configuration in TRUE as SON" page
And I move "2" segments from "son-chain" list by searching in "segments" "target" "search" "textbox" and by clicking on "segments" "target" "move" "button"
And I click on "segments" "submit" "button"
Then I validate that "2" configured segments are updated as "chain segment" in the Db
@testcase=57684
Scenario: Verify that only SON segments are listed in the initial step of the operational cutoff wizard
Given I am logged in as "admin"
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
Then I should see "Start" "link"
And I validate that all the segments from "InitTicket" "Segment" "combobox" dropdown are active SON segments
@testcase=57685
Scenario: Verify that only SON segments are listed in the criteria step of the ownership calculation wizard
Given I am logged in as "admin"
When I navigate to "Property determination by segment" page
And I click on "NewBalance" "button"
Then I validate that all the segments from "OwnershipCal" "Segment" "dropdown" dropdown are active SON segments
@testcase=57686
Scenario: Verify that only SON segments are listed during selection of filters to generate the operational balance with or without ownership report
Given I am logged in as "admin"
When I navigate to "Operating balance with or without property" page
And I select "Segmento" from "category" combo box in "reportFilter" grid
Then I validate that all the segments from "reportFilter" "element" "dropdown" dropdown are active SON segments
@testcase=57687
Scenario: Verify that only SON segments are listed when user is on the page to select the filters to generate node status report
Given I am logged in as "admin"
When I navigate to "Node States" page
Then I validate that all the segments from "nodeFilter" "element" "dropdown" dropdown are active SON segments
@testcase=57688 
Scenario: Verify that only SON segments are listed when user is on the page to select the filters to generate control chart report
Given I am logged in as "admin"
When I navigate to "Control letter" page
Then I validate that all the segments from "nodeFilter" "element" "dropdown" dropdown are active SON segments
