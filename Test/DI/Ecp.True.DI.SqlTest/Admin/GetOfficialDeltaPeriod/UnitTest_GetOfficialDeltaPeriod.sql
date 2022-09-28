/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	MAR-13-2020

-- Description:     These test cases are for stored procedure [Admin].[usp_GetOfficialDeltaPeriod]

-- Database backup Used:	dev_ecopetrol
-- ==============================================================================================================================*/

/*
The below insert statements are required to populate data into 
[Movement]
[MovementPeriod]
[Ticket]
[CategoryElement]
*/
DECLARE @InsertedMovementId INT;
DECLARE @InsertedElementId INT;
-- INSERT INTO CategoryElement Table for additional Segment when @SegmentId=0

INSERT Admin.CategoryElement (Name, CategoryId, IsActive, CreatedBy) VALUES ('Test_Segment1', 2, 1, 'Test');
SET @InsertedElementId = (SELECT IDENT_CURRENT('Admin.CategoryElement'))

-- For @IsPerNodeReport=0

-- INSERT INTO Movement Table
INSERT Offchain.Movement (SegmentId, MessageTypeId, SystemTypeId, SourceSystem, EventType, MovementId, MovementTypeId, OperationalDate, NetStandardVolume, ScenarioId, Classification, IsDeleted, CreatedBy) VALUES (10, 1,2,'SINOPER','Insert',1,53404,'2020-05-25 00:00:00.000',6888.82,2,'Movimiento',0,'Test')

-- INSERT INTO MovementPeriod Table
SET @InsertedMovementId = (SELECT IDENT_CURRENT('Offchain.Movement'))
INSERT Offchain.MovementPeriod (MovementTransactionId, StartTime, EndTime, CreatedBy) VALUES (@InsertedMovementId, '2020-07-07 01:37:53.177', '2020-07-07 01:37:53.177', 'Test')

-- INSERT INTO Movement Table
INSERT Offchain.Movement (SegmentId, MessageTypeId, SystemTypeId, SourceSystem, EventType, MovementId, MovementTypeId, OperationalDate, NetStandardVolume, ScenarioId, Classification, IsDeleted, CreatedBy) VALUES (10, 1,2,'SINOPER','Insert',1,53404,'2020-05-25 00:00:00.000',6888.82,2,'Movimiento',0,'Test')

-- INSERT INTO MovementPeriod Table
SET @InsertedMovementId = (SELECT IDENT_CURRENT('Offchain.Movement'))
INSERT Offchain.MovementPeriod (MovementTransactionId, StartTime, EndTime, CreatedBy) VALUES (@InsertedMovementId, '2020-01-01 01:37:53.177', '2020-01-02 01:37:53.177', 'Test')

-- INSERT INTO Movement Table
INSERT Offchain.Movement (SegmentId, MessageTypeId, SystemTypeId, SourceSystem, EventType, MovementId, MovementTypeId, OperationalDate, NetStandardVolume, ScenarioId, Classification, IsDeleted, CreatedBy) VALUES (10, 1,2,'SINOPER','Insert',1,53404,'2020-05-25 00:00:00.000',6888.82,2,'Movimiento',0,'Test')

-- INSERT INTO MovementPeriod Table
SET @InsertedMovementId = (SELECT IDENT_CURRENT('Offchain.Movement'))
INSERT Offchain.MovementPeriod (MovementTransactionId, StartTime, EndTime, CreatedBy) VALUES (@InsertedMovementId, '2020-06-06 01:37:53.177', '2020-06-07 01:37:53.177', 'Test')

-- INSERT INTO Movement Table for new segment
INSERT Offchain.Movement (SegmentId, MessageTypeId, SystemTypeId, SourceSystem, EventType, MovementId, MovementTypeId, OperationalDate, NetStandardVolume, ScenarioId, Classification, IsDeleted, CreatedBy) VALUES (@InsertedElementId, 1,2,'SINOPER','Insert',1,53404,'2020-05-25 00:00:00.000',6888.82,2,'Movimiento',0,'Test')

-- INSERT INTO MovementPeriod Table for new segment
SET @InsertedMovementId = (SELECT IDENT_CURRENT('Offchain.Movement'))
INSERT Offchain.MovementPeriod (MovementTransactionId, StartTime, EndTime, CreatedBy) VALUES (@InsertedMovementId, '2020-01-01 01:37:53.177', '2020-01-29 01:37:53.177', 'Test')

