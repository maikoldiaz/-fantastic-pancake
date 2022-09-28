/*-- ========================================================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Oct-29-2020
-- <Description>:   This procedure is to Save delta bulk movements.
-- ========================================================================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_SaveDeltaBulkMovements]
(
		 @Movements		 [ADMIN].[MovementType] READONLY,
		 @MovementOwners [ADMIN].[MovementOwnerType] READONLY
)
AS
BEGIN

DECLARE @TodaysDateTime  DATETIME = Admin.udf_GetTrueDate()

IF OBJECT_ID('tempdb..#TempMovementMapping') IS NOT NULL
			DROP TABLE #TempMovementMapping

IF OBJECT_ID('tempdb..#TempMovement') IS NOT NULL
			DROP TABLE #TempMovement

IF OBJECT_ID('tempdb..#TempMovementOwner') IS NOT NULL
			DROP TABLE #TempMovementOwner

CREATE TABLE #TempMovement ( 
	        [Id]							INT,
	        [TempId]						INT,
	        [MovementTypeId]				INT,
	        [MessageTypeId]					INT,
	        [SystemTypeId]					INT,
	        [SourceSystemId]				INT,
	        [EventType]						NVARCHAR (25),
	        [MovementId]					NVARCHAR (50) collate SQL_Latin1_General_CP1_CS_AS,
	        [IsSystemGenerated]				BIT,
	        [OfficialDeltaTicketId]			INT,
	        [ScenarioId]					INT,
	        [Observations]					NVARCHAR (150),
	        [Classification]				NVARCHAR(30),
	        [PendingApproval]				BIT,
	        [NetStandardVolume]				DECIMAL (18, 2),
	        [SourceMovementTransactionId]	INT,
	        [MeasurementUnit]				INT,
	        [SegmentId]						INT,
	        [OperationalDate]				DATE,
	        [OfficialDeltaMessageTypeId]	INT,
	        [PeriodStartTime]				DATETIME,
	        [PeriodEndTime]					DATETIME,
	        [SourceNodeId]					INT,
	        [SourceProductId]				NVARCHAR (20),
	        [SourceProductTypeId]			INT,
	        [DestinationNodeId]				INT,
	        [DestinationProductId]			NVARCHAR (20),
	        [DestinationProductTypeId]		INT,
			[SourceInventoryProductId]      INT NULL,
			[ConsolidatedMovementTransactionId]  INT NULL,
			[ConsolidatedInventoryProductId]	 INT NULL,
			[OriginalMovementTransactionId]		 INT NULL,
			[BlockchainStatus]                   INT NOT NULL,
	        [CreatedBy]						NVARCHAR (260))

-- Insert into temp table for performance
INSERT INTO #TempMovement (
			[Id]
	        ,[TempId]						
	        ,[MovementTypeId]				
	        ,[MessageTypeId]					
	        ,[SystemTypeId]					
	        ,[SourceSystemId]				
	        ,[EventType]						
	        ,[MovementId]					
	        ,[IsSystemGenerated]				
	        ,[OfficialDeltaTicketId]			
	        ,[ScenarioId]					
	        ,[Observations]					
	        ,[Classification]				
	        ,[PendingApproval]				
	        ,[NetStandardVolume]				
	        ,[SourceMovementTransactionId]	
	        ,[MeasurementUnit]				
	        ,[SegmentId]						
	        ,[OperationalDate]				
	        ,[OfficialDeltaMessageTypeId]	
	        ,[PeriodStartTime]				
	        ,[PeriodEndTime]					
	        ,[SourceNodeId]					
	        ,[SourceProductId]				
	        ,[SourceProductTypeId]			
	        ,[DestinationNodeId]				
	        ,[DestinationProductId]			
	        ,[DestinationProductTypeId]
			,[SourceInventoryProductId]
			,[ConsolidatedMovementTransactionId]
			,[ConsolidatedInventoryProductId]
			,[OriginalMovementTransactionId]
			,[BlockchainStatus]
	        ,[CreatedBy])
SELECT * FROM @Movements

CREATE TABLE #TempMovementOwner ( 
	        [Id]					INT
	       ,[TempId]				INT
	       ,[OwnerId]				INT
	       ,[OwnershipValue]		DECIMAL (18, 2)
	       ,[OwnershipValueUnit]	NVARCHAR (50)
		   ,[BlockchainStatus]      INT NOT NULL
	       ,[CreatedBy]				NVARCHAR (260))

-- Insert into temp table for performance
INSERT INTO #TempMovementOwner (
			[Id]					
	       ,[TempId]				
	       ,[OwnerId]				
	       ,[OwnershipValue]		
	       ,[OwnershipValueUnit]	
		   ,[BlockchainStatus]
	       ,[CreatedBy])
SELECT * FROM @MovementOwners

CREATE TABLE #TempMovementMapping (MovementTransactionId INT, TempId INT)

	 BEGIN TRY
		 BEGIN TRANSACTION

		 -- Insert into Offchain.Movement Table
		 MERGE INTO Offchain.Movement M
		 using #TempMovement TM on TM.Id = M.MovementTransactionId
		 WHEN NOT MATCHED THEN
			INSERT(
			 [MovementTypeId]
			,[MessageTypeId]
			,[SystemTypeId]
			,[SourceSystemId]
			,[EventType]
			,[MovementId]
			,[IsSystemGenerated]
			,[OfficialDeltaTicketId]
			,[ScenarioId]
			,[Observations]
			,[Classification]
			,[PendingApproval]
			,[NetStandardVolume]
			,[SourceMovementTransactionId]
			,[MeasurementUnit]
			,[SegmentId]
			,[OperationalDate]
			,[OfficialDeltaMessageTypeId]
			,[SourceInventoryProductId]
			,[ConsolidatedMovementTransactionId]
			,[ConsolidatedInventoryProductId]
			,[OriginalMovementTransactionId]
			,[BlockchainStatus]
			,[CreatedBy]
			,[CreatedDate])
		VALUES(
		     TM.[MovementTypeId]
		    ,TM.[MessageTypeId]
			,TM.[SystemTypeId]
			,TM.[SourceSystemId]
			,TM.[EventType]
			,TM.[MovementId]
			,TM.[IsSystemGenerated]
			,TM.[OfficialDeltaTicketId]
			,TM.[ScenarioId]
			,TM.[Observations]
			,TM.[Classification]
			,TM.[PendingApproval]
			,TM.[NetStandardVolume]
			,TM.[SourceMovementTransactionId]
			,TM.[MeasurementUnit]
			,TM.[SegmentId]
			,TM.[OperationalDate]
			,TM.[OfficialDeltaMessageTypeId]
			,TM.[SourceInventoryProductId]
			,TM.[ConsolidatedMovementTransactionId]
			,TM.[ConsolidatedInventoryProductId]
			,TM.[OriginalMovementTransactionId]
			,TM.[BlockchainStatus]
			,TM.[CreatedBy]
			,@TodaysDateTime
		)
		OUTPUT inserted.MovementTransactionId, TM.TempId
		INTO #TempMovementMapping (MovementTransactionId, TempId);

		-- Insert into Offchain.MovementSource table
		INSERT INTO Offchain.MovementSource(
			 [MovementTransactionId]
			,[SourceNodeId]
			,[SourceProductId]
			,[SourceProductTypeId]
			,[CreatedBy] 
			,[CreatedDate] )
	    SELECT 
		     TM.[MovementTransactionId]
	        ,M.[SourceNodeId]
	        ,M.[SourceProductId]
	        ,M.[SourceProductTypeId]
	        ,M.[CreatedBy]
	        ,@TodaysDateTime
		FROM #TempMovementMapping TM
		JOIN @Movements M ON TM.TempId = M.TempId 

		-- Insert into Offchain.MovementDestination table
		INSERT INTO Offchain.MovementDestination(
			 [MovementTransactionId]
			,[DestinationNodeId]
			,[DestinationProductId]
			,[DestinationProductTypeId]
			,[CreatedBy] 
			,[CreatedDate] )
	    SELECT
		     TM.[MovementTransactionId]
	        ,M.[DestinationNodeId]
	        ,M.[DestinationProductId]
	        ,M.[DestinationProductTypeId]
	        ,M.[CreatedBy]
	        ,@TodaysDateTime
		FROM #TempMovementMapping TM
		JOIN @Movements M ON TM.TempId = M.TempId

		-- Insert into Offchain.MovementPeriod table
		INSERT INTO Offchain.MovementPeriod (
			 [MovementTransactionId]
			,[StartTime]
			,[EndTime]
			,[CreatedBy] 
			,[CreatedDate] )
	    SELECT 
		     TM.[MovementTransactionId]
	        ,M.[PeriodStartTime]
	        ,M.[PeriodEndTime]
	        ,M.[CreatedBy]
	        ,@TodaysDateTime
		FROM #TempMovementMapping TM
		JOIN @Movements M ON TM.TempId = M.TempId

		-- Insert into Offchain.Owner table
        MERGE INTO Offchain.OWNER AS TARGET
	    USING (SELECT 
				  TM.MovementTransactionId
				 ,TM.TempId
				 ,MO.[Id]
				 ,MO.[OwnerId]
				 ,MO.[OwnershipValue]
				 ,MO.[OwnershipValueUnit]
				 ,MO.[BlockchainStatus]
	             ,MO.[CreatedBy]
			 FROM #TempMovementMapping TM
			 JOIN #TempMovementOwner MO ON TM.TempId = MO.TempId) AS SOURCE
			 ON TARGET.Id = SOURCE.Id
	    WHEN NOT MATCHED THEN
			INSERT(
			 [MovementTransactionId]
			,[OwnerId]
			,[OwnershipValue]
			,[OwnershipValueUnit] 
			,[BlockchainStatus]
			,[CreatedBy] 
			,[CreatedDate])
	    VALUES(
		     SOURCE.[MovementTransactionId]
	        ,SOURCE.[OwnerId]
	        ,SOURCE.[OwnershipValue]
	        ,SOURCE.[OwnershipValueUnit]
			,SOURCE.[BlockchainStatus]
	        ,SOURCE.[CreatedBy]
	        ,@TodaysDateTime);

		 COMMIT TRANSACTION
	 END TRY
	 BEGIN CATCH
             IF @@TRANCOUNT > 0
                    ROLLBACK TRANSACTION;
             THROW
     END CATCH
END

 GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This procedure is to Save delta bulk movements',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveDeltaBulkMovements',
    @level2type = NULL,
    @level2name = NULL

 
