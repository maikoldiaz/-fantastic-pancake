@owner=jagudelos @testplan=24148 @testsuite=24163 @ui @parallel=false
Feature: AdjustmentsToImproveUserExperience
As a Balance Segment Professional User,
I need some adjustments to be made in
the system to improve the user experience


@testcase=25116 @output @bvt @output=QueryAll(GetNodes) @version=2 @bvt1.5
Scenario: Verify TRUE User is able to return to a geneal listing from a specific page
Given I am logged in as "admin"
And I have "nodes" in the system
When I navigate to "ConfigureAttributesNodes" page
When I provide "NodeName" for "nodeAttributes" "Name" "textbox" filter
And I click on "nodeAttributes" "edit" "link"
And I click on "returnListing" "button"

@testcase=25117 @output=QueryAll(GetNodeConnection) @bvt @version=2 @bvt1.5
Scenario: Verify TRUE User is able to return to a geneal listing from node connection attributes
Given I am logged in as "admin"
And I have "node-connection" in the system
When I navigate to "ConfigureAttributesConnections" page
When I provide "SourceNodeName" for "connAttributes" "SourceNode" "Name" "textbox" filter
And I click on "connAttributes" "edit" "link"
And I click on "returnListing" "button"

@testcase=25118 @bvt @version=3 @bvt1.5
Scenario: Verify TRUE User is able to return to a geneal listing from summary page of a cutoff calculation
Given I am logged in as "admin"
When I navigate to "Operational Cutoff" page
And I have filter data
And I click on "Tickets" "viewSummary" "link"
And I click on "returnListing" "button"

@testcase=25119 @version=2 @bvt @bvt1.5
Scenario: Verify TRUE user is not able to select the current association item from the target list
Given I am logged in as "admin"
When I navigate to "nodetags" page
And I click on "NodeTags" "edit" "button"
Then I verify that the source node selected is not in the target list dropdown