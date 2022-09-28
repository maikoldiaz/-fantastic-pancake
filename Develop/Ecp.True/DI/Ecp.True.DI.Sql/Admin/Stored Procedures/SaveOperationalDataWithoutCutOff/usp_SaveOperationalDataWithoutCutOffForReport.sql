/*-- ======================================================================================================================================================================================
-- Author:          Microsoft
-- Created Date: Dec-12-2019
-- Updated Date:  Mar-20-2020
-- Updated Date:  Jun-18-2020 Added usp_SaveOperationalMovementOwnerWithoutCutOffForSystem,usp_SaveInventoryOwnerDetailsWithoutCutOffForSystem,usp_SaveOperationalMovementOwnerWithoutCutOffForSegment,usp_SaveInventoryOwnerDetailsWithoutCutOffForSystem
-- Updated Date:  Jul-02-2020 Updated system SPs logic for parallel execution
-- Updated date:   Aug-04-2020  -- Changed input paramters from STRING to INTEGER AND removed delete old data
-- Updated date:    Aug-07-2020  -- Removing unrequired columns and conditions
-- Updated date:    Aug-12-2020  -- Removed extra joins
-- Updated date: Jun-06-2021 -- Se excluyen los tipos de movimientos (evacuaciones de entrada, evacuaciones de salida, anulaciones de entrada y anulaciones de salida)
-- <Description>: This Procedure is used to call the Segment and System SP's for Operational Data to be displayed in the Report for Before Cutoff Scenario (Time 1). </Description>
--EXEC [Admin].[usp_SaveOperationalDataWithoutCutOffForReport] 'Segmento','Automation_xdttv','ALL','2019-12-13','2020-01-27','738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37'
--EXEC [Admin].[usp_SaveOperationalDataWithoutCutOffForReport] 'Sistema','SystemForData','ALL','2019-12-13','2020-01-27','738BCF0F-5F50-4EB8-A6C9-C27C99AA5A37'
-- ======================================================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveOperationalDataWithoutCutOffForReport]
(
                    @CategoryId                 INT 
                   ,@ElementId                  INT 
                   ,@NodeId                     INT 
                   ,@StartDate                  DATE                      
                   ,@EndDate                    DATE                      
                   ,@ExecutionId                INT
)
AS 
BEGIN
  SET NOCOUNT ON

    --Variables Declaration
    DECLARE   @Category                   NVARCHAR(250) = (SELECT [Name] FROM [Admin].[Category] WHERE CategoryId = @CategoryId)
             ,@NoOfDays                   INT
             ,@PreviousDayOfStartDate     DATE
             ,@Previousdate               DATE =  [Admin].[udf_GetTrueDate] ()-1
             ,@Todaysdate                 DATE =  [Admin].[udf_GetTrueDate] ()
                             
/* SYSTEM RELATED CALCULATION --> START*/							  
   IF @Category = 'Sistema'
      BEGIN
                      IF OBJECT_ID('tempdb..#TempNodeTagSystem')IS NOT NULL
                      DROP TABLE #TempNodeTagSystem
                      
                     
                      --Common Code Temp table Used In Procedures(usp_SaveOperationalMovementWithoutCutOffForSystem,usp_SaveOperationalDataWithoutCutOffForSystem) Start

                      INSERT INTO [Admin].[MovementInformationMovforSystemReport] 
                      ( 
                            OperationalDate              
                           ,SourceProductId              
                           ,SourceProductName            
                           ,SegmentID                    
                           ,SourceNodeId                 
                           ,SourceNodeName               
                           ,SourceNodeNameIsGeneric       
                           ,DestinationProductId         
                           ,DestinationProductName       
                           ,DestinationNodeId            
                           ,DestinationNodeName          
                           ,DestinationNodeNameIsGeneric  
                           ,MessageTypeId    
						   ,[Classification]
                           ,MovementID                    
                           ,MovementTypeName              
                           ,MeasurementUnit               
                           ,MovementTransactionId         
                           ,EventType                     
                           ,SystemName                   
                           ,SourceSystem
						   ,BatchId
                           ,NetStandardVolume             
                           ,GrossStandardVolume           
                           ,UncertaintyPercentage         
                           ,ExecutionId                  
                           ,CreatedDate                     
                           ,CreatedBy                    
                        )
					 SELECT Mov.OperationalDate                    AS OperationalDate,
                            Mov.SourceProductId                    AS SourceProductId,
                            Mov.SourceProductName                  AS SourceProductName, 
                            Mov.SegmentId                          AS SegmentID,     
                            Mov.SourceNodeId                       AS SourceNodeId,
                            Mov.SourceNodeName                     AS SourceNodeName,
							CASE WHEN CHARINDEX('',Mov.SourceNodeName) > 0 
                                 THEN 1 
                                 ELSE 0 
                                 END                               AS SourceNodeNameIsGeneric,
                            Mov.DestinationProductId               AS DestinationProductId,
                            Mov.DestinationProductName             AS DestinationProductName,        
                            Mov.DestinationNodeId                  AS DestinationNodeId,
                            Mov.DestinationNodeName                AS DestinationNodeName,
							CASE WHEN CHARINDEX('',Mov.DestinationNodeName) > 0 
                                 THEN 1 
                                 ELSE 0 
                                 END                               AS DestinationNodeNameIsGeneric,
                            ISNULL(Mov.MessageTypeId,0)            AS MessageTypeId,
                            ISNULL(Mov.[Classification],'')        AS [Classification],
                            Mov.MovementID                         AS MovementID,
                            Mov.MovementTypeName                   AS MovementTypeName,
                            CEUnits.[Name]                         AS MeasurementUnit,
                            Mov.MovementTransactionId              AS MovementTransactionId,
                            Mov.EventType                          AS EventType,
                            Mov.SystemName                         AS SystemName,
                            Mov.SourceSystem                       AS SourceSystem,
                            Mov.Batchid                            AS Batchid, 
                            ISNULL(Mov.NetStandardVolume,0.00)     AS NetStandardVolume,
                            ISNULL(Mov.GrossStandardVolume,0.00)   AS GrossStandardVolume,
                            ISNULL(Mov.UncertaintyPercentage,0.00) AS UncertaintyPercentage
							,@ExecutionId                          AS ExecutionId
                            ,@Todaysdate
                            ,'System'
					FROM Admin.view_MovementInformation Mov
					INNER JOIN [Admin].[CategoryElement] CEUnits
					ON CEUnits.ElementId = Mov.MeasurementUnit
					AND CEUnits.[CategoryId] = 6 --'Unidad de Medida'
					WHERE Mov.ScenarioId = 1 
					  AND ISNULL(Mov.SourceSystem,'') != 'TRUE'
					  AND Mov.OperationalDate  BETWEEN @StartDate AND @EndDate
                      AND Mov.MovementTypeId not in (153,154,155,156) --- Task: 128313 - Se excluyen los tipos de movimientos (evacuaciones de entrada, evacuaciones de salida, anulaciones de entrada y anulaciones de salida)
                      --Common Code Temp table Used In Procedures(usp_SaveOperationalMovementWithoutCutOffForSystem,usp_SaveOperationalDataWithoutCutOffForSystem) End

                      --Common Code Used For Procedures(usp_SaveOperationalDataWithoutCutOffForSystem,usp_SaveInventoryDetailsWithoutCutOffForSystem) Start
          
                      SELECT @NoOfDays = DATEDIFF(DAY,@StartDate,@EndDate)
                      SET @PreviousDayOfStartDate = DATEADD(DAY,-1,@StartDate)
          
                      SELECT * INTO #TempNodeTagSystem
                      FROM (
                            SELECT DISTINCT NT.NodeId,
                                            NT.ElementId,
                                            CE.[Name] AS ElementName,
                                            CASE WHEN NT.StartDate < @StartDate 
                                                 THEN @PreviousDayOfStartDate
                                                 ELSE NT.StartDate END
                                                   AS StartDate,
                                            CASE WHEN (   CAST(NT.EndDate AS DATE) > (CAST(@Todaysdate AS DATE)) 
                                                       OR CAST(NT.EndDate AS DATE) > (DATEADD(DAY,@NoOfDays,@StartDate))
                                                       )
                                                 THEN @EndDate
                                                 ELSE NT.EndDate 
                                                 END AS EndDate 
                            FROM Admin.NodeTag NT
                            INNER JOIN Admin.CategoryElement CE
                            ON CE.ElementId = NT.ElementId
                            WHERE NT.ElementId = @ElementId
                            AND CE.CategoryId = 8 
                            AND (NT.NodeId = @NodeId OR @NodeId = 0)
                      ) A
                              
                      INSERT INTO [Admin].[NodeTagCalculationDateForSystemReport]
                      (
                           NodeId                               
                          ,ElementId                         
                          ,ElementName                  
                          ,CalculationDate               
                          ,ExecutionId
                          ,CreatedDate                     
                          ,CreatedBy                                                                     
                      )
                      SELECT  NT.NodeId                              AS NodeId 
                             ,NT.ElementId                           AS ElementId
                             ,NT.ElementName                         AS ElementName
                             ,Ca.CalculationDate                     AS CalculationDate
                             ,@ExecutionId                           AS ExecutionId
                             ,@Todaysdate
                             ,'System'
                      FROM #TempNodeTagSystem NT
                      CROSS APPLY (SELECT dates   AS  CalculationDate
                                   FROM [Admin].[udf_GetAllDates]( NT.StartDate, 
                                                                   NT.EndDate, 
                                                                   NT.NodeId, 
                                                                   NT.ElementId)
                                  ) CA
                      WHERE CA.CalculationDate BETWEEN @PreviousDayOfStartDate AND @EndDate
       --Common Code Used For Procedures(usp_SaveOperationalDataWithoutCutOffForSystem,usp_SaveInventoryDetailsWithoutCutOffForSystem) End
       END
/* SYSTEM RELATED CALCULATION --> END*/

/* SEGMENT RELATED CALCULATION --> START*/
       ELSE 
       IF @Category = 'Segmento'
       BEGIN

                      IF OBJECT_ID('tempdb..#TempNodeTagSegement')IS NOT NULL
                      DROP TABLE #TempNodeTagSegement
                      
                      --Common Code Temp table Used In Procedures(usp_SaveOperationalMovementWithoutCutOffForSegment,usp_SaveOperationalDataWithoutCutOffForSegment) Start
                      INSERT INTO [Admin].[MovementInformationMovforSegmentReport] 
                      ( 
                            OperationalDate              
                           ,SourceProductId              
                           ,SourceProductName            
                           ,SegmentID                    
                           ,SourceNodeId                 
                           ,SourceNodeName               
                           ,SourceNodeNameIsGeneric       
                           ,DestinationProductId         
                           ,DestinationProductName       
                           ,DestinationNodeId            
                           ,DestinationNodeName          
                           ,DestinationNodeNameIsGeneric  
                           ,MessageTypeId    
						   ,[Classification]
                           ,MovementID                    
                           ,MovementTypeName              
                           ,MeasurementUnit               
                           ,MovementTransactionId         
                           ,EventType                     
                           ,SystemName                   
                           ,SourceSystem
						   ,BatchId
                           ,NetStandardVolume             
                           ,GrossStandardVolume           
                           ,UncertaintyPercentage         
                           ,ExecutionId                  
                           ,CreatedDate                     
                           ,CreatedBy                    
                        )
                     SELECT  Mov.OperationalDate                                                     AS OperationalDate,
                             Mov.SourceProductId                                                     AS SourceProductId,
                             Mov.SourceProductName                                                   AS SourceProductName,               
                             Mov.SegmentId                                                           AS SegmentID,                  
                             Mov.SourceNodeId                                                        AS SourceNodeId,
                             Mov.SourceNodeName                                                      AS SourceNodeName,
                             CASE WHEN CHARINDEX('',Mov.SourceNodeName) > 0 
                                                        THEN 1 
                                                        ELSE 0 
                                                        END                                          AS SourceNodeNameIsGeneric,
                             Mov.DestinationProductId                                                AS DestinationProductId,
                             Mov.DestinationProductName                                              AS DestinationProductName,                      
                             Mov.DestinationNodeId                                                   AS DestinationNodeId,
                             Mov.DestinationNodeName                                                 AS DestinationNodeName,
                             CASE WHEN CHARINDEX('',Mov.DestinationNodeName) > 0 
                                                        THEN 1 
                                                        ELSE 0 
                                                        END                                          AS DestinationNodeNameIsGeneric,
                             ISNULL(Mov.MessageTypeId,0)											 AS MessageTypeId,
							 ISNULL(Mov.[Classification],'')										 AS [Classification],
                             Mov.MovementID                                                          AS MovementID,
                             Mov.MovementTypeName                                                    AS MovementTypeName,
                             CEUnits.[Name]                                                          AS MeasurementUnit,
                             Mov.MovementTransactionId                                               AS MovementTransactionId,
                             Mov.EventType                                                           AS EventType,
                             Mov.SystemName                                                          AS SystemName,
                             Mov.SourceSystem                                                        AS SourceSystem,
							 Mov.Batchid                                                             AS Batchid, 
                             ISNULL(Mov.NetStandardVolume,0.00)                                      AS NetStandardVolume,
                             ISNULL(Mov.GrossStandardVolume,0.00)                                    AS GrossStandardVolume,
                             ISNULL(Mov.UncertaintyPercentage,0.00)                                  AS UncertaintyPercentage,
                             @ExecutionId                                                            AS ExecutionId,
                             @Todaysdate                                                             AS CreatedDate,
                             'System'                                                                AS CreatedBy
                      FROM Admin.view_MovementInformation Mov
                      INNER JOIN [Admin].[CategoryElement] CEUnits
                      ON CEUnits.ElementId = Mov.MeasurementUnit
                      AND CEUnits.[CategoryId] = 6 --'Unidad de Medida'
                      WHERE Mov.SegmentId = @ElementId
                      AND Mov.OperationalDate  BETWEEN @StartDate AND @EndDate
                      AND ISNULL(Mov.SourceSystem,'') != 'TRUE'
					 AND Mov.ScenarioId = 1 
                     AND Mov.MovementTypeId not in (153,154,155,156) --- Task: 128313 - Se excluyen los tipos de movimientos (evacuaciones de entrada, evacuaciones de salida, anulaciones de entrada y anulaciones de salida)
          --Common Code Temp table Used In Procedures(usp_SaveOperationalMovementWithoutCutOffForSegment,usp_SaveOperationalDataWithoutCutOffForSegment) End

          --Common Code Used For Procedures(usp_SaveOperationalDataWithoutCutOffForSegment,usp_SaveInventoryDetailsWithoutCutOffForSegment) Start
          
            SELECT @NoOfDays = DATEDIFF(DAY,@StartDate,@EndDate)
            SET @PreviousDayOfStartDate = DATEADD(DAY,-1,@StartDate)

			
            SELECT * INTO #TempNodeTagSegement 
            FROM (
                  SELECT DISTINCT NT.NodeId,
                                  NT.ElementId,
                                  Ce.Name AS ElementName,
                                  CASE WHEN NT.StartDate < @StartDate 
                                       THEN @PreviousDayOfStartDate
                                       ELSE NT.StartDate END
                                         AS StartDate,
                                  CASE WHEN (   CAST(NT.EndDate AS DATE) > (CAST(@Todaysdate AS DATE)) 
                                             OR CAST(NT.EndDate AS DATE) > (DATEADD(DAY,@NoOfDays,@StartDate))
                                             )
                                       THEN @EndDate
                                       ELSE NT.EndDate 
                                       END AS EndDate 
                  FROM Admin.NodeTag NT
                  INNER JOIN [Admin].CategoryElement CE
                  ON CE.ElementId = NT.ElementId
                  WHERE NT.ElementId = @ElementId
                  AND CE.CategoryId = 2 
                  AND (NT.NodeId = @NodeId OR @NodeId =  0)
            ) A
                    
            INSERT INTO [Admin].[NodeTagCalculationDateForSegmentReport]
            (
                 NodeId                               
                ,ElementId                         
                ,ElementName                  
                ,CalculationDate               
                ,ExecutionId
                ,CreatedDate                     
                ,CreatedBy                                                                     
            )
            SELECT  NT.NodeId                              AS NodeId 
                   ,NT.ElementId                           AS ElementId
                   ,NT.ElementName                         AS ElementName
                   ,Ca.CalculationDate                     AS CalculationDate
                   ,@ExecutionId                           AS ExecutionId
                   ,@Todaysdate
                   ,'System'
            FROM #TempNodeTagSegement NT
            CROSS APPLY (SELECT dates   AS  CalculationDate
                         FROM [Admin].[udf_GetAllDates]( NT.StartDate, 
                                                         NT.EndDate, 
                                                         NT.NodeId, 
                                                         NT.ElementId)
                        ) CA
            WHERE CA.CalculationDate BETWEEN @PreviousDayOfStartDate AND @EndDate
 --Common Code Used For Procedures(usp_SaveOperationalDataWithoutCutOffForSegment,usp_SaveInventoryDetailsWithoutCutOffForSegment) End

   END
/* SEGMENT RELATED CALCULATION --> END*/

END
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to call the Segment and System SPs for Operational Data to be displayed in the Report for Before Cutoff Scenario (Time 1).',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveOperationalDataWithoutCutOffForReport',
    @level2type = NULL,
    @level2name = NULL