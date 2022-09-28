/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	MAR-13-2020

-- Description:     These test cases are for stored procedure [Admin].[usp_GetGraphicalDestinationNodeConnectionsDetails]

-- Database backup Used:	appdb_dev_0313
-- ==============================================================================================================================*/

/*
The below insert statements are required to populate data into 
[Node]
[NodeTag]
[NodeConnection]
*/


-- INSERT INTO NODE TABLE 

INSERT INTO Admin.Node (Name,Description,IsActive,SendToSAP,ControlLimit,[Order],UnitId,Capacity,CreatedBy,CreatedDate)
VALUES('Node_devtest01','Node created for testing',1,0,2.99,09,null,190,'System',GETDATE())

INSERT INTO Admin.Node (Name,Description,IsActive,SendToSAP,ControlLimit,[Order],UnitId,Capacity,CreatedBy,CreatedDate)
VALUES('Node_devtest02','Node created for testing',1,0,8.99,98,null,390,'System',GETDATE())

INSERT INTO Admin.Node (Name,Description,IsActive,SendToSAP,ControlLimit,[Order],UnitId,Capacity,CreatedBy,CreatedDate)
VALUES('Node_devtest03','Node created for testing',1,0,7.99,70,null,90,'System',GETDATE())

INSERT INTO Admin.Node (Name,Description,IsActive,SendToSAP,ControlLimit,[Order],UnitId,Capacity,CreatedBy,CreatedDate)
VALUES('Node_devtest04','Node created for testing',1,0,2.87,20,null,40,'System',GETDATE())

INSERT INTO Admin.Node (Name,Description,IsActive,SendToSAP,ControlLimit,[Order],UnitId,Capacity,CreatedBy,CreatedDate)
VALUES('Node_devtest05','Node created for testing',1,0,0.99,90,null,50,'System',GETDATE())


SELECT * from Admin.CategoryElement WHERE CategoryId in (1,2,3,8)
/*
CategoryId	Name			ElementId			ElementName
1			Tipo de Nodo	2					Oleoducto
2			Segmento		11					Producción
3			Operador		20					FRONTERA
8			Sistema			9868				SystemElement3
*/

-- INSERT INTO NODE TAG TABLE --

SELECT * FROM Admin.NodeTag ORDER BY CreatedDate DESC
INSERT INTO Admin.NodeTag (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
VALUES(6590,2,'2020-03-10 00:00:00.000','9999-12-31 00:00:00.000','System',GETDATE())
INSERT INTO Admin.NodeTag (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
VALUES(6590,11,'2020-03-10 00:00:00.000','9999-12-31 00:00:00.000','System',GETDATE())
INSERT INTO Admin.NodeTag (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
VALUES(6590,9868,'2020-03-10 00:00:00.000','9999-12-31 00:00:00.000','System',GETDATE())
INSERT INTO Admin.NodeTag (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
VALUES(6591,20,'2020-03-10 00:00:00.000','9999-12-31 00:00:00.000','System',GETDATE())
INSERT INTO Admin.NodeTag (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
VALUES(6592,20,'2020-03-10 00:00:00.000','9999-12-31 00:00:00.000','System',GETDATE())
INSERT INTO Admin.NodeTag (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
VALUES(6592,11,'2020-03-10 00:00:00.000','9999-12-31 00:00:00.000','System',GETDATE())
INSERT INTO Admin.NodeTag (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
VALUES(6593,2,'2020-03-10 00:00:00.000','9999-12-31 00:00:00.000','System',GETDATE())
INSERT INTO Admin.NodeTag (NodeId,ElementId,StartDate,EndDate,CreatedBy,CreatedDate)
VALUES(6594,11,'2020-03-10 00:00:00.000','9999-12-31 00:00:00.000','System',GETDATE())

-- INSERT DATA INTO NodeConnection -- 
SELECT * FROM Admin.NodeConnection ORDER BY CreatedDate DESC 
INSERT INTO Admin.NodeConnection(SourceNodeId,DestinationNodeId,Description,CreatedBy,CreatedDate)
VALUES (6590,6592,'Node_devtest01_Node_devtest03','System',GETDATE())
INSERT INTO Admin.NodeConnection(SourceNodeId,DestinationNodeId,Description,CreatedBy,CreatedDate)
VALUES (6591,6593,'Node_devtest02_Node_devtest04','System',GETDATE())
INSERT INTO Admin.NodeConnection(SourceNodeId,DestinationNodeId,Description,CreatedBy,CreatedDate)
VALUES (6591,6592,'Node_devtest02_Node_devtest03','System',GETDATE())
INSERT INTO Admin.NodeConnection(SourceNodeId,DestinationNodeId,Description,CreatedBy,CreatedDate)
VALUES (6593,6594,'Node_devtest04_Node_devtest05','System',GETDATE())

--EXECUTE STORED PROCEDURE
EXEC [Admin].[usp_GetGraphicalDestinationNodeConnectionsDetails] 6591

/* OUTPUT

SourceNodeId	DestinationNodeId	State
6590			6592				Active
6591			6592				Active
6591			6593				Active
6593			6594				Active																																																																																																																																																																																																																																																																																																																																																									1
*/

-- Setting the IsDeleted to Inactive, therefore the Inactive Connection will not be displayed

UPDATE Admin.NodeConnection SET IsDeleted = 1 WHERE NodeConnectionId = 2661

EXEC [Admin].[usp_GetGraphicalDestinationNodeConnectionsDetails] 6591

/* OUTPUT

SourceNodeId	DestinationNodeId	State
6590			6592				Active
6591			6592				Active
6591			6593				Active

*/