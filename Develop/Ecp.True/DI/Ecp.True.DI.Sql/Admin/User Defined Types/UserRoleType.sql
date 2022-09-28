/*-- ==============================================================================================================================
-- Author:          Intergrupo  
-- Created Date:    Jun-16-2021
 <Description>:		This is used as a parameter for UserRol SP.  </Description>

-- ================================================================================================================================*/

CREATE TYPE [Admin].[UserRoleType] AS TABLE
(
	[UserId] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(250) NULL,
    [UserType] NVARCHAR(50) NOT NULL,
    [Email] NVARCHAR(250) NULL,
    [RoleId] INT NOT NULL
)

GO
