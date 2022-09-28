/*
 Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Feb-14-2020
-- Description:     To migrate existing Owner Otros in tables
-- ==============================================================================================================================*/


IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='09a68a2b-340e-4347-b2df-597b561b7204' AND Status = 1)
	BEGIN
		BEGIN TRY

            DECLARE @id INT;
            IF (@ServerName = 'sq-aeu-ecp-qat')
            BEGIN
	            SET @id = 1001;
            END
            ELSE
            BEGIN
                SET @id = 1017;
            END

            UPDATE [Offchain].[Owner] SET [OwnerId]=124 where [OwnerId]=@id;
            UPDATE [Offchain].[Ownership] SET [OwnerId]=124 where [OwnerId]=@id;
            UPDATE [Admin].[OwnershipCalculation] SET [OwnerId]=124 where [OwnerId]=@id;
            UPDATE [Admin].[OwnershipCalculationResult] SET [OwnerId]=124 where [OwnerId]=@id;
            UPDATE [Admin].[OwnershipResult] SET [OwnerId]=124 where [OwnerId]=@id;
            UPDATE [Admin].[NodeConnectionProductOwner] SET [OwnerId]=124 where [OwnerId]=@id;
            UPDATE [Admin].[Event] SET [Owner1Id]=124 where [Owner1Id]=@id;
            UPDATE [Admin].[PendingTransaction] SET [OwnerId]=124 where [OwnerId]=@id;
            UPDATE [Admin].[SegmentOwnershipCalculation] SET [OwnerId]=124 where [OwnerId]=@id;
            UPDATE [Admin].[SystemOwnershipCalculation] SET [OwnerId]=124 where [OwnerId]=@id;
            UPDATE [Admin].[StorageLocationProductOwner] SET [OwnerId]=124 where [OwnerId]=@id;
            UPDATE [Admin].[Ticket] SET [OwnerId]=124 where [OwnerId]=@id;
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('09a68a2b-340e-4347-b2df-597b561b7204', 1, 'Post');
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('09a68a2b-340e-4347-b2df-597b561b7204', 0, 'Post');
		END CATCH
	END
END