@sharedsteps=4013 @owner=jagudelos @ui @testsuite=55118 @testplan=55104
Feature: DataConfigurationPerTransferPointToTRUE
As a TRUE system, I need to consume a SAP PO Web API to deliver the official data configuration per transfer point to TRUE
@testcase=57689
Scenario: To verify Contract of Web API Official Transfer Points
Given Application need to consume an SAP PO Web API to deliver the official data configuration per transfer point
When Request is sent to SAP web API
Then verify autentication is done using the securely stored "Username" and "password"
And Make a request to the  service without parameters
And Interpret the service response based on the fields and data types defined in the contract.
And verify fields are Ignored submitted when they are not part of the contract definition

@testcase=57690 @manual
Scenario: To verify Periodic synchronity
Given Application need to consume an SAP PO Web API to deliver the official data configuration per transfer point
When data is used to consult in reports
Then Verify information is automatically synchronize between SAP PO and TRUE every hour using the Official Data Configuration Web API per transfer point
And Verify information is shown used in reports one hour late regarding the source of the information

@testcase=57691 @manual
Scenario: To verify technical errors handling
Given Application need to consume an SAP PO Web API to deliver the official data configuration per transfer point
When service is consulted and responds with a different Http response code other than the "200"
Then Verify error is logged to the system

@testcase=57692 @manual
Scenario: To verify technical error reties
Given Application need to consume an SAP PO Web API to deliver the official data configuration per transfer point
When service is consulted and responds with a different Http response code other than the "200"
Then Verify the system should retries to consult the servive for "6" time
And Verify if retries exceed count "6"
And Verify error is logged to the system
