/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-03-2020
--<Description>: This table holds the data for Owner and corresponding partner mapping data. </Description>
==================================================================================================================================*/
CREATE TABLE [Admin].[PartnerOwnerMapping]
(
   -- Columns 
   [PartnerOwnerMappingId]      INT         IDENTITY (1,1),
   [GrandOwnerId]               INT         NOT NULL,
   [PartnerOwnerId]             INT         NOT NULL,

   --Constraints
    CONSTRAINT [FK_GrandOwnerId]		    FOREIGN KEY ([GrandOwnerId]) REFERENCES [Admin].[CategoryElement] ([ElementId]),
	CONSTRAINT [FK_PartnerOwnerId]		    FOREIGN KEY ([PartnerOwnerId]) REFERENCES [Admin].[CategoryElement] ([ElementId]),
);
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for Owner and corresponding partner mapping data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PartnerOwnerMapping',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PartnerOwnerMapping',
    @level2type = N'COLUMN',
    @level2name = N'PartnerOwnerMappingId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PartnerOwnerMapping',
    @level2type = N'COLUMN',
    @level2name = N'GrandOwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The description of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'PartnerOwnerMapping',
    @level2type = N'COLUMN',
    @level2name = N'PartnerOwnerId'
GO
