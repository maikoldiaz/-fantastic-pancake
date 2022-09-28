/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF EXISTS (Select 'X' from sys.schemas Where name = 'Admin')
BEGIN
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Admin' AND  TABLE_NAME = 'Unbalance'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='9ce9afb9-545c-4e86-8aad-56d17c86178b' AND Status = 1)
		BEGIN
			BEGIN TRY
				SET IDENTITY_INSERT [Offchain].[Unbalance] ON
				INSERT INTO [Offchain].[Unbalance]
				(
					UnbalanceId,
					TicketId,
					NodeId,
					ProductId,
					InitialInventory,
					Inputs,
					Outputs,
					FinalInvnetory,
					IdentifiedLosses,
					Unbalance,
					Interface,
					Tolerance,
					UnidentifiedLosses,
					CalculationDate,
					InterfaceUnbalance,
					ToleranceUnbalance,
					UnidentifiedLossesUnbalance,
					StandardUncertainty,
					AverageUncertainty,
					AverageUncertaintyUnbalancePercentage,
					Warning,
					[Action],
					ControlTolerance,
					ToleranceIdentifiedLosses,
					ToleranceInputs,
					ToleranceOutputs,
					ToleranceInitialInventory,
					ToleranceFinalInventory,
					TransactionHash,
					BlockNumber,
					BlockchainStatus,
					RetryCount,
					CreatedBy,
					CreatedDate,
					LastModifiedBy,
					LastModifiedDate 
				)
				SELECT UnbalanceId,
					   TicketId,
					   NodeId,
					   ProductId,
					   InitialInventory,
					   Inputs,
					   Outputs,
					   FinalInvnetory,
					   IdentifiedLosses,
					   Unbalance,
					   Interface,
					   Tolerance,
					   UnidentifiedLosses,
					   CalculationDate,
					   InterfaceUnbalance,
					   ToleranceUnbalance,
					   UnidentifiedLossesUnbalance,
					   StandardUncertainty,
					   AverageUncertainty,
					   AverageUncertaintyUnbalancePercentage,
					   Warning,
					   [Action],
					   ControlTolerance,
					   ToleranceIdentifiedLosses,
					   ToleranceInputs,
					   ToleranceOutputs,
					   ToleranceInitialInventory,
					   ToleranceFinalInventory,
					   TransactionHash,
					   BlockNumber,
					   BlockchainStatus,
					   RetryCount,
					   CreatedBy,
					   CreatedDate,
					   LastModifiedBy,
					   LastModifiedDate 
				FROM [Admin].[Unbalance];
				SET IDENTITY_INSERT [Offchain].[Unbalance] OFF
				DROP TABLE [Admin].[Unbalance];
				INSERT [Admin].[ControlScript] ([Id], [DeploymentType], [Status]) VALUES ('9ce9afb9-545c-4e86-8aad-56d17c86178b', 'POST', 1);
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [DeploymentType], [Status]) VALUES ('9ce9afb9-545c-4e86-8aad-56d17c86178b', 'POST', 0);
			END CATCH
		END
	END
END

IF EXISTS (SELECT 'X' from sys.schemas Where name = 'Offchain')
BEGIN
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Offchain' AND  TABLE_NAME = 'Attribute'))
	BEGIN
		IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='a6b7581e-4bfe-4f2a-a565-0fc1f0a2041a' AND Status = 1)
		BEGIN
			BEGIN TRY
				SET IDENTITY_INSERT [Admin].[Attribute] ON
				INSERT INTO [Admin].[Attribute]
				(
					Id,
					AttributeId,
					AttributeValue,
					ValueAttributeUnit,
					AttributeDescription,
					InventoryProductId,
					MovementTransactionId,
					AttributeType,
					CreatedBy,
					CreatedDate,
					LastModifiedBy,
					LastModifiedDate
				)
				SELECT Id,
					   AttributeId,
					   AttributeValue,
					   ValueAttributeUnit,
					   AttributeDescription,
					   InventoryProductId,
					   MovementTransactionId,
					   AttributeType,
					   CreatedBy,
					   CreatedDate,
					   LastModifiedBy,
					   LastModifiedDate
				FROM [Offchain].[Attribute];
				SET IDENTITY_INSERT [Admin].[Attribute] OFF
				DROP TABLE [Offchain].[Attribute];
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('a6b7581e-4bfe-4f2a-a565-0fc1f0a2041a', 1, 'POST');
			END TRY

			BEGIN CATCH
				INSERT [Admin].[ControlScript] ([Id], [Status], [DeploymentType]) VALUES ('a6b7581e-4bfe-4f2a-a565-0fc1f0a2041a', 0, 'POST');
			END CATCH
		END
	END
END