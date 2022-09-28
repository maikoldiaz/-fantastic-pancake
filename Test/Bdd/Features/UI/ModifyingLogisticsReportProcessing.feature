@sharedsteps=4013 @owner=jagudelos  @ui @testplan=31102 @testsuite=31110
Feature: ModifyingLogisticsReportProcessing
As a TRUE system, I require modifying the logistic report processing
to generate it per node and include inventories

Background: Login
Given I am logged in as "admin"
@testcase=33822
Scenario: Verify the structure of the file with ownership logistic information for all Nodes
Given I have ownership calculation data generated in the system
When I navigate to "Logistic Report Generation" page
And I click on "CreateLogistics" "button"
And I should see "CreateLogistics" "Create" "Interface"
And I selected Segment from "Segment" "CreateLogistics" "combobox"
And I selected TODOS from the list of options provided under the node dropdown
And I select Owner on the Create file interface
And I select Start date and End Date on Create file Interface
And I click on "CreateLogistics" "Submit" "button"
Then I should see Logistic Report for selected segment in the Logistic Report Generation page
And the movement sheet should name be changed from MASIVO to Movimientos
And a new sheet called Inventarios must be present in created Excel file
And verify that all the nodes which have Send To SAP are processed in the output Excel
@testcase=33823
Scenario: Verify all the columns of Inventory column are properly created
Given I have ownership calculation data generated in the system
When I navigate to "Logistic Report Generation" page
And I click on "CreateLogistics" "button"
And I should see "CreateLogistics" "Create" "Interface"
And I selected Segment from "Segment" "CreateLogistics" "combobox"
And I selected TODOS from the list of options provided under Node
And I select Owner on the Create file interface
And I select Start date and End Date on Create file Interface
And I click on "CreateLogistics" "Submit" "button"
Then I should see Logistic Report for selected segment in the Logistic Report Generation page
And verify that the following columns are present in the Inventory sheet
| Columns         |
| INVENTARIO      |
| ALMACEN         |
| PRODUCTO        |
| VALOR           |
| UOM             |
| HALLAZGO        |
| DIAGNOSTICO     |
| IMPACTO         |
| SOLUCION        |
| ESTADO          |
| ORDEN           |
| FECHA-OPERATIVA |
And verify that all the columns are in text format
And verify that the VALOR column is in number format with the maximum number of decimals allowed being 2
And verify that the FECH-OPERATIVA column is in date format
@testcase=33824
Scenario: Verify all the Inventory column is populated for all nodes which have Send To SAP property enabled
Given I have ownership calculation data generated in the system
When I navigate to "Logistic Report Generation" page
And I click on "CreateLogistics" "button"
And I should see "CreateLogistics" "Create" "Interface"
And I selected Segment from "Segment" "CreateLogistics" "combobox"
And I selected TODOS from the list of options provided under Node
And I select Owner on the Create file interface
And I select Start date and End Date on Create file Interface
And I click on "CreateLogistics" "Submit" "button"
Then I should see Logistic Report for selected segment in the Logistic Report Generation page
And verify that the entries are present for all the nodes which have Send To SAP property enabled
And verify that the inventory column is filled from the Final Inventories list owned by the Segment for the nodes in the Segment
@testcase=33825
Scenario: Verify that the file is not generated when there is an error during the processing of the file
Given I have ownership calculation data generated in the system
When I navigate to "Logistic Report Generation" page
And I click on "CreateLogistics" "button"
And I should see "CreateLogistics" "Create" "Interface"
And I selected Segment from "Segment" "CreateLogistics" "combobox"
And I selected TODOS from the list of options provided under Node
And I select Owner on the Create file interface
And I select Start date and End Date on Create file Interface
And I click on "CreateLogistics" "Submit" "button"
And if there is any error during the processing of the file
Then verify that a proper error is displayed showing the actual error message
And verify that the file is not generated as part of the processing
@testcase=33826
Scenario: Verify the file is processed for any of the provided node options
Given I have ownership calculation data generated in the system
When I navigate to "Logistic Report Generation" page
And I click on "CreateLogistics" "button"
And I should see "CreateLogistics" "Create" "Interface"
And I selected Segment from "Segment" "CreateLogistics" "combobox"
And I selected any node other than TODOS from the list of options provided under the node dropdown
And I select Owner on the Create file interface
And I select Start date and End Date on Create file Interface
And I click on "CreateLogistics" "Submit" "button"
Then I should see Logistic Report for selected segment in the Logistic Report Generation page
And verify that all the processing is performed only for the movements where the provided node is a source node or destination node and Send to SAP property is enabled
And verify that if the Source Node or Destination Node is empty for the selected Node,  even those entries are recorded in the processed output file
@testcase=33827 
Scenario Outline: Verify the processing of Movements is updated for various combinations of Source and Destination Nodes and Products
Given I have ownership calculation data generated in the system
When I navigate to "Logistic Report Generation" page
And I click on "CreateLogistics" "button"
And I should see "CreateLogistics" "Create" "Interface"
And I selected Segment from "Segment" "CreateLogistics" "combobox"
And I selected any node other than TODOS from the list of options provided under the node dropdown
And I select Owner on the Create file interface
And I select Start date and End Date on Create file Interface
And obtain the details for the Source Node/Destination Node/Source Product/Destination Product
And obtain the logistics center storage location information associated with the node
And the node and product satisfy the provided conditions for <Same Product Category>, <Same Logistics Center Category> and <Same Storage Location Category>
And I click on "CreateLogistics" "Submit" "button"
Then I should see Logistic Report for selected segment in the Logistic Report Generation page
And verify that the <Output value> are coming up as per the below combinations

Examples:
| Same Product Category | Same Logistics Center Category | Same Storage Location Category | Output value                                                       |
| FALSE                 | FALSE                          | FALSE                          | Traslado Materia a Material                                        |
| FALSE                 | FALSE                          | TRUE                           | Traslado Materia a Material                                        |
| FALSE                 | TRUE                           | FALSE                          | Traslado Materia a Material                                        |
| FALSE                 | TRUE                           | TRUE                           | Traslado Materia a Material                                        |
| TRUE                  | FALSE                          | FALSE                          | Traslado de Centro a Centro                                        |
| TRUE                  | FALSE                          | TRUE                           | Traslado de Centro a Centro                                        |
| TRUE                  | TRUE                           | FALSE                          | Traslado de Almacen a Almacen                                      |
| TRUE                  | TRUE                           | TRUE                           | Lanzar Error / Throws an-error INVALID_COMBINATION_TO_SIV_MOVEMENT |
