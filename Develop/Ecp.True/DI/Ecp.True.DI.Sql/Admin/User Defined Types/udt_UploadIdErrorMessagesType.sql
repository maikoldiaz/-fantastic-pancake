/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Sep-23-2019
-- Updated Date:	Mar-20-2020
 <Description>:		This is used as a parameter for UpdateUploadId SP.  </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[UploadIdErrorMessagesType] AS TABLE
(

	--Columns
	[TempId]						INT				  NOT NULL, 
	[FileRegistrationErrorId]		INT				  NULL,
	[FileRegistrationId]			INT				  NULL,
	[ErrorMessage]					NVARCHAR(MAX)	  NOT NULL,

	--Internal Common Columns
	[CreatedBy]					    NVARCHAR (260)	  NULL,
	[CreatedDate]					DATETIME		  NULL,
	[LastModifiedBy]				NVARCHAR (260)	  NULL,
	[LastModifiedDate]				DATETIME		  NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is used as a parameter for UpdateUploadId SP.',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'TYPE',
    @level1name = N'UploadIdErrorMessagesType',
    @level2type = NULL,
    @level2name = NULL