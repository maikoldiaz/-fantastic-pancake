/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Apr-30-2020
-- Update date: 	Sept-03-2021 Change Inner for Left join in Node
-- Description:     This Procedure is to Get Event Details based on Element, NodeName, StartDate and EndDate.
-- EXEC [Admin].[usp_GetContractsInformation] 'Automation_l29o4','','2020-01-01','2020-01-08','68596201-689E-460F-9F2D-189BE62DA446'
   SELECT * FROM [Admin].[ContractsInformation]
============================================================================================================================ --*/
CREATE PROCEDURE [Admin].[usp_GetContractsInformation]
(
	     @ElementName	NVARCHAR(250) 
		,@NodeName	    NVARCHAR(250) 
		,@StartDate		DATE		  
		,@EndDate		DATE		  
		,@ExecutionId	NVARCHAR(250) 
)
AS
BEGIN
	SET NOCOUNT ON
	  
	-- Deleting records from [Admin].[EventsInformation] table which are older than 24 hours based on colombian timestamp
	DELETE FROM [Admin].[ContractsInformation]
	WHERE CreatedDate < (
	                     SELECT [Admin].[udf_GetTrueDate] ()-1
						 )
	   OR (    InputElement = @ElementName
	       AND ExecutionId = @ExecutionId
          )

			IF OBJECT_ID('tempdb..#TempContracts')IS NOT NULL
			DROP TABLE #TempContracts

	--Variables Declaration
	DECLARE  @ElementId			INT = 0
			,@NodeId			INT 
			,@NoOfDays			INT

	SELECT @ElementId = Ce.ElementId
	FROM Admin.CategoryElement CE
	WHERE CE.Name = @ElementName
	

	SELECT @NodeId = ND.NodeID
	FROM Admin.Node ND
	WHERE ND.Name = @NodeName
			

     SELECT * INTO #TempContracts
	   FROM (
	    		SELECT DISTINCT
				       Ctrt.[DocumentNumber]												
		              ,Ctrt.[Position]													
		              ,Ctrt.[StartDate]													
		              ,Ctrt.[EndDate]														
		              ,Ctrt.[Owner1Id]													
		              ,Ctrt.[Owner2Id]
					  ,Ctrt.[SourceNodeId]
					  ,Ctrt.[DestinationNodeId]
					  ,Ctrt.[ProductId]
					  ,Ctrt.[Volume]
					  ,Ctrt.[MeasurementUnit]
					  ,Ctrt.[MovementTypeId]
					  ,NT.ElementId
					  ,Ctrt.[SourceSystem]
					  ,Ctrt.[PurchaseOrderType]
					  ,Ctrt.[Status]
					  ,Ctrt.[PositionStatus]
					  ,Ctrt.[Frequency]					  
					  ,Ctrt.[Tolerance]
					  ,Ctrt.[ExpeditionClass]
					  ,Ctrt.[EstimatedVolume]
		 		  FROM [Admin].[Contract] Ctrt
				  JOIN [Admin].NodeTag NT
				    ON (   Ctrt.SourceNodeId      = NT.NodeId
					    OR Ctrt.DestinationNodeId = NT.NodeId
					   )
	    		WHERE NT.ElementId = @ElementId
				  AND (   SourceNodeId       = @NodeId OR @NodeId IS NULL 
	    			   OR DestinationNodeId  = @NodeId OR @NodeId IS NULL 
   	            		)
	    		  AND (   CAST(Ctrt.StartDate AS DATE) BETWEEN @StartDate AND @EndDate
	               	   OR CAST(Ctrt.EndDate AS DATE) BETWEEN @StartDate AND @EndDate
	               		)
                  AND Ctrt.isdeleted=0 -- Record should not be deleted 
	    	 )A



      INSERT INTO [Admin].[ContractsInformation] ( [RNo]					
										          ,[DocumentNumber]		
										          ,[Position]			
	                                              ,[TypeOfMovement]	
										          ,[SourceNode]			
										          ,[DestinationNode]		
										          ,[Product]				
										          ,[StartDate]           
										          ,[EndDate]             
										          ,[Owner1Name]			
										          ,[Owner2Name]			
										          ,[Volume]				
										          ,[MeasurementUnit]		
										          ,[InputElement]
												  ,[SourceSystem]				
												  ,[PurchaseOrderType]			
												  ,[Status]					
												  ,[PositionStatus]				
												  ,[Frequency]							  
												  ,[Tolerance]					
												  ,[ExpeditionClass]
												  ,[EstimatedVolume]												  
										          ,[InputNodeName]       
										          ,[ExecutionId] 		
											      ,[CreatedBy]			
                                     	          )
     SELECT ROW_NUMBER() OVER (ORDER BY SourceNode,StartDate ASC) AS RNo,*,@NodeName,@ExecutionId,'ReportUser' 
	   FROM ( 
			SELECT DISTINCT  Contr.[DocumentNumber]		AS DocumentNumber
		                    ,Contr.[Position]			AS Position
		                    ,MovementType.[Name]		AS [TypeOfMovement]
		                    ,SourceNode.[Name]			AS [SourceNode]
		                    ,DestNode.[Name]			AS [DestinationNode]
		                    ,Product.[Name]				AS [Product]
		                    ,Contr.StartDate			AS StartDate
		                    ,Contr.EndDate				AS EndDate
		                    ,[Owner1].[Name]			AS Owner1Name
		                    ,[Owner2].[Name]			AS Owner2Name
		                    ,Contr.Volume				AS Volume
		                    ,Measurement.[Name]			AS MeasurementUnit
		                    ,Element.[Name]				AS Element
							,Contr.[SourceSystem]		AS SourceSystem
						    ,OrderType.[Name]			AS PurchaseOrderType
						    ,Contr.[Status]				AS [Status]
						    ,Contr.[PositionStatus]		AS PositionStatus
						    ,Contr.[Frequency]			AS Frequency		  
						    ,Contr.[Tolerance]			AS Tolerance
						    ,ExpeditionClass.[Name]		AS ExpeditionClass
							,isnull(Contr.[EstimatedVolume],Contr.Volume)	AS EstimatedVolume
		      FROM #TempContracts Contr
			  JOIN [Admin].CategoryElement Element
			  	ON Contr.ElementId = Element.ElementId 
			   AND Element.CategoryId=2 -- Condition for segment
			  JOIN [Admin].CategoryElement MovementType
			  	ON Contr.MovementTypeId = MovementType.ElementId 
			   AND MovementType.CategoryId= 9 -- Condition for Tipo Movimiento
     		  LEFT JOIN [Admin].[Node] SourceNode 
			  	ON Contr.SourceNodeId=SourceNode.NodeId
			  LEFT JOIN [Admin].[Node] DestNode
			  	ON Contr.DestinationNodeId=DestNode.NodeId
			  JOIN [Admin].Product Product
			  	ON Contr.ProductId=Product.ProductId
			  JOIN [Admin].CategoryElement [Owner1]
			  	ON Contr.Owner1Id = [Owner1].ElementId 
			   AND [Owner1].CategoryId= 7 -- Condition for Customer 
			  JOIN [Admin].CategoryElement [Owner2]
			  	ON Contr.Owner2Id = [Owner2].ElementId 
			   AND [Owner2].CategoryId= 7 -- Second Owner
			  JOIN [Admin].CategoryElement Measurement
			  	ON Contr.MeasurementUnit = Measurement.ElementId 
			   AND Measurement.CategoryId= 6
			  lEFT JOIN [Admin].CategoryElement ExpeditionClass
			  	ON Contr.ExpeditionClass = ExpeditionClass.ElementId 
				AND ExpeditionClass.CategoryId= 1
			  lEFT JOIN [Admin].CategoryElement OrderType
			  	ON Contr.[PurchaseOrderType] = OrderType.ElementId
			   AND OrderType.CategoryId= 23
       )Final
 
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get Event Details based on Element, NodeName, StartDate and EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetContractsInformation',
    @level2type = NULL,
    @level2name = NULL