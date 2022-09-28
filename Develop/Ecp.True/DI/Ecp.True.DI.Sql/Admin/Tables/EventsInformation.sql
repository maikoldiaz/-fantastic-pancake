/*==============================================================================================================================
--Author:        Microsoft
--Created date : Apr-30-2020
--<Description>: This table holds the data for EventsInformation, used in EventContract report. </Description>
================================================================================================================================*/
CREATE TABLE [Admin].[EventsInformation] 
(
	--Columns
      [RNo]					INT				 NOT NULL,
      [PropertyEvent]		NVARCHAR (150)	 NOT NULL,
      [SourceNode]			NVARCHAR (150)	 NULL,
      [DestinationNode]		NVARCHAR (150)	 NULL,
      [SourceProduct]		NVARCHAR (150)	 NULL,
      [DestinationProduct]	NVARCHAR (150)	 NULL,
      [StartDate]           DATETIME         NOT NULL,		 
      [EndDate]             DATETIME         NOT NULL,		 
      [Owner1Name]			NVARCHAR (150)	 NOT NULL,
      [Owner2Name]			NVARCHAR (150)	 NOT NULL,
      [Volume]				DECIMAL(18, 2)	 NOT NULL,
      [MeasurementUnit]		NVARCHAR (150)	 NOT NULL,
      [InputElement]		NVARCHAR (150)	 NOT NULL,
      [InputNodeName]       NVARCHAR (150)   NOT NULL,
      [ExecutionId] 		NVARCHAR (250)	 NOT NULL,
    
     --Internal Common Columns															
     [CreatedBy]			NVARCHAR (260)	NOT NULL,
     [CreatedDate]			DATETIME		NOT NULL    DEFAULT Admin.udf_GetTrueDate()
)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for EventsInformation, used in EventContract report.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'A rank generated over source node and start date and inserted in this column',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'RNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the event type category element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'PropertyEvent'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'SourceNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the destination node',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'DestinationNode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the source product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'SourceProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of destination product',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'DestinationProduct'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The start date of the event',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'StartDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The end date of the event',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'EndDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The first owner name (category element of owner category, like Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'Owner1Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The second owner name (category element of owner category, like Ecopetrol)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'Owner2Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The event volume',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'Volume'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the measurement unit (category element of unit category)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'MeasurementUnit'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'InputElement'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the node name',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'InputNodeName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The execution id unique to session received from UI',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'ExecutionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created  (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'EventsInformation',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'