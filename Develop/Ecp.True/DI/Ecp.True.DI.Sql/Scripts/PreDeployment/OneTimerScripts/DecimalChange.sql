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
	IF NOT EXISTS (SELECT 'X' FROM [Admin].[ControlScript] where Id='AC11824F-5188-4A88-B4F9-B271FA41F793' AND Status = 1)
	BEGIN
		BEGIN TRY
			
				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'Node'
					AND COLUMN_NAME = 'AcceptableBalancePercentage')
				BEGIN
					UPDATE [Admin].[Node]
					SET [AcceptableBalancePercentage] = 100.00
					WHERE AcceptableBalancePercentage > 999.99
					ALTER TABLE [Admin].[Node] ALTER COLUMN [AcceptableBalancePercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'NodeConnectionProduct'
					AND COLUMN_NAME = 'UncertaintyPercentage')
				BEGIN
					UPDATE [Admin].[NodeConnectionProduct]
					SET [UncertaintyPercentage] = 100.00
					WHERE UncertaintyPercentage > 999.99
					ALTER TABLE [Admin].[NodeConnectionProduct] ALTER COLUMN [UncertaintyPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'NodeConnectionProductOwner'
					AND COLUMN_NAME = 'OwnershipPercentage')
				BEGIN
					UPDATE [Admin].[NodeConnectionProductOwner]
					SET [OwnershipPercentage] = 100.00
					WHERE OwnershipPercentage > 999.99
					ALTER TABLE [Admin].[NodeConnectionProductOwner] ALTER COLUMN [OwnershipPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OperationalInventory'
					AND COLUMN_NAME = 'PercentStandardUnCertainty')
				BEGIN
					UPDATE [Admin].[OperationalInventory]
					SET [PercentStandardUnCertainty] = 100.00
					WHERE PercentStandardUnCertainty > 999.99
					ALTER TABLE [Admin].[OperationalInventory] ALTER COLUMN [PercentStandardUnCertainty] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OperationalMovement'
					AND COLUMN_NAME = 'PercentStandardUnCertainty')
				BEGIN
					UPDATE [Admin].[OperationalMovement]
					SET [PercentStandardUnCertainty] = 100.00
					WHERE PercentStandardUnCertainty > 999.99
					ALTER TABLE [Admin].[OperationalMovement] ALTER COLUMN [PercentStandardUnCertainty] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OperationalMovementQuality'
					AND COLUMN_NAME = 'PercentStandardUnCertainty')
				BEGIN
					UPDATE [Admin].[OperationalMovementQuality]
					SET [PercentStandardUnCertainty] = 100.00
					WHERE PercentStandardUnCertainty > 999.99
					ALTER TABLE [Admin].[OperationalMovementQuality] ALTER COLUMN [PercentStandardUnCertainty] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'FinalInventoryPercentage')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [FinalInventoryPercentage] = 100.00
					WHERE FinalInventoryPercentage > 999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [FinalInventoryPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'IdentifiedLossesPercentage')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [IdentifiedLossesPercentage] = 100.00
					WHERE IdentifiedLossesPercentage > 999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [IdentifiedLossesPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'IdentifiedLossesUnbalancePercentage')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [IdentifiedLossesUnbalancePercentage] = 100.00
					WHERE IdentifiedLossesUnbalancePercentage > 999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [IdentifiedLossesUnbalancePercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'InitialInventoryPercentage')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [InitialInventoryPercentage] = 100.00
					WHERE InitialInventoryPercentage > 999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [InitialInventoryPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'InputPercentage')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [InputPercentage] = 100.00
					WHERE InputPercentage > 999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [InputPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'InterfacePercentage')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [InterfacePercentage] = 100.00
					WHERE InterfacePercentage > 999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [InterfacePercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'InterfaceUnbalancePercentage')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [InterfaceUnbalancePercentage] = 100.00
					WHERE InterfaceUnbalancePercentage > 999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [InterfaceUnbalancePercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'OutputPercentage')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [OutputPercentage] = 100.00
					WHERE OutputPercentage > 999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [OutputPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'TolerancePercentage')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [TolerancePercentage] = 100.00
					WHERE TolerancePercentage > 999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [TolerancePercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'ToleranceUnbalancePercentage')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [ToleranceUnbalancePercentage] = 100.00
					WHERE ToleranceUnbalancePercentage > 999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [ToleranceUnbalancePercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'UnbalancePercentage')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [UnbalancePercentage] = 100.00
					WHERE UnbalancePercentage > 999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [UnbalancePercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'UnidentifiedLossesPercentage')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [UnidentifiedLossesPercentage] = 100.00
					WHERE UnidentifiedLossesPercentage > 999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [UnidentifiedLossesPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculationResult'
					AND COLUMN_NAME = 'OwnershipPercentage')
				BEGIN
					UPDATE [Admin].[OwnershipCalculationResult]
					SET [OwnershipPercentage] = 100.00
					WHERE OwnershipPercentage > 999.99
					ALTER TABLE [Admin].[OwnershipCalculationResult] ALTER COLUMN [OwnershipPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipResult'
					AND COLUMN_NAME = 'OwnershipPercentage')
				BEGIN
					UPDATE [Admin].[OwnershipResult]
					SET [OwnershipPercentage] = 100.00
					WHERE OwnershipPercentage > 999.99
					ALTER TABLE [Admin].[OwnershipResult] ALTER COLUMN [OwnershipPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'FinalInventoryPercentage')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [FinalInventoryPercentage] = 100.00
					WHERE FinalInventoryPercentage > 999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [FinalInventoryPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'IdentifiedLossesPercentage')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [IdentifiedLossesPercentage] = 100.00
					WHERE IdentifiedLossesPercentage > 999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [IdentifiedLossesPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'InitialInventoryPercentage')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [InitialInventoryPercentage] = 100.00
					WHERE InitialInventoryPercentage > 999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [InitialInventoryPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'InputPercentage')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [InputPercentage] = 100.00
					WHERE InputPercentage > 999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [InputPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'InterfacePercentage')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [InterfacePercentage] = 100.00
					WHERE InterfacePercentage > 999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [InterfacePercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'OutputPercentage')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [OutputPercentage] = 100.00
					WHERE OutputPercentage > 999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [OutputPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'TolerancePercentage')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [TolerancePercentage] = 100.00
					WHERE TolerancePercentage > 999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [TolerancePercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'UnbalancePercentage')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [UnbalancePercentage] = 100.00
					WHERE UnbalancePercentage > 999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [UnbalancePercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'UnidentifiedLossesPercentage')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [UnidentifiedLossesPercentage] = 100.00
					WHERE UnidentifiedLossesPercentage > 999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [UnidentifiedLossesPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'StorageLocationProduct'
					AND COLUMN_NAME = 'UncertaintyPercentage')
				BEGIN
					UPDATE [Admin].[StorageLocationProduct]
					SET [UncertaintyPercentage] = 100.00
					WHERE UncertaintyPercentage > 999.99
					ALTER TABLE [Admin].[StorageLocationProduct] ALTER COLUMN [UncertaintyPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'StorageLocationProductOwner'
					AND COLUMN_NAME = 'OwnershipPercentage')
				BEGIN
					UPDATE [Admin].[StorageLocationProductOwner]
					SET [OwnershipPercentage] = 100.00
					WHERE OwnershipPercentage > 999.99
					ALTER TABLE [Admin].[StorageLocationProductOwner] ALTER COLUMN [OwnershipPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'FinalInventoryPercentage')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [FinalInventoryPercentage] = 100.00
					WHERE FinalInventoryPercentage > 999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [FinalInventoryPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'IdentifiedLossesPercentage')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [IdentifiedLossesPercentage] = 100.00
					WHERE IdentifiedLossesPercentage > 999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [IdentifiedLossesPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'InitialInventoryPercentage')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [InitialInventoryPercentage] = 100.00
					WHERE InitialInventoryPercentage > 999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [InitialInventoryPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'InputPercentage')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [InputPercentage] = 100.00
					WHERE InputPercentage > 999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [InputPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'InterfacePercentage')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [InterfacePercentage] = 100.00
					WHERE InterfacePercentage > 999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [InterfacePercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'OutputPercentage')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [OutputPercentage] = 100.00
					WHERE OutputPercentage > 999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [OutputPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'TolerancePercentage')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [TolerancePercentage] = 100.00
					WHERE TolerancePercentage > 999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [TolerancePercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'UnbalancePercentage')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [UnbalancePercentage] = 100.00
					WHERE UnbalancePercentage > 999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [UnbalancePercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'UnidentifiedLossesPercentage')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [UnidentifiedLossesPercentage] = 100.00
					WHERE UnidentifiedLossesPercentage > 999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [UnidentifiedLossesPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Analytics'
					AND TABLE_NAME = 'ModelEvaluation'
					AND COLUMN_NAME = 'MeanAbsoluteError')
				BEGIN
					UPDATE [Analytics].[ModelEvaluation]
					SET [MeanAbsoluteError] = 100.00
					WHERE MeanAbsoluteError > 999.99
					ALTER TABLE [Analytics].[ModelEvaluation] ALTER COLUMN [MeanAbsoluteError] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Analytics'
					AND TABLE_NAME = 'ModelEvaluation'
					AND COLUMN_NAME = 'OwnershipPercentage')
				BEGIN
					UPDATE [Analytics].[ModelEvaluation]
					SET [OwnershipPercentage] = 100.00
					WHERE OwnershipPercentage > 999.99
					ALTER TABLE [Analytics].[ModelEvaluation] ALTER COLUMN [OwnershipPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Analytics'
					AND TABLE_NAME = 'OwnershipPercentageValues'
					AND COLUMN_NAME = 'OwnershipPercentage')
				BEGIN
					UPDATE [Analytics].[OwnershipPercentageValues]
					SET [OwnershipPercentage] = 100.00
					WHERE OwnershipPercentage > 999.99
					ALTER TABLE [Analytics].[OwnershipPercentageValues] ALTER COLUMN [OwnershipPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Inventory'
					AND COLUMN_NAME = 'UncertaintyPercentage')
				BEGIN
					UPDATE [Offchain].[Inventory]
					SET [UncertaintyPercentage] = 100.00
					WHERE UncertaintyPercentage > 999.99
					ALTER TABLE [Offchain].[Inventory] ALTER COLUMN [UncertaintyPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'InventoryProduct'
					AND COLUMN_NAME = 'UncertaintyPercentage')
				BEGIN
					UPDATE [Offchain].[InventoryProduct]
					SET [UncertaintyPercentage] = 100.00
					WHERE UncertaintyPercentage > 999.99
					ALTER TABLE [Offchain].[InventoryProduct] ALTER COLUMN [UncertaintyPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Movement'
					AND COLUMN_NAME = 'UncertaintyPercentage')
				BEGIN
					UPDATE [Offchain].[Movement]
					SET [UncertaintyPercentage] = 100.00
					WHERE UncertaintyPercentage > 999.99
					ALTER TABLE [Offchain].[Movement] ALTER COLUMN [UncertaintyPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Ownership'
					AND COLUMN_NAME = 'OwnershipPercentage')
				BEGIN
					UPDATE [Offchain].[Ownership]
					SET [OwnershipPercentage] = 100.00
					WHERE OwnershipPercentage > 999.99
					ALTER TABLE [Offchain].[Ownership] ALTER COLUMN [OwnershipPercentage] DECIMAL(5, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'Contract'
					AND COLUMN_NAME = 'Volume')
				BEGIN
					UPDATE [Admin].[Contract]
					SET [Volume] = 9999999999999999.99
					WHERE Volume > 9999999999999999.99
					ALTER TABLE [Admin].[Contract] ALTER COLUMN [Volume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'Event'
					AND COLUMN_NAME = 'Volume')
				BEGIN
					UPDATE [Admin].[Event]
					SET [Volume] = 9999999999999999.99
					WHERE Volume > 9999999999999999.99
					ALTER TABLE [Admin].[Event] ALTER COLUMN [Volume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'Node'
					AND COLUMN_NAME = 'ControlLimit')
				BEGIN
					UPDATE [Admin].[Node]
					SET [ControlLimit] = 9999999999999999.99
					WHERE ControlLimit > 9999999999999999.99
					ALTER TABLE [Admin].[Node] ALTER COLUMN [ControlLimit] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'NodeConnection'
					AND COLUMN_NAME = 'ControlLimit')
				BEGIN
					UPDATE [Admin].[NodeConnection]
					SET [ControlLimit] = 9999999999999999.99
					WHERE ControlLimit > 9999999999999999.99
					ALTER TABLE [Admin].[NodeConnection] ALTER COLUMN [ControlLimit] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'Operational'
					AND COLUMN_NAME = 'FinalInventory')
				BEGIN
					UPDATE [Admin].[Operational]
					SET [FinalInventory] = 9999999999999999.99
					WHERE FinalInventory > 9999999999999999.99
					ALTER TABLE [Admin].[Operational] ALTER COLUMN [FinalInventory] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'Operational'
					AND COLUMN_NAME = 'IdentifiedLosses')
				BEGIN
					UPDATE [Admin].[Operational]
					SET [IdentifiedLosses] = 9999999999999999.99
					WHERE IdentifiedLosses > 9999999999999999.99
					ALTER TABLE [Admin].[Operational] ALTER COLUMN [IdentifiedLosses] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'Operational'
					AND COLUMN_NAME = 'Inputs')
				BEGIN
					UPDATE [Admin].[Operational]
					SET [Inputs] = 9999999999999999.99
					WHERE Inputs > 9999999999999999.99
					ALTER TABLE [Admin].[Operational] ALTER COLUMN [Inputs] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'Operational'
					AND COLUMN_NAME = 'IntialInventory')
				BEGIN
					UPDATE [Admin].[Operational]
					SET [IntialInventory] = 9999999999999999.99
					WHERE IntialInventory > 9999999999999999.99
					ALTER TABLE [Admin].[Operational] ALTER COLUMN [IntialInventory] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'Operational'
					AND COLUMN_NAME = 'Outputs')
				BEGIN
					UPDATE [Admin].[Operational]
					SET [Outputs] = 9999999999999999.99
					WHERE Outputs > 9999999999999999.99
					ALTER TABLE [Admin].[Operational] ALTER COLUMN [Outputs] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'Operational'
					AND COLUMN_NAME = 'UnBalance')
				BEGIN
					UPDATE [Admin].[Operational]
					SET [UnBalance] = 9999999999999999.99
					WHERE UnBalance > 9999999999999999.99
					ALTER TABLE [Admin].[Operational] ALTER COLUMN [UnBalance] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OperationalInventory'
					AND COLUMN_NAME = 'NetStandardVolume')
				BEGIN
					UPDATE [Admin].[OperationalInventory]
					SET [NetStandardVolume] = 9999999999999999.99
					WHERE NetStandardVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OperationalInventory] ALTER COLUMN [NetStandardVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OperationalInventoryQuality'
					AND COLUMN_NAME = 'NetStandardVolume')
				BEGIN
					UPDATE [Admin].[OperationalInventoryQuality]
					SET [NetStandardVolume] = 9999999999999999.99
					WHERE NetStandardVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OperationalInventoryQuality] ALTER COLUMN [NetStandardVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OperationalInventoryQuality'
					AND COLUMN_NAME = 'PercentStandardUnCertainty')
				BEGIN
					UPDATE [Admin].[OperationalInventoryQuality]
					SET [PercentStandardUnCertainty] = 9999999999999999.99
					WHERE PercentStandardUnCertainty > 9999999999999999.99
					ALTER TABLE [Admin].[OperationalInventoryQuality] ALTER COLUMN [PercentStandardUnCertainty] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OperationalMovement'
					AND COLUMN_NAME = 'GrossStandardVolume')
				BEGIN
					UPDATE [Admin].[OperationalMovement]
					SET [GrossStandardVolume] = 9999999999999999.99
					WHERE GrossStandardVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OperationalMovement] ALTER COLUMN [GrossStandardVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OperationalMovement'
					AND COLUMN_NAME = 'NetStandardVolume')
				BEGIN
					UPDATE [Admin].[OperationalMovement]
					SET [NetStandardVolume] = 9999999999999999.99
					WHERE NetStandardVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OperationalMovement] ALTER COLUMN [NetStandardVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OperationalMovement'
					AND COLUMN_NAME = 'Uncertainty')
				BEGIN
					UPDATE [Admin].[OperationalMovement]
					SET [Uncertainty] = 9999999999999999.99
					WHERE Uncertainty > 9999999999999999.99
					ALTER TABLE [Admin].[OperationalMovement] ALTER COLUMN [Uncertainty] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OperationalMovementQuality'
					AND COLUMN_NAME = 'GrossStandardVolume')
				BEGIN
					UPDATE [Admin].[OperationalMovementQuality]
					SET [GrossStandardVolume] = 9999999999999999.99
					WHERE GrossStandardVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OperationalMovementQuality] ALTER COLUMN [GrossStandardVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OperationalMovementQuality'
					AND COLUMN_NAME = 'NetStandardVolume')
				BEGIN
					UPDATE [Admin].[OperationalMovementQuality]
					SET [NetStandardVolume] = 9999999999999999.99
					WHERE NetStandardVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OperationalMovementQuality] ALTER COLUMN [NetStandardVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OperationalMovementQuality'
					AND COLUMN_NAME = 'Uncertainty')
				BEGIN
					UPDATE [Admin].[OperationalMovementQuality]
					SET [Uncertainty] = 9999999999999999.99
					WHERE Uncertainty > 9999999999999999.99
					ALTER TABLE [Admin].[OperationalMovementQuality] ALTER COLUMN [Uncertainty] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'FinalInventoryVolume')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [FinalInventoryVolume] = 9999999999999999.99
					WHERE FinalInventoryVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [FinalInventoryVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'IdentifiedLossesUnbalanceVolume')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [IdentifiedLossesUnbalanceVolume] = 9999999999999999.99
					WHERE IdentifiedLossesUnbalanceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [IdentifiedLossesUnbalanceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'IdentifiedLossesVolume')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [IdentifiedLossesVolume] = 9999999999999999.99
					WHERE IdentifiedLossesVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [IdentifiedLossesVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'InitialInventoryVolume')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [InitialInventoryVolume] = 9999999999999999.99
					WHERE InitialInventoryVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [InitialInventoryVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'InputVolume')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [InputVolume] = 9999999999999999.99
					WHERE InputVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [InputVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'InterfaceUnbalanceVolume')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [InterfaceUnbalanceVolume] = 9999999999999999.99
					WHERE InterfaceUnbalanceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [InterfaceUnbalanceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'InterfaceVolume')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [InterfaceVolume] = 9999999999999999.99
					WHERE InterfaceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [InterfaceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'OutputVolume')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [OutputVolume] = 9999999999999999.99
					WHERE OutputVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [OutputVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'ToleranceUnbalanceVolume')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [ToleranceUnbalanceVolume] = 9999999999999999.99
					WHERE ToleranceUnbalanceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [ToleranceUnbalanceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'ToleranceVolume')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [ToleranceVolume] = 9999999999999999.99
					WHERE ToleranceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [ToleranceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'UnbalanceVolume')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [UnbalanceVolume] = 9999999999999999.99
					WHERE UnbalanceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [UnbalanceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculation'
					AND COLUMN_NAME = 'UnidentifiedLossesVolume')
				BEGIN
					UPDATE [Admin].[OwnershipCalculation]
					SET [UnidentifiedLossesVolume] = 9999999999999999.99
					WHERE UnidentifiedLossesVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipCalculation] ALTER COLUMN [UnidentifiedLossesVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipCalculationResult'
					AND COLUMN_NAME = 'OwnershipVolume')
				BEGIN
					UPDATE [Admin].[OwnershipCalculationResult]
					SET [OwnershipVolume] = 9999999999999999.99
					WHERE OwnershipVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipCalculationResult] ALTER COLUMN [OwnershipVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipResult'
					AND COLUMN_NAME = 'FinalInventory')
				BEGIN
					UPDATE [Admin].[OwnershipResult]
					SET [FinalInventory] = 9999999999999999.99
					WHERE FinalInventory > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipResult] ALTER COLUMN [FinalInventory] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipResult'
					AND COLUMN_NAME = 'InitialInventory')
				BEGIN
					UPDATE [Admin].[OwnershipResult]
					SET [InitialInventory] = 9999999999999999.99
					WHERE InitialInventory > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipResult] ALTER COLUMN [InitialInventory] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipResult'
					AND COLUMN_NAME = 'Input')
				BEGIN
					UPDATE [Admin].[OwnershipResult]
					SET [Input] = 9999999999999999.99
					WHERE Input > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipResult] ALTER COLUMN [Input] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipResult'
					AND COLUMN_NAME = 'Output')
				BEGIN
					UPDATE [Admin].[OwnershipResult]
					SET [Output] = 9999999999999999.99
					WHERE Output > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipResult] ALTER COLUMN [Output] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'OwnershipResult'
					AND COLUMN_NAME = 'OwnershipVolume')
				BEGIN
					UPDATE [Admin].[OwnershipResult]
					SET [OwnershipVolume] = 9999999999999999.99
					WHERE OwnershipVolume > 9999999999999999.99
					ALTER TABLE [Admin].[OwnershipResult] ALTER COLUMN [OwnershipVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'FinalInventoryVolume')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [FinalInventoryVolume] = 9999999999999999.99
					WHERE FinalInventoryVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [FinalInventoryVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'IdentifiedLossesVolume')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [IdentifiedLossesVolume] = 9999999999999999.99
					WHERE IdentifiedLossesVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [IdentifiedLossesVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'InitialInventoryVolume')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [InitialInventoryVolume] = 9999999999999999.99
					WHERE InitialInventoryVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [InitialInventoryVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'InputVolume')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [InputVolume] = 9999999999999999.99
					WHERE InputVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [InputVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'InterfaceVolume')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [InterfaceVolume] = 9999999999999999.99
					WHERE InterfaceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [InterfaceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'OutputVolume')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [OutputVolume] = 9999999999999999.99
					WHERE OutputVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [OutputVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'ToleranceVolume')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [ToleranceVolume] = 9999999999999999.99
					WHERE ToleranceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [ToleranceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'UnbalanceVolume')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [UnbalanceVolume] = 9999999999999999.99
					WHERE UnbalanceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [UnbalanceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentOwnershipCalculation'
					AND COLUMN_NAME = 'UnidentifiedLossesVolume')
				BEGIN
					UPDATE [Admin].[SegmentOwnershipCalculation]
					SET [UnidentifiedLossesVolume] = 9999999999999999.99
					WHERE UnidentifiedLossesVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentOwnershipCalculation] ALTER COLUMN [UnidentifiedLossesVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentUnbalance'
					AND COLUMN_NAME = 'FinalInventoryVolume')
				BEGIN
					UPDATE [Admin].[SegmentUnbalance]
					SET [FinalInventoryVolume] = 9999999999999999.99
					WHERE FinalInventoryVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentUnbalance] ALTER COLUMN [FinalInventoryVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentUnbalance'
					AND COLUMN_NAME = 'IdentifiedLossesVolume')
				BEGIN
					UPDATE [Admin].[SegmentUnbalance]
					SET [IdentifiedLossesVolume] = 9999999999999999.99
					WHERE IdentifiedLossesVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentUnbalance] ALTER COLUMN [IdentifiedLossesVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentUnbalance'
					AND COLUMN_NAME = 'InitialInventoryVolume')
				BEGIN
					UPDATE [Admin].[SegmentUnbalance]
					SET [InitialInventoryVolume] = 9999999999999999.99
					WHERE InitialInventoryVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentUnbalance] ALTER COLUMN [InitialInventoryVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentUnbalance'
					AND COLUMN_NAME = 'InputVolume')
				BEGIN
					UPDATE [Admin].[SegmentUnbalance]
					SET [InputVolume] = 9999999999999999.99
					WHERE InputVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentUnbalance] ALTER COLUMN [InputVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentUnbalance'
					AND COLUMN_NAME = 'InterfaceVolume')
				BEGIN
					UPDATE [Admin].[SegmentUnbalance]
					SET [InterfaceVolume] = 9999999999999999.99
					WHERE InterfaceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentUnbalance] ALTER COLUMN [InterfaceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentUnbalance'
					AND COLUMN_NAME = 'OutputVolume')
				BEGIN
					UPDATE [Admin].[SegmentUnbalance]
					SET [OutputVolume] = 9999999999999999.99
					WHERE OutputVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentUnbalance] ALTER COLUMN [OutputVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentUnbalance'
					AND COLUMN_NAME = 'ToleranceVolume')
				BEGIN
					UPDATE [Admin].[SegmentUnbalance]
					SET [ToleranceVolume] = 9999999999999999.99
					WHERE ToleranceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentUnbalance] ALTER COLUMN [ToleranceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentUnbalance'
					AND COLUMN_NAME = 'UnbalanceVolume')
				BEGIN
					UPDATE [Admin].[SegmentUnbalance]
					SET [UnbalanceVolume] = 9999999999999999.99
					WHERE UnbalanceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentUnbalance] ALTER COLUMN [UnbalanceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SegmentUnbalance'
					AND COLUMN_NAME = 'UnidentifiedLossesVolume')
				BEGIN
					UPDATE [Admin].[SegmentUnbalance]
					SET [UnidentifiedLossesVolume] = 9999999999999999.99
					WHERE UnidentifiedLossesVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SegmentUnbalance] ALTER COLUMN [UnidentifiedLossesVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'FinalInventoryVolume')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [FinalInventoryVolume] = 9999999999999999.99
					WHERE FinalInventoryVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [FinalInventoryVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'IdentifiedLossesVolume')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [IdentifiedLossesVolume] = 9999999999999999.99
					WHERE IdentifiedLossesVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [IdentifiedLossesVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'InitialInventoryVolume')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [InitialInventoryVolume] = 9999999999999999.99
					WHERE InitialInventoryVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [InitialInventoryVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'InputVolume')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [InputVolume] = 9999999999999999.99
					WHERE InputVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [InputVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'InterfaceVolume')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [InterfaceVolume] = 9999999999999999.99
					WHERE InterfaceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [InterfaceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'OutputVolume')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [OutputVolume] = 9999999999999999.99
					WHERE OutputVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [OutputVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'ToleranceVolume')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [ToleranceVolume] = 9999999999999999.99
					WHERE ToleranceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [ToleranceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'UnbalanceVolume')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [UnbalanceVolume] = 9999999999999999.99
					WHERE UnbalanceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [UnbalanceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemOwnershipCalculation'
					AND COLUMN_NAME = 'UnidentifiedLossesVolume')
				BEGIN
					UPDATE [Admin].[SystemOwnershipCalculation]
					SET [UnidentifiedLossesVolume] = 9999999999999999.99
					WHERE UnidentifiedLossesVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemOwnershipCalculation] ALTER COLUMN [UnidentifiedLossesVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemUnbalance'
					AND COLUMN_NAME = 'FinalInventoryVolume')
				BEGIN
					UPDATE [Admin].[SystemUnbalance]
					SET [FinalInventoryVolume] = 9999999999999999.99
					WHERE FinalInventoryVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemUnbalance] ALTER COLUMN [FinalInventoryVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemUnbalance'
					AND COLUMN_NAME = 'IdentifiedLossesVolume')
				BEGIN
					UPDATE [Admin].[SystemUnbalance]
					SET [IdentifiedLossesVolume] = 9999999999999999.99
					WHERE IdentifiedLossesVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemUnbalance] ALTER COLUMN [IdentifiedLossesVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemUnbalance'
					AND COLUMN_NAME = 'InitialInventoryVolume')
				BEGIN
					UPDATE [Admin].[SystemUnbalance]
					SET [InitialInventoryVolume] = 9999999999999999.99
					WHERE InitialInventoryVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemUnbalance] ALTER COLUMN [InitialInventoryVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemUnbalance'
					AND COLUMN_NAME = 'InputVolume')
				BEGIN
					UPDATE [Admin].[SystemUnbalance]
					SET [InputVolume] = 9999999999999999.99
					WHERE InputVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemUnbalance] ALTER COLUMN [InputVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemUnbalance'
					AND COLUMN_NAME = 'InterfaceVolume')
				BEGIN
					UPDATE [Admin].[SystemUnbalance]
					SET [InterfaceVolume] = 9999999999999999.99
					WHERE InterfaceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemUnbalance] ALTER COLUMN [InterfaceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemUnbalance'
					AND COLUMN_NAME = 'OutputVolume')
				BEGIN
					UPDATE [Admin].[SystemUnbalance]
					SET [OutputVolume] = 9999999999999999.99
					WHERE OutputVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemUnbalance] ALTER COLUMN [OutputVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemUnbalance'
					AND COLUMN_NAME = 'ToleranceVolume')
				BEGIN
					UPDATE [Admin].[SystemUnbalance]
					SET [ToleranceVolume] = 9999999999999999.99
					WHERE ToleranceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemUnbalance] ALTER COLUMN [ToleranceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemUnbalance'
					AND COLUMN_NAME = 'UnbalanceVolume')
				BEGIN
					UPDATE [Admin].[SystemUnbalance]
					SET [UnbalanceVolume] = 9999999999999999.99
					WHERE UnbalanceVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemUnbalance] ALTER COLUMN [UnbalanceVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'SystemUnbalance'
					AND COLUMN_NAME = 'UnidentifiedLossesVolume')
				BEGIN
					UPDATE [Admin].[SystemUnbalance]
					SET [UnidentifiedLossesVolume] = 9999999999999999.99
					WHERE UnidentifiedLossesVolume > 9999999999999999.99
					ALTER TABLE [Admin].[SystemUnbalance] ALTER COLUMN [UnidentifiedLossesVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Unbalance'
					AND COLUMN_NAME = 'FinalInvnetory')
				BEGIN
					UPDATE [Offchain].[Unbalance]
					SET [FinalInvnetory] = 9999999999999999.99
					WHERE FinalInvnetory > 9999999999999999.99
					ALTER TABLE [Offchain].[Unbalance] ALTER COLUMN [FinalInvnetory] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Unbalance'
					AND COLUMN_NAME = 'IdentifiedLosses')
				BEGIN
					UPDATE [Offchain].[Unbalance]
					SET [IdentifiedLosses] = 9999999999999999.99
					WHERE IdentifiedLosses > 9999999999999999.99
					ALTER TABLE [Offchain].[Unbalance] ALTER COLUMN [IdentifiedLosses] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Unbalance'
					AND COLUMN_NAME = 'InitialInventory')
				BEGIN
					UPDATE [Offchain].[Unbalance]
					SET [InitialInventory] = 9999999999999999.99
					WHERE InitialInventory > 9999999999999999.99
					ALTER TABLE [Offchain].[Unbalance] ALTER COLUMN [InitialInventory] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Unbalance'
					AND COLUMN_NAME = 'Inputs')
				BEGIN
					UPDATE [Offchain].[Unbalance]
					SET [Inputs] = 9999999999999999.99
					WHERE Inputs > 9999999999999999.99
					ALTER TABLE [Offchain].[Unbalance] ALTER COLUMN [Inputs] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Unbalance'
					AND COLUMN_NAME = 'Interface')
				BEGIN
					UPDATE [Offchain].[Unbalance]
					SET [Interface] = 9999999999999999.99
					WHERE Interface > 9999999999999999.99
					ALTER TABLE [Offchain].[Unbalance] ALTER COLUMN [Interface] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Unbalance'
					AND COLUMN_NAME = 'InterfaceUnbalance')
				BEGIN
					UPDATE [Offchain].[Unbalance]
					SET [InterfaceUnbalance] = 9999999999999999.99
					WHERE InterfaceUnbalance > 9999999999999999.99
					ALTER TABLE [Offchain].[Unbalance] ALTER COLUMN [InterfaceUnbalance] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Unbalance'
					AND COLUMN_NAME = 'Outputs')
				BEGIN
					UPDATE [Offchain].[Unbalance]
					SET [Outputs] = 9999999999999999.99
					WHERE Outputs > 9999999999999999.99
					ALTER TABLE [Offchain].[Unbalance] ALTER COLUMN [Outputs] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Unbalance'
					AND COLUMN_NAME = 'Tolerance')
				BEGIN
					UPDATE [Offchain].[Unbalance]
					SET [Tolerance] = 9999999999999999.99
					WHERE Tolerance > 9999999999999999.99
					ALTER TABLE [Offchain].[Unbalance] ALTER COLUMN [Tolerance] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Unbalance'
					AND COLUMN_NAME = 'ToleranceUnbalance')
				BEGIN
					UPDATE [Offchain].[Unbalance]
					SET [ToleranceUnbalance] = 9999999999999999.99
					WHERE ToleranceUnbalance > 9999999999999999.99
					ALTER TABLE [Offchain].[Unbalance] ALTER COLUMN [ToleranceUnbalance] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Unbalance'
					AND COLUMN_NAME = 'Unbalance')
				BEGIN
					UPDATE [Offchain].[Unbalance]
					SET [Unbalance] = 9999999999999999.99
					WHERE Unbalance > 9999999999999999.99
					ALTER TABLE [Offchain].[Unbalance] ALTER COLUMN [Unbalance] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Unbalance'
					AND COLUMN_NAME = 'UnidentifiedLosses')
				BEGIN
					UPDATE [Offchain].[Unbalance]
					SET [UnidentifiedLosses] = 9999999999999999.99
					WHERE UnidentifiedLosses > 9999999999999999.99
					ALTER TABLE [Offchain].[Unbalance] ALTER COLUMN [UnidentifiedLosses] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Unbalance'
					AND COLUMN_NAME = 'UnidentifiedLossesUnbalance')
				BEGIN
					UPDATE [Offchain].[Unbalance]
					SET [UnidentifiedLossesUnbalance] = 9999999999999999.99
					WHERE UnidentifiedLossesUnbalance > 9999999999999999.99
					ALTER TABLE [Offchain].[Unbalance] ALTER COLUMN [UnidentifiedLossesUnbalance] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'UnbalanceComment'
					AND COLUMN_NAME = 'ControlLimit')
				BEGIN
					UPDATE [Admin].[UnbalanceComment]
					SET [ControlLimit] = 9999999999999999.99
					WHERE ControlLimit > 9999999999999999.99
					ALTER TABLE [Admin].[UnbalanceComment] ALTER COLUMN [ControlLimit] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'UnbalanceComment'
					AND COLUMN_NAME = 'Unbalance')
				BEGIN
					UPDATE [Admin].[UnbalanceComment]
					SET [Unbalance] = 9999999999999999.99
					WHERE Unbalance > 9999999999999999.99
					ALTER TABLE [Admin].[UnbalanceComment] ALTER COLUMN [Unbalance] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Admin'
					AND TABLE_NAME = 'UnbalanceComment'
					AND COLUMN_NAME = 'UnbalancePercentage')
				BEGIN
					UPDATE [Admin].[UnbalanceComment]
					SET [UnbalancePercentage] = 9999999999999999.99
					WHERE UnbalancePercentage > 9999999999999999.99
					ALTER TABLE [Admin].[UnbalanceComment] ALTER COLUMN [UnbalancePercentage] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Analytics'
					AND TABLE_NAME = 'OperativeMovements'
					AND COLUMN_NAME = 'NetStandardVolume')
				BEGIN
					UPDATE [Analytics].[OperativeMovements]
					SET [NetStandardVolume] = 9999999999999999.99
					WHERE NetStandardVolume > 9999999999999999.99
					ALTER TABLE [Analytics].[OperativeMovements] ALTER COLUMN [NetStandardVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Analytics'
					AND TABLE_NAME = 'OperativeMovementsWithOwnership'
					AND COLUMN_NAME = 'OwnershipVolume')
				BEGIN
					UPDATE [Analytics].[OperativeMovementsWithOwnership]
					SET [OwnershipVolume] = 9999999999999999.99
					WHERE OwnershipVolume > 9999999999999999.99
					ALTER TABLE [Analytics].[OperativeMovementsWithOwnership] ALTER COLUMN [OwnershipVolume] DECIMAL(18, 2)
				END


				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'InventoryProduct'
					AND COLUMN_NAME = 'ProductVolume')
				BEGIN
					UPDATE [Offchain].[InventoryProduct]
					SET [ProductVolume] = 9999999999999999.99
					WHERE ProductVolume > 9999999999999999.99
					ALTER TABLE [Offchain].[InventoryProduct] ALTER COLUMN [ProductVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Movement'
					AND COLUMN_NAME = 'GrossStandardVolume')
				BEGIN
					UPDATE [Offchain].[Movement]
					SET [GrossStandardVolume] = 9999999999999999.99
					WHERE GrossStandardVolume > 9999999999999999.99
					ALTER TABLE [Offchain].[Movement] ALTER COLUMN [GrossStandardVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Movement'
					AND COLUMN_NAME = 'NetStandardVolume')
				BEGIN
					UPDATE [Offchain].[Movement]
					SET [NetStandardVolume] = 9999999999999999.99
					WHERE NetStandardVolume > 9999999999999999.99
					ALTER TABLE [Offchain].[Movement] ALTER COLUMN [NetStandardVolume] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Owner'
					AND COLUMN_NAME = 'OwnershipValue')
				BEGIN
					UPDATE [Offchain].[Owner]
					SET [OwnershipValue] = 9999999999999999.99
					WHERE OwnershipValue > 9999999999999999.99
					ALTER TABLE [Offchain].[Owner] ALTER COLUMN [OwnershipValue] DECIMAL(18, 2)
				END

				IF EXISTS (SELECT
						1
					FROM INFORMATION_SCHEMA.COLUMNS
					WHERE TABLE_SCHEMA = 'Offchain'
					AND TABLE_NAME = 'Ownership'
					AND COLUMN_NAME = 'OwnershipVolume')
				BEGIN
					UPDATE [Offchain].[Ownership]
					SET [OwnershipVolume] = 9999999999999999.99
					WHERE OwnershipVolume > 9999999999999999.99
					ALTER TABLE [Offchain].[Ownership] ALTER COLUMN [OwnershipVolume] DECIMAL(18, 2)
				END
 
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('AC11824F-5188-4A88-B4F9-B271FA41F793', 1);
		END TRY

		BEGIN CATCH
			INSERT [Admin].[ControlScript] ([Id], [Status]) VALUES ('AC11824F-5188-4A88-B4F9-B271FA41F793', 0);
		END CATCH
	END
END