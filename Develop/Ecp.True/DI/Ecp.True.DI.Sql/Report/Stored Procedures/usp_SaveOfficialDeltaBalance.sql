/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Aug-24-2020
-- Update date: 	Aug-25-2020  Changed the logic to calculate partner & owner calculation
-- Update date: 	Sep-25-2020  Deleting the records which are in given start and end date to calculate the 
                                 partner owner mapping details always with new data as per BUG 81891
-- Update date: 	Sep-30-2020  When Owner is null then show "OTROS" by default as OWNER.
                                 Removed commented code
-- Update date: 	Oct-01-2020  Using DISTINCT to remove the duplicated records
-- Update date: 	Oct-09-2020  Passing ElementId as inputs to SP
-- Update date: 	Oct-15-2020  While deleting the records along with start and end date now considering segmentid as well.
-- Description:     This Procedure is to populate official consolidated delta balance Data into [Report].[OfficialDeltaBalance].
-- EXEC [Admin].[usp_SaveOfficialDeltaBalance] 1528,'2020-07-01','2020-07-31'                     

   SELECT * FROM [Report].[OfficialDeltaBalance] 
-- ==============================================================================================================================*/
CREATE PROCEDURE [Report].[usp_SaveOfficialDeltaBalance]
(
 @SegmentId INT
,@StartDate DATE
,@EndDate   DATE                      
)
AS
BEGIN
  SET NOCOUNT ON                   

		 IF OBJECT_ID('tempdb..#DeltaBalance')IS NOT NULL
		 DROP TABLE #DeltaBalance

		 IF OBJECT_ID('tempdb..#PartnerMapping')IS NOT NULL
		 DROP TABLE #PartnerMapping
		 
		 IF OBJECT_ID('tempdb..#TempFinal')IS NOT NULL
		 DROP TABLE #TempFinal

	 -- Variables Declaration
		DECLARE @Todaysdate	   DATETIME =  [Admin].[udf_GetTrueDate] ()
				

         SELECT DB.*,CEUnits.[Name] AS MeasurementUnitName 
		   INTO #DeltaBalance 
		   FROM [Report].[DeltaBalance] DB
		   INNER JOIN [Admin].[CategoryElement] CEUnits
		   ON CEUnits.ElementId = DB.MeasurementUnit
		   AND CEUnits.[CategoryId] = 6 --'Unidad de Medida'
		  WHERE SegmentId = @SegmentId
		    AND StartDate = @StartDate 
		    AND EndDate   = @EndDate   


		 -- Getting Partner and Owner Mapping Names

         SELECT CASE WHEN POM.GrandOwnerId IS NULL 
		             THEN 124
		             ELSE POM.GrandOwnerId
				END AS GrandOwnerId
		       ,CASE WHEN Own.[Name] IS NULL 
			         THEN 'OTROS' 
				     ELSE Own.[Name] 
				END AS GrandOwner
			   ,DB.ElementOwnerId AS PartnerOwnerId
			   ,Par.[Name] AS [Partner]
		  INTO #PartnerMapping
		  FROM #DeltaBalance DB
		  LEFT JOIN [Admin].PartnerOwnerMapping POM
		  ON DB.ElementOwnerId = POM.GrandOwnerId
		  OR DB.ElementOwnerId = POM.PartnerOwnerId
		  LEFT JOIN [Admin].CategoryElement Own
		  ON Own.ElementId = POM.GrandOwnerId
		  INNER JOIN [Admin].CategoryElement Par
		  ON Par.ElementId = DB.ElementOwnerId


         DELETE  
		   FROM [Report].[OfficialDeltaBalance]  
		  WHERE StartDate = @StartDate 
		    AND EndDate   = @EndDate
			AND SegmentId = @SegmentId

         SELECT * INTO #TempFinal 
		  FROM (
                 SELECT DISTINCT
				        CDF.SegmentId
				       ,Par.PartnerOwnerId AS ElementOwnerId
					   ,CDF.NodeId
					   ,CDF.ProductId
					   ,CDF.StartDate
					   ,CDF.EndDate
					   ,Prd.[Name]       AS Product
				       ,Par.[GrandOwner] AS [Owner]
					   ,[Partner]        AS [Partner] 
				      ,MeasurementUnitName
					  ,InitialInventory
					  ,FinalInventory
					  ,Input
					  ,[Output]
                      ,DeltaInitialInventory
					  ,DeltaFinalInventory
					  ,DeltaInput
					  ,DeltaOutput
					  ,CAST((ISNULL(InitialInventory,'0.0')+ISNULL(DeltaInitialInventory,'0.0')+ISNULL(Input,'0.0')
				           +ISNULL(DeltaInput,'0.0')-ISNULL([Output],'0.0')-ISNULL(DeltaOutput,'0.0')-ISNULL(FinalInventory,'0.0')
				           -ISNULL(DeltaFinalInventory,'0.0')
				           ) AS DECIMAL(21,2)
						   )  
					   AS [Control]
				 FROM #DeltaBalance CDF
				 LEFT JOIN #PartnerMapping Par
				 ON Par.[PartnerOwnerId] = CDF.[ElementOwnerId]
				 INNER JOIN [Admin].[Product] Prd
				 ON CDF.ProductId = Prd.ProductId
				)SQ

               MERGE INTO [Report].[OfficialDeltaBalance] Dest
			   USING #TempFinal Src
			   ON  ISNULL(Dest.Product,'')                 = ISNULL(Src.Product,'')
			   AND ISNULL(Dest.[Owner],'')                 = ISNULL(Src.[Owner],'')           
			   AND ISNULL(Dest.[Partner],'')			   = ISNULL(Src.[Partner],'')			
			   AND ISNULL(Dest.[MeasurementUnit],'')	   = ISNULL(Src.[MeasurementUnitName],'')
			   AND ISNULL(Dest.[ElementOwnerId],0)	       = ISNULL(Src.[ElementOwnerId],0)
			   AND ISNULL(Dest.[NodeId],0)	               = ISNULL(Src.[NodeId],0)
			   AND ISNULL(Dest.[ProductId],0)	           = ISNULL(Src.[ProductId],0)
			   AND ISNULL(Dest.[StartDate],'9999-12-31')   = ISNULL(Src.[StartDate],'9999-12-31')
			   AND ISNULL(Dest.[EndDate],'9999-12-31')     = ISNULL(Src.[EndDate],'9999-12-31')
			   AND ISNULL(Dest.SegmentId,0)				   = ISNULL(Src.SegmentId,0)	
			   WHEN MATCHED 
			    AND (
					
				        ISNULL(Dest.InitialInventory,0.0)       <> ISNULL(Src.InitialInventory,0.0)
					 OR ISNULL(Dest.FinalInventory,0.0)		    <> ISNULL(Src.FinalInventory,0.0)
					 OR ISNULL(Dest.Input,0.0)			        <> ISNULL(Src.Input,0.0)
					 OR ISNULL(Dest.[Output],0.0)			    <> ISNULL(Src.[Output],0.0)
					 OR ISNULL(Dest.DeltaInitialInventory,0.0)  <> ISNULL(Src.DeltaInitialInventory,0.0)
					 OR ISNULL(Dest.DeltaFinalInventory,0.0)	<> ISNULL(Src.DeltaFinalInventory,0.0)
					 OR ISNULL(Dest.DeltaInput,0.0)	            <> ISNULL(Src.DeltaInput,0.0)
					 OR ISNULL(Dest.DeltaOutput,0.0)	        <> ISNULL(Src.DeltaOutput,0.0)
					 OR ISNULL(Dest.[Control],0.0)			    <> ISNULL(Src.[Control],0.0)
				    )
               THEN
			   UPDATE
			      SET Dest.[InitialInventory]       = Src.[InitialInventory]
				     ,Dest.[FinalInventory]         = Src.[FinalInventory]
					 ,Dest.[Input]                  = Src.[Input]
					 ,Dest.[Output]                 = Src.[Output]
					 ,Dest.[DeltaInput]             = Src.[DeltaInput]
					 ,Dest.[DeltaOutput]            = Src.[DeltaOutput]
					 ,Dest.[DeltaInitialInventory]  = Src.[DeltaInitialInventory]
					 ,Dest.[DeltaFinalInventory]    = Src.[DeltaFinalInventory]
					 ,Dest.[LastModifiedBy]         = 'ReportUser'
					 ,Dest.[LastModifiedDate]       = @TodaysDate

               WHEN NOT MATCHED THEN
			   INSERT (
						[SegmentId]
			           ,[Product]     
			           ,[Owner]      
			           ,[Partner]	
			           ,[MeasurementUnit]
			           ,[ElementOwnerId]
			           ,[NodeId] 
			           ,[ProductId]	
			           ,[StartDate]
			           ,[EndDate]
                       ,[InitialInventory] 
					   ,[FinalInventory]	
					   ,[Input]	   
					   ,[Output]
					   ,[DeltaInitialInventory]
					   ,[DeltaFinalInventory]
					   ,[DeltaInput]
					   ,[DeltaOutput]   
					   ,[Control]	
					   ,[CreatedBy]
					   ,[CreatedDate]
					   ,[LastModifiedBy]
					   ,[LastModifiedDate]
			          )
               VALUES (
						Src.[SegmentId]
			           ,Src.[Product]     
			           ,Src.[Owner]      
			           ,Src.[Partner]	
			           ,Src.[MeasurementUnitName]
			           ,Src.[ElementOwnerId]
			           ,Src.[NodeId] 
			           ,Src.[ProductId]	
			           ,Src.[StartDate]
			           ,Src.[EndDate]
                       ,Src.[InitialInventory] 
					   ,Src.[FinalInventory]	
					   ,Src.[Input]	   
					   ,Src.[Output]
					   ,Src.[DeltaInitialInventory]
					   ,Src.[DeltaFinalInventory]
					   ,Src.[DeltaInput]
					   ,Src.[DeltaOutput]   
					   ,Src.[Control]
					   ,'ReportUser'
					   ,@TodaysDate
					   ,'ReportUser'
					   ,@TodaysDate
			          );
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to populate official consolidated delta balance Data into [Report].[OfficialDeltaBalance].',
	@level0type = N'SCHEMA',
    @level0name = N'Report',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveOfficialDeltaBalance',
    @level2type = NULL,
    @level2name = NULL