-- INSERT INTO Movement Table for new segment
INSERT Offchain.Movement (SegmentId, MessageTypeId, SystemTypeId, SourceSystem, EventType, MovementId, MovementTypeId, OperationalDate, NetStandardVolume, ScenarioId, Classification, IsDeleted, CreatedBy) VALUES (@InsertedElementId, 1,2,'SINOPER','Insert',1,53404,'2020-05-25 00:00:00.000',6888.82,2,'Movimiento',0,'Test')

-- INSERT INTO MovementPeriod Table for new segment
SET @InsertedMovementId = (SELECT IDENT_CURRENT('Offchain.Movement'))
INSERT Offchain.MovementPeriod (MovementTransactionId, StartTime, EndTime, CreatedBy) VALUES (@InsertedMovementId, '2020-03-04 01:37:53.177', '2020-03-19 01:37:53.177', 'Test')

-- INSERT INTO Movement Table for new segment
INSERT Offchain.Movement (SegmentId, MessageTypeId, SystemTypeId, SourceSystem, EventType, MovementId, MovementTypeId, OperationalDate, NetStandardVolume, ScenarioId, Classification, IsDeleted, CreatedBy) VALUES (@InsertedElementId, 1,2,'SINOPER','Insert',1,53404,'2020-05-25 00:00:00.000',6888.82,2,'Movimiento',0,'Test')

-- INSERT INTO MovementPeriod Table for new segment
SET @InsertedMovementId = (SELECT IDENT_CURRENT('Offchain.Movement'))
INSERT Offchain.MovementPeriod (MovementTransactionId, StartTime, EndTime, CreatedBy) VALUES (@InsertedMovementId, '2020-09-15 01:37:53.177', '2020-09-22 01:37:53.177', 'Test')


--EXECUTE STORED PROCEDURE for specific segment
EXEC [Admin].[usp_GetOfficialDeltaPeriod] @SegmentId=10, @NoOfYears=5
--OR
EXEC [Admin].[usp_GetOfficialDeltaPeriod] @SegmentId=10, @NoOfYears=5,@IsPerNodeReport=0

/* OUTPUT

YearInfo	MonthInfo
2016         0
2017         0
2018         0
2019         0
2020         1
2020         6
2020         7

*/

--EXECUTE STORED PROCEDURE for all segments
EXEC [Admin].[usp_GetOfficialDeltaPeriod] @SegmentId=0, @NoOfYears=5
--OR
EXEC [Admin].[usp_GetOfficialDeltaPeriod] @SegmentId=0, @NoOfYears=5,@IsPerNodeReport=0

/* OUTPUT

YearInfo	MonthInfo
2016         0
2017         0
2018         0
2019         0
2020         1
2020         3
2020         6
2020         7
2020         3
*/

-- For @IsPerNodeReport=1

-- INSERT INTO Ticket Table

INSERT Admin.Ticket (CategoryElementId, StartDate, EndDate, Status, TicketTypeId, CreatedBy) VALUES (0, '2020-04-01 01:37:53.177', '2020-04-15 01:37:53.177', 4, 5, 'Test')

-- INSERT INTO TICKET Table

INSERT Admin.Ticket (CategoryElementId, StartDate, EndDate, Status, TicketTypeId, CreatedBy) VALUES (0, '2020-06-02 01:37:53.177', '2020-06-21 01:37:53.177', 4, 5, 'Test')


-- INSERT INTO Ticket Table for new segment
INSERT Admin.Ticket (CategoryElementId, StartDate, EndDate, Status, TicketTypeId, CreatedBy) VALUES (@InsertedElementId, '2019-01-12 01:37:53.177', '2019-01-15 01:37:53.177', 4, 5, 'Test')

-- INSERT INTO Ticket Table for new segment
INSERT Admin.Ticket (CategoryElementId, StartDate, EndDate, Status, TicketTypeId, CreatedBy) VALUES (@InsertedElementId, '2020-09-03 01:37:53.177', '2020-09-22 01:37:53.177', 4, 5, 'Test')

--EXECUTE STORED PROCEDURE for specific segment
EXEC [Admin].[usp_GetOfficialDeltaPeriod] @SegmentId=10, @NoOfYears=5,@IsPerNodeReport=1

/* OUTPUT

YearInfo	MonthInfo
2016         0
2017         0
2018         0
2019         0
2020         4
2020         6

*/

--EXECUTE STORED PROCEDURE for all segments
EXEC [Admin].[usp_GetOfficialDeltaPeriod] @SegmentId=0, @NoOfYears=5,@IsPerNodeReport=1

/* OUTPUT

YearInfo	MonthInfo
2016         0
2017         0
2018         0
2019         1
2020         4
2020         6
2020         9
*/