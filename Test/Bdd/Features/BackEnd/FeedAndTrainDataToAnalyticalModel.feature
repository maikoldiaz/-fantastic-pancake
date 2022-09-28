@owner=jagudelos @backend @database @testplan=19772 @testsuite=19780
Feature: FeedAndTrainDataToAnalyticalModel
In order to calculate the property based on historical data
As a TRUE system
I need to deliver the data to feed and train the analytical model

@testcase=21131 @bvt
Scenario: Generation of current operational movement data from the TRUE system to store it in the historical table
Given I need to deliver the data to feed and train the analytical model that calculates the property based on historical and current data
When I obtain the current data from the TRUE system of the operational movements for a given date
Then the generation of operational movements from the TRUE system must be validated and checked in the operational movements table

@testcase=21132 @bvt @ignore
Scenario: Generation of the current data of the operative movements with ownership from the TRUE system to store it in the historical table
Given I need to deliver the data to feed and train the analytical model that calculates the property based on historical and current data
When I obtain the current data from the TRUE system of the operative movements with ownership for a given date
Then the generation of the operative movements with ownership from the TRUE system must be validated and checked in the logistic movements table
