@sharedsteps=4013 @owner=jagudelos @ui @testplan=55104 @testsuite=55115 @S14 @MVP2and3 @parallel=false
Feature: TransferPoint
	As a TRUE system, I need to inform SAP PO that a transfer point 
	movement has been recorded in order for SAP PO to determine which is the official point

Background: Login
Given I am logged in as "admin"

@manual @testcase=57743
Scenario: To verify the failed transfer point movement report received from an Excel file
Given user upload a "excelFile" with a movement of the operative scenario
And movement belong to "SONsegment" and event type "Insert" or "Update"
And movement has a source and destination nodes belongs to different segments
And true application store the "movement" successfully
When send the "transferpoint" movement by consuming the SAP PO Web API
And verify the Web API returns a status code other than "200" after retries
Then verify application must update the "movement" and stores the "errordescription" by the SAP PO Web API

@manual @testcase=57744
Scenario: To verify the failed transfer point movement report received from the Movements Web API
Given application receives a movement from the Movements Web API
And movement belong to "SONsegment" and event type "Insert" or "Update"
And movement has a source and destination nodes belongs to different segments
And true application store the "movement" successfully
When send the "transferpoint" movement by consuming the SAP PO Web API
And verify the Web API returns a status code other than "200" after retries
Then verify application must update the "movement" and stores the "errordescription" by the SAP PO Web API

@manual @testcase=57745
Scenario: To verify transfer point movement report received from an Excel file
Given user upload a "excelFile" with a movement of the operative scenario
And movement belong to "SONsegment" and event type "Insert"
And movement has a source and destination nodes belongs to different segments
And true application store the "movement" successfully
When send the "transferpoint" movement by consuming the SAP PO Web API
And verify the Web API returns a status code "200"
Then verify application must update the "movement" as a transfer point sent to SAP PO Web API and stores the send "DateTime"

@manual @testcase=57746
Scenario: To verify the new transfer point movement report received from the Movements Web API
Given application receives a movement from the Movements Web API
And movement belong to "SONsegment" and event type "Insert"
And movement has a source and destination nodes belongs to different segments
And true application store the "movement" successfully
When send the "transferpoint" movement by consuming the SAP PO Web API
And verify the Web API returns a status code "200"
Then verify application must update the "movement" as a transfer point sent to SAP PO Web API and stores the send "DateTime"

@manual @testcase=57747
Scenario: To verify the updated transfer point movement report received from an Excel file
Given user upload a "excelFile" with a movement of the operative scenario
And movement belong to "SONsegment" and event type "update"
And movement has a source and destination nodes belongs to different segments
And true application store the "movement" successfully
When user send the "updated" "transferpoint" movement by consuming the SAP PO Web API
And verify the Web API returns a status code "200"
Then verify application must mark as a transfer point sent to SAP PO, the updated movement and the movement with negative volume, and store for the two movements the same send date-time

@manual @testcase=57748
Scenario: To verify the updated transfer point movement report received from the Movements Web API
Given application receives a movement from the Movements Web API
And movement belong to "SONsegment" and event type "update"
And movement has a source and destination nodes belongs to different segments
And true application store the "movement" successfully
When user send the "updated" "transferpoint" movement by consuming the SAP PO Web API
And verify the Web API returns a status code "200"
Then verify application must mark as a transfer point sent to SAP PO, the updated movement and the movement with negative volume, and store for the two movements the same send date-time

@manual @testcase=57749
Scenario: To verify the transfer point of the official scenario
Given verify application receives a transfer point movement belongs to the official scenario and to an "SONsegment" through "excelFile" upload or from the Movements Web API
When verify application stores the "movement" successfully.
Then verify movement should not be reported to SAP PO as a transfer point.

@manual @testcase=57750
Scenario: To verify the transfer point from different segments to SON segments
Given Application receives a transfer point movement of the official scenario from upload a "excelFile" or from the Movements Web API
And verify transfer point movement is not belongs to an "SONsegment"
When verify application stores the "movement" successfully.
Then verify movement should not be reported to SAP PO as a transfer point.

@manual @testcase=57751
Scenario: To verify the movements records with delete event type
Given application receives a transfer point movement from the Movements Web API or from upload "ExcelFile" which belongs to the official scenario
And verify transfer point movement belong to "SONsegment" and event type "delete"
And true application store the "movement" successfully
Then verify movement should not be reported to SAP PO as a transfer point.

@manual @testcase=57752 
Scenario: To verify the transfer point report retries
Given application consumes the SAP PO Web API to report a transfer point
When The SAP PO Web API returns a status code different to "200"
Then the true application must perform upto "6" retries


Scenario: Verify SAP PO got information about a new movement from a transfer point so SAP PO determines which is the official point when inserted a movement through Excel system
When I have inserted both operative and official movements to verify whether SAP Po got required information
Then application must mark the operative movement as a transfer point sent to SAP PO
And inserted official movements should not be reported to SAP PO as a transfer point
When I have updated both operative and official movements to verify whether SAP Po got required information
Then updated official movements should not be reported to SAP PO as a transfer point
And application must mark the updated operative movement as a transfer point sent to SAP PO
When I have deleted movements to verify whether SAP Po got required information
Then these movements should not be reported to SAP PO as a transfer point


Scenario: Verify SAP PO got information about a new movement from a transfer point so SAP PO determines which is the official point when inserted a movement through Sap system
When I have inserted both operative and official movements to verify whether SAP Po got required information using Sap system
Then application must mark the sap inserted operative movement as a transfer point sent to SAP PO
And sap inserted official movements should not be reported to SAP PO as a transfer point
When I have "updated" both sap inserted operative and official movements to verify whether SAP Po got required information
Then sap updated official movements should not be reported to SAP PO as a transfer point
And application must mark the sap updated operative movement as a transfer point sent to SAP PO
When I have "deleted" both sap inserted operative and official movements to verify whether SAP Po got required information
Then sap inserted movements should not be reported to SAP PO as a transfer point