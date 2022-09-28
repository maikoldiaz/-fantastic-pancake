/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	FEB-18-2020

-- Description:     These test cases are for stored procedure [Admin].[GetGraphicalNode]

-- Database backup Used:	appdb_17_Feb_2020
-- ==============================================================================================================================*/

/*
The below insert statements are required to populate data into 
[CategoryElement]
[Node]
[NodeTag]
[NodeConnection]
*/
------------------ INSERT DATA INTO CATEGORY ELEMENT/ NODE/ NodeTag/ NodeConnection/ Icon/ Color TABLES ---------------------------------------
DECLARE @SegCategoryElementId INT,@SegNode1 INT,@SegNode2 INT, @SegNode3 INT, @SegNode4 INT
       ,@SysCategoryElementId INT,@SysNode1 INT,@SysNode2 INT, @SysNode3 INT, @SysNode4 INT
--------------################# SEGEMENT #####################-------------------
INSERT INTO [Admin].[CategoryElement] ([Name],[Description],CategoryId,Color,IsActive,CreatedBy,CreatedDate)  
                               VALUES ('Automation_Segement143','Vijay_Category_Segment','#CD5C5C',2,1,'Vijay',GETDATE()) --> CategoryId
SELECT @SegCategoryElementId = SCOPE_IDENTITY(); 

--########--NODE
INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node1_Segement143','Vijay_Node',4033,1,0,'0.1','0.2',1,'Vijay','Vijay',GETDATE()) --> SegmentNodeId 1
SELECT @SegNode1 = SCOPE_IDENTITY();

INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node2_Segement143','Vijay_Node',4033,1,0,'0.2','0.2',1,'Vijay','Vijay',GETDATE()) --> SegmentNodeId 2
SELECT @SegNode2 = SCOPE_IDENTITY();

INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node3_Segement143','Vijay_Node',4033,1,0,'0.3','0.2',1,'Vijay','Vijay',GETDATE()) --> SegmentNodeId 3
SELECT @SegNode3 = SCOPE_IDENTITY();

INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node4_Segement143','Vijay_Node',4033,1,0,'0.4','0.4',1,'Vijay','Vijay',GETDATE()) --> SegmentNodeId 4
SELECT @SegNode4 = SCOPE_IDENTITY();

--########--NODE TAG
INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SegNode1,@SegCategoryElementId,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SegNode2,@SegCategoryElementId,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SegNode3,@SegCategoryElementId,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SegNode4,@SegCategoryElementId,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

--########--NODE CONNECTION
INSERT INTO [Admin].[NodeConnection] (SourceNodeId,DestinationNodeId,Description,ControlLimit,AlgorithmId,IsActive,IsDeleted,IsTransfer,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
                              VALUES (@SegNode1,@SegNode2,'VijayTestNodeConnections','0.1',1,1,0,0,'Vijay',GETDATE(),'Vijay',GETDATE())

INSERT INTO [Admin].[NodeConnection] (SourceNodeId,DestinationNodeId,Description,ControlLimit,AlgorithmId,IsActive,IsDeleted,IsTransfer,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
                              VALUES (@SegNode2,@SegNode3,'VijayTestNodeConnections','0.2',1,1,0,0,'Vijay',GETDATE(),'Vijay',GETDATE())

INSERT INTO [Admin].[NodeConnection] (SourceNodeId,DestinationNodeId,Description,ControlLimit,AlgorithmId,IsActive,IsDeleted,IsTransfer,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
                              VALUES (@SegNode3,@SegNode4,'VijayTestNodeConnections','0.3',1,1,0,0,'Vijay',GETDATE(),'Vijay',GETDATE())

INSERT INTO [Admin].[NodeConnection] (SourceNodeId,DestinationNodeId,Description,ControlLimit,AlgorithmId,IsActive,IsDeleted,IsTransfer,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
                              VALUES (@SegNode4,@SegNode1,'VijayTestNodeConnections','0.4',1,1,0,0,'Vijay',GETDATE(),'Vijay',GETDATE())

INSERT INTO [Admin].[NodeConnection] (SourceNodeId,DestinationNodeId,Description,ControlLimit,AlgorithmId,IsActive,IsDeleted,IsTransfer,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
                              VALUES (@SegNode1,@SegNode3,'VijayTestNodeConnections','0.5',1,1,0,0,'Vijay',GETDATE(),'Vijay',GETDATE())

INSERT INTO [Admin].[NodeConnection] (SourceNodeId,DestinationNodeId,Description,ControlLimit,AlgorithmId,IsActive,IsDeleted,IsTransfer,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
                              VALUES (@SegNode3,@SegNode1,'VijayTestNodeConnections','0.6',1,1,0,0,'Vijay',GETDATE(),'Vijay',GETDATE())

INSERT INTO [Admin].[NodeConnection] (SourceNodeId,DestinationNodeId,Description,ControlLimit,AlgorithmId,IsActive,IsDeleted,IsTransfer,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
                              VALUES (@SegNode4,@SegNode2,'VijayTestNodeConnections','0.7',1,1,0,0,'Vijay',GETDATE(),'Vijay',GETDATE())

--------------################# SYSTEM #####################-------------------
INSERT INTO [Admin].[CategoryElement] (Name,Description,CategoryId,Color,IsActive,CreatedBy,CreatedDate)  
                               VALUES ('Automation_System143','Vijay_Category_System','#F08080',8,1,'Vijay',GETDATE())
SELECT @SysCategoryElementId = SCOPE_IDENTITY(); 

--########--NODE
INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node1_System143','Vijay_Node',4033,1,0,'0.2','0.3',1,'Vijay','Vijay',GETDATE())
SELECT @SysNode1 = SCOPE_IDENTITY();

INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node2_System143','Vijay_Node',4033,1,0,'0.3','0.4',1,'Vijay','Vijay',GETDATE())
SELECT @SysNode2 = SCOPE_IDENTITY();

INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node3_System143','Vijay_Node',4033,1,0,'0.4','0.5',1,'Vijay','Vijay',GETDATE())
SELECT @SysNode3 = SCOPE_IDENTITY();

INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node4_System143','Vijay_Node',4033,1,0,'0.5','0.6',1,'Vijay','Vijay',GETDATE())
SELECT @SysNode4 = SCOPE_IDENTITY();

--########--NODE TAG
INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SysNode1,@SysCategoryElementId,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SysNode2,@SysCategoryElementId,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SysNode3,@SysCategoryElementId,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SysNode4,@SysCategoryElementId,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

--########--NODE CONNECTION
INSERT INTO [Admin].[NodeConnection] (SourceNodeId,DestinationNodeId,Description,ControlLimit,AlgorithmId,IsActive,IsDeleted,IsTransfer,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
                              VALUES (@SysNode1,@SysNode2,'VijayTestNodeConnections','0.3',1,1,0,0,'Vijay',GETDATE(),'Vijay',GETDATE())

INSERT INTO [Admin].[NodeConnection] (SourceNodeId,DestinationNodeId,Description,ControlLimit,AlgorithmId,IsActive,IsDeleted,IsTransfer,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
                              VALUES (@SysNode2,@SysNode3,'VijayTestNodeConnections','0.4',1,1,0,0,'Vijay',GETDATE(),'Vijay',GETDATE())

INSERT INTO [Admin].[NodeConnection] (SourceNodeId,DestinationNodeId,Description,ControlLimit,AlgorithmId,IsActive,IsDeleted,IsTransfer,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
                              VALUES (@SysNode3,@SysNode4,'VijayTestNodeConnections','0.5',1,1,0,0,'Vijay',GETDATE(),'Vijay',GETDATE())

INSERT INTO [Admin].[NodeConnection] (SourceNodeId,DestinationNodeId,Description,ControlLimit,AlgorithmId,IsActive,IsDeleted,IsTransfer,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
                              VALUES (@SysNode4,@SysNode1,'VijayTestNodeConnections','0.6',1,1,0,0,'Vijay',GETDATE(),'Vijay',GETDATE())

INSERT INTO [Admin].[NodeConnection] (SourceNodeId,DestinationNodeId,Description,ControlLimit,AlgorithmId,IsActive,IsDeleted,IsTransfer,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
                              VALUES (@SysNode1,@SysNode3,'VijayTestNodeConnections','0.7',1,1,0,0,'Vijay',GETDATE(),'Vijay',GETDATE())

INSERT INTO [Admin].[NodeConnection] (SourceNodeId,DestinationNodeId,Description,ControlLimit,AlgorithmId,IsActive,IsDeleted,IsTransfer,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
                              VALUES (@SysNode3,@SysNode1,'VijayTestNodeConnections','0.8',1,1,0,0,'Vijay',GETDATE(),'Vijay',GETDATE())

INSERT INTO [Admin].[NodeConnection] (SourceNodeId,DestinationNodeId,Description,ControlLimit,AlgorithmId,IsActive,IsDeleted,IsTransfer,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate)
                              VALUES (@SysNode4,@SysNode2,'VijayTestNodeConnections','0.9',1,1,0,0,'Vijay',GETDATE(),'Vijay',GETDATE())

--########--TAGGING NODES WITH NODE TYPE CATEGORY
INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SegNode1,1,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SegNode2,2,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SegNode3,3,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SysNode1,2,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SysNode2,3,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SysNode4,4,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

--########--TAGGING NODES WITH OPERADOR CATEGORY
INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SegNode1,14,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SegNode2,15,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SegNode4,16,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SysNode1,15,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SysNode3,16,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SysNode4,16,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

--########--TAGGING SEGMENT NODE WITH SYSTEM CATEGORY
INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SegNode1,9888,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

--########--TAGGING SYSTEM NODE WITH SYSTEM CATEGORY
INSERT INTO [Admin].[NodeTag] (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
                       VALUES (@SysNode1,10,'2019-10-07 00:00:00.000','2020-10-07 00:00:00.000','Vijay',GETDATE()) 

-- INSERT RECORDS OUTPUT
SELECT @SegCategoryElementId AS SegCategoryElementId, @SegNode1 AS SegNode1, @SegNode2 AS SegNode2 ,@SegNode3 AS SegNode3 ,@SegNode4 AS SegNode4
SELECT @SysCategoryElementId AS SysCategoryElementId, @SysNode1 AS SysNode1 ,@SysNode2 AS SysNode2 ,@SysNode3 AS SysNode3 ,@SysNode4 AS SysNode4

--EXECUTE STORED PROCEDURE
EXEC [Admin].[usp_GetGraphicalNodeConnection] 'Segmento','Automation_Segement143',''

/* OUTPUT
SourceNodeId	DestinationNodeId	IsActive	IsTransfer
5559	            5560	           1	       0
5559	            5561	           1	       0
5560	            5561	           1	       0
5561	            5559	           1	       0
5561	            5562	           1	       0
5562	            5559	           1	       0
5562	            5560	           1	       0
*/

EXEC [Admin].[usp_GetGraphicalNodeConnection] 'Sistema','Automation_System143',''

/* OUTPUT
SourceNodeId	DestinationNodeId	IsActive	IsTransfer
5563	            5564	          1	            0
5563	            5565	          1	            0
5564	            5565	          1	            0
5565	            5563	          1	            0
5565	            5566	          1	            0
5566	            5563	          1	            0
5566	            5564	          1	            0
*/
