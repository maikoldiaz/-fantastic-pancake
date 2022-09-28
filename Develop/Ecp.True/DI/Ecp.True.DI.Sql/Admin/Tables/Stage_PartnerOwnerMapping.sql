/*==============================================================================================================================
--Author:        Microsoft
--Created date : Aug-12-2020
--<Description>: This table holds the data for Owner and corresponding partner mapping data. </Description>
==================================================================================================================================*/
CREATE TABLE [Admin].[Stage_PartnerOwnerMapping]
(
   -- Columns 
   [GrandOwnerId]           INT,
   [PartnerOwnerId]         INT,

);
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds the data for Owner and corresponding partner mapping data.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Stage_PartnerOwnerMapping',
    @level2type = NULL,
    @level2name = NULL
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Stage_PartnerOwnerMapping',
    @level2type = N'COLUMN',
    @level2name = N'GrandOwnerId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The description of the element',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'Stage_PartnerOwnerMapping',
    @level2type = N'COLUMN',
    @level2name = N'PartnerOwnerId'
GO