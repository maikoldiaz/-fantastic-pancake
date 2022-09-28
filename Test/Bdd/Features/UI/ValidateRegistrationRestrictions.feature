@sharedsteps=4013 @Owner=jagudelos @ui @manual @testplan=14709 @testsuite=16906
Feature: RegistarionNewValidations
As a Balance Segment Professional User,
I need new validations to be included in the movement and inventory
register to ensure the information consistency

@testcase=18069 @ui @manual
Scenario: Verify general exception when Homologation doesnot exist between Source and Destination System
When  I donot have any Homologation Mapping between Systems
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then I should see exception

@testcase=18070 @ui @manual
Scenario: Verify Error when I donot have Homologation for ProductType of Inventory
When  I donot have any Homologation Mapping for ProductType of Inventory
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then I should see exception message "Identificador del tipo de producto no encontrado"

@testcase=18071 @ui @manual
Scenario: Verify Error when I donot have Homologation for MeasurementUnit of Inventory
When  I donot have any Homologation Mapping for MeasurementUnit of Inventory
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then I should see exception message "Identificador de la unidad no encontrado"

@testcase=18072 @ui @manual
Scenario: Verify Error when I donot have Homologation for OwnerId of Inventory
When  I donot have any Homologation Mapping for OwnerId of Inventory
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then I should see exception message "Identificador del propietario no encontrado"

@testcase=18073 @ui @manual
Scenario: Verify Error when I donot have Homologation for MovementTypeId of Movement
When  I donot have any Homologation Mapping for MovementTypeId of Movement
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then I should see exception message "Identificador del tipo de movimiento no encontrado"

@testcase=18074 @ui @manual
Scenario: Verify Error when I donot have Homologation for SourceProductTypeId of Movement
When  I donot have any Homologation Mapping for SourceProductTypeId of Movement
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then I should see exception message "Identificador del tipo de producto origen no encontrado"

@testcase=18075 @ui @manual
Scenario: Verify Error when I donot have Homologation for DestinationProductTypeId of Movement
When  I donot have any Homologation Mapping for DestinationProductTypeId of Movement
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then I should see exception message "Identificador del tipo de producto destino no encontrado"

@ui @manual
Scenario: Verify Error when I donot have Homologation for MeasurementUnit of Movement
When  I donot have any Homologation Mapping for MeasurementUnit of Movement
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then I should see exception message "Identificador de la unidad no encontrado"

@testcase=21315 @ui @manual
Scenario: Verify Error when I donot have Homologation for Ownership of Movement
When  I donot have any Homologation Mapping for Ownership of Movement
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then I should see exception message "Identificador del propietario no encontrado"

@testcase=16904 @ui @manual
Scenario: Verify Movement Registration without Homologation but correct data types
When  I donot have any Homologation Mapping but data types are correct for Movement
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then movement should be successfully registered

@testcase=16905 @ui @manual
Scenario: Verify Inventory Registration without Homologation but correct data types
When  I donot have any Homologation Mapping but data types are correct for Inventory
When I navigate to "FileUpload" page
And I click on "FileUpload" "button"
When I select "Insert" from FileUpload dropdown
And I click on "Browse" to upload
And I select "ValidExcel" file from directory
And I click on "uploadFile" "Submit" "button"
Then inventory should be successfully registered
