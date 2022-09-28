/*==============================================================================================================================
--Author:        Microsoft
--Created date : Jun-10-2020
--<Description>: This table holds the data Movement Related to Report Of Segment.</Description>
=================================================================================================================================*/
CREATE TABLE [Admin].[MovementInformationMovforSegmentReport] 
( 
     MovementInformationMovforSegmentReportId   INT               NOT NULL IDENTITY(1,1),
     OperationalDate                            DATE              NULL, 
     SourceProductId                            NVARCHAR(20)      NULL, 
     SourceProductName                          NVARCHAR(150)     NULL, 
     SegmentID                                  INT               NULL, 
     SourceNodeId                               INT               NULL, 
     SourceNodeName                             NVARCHAR(150)     NULL, 
     SourceNodeNameIsGeneric                    INT               NULL, 
     DestinationProductId                       NVARCHAR(20)      NULL, 
     DestinationProductName                     NVARCHAR(150)     NULL, 
     DestinationNodeId                          INT               NULL, 
     DestinationNodeName                        NVARCHAR(150)     NULL, 
     DestinationNodeNameIsGeneric               INT               NULL, 
     MessageTypeId                              INT               NULL,
     [Classification]                           NVARCHAR(30)      NULL,
     MovementID                                 VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS NULL, 
     MovementTypeName                           NVARCHAR(150)     NULL, 
     MeasurementUnit                            NVARCHAR(150)     NULL, 
     MovementTransactionId                      INT               NULL, 
     EventType                                  NVARCHAR(25)      NULL, 
     SystemName                                 VARCHAR(50)       NULL, 
     SourceSystem                               NVARCHAR(25)      NULL, 
     NetStandardVolume                          DECIMAL(18, 2)    NULL, 
     GrossStandardVolume                        DECIMAL(18, 2)    NULL, 
     UncertaintyPercentage                      DECIMAL(5, 2)     NULL, 
     BatchId                                    NVARCHAR(255)     NULL,
     ExecutionId                                INT               NULL, 
	
	 --Internal Common Columns	 
     CreatedBy                                  NVARCHAR (260)    NULL, 
     CreatedDate                                DATETIME          NULL,

	--Constraints
    CONSTRAINT [PK_MovementInformationMovforSegmentReportId]	            PRIMARY KEY CLUSTERED (MovementInformationMovforSegmentReportId ASC),
    CONSTRAINT [FK_MovementInformationMovforSegmentReport_ReportExecution]	FOREIGN KEY ([ExecutionId])			REFERENCES [Admin].[ReportExecution] ([ExecutionId])
) 
GO
 
ALTER TABLE [Admin].[MovementInformationMovforSegmentReport]  SET (LOCK_ESCALATION = DISABLE)
GO
--CREATE NONCLUSTERED INDEX NCI_MovementInformationMovforSegmentReport
--ON [Admin].[MovementInformationMovforSegmentReport] ([InputCategory],[InputElementName],[InputNodeName],[InputStartDate],[InputEndDate],[ExecutionId])
--INCLUDE (
--         [SourceProductId],[SourceProductName],[SegmentID],[SourceNodeId],[SourceNodeName]
--        ,[SourceNodeNameIsGeneric],[DestinationProductId],[DestinationProductName],[DestinationNodeId],[DestinationNodeName]
--        ,[DestinationNodeNameIsGeneric],[MessageTypeId],[Classification],[MovementID],[MovementTransactionId],[SourceSystem]
--        ,[NetStandardVolume],[GrossStandardVolume],[UncertaintyPercentage]
--        )
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data Movement Related to Report Of Segment.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'MovementInformationMovforSegmentReportId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The operational date',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'OperationalDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'SourceProductName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the element of segment category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'SegmentID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag to show if source node name is generic',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'SourceNodeNameIsGeneric'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProductName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The flag to show if destination node name is generic',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNodeNameIsGeneric'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of message type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'MessageTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The classification',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'Classification'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The business identifier of movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'MovementID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the element of movement type category',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'MovementTypeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The measurement unit',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the movement',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'MovementTransactionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The event type',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'EventType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'SystemName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source system',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'SourceSystem'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The net standard volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'NetStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The gross standard volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'GrossStandardVolume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The uncertainty percentage',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'UncertaintyPercentage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the batch',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'BatchId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The execution id unique to session received from UI',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'MovementInformationMovforSegmentReport',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'