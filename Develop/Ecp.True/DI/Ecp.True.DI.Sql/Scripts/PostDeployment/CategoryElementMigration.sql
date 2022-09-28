/* Note: The below script is used to generate all the tables which references the 
         ElementId of CategoryElement table as foreign key.
         And it also generates the UPDATE statement for all such tables.

         This should always be used whenever a new table is created which uses the CategoryElemnt as Fk.
         Or a new column is added in the existing table which uses CategoryElemnt as Fk.
 */
--------------------------------------------------------------------------------------


-- ############################## Do Not Remove these update scripts for HomologationdataMapping : START ###########################

UPDATE A SET A.SourceValue = CAST(B.NewElementId As NVARCHAR(100)), LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() 
FROM Admin.HomologationDataMapping A 
Inner JOIN Admin.tempCategoryElementMapping B 
On A.SourceValue = CAST(B.OldElementId As NVARCHAR(100))


UPDATE A SET A.DestinationValue = CAST(B.NewElementId As NVARCHAR(100)), LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() 
FROM Admin.HomologationDataMapping A 
Inner JOIN Admin.tempCategoryElementMapping B 
On A.DestinationValue = CAST(B.OldElementId As NVARCHAR(100))


UPDATE A SET A.[MeasurementUnit] = CAST(B.NewElementId As NVARCHAR(50)), LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() 
FROM [Admin].[Event] A 
Inner JOIN Admin.tempCategoryElementMapping B 
On A.[MeasurementUnit] = CAST(B.OldElementId As NVARCHAR(50))


UPDATE A SET A.[Units] = CAST(B.NewElementId As NVARCHAR(50)), LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() 
FROM [Admin].[PendingTransaction] A 
Inner JOIN Admin.tempCategoryElementMapping B 
On A.[Units] = CAST(B.OldElementId As NVARCHAR(50))


UPDATE A SET A.[MeasurementUnit] = CAST(B.NewElementId As NVARCHAR(50)), LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() 
FROM [Offchain].[InventoryProduct] A 
Inner JOIN Admin.tempCategoryElementMapping B 
On A.[MeasurementUnit] = CAST(B.OldElementId As NVARCHAR(50))


UPDATE A SET A.[ProductType] = CAST(B.NewElementId As NVARCHAR(50)), LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() 
FROM [Offchain].[InventoryProduct] A 
Inner JOIN Admin.tempCategoryElementMapping B 
On A.[ProductType] = CAST(B.OldElementId As NVARCHAR(50))


UPDATE A SET A.[MeasurementUnit] = CAST(B.NewElementId As NVARCHAR(50)), LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() 
FROM [Offchain].[Movement] A 
Inner JOIN Admin.tempCategoryElementMapping B 
On A.[MeasurementUnit] = CAST(B.OldElementId As NVARCHAR(50))


UPDATE A SET A.[MovementTypeId] = CAST(B.NewElementId As NVARCHAR(50)), LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() 
FROM [Offchain].[Movement] A 
Inner JOIN Admin.tempCategoryElementMapping B 
On A.[MovementTypeId] = CAST(B.OldElementId As NVARCHAR(50))


UPDATE A SET A.[DestinationProductTypeId] = CAST(B.NewElementId As NVARCHAR(50)), LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() 
FROM [Offchain].[MovementDestination] A 
Inner JOIN Admin.tempCategoryElementMapping B 
On A.[DestinationProductTypeId] = CAST(B.OldElementId As NVARCHAR(50))


UPDATE A SET A.[SourceProductTypeId] = CAST(B.NewElementId As NVARCHAR(50)), LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() 
FROM [Offchain].[MovementSource] A 
Inner JOIN Admin.tempCategoryElementMapping B 
On A.[SourceProductTypeId] = CAST(B.OldElementId As NVARCHAR(50))


UPDATE A SET A.[Units] = CAST(B.NewElementId As NVARCHAR(50))
FROM Admin.UnbalanceComment A 
Inner JOIN Admin.tempCategoryElementMapping B 
On A.[Units] = CAST(B.OldElementId As NVARCHAR(50))


UPDATE A SET A.[OwnerId] = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() 
FROM Admin.OwnershipResult A 
Inner JOIN Admin.tempCategoryElementMapping B 
On A.[OwnerId] = B.OldElementId


