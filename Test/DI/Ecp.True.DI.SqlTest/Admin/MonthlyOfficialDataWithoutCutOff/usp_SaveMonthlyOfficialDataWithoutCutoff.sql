/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jul-10-2020
-- Description:     This test script is to test usp_SaveMonthlyOfficialDataWithoutCutoff based on Node, StartDate, EndDate and ExecutionID.
-- EXEC [Admin].[usp_SaveMonthlyOfficialDataWithoutCutOff] @ElementId, @NodeId, '2021-08-08' ,'2021-08-08', @ReportExecutionId
-- SELECT * FROM [Admin].[OfficialMonthlyBalance]
-- Updated date: Ago-08-2020  -- To add missing transfer to the Official Monthly Balance
-- ==============================================================================================================================*/

DECLARE @FileRegistrationId             INT,
        @FileRegistrationTransactionId  INT,
        @ReportExecutionId              INT,
        @ElementId                      INT             =           15617,
        @NodeId                         INT             =           4127,
        @UploadId                       NVARCHAR(50)    =           '0B5D8AC0-FB0B-4D6E-BB31-ACAC0AE407BE',
        @BlobPath                       NVARCHAR(50)    =           '/container/registerfiles',
        @StartDate                      DATE            =           '2021-08-08',
        @EndDate                        DATE            =           '2021-08-08',
        @Control1                       INT,
        @Control2                       INT,
        @COUNT                          INT


--___________________________________________________________________________________________________________________________________
-------------------------------------------------------- File registration ----------------------------------------------------------

-- Insert rows into table 'FileRegistration' in schema '[Admin]'
INSERT INTO [Admin].[FileRegistration]
    ([UploadId], [UploadDate], [Name], [Action], [Status], [SystemTypeId], [BlobPath], [CreatedBy])
VALUES
    (@UploadId, @StartDate, 'FileName_0.xls', 2, 1, 3, @BlobPath, 'System')

SET @FileRegistrationId = SCOPE_IDENTITY();

--___________________________________________________________________________________________________________________________________
-------------------------------------------------------- Insert Report excecution Data ----------------------------------------------


-- Insert rows into table 'ReportExcecution' in schema '[Admin]'
INSERT INTO [Admin].[ReportExecution]
    ([StartDate], [EndDate], [StatusTypeId], [ReportTypeId], [ScenarioId], [CreatedBy], [CreatedDate], [Hash])
VALUES
    ([Admin].[udf_GetTrueDate] (), [Admin].[udf_GetTrueDate] (), 0, 3, 2, 'System', [Admin].[udf_GetTrueDate] (), '778899112233')

SET @ReportExecutionId = SCOPE_IDENTITY()

--___________________________________________________________________________________________________________________________________
-------------------------------------------------------- Insert File Registration Transaction Data ----------------------------------


-- Insert rows into table 'FileRegistrationTransaction' in schema '[Admin]' 
INSERT INTO [Admin].[FileRegistrationTransaction]
    ([FileRegistrationId], [StatusTypeId], [CreatedBy], [CreatedDate])
VALUES
    (@FileRegistrationId, 1, 'System', [Admin].[udf_GetTrueDate] ())

SET @FileRegistrationTransactionId = SCOPE_IDENTITY();

--____________________________________________________________________________________________________________________________________
-------------------------------------------------------- Insert Movement Data --------------------------------------------------------


INSERT INTO [OffChain].[Movement]
    (MessageTypeId, SystemTypeId, SourceSystemId, EventType, MovementId, MovementTypeId, TicketId, SegmentId, OperationalDate, GrossStandardVolume, NetStandardVolume, UncertaintyPercentage, MeasurementUnit, ScenarioId, Observations, Classification, IsDeleted, FileRegistrationTransactionId, BlockchainStatus, BlockchainMovementTransactionId, TransactionHash, BlockNumber, RetryCount, IsOfficial, Version, IsTransferPoint, CreatedBy, CreatedDate, LastmodifiedBy)
VALUES
    (1, 3, 164, 'Insert', '1800299', '49', 24271, @ElementId, @StartDate, 200.00, 200.00, 0.22, '31', 2, 'Reporte Operativo Cusiana - Fecha', 'Movimiento', 0, @FileRegistrationId, 1, '1DC2CBE4-426A-4D6F-B667-6F37BAEA6C12', '0x77b599bf3dcd803810156ab00a31d69a2eb517be99cb1b3bb3a799f07578216d', '0x2e14', 0, 0, '1', 0, 'Test', @StartDate, 'trueadmin'),
    (1, 3, 164, 'Insert', '3697954', '49', 24271, @ElementId, @StartDate, 200.00, 200.00, 0.22, '31', 2, 'Reporte Operativo Cusiana - Fecha', 'Movimiento', 0, @FileRegistrationId, 1, 'EC9300AF-6DDA-4516-ADC7-CA1594D23AF1', '0x23ecbbce36b138df1f731a43eaa22322dc1701cda99dc6604962305fcb41561d', '0x2e14', 0, 0, '1', 0, 'Test', @StartDate, 'trueadmin'),
    (1, 3, 164, 'Insert', '5501360', '49', 24271, @ElementId, @StartDate, 200.00, 200.00, 0.22, '31', 2, 'Reporte Operativo Cusiana - Fecha', 'Movimiento', 0, @FileRegistrationId, 1, '0FDD92D7-F8CB-4C66-9AAD-C95B878D1025', '0x4b57f13bffbe72ebb9e755efd41cf5c52e2ee129ea557abd239d1cd9cc4cd8b9', '0x2e14', 0, 0, '1', 0, 'Test', @StartDate, 'trueadmin'),
    (1, 3, 164, 'Insert', '3397480', '49', 24271, @ElementId, @StartDate, 200.00, 200.00, 0.22, '31', 2, 'Reporte Operativo Cusiana - Fecha', 'Movimiento', 0, @FileRegistrationId, 1, '070A8743-16C6-42EE-AD7F-1637FC9AB097', '0xdb68b4d9726ba160c9782cb1fabfe9ceaaa5f4215762cee622df6f879a10cc1f', '0x2e14', 0, 0, '1', 0, 'Test', @StartDate, 'trueadmin')


--___________________________________________________________________________________________________________________________________
-------------------------------------------------------- Movement Source Data -------------------------------------------------------


INSERT INTO [Offchain].[MovementSource]
    (MovementTransactionId, CreatedBy, [SourceNodeId], SourceProductId, CreatedDate)
VALUES
    ((SELECT TOP 1
            MovementTransactionId
        FROM [OffChain].[Movement]
        WHERE MovementId = '3697954'), 'System', @NodeId, 10000002049, [Admin].[udf_GetTrueDate] ()),
    ((SELECT TOP 1
            MovementTransactionId
        FROM [OffChain].[Movement]
        WHERE MovementId = '1800299'), 'System', @NodeId, 10000002199, [Admin].[udf_GetTrueDate] ()),
    ((SELECT TOP 1
            MovementTransactionId
        FROM [OffChain].[Movement]
        WHERE MovementId = '5501360'), 'System', @NodeId, 10000002049, [Admin].[udf_GetTrueDate] ()),
    ((SELECT TOP 1
            MovementTransactionId
        FROM [OffChain].[Movement]
        WHERE MovementId = '3397480'), 'System', @NodeId, 10000002199, [Admin].[udf_GetTrueDate] ())


--___________________________________________________________________________________________________________________________________
-------------------------------------------------------- Movement Destination Data -------------------------------------------------------


INSERT INTO [Offchain].[MovementDestination]
    (MovementTransactionId, CreatedBy, [DestinationNodeId], DestinationProductId, CreatedDate)
VALUES
    ((SELECT TOP 1
            MovementTransactionId
        FROM [OffChain].[Movement]
        WHERE MovementId = '3697954'), 'System', @NodeId, 10000002199, [Admin].[udf_GetTrueDate] ()),
    ((SELECT TOP 1
            MovementTransactionId
        FROM [OffChain].[Movement]
        WHERE MovementId = '1800299'), 'System', @NodeId, 10000002049, [Admin].[udf_GetTrueDate] ()),
    ((SELECT TOP 1
            MovementTransactionId
        FROM [OffChain].[Movement]
        WHERE MovementId = '5501360'), 'System', @NodeId, 10000002199, [Admin].[udf_GetTrueDate] ()),
    ((SELECT TOP 1
            MovementTransactionId
        FROM [OffChain].[Movement]
        WHERE MovementId = '3397480'), 'System', @NodeId, 10000002049, [Admin].[udf_GetTrueDate] ())

--___________________________________________________________________________________________________________________________________
-------------------------------------------------------- Insert Owner Data ----------------------------------------------------------


INSERT INTO [Offchain].[Owner]
    (OwnerId, OwnershipValue, OwnershipValueUnit, InventoryProductId, MovementTransactionId, BlockchainStatus, BlockchainMovementTransactionId, BlockchainInventoryProductTransactionId, CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
VALUES
    (30, 100.00, '%', 1, (SELECT TOP 1
            MovementTransactionId
        FROM [OffChain].[Movement]
        WHERE MovementId = '3697954'), 0, NULL, 'D5BC9378-F9FB-4738-8BFF-3E9D28EF3DE4', 'Test', @StartDate, NULL, NULL),
    (30, 100.00, '%', 1, (SELECT TOP 1
            MovementTransactionId
        FROM [OffChain].[Movement]
        WHERE MovementId = '1800299'), 0, NULL, 'FB65451C-1DD1-4353-85BA-F5ABCD138287', 'Test', @StartDate, NULL, NULL),
    (30, 100.00, '%', 1, (SELECT TOP 1
            MovementTransactionId
        FROM [OffChain].[Movement]
        WHERE MovementId = '5501360'), 0, NULL, 'FB65451C-1DD1-4353-85BA-F5ABC7138287', 'Test', @StartDate, NULL, NULL),
    (30, 100.00, '%', 1, (SELECT TOP 1
            MovementTransactionId
        FROM [OffChain].[Movement]
        WHERE MovementId = '3397480'), 0, NULL, 'FB65451C-1DD1-4353-85BA-F5ABA7138287', 'Test', @StartDate, NULL, NULL)


--___________________________________________________________________________________________________________________________________
-------------------------------------------------------- Execute SPs to Produce Report Data -----------------------------------------


EXEC [Admin].[usp_SaveMonthlyOfficialDataWithoutCutOffReport]       @ElementId, @NodeId, @StartDate, @EndDate, @ReportExecutionId
EXEC [Admin].[usp_SaveMonthlyOfficialMovementDetailsWithoutCutOff]  @ElementId, @NodeId, @StartDate, @EndDate, @ReportExecutionId
EXEC [Admin].[usp_SaveMonthlyOfficialDataWithoutCutOff]             @ElementId, @NodeId, @StartDate, @EndDate, @ReportExecutionId

--___________________________________________________________________________________________________________________________________
-------------------------------------------------------- Execute Test Cases ---------------------------------------------------------


SELECT *
FROM [Admin].[OfficialMonthlyBalance]
WHERE ExecutionId = @ReportExecutionId
ORDER BY Product ASC

SELECT TOP 1
    @Control1 = Control
FROM [Admin].[OfficialMonthlyBalance]
WHERE ExecutionId = @ReportExecutionId
ORDER BY Product ASC

SELECT TOP 1
    @Control2 = Control
FROM [Admin].[OfficialMonthlyBalance]
WHERE ExecutionId = @ReportExecutionId
ORDER BY Product ASC

SET @COUNT = (SELECT COUNT(*)
FROM [Admin].[OfficialMonthlyBalance]
WHERE ExecutionId = @ReportExecutionId)

IF @COUNT <> 2 THROW 50000, 'The official monthly balance count should be 2, one per product', 1;
IF @Control1 <> 0.00 THROW 50000, 'The control value of product1 should be 0.00', 1;
IF @Control2 <> 0.00 THROW 50000, 'The control value of product2 should be 0.00', 1;

--___________________________________________________________________________________________________________________________________
-------------------------------------------------------- Delete Test Data -----------------------------------------------------------

-- Delete rows from a Table or View '[Movement]' in schema '[OffChain]'
DELETE
FROM [OffChain].[MovementSource]
WHERE MovementTransactionId IN 
(
    (SELECT TOP 1
    MovementTransactionId
FROM [OffChain].[Movement]
WHERE MovementId = '1800299'),
    (SELECT TOP 1
    MovementTransactionId
FROM [OffChain].[Movement]
WHERE MovementId = '3697954'),
    (SELECT TOP 1
    MovementTransactionId
FROM [OffChain].[Movement]
WHERE MovementId = '5501360'),
    (SELECT TOP 1
    MovementTransactionId
FROM [OffChain].[Movement]
WHERE MovementId = '3397480')
)

-- Delete rows from a Table or View '[Movement]' in schema '[OffChain]'
DELETE
FROM [OffChain].[MovementDestination]
WHERE MovementTransactionId IN 
(
    (SELECT TOP 1
    MovementTransactionId
FROM [OffChain].[Movement]
WHERE MovementId = '1800299'),
    (SELECT TOP 1
    MovementTransactionId
FROM [OffChain].[Movement]
WHERE MovementId = '3697954'),
    (SELECT TOP 1
    MovementTransactionId
FROM [OffChain].[Movement]
WHERE MovementId = '5501360'),
    (SELECT TOP 1
    MovementTransactionId
FROM [OffChain].[Movement]
WHERE MovementId = '3397480')
)

-- Delete rows from a Table or View '[Owner]' in schema '[OffChain]'
DELETE
FROM [OffChain].[Owner]
WHERE BlockChainInventoryProductTransactionId IN
(
    'D5BC9378-F9FB-4738-8BFF-3E9D28EF3DE4',
    'FB65451C-1DD1-4353-85BA-F5ABCD138287',
    'FB65451C-1DD1-4353-85BA-F5ABC7138287',
    'FB65451C-1DD1-4353-85BA-F5ABA7138287'
)

-- Delete rows from a Table or View '[Movement]' in schema '[OffChain]'
DELETE
FROM [OffChain].[Movement]
WHERE BlockchainMovementTransactionId IN 
(
   '1DC2CBE4-426A-4D6F-B667-6F37BAEA6C12',
   'EC9300AF-6DDA-4516-ADC7-CA1594D23AF1',
   '0FDD92D7-F8CB-4C66-9AAD-C95B878D1025',
   '070A8743-16C6-42EE-AD7F-1637FC9AB097'
)

-- Delete rows from table '[FileRegistrationTransaction]' in schema '[Admin]'
DELETE FROM [Admin].[FileRegistrationTransaction]
WHERE FileRegistrationId = (SELECT FileRegistrationId
FROM [Admin].[FileRegistration]
WHERE UploadId = @UploadId)

-- Delete rows from table '[FileRegistration]' in schema '[Admin]'
DELETE FROM [Admin].[FileRegistration]
WHERE [UploadId] = @UploadId

-- Delete rows from table '[OfficialMovementInformation]' in schema '[Admin]'
DELETE FROM [Admin].[OfficialMovementInformation]
WHERE ExecutionId IN (SELECT ExecutionId
FROM [Admin].[ReportExecution]
WHERE [Hash] = '778899112233')

-- Delete rows from table '[OfficialNodeTagCalculationDate]' in schema '[Admin]'
DELETE FROM [Admin].[OfficialNodeTagCalculationDate]
WHERE ExecutionId IN (SELECT ExecutionId
FROM [Admin].[ReportExecution]
WHERE [Hash] = '778899112233')

-- Delete rows from table '[TableName]' in schema '[dbo]'
DELETE FROM [Admin].[OfficialMonthlyMovementDetails]
WHERE ExecutionId IN (SELECT ExecutionId
FROM [Admin].[ReportExecution]
WHERE [Hash] = '778899112233')

-- Delete rows from table '[OfficialMonthlyBalance]' in schema '[Admin]'
DELETE FROM [Admin].[OfficialMonthlyBalance]
WHERE ExecutionId IN (SELECT ExecutionId
FROM [Admin].[ReportExecution]
WHERE [Hash] = '778899112233')

-- Delete rows from table '[ReportExcecution]' in schema '[Admin]'
DELETE FROM [Admin].[ReportExecution]
WHERE [Hash] = '778899112233'
