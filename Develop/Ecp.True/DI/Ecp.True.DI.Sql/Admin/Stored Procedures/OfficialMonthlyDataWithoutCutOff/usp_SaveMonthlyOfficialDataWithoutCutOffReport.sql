/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jul-10-2020
-- Update date: 	Aug-05-2020  Exclude Movements where source system in "ManualMovOficial & ManualInvOficial" as per PBI #28634
-- Update date: 	Aug-12-2020  Removed unnecessary join
-- Update date: 	Aug-12-2020  Added missed condition PBI 3540
-- Update date: 	Sep-25-2020  Filtering the data based on Element and Node
-- Description:     This Procedure is to Get Official Monthly InventoryQuality details based on Element, Node, StartDate, EndDate.
--EXEC [Admin].[usp_SaveMonthlyOfficialDataWithoutCutOffReport] 137236,30812,'2020-06-26','2020-06-28','bd5b494e-ac14-466b-b507-0f93fd22ed7f'
--SELECT * FROM [ADMIN].[OfficialMovementInformation]
--SELECT * FROM [Admin].[OfficialNodeTagCalculationDate]
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveMonthlyOfficialDataWithoutCutOffReport] 
(
                    @ElementId             INT
                   ,@NodeId                INT 
                   ,@StartDate             DATE                      
                   ,@EndDate               DATE                      
                   ,@ExecutionId           INT 
)
AS 
BEGIN
  SET NOCOUNT ON
  
    --Variables Declaration
    DECLARE   @NoOfDays                     INT
             ,@PreviousDayOfStartDate       DATE
             ,@Previousdate                 DATE =  [Admin].[udf_GetTrueDate] ()-1
             ,@Todaysdate                   DATE =  [Admin].[udf_GetTrueDate] ()
                              

     IF OBJECT_ID('tempdb..#TempNodeTagSystem')IS NOT NULL
     DROP TABLE #TempNodeTagSystem
          
  -- Common Code Temp table Used In Procedures(usp_SaveOfficialDataWithoutCutOffReport, usp_SaveOfficialMovementDetailsWithoutCutOff) Start

     INSERT INTO [Admin].[OfficialMovementInformation] 
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
			   ,[Version]
			   ,OwnerName
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
                Mov.BatchId                            AS BatchId, 
				Mov.[Version]                          AS [Version],
				Own.[Name]                             AS OwnerName,
                ISNULL(Mov.NetStandardVolume,0.00)     AS NetStandardVolume,
                ISNULL(Mov.GrossStandardVolume,0.00)   AS GrossStandardVolume,
                ISNULL(Mov.UncertaintyPercentage,0.00) AS UncertaintyPercentage,
				@ExecutionId                           AS ExecutionId,
                @Todaysdate                            AS CreatedDate,
				'System'							   AS CreatedBy
		FROM Admin.view_MovementInformation Mov
		INNER JOIN [Admin].[CategoryElement] CEUnits
		ON CEUnits.ElementId = Mov.MeasurementUnit
		AND CEUnits.[CategoryId] = 6 --'Unidad de Medida'
		LEFT JOIN [Offchain].[Owner] O
        ON  Mov.MovementTransactionId  = O.MovementTransactionId
        INNER JOIN [Admin].[CategoryElement] Own -- Owner
        ON Own.ElementId = O.OwnerId
		WHERE Mov.ScenarioId = 2 
		  AND Mov.SegmentId = @ElementId
		  AND Mov.OperationalDate  BETWEEN @StartDate AND @EndDate
		  AND ISNULL(Mov.SourceSystem,'') NOT IN ('TRUE','FICO','Official Manual','ManualMovOficial','ManualInvOficial')
		  AND (   Mov.SourceNodeId = @NodeId
		       OR Mov.DestinationNodeId = @NodeId
			  )

       -- Common Code Temp table Used In Procedures(usp_SaveOfficialDataWithoutCutOffReport,usp_SaveOfficialMovementDetailsWithoutCutOff) End

       -- Common Code Used For Procedures(usp_SaveOfficialDataWithoutCutOffReport,usp_SaveOfficialInventoryDetailsWithoutCutOff) Start
                               
          SELECT @NoOfDays = DATEDIFF(DAY,@StartDate,@EndDate)
          SET @PreviousDayOfStartDate = DATEADD(DAY,-1,@StartDate)
          
          SELECT * INTO #TempNodeTagSystem
          FROM (
                SELECT DISTINCT NT.NodeId,
                                NT.ElementId,
                                Ce.Name AS ElementName,
                                CASE WHEN NT.StartDate < @StartDate 
                                     THEN @StartDate
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
                  AND NT.NodeId = @NodeId
                ) A
                              
         INSERT INTO [Admin].[OfficialNodeTagCalculationDate]
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
                ,@Todaysdate                            AS CreatedDate
                ,'System'                               AS CreatedBy
         FROM #TempNodeTagSystem NT
         CROSS APPLY (SELECT dates   AS  CalculationDate
                      FROM [Admin].[udf_GetAllDates]( NT.StartDate, 
                                                      NT.EndDate, 
                                                      NT.NodeId, 
                                                      NT.ElementId)
                     ) CA
         WHERE CA.CalculationDate BETWEEN @StartDate AND @EndDate
       --Common Code Used For Procedures(usp_SaveOperationalDataWithoutCutOffReport,usp_SaveInventoryDetailsWithoutCutOff) End
END


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to feed the [OfficialMovementInformation] ,[OfficialNodeTagCalculationDate] tables ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveMonthlyOfficialDataWithoutCutOffReport',
    @level2type = NULL,
    @level2name = NULL