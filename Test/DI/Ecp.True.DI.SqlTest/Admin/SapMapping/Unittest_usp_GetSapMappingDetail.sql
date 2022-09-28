/******************************************************************************
-- Type = Execute stored procedure by using the given paramters and load the data into [Admin].[SapMappingDetail]
-- INSERT --> Insert into Admin.SapMapping table.
Scenario: We should fetch entity names based on Ids in SapMapping table from corresponding Mapping tables
--        OfficialSytem -> admin.Categoryelement where categoryid=8
--        SourceSytem -> admin.Categoryelement where categoryid=8
--        DestinationSytem -> admin.Categoryelement where categoryid=8
--        sourceMovementType -> admin.Categoryelement where categoryid=9
--        DestinationMovementType -> admin.Categoryelement where categoryid=9
--        SourceSystemSourceNode -> admin.Node
--        SourceSystemDestinationNode-> admin.Node
--        DestinationSystemSourceNode-> admin.Node
--        DestinationSystemDestinationNode-> admin.Node
--        SourceProduct -> admin.Product
--        DestinationProduct -> admin.Product

Expectation: Stored procedure has to fetch entity names by joing sapMapping table with master tables (mentioned above) if they exists otherwise show entity ids present in sapMapping table
********************************************************************************/
/* -------- DATA PREPERATION -- */

-- Selecting some system values --
select * From admin.CategoryElement where  CategoryId=8
/*121071,121089,121107,121148,121172,121190,121375,121353,121441*/

-- Selesting some values not present as systems--
select * From admin.CategoryElement where  CategoryId=8 and elementid in ( 9,10,11)

---- Selecting some Movement type values --
select * From admin.CategoryElement where  CategoryId=9
/*42,43,44,48,49,153,154*/

-- Selecting some values not present as movement types--
select * From admin.CategoryElement where  CategoryId=9 and elementid in ( 9,10,11)

-- Selecting some product values --
select * from admin.Product
/*
10000002049,10000002093,10000002199,10000002403,10000003001,10000003004,10000003003
*/

-- sleecting some node values --
select * From admin.Node  
--27153,27154,27155,27180,27181,27182,27166,27165

-- INSERTING INTO SAPMAPPING TABLE--
 -- CASE 1:
 -- MATCHING VALUES FOR ALL COLUMNS IN CORRESPONDING MASTERS TABLES - TEXT VALUES SHOULD BE DISPLAYED
 INSERT INTO Admin.SapMapping (OfficialSystem,SourceSystemId,SourceMovementTypeId,SourceProductId,SourceSystemSourceNodeId,SourceSystemDestinationNodeId
 ,DestinationSystemId,DestinationMovementTypeId,DestinationProductId,DestinationSystemSourceNodeId,DestinationSystemDestinationNodeId,[CreatedBy],[CreatedDate])
 VALUES(121071,121089,48,10000002049,27153,27154,121190,44,10000002199,27155,27156,'rijosh',GETDATE())
 
 INSERT INTO Admin.SapMapping (OfficialSystem,SourceSystemId,SourceMovementTypeId,SourceProductId,SourceSystemSourceNodeId,SourceSystemDestinationNodeId
 ,DestinationSystemId,DestinationMovementTypeId,DestinationProductId,DestinationSystemSourceNodeId,DestinationSystemDestinationNodeId,[CreatedBy],[CreatedDate])
 VALUES(121107,121089,42,10000003001,27153,27181,121148,48,10000002403,27180,27156,'rijosh',GETDATE())

  INSERT INTO Admin.SapMapping (OfficialSystem,SourceSystemId,SourceMovementTypeId,SourceProductId,SourceSystemSourceNodeId,SourceSystemDestinationNodeId
 ,DestinationSystemId,DestinationMovementTypeId,DestinationProductId,DestinationSystemSourceNodeId,DestinationSystemDestinationNodeId,[CreatedBy],[CreatedDate])
 VALUES(121148,121148,43,10000002093,27180,27182,121190,49,10000002049,27182,27156,'rijosh',GETDATE())

 INSERT INTO Admin.SapMapping (OfficialSystem,SourceSystemId,SourceMovementTypeId,SourceProductId,SourceSystemSourceNodeId,SourceSystemDestinationNodeId
 ,DestinationSystemId,DestinationMovementTypeId,DestinationProductId,DestinationSystemSourceNodeId,DestinationSystemDestinationNodeId,[CreatedBy],[CreatedDate])
 VALUES(121441,121375,154,10000002093,27180,27182,121190,49,10000003001,27182,27156,'rijosh',GETDATE())

   INSERT INTO Admin.SapMapping (OfficialSystem,SourceSystemId,SourceMovementTypeId,SourceProductId,SourceSystemSourceNodeId,SourceSystemDestinationNodeId
 ,DestinationSystemId,DestinationMovementTypeId,DestinationProductId,DestinationSystemSourceNodeId,DestinationSystemDestinationNodeId,[CreatedBy],[CreatedDate])
 VALUES(121375,121148,50,10000003003,27180,27182,121441,154,10000003004,27182,27156,'rijosh',GETDATE())
 -- CASE 2:
 -- ENTITIES IN Admin.SapMapping, NOT PRESENT IN MASTER TABLES - ENTITY IDs SHOULD BE DISPLAYED
 INSERT INTO Admin.SapMapping (OfficialSystem,SourceSystemId,SourceMovementTypeId,SourceProductId,SourceSystemSourceNodeId,SourceSystemDestinationNodeId
 ,DestinationSystemId,DestinationMovementTypeId,DestinationProductId,DestinationSystemSourceNodeId,DestinationSystemDestinationNodeId,[CreatedBy],[CreatedDate])
 VALUES(1,9,48,1000000000,1,2,121190,44,10000002099,3,4,'rijosh',GETDATE())

 -- * SP EXECUTION * --
declare @ExecutionId	NVARCHAR(250) 
set @ExecutionId='rijosh_test1'
exec  admin.usp_GetSapMappingDetail @ExecutionId

-- VERIFY RESULTS IN THE TABLE --
select * from [Admin].[SapMappingDetail]
