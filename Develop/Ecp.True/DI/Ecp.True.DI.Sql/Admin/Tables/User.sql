/*===========================================================================================================================================================
-- Author:          Microsoft
-- Created date:	Dec-12-2019
-- Updated date:	Mar-20-2020
--<Description>: This table holds information for Users registered in the system. </Description>
=============================================================================================================================================================*/
CREATE TABLE [Admin].[User]
(
	--Columns
	[UserId]			INT IDENTITY (1, 1)					NOT NULL,
	[Name]			VARCHAR(260)							NOT NULL,
	[Email]			VARCHAR(260)							NOT NULL,

	--Internal Common Columns
	[CreatedBy]						NVARCHAR (260)			NOT NULL,
	[CreatedDate]					DATETIME				NOT NULL    DEFAULT Admin.udf_GetTrueDate(),

	--Constraints
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC),

)



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The identifier of the user',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'User',
    @level2type = N'COLUMN',
    @level2name = N'UserId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The name of the user',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'User',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The email of the user ',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'User',
    @level2type = N'COLUMN',
    @level2name = N'Email'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The creator of the record, normally system (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'User',
    @level2type = N'COLUMN',
    @level2name = N'CreatedBy'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The datetime when the record is created (common column)',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'User',
    @level2type = N'COLUMN',
    @level2name = N'CreatedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table holds information for Users registered in the system.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TABLE',
    @level1name = N'User',
    @level2type = NULL,
    @level2name = NULL