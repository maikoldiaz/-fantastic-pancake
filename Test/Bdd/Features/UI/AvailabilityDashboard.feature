@owner=jagudelos @ui @manual @testsuite=70817 @testplan=70526 @S16
Feature: Availability Dashboard
Availability Dashboard section has the availability tests that must be configured and must be published in an availability dashboard
@testcase=72823
Scenario: Verify Availability Test for Portal Management features
Given I am logged in Azure Portal
When I Navigate to "Application Insights"
And I Navigate to "Availability"
And I click on "TrueAdmin"
Then Verify Availability Test results are displayed and count increase every 5 mintues
@testcase=72824
Scenario: Verify Availability Test for Portal Approval features
Given I am logged in Azure Portal
When I Navigate to "Application Insights"
And I Navigate to "Availability"
And I click on "TrueApprovals"
Then Verify Availability Test results are displayed and count increase every 5 mintues
@testcase=72825
Scenario: Verify Availability Test for Portal File Loading features
Given I am logged in Azure Portal
When I Navigate to "Application Insights"
And I Navigate to "Availability"
And I click on "TrueLoadFiles_Transport"
Then Verify Availability Test results are displayed and count increase every 5 mintues
@testcase=72826
Scenario: Verify Availability Test for Operative CutOff
Given I am logged in Azure Portal
When I Navigate to "Application Insights"
And I Navigate to "Availability"
And I click on "TrueCutOff"
Then Verify Availability Test results are displayed and count increase every 5 mintues
@testcase=72827
Scenario: Verify Availability Test for TrueOperativeDeltas
Given I am logged in Azure Portal
When I Navigate to "Application Insights"
And I Navigate to "Availability"
And I click on "TrueOperativeDeltas"
Then Verify Availability Test results are displayed and count increase every 5 mintues
@testcase=72828
Scenario: Verify Availability Test for ownership calculation
Given I am logged in Azure Portal
When I Navigate to "Application Insights"
And I Navigate to "Availability"
And I click on "TrueOwnership"
Then Verify Availability Test results are displayed and count increase every 5 mintues
@testcase=72829
Scenario: Verify Availability Test for ownership adjustments
Given I am logged in Azure Portal
When I Navigate to "Application Insights"
And I Navigate to "Availability"
And I click on "TrueOwnershipAdj"
Then Verify Availability Test results are displayed and count increase every 5 mintues
@testcase=72830
Scenario: Verify Availability Test for portal files loading  features
Given I am logged in Azure Portal
When I Navigate to "Application Insights"
And I Navigate to "Availability"
And I click on "TrueLoadFiles_Chain"
Then Verify Availability Test results are displayed and count increase every 5 mintues
@testcase=72831
Scenario: Verify Availability Test for official delta calculation
Given I am logged in Azure Portal
When I Navigate to "Application Insights"
And I Navigate to "Availability"
And I click on "TrueOfficialDeltas"
Then Verify Availability Test results are displayed and count increase every 5 mintues
@testcase=72832
Scenario: Verify Availability Test for SAP PO API
Given I am logged in Azure Portal
When I Navigate to "Application Insights"
And I Navigate to "Availability"
And I click on "TrueSAP_POAPI"
Then Verify Availability Test results are displayed and count increase every 5 mintues
@testcase=72833
Scenario: Verify Availability Test for reports enabled at portal
Given I am logged in Azure Portal
When I Navigate to "Application Insights"
And I Navigate to "Availability"
And I click on "TrueReports"
Then Verify Availability Test results are displayed and count increase every 5 mintues
@testcase=72834
Scenario: Verify if Availability test failure results are logged
Given I am logged in Azure Portal
When I navigate to function App
And Configure Application Chaos Variable as "true"
Then availablity test for that function should fail
@testcase=72835 
Scenario: Verify failure results are logged in Application Insights
Given I am logged in Azure Portal
When I Navigate to "Application Insights"
And I search "Availability Status Failed"
And I open the Latest Exception Logged
Then Verify that Error message is logged in FormattedMessage
