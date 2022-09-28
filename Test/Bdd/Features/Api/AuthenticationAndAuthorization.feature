@owner=jagudelos @api @testplan=11317 @testsuite=11325 @version =1
Feature: AuthenticationAndAuthorization
In order to handle the application for different users
I want to implement Authorization for api

@testcase=12597 @bvt @output=QueryAll(GetCategories)
Scenario: Verify category name exists API for accessible roles
	Given I am authenticated as "administrador"
	And I have "Transport Categories" in the system
	When I verify the existence of "Category Name"
	Then the response should be successful

@testcase=12598 @bvt @output=QueryAll(GetCategories)
Scenario Outline: Verify get all categories API for accessible roles
	Given I am authenticated as "<User>"
	And I have "Transport Categories" in the system
	When I Get all records
	Then the response should be successfully authenticated

	Examples:
		| User        |
		| aprobador   |
		| profesional |
		| programador |
		| consulta    |

@testcase=12599 @output=QueryAll(GetCategories)
Scenario Outline: Verify category related APIs for inaccessible roles
	Given I am authenticated as "<User>"
	And I have "Transport Categories" in the system
	When I provide the required fields
	Then the response should fail with "Forbidden"
	When I update a record with required fields
	Then the response should fail with "Forbidden"
	When I Get record with valid Id
	Then the response should fail with "Forbidden"
	When I verify the existence of "Category Name"
	Then the response should fail with "Forbidden"

	Examples:
		| User        |
		| aprobador   |
		| profesional |
		| programador |
		| consulta    |

@testcase=12600 @bvt @output=QueryAll(GetCategoryElements)
Scenario: Verify category element name exists API for accessible roles
	Given I am authenticated as "admin"
	And I have "Category Elements" in the system
	When I verify the existence of "Element Name"
	Then the response should be successful

@testcase=12601 @bvt @output=QueryAll(GetCategoryElements)
Scenario Outline: Verify get all category elements API for accessible roles
	Given I am authenticated as "<User>"
	And I have "Category Elements" in the system
	When I Get all records
	Then the response should be successfully authenticated

	Examples:
		| User        |
		| aprobador   |
		| profesional |
		| programador |
		| consulta    |

@testcase=12602 @output=QueryAll(GetCategoryElements)
Scenario Outline: Verify category element related APIs for inaccessible roles
	Given I am authenticated as "<User>"
	And I have "Category Elements" in the system
	When I provide the required fields
	Then the response should fail with "Forbidden"
	When I update a record with required fields
	Then the response should fail with "Forbidden"
	When I Get record with valid Id
	Then the response should fail with "Forbidden"
	When I verify the existence of "Element Name"
	Then the response should fail with "Forbidden"
	Given there are "Category's Elements" where Active column is True in the system
	When I Get records with Active field is True
	Then the response should fail with "Forbidden"

	Examples:
		| User        |
		| aprobador   |
		| profesional |
		| programador |
		| consulta    |

@testcase=12603 @bvt @output=QueryAll(GetHomologations)
Scenario Outline: Verify homologation related APIs for accessible roles
	Given I am authenticated as "<User>"
	And I want to create a "Homologation" in the system
	When I provide the valid data for "Nodes" of "Source"
	Then the response should succeed with message "Homologación  creada con éxito"
	Given I have "Homologations" in the system
	When I Get all records
	Then the response should be successfully authenticated
	When I Get record with valid Id
	Then the response should be successfully authenticated
	When I Get record by "HomologationId" and "GroupTypeId"
	Then the response should be successfully authenticated
	When I Get record by "HomologationId" and "HomologationGroupName"
	Then the response should be successfully authenticated

	Examples:
		| User        |
		| aprobador   |
		| profesional |
		| programador |
		| consulta    |

@testcase=12604 @bvt @output=QueryAll(GetActiveNodeConnection)
Scenario Outline: Verify get all node connections API for accessible roles
	Given I am authenticated as "<User>"
	And I have "node-connection"
	When I Get all node connection products
	Then the response should be successfully authenticated

	Examples:
		| User          |
		| administrador |
		| aprobador     |
		| profesional   |
		| programador   |
		| consulta      |

@testcase=12605 @bvt @output=QueryAll(GetActiveNodeConnection)
Scenario: Verify update node connections APIs for accessible roles
	Given I am authenticated as "administrador"
	And I have "node-connection" in the system
	When I update a product in the node connection
	Then the response should be successfully authenticated
	When I update owners of a product in the node connection
	Then the response should be successfully authenticated

@testcase=12606 @output=QueryAll(GetActiveNodeConnection)
Scenario Outline: Verify node connection related APIs for inaccessible roles
	Given I am authenticated as "<User>"
	And I have "node-connection"
	When I provide the required fields
	Then the response should fail with "Forbidden"
	When I update a record with required fields
	Then the response should fail with "Forbidden"
	When I delete a record with "no movements"
	Then the response should fail with "Forbidden"
	When I Get record with "SourceNodeId" and "DestinationNodeId"
	Then the response should fail with "Forbidden"
	When I update a product in the node connection
	Then the response should fail with "Forbidden"
	When I update owners of a product in the node connection
	Then the response should fail with "Forbidden"

	Examples:
		| User        |
		| aprobador   |
		| profesional |
		| programador |
		| consulta    |

@testcase=12607 @bvt @output=QueryAll(GetNodes) @version=3
Scenario Outline: Verify node related APIs for accessible roles
	Given I am authenticated as "<User>"
	And I have "Nodes" in the system
	When I Get all records
	Then the response should be successfully authenticated
	When I Get "StorageLocation" by node id
	Then the response should be successfully authenticated
	When I Get "StorageLocation Products" by node id
	Then the response should be successfully authenticated

	Examples:
		| User          |
		| administrador |
		| aprobador     |
		| profesional   |
		| programador   |
		| consulta      |

@testcase=12608 @bvt @output=QueryAll(GetNodes)
Scenario: Verify node name exists API for accessible roles
	Given I am authenticated as "administrador"
	And I have "Nodes" in the system
	When I verify the existence of "Node Name"
	Then the response should be successful

@testcase=12609 @bvt @output=QueryAll(GetNodes)
Scenario: Verify filter nodes API for accessible roles
	Given I am authenticated as "administrador"
	And I have "Nodes" in the system
	When I filter the nodes
	Then the response should be successfully authenticated

@testcase=12610 @output=QueryAll(GetNodes)
Scenario Outline: Verify node related APIs for inaccessible roles
	Given I am authenticated as "<User>"
	And I have "Nodes" in the system
	When I provide the required fields
	Then the response should fail with "Forbidden"
	When I update a record with required fields
	Then the response should fail with "Forbidden"
	When I Get record with valid Id
	Then the response should fail with "Forbidden"
	When I verify the existence of "Node Name"
	Then the response should fail with "Forbidden"
	When I verify the existence of "StorageLocation Name"
	Then the response should fail with "Forbidden"
	When I provide the Acceptable Balance Percentage and Control Limit
	Then the response should fail with "Forbidden"
	When I provide the uncertainity percentage
	Then the response should fail with "Forbidden"
	When I provide the owner data
	Then the response should fail with "Forbidden"
	When I filter the nodes
	Then the response should fail with "Forbidden"

	Examples:
		| User        |
		| aprobador   |
		| profesional |
		| programador |
		| consulta    |

@testcase=12611 @bvt @output=QueryAll(GetNodeTags) @version=2
Scenario Outline: Verify get node tags API for accessible roles
	Given I am authenticated as "<User>"
	And I have "nodetags"
	When I Get all records
	Then the response should be successfully authenticated

	Examples:
		| User          |
		| administrador |
		| aprobador     |
		| profesional   |
		| programador   |
		| consulta      |

@testcase=12612 @bvt
Scenario: Verify associate nodes API for accessible roles
	Given I am authenticated as "admin"
	And I have "nodetags"
	When I associate the nodetags
	Then the response should be successfully authenticated

@testcase=12613
Scenario Outline: Verify associate nodes API for inaccessible roles
	Given I am authenticated as "<User>"
	And I have "nodetags"
	When I associate the nodetags
	Then the response should fail with "Forbidden"

	Examples:
		| User        |
		| aprobador   |
		| profesional |
		| programador |
		| consulta    |

@testcase=12614 @bvt @output=QueryAll(GetTickets)
Scenario Outline: Verify get tickets APIs for accessible roles
	Given I am authenticated as "<User>"
	And I have "Tickets" in the system
	When I Get all records
	Then the response should be successfully authenticated
	Given I have "TicketInfo" in the system
	When I Get record with valid Id
	Then the response should be successfully authenticated

	Examples:
		| User          |
		| administrador |
		| profesional   |

@testcase=12615 @output=QueryAll(GetTickets)
Scenario Outline: Verify get tickets APIs for inaccessible roles
	Given I am authenticated as "<User>"
	And I have "Tickets" in the system
	When I Get all records
	Then the response should fail with "Forbidden"
	Given I have "TicketInfo" in the system
	When I Get record with valid Id
	Then the response should fail with "Forbidden"

	Examples:
		| User        |
		| aprobador   |
		| programador |
		| consulta    |

@testcase=12616 @bvt
Scenario Outline: Verify operational cutoff API for accessible roles
	Given I am authenticated as "<User>"
	And I want to create a "operationalcutoff" in the system
	When I provide the required fields
	Then the response should be successfully authenticated

	Examples:
		| User          |
		| administrador |
		| profesional   |

@testcase=12617
Scenario Outline: Verify operational cutoff API for inaccessible roles
	Given I am authenticated as "<User>"
	And I want to create a "operationalcutoff" in the system
	When I provide the required fields
	Then the response should fail with "Forbidden"

	Examples:
		| User        |
		| aprobador   |
		| programador |
		| consulta    |

@testcase=12618 @bvt
Scenario Outline: Verify unbalances API for accessible roles
	Given I am authenticated as "<User>"
	And I have "Unbalances"
	When I get unbalances
	Then the response should be successfully authenticated

	Examples:
		| User          |
		| administrador |
		| profesional   |

@testcase=12619
Scenario Outline: Verify unbalances API for inaccessible roles
	Given I am authenticated as "<User>"
	And I have "Unbalances"
	When I get unbalances
	Then the response should fail with "Forbidden"

	Examples:
		| User        |
		| aprobador   |
		| programador |
		| consulta    |

@testcase=12620 @bvt @output=QueryAll(GetFileRegistration)
Scenario Outline: Verify file registration related APIs for accessible roles
	Given I am authenticated as "<User>"
	And I have "fileregistration"
	When I provide the required fields for fileregistration
	Then the response should be successfully authenticated
	When I Get register files by ids
	Then the response should be successfully authenticated
	When I Get upload access info for blob file name
	Then the response should be successfully authenticated
	When I Get read access info
	Then the response should be successfully authenticated

	Examples:
		| User          |
		| administrador |
		| profesional   |

@testcase=12621
Scenario Outline: Verify file registration related APIs for inaccessible roles
	Given I am authenticated as "<User>"
	And I have "fileregistration"
	When I Get all records
	Then the response should fail with "Forbidden"
	When I provide the required fields
	Then the response should fail with "Forbidden"
	When I Get register files by ids
	Then the response should fail with "Forbidden"
	When I Get upload access info for blob file name
	Then the response should fail with "Forbidden"
	When I Get read access info
	Then the response should fail with "Forbidden"

	Examples:
		| User        |
		| aprobador   |
		| programador |
		| consulta    |

@testcase=12622 @output=QueryAll(GetLogisticCenters)
Scenario Outline: Verify get logistic centers API for accessible roles
	Given I am authenticated as "<User>"
	And I have "Logistic Centers" in the system
	When I Get all records
	Then the response should be successfully authenticated

	Examples:
		| User        |
		| aprobador   |
		| programador |
		| consulta    |
		| profesional |

@testcase=12623 @output=QueryAll(GetStorageLocations)
Scenario Outline: Verify get storage locations API for accessible roles
	Given I am authenticated as "<User>"
	And I have "Storage Locations" in the system
	When I Get all records
	Then the response should be successfully authenticated

	Examples:
		| User        |
		| aprobador   |
		| programador |
		| consulta    |
		| profesional |

@testcase=12624 @bvt @output=QueryAll(GetRules)
Scenario Outline: Verify get rules API for accessible roles
	Given I am authenticated as "<User>"
	And I have "Rules" in the system
	When I Get all records
	Then the response should be successfully authenticated

	Examples:
		| User          |
		| administrador |
		| aprobador     |
		| programador   |
		| consulta      |
		| profesional   |

@testcase=12625 @bvt @output=QueryAll(GetProducts)
Scenario Outline: Verify get products API for accessible roles
	Given I am authenticated as "<User>"
	And I have "Products" in the system
	When I Get all records
	Then the response should be successfully authenticated

	Examples:
		| User          |
		| administrador |
		| aprobador     |
		| programador   |
		| consulta      |
		| profesional   |

@testcase=12626 @bvt @output=QueryAll(GetScenarios)
Scenario Outline: Verify get scenarios API for accessible roles
	Given I am authenticated as "admin"
	And I have "Scenarios" in the system
	When I Get all records
	Then the response should be successfully authenticated

	Examples:
		| User          |
		| administrador |
		| aprobador     |
		| programador   |
		| consulta      |
		| profesional   |

@testcase=12627 @bvt
Scenario: Verify get pending transaction errors API for accessible roles
	Given I am authenticated as "admin"
	And I have "TransactionErrors"
	When I Get pending transaction errors
	Then the response should be successfully authenticated

@testcase=12628
Scenario Outline: Verify get pending transaction errors API for inaccessible roles
	Given I am authenticated as "<User>"
	And I have "TransactionErrors"
	When I Get pending transaction errors
	Then the response should fail with "Forbidden"

	Examples:
		| User        |
		| aprobador   |
		| programador |
		| consulta    |
		| profesional |