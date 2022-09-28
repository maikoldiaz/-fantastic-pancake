/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	MAR-13-2020

-- Description:     These test cases are for stored procedure [Admin].[usp_GetGraphicalSourceNodesDetails]

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
EXEC [Admin].[usp_GetGraphicalSourceNodesDetails] 6592

/* OUTPUT

NodeId	NodeName		AcceptableBalancePercentage	ControlLimit	Segment		Operador	NodeType	SegmentColor	NodeTypeIcon	IsActive
6590	Node_devtest01		NULL						2.99		Producción	NULL		Oleoducto	#08956C			<svg id="guias" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 150 150"><title>icon</title><path d="M80.5,105.65c-4.63,0-8.39-3.31-8.39-7.38a1,1,0,0,1,2,0c0,3,2.86,5.38,6.38,5.38s6.4-2.41,6.4-5.38a1,1,0,0,1,2,0C88.89,102.34,85.12,105.65,80.5,105.65Z" style="fill:#87d029"/><path d="M80.55,113.14a12.74,12.74,0,0,1-12.29-9.86,3.11,3.11,0,0,0-.16-.49h-.16c-3.14,0-6.27,0-9.41,0h-4v6.28a1,1,0,0,1-1,1H49.17a1,1,0,0,1-1-1v-4.56H48v4.55a1,1,0,0,1-1,1H42.75a1,1,0,0,1-1-.94l0-.44c0-.32,0-.63,0-.94q0-13.2,0-26.41a2.8,2.8,0,0,1,.55-2.17,2.83,2.83,0,0,1,2.18-.5h1l1.37,0a1.28,1.28,0,0,1,.67.23,1.13,1.13,0,0,1,.56.91v3.67h.17V79.84a1,1,0,0,1,1-1h4.28a1,1,0,0,1,1,1v6.08H67.78V78.29h-7.1a1,1,0,0,1-1-1V72.92a1,1,0,0,1,1-1h2.17v-.79H60.7a1,1,0,0,1-1-1V65.5a1,1,0,0,1,1-1h8.66V62.24a1,1,0,0,1,1-1l20.37,0a1,1,0,0,1,1,1V64.4h9.48a1,1,0,0,1,1,1v4.73a1,1,0,0,1-1,1H99v.79h2.19a1,1,0,0,1,1,1v4.3a1,1,0,0,1-1,1H93.36V85.9h13.26V79.79a1,1,0,0,1,1-1H112a1,1,0,0,1,1,1V84h.18V79.82a1,1,0,0,1,1-1h4.13a1,1,0,0,1,1,1V109.1a1,1,0,0,1-1,1h-4.13a1,1,0,0,1-1-1V105H113v4a1,1,0,0,1-1,1h-4.31a1,1,0,0,1-1-1v-6.23h-4.08c-3.08,0-6.16,0-9.23,0a1.14,1.14,0,0,0-.39,0h0a1,1,0,0,0-.16.43,12.67,12.67,0,0,1-12.26,9.91ZM68,100.79a2.2,2.2,0,0,1,2.25,2,10.74,10.74,0,0,0,10.35,8.34h0a10.54,10.54,0,0,0,10.32-8.38,2.31,2.31,0,0,1,2.47-2h.05c3.07,0,6.14,0,9.22,0h3.88a8.17,8.17,0,0,1,.82,0l.44,0a1,1,0,0,1,.93,1v6.16H111v-4a1,1,0,0,1,1-1h2.18a1,1,0,0,1,1,1v4.09h2.13V80.82h-2.13V85a1,1,0,0,1-1,1H112a1,1,0,0,1-1-1v-4.2h-2.38V86.9a1,1,0,0,1-1,1H92.36a1,1,0,0,1-1-1V77.22a1,1,0,0,1,1-1h7.85v-2.3H98a1,1,0,0,1-1-1V70.13a1,1,0,0,1,1-1h2.19V66.4H90.73a1,1,0,0,1-1-1V63.21l-12.3,0H71.36V65.5a1,1,0,0,1-1,1H61.7v2.63h2.15a1,1,0,0,1,1,1v2.79a1,1,0,0,1-1,1H61.68v2.37h7.1a1,1,0,0,1,1,1v9.63a1,1,0,0,1-1,1H53.45a1,1,0,0,1-1-1V80.84H50.17v3.67a1,1,0,0,1-1,1H47a1,1,0,0,1-1-1V80.69H44.36c-.16,0-.47,0-.68,0,0,.21,0,.52,0,.67q0,13.2,0,26.41c0,.1,0,.21,0,.32H46v-4.55a1,1,0,0,1,1-1h2.17a1,1,0,0,1,1,1v4.56h2.35V101.9a1,1,0,0,1,.85-1l.24,0a3.16,3.16,0,0,1,.56-.07h4.36c3.13,0,6.27,0,9.4,0ZM46.73,80.7h0Z" style="fill:#87d029"/><path d="M91.87,78.11H68.17a1,1,0,0,1,0-2h23.7a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M93,87.68H67.6a1,1,0,0,1,0-2H93a1,1,0,1,1,0,2Z" style="fill:#87d029"/><path d="M48.11,100.57a1,1,0,0,1-1-1V88.08a1,1,0,0,1,2,0V99.57A1,1,0,0,1,48.11,100.57Z" style="fill:#87d029"/><path d="M113.18,100.57a1,1,0,0,1-1-1V88.66a1,1,0,0,1,2,0V99.57A1,1,0,0,1,113.18,100.57Z" style="fill:#87d029"/><path d="M80.53,62.89a1,1,0,0,1-1-1V43.73a1,1,0,1,1,2,0V61.89A1,1,0,0,1,80.53,62.89Z" style="fill:#87d029"/><path d="M99.48,50.57H62.37a1,1,0,0,1,0-2H99.48a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M87.32,53.1H74.21a1,1,0,0,1,0-2H87.32a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M94.57,71.77H66.34a1,1,0,0,1,0-2H94.57a1,1,0,0,1,0,2Z" style="fill:#87d029"/></svg>	1
6591	Node_devtest02		NULL						8.99		NULL		FRONTERA	NULL		NULL			NULL																																																																																																																																																																																																																																																																																																																																																									1
6592	Node_devtest03		NULL						7.99		Producción	FRONTERA	NULL	#	08956C			NULL																																																																																																																																																																																																																																																																																																																																																									1
*/

-- Setting the IsDeleted to Inactive, therefore that NodeId will not be displayed for the Inactive Connection

UPDATE Admin.NodeConnection SET IsDeleted = 1 WHERE NodeConnectionId = 2660

EXEC [Admin].[usp_GetGraphicalSourceNodesDetails] 6592

/* OUTPUT

NodeId	NodeName	AcceptableBalancePercentage	ControlLimit	Segment		Operador	NodeType	SegmentColor	NodeTypeIcon	IsActive
6590	Node_devtest01	NULL						2.99		Producción	NULL		Oleoducto	#08956C	<svg id="guias" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 150 150"><title>icon</title><path d="M80.5,105.65c-4.63,0-8.39-3.31-8.39-7.38a1,1,0,0,1,2,0c0,3,2.86,5.38,6.38,5.38s6.4-2.41,6.4-5.38a1,1,0,0,1,2,0C88.89,102.34,85.12,105.65,80.5,105.65Z" style="fill:#87d029"/><path d="M80.55,113.14a12.74,12.74,0,0,1-12.29-9.86,3.11,3.11,0,0,0-.16-.49h-.16c-3.14,0-6.27,0-9.41,0h-4v6.28a1,1,0,0,1-1,1H49.17a1,1,0,0,1-1-1v-4.56H48v4.55a1,1,0,0,1-1,1H42.75a1,1,0,0,1-1-.94l0-.44c0-.32,0-.63,0-.94q0-13.2,0-26.41a2.8,2.8,0,0,1,.55-2.17,2.83,2.83,0,0,1,2.18-.5h1l1.37,0a1.28,1.28,0,0,1,.67.23,1.13,1.13,0,0,1,.56.91v3.67h.17V79.84a1,1,0,0,1,1-1h4.28a1,1,0,0,1,1,1v6.08H67.78V78.29h-7.1a1,1,0,0,1-1-1V72.92a1,1,0,0,1,1-1h2.17v-.79H60.7a1,1,0,0,1-1-1V65.5a1,1,0,0,1,1-1h8.66V62.24a1,1,0,0,1,1-1l20.37,0a1,1,0,0,1,1,1V64.4h9.48a1,1,0,0,1,1,1v4.73a1,1,0,0,1-1,1H99v.79h2.19a1,1,0,0,1,1,1v4.3a1,1,0,0,1-1,1H93.36V85.9h13.26V79.79a1,1,0,0,1,1-1H112a1,1,0,0,1,1,1V84h.18V79.82a1,1,0,0,1,1-1h4.13a1,1,0,0,1,1,1V109.1a1,1,0,0,1-1,1h-4.13a1,1,0,0,1-1-1V105H113v4a1,1,0,0,1-1,1h-4.31a1,1,0,0,1-1-1v-6.23h-4.08c-3.08,0-6.16,0-9.23,0a1.14,1.14,0,0,0-.39,0h0a1,1,0,0,0-.16.43,12.67,12.67,0,0,1-12.26,9.91ZM68,100.79a2.2,2.2,0,0,1,2.25,2,10.74,10.74,0,0,0,10.35,8.34h0a10.54,10.54,0,0,0,10.32-8.38,2.31,2.31,0,0,1,2.47-2h.05c3.07,0,6.14,0,9.22,0h3.88a8.17,8.17,0,0,1,.82,0l.44,0a1,1,0,0,1,.93,1v6.16H111v-4a1,1,0,0,1,1-1h2.18a1,1,0,0,1,1,1v4.09h2.13V80.82h-2.13V85a1,1,0,0,1-1,1H112a1,1,0,0,1-1-1v-4.2h-2.38V86.9a1,1,0,0,1-1,1H92.36a1,1,0,0,1-1-1V77.22a1,1,0,0,1,1-1h7.85v-2.3H98a1,1,0,0,1-1-1V70.13a1,1,0,0,1,1-1h2.19V66.4H90.73a1,1,0,0,1-1-1V63.21l-12.3,0H71.36V65.5a1,1,0,0,1-1,1H61.7v2.63h2.15a1,1,0,0,1,1,1v2.79a1,1,0,0,1-1,1H61.68v2.37h7.1a1,1,0,0,1,1,1v9.63a1,1,0,0,1-1,1H53.45a1,1,0,0,1-1-1V80.84H50.17v3.67a1,1,0,0,1-1,1H47a1,1,0,0,1-1-1V80.69H44.36c-.16,0-.47,0-.68,0,0,.21,0,.52,0,.67q0,13.2,0,26.41c0,.1,0,.21,0,.32H46v-4.55a1,1,0,0,1,1-1h2.17a1,1,0,0,1,1,1v4.56h2.35V101.9a1,1,0,0,1,.85-1l.24,0a3.16,3.16,0,0,1,.56-.07h4.36c3.13,0,6.27,0,9.4,0ZM46.73,80.7h0Z" style="fill:#87d029"/><path d="M91.87,78.11H68.17a1,1,0,0,1,0-2h23.7a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M93,87.68H67.6a1,1,0,0,1,0-2H93a1,1,0,1,1,0,2Z" style="fill:#87d029"/><path d="M48.11,100.57a1,1,0,0,1-1-1V88.08a1,1,0,0,1,2,0V99.57A1,1,0,0,1,48.11,100.57Z" style="fill:#87d029"/><path d="M113.18,100.57a1,1,0,0,1-1-1V88.66a1,1,0,0,1,2,0V99.57A1,1,0,0,1,113.18,100.57Z" style="fill:#87d029"/><path d="M80.53,62.89a1,1,0,0,1-1-1V43.73a1,1,0,1,1,2,0V61.89A1,1,0,0,1,80.53,62.89Z" style="fill:#87d029"/><path d="M99.48,50.57H62.37a1,1,0,0,1,0-2H99.48a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M87.32,53.1H74.21a1,1,0,0,1,0-2H87.32a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M94.57,71.77H66.34a1,1,0,0,1,0-2H94.57a1,1,0,0,1,0,2Z" style="fill:#87d029"/></svg>	1
6592	Node_devtest03	NULL						7.99		Producción	FRONTERA	NULL		#08956C				NULL	1
*/