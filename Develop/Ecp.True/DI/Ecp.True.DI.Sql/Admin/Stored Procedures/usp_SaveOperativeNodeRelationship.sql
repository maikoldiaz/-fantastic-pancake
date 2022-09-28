/*-- =================================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Dec-17-2019  
-- Updated Date:	Mar-20-2020
-- <Description>:	This Procedure is used to save the Operative Node Relationship Information for a given Ticket Id.  </Description>
-- EXEC [Admin].[usp_SaveOperativeNodeRelationship] 25285
-- ==================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveOperativeNodeRelationship] 
(
       @TicketId          INT
)
AS 
  BEGIN 
    SET NOCOUNT ON   		
         BEGIN TRY
         DECLARE @lnvErrorMessage NVARCHAR(MAX)

               BEGIN TRANSACTION
                          
				IF OBJECT_ID('tempdb..#TempOperNodeRelData')IS NOT NULL
				DROP TABLE #TempOperNodeRelData

				IF OBJECT_ID('tempdb..#TempOperMovPerData')IS NOT NULL
				DROP TABLE #TempOperMovPerData

				SELECT  OperNodeRel.[DestinationNode] 
					   ,OperNodeRel.[DestinationNodeType] 
					   ,OperNodeRel.[MovementType] 
					   ,OperNodeRel.[SourceNode] 
					   ,OperNodeRel.[SourceNodeType] 
					   ,OperNodeRel.[SourceProduct] 
					   ,OperNodeRel.[SourceProductType]
					   ,OperNodeRel.TransferPoint          
					   ,OperNodeRel.SourceField            
					   ,OperNodeRel.FieldWaterProduction   
					   ,OperNodeRel.RelatedSourceField    
				INTO #TempOperNodeRelData
				FROM [Analytics].[OperativeNodeRelationship] OperNodeRel 
				 WHERE
					  (   OperNodeRel.TransferPoint           <> 'NA'
					   OR OperNodeRel.SourceField             <> 'NA'
					   OR OperNodeRel.FieldWaterProduction    <> 'NA'
					   OR OperNodeRel.RelatedSourceField      <> 'NA'
					   )

				SELECT       OperMovPer.DestinationNode, 
							 OperMovPer.DestinationNodeType, 
							 OperMovPer.MovementType, 
							 OperMovPer.SourceNode, 
							 OperMovPer.SourceNodeType, 
							 OperMovPer.SourceProduct, 
							 OperMovPer.SourceProductType, 
							 OperMovPer.NetStandardVolume,
							 OperMovPer.[OperationalDate] AS [OperationalDate]
				INTO #TempOperMovPerData
				FROM [Admin].[view_OperativeMovementsPeriodic] OperMovPer
				WHERE TicketId = @TicketId

                         MERGE [Analytics].[OperativeMovements] AS Trgt
                         USING (      

								SELECT       OperMovPer.[OperationalDate]  AS [OperationalDate],
											 OperNodeRel.TransferPoint, 
											 OperNodeRel.SourceField, 
											 OperNodeRel.FieldWaterProduction, 
											 OperNodeRel.RelatedSourceField, 
											 OperMovPer.DestinationNode, 
											 OperMovPer.DestinationNodeType, 
											 OperMovPer.MovementType, 
											 OperMovPer.SourceNode, 
											 OperMovPer.SourceNodeType, 
											 OperMovPer.SourceProduct, 
											 OperMovPer.SourceProductType, 
											 OperMovPer.NetStandardVolume, 
											 'TRUE' AS SourceSystem,
											 NEWID() AS ExecutionID
								 FROM   #TempOperMovPerData OperMovPer 
								 INNER JOIN #TempOperNodeRelData OperNodeRel 
								 ON  OperMovPer.[DestinationNode]          = OperNodeRel.[DestinationNode] 
								 AND OperMovPer.[DestinationNodeType]     = OperNodeRel.[DestinationNodeType] 
								 AND OperMovPer.[MovementType]            = OperNodeRel.[MovementType] 
								 AND OperMovPer.[SourceNode]              = OperNodeRel.[SourceNode] 
								 AND OperMovPer.[SourceNodeType]          = OperNodeRel.[SourceNodeType] 
								 AND OperMovPer.[SourceProduct]           = OperNodeRel.[SourceProduct] 
								 AND OperMovPer.[SourceProductType]       = OperNodeRel.[SourceProductType] 
								 WHERE
									  (   OperNodeRel.TransferPoint           <> 'NA'
									   OR OperNodeRel.SourceField             <> 'NA'
									   OR OperNodeRel.FieldWaterProduction    <> 'NA'
									   OR OperNodeRel.RelatedSourceField      <> 'NA'
									  )

                                  )AS Src
                         ON  Src.[DestinationNode]              = Trgt.[DestinationNode] 
                         AND Src.[DestinationNodeType]          = Trgt.[DestinationNodeType] 
                         AND Src.[MovementType]                 = Trgt.[MovementType] 
                         AND Src.[SourceNode]                   = Trgt.[SourceNode] 
                         AND Src.[SourceNodeType]               = Trgt.[SourceNodeType] 
                         AND Src.[SourceProduct]                = Trgt.[SourceProduct] 
                         AND Src.[SourceProductType]            = Trgt.[SourceProductType]
		                AND  Src.[OperationalDate]              = CAST(Trgt.[OperationalDate] AS DATE)
                         WHEN MATCHED 
	                     AND 
		                    (
			                    Src.TransferPoint			<>	Trgt.TransferPoint
		                    OR  Src.SourceField				<>	Trgt.SourceField			
		                    OR	Src.FieldWaterProduction	<>	Trgt.FieldWaterProduction
		                    OR	Src.RelatedSourceField		<>	Trgt.RelatedSourceField
		                    )
                         THEN
                         UPDATE        SET
                                   Trgt.TransferPoint           =       Src.TransferPoint          
                                  ,Trgt.SourceField             =       Src.SourceField                  
                                  ,Trgt.FieldWaterProduction    =       Src.FieldWaterProduction
                                  ,Trgt.RelatedSourceField      =       Src.RelatedSourceField     
                                  ,Trgt.ExecutionID             =       NEWID()
                                  ,Trgt.LastModifiedBy          =       'SYSTEM'
                                  ,Trgt.LastModifiedDate        =       Admin.udf_GetTrueDate()
                         WHEN NOT MATCHED       
                         THEN
                         INSERT(OperationalDate, 
                                TransferPoint, 
                                SourceField, 
                                FieldWaterProduction, 
                                RelatedSourceField, 
                                DestinationNode, 
                                DestinationNodeType, 
                                MovementType, 
                                SourceNode, 
                                SourceNodeType, 
                                SourceProduct, 
                                SourceProductType, 
                                NetStandardVolume, 
                                SourceSystem,
                                CreatedBy,
                                ExecutionID
                              ) 
                         VALUES          
                         (
                                Src.[OperationalDate],
                                Src.TransferPoint, 
                                Src.SourceField, 
                                Src.FieldWaterProduction, 
                                Src.RelatedSourceField, 
                                Src.DestinationNode, 
                                Src.DestinationNodeType, 
                                Src.MovementType, 
                                Src.SourceNode, 
                                Src.SourceNodeType, 
                                Src.SourceProduct, 
                                Src.SourceProductType,  
                                Src.NetStandardVolume, 
                                Src.SourceSystem,
                                'SYSTEM',
                                NEWID()
                         );

                     UPDATE [Admin].[Ticket]
                     SET [AnalyticsStatus] = 1 -- SUCCESS
                        ,[AnalyticsErrorMessage] = NULL
                     WHERE TicketId = @TicketId

					IF OBJECT_ID('tempdb..#TempOperNodeRelData')IS NOT NULL
					DROP TABLE #TempOperNodeRelData

					IF OBJECT_ID('tempdb..#TempOperMovPerData')IS NOT NULL
					DROP TABLE #TempOperMovPerData

                    COMMIT TRANSACTION
      END TRY


	 BEGIN CATCH    
		SELECT @lnvErrorMessage = ERROR_MESSAGE() 
		ROLLBACK TRANSACTION

         UPDATE [Admin].[Ticket]
         SET [AnalyticsStatus]       = 0 -- FAILURE
            ,[AnalyticsErrorMessage] = @lnvErrorMessage
         WHERE TicketId = @TicketId

		RAISERROR (@lnvErrorMessage,16,1)  
	 END CATCH   
	  
  END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to save the Operative Node Relationship Information for a given Ticket Id.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveOperativeNodeRelationship',
    @level2type = NULL,
    @level2name = NULL