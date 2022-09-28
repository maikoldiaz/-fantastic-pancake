/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Apr-30-2020
-- Description:     This Procedure is to Get Event Details based on ElementName, NodeName, StartDate and EndDate.
-- EXEC [Admin].[usp_GetEventsInformation] 'Automation_4mo17','','2020-12-10','2020-12-15','BAD42B65-D36C-4760-BD57-E8F978BBE47E'
   SELECT * FROM [Admin].[EventsInformation]
============================================================================================================================ --*/
CREATE PROCEDURE [Admin].[usp_GetEventsInformation]
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
	DELETE FROM [Admin].[EventsInformation]
	WHERE CreatedDate < (
	                     SELECT [Admin].[udf_GetTrueDate] ()-1
						 )
	   OR (    InputElement = @ElementName
	       AND ExecutionId = @ExecutionId
          )

			IF OBJECT_ID('tempdb..#TempEvents')IS NOT NULL
			DROP TABLE #TempEvents

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
			

     SELECT * INTO #TempEvents 
	   FROM (
	    		SELECT DISTINCT
				       Eve.EventId,
				       Eve.EventTypeId,
					   Eve.SourceNodeId,
					   Eve.DestinationNodeId,
					   Eve.SourceProductId,
					   Eve.DestinationProductId,
					   Eve.Owner1Id,
					   Eve.Owner2Id,
					   Eve.MeasurementUnit,
				       Eve.StartDate,
				       Eve.EndDate,
					   Eve.Volume,
					   NT.ElementId
				  FROM [Admin].[Event] Eve
				  JOIN [Admin].NodeTag NT
				    ON (   Eve.SourceNodeId      = NT.NodeId
					    OR Eve.DestinationNodeId = NT.NodeId
					   )
	    		WHERE NT.ElementId = @ElementId
				  AND (   SourceNodeId       = @NodeId OR @NodeId IS NULL 
	    				OR DestinationNodeId = @NodeId OR @NodeId IS NULL 
   	            		)
	    		  AND (   CAST(Eve.StartDate AS DATE) BETWEEN @StartDate AND @EndDate
	               	   OR CAST(Eve.EndDate AS DATE) BETWEEN @StartDate AND @EndDate
	               		)
				  AND [Eve].isdeleted=0		
	    	)A



      INSERT INTO [Admin].[EventsInformation] ( [RNo]				
										       ,[PropertyEvent]		
										       ,[SourceNode]		
										       ,[DestinationNode]	
										       ,[SourceProduct]		
										       ,[DestinationProduct]
										       ,[StartDate]         	 
										       ,[EndDate]           	 
										       ,[Owner1Name]		
										       ,[Owner2Name]		
										       ,[Volume]			
										       ,[MeasurementUnit]	
										       ,[InputElement]		
										       ,[InputNodeName]     
										       ,[ExecutionId]
											   ,[CreatedBy]
	                                           )
     SELECT ROW_NUMBER() OVER (ORDER BY SourceNode,StartDate ASC) AS RNo,*,@NodeName,@ExecutionId,'ReportUser' 
	   FROM ( 
			SELECT DISTINCT PropertyEvent.[Name]   AS PropertyEvent
				           ,SourceNode.[Name]      AS SourceNode
				           ,DestNode.[Name]        AS DestinationNode
				           ,SourceProduct.[Name]   AS SourceProduct
				           ,DestProduct.[Name]     AS DestinaionProduct
				           ,Even.StartDate         AS StartDate
				           ,Even.EndDate           AS EndDate
				           ,[Owner1].[Name]    	   AS Owner1Name
				           ,[Owner2].[Name]    	   AS Owner2Name
				           ,Even.Volume            AS Volume
				           ,Measurement.[Name]	   AS MeasurementUnit
						   ,Element.[Name]         AS Element 
		      FROM #TempEvents Even
			  JOIN [Admin].CategoryElement Element
			  	ON Even.ElementId = Element.ElementId 
			   AND Element.CategoryId=2 -- Condition for segment
			  JOIN [Admin].CategoryElement PropertyEvent
			  	ON [Even].EventTypeId = PropertyEvent.ElementId 
			   AND PropertyEvent.CategoryId= 12 -- Condition for Tipo Event
     		  JOIN [Admin].[Node] SourceNode 
			  	ON [Even].SourceNodeId=SourceNode.NodeId
			  JOIN [Admin].[Node] DestNode
			  	ON [Even].DestinationNodeId=DestNode.NodeId
			  JOIN [Admin].Product SourceProduct
			  	ON [Even].SourceProductId=SourceProduct.ProductId
			  JOIN [Admin].Product DestProduct
			  	ON [Even].DestinationProductId=DestProduct.ProductId
			  JOIN [Admin].CategoryElement [Owner1]
			  	ON [Even].Owner1Id = [Owner1].ElementId 
			   AND [Owner1].CategoryId= 7 -- Condition for Customer 
			  JOIN [Admin].CategoryElement [Owner2]
			  	ON [Even].Owner2Id = [Owner2].ElementId 
			   AND [Owner2].CategoryId= 7 -- Second Owner
			  JOIN [Admin].CategoryElement Measurement
			  	ON [Even].MeasurementUnit = Measurement.ElementId 
			   AND Measurement.CategoryId= 6
       )Final
 
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to Get Event Details based on ElementName, NodeName, StartDate and EndDate.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetEventsInformation',
    @level2type = NULL,
    @level2name = NULL