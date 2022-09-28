@sharedsteps=4013 @owner=jagudelos @ui @testplan=24148 @testsuite=24154 @parallel=false
Feature: ExcelProcessToRegisterPurchaseAndSalesContracts
As TRUE system, I need to process an Excel file with 
purchases and sales contracts to register them in the system	

Background: Login
	Given I am logged in as "admin"

@bvt @ui @testcase=25173 @bvt1.5
Scenario: Verify when TRUE User uploads an excel with valid data
	Given I have "SalesAndPurchase" homologation data in the system
	And I update data in "InsertContractExcel"
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "InsertContractExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "Contract" "Inserted" in the system

@bvt @testcase=25174
Scenario Outline: Verify when TRUE user uploads an excel with missing mandatory fields
	Given I have "SalesAndPurchase" data in  the system
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "FileUpload" "button"
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "ValidExcel" file from purchase sales directory
	And I click on "uploadFile" "Submit" "button"
	Then I verify i see the "Requerido" in audit log

	Examples:
		| fields          |
		| DocumentNumber  |
		| Position        |
		| Type            |
		| Source Node     |
		| DestinationNode |
		| Product         |
		| Comercial       |
		| StartDate       |
		| EndDate         |
		| Value           |
		| unit            |
		| Frequency       |

@bvt @testcase=25175
Scenario Outline: Verify when TRUE user uploads an excel with invalid data
	Given I have "SalesAndPurchase" data in  the system
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "FileUpload" "button"
	And I update invalid <"fields"> in PurchaseAndSales Excel
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload
	And I select "WithoutRecords" file from purchase sales directory
	And I click on "uploadFile" "Submit" "button"
	Then I verify i see the <"message"> in audit log

	Examples:
		| fields          | message                                                                              |
		| Source Node     | Identificador del nodo origen no encontrado                                          |
		| DestinationNode | Identificador del nodo destino no encontrado                                         |
		| Product         | Identificador del producto destino no encontrado                                     |
		| Comercial       | Identificador del comercial no encontrado                                            |
		| unit            | Identificador de la unidad no encontrado                                             |
		| Frequency       | La frecuencia solo permite los valores: "diaria", "semanal", "quincenal" y "mensual" |

@bvt @testcase=25176
Scenario Outline: Verify when TRUE user upload an excel with wrong dates
	Given I have "SalesAndPurchase" data in  the system
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "ValidExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I verify i see the <"message"> in audit log

	Examples:
		| scenario                             | message                                                                            |
		| Iniial date in the month before      | La fecha inicial debe ser mayor o igual a la fecha del primer día del mes anterior |
		| Initial Date greater than Final date | La fecha final debe ser mayor a la fecha inicial                                   |
		| Final Date less than current day     | La fecha final debe ser mayor o igual a la fecha actual                            |

@bvt @testcase=25177 @bvt1.5 @version=2
Scenario: Verify when TRUE user uploads an excel which has already been processed
	And I have "Contract" datas in the system
	When I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "InsertContractExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I should see "El contrato ya existe" in PendingTransactions for "Contract"

@bvt @testcase=25178 @bvt1.5
Scenario: Verify when TRUE user uploads an excel to update existing contracts
	Given I have "Contract" datas in the system
	And I update data in "UpdateContractExcel"
	When I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Update" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "UpdateContractExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "Contract" "Updated" in the system

@bvt @testcase=25179
Scenario: : Verify when TRUE user uploads an excel to update existing contracts with no previous data
	Given I have "SalesAndPurchase" data in  the system
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Update" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "ValidExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I verify i see the "El contrato no existe" in audit log

@bvt @testcase=25180 @bvt1.5 @version=2
Scenario: Verify TRUE user is able to delete existing contracts from system
	Given I have "Contract" datas in the system
	And I update data in "DeleteContractExcel"
	When I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Delete" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "DeleteContractExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I should see the "Contract" "Deleted" in the system

@bvt @testcase=25181
Scenario: Verify the error message when TRUE user tries to delete contract not in the system
	Given I have "SalesAndPurchase" data in  the system
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Insert" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "ValidExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
	Then I verify if i have registered the contracts
	When I navigate to "FileUploadForSalesAndPurchases" page
	And I click on "LoadNew" "button"
	And I select in "Contracts" from movement type dropdown
	And I select "Delete" from FileUpload dropdown
	And I click on "Browse" to upload contracts
	And I select "ValidExcel" file from purchase sales
	And I click on "uploadFile" "Submit" "button"
