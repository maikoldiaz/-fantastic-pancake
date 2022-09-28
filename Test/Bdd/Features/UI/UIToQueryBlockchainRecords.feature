@owner=jagudelos @ui @MVP2and3 @S15 @testsuite=61558 @testplan=61542
Feature: UIQueryBlockchainRecords
In order to query blockchain records
as an Auditor/Administrator
I need an UI in TRUE
@testcase=68087
Scenario Outline: Verify Blockchain data access page is available to Auditor and Administrator
Given I am logged in as "<User>"
When I navigate to Blockchain Data access page
Then I should see a page to configure query data

Examples:
| user  |
| admin |
| audit |
@testcase=68088
Scenario Outline: Verify Blockchain data access page is not available to Non-Administrator and Non-Auditor roles
Given I am logged in as "<User>"
When I navigate to Blockchain Data access page
Then I should see a unauthorized page

Examples:
| user        |
| aprobador   |
| profesional |
| programador |
| consulta    |
@testcase=68089
Scenario Outline: Verify Blockchain data access filter for Por registro
Given I am logged in as "<User>"
When I navigate to Blockchain Data access page
Then I should see a filter page
Then Filter dropdown should have "Por bloque" and "Por Registro" filter items
When I select "Por Registro" "dropdown" filter item
Then I see "textfield" "transactionid" field enabled
Then I see "consultar" button

Examples:
| user  |
| admin |
| audit |

@testcase=68090 @bvt
Scenario Outline: Verify Blockchain data access filter for Por bloque
Given I am logged in as "<User>"
When I navigate to Blockchain Data access page
And I should see a filter page
And Filter dropdown should have "Por bloque" and "Por Registro" filter items
And I select "Por bloque" "dropdown" filter item
And I click on "Consultar" "button"
Then I see "transactions grid" "grid" with the latest 100 transactions from blockchain
And I see transaction grid with "Date" and "Transaction Id" fields
And I see each transaction with transactionid link
And I see Page size dropdown with 10,50 and 100 options

Examples:
| user  |
| admin |
| audit |

@testcase=68091 @bvt
Scenario Outline: Verify Blockchain data , transaction id access for Por registro
Given I am logged in as "<User>"
When I navigate to Blockchain Data access page
And I should see a filter page
And Filter dropdown should have "Por bloque" and "Por Registro" filter items
And I select "Por registro" "dropdown" filter item
Then I see "transactions id" "textbox"
And I see "Consultar" button
And I click on "Consultar" "button"
And I should see the error message "El campo Identificador de transacción en blockchain es obligatorio"
And I enter incorrect transactionid
And I should see the error message "El Id de transacción ingresado no existe en Blockchain"
And I enter correct transaction id
And I click on "Consultar" "button"
And I should see Transaction Details with "Transaction Hash", "Block number", "Block Hash", "Type of Contract" and "Contract Content"
And I should see a button to return to filter page

Examples:
| user  |
| admin |
| audit |
