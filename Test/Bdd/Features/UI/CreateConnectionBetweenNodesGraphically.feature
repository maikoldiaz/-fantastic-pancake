@sharedsteps=4013 @owner=jagudelos @ui @testsuite=31117 @testplan=31102
Feature: CreateConnectionBetweenNodesGraphically
As an application administrator
I need to graphically create a connection between nodes to add it to the current network configuration

Background: Login
Given I am logged in as "admin"

@testcase=32746 @bvt
Scenario: Verify the create connection graphically
Given I have data configured for "segment" network
When I select all the required fields
And I click on "ToShow" "button"
And I select the output of an active node and drag and drop it into the input of an active node
And I see confirmation dialog with accept and cancel options
And I click on "Accept" "button"
Then I should see a connection created between the two nodes
@testcase=32747
Scenario: Verify the cancel functionality of create connection
Given I have data configured for "segment" network
When I select all the required fields
And I click on "ToShow" "button"
And I select the output of an active node and drag and drop it into the input of an active node
And I see confirmation dialog with accept and cancel options
And I click on "Cancel" "button"
Then I should not see a connection created between the two nodes
@testcase=32748
Scenario: Verify the concatenation of the description field
Given I have data configured for "segment" network
When I select all the required fields
And I click on "ToShow" "button"
And I select the output of an active node and drag and drop it into the input of an active node
Then I should see the concatenation of the source and destination node is sent as a connection creation service parameter in the description variable

@testcase=32749 @bvt
Scenario: Verify the graphical representation of unsaved connection firmly
Given I have data configured for "segment" network
When I select all the required fields
And I click on "ToShow" "button"
And I select the output of an active node and drag and drop it into the input of an active node
And the connection has not been saved in db
Then I should see the connection according to the graphic standard defined for unsaved connection

@testcase=32750 @bvt
Scenario: Verify the graphical representation of saved connection
Given I have data configured for "segment" network
When I select all the required fields
And I click on "ToShow" "button"
And I select the output of an active node and drag and drop it into the input of an active node
And the connection has been saved in db
Then I should see the connection according to the graphic standard defined for saved connection
@testcase=32751 
Scenario: Verify the connection creation failure
Given I have data configured for "segment" network
When I select all the required fields
And I click on "ToShow" "button"
And I select the output of an active node and drag and drop it into the input of an active node
And I get an error when saving a connection
And I see an acceptance dialog with the error message
And I click on "Accept" "button"
Then I should not see a connection created between the two nodes