UPDATE A SET A.[OwnerId] = CAST(B.NewElementId As NVARCHAR(50)), LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() 
FROM Offchain.Owner A 
Inner JOIN Admin.tempCategoryElementMapping B 
On A.[OwnerId] = CAST(B.OldElementId As NVARCHAR(50))

-- ############################## Do Not Remove these update scripts for HomologationdataMapping : END ###########################

UPDATE A SET A.SourceMovementTypeId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Annulation A Inner JOIN Admin.tempCategoryElementMapping B On A.SourceMovementTypeId = B.OldElementId
UPDATE A SET A.AnnulationMovementTypeId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Annulation A Inner JOIN Admin.tempCategoryElementMapping B On A.AnnulationMovementTypeId = B.OldElementId
IF OBJECT_ID('Offchain.Attribute') IS NOT NULL
BEGIN
UPDATE A SET A.AttributeId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.Attribute A Inner JOIN Admin.tempCategoryElementMapping B On A.AttributeId = B.OldElementId where isnumeric(attributeid) = 1
UPDATE A SET A.ValueAttributeUnit = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.Attribute A Inner JOIN Admin.tempCategoryElementMapping B On A.ValueAttributeUnit = B.OldElementId where isnumeric(attributeid) = 1
END
ELSE
BEGIN
UPDATE A SET A.AttributeId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Attribute A Inner JOIN Admin.tempCategoryElementMapping B On A.AttributeId = B.OldElementId
UPDATE A SET A.ValueAttributeUnit = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Attribute A Inner JOIN Admin.tempCategoryElementMapping B On A.ValueAttributeUnit = B.OldElementId
END
UPDATE A SET A.SegmentId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.ConsolidatedInventoryProduct A Inner JOIN Admin.tempCategoryElementMapping B On A.SegmentId = B.OldElementId
UPDATE A SET A.SourceSystemId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.ConsolidatedInventoryProduct A Inner JOIN Admin.tempCategoryElementMapping B On A.SourceSystemId = B.OldElementId
UPDATE A SET A.SegmentId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.ConsolidatedMovement A Inner JOIN Admin.tempCategoryElementMapping B On A.SegmentId = B.OldElementId
UPDATE A SET A.SourceSystemId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.ConsolidatedMovement A Inner JOIN Admin.tempCategoryElementMapping B On A.SourceSystemId = B.OldElementId
UPDATE A SET A.OwnerId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.ConsolidatedOwner A Inner JOIN Admin.tempCategoryElementMapping B On A.OwnerId = B.OldElementId
UPDATE A SET A.Owner1Id = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Contract A Inner JOIN Admin.tempCategoryElementMapping B On A.Owner1Id = B.OldElementId
UPDATE A SET A.Owner2Id = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Contract A Inner JOIN Admin.tempCategoryElementMapping B On A.Owner2Id = B.OldElementId
UPDATE A SET A.MovementTypeId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Contract A Inner JOIN Admin.tempCategoryElementMapping B On A.MovementTypeId = B.OldElementId
UPDATE A SET A.MeasurementUnit = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Contract A Inner JOIN Admin.tempCategoryElementMapping B On A.MeasurementUnit = B.OldElementId
UPDATE A SET A.Owner1Id = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Event A Inner JOIN Admin.tempCategoryElementMapping B On A.Owner1Id = B.OldElementId
UPDATE A SET A.Owner2Id = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Event A Inner JOIN Admin.tempCategoryElementMapping B On A.Owner2Id = B.OldElementId
UPDATE A SET A.EventTypeId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Event A Inner JOIN Admin.tempCategoryElementMapping B On A.EventTypeId = B.OldElementId
UPDATE A SET A.SegmentId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.FileRegistration A Inner JOIN Admin.tempCategoryElementMapping B On A.SegmentId = B.OldElementId
UPDATE A SET A.Owner1Id = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.MovementContract A Inner JOIN Admin.tempCategoryElementMapping B On A.Owner1Id = B.OldElementId
UPDATE A SET A.Owner2Id = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.MovementContract A Inner JOIN Admin.tempCategoryElementMapping B On A.Owner2Id = B.OldElementId
UPDATE A SET A.MovementTypeId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.MovementContract A Inner JOIN Admin.tempCategoryElementMapping B On A.MovementTypeId = B.OldElementId
UPDATE A SET A.MeasurementUnit = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.MovementContract A Inner JOIN Admin.tempCategoryElementMapping B On A.MeasurementUnit = B.OldElementId
UPDATE A SET A.EventTypeId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.MovementEvent A Inner JOIN Admin.tempCategoryElementMapping B On A.EventTypeId = B.OldElementId
UPDATE A SET A.Owner1Id = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.MovementEvent A Inner JOIN Admin.tempCategoryElementMapping B On A.Owner1Id = B.OldElementId
UPDATE A SET A.Owner2Id = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.MovementEvent A Inner JOIN Admin.tempCategoryElementMapping B On A.Owner2Id = B.OldElementId
UPDATE A SET A.UnitId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Node A Inner JOIN Admin.tempCategoryElementMapping B On A.UnitId = B.OldElementId
UPDATE A SET A.OwnerId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.NodeConnectionProductOwner A Inner JOIN Admin.tempCategoryElementMapping B On A.OwnerId = B.OldElementId
UPDATE A SET A.StorageLocationTypeId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.NodeStorageLocation A Inner JOIN Admin.tempCategoryElementMapping B On A.StorageLocationTypeId = B.OldElementId
UPDATE A SET A.ElementId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.NodeTag A Inner JOIN Admin.tempCategoryElementMapping B On A.ElementId = B.OldElementId
UPDATE A SET A.OwnerId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.OwnershipCalculation A Inner JOIN Admin.tempCategoryElementMapping B On A.OwnerId = B.OldElementId
UPDATE A SET A.OwnerId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.OwnershipCalculationResult A Inner JOIN Admin.tempCategoryElementMapping B On A.OwnerId = B.OldElementId
UPDATE A SET A.ReasonId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.OwnershipNode A Inner JOIN Admin.tempCategoryElementMapping B On A.ReasonId = B.OldElementId
UPDATE A SET A.GrandOwnerId = B.NewElementId FROM Admin.PartnerOwnerMapping A Inner JOIN Admin.tempCategoryElementMapping B On A.GrandOwnerId = B.OldElementId
UPDATE A SET A.PartnerOwnerId = B.NewElementId FROM Admin.PartnerOwnerMapping A Inner JOIN Admin.tempCategoryElementMapping B On A.PartnerOwnerId = B.OldElementId
UPDATE A SET A.OwnerId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.PendingTransaction A Inner JOIN Admin.tempCategoryElementMapping B On A.OwnerId = B.OldElementId
UPDATE A SET A.TypeId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.PendingTransaction A Inner JOIN Admin.tempCategoryElementMapping B On A.TypeId = B.OldElementId
UPDATE A SET A.Units = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.PendingTransaction A Inner JOIN Admin.tempCategoryElementMapping B On A.Units = B.OldElementId
UPDATE A SET A.ElementId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.ReportExecution A Inner JOIN Admin.tempCategoryElementMapping B On A.ElementId = B.OldElementId
UPDATE A SET A.SegmentId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.SegmentOwnershipCalculation A Inner JOIN Admin.tempCategoryElementMapping B On A.SegmentId = B.OldElementId
UPDATE A SET A.OwnerId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.SegmentOwnershipCalculation A Inner JOIN Admin.tempCategoryElementMapping B On A.OwnerId = B.OldElementId
UPDATE A SET A.SegmentId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.SegmentUnbalance A Inner JOIN Admin.tempCategoryElementMapping B On A.SegmentId = B.OldElementId
UPDATE A SET A.OwnershipRuleId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.StorageLocationProduct A Inner JOIN Admin.tempCategoryElementMapping B On A.OwnershipRuleId = B.OldElementId
UPDATE A SET A.OwnerId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.StorageLocationProductOwner A Inner JOIN Admin.tempCategoryElementMapping B On A.OwnerId = B.OldElementId
UPDATE A SET A.OwnerId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.SystemOwnershipCalculation A Inner JOIN Admin.tempCategoryElementMapping B On A.OwnerId = B.OldElementId
UPDATE A SET A.SystemId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.SystemOwnershipCalculation A Inner JOIN Admin.tempCategoryElementMapping B On A.SystemId = B.OldElementId
UPDATE A SET A.SegmentId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.SystemOwnershipCalculation A Inner JOIN Admin.tempCategoryElementMapping B On A.SegmentId = B.OldElementId
UPDATE A SET A.SystemId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.SystemUnbalance A Inner JOIN Admin.tempCategoryElementMapping B On A.SystemId = B.OldElementId
UPDATE A SET A.SegmentId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.SystemUnbalance A Inner JOIN Admin.tempCategoryElementMapping B On A.SegmentId = B.OldElementId
UPDATE A SET A.CategoryElementId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Ticket A Inner JOIN Admin.tempCategoryElementMapping B On A.CategoryElementId = B.OldElementId
UPDATE A SET A.OwnerId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Ticket A Inner JOIN Admin.tempCategoryElementMapping B On A.OwnerId = B.OldElementId
UPDATE A SET A.DestinationMeasurementId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Transformation A Inner JOIN Admin.tempCategoryElementMapping B On A.DestinationMeasurementId = B.OldElementId
UPDATE A SET A.OriginMeasurementId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.Transformation A Inner JOIN Admin.tempCategoryElementMapping B On A.OriginMeasurementId = B.OldElementId
UPDATE A SET A.SegmentId = B.NewElementId FROM Admin.UnbalanceComment A Inner JOIN Admin.tempCategoryElementMapping B On A.SegmentId = B.OldElementId
UPDATE A SET A.ReasonId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.InventoryProduct A Inner JOIN Admin.tempCategoryElementMapping B On A.ReasonId = B.OldElementId
UPDATE A SET A.OperatorId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.InventoryProduct A Inner JOIN Admin.tempCategoryElementMapping B On A.OperatorId = B.OldElementId
UPDATE A SET A.SourceSystemId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.InventoryProduct A Inner JOIN Admin.tempCategoryElementMapping B On A.SourceSystemId = B.OldElementId
UPDATE A SET A.ProductType = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.InventoryProduct A Inner JOIN Admin.tempCategoryElementMapping B On A.ProductType = B.OldElementId
UPDATE A SET A.MeasurementUnit = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.InventoryProduct A Inner JOIN Admin.tempCategoryElementMapping B On A.MeasurementUnit = B.OldElementId
UPDATE A SET A.SystemId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.InventoryProduct A Inner JOIN Admin.tempCategoryElementMapping B On A.SystemId = B.OldElementId
UPDATE A SET A.SegmentId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.InventoryProduct A Inner JOIN Admin.tempCategoryElementMapping B On A.SegmentId = B.OldElementId
UPDATE A SET A.OperatorId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.Movement A Inner JOIN Admin.tempCategoryElementMapping B On A.OperatorId = B.OldElementId
UPDATE A SET A.ReasonId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.Movement A Inner JOIN Admin.tempCategoryElementMapping B On A.ReasonId = B.OldElementId
UPDATE A SET A.SegmentId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.Movement A Inner JOIN Admin.tempCategoryElementMapping B On A.SegmentId = B.OldElementId
UPDATE A SET A.SourceSystemId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.Movement A Inner JOIN Admin.tempCategoryElementMapping B On A.SourceSystemId = B.OldElementId
UPDATE A SET A.SystemId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.Movement A Inner JOIN Admin.tempCategoryElementMapping B On A.SystemId = B.OldElementId
UPDATE A SET A.MovementTypeId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.Movement A Inner JOIN Admin.tempCategoryElementMapping B On A.MovementTypeId = B.OldElementId
UPDATE A SET A.MeasurementUnit = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.Movement A Inner JOIN Admin.tempCategoryElementMapping B On A.MeasurementUnit = B.OldElementId
UPDATE A SET A.DestinationProductTypeId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.MovementDestination A Inner JOIN Admin.tempCategoryElementMapping B On A.DestinationProductTypeId = B.OldElementId
UPDATE A SET A.SourceProductTypeId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.MovementSource A Inner JOIN Admin.tempCategoryElementMapping B On A.SourceProductTypeId = B.OldElementId
UPDATE A SET A.OwnerId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.Owner A Inner JOIN Admin.tempCategoryElementMapping B On A.OwnerId = B.OldElementId
UPDATE A SET A.OwnerId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Offchain.Ownership A Inner JOIN Admin.tempCategoryElementMapping B On A.OwnerId = B.OldElementId

UPDATE A SET A.MovementTypeId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.NodeCostCenter A Inner JOIN Admin.tempCategoryElementMapping B On A.MovementTypeId = B.OldElementId
UPDATE A SET A.CostCenterId = B.NewElementId, LastModifiedBy = 'MigrationSript', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM Admin.NodeCostCenter A Inner JOIN Admin.tempCategoryElementMapping B On A.CostCenterId = B.OldElementId

Delete Admin.[CategoryElement] Where ElementId 
IN 
(Select OldElementId From Admin.tempCategoryElementMapping)

