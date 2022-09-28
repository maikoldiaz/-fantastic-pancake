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
*/
------------------ INSERT DATA INTO CATEGORY ELEMENT/ NODE/ NodeTag/ NodeConnection/ Icon/ Color TABLES ---------------------------------------
DECLARE @SegCategoryElementId INT,@SegNode1 INT,@SegNode2 INT, @SegNode3 INT, @SegNode4 INT
       ,@SysCategoryElementId INT,@SysNode1 INT,@SysNode2 INT, @SysNode3 INT, @SysNode4 INT
--------------################# SEGEMENT #####################-------------------
INSERT INTO [Admin].[CategoryElement] ([Name],[Description],CategoryId,Color,IsActive,CreatedBy,CreatedDate)  
                               VALUES ('Automation_Segement999','Vijay_Category_Segment','#F08080',2,1,'Vijay',GETDATE()) --> CategoryId
SELECT @SegCategoryElementId = SCOPE_IDENTITY(); 

--########--NODE
INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node1_Segement999','Vijay_Node',4033,1,0,'0.1','0.2',1,'Vijay','Vijay',GETDATE()) --> SegmentNodeId 1
SELECT @SegNode1 = SCOPE_IDENTITY();

INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node2_Segement999','Vijay_Node',4033,1,0,'0.2','0.2',1,'Vijay','Vijay',GETDATE()) --> SegmentNodeId 2
SELECT @SegNode2 = SCOPE_IDENTITY();

INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node3_Segement999','Vijay_Node',4033,1,0,'0.3','0.2',1,'Vijay','Vijay',GETDATE()) --> SegmentNodeId 3
SELECT @SegNode3 = SCOPE_IDENTITY();

INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node4_Segement999','Vijay_Node',4033,1,0,'0.4','0.4',1,'Vijay','Vijay',GETDATE()) --> SegmentNodeId 4
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

--------------################# SYSTEM #####################-------------------
INSERT INTO [Admin].[CategoryElement] (Name,Description,CategoryId,Color,IsActive,CreatedBy,CreatedDate)  
                               VALUES ('Automation_System999','Vijay_Category_System','#FFA07A',8,1,'Vijay',GETDATE())
SELECT @SysCategoryElementId = SCOPE_IDENTITY(); 

--########--NODE
INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node1_System999','Vijay_Node',4033,1,0,'0.2','0.3',1,'Vijay','Vijay',GETDATE())
SELECT @SysNode1 = SCOPE_IDENTITY();

INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node2_System999','Vijay_Node',4033,1,0,'0.3','0.4',1,'Vijay','Vijay',GETDATE())
SELECT @SysNode2 = SCOPE_IDENTITY();

INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node3_System999','Vijay_Node',4033,1,0,'0.4','0.5',1,'Vijay','Vijay',GETDATE())
SELECT @SysNode3 = SCOPE_IDENTITY();

INSERT INTO [Admin].[Node] (Name,Description,LogisticCenterId,IsActive,SendToSAP,ControlLimit,AcceptableBalancePercentage,[Order],CreatedBy,LastModifiedBy,LastModifiedDate) 
                    VALUES ('Node4_System999','Vijay_Node',4033,1,0,'0.5','0.6',1,'Vijay','Vijay',GETDATE())
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
EXEC [Admin].[usp_GetGraphicalNode] 'Segmento','Automation_Segement999',''

/*OUTPUT
NodeId	NodeName	        AcceptableBalancePercentage	ControlLimit	Segment	            Operador	NodeType	SegmentColor	NodeTypeIcon	IsActive	Input	Output
5575	Node1_Segement999	0.20	                      0.10	     Automation_Segement999	ECOPETROL	Facilidad	      #F08080	<svg id="guias" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 150 150"><title>icon</title><path d="M30.06,117.26h0a1,1,0,0,1-1-1l2.3-66.1a1,1,0,0,1,1-1H45.07a1,1,0,0,1,1,1l1.08,32,3.5-.84c4.1-1,8.08-1.93,12.05-2.9l.28-.08-.14-.14c-.28-.23-.8-.68-.77-17.44V59.7c0-1.73,0-3.45,0-5.18,0-.61.36-1.65,1.23-1.84l.13,0v0a3.26,3.26,0,0,1-.19-.61,6.91,6.91,0,0,0-.47-1.36l-.33-.81a1,1,0,0,1,.09-.89,1,1,0,0,1,.77-.46l1.75-.14.14-1.08a1,1,0,0,1,1-.88h6.12a1,1,0,0,1,1,.91l.1,1.08,1.57.06a1,1,0,0,1,.78.43,1,1,0,0,1,.12.89c-.09.28-.19.56-.29.83a11.29,11.29,0,0,0-.49,1.54c0,.15-.09.31-.14.47l0,.05.13,0a1.91,1.91,0,0,1,1.31,1.8c0,7.62,0,16.62,0,25.61a1,1,0,0,1-1,1h0a1,1,0,0,1-1-1c.06-9,.05-17.92,0-25.51a2.05,2.05,0,0,1-1.3-.9,2,2,0,0,1,0-1.72c0-.08.05-.16.07-.23a12.12,12.12,0,0,1,.39-1.34l-1.08,0a1,1,0,0,1-1-.91l-.1-1H67.08l-.12,1a1,1,0,0,1-.92.87l-1.2.1a6.78,6.78,0,0,1,.39,1.28,1.44,1.44,0,0,0,.08.25,2,2,0,0,1,0,1.74,2.09,2.09,0,0,1-1.25.91v0c0,1.7,0,3.42,0,5.13v1.07c0,2.79.11,14.22.33,16.17a2,2,0,0,1,.65,1.89,2.25,2.25,0,0,1-1.87,1.55l-12.06,2.9-4.7,1.12a1,1,0,0,1-.83-.17,1,1,0,0,1-.4-.76l-1.08-32.3H33.32L31.06,116.3A1,1,0,0,1,30.06,117.26Zm33-38.75Zm1-1.82h0ZM74.38,54.47h0ZM63.69,52.56Zm11,0Zm40.44,64.71a1,1,0,0,1-1-1V80.49L90.09,86.31a1,1,0,1,1-.47-1.94l25.32-6.12a1,1,0,0,1,.85.18,1,1,0,0,1,.38.79v37A1,1,0,0,1,115.17,117.26ZM96.26,82a1,1,0,0,1-1-1c0-1,0-9.08,0-16.18,0-4.94,0-9.6,0-10.32h0A1.76,1.76,0,0,1,93.92,52a1.15,1.15,0,0,0,.06-.21,5.92,5.92,0,0,1,.46-1.46l-1.2-.13a1,1,0,0,1-.89-.94l-.06-.91H88l-.12,1a1,1,0,0,1-.9.87l-1.26.12a5.51,5.51,0,0,1,.46,1.33,1.11,1.11,0,0,0,.07.22,2,2,0,0,1,0,1.7,2.06,2.06,0,0,1-1.31.94.16.16,0,0,0,0,.08c-.05.93,0,1.9,0,2.85v1h0s.06,14.15.06,20.19a1,1,0,1,1-2,0c0-6-.06-20.19-.06-20.19v-1c0-1,0-2,0-3a2.08,2.08,0,0,1,1.36-1.84l.1,0v0a3.44,3.44,0,0,1-.16-.53,5.44,5.44,0,0,0-.53-1.34c-.13-.27-.26-.54-.39-.84a1,1,0,0,1,.06-.9,1,1,0,0,1,.77-.49L86,48.33l.13-1.08a1,1,0,0,1,1-.88h6.14a1,1,0,0,1,1,.94l.06,1,1.74.19a1,1,0,0,1,.75.48,1,1,0,0,1,.07.89c-.12.28-.25.56-.37.82A6,6,0,0,0,96,52.1a2.46,2.46,0,0,1-.14.53v0l.14,0c1,.13,1.26,1.42,1.27,1.87,0,.74,0,5.2,0,10.36,0,6.73,0,15.11,0,16.1a1,1,0,0,1-1,1Zm-.68-29.46Zm-11,0Z" style="fill:#87d029"/><path d="M45.67,48.37a1,1,0,0,1-.93-.64,5.58,5.58,0,0,1,.16-5C45.92,41,48,39.91,51,39.5c.93-.13,1.87-.24,2.8-.36,1.72-.21,3.5-.43,5.21-.73,2.5-.45,4-1.28,4.68-2.62a1,1,0,0,1,1.78.92c-1.31,2.55-4.12,3.31-6.11,3.67-1.76.31-3.57.53-5.31.75-.93.11-1.85.22-2.77.35-2.37.33-4,1.12-4.69,2.3a3.66,3.66,0,0,0,0,3.23A1,1,0,0,1,46,48.3.92.92,0,0,1,45.67,48.37Z" style="fill:#87d029"/><path d="M57.26,81.51a1,1,0,0,1-1-1V68.65a2.87,2.87,0,0,1,2.77-3h3.65a1,1,0,0,1,0,2H59a.89.89,0,0,0-.77,1V80.51A1,1,0,0,1,57.26,81.51Z" style="fill:#87d029"/><path d="M78.31,81.24a1,1,0,0,1-1-1V68.38a2.86,2.86,0,0,1,2.77-2.95h3.64a1,1,0,0,1,0,2H80.08a.88.88,0,0,0-.77.95V80.24A1,1,0,0,1,78.31,81.24Z" style="fill:#87d029"/><path d="M102.31,83.24a1,1,0,0,1-1-1V68.38a.88.88,0,0,0-.77-.95H96.89a1,1,0,1,1,0-2h3.65a2.86,2.86,0,0,1,2.77,2.95V82.24A1,1,0,0,1,102.31,83.24Z" style="fill:#87d029"/><path d="M64.59,83.83a1,1,0,0,1-.19-2l27.34-5.19a1,1,0,0,1,.37,2l-27.33,5.2Z" style="fill:#87d029"/><path d="M48.43,103H40.35a1,1,0,0,1-1-1V95.64a1,1,0,0,1,1-1h8.08a1,1,0,0,1,1,1V102A1,1,0,0,1,48.43,103Zm-7.08-2h6.08V96.64H41.35Z" style="fill:#87d029"/><path d="M77.79,103H69.57a1,1,0,0,1-1-1V95.67a1,1,0,0,1,1-1h8.22a1,1,0,0,1,1,1V102A1,1,0,0,1,77.79,103Zm-7.22-2h6.22V96.67H70.57Z" style="fill:#87d029"/><path d="M92.36,103H84.3a1,1,0,0,1-1-1v-6.4a1,1,0,0,1,1-1h8.06a1,1,0,0,1,1,1V102A1,1,0,0,1,92.36,103Zm-7.06-2h6.06v-4.4H85.3Z" style="fill:#87d029"/><path d="M63.09,103H55a1,1,0,0,1-1-1V95.63a1,1,0,0,1,1-1h8.11a1,1,0,0,1,1,1V102A1,1,0,0,1,63.09,103ZM56,101h6.11V96.63H56Z" style="fill:#87d029"/><path d="M107,103H98.93a1,1,0,0,1-1-1V95.64a1,1,0,0,1,1-1H107a1,1,0,0,1,1,1V102A1,1,0,0,1,107,103Zm-7.09-2H106V96.64H99.93Z" style="fill:#87d029"/><path d="M130.6,119h-112a1,1,0,0,1,0-2h112a1,1,0,0,1,0,2Z" style="fill:#87d029"/></svg>	1	2	2
5576	Node2_Segement999	0.20	                      0.20	     Automation_Segement999	CENIT	    Oleoducto	      #F08080	<svg id="guias" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 150 150"><title>icon</title><path d="M80.5,105.65c-4.63,0-8.39-3.31-8.39-7.38a1,1,0,0,1,2,0c0,3,2.86,5.38,6.38,5.38s6.4-2.41,6.4-5.38a1,1,0,0,1,2,0C88.89,102.34,85.12,105.65,80.5,105.65Z" style="fill:#87d029"/><path d="M80.55,113.14a12.74,12.74,0,0,1-12.29-9.86,3.11,3.11,0,0,0-.16-.49h-.16c-3.14,0-6.27,0-9.41,0h-4v6.28a1,1,0,0,1-1,1H49.17a1,1,0,0,1-1-1v-4.56H48v4.55a1,1,0,0,1-1,1H42.75a1,1,0,0,1-1-.94l0-.44c0-.32,0-.63,0-.94q0-13.2,0-26.41a2.8,2.8,0,0,1,.55-2.17,2.83,2.83,0,0,1,2.18-.5h1l1.37,0a1.28,1.28,0,0,1,.67.23,1.13,1.13,0,0,1,.56.91v3.67h.17V79.84a1,1,0,0,1,1-1h4.28a1,1,0,0,1,1,1v6.08H67.78V78.29h-7.1a1,1,0,0,1-1-1V72.92a1,1,0,0,1,1-1h2.17v-.79H60.7a1,1,0,0,1-1-1V65.5a1,1,0,0,1,1-1h8.66V62.24a1,1,0,0,1,1-1l20.37,0a1,1,0,0,1,1,1V64.4h9.48a1,1,0,0,1,1,1v4.73a1,1,0,0,1-1,1H99v.79h2.19a1,1,0,0,1,1,1v4.3a1,1,0,0,1-1,1H93.36V85.9h13.26V79.79a1,1,0,0,1,1-1H112a1,1,0,0,1,1,1V84h.18V79.82a1,1,0,0,1,1-1h4.13a1,1,0,0,1,1,1V109.1a1,1,0,0,1-1,1h-4.13a1,1,0,0,1-1-1V105H113v4a1,1,0,0,1-1,1h-4.31a1,1,0,0,1-1-1v-6.23h-4.08c-3.08,0-6.16,0-9.23,0a1.14,1.14,0,0,0-.39,0h0a1,1,0,0,0-.16.43,12.67,12.67,0,0,1-12.26,9.91ZM68,100.79a2.2,2.2,0,0,1,2.25,2,10.74,10.74,0,0,0,10.35,8.34h0a10.54,10.54,0,0,0,10.32-8.38,2.31,2.31,0,0,1,2.47-2h.05c3.07,0,6.14,0,9.22,0h3.88a8.17,8.17,0,0,1,.82,0l.44,0a1,1,0,0,1,.93,1v6.16H111v-4a1,1,0,0,1,1-1h2.18a1,1,0,0,1,1,1v4.09h2.13V80.82h-2.13V85a1,1,0,0,1-1,1H112a1,1,0,0,1-1-1v-4.2h-2.38V86.9a1,1,0,0,1-1,1H92.36a1,1,0,0,1-1-1V77.22a1,1,0,0,1,1-1h7.85v-2.3H98a1,1,0,0,1-1-1V70.13a1,1,0,0,1,1-1h2.19V66.4H90.73a1,1,0,0,1-1-1V63.21l-12.3,0H71.36V65.5a1,1,0,0,1-1,1H61.7v2.63h2.15a1,1,0,0,1,1,1v2.79a1,1,0,0,1-1,1H61.68v2.37h7.1a1,1,0,0,1,1,1v9.63a1,1,0,0,1-1,1H53.45a1,1,0,0,1-1-1V80.84H50.17v3.67a1,1,0,0,1-1,1H47a1,1,0,0,1-1-1V80.69H44.36c-.16,0-.47,0-.68,0,0,.21,0,.52,0,.67q0,13.2,0,26.41c0,.1,0,.21,0,.32H46v-4.55a1,1,0,0,1,1-1h2.17a1,1,0,0,1,1,1v4.56h2.35V101.9a1,1,0,0,1,.85-1l.24,0a3.16,3.16,0,0,1,.56-.07h4.36c3.13,0,6.27,0,9.4,0ZM46.73,80.7h0Z" style="fill:#87d029"/><path d="M91.87,78.11H68.17a1,1,0,0,1,0-2h23.7a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M93,87.68H67.6a1,1,0,0,1,0-2H93a1,1,0,1,1,0,2Z" style="fill:#87d029"/><path d="M48.11,100.57a1,1,0,0,1-1-1V88.08a1,1,0,0,1,2,0V99.57A1,1,0,0,1,48.11,100.57Z" style="fill:#87d029"/><path d="M113.18,100.57a1,1,0,0,1-1-1V88.66a1,1,0,0,1,2,0V99.57A1,1,0,0,1,113.18,100.57Z" style="fill:#87d029"/><path d="M80.53,62.89a1,1,0,0,1-1-1V43.73a1,1,0,1,1,2,0V61.89A1,1,0,0,1,80.53,62.89Z" style="fill:#87d029"/><path d="M99.48,50.57H62.37a1,1,0,0,1,0-2H99.48a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M87.32,53.1H74.21a1,1,0,0,1,0-2H87.32a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M94.57,71.77H66.34a1,1,0,0,1,0-2H94.57a1,1,0,0,1,0,2Z" style="fill:#87d029"/></svg>	1	1	2
5577	Node3_Segement999	0.20	                      0.30	     Automation_Segement999	NULL	    Estación	      #F08080	<svg id="guias" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 150 150"><title>icon</title><path d="M110.42,69.34H39.58a1,1,0,0,1-1-1V62.57a1,1,0,0,1,1-1h70.84a1,1,0,0,1,1,1v5.77A1,1,0,0,1,110.42,69.34Zm-69.84-2h68.84V63.57H40.58Z" style="fill:#87d029"/><path d="M110.42,110.67a1,1,0,0,1-1-1V63.57H40.58v46.1a1,1,0,0,1-2,0V62.57a1,1,0,0,1,1-1h70.84a1,1,0,0,1,1,1v47.1A1,1,0,0,1,110.42,110.67Z" style="fill:#87d029"/><path d="M101.5,109.37a1,1,0,0,1-1-1v-39h-51v39a1,1,0,0,1-2,0v-40a1,1,0,0,1,1-1h53a1,1,0,0,1,1,1v40A1,1,0,0,1,101.5,109.37Z" style="fill:#87d029"/><path d="M101.5,75.06h-53a1,1,0,0,1,0-2h53a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M101.5,80.77h-53a1,1,0,1,1,0-2h53a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M101.5,86.49h-53a1,1,0,0,1,0-2h53a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M101.5,92.21h-53a1,1,0,1,1,0-2h53a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M101.5,97.93h-53a1,1,0,0,1,0-2h53a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M101.5,103.65h-53a1,1,0,0,1,0-2h53a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M110.42,63.57H39.58a1,1,0,0,1-1-1V54.7a1,1,0,0,1,.65-.94L74.65,40.69a1.05,1.05,0,0,1,.7,0l35.42,13.07a1,1,0,0,1,.65.94v7.87A1,1,0,0,1,110.42,63.57Zm-69.84-2h68.84V55.39L75,42.7,40.58,55.39Z" style="fill:#87d029"/><path d="M139.66,111H10.34a1,1,0,0,1,0-2H139.66a1,1,0,0,1,0,2Z" style="fill:#87d029"/></svg>	1	2	2
5578	Node4_Segement999	0.40	                      0.40	     Automation_Segement999	OCENSA	    NULL	          #F08080	    NULL	              1	2	1
*/


EXEC [Admin].[usp_GetGraphicalNode] 'Sistema','Automation_System999',''

/*OUTPUT
NodeId	NodeName	    AcceptableBalancePercentage	ControlLimit	Segment	          Operador	NodeType	SegmentColor	NodeTypeIcon	IsActive	Input	Output
5579	Node1_System999	0.30	                        0.20	Automation_System999	CENIT	Oleoducto	#FFA07A	         <svg id="guias" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 150 150"><title>icon</title><path d="M80.5,105.65c-4.63,0-8.39-3.31-8.39-7.38a1,1,0,0,1,2,0c0,3,2.86,5.38,6.38,5.38s6.4-2.41,6.4-5.38a1,1,0,0,1,2,0C88.89,102.34,85.12,105.65,80.5,105.65Z" style="fill:#87d029"/><path d="M80.55,113.14a12.74,12.74,0,0,1-12.29-9.86,3.11,3.11,0,0,0-.16-.49h-.16c-3.14,0-6.27,0-9.41,0h-4v6.28a1,1,0,0,1-1,1H49.17a1,1,0,0,1-1-1v-4.56H48v4.55a1,1,0,0,1-1,1H42.75a1,1,0,0,1-1-.94l0-.44c0-.32,0-.63,0-.94q0-13.2,0-26.41a2.8,2.8,0,0,1,.55-2.17,2.83,2.83,0,0,1,2.18-.5h1l1.37,0a1.28,1.28,0,0,1,.67.23,1.13,1.13,0,0,1,.56.91v3.67h.17V79.84a1,1,0,0,1,1-1h4.28a1,1,0,0,1,1,1v6.08H67.78V78.29h-7.1a1,1,0,0,1-1-1V72.92a1,1,0,0,1,1-1h2.17v-.79H60.7a1,1,0,0,1-1-1V65.5a1,1,0,0,1,1-1h8.66V62.24a1,1,0,0,1,1-1l20.37,0a1,1,0,0,1,1,1V64.4h9.48a1,1,0,0,1,1,1v4.73a1,1,0,0,1-1,1H99v.79h2.19a1,1,0,0,1,1,1v4.3a1,1,0,0,1-1,1H93.36V85.9h13.26V79.79a1,1,0,0,1,1-1H112a1,1,0,0,1,1,1V84h.18V79.82a1,1,0,0,1,1-1h4.13a1,1,0,0,1,1,1V109.1a1,1,0,0,1-1,1h-4.13a1,1,0,0,1-1-1V105H113v4a1,1,0,0,1-1,1h-4.31a1,1,0,0,1-1-1v-6.23h-4.08c-3.08,0-6.16,0-9.23,0a1.14,1.14,0,0,0-.39,0h0a1,1,0,0,0-.16.43,12.67,12.67,0,0,1-12.26,9.91ZM68,100.79a2.2,2.2,0,0,1,2.25,2,10.74,10.74,0,0,0,10.35,8.34h0a10.54,10.54,0,0,0,10.32-8.38,2.31,2.31,0,0,1,2.47-2h.05c3.07,0,6.14,0,9.22,0h3.88a8.17,8.17,0,0,1,.82,0l.44,0a1,1,0,0,1,.93,1v6.16H111v-4a1,1,0,0,1,1-1h2.18a1,1,0,0,1,1,1v4.09h2.13V80.82h-2.13V85a1,1,0,0,1-1,1H112a1,1,0,0,1-1-1v-4.2h-2.38V86.9a1,1,0,0,1-1,1H92.36a1,1,0,0,1-1-1V77.22a1,1,0,0,1,1-1h7.85v-2.3H98a1,1,0,0,1-1-1V70.13a1,1,0,0,1,1-1h2.19V66.4H90.73a1,1,0,0,1-1-1V63.21l-12.3,0H71.36V65.5a1,1,0,0,1-1,1H61.7v2.63h2.15a1,1,0,0,1,1,1v2.79a1,1,0,0,1-1,1H61.68v2.37h7.1a1,1,0,0,1,1,1v9.63a1,1,0,0,1-1,1H53.45a1,1,0,0,1-1-1V80.84H50.17v3.67a1,1,0,0,1-1,1H47a1,1,0,0,1-1-1V80.69H44.36c-.16,0-.47,0-.68,0,0,.21,0,.52,0,.67q0,13.2,0,26.41c0,.1,0,.21,0,.32H46v-4.55a1,1,0,0,1,1-1h2.17a1,1,0,0,1,1,1v4.56h2.35V101.9a1,1,0,0,1,.85-1l.24,0a3.16,3.16,0,0,1,.56-.07h4.36c3.13,0,6.27,0,9.4,0ZM46.73,80.7h0Z" style="fill:#87d029"/><path d="M91.87,78.11H68.17a1,1,0,0,1,0-2h23.7a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M93,87.68H67.6a1,1,0,0,1,0-2H93a1,1,0,1,1,0,2Z" style="fill:#87d029"/><path d="M48.11,100.57a1,1,0,0,1-1-1V88.08a1,1,0,0,1,2,0V99.57A1,1,0,0,1,48.11,100.57Z" style="fill:#87d029"/><path d="M113.18,100.57a1,1,0,0,1-1-1V88.66a1,1,0,0,1,2,0V99.57A1,1,0,0,1,113.18,100.57Z" style="fill:#87d029"/><path d="M80.53,62.89a1,1,0,0,1-1-1V43.73a1,1,0,1,1,2,0V61.89A1,1,0,0,1,80.53,62.89Z" style="fill:#87d029"/><path d="M99.48,50.57H62.37a1,1,0,0,1,0-2H99.48a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M87.32,53.1H74.21a1,1,0,0,1,0-2H87.32a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M94.57,71.77H66.34a1,1,0,0,1,0-2H94.57a1,1,0,0,1,0,2Z" style="fill:#87d029"/></svg>	1	2	2
5580	Node2_System999	0.40	                        0.30	Automation_System999	NULL	Estación	#FFA07A	         <svg id="guias" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 150 150"><title>icon</title><path d="M110.42,69.34H39.58a1,1,0,0,1-1-1V62.57a1,1,0,0,1,1-1h70.84a1,1,0,0,1,1,1v5.77A1,1,0,0,1,110.42,69.34Zm-69.84-2h68.84V63.57H40.58Z" style="fill:#87d029"/><path d="M110.42,110.67a1,1,0,0,1-1-1V63.57H40.58v46.1a1,1,0,0,1-2,0V62.57a1,1,0,0,1,1-1h70.84a1,1,0,0,1,1,1v47.1A1,1,0,0,1,110.42,110.67Z" style="fill:#87d029"/><path d="M101.5,109.37a1,1,0,0,1-1-1v-39h-51v39a1,1,0,0,1-2,0v-40a1,1,0,0,1,1-1h53a1,1,0,0,1,1,1v40A1,1,0,0,1,101.5,109.37Z" style="fill:#87d029"/><path d="M101.5,75.06h-53a1,1,0,0,1,0-2h53a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M101.5,80.77h-53a1,1,0,1,1,0-2h53a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M101.5,86.49h-53a1,1,0,0,1,0-2h53a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M101.5,92.21h-53a1,1,0,1,1,0-2h53a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M101.5,97.93h-53a1,1,0,0,1,0-2h53a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M101.5,103.65h-53a1,1,0,0,1,0-2h53a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M110.42,63.57H39.58a1,1,0,0,1-1-1V54.7a1,1,0,0,1,.65-.94L74.65,40.69a1.05,1.05,0,0,1,.7,0l35.42,13.07a1,1,0,0,1,.65.94v7.87A1,1,0,0,1,110.42,63.57Zm-69.84-2h68.84V55.39L75,42.7,40.58,55.39Z" style="fill:#87d029"/><path d="M139.66,111H10.34a1,1,0,0,1,0-2H139.66a1,1,0,0,1,0,2Z" style="fill:#87d029"/></svg>	1	1	2
5581	Node3_System999	0.50	                        0.40	Automation_System999	OCENSA	NULL	    #FFA07A	         NULL	       1	2	2
5582	Node4_System999	0.60	                        0.50	Automation_System999	OCENSA	Tanque	    #FFA07A	         <svg id="guias" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 150 150"><title>icon</title><path d="M123,120.55H29.64a1,1,0,0,1,0-2H123a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M63.06,83.61h-14a1,1,0,0,1,0-2h14a1,1,0,1,1,0,2Z" style="fill:#87d029"/><path d="M63.06,95.27h-14a1,1,0,0,1,0-2h14a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M63.06,71.94h-14a1,1,0,0,1,0-2h14a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M63.06,60.28h-14a1,1,0,0,1,0-2h14a1,1,0,0,1,0,2Z" style="fill:#87d029"/><path d="M49.08,110.83a1,1,0,0,1-1-1v-68a1,1,0,0,1,2,0v68.05A1,1,0,0,1,49.08,110.83Z" style="fill:#87d029"/><path d="M64.63,110.83a1,1,0,0,1-1-1v-68a1,1,0,0,1,2,0v68.05A1,1,0,0,1,64.63,110.83Z" style="fill:#87d029"/><path d="M99.63,120.55H35.47a1,1,0,0,1-1-1V59.28c0-7.1,14.53-12.67,33.08-12.67s33.08,5.57,33.08,12.67v60.27A1,1,0,0,1,99.63,120.55Zm-63.16-2H98.63V59.28c0-5.15-12.49-10.67-31.08-10.67S36.47,54.13,36.47,59.28Z" style="fill:#87d029"/><path d="M117.13,120.55a1,1,0,0,1-1-1V75.26c0-2.74-5.82-6.2-16.59-7.2A1,1,0,0,1,98.63,67a1,1,0,0,1,1.09-.9c9.16.85,18.41,4,18.41,9.19v44.29A1,1,0,0,1,117.13,120.55Z" style="fill:#87d029"/><path d="M82.43,84.38a1,1,0,0,1-.71-.29l-5-5a1,1,0,0,1,0-1.42l5-5a1,1,0,0,1,1.42,0l5,5a1,1,0,0,1,0,1.42l-5,5A1,1,0,0,1,82.43,84.38Zm-3.62-6L82.43,82l3.62-3.62-3.62-3.62Z" style="fill:#87d029"/><path d="M89.55,91a1,1,0,0,1-.71-.3l-5-5a1,1,0,0,1,0-1.41l5-5a1,1,0,0,1,1.41,0l5,5a1,1,0,0,1,0,1.41l-5,5A1,1,0,0,1,89.55,91Zm-3.62-6,3.62,3.62,3.61-3.62-3.61-3.61Z" style="fill:#87d029"/><path d="M82.43,97.57a1,1,0,0,1-.71-.29l-5-5a1,1,0,0,1,0-1.42l5-5a1,1,0,0,1,1.42,0l5,5a1,1,0,0,1,0,1.42l-5,5A1,1,0,0,1,82.43,97.57Zm-3.62-6,3.62,3.62,3.62-3.62-3.62-3.62Z" style="fill:#87d029"/><path d="M75.31,91a1,1,0,0,1-.7-.3l-5-5a1,1,0,0,1,0-1.41l5-5a1,1,0,0,1,1.41,0l5,5a1,1,0,0,1,0,1.41l-5,5A1,1,0,0,1,75.31,91Zm-3.62-6,3.62,3.62,3.62-3.62-3.62-3.61Z" style="fill:#87d029"/></svg>	1	2	1
*/

