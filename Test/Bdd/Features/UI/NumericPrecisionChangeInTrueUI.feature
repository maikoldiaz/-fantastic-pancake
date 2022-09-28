@owner=jagudelos @ui @testplan=31102 @testsuite=31119
Feature: NumericPrecisionChangeInTrueUI
As a TRUE system, I require that the calculations made in the application be rounded to two decimal places
so that the system is consistent with the other applications in the chain
@testcase=33829
Scenario: Verify that the numeric output fields in the user interface show a maximum of 16 integers and upto 2 decimals
Given I navigate to any page in the user interface
When I view the numeric output fields displayed in the user interface
Then I verify that all the numeric output fields throughout the UI do not exceed 16 integers and 2 decimals
@testcase=33830
Scenario: Verify that the numeric input fields in the user interface accept 16 integers and upto 2 decimals
Given I navigate to any page in the user interface
When I view the numeric output fields displayed in the user interface
And I try to enter valid numeric values in the fields with 16 integers and 2 decimals
Then I verify that all the numeric fields entered in the user interface are accepted without throwing any error
And also verify that we can enter negative numbers when the total digit count does not exceed 20 including decimals
@testcase=33831
Scenario: Verify that the numeric input fields throw a valid error in the User Interface when invalid format is provided for numbers
Given I navigate to any page in the user interface
When I view the numeric input fields displayed in the user interface
Then I try to enter invalid numeric values in the fields with numbers exceeding 16 integers or 2 decimal places or a combination of both
And I verify that proper error message is thrown indicating that the numbers are entered in an incorrect format
@testcase=33832
Scenario: Register a new SINOPER inventory with invalid numeric data
Given I am authenticated as "admin"
Given I want to register an "Movements" in the system
When it meets all input validations
And we set the "Criterion" value in the xml to "118020.435"
And the "EventType" field is equal to "Insert"
Then it should be registered
Then it must be stored in a Pendingtransactions repository with validation "Invalid data entered for numeric format"
@testcase=33833
Scenario: Register a new SINOPER Movement with invalid decimal count
Given I am authenticated as "admin"
Given I want to register an "Movements" in the system
When it meets all input validations
And we set the "VALUE" field in the xml to "118020.435"
And the "EventType" field is equal to "Insert"
Then it should be registered
And the decimal count should be rounded off to 2 decimals in the database
And decimals should be rounded off to 2 decimals in block chain
@testcase=33834
Scenario: Register a new SINOPER Movement with invalid numeric data
Given I want to register an "Movements" in the system
And I have data coming from SINOPER
And I have invalid numeric data in the movement with numbers having more than 16 integers
When it meets all input validations
And the "EventType" field is equal to "Insert"
Then an error should be thrown indicating that the numbers entered are in incorrect format
@testcase=33835
Scenario: Register a new EXCEL Movement with invalid numeric data
Given I want to register a "Homologation" in the system
And I have incorrect integer counts for the numeric data exceeding 16 integers
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select segment from "FileUpload" "segment" "dropdown"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then a proper error message should be displayed indicating that there was an error in the numeric data

@testcase=33836
Scenario: Register a new EXCEL Movement with invalid decimal count in numeric data
Given I want to register a "Homologation" in the system
And I have incorrect decimal counts for the numeric data with decimal counts more than 2
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select segment from "FileUpload" "segment" "dropdown"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then it should be registered in the system
And the decimal count should be rounded off to 2 decimals in the database
And decimals should be rounded off to 2 decimals in block chain

@testcase=33837
Scenario: Verify that the numeric fields in the Power BI reports are in proper format
Given I generate any report in the User Interface
When I view the number formatting in the PowerBI reports
Then I need to verify that all the numbers are in proper format with a maximum of 16 integers and 2 decimals
@testcase=33838
Scenario: Verify that numeric data is displayed properly when there are unbalances greater than acceptable balance percentage
Given I want to calculate the "OperationalBalance" in the system
When I navigate to "Operational Cutoff" page
And I click on "NewCut" "button"
And I choose CategoryElement from "InitTicket" "Segment" "combobox"
And I select the FinalDate lessthan "2" days from CurrentDate on "Cutoff" DatePicker
And I click on "InitTicket" "submit" "button"
And I select all pending records from grid
And I click on "ErrorsGrid" "AddNote" "button"
And I should see "Add Note Functions" interface
And I enter valid value into "AddComment" "Comment" "textbox"
And I click on "AddComment" "Submit" "button"
Then validate that "ErrorsGrid" "Submit" "button" as enabled
And I click on "ErrorsGrid" "Submit" "button"
And I should see unbalances of the nodes that exceed the "Acceptable Balance Percentage" value configured for each node
And I should see all the column data related to it
And I can verify that all the numeric fields are properly displayed and all the decimal values in the output are rounded off to 2 decimals
@testcase=33839 
Scenario: Verify that number formatting for any Excel file exported for SIV/FICO Transitional model
Given I have data with logistic center information
When I navigate to "Logistic Report Generation" page
And I click on "CreateLogistics" "button"
And I should see "CreateLogistics" "Create" "Interface"
And I selected Segment from "Segment" "CreateLogistics" "combobox"
And I select Owner on the Create file interface
And I select Start date and End Date on Create file Interface
And I click on "CreateLogistics" "Submit" "button"
Then I should see Logistic Report for selected segment in the Logistic Report Generation page
And I need to view that all the numbers are in proper format with a maximum of 16 integers and 2 decimals
