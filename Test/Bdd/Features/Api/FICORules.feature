@owner=jagudelos @api @testplan=35673 @testsuite=35681 @parallel=false
Feature: FICORules
As a TRUE Administrator,
I want to to update the source of the ownership rules to use the FICO rules

@testcase=41339 @bvt @bvt1.5
Scenario: Validate that Fico Rules are successfully loaded into the DB at the Node Level
Given I have Fico Connection setup into the system
When I invoke the FICO service to fetch the strategies with input parameter tipoLlamada: "BUSCA_ESTRATEGIA" and estado: "true"
Then Validate that response are successfully loading into the table at "Node" Level for which id is "estrategiaPropiedadNodo"

@testcase=41340 @bvt @bvt1.5
Scenario: Validate that Fico Rules are successfully loaded into the DB at the node/product level
Given I have Fico Connection setup into the system
When I invoke the FICO service to fetch the strategies with input parameter tipoLlamada: "BUSCA_ESTRATEGIA" and estado: "true"
Then Validate that response are successfully loading into the table at "Node/Product" Level for which id is "estrategiaPropiedadNodoProducto"

@testcase=41341 @bvt @bvt1.5
Scenario: Validate that Fico Rules are successfully loaded into the DB at the connection/product level
Given I have Fico Connection setup into the system
When I invoke the FICO service to fetch the strategies with input parameter tipoLlamada: "BUSCA_ESTRATEGIA" and estado: "true"
Then Validate that response are successfully loading into the table at "Connection/Product" Level for which id is "estrategiaConexiones"

@testcase=37265 @manual
Scenario: Validate that Fico Rules strategies would be refreshed every N configurable hours.
Given I have Fico Connection setup into the system
When I invoke the FICO service to fetch the strategies with input parameter tipoLlamada: "BUSCA_ESTRATEGIA" and estado: "true"
Then Validate the Rules should be stored in DB for "N" configurable hours.
And It should automatically refreshed after "N" hrs.

@testcase=37266 @manual
Scenario: Validate that when we change the configuration from N to M hours then it should refresh after M hours in next run.
Given I have Fico Connection setup into the system
And I change the configuration hours from "N" to "M" hours
When I invoke the FICO service to fetch the strategies with input parameter tipoLlamada: "BUSCA_ESTRATEGIA" and estado: "true"
Then Validate the Rules should be stored in DB for "M" configurable hours.
And It should automatically refreshed after "M" hrs.

@testcase=37267 @manual
Scenario: Validate that True will Retry the Fico service invocation three times if it responds with a different Http response code than (200.X)
Given I have Fico Connection setup into the system
When I invoke the FICO service to fetch the strategies with input parameter tipoLlamada: "BUSCA_ESTRATEGIA" and estado: "true"
And FICO service responds with a different Http response code than (200.X)
Then the system should Retry the service invocation.

@testcase=37268 @manual
Scenario: Validate that error Message displayed when Fico services responds with a different Http response code than (200.X) after three failed attempts
Given I have Fico Connection setup into the system
When I invoke the FICO service to fetch the strategies with input parameter tipoLlamada: "BUSCA_ESTRATEGIA" and estado: "true"
And FICO service responds with a different Http response code than (200.X) three times
Then I should see "Se produjo un error y no podemos actualizar las estrategias de propiedad.  Por favor intente nuevamente." error Message
And Error Message should be stored in the app insights

@testcase=37269 @bvt
Scenario: Validate that system should save invocation response time and log when Fico services responds successfully
Given I have Fico Connection setup into the system
When I invoke the FICO service to fetch the strategies with input parameter tipoLlamada: "BUSCA_ESTRATEGIA" and estado: "true"
And FICO service responds with success response code
Then verify that auditedStep from the response should be stored in the app insights
And verify that response log time should be stored in the app insights
