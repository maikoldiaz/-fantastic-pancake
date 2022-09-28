@sharedsteps=71700 @owner=jagudelos @ui @MVP2and3 @S16 @testsuite=70798 @testplan=70526
Feature: ExtendedFICOProcessForOfficialDelta
As TRUE system, I need to send to FICO the movements originated
by deltas to execute the official deltas calculation

Background: Login
	Given I am logged in as "admin"

@testcase=71701 @version=2 @bvt
Scenario: Verify movements in the FICO collection "deltasOficialesInventarios" when Movement originated by a manual inventory delta for previous period
	Given I have an official movements in the system with movements originated by manual inventory deltas
	And I have Calculate deltas by official adjustment for previous period
	When a node of the segment ticket is approved in the previous period
	And The operational date is equal to the start date of the period minus one day
	And the created date is "less than" the approval date of the node in the previous period
	And I have Calculate deltas by official adjustment for next period
	Then Verify that system should send the official movements details in the "deltasOficialesInventarios" FICO Collection

@testcase=71702 @version=2
Scenario: Verify movements in the FICO collection "deltasOficialesInventarios" when Movement originated by an inventory delta for previous period
	Given I have an official movements in the system with movements originated by an inventory delta
	And I have Calculate deltas by official adjustment for previous period
	When a node of the segment ticket is approved in the previous period
	And The operational date is equal to the start date of the period minus one day
	And the created date is "less than" the approval date of the node in the previous period
	And I have Calculate deltas by official adjustment for next period
	Then Verify that system should send the official movements details in the "deltasOficialesInventarios" FICO Collection

@testcase=71703 @version=2
Scenario: Verify movements in the FICO collection "deltasOficialesInventarios" when Movement originated by an inventory delta and node without approvals for the previous period
	Given I have an official movements in the system with movements originated by manual inventory or an inventory delta
	And I have Calculate deltas by official adjustment for previous period
	When a node of the segment ticket is approved in the previous period
	And a node of the segment ticket is Deltas or Rejected state and has not been previously approved for current period
	And The operational date is equal to the start date of the period minus one day
	And the created date is "greater than" the approval date of the node in the previous period
	And I have Calculate deltas by official adjustment for next period
	Then Verify that system should not send the official movements details in the "deltasOficialesInventarios" FICO Collection

@testcase=71704 @version=2
Scenario: Verify movements in the FICO collection "deltasOficialesInventarios" when Movement originated by an inventory delta and node without approvals for the ticket period
	Given I have an official movements in the system with movements originated by manual inventory or an inventory delta
	And I have Calculate deltas by official adjustment for previous period
	When a node of the segment ticket is approved in the previous period
	And a node of the segment ticket is Deltas or Rejected state and has not been previously approved for current period
	And The operational date is equal to the end date of the period
	And I have Calculate deltas by official adjustment for next period
	Then Verify that system should not send the official movements details in the "deltasOficialesInventarios" FICO Collection

@testcase=71705 @version=2
Scenario: Verify movements in the FICO collection "deltasOficialesInventarios" when Movement originated by an inventory delta and node with approvals for the previous period
	Given I have an official movements in the system with movements originated by manual inventory or an inventory delta
	And I have Calculate deltas by official adjustment for previous period
	When a node of the segment ticket is approved in the previous period
	And a node of the segment ticket is Deltas or Rejected state and has been previously approved or is in reopened state for current period 
	And The operational date is equal to the start date of the period minus one day
	And the created date is "greater than" the approval date of the node in the previous period and less than the last approval date of the node in the ticket period
	And I have Calculate deltas by official adjustment for next period
	Then Verify that system should send the official movements details in the "deltasOficialesInventarios" FICO Collection

@testcase=71706 @version=2
Scenario: Verify movements in the FICO collection "deltasOficialesInventarios" when Movement originated by an inventory delta and node with approvals for the ticket period
	Given I have an official movements in the system with movements originated by manual inventory or an inventory delta
	And I have Calculate deltas by official adjustment for previous period
	When a node of the segment ticket is approved in the previous period
	And a node of the segment ticket is Deltas or Rejected state and has been previously approved or is in reopened state for current period 
	And The operational date is equal to the end date of the period
	And the created date is "less than" the last approval date of the node in the ticket period
	And I have Calculate deltas by official adjustment for next period
	Then Verify that system should send the official movements details in the "deltasOficialesInventarios" FICO Collection

@testcase=71707 @version=2
Scenario: Verify movements in the FICO collection "deltasOficialesInventarios" when Movement originated by an inventory delta and node with approvals for the previous period stored after the node's approval date
	Given I have an official movements in the system with movements originated by manual inventory or an inventory delta
	And I have Calculate deltas by official adjustment for previous period
	When a node of the segment ticket is approved in the previous period
	And The operational date is equal to the start date of the period minus one day
	And the created date is "greater than" the last approval date of the node in the ticket period
	And I have Calculate deltas by official adjustment for next period
	Then Verify that system should not send the official movements details in the "deltasOficialesInventarios" FICO Collection

@testcase=71708 @version=2
Scenario: Verify movements in the FICO collection "deltasOficialesInventarios" when Movement originated by an inventory delta and node with approvals for the ticket period stored after the node's approval date
	Given I have an official movements in the system with movements originated by manual inventory or an inventory delta
	And I have Calculate deltas by official adjustment for previous period
	When a node of the segment ticket is approved in the previous period
	And The operational date is equal to the end date of the period
	And the created date is "greater than" the last approval date of the node in the ticket period
	And I have Calculate deltas by official adjustment for next period
	Then Verify that system should not send the official movements details in the "deltasOficialesInventarios" FICO Collection

@testcase=71709 @version=2
Scenario: Verify movements in the FICO collection "deltasOficialesMovimientos" when Movement originated by a movement delta and node without approvals
	Given I have an official movements in the system with movements originated by manual delta movements or movement deltas
	And I have Calculate deltas by official adjustment for previous period
	When a node of the segment ticket is Deltas or Rejected state and has not been previously approved
	And the start and end dates of the movements are equal to the start and end dates of the period
	And I have Calculate deltas by official adjustment for next period
	Then Verify that system should not send the official movements details in the "deltasOficialesMovimientos" FICO Collection

@testcase=71710 @version=2
Scenario: Verify movements in the FICO collection "deltasOficialesMovimientos" when Movement originated by a movement delta and node with approvals
	Given I have an official movements in the system with movements originated by manual delta movements or movement deltas
	And I have Calculate deltas by official adjustment for previous period
	When a node of the segment ticket is Deltas or Rejected state and has been previously approved or is in reopened state for current period
	And the start and end dates of the movements are equal to the start and end dates of the period
	And the created date is "less than" the last approval date of the node in the ticket period
	And I have Calculate deltas by official adjustment for next period
	Then Verify that system should send the official movements details in the "deltasOficialesMovimientos" FICO Collection

@testcase=71711 @version=2
Scenario: Verify movements in the FICO collection "deltasOficialesMovimientos" when Movement originated by a movement delta and node with approvals with movement stored after the node's approval date
	Given I have an official movements in the system with movements originated by manual delta movements or movement deltas
	And I have Calculate deltas by official adjustment for previous period
	When a node of the segment ticket is approved in the previous period
	And the start and end dates of the movements are equal to the start and end dates of the period
	And the created date is "greater than" the last approval date of the node in the ticket period
	And I have Calculate deltas by official adjustment for next period
	Then Verify that system should not send the official movements details in the "deltasOficialesMovimientos" FICO Collection