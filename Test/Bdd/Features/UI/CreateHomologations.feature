@sharedsteps=4013 @owner=jagudelos  @ui @testplan=19722 @testsuite=19787
Feature: CreateHomologations
As a TRUE System Administrator,
I need an homologation management UI to store data mappings between TRUE and other applications

Background: Login
Given I am logged in as "admin"

@testcase=21185 @bvt
Scenario: Verify TRUE admin user can create Homologation for Data Mapping with TRUE as Destination system
#Given I do not have Homologation group in the system
When I navigate to "Homologation" page
And I click on "createHomologation" "button"
Then I should see "homologation" interface
When I select any "SourceLocation1" from "SourceLocation" "type" "dropdown"
And I select any "DestinationLocation1" from "DestinationLocation" "type" "dropdown"
And I select any "GroupValue1" from "Group" "segment" "dropdown"
And I click on "homologation" "continue" "button"
When I click on "Add objects" "submit" "button"
And I select any "Object" from "Add objects" "type" "dropdown"
And I click on "objectsFlyout" "submit" "button"
When I click on "Add Record" "create" "button"
Then I should see "homologationData" interface
When I provide the value for "OriginData" "sourceValue" "textbox"
And I provide the value for "DestinationData" "name" "list"
And I click on "homologationData" "add" "button"
When I click on "Add Record" "create" "button"
Then I should see "homologationData" interface
When I provide the value for "OriginData" "name" "textbox"
And I provide the value for "DestinationData" "name" "list"
And I click on "homologationData" "add" "button"
Then I should see "Save" "button" as enabled
When I click on "submit" "button"
Then I should see the message on interface "Homologación almacenada correctamente"
And I should see the "Homologation" grid
And it should be registered in the system with entered data
@testcase=21186
Scenario: Verify message when no  Homologation is present
Given I do not have Homologation group in the system
When I navigate to "Homologation" page
Then I should see the message "Sin registros"
@testcase=21187
Scenario: Create Homologation without Mandatory fields
Given I do not have Homologation group in the system
When I navigate to "Homologation" page
And I click on "createHomologation" "button"
And I click on "Continue" "submit" "button"
Then I should see the message on interface "Requerido"
@testcase=21188
Scenario: Verify user can create Homologation for Data Mapping without TRUE as Source or Destination system
Given I do not have Homologation group in the system
When I navigate to "Homologation" page
And I click on "createHomologation" "button"
Then I should see "homologation" interface
When I select any "SourceLocation1" from "SourceLocation" "type" "dropdown"
And I select any "DestinationLocation2" from "DestinationLocation" "type" "dropdown"
And I select any "GroupValue1" from "Group" "segment" "dropdown"
And I click on "Continue" "submit" "button"
Then I should see the message on interface "TRUE debe ser seleccionadocomo sistema origen o como sistema destino"
@testcase=21189
Scenario: Verify TRUE admin user can create Homologation for Data Mapping with TRUE as Source system and validate duplicate Homologation group
Given I do not have Homologation group in the system
When I navigate to "Homologation" page
And I click on "createHomologation" "button"
Then I should see "homologation" interface
When I select any "SourceLocation2" from "SourceLocation" "type" "dropdown"
And I select any "DestinationLocation3" from "DestinationLocation" "type" "dropdown"
And I select any "GroupValue1" from "Group" "segment" "dropdown""
And I click on "Continue" "submit" "button"
When I click on "Add objects" "submit" "button"
And I select any "Object" from "Add objects" "type" "dropdown"
And I click on "objectsFlyout" "submit" "button"
When I click on "Add Record" "create" "button"
Then I should see "homologationData" interface
When I provide the value for "OriginData" "sourceValue" "list"
And I provide the value for "DestinationData" "name" "textbox"
And I click on "homologationData" "add" "button"
Then I should see "homologationData" interface
When I provide the value for "OriginData" "sourceValue" "list"
And I provide the value for "DestinationData" "name" "textbox"
And I click on "homologationData" "add" "button"
Then I should see "Save" "button" as enabled
When I click on "submit" "button"
Then I should see the message on interface "Homologación almacenada correctamente"
When I navigate to "Homologation" page
And I click on "createHomologation" "button"
Then I should see "homologation" interface
When I select any "SourceLocation2" from "SourceLocation" "type" "dropdown"
And I select any "DestinationLocation1" from "DestinationLocation" "type" "dropdown"
And I select any "GroupValue1" from "Group" "segment" "dropdown"
And I click on "homologation" "continue" "button"
Then I should see the message on interface "El grupo de homologación yaexiste"
@testcase=21190
Scenario: Verify user can Delete Homologation group
Given I have Homologation group in the system
When I navigate to "Homologation" page
And I should see the "Homologation" grid
And I click on "Delete" for "DeleteHomologationName" to be deleted
And I should see "Delete" "Confirmation" "interface"
Then I should see the message on interface "El grupo de homologación contiene {n} registros, confirme la eliminación del grupo y todos los registros."
And I click on "Cancel" "button"
Then I should see the "Homologation" grid
And I select "Homologation" to be deleted
And I should see "Delete" "Confirmation" "interface"
And I click on "Accept" "button"
Then I should see the "Homologation" grid
And I dont find "DeleteHomologationName" in grid
@testcase=21191
Scenario: Verify user can Edit Homologation group
Given I have Homologation group in the system
When I navigate to "Homologation" page
And I should see the "Homologation" grid
And I select "EditHomologationName" to be edited
Then I should see "homologation" interface
When I select any "SourceLocation1" from "SourceLocation" "type" "dropdown"
And I select any "DestinationLocation1" from "DestinationLocation" "type" "dropdown"
And I click on "homologation" "continue" "button"
@testcase=21192
Scenario: Verify user can view details of existing Homologation group
Given I do not have Homologation group in the system
When I navigate to "Homologation" page
And I click on "createHomologation" "button"
Then I should see "homologation" interface
When I select any "SourceLocation1" from "SourceLocation" "type" "dropdown"
And I select any "DestinationLocation1" from "DestinationLocation" "type" "dropdown"
And I select any "GroupValue1" from "Group" "segment" "dropdown"
And I click on "homologation" "continue" "button"
When I select value from "Object" "type" "dropdown"
When I click on "Add Record" "create" "button"
Then I should see "Add Record" "interface"
When I provide the value for "OriginData" "name" "textbox"
And I provide the value for "DestinationData" "name" "list"
And I click on "Add" "submit" "button"
Then I should see "Save" "button" as enabled
When I click on "Save" "button"
Then I should see the "Homologation" grid
When I click on "Add objects" "submit" "button"
Then I verify selected objects from "Object" "type" "dropdown"
And I verify the "OriginData" and "SourceData"
@testcase=21193
Scenario: Verify user can create Homologation group with blank Souce or Destination data
Given I do not have Homologation group in the system
When I navigate to "Homologation" page
And I click on "createHomologation" "button"
Then I should see "homologation" interface
When I select any "SourceLocation1" from "SourceLocation" "type" "dropdown"
And I select any "DestinationLocation1" from "DestinationLocation" "type" "dropdown"
And I select any "GroupValue1" from "Group" "segment" "dropdown"
And I click on "homologation" "continue" "button"
When I click on "Add objects" "submit" "button"
And I select any "Object" from "Add objects" "type" "dropdown"
And I click on "objectsFlyout" "submit" "button"
When I click on "Add Record" "create" "button"
Then I should see "homologationData" interface
When I click on "homologationData" "add" "button"
Then I should see the message on interface "Requerido"
@testcase=21194
Scenario: Verify user can edit Data Mapping with TRUE as Source system
Given I do not have Homologation group in the system
When I navigate to "Homologation" page
And I click on "createHomologation" "button"
Then I should see "homologation" interface
When I select any "SourceLocation2" from "SourceLocation" "type" "dropdown"
And I select any "DestinationLocation3" from "DestinationLocation" "type" "dropdown"
And I select any "GroupValue1" from "Group" "segment" "dropdown"
And I click on "homologation" "continue" "button"
When I click on "Add objects" "submit" "button"
And I select any "Object" from "Add objects" "type" "dropdown"
And I click on "objectsFlyout" "submit" "button"
When I click on "Add Record" "create" "button"
Then I should see "homologationData" interface
When I provide the value for "OriginData1" "sourceValue" "list"
And I provide the value for "DestinationData1" "name" "textbox"
And I click on "homologationData" "add" "button"
Then I should see "Save" "button" as enabled
When I click on "submit" "button"
And I should see the "Homologation" grid
And I click on "Edit record"
When I provide the value for "OriginData2" "sourceValue" "list"
And I provide the value for "DestinationData2" "name" "textbox"
And I click on "homologationData" "add" "button"
@testcase=21195
Scenario: Verify user can edit Data Mapping with TRUE as Destination system
Given I do not have Homologation group in the system
When I navigate to "Homologation" page
And I click on "createHomologation" "button"
Then I should see "homologation" interface
When I select any "SourceLocation1" from "SourceLocation" "type" "dropdown"
And I select any "DestinationLocation1" from "DestinationLocation" "type" "dropdown"
And I select any "GroupValue1" from "Group" "segment" "dropdown"
And I click on "homologation" "continue" "button"
When I click on "Add objects" "submit" "button"
And I select any "Object" from "Add objects" "type" "dropdown"
And I click on "objectsFlyout" "submit" "button"
When I click on "Add Record" "create" "button"
Then I should see "homologationData" interface
When I provide the value for "OriginData1" "sourceValue" "textbox"
And I provide the value for "DestinationData1" "name" "list"
And I click on "homologationData" "add" "button"
Then I should see "Save" "button" as enabled
When I click on "submit" "button"
And I should see the "Homologation" grid
And I click on "Edit record"
When I provide the value for "OriginData2" "sourceValue" "textbox"
And I provide the value for "DestinationData2" "name" "list"
And I click on "homologationData" "add" "button"
@testcase=21196
Scenario: Verify user can delete Data Mapping from a Homologation group
Given I do not have Homologation group in the system
When I navigate to "Homologation" page
And I click on "createHomologation" "button"
Then I should see "homologation" interface
When I select any "SourceLocation1" from "SourceLocation" "type" "dropdown"
And I select any "DestinationLocation1" from "DestinationLocation" "type" "dropdown"
And I select any "GroupValue1" from "Group" "segment" "dropdown"
And I click on "homologation" "continue" "button"
When I click on "Add objects" "submit" "button"
And I select any "Object" from "Add objects" "type" "dropdown"
And I click on "objectsFlyout" "submit" "button"
When I click on "Add Record" "create" "button"
Then I should see "homologationData" interface
When I provide the value for "OriginData" "sourceValue" "textbox"
And I provide the value for "DestinationData" "name" "list"
And I click on "homologationData" "add" "button"
Then I should see "Save" "button" as enabled
When I click on "submit" "button"
And I should see the "Homologation" grid
And I click on "Delete Record" for "DeleteDataMapping" to be deleted
Then I dont find "DeleteDataMapping" in grid
@testcase=21197
Scenario: Verify pagination, sorting and filtering of columns is available for Homologation Group page
Given I do not have Homologation group in the system
When I navigate to "Homologation" page
And I click on "createHomologation" "button"
Then I should see "homologation" interface
When I select any "SourceLocation1" from "SourceLocation" "type" "dropdown"
And I select any "DestinationLocation1" from "DestinationLocation" "type" "dropdown"
And I select any "GroupValue1" from "Group" "segment" "dropdown"
And I click on "homologation" "continue" "button"
When I click on "Add objects" "submit" "button"
And I select any "Object" from "Add objects" "type" "dropdown"
And I click on "objectsFlyout" "submit" "button"
When I click on "Add Record" "create" "button"
Then I should see "homologationData" interface
When I provide the value for "OriginData" "sourceValue" "textbox"
And I provide the value for "DestinationData" "name" "list"
And I click on "homologationData" "add" "button"
Then I should see "Save" "button" as enabled
When I click on "submit" "button"
Then I should see the message on interface "Homologación almacenada correctamente"
And I should see the "Homologation" grid
When I filter "OriginData" as "SINOPER"
Then I should see Homologation with "OriginData" as "SINOPER"
When I filter "DestinationData" as "TRUE"
Then I should see Homologation with "DestinationData" as "TRUE"
And I should see "elementos por página" option on Homologation Group page

@testcase=21198 
Scenario: Verify pagination, sorting and filtering of columns is available for Data Mapping page
Given I do not have Homologation group in the system
When I navigate to "Homologation" page
And I click on "createHomologation" "button"
Then I should see "homologation" interface
When I select any "SourceLocation1" from "SourceLocation" "type" "dropdown"
And I select any "DestinationLocation1" from "DestinationLocation" "type" "dropdown"
And I select any "GroupValue1" from "Group" "segment" "dropdown"
And I click on "homologation" "continue" "button"
When I click on "Add objects" "submit" "button"
And I select any "Object" from "Add objects" "type" "dropdown"
And I click on "objectsFlyout" "submit" "button"
When I click on "Add Record" "create" "button"
Then I should see "homologationData" interface
When I provide the value for "OriginData" "sourceValue" "textbox"
And I provide the value for "DestinationData" "name" "list"
And I click on "homologationData" "add" "button"
Then I should see "Save" "button" as enabled
When I click on "submit" "button"
Then I should see the message on interface "Homologación almacenada correctamente"
When I filter "OriginData" as "SINOPER"
And I filter "DestinationData" as "TRUE"
And I filter "Group" as "Productos"
Then I should see Homologation with "DestinationData" as "TRUE"
When I filter "OriginData" as "Isla VI Sinoper"
Then I should see Data Mapping with "OriginData" as "Isla VI Sinoper"
And I filter "DestinationData" as "Isla VI"
Then I should see Data Mapping with "DestinationData" as "Isla VI"
And I should see "elementos por página" option on Data Mapping page




