// --------------------------------------------------------------------------------------------------------------------
// <copyright file = "SqlQueries.cs" company = "Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Entities
{
    using System.Configuration;

    public static class SqlQueries
    {
        public static readonly string UpdateNodeStatus = $"UPDATE [{DbCatalog}].[Admin].[OwnershipNode] SET OwnershipStatusId= (SELECT OwnershipNodeStatusTypeId FROM [{DbCatalog}].[Admin].[OwnershipNodeStatusType] WHERE Name= @status) where TicketId=@ticketId";
        public static readonly string GetActiveNodes = $"SELECT * FROM [{DbCatalog}].[Admin].[Node] WHERE IsActive = 1";
        public static readonly string GetActiveNodesCount = $"SELECT COUNT(1) FROM [{DbCatalog}].[Admin].[Node] WHERE IsActive = 1";
        public static readonly string GetNode = $"SELECT * FROM [{DbCatalog}].[Admin].[Node] WHERE NodeId = @nodeId";
        public static readonly string GetCategories = $"SELECT * FROM [{DbCatalog}].[Admin].[Category]";
        public static readonly string GetActiveCategoriesCount = $"SELECT COUNT(1) FROM [{DbCatalog}].[Admin].[Category] WHERE IsActive = 1";
        public static readonly string GetCategory = $"SELECT * FROM [{DbCatalog}].[Admin].[Category] WHERE CategoryId = @categoryId";
        public static readonly string GetCategoryElements = $"SELECT * FROM [{DbCatalog}].[Admin].[CategoryElement]";
        public static readonly string GetActiveCategoryElements = $"SELECT * FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE IsActive = 1 AND categoryId = 10";
        ////public static readonly string GetInActiveCategoryElements = $"SELECT * FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE IsActive = 0";
        public static readonly string GetActiveCategoryElementsCount = $"SELECT COUNT(1) FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE IsActive = 1";
        public static readonly string GetActiveCategoryElement = $"SELECT * FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE ElementId = @elementId";
        public static readonly string GetLastCategoryElement = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[CategoryElement] ORDER BY ElementId DESC";
        public static readonly string GetLastButOneCategoryElement = $"SELECT TOP 1 * From (SELECT TOP 2 * FROM [{DbCatalog}].[Admin].[CategoryElement] ORDER BY ElementId DESC) x ORDER BY ElementId";
        public static readonly string GetActiveCategory = $"SELECT * FROM [{DbCatalog}].[Admin].[Category] WHERE CategoryId = @categoryId";
        public static readonly string GetCategoryId = $"SELECT TOP 1 CategoryId FROM [{DbCatalog}].[Admin].[Category]";
        public static readonly string GetLastCategory = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Category] ORDER BY CategoryId DESC";
        public static readonly string GetLastButOneCategory = $"SELECT TOP 1 * From (SELECT TOP 2 * FROM [{DbCatalog}].[Admin].[Category] ORDER BY CategoryId DESC) x ORDER BY CategoryId";
        public static readonly string GetLastCategoryWithDescription = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Category] where NOT Description = '' OR NOT Description = NULL ORDER BY CreatedDate DESC";
        public static readonly string GetLastCategoryElementWithDescription = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[CategoryElement] where Description is not null ORDER BY CreatedDate DESC";
        public static readonly string GetNodes = $"SELECT * FROM [{DbCatalog}].[Admin].[Node] order by CreatedDate Asc";
        public static readonly string GetHomologations = $"SELECT * FROM [{DbCatalog}].[Admin].[Homologation]";
        public static readonly string GetHomologationGroups = $"SELECT * FROM [{DbCatalog}].[Admin].[HomologationGroup]";
        public static readonly string GetLastNode = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Node] order by CreatedDate DESC";
        public static readonly string GetLastStorageLocation = $"SELECT * FROM [{DbCatalog}].[Admin].[NodeStorageLocation] WHERE NodeId = @nodeId";
        public static readonly string GetLogisticCenters = $"SELECT * FROM [{DbCatalog}].[Admin].[LogisticCenter]";
        public static readonly string GetStorageLocations = $"SELECT * FROM [{DbCatalog}].[Admin].[StorageLocation]";
        public static readonly string GetNodeStorageLocationStatus = $"SELECT IsActive FROM [{DbCatalog}].[Admin].[NodeStorageLocation] WHERE NodeId = @nodeId";
        public static readonly string GetAuditLogDetails = $"SELECT TOP 1 * FROM [{DbCatalog}].[AUDIT].[AUDITLOG] WHERE NodeCode = @nodeId ORDER BY AuditLogId DESC";
        public static readonly string GetProductLocationStatus = $"SELECT IsActive FROM [{DbCatalog}].[Admin].[StorageLocationProduct] WHERE NodeStorageLocationId = @nodeStorageLocationId";
        public static readonly string GetLastStorageLocationProduct = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[StorageLocationProduct] ORDER BY StorageLocationProductId DESC";
        public static readonly string GetStorageLocationName = $"SELECT TOP 1 Name FROM [{DbCatalog}].Admin.NodeStorageLocation WHERE NodeId= @nodeId ORDER BY NodeStorageLocationId DESC";
        public static readonly string GetAuditLogDetailsForProduct = $"SELECT TOP 1 * FROM [{DbCatalog}].[AUDIT].[AUDITLOG] WHERE LogType = @newLogType AND Field = @field ORDER BY AuditLogId DESC";
        public static readonly string GetNewAuditStorageLocationName = $"SELECT TOP 1 * FROM [{DbCatalog}].[AUDIT].[AUDITLOG] WHERE NodeCode = @nodeId AND LogType = @newLogType AND Field = @field AND Entity = 'Admin.NodeStorageLocation' ORDER BY auditlogid DESC";
        public static readonly string GetStorageLocation = $"SELECT * FROM [{DbCatalog}].[Admin].[NodeStorageLocation] WHERE NodeId = @nodeId";
        public static readonly string GetOperationalBalance = $"Exec [Admin].[usp_CalculateUnbalance] @NodeId, @StartDate,@EndDate";
        public static readonly string GetInputData = $"Select m.MovementId, m.MovementTypeId, m.OperationalDate, ms.SourceNode, md.DestinationNode, ms.SourceProduct, md.DestinationProduct, m.NetStandardVolume, m.Scenario, mt.Name from [Admin].[Movement] m JOIN(Select msi.*, n.Name As SourceNode, slp.ProductName As SourceProduct from [Admin].[MovementSource] msi LEFT JOIN[Admin].[Node] n ON msi.SourceNodeId = n.NodeId LEFT JOIN [Admin].[view_StorageLocationProductWithProductName] slp ON msi.SourceProductId = slp.StorageLocationProductId) ms ON m.MovementTransactionId = ms.MovementTransactionId JOIN(Select mdi.*, n.Name As DestinationNode, slp.ProductName As DestinationProduct from [Admin].[MovementDestination] mdi LEFT JOIN[Admin].[Node] n ON mdi.DestinationNodeId = n.NodeId LEFT JOIN [Admin].[view_StorageLocationProductWithProductName] slp ON mdi.DestinationProductId = slp.StorageLocationProductId) md ON m.MovementTransactionId = md.MovementTransactionId LEFT JOIN[Admin].[MessageType] mt on mt.MessageTypeId = m.MessageTypeId WHERE   md.DestinationNodeId = @NodeId AND mt.MessageTypeId = 1 AND @StartDate <= m.OperationalDate AND m.OperationalDate<(select DATEADD(dd, DATEDIFF(dd, 0, @EndDate), 1));";
        public static readonly string GetOutputData = $"Select m.MovementId, m.MovementTypeId, m.OperationalDate, ms.SourceNode, md.DestinationNode, ms.SourceProduct, md.DestinationProduct, m.NetStandardVolume, m.Scenario, mt.Name from [Admin].[Movement] m JOIN(Select msi.*, n.Name As SourceNode, slp.ProductName As SourceProduct from [Admin].[MovementSource] msi LEFT JOIN[Admin].[Node] n ON msi.SourceNodeId = n.NodeId LEFT JOIN [Admin].[view_StorageLocationProductWithProductName] slp ON msi.SourceProductId = slp.StorageLocationProductId) ms ON m.MovementTransactionId = ms.MovementTransactionId JOIN(Select mdi.*, n.Name As DestinationNode, slp.ProductName As DestinationProduct from [Admin].[MovementDestination] mdi LEFT JOIN[Admin].[Node] n ON mdi.DestinationNodeId = n.NodeId LEFT JOIN [Admin].[view_StorageLocationProductWithProductName] slp ON mdi.DestinationProductId = slp.StorageLocationProductId) md ON m.MovementTransactionId = md.MovementTransactionId LEFT JOIN[Admin].[MessageType] mt on mt.MessageTypeId = m.MessageTypeId WHERE   ms.SourceNodeId = @NodeId AND mt.MessageTypeId = 1 AND @StartDate <= m.OperationalDate AND m.OperationalDate<(select DATEADD(dd, DATEDIFF(dd, 0, @EndDate), 1));";
        public static readonly string GetLosses = $"Select m.MovementId, m.MovementTypeId, m.OperationalDate, ms.SourceNode, NULL as DestinationNode, ms.SourceProduct, NULL as DestinationProduct , m.NetStandardVolume, m.Scenario, mt.Name from [Admin].[Movement] m JOIN(Select msi.*, n.Name As SourceNode, slp.ProductName As SourceProduct from [Admin].[MovementSource] msi LEFT JOIN[Admin].[Node] n ON msi.SourceNodeId = n.NodeId LEFT JOIN [Admin].[view_StorageLocationProductWithProductName] slp ON msi.SourceProductId = slp.StorageLocationProductId) ms ON m.MovementTransactionId = ms.MovementTransactionId LEFT JOIN[Admin].[MessageType] mt on mt.MessageTypeId = m.MessageTypeId WHERE   ms.SourceNodeId = @NodeId AND mt.MessageTypeId = 2 AND @StartDate <= m.OperationalDate AND m.OperationalDate<(select DATEADD(dd, DATEDIFF(dd, 0, @EndDate), 1));";
        public static readonly string GetHomologationGroupId = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[HomologationGroup] WHERE HomologationId=@homologationId";
        public static readonly string GetLastHomologation = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Homologation] ORDER BY HomologationId DESC";
        public static readonly string GetLastHomologationGroup = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[HomologationGroup] ORDER BY HomologationGroupId DESC";
        public static readonly string GetHomologationGroupName = $"SELECT Name FROM [{DbCatalog}].[Admin].[Category] WHERE CategoryId=@groupTypeId";
        public static readonly string GetHomologationTypeId = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[HomologationObjectType] ORDER BY CreatedDate DESC";
        public static readonly string GetHomologationGroupTypeId = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Category] ORDER BY CreatedDate DESC";
        public static readonly string GetGroupTypeId = $"SELECT CategoryId FROM [{DbCatalog}].[Admin].[Category] WHERE Name = @name";
        public static readonly string GetTrueId = $"SELECT SystemTypeId FROM [{DbCatalog}].[Admin].[SystemType] WHERE Name = 'TRUE'";
        public static readonly string GetSinoperId = $"SELECT SystemTypeId FROM [{DbCatalog}].[Admin].[SystemType] WHERE Name = 'SINOPER'";
        public static readonly string GetExcelId = $"SELECT SystemTypeId FROM [{DbCatalog}].[Admin].[SystemType] WHERE Name = 'EXCELO'";
        public static readonly string GetNodeId = $"SELECT TOP 1 NodeId FROM [{DbCatalog}].[Admin].[Node] ORDER BY NodeId DESC";
        public static readonly string GetStorageLocationId = $"SELECT TOP 1 NodeStorageLocationId FROM Admin.NodeStorageLocation ORDER BY NodeStorageLocationId DESC";
        public static readonly string GetProductId = $"SELECT TOP 1 ProductId FROM [{DbCatalog}].[Admin].[StorageLocationProduct] ORDER BY StorageLocationProductId DESC";
        public static readonly string GetCategoryElementId = $"SELECT TOP 1 ElementId FROM [{DbCatalog}].[Admin].[CategoryElement] ORDER BY ElementId DESC";
        public static readonly string GetHomologationGroupIdBySourceValue = $"SELECT HomologationGroupId FROM [{DbCatalog}].[Admin].[HomologationDataMapping] WHERE SourceValue = @value";
        public static readonly string GetHomologationGroupIdByDestinationValue = $"SELECT HomologationGroupId FROM [{DbCatalog}].[Admin].[HomologationDataMapping] WHERE DestinationValue = @value";
        public static readonly string GetAuditLogDetailsForHomologation = $"SELECT TOP 1 * FROM [{DbCatalog}].[AUDIT].[AUDITLOG] ORDER BY AuditLogId DESC";
        public static readonly string GetAuditLogDetailsForHomologationGroup = $"SELECT TOP 1 * FROM [{DbCatalog}].[AUDIT].[AUDITLOG] WHERE Field = 'HomologationGroupId' ORDER BY AuditLogId DESC";
        public static readonly string GetAuditLogDetailsOfHomologation = $"SELECT TOP 1 * FROM [{DbCatalog}].[AUDIT].[AUDITLOG] WHERE Field = 'HomologationId' ORDER BY AuditLogId DESC";
        public static readonly string GetHomologationId = $"SELECT HomologationId FROM [{DbCatalog}].[Admin].[HomologationGroup] WHERE HomologationGroupId = @homologationGroupId";
        public static readonly string GetHomologationObject = $"SELECT * FROM [{DbCatalog}].[Admin].[HomologationObject] WHERE HomologationGroupId = @homologationGroupId";
        public static readonly string GetHomologationDataMapping = $"SELECT * FROM [{DbCatalog}].[Admin].[HomologationDataMapping] WHERE HomologationGroupId = @homologationGroupId";
        public static readonly string GetHomologationGroup = $"SELECT * FROM [{DbCatalog}].[Admin].[HomologationGroup] WHERE HomologationGroupId = @homologationGroupId";
        public static readonly string GetHomologation = $"SELECT * FROM [{DbCatalog}].[Admin].[Homologation] WHERE HomologationId = @homologationId";
        public static readonly string DeleteHomologationData = $"DELETE FROM [{DbCatalog}].[Admin].[HomologationObject] WHERE HomologationGroupId = @homologationGroupId;" +
                                                     $"DELETE FROM [{DbCatalog}].[Admin].[HomologationDataMapping] WHERE HomologationGroupId = @homologationGroupId;" +
                                                     $"DELETE FROM [{DbCatalog}].[Admin].[HomologationGroup] WHERE HomologationId = @homologationId;" +
                                                     $"DELETE FROM [{DbCatalog}].[Admin].[Homologation] WHERE HomologationId = @homologationId";

        public static readonly string DeleteAllHomologationData = $"Update [{DbCatalog}].[Admin].[HomologationGroup] SET LastModifiedBy = 'Automation';" +
            $"DELETE FROM [{DbCatalog}].[Admin].[HomologationObject]" + $"DELETE FROM [{DbCatalog}].[Admin].[HomologationDataMapping]" +
            $"DELETE FROM [{DbCatalog}].[Admin].[HomologationGroup]" +
            $"DELETE FROM [{DbCatalog}].[Admin].[Homologation]";

        public static readonly string GetNodeConnection = $"SELECT * FROM [{DbCatalog}].[Admin].[NodeConnection]";
        public static readonly string GetActiveNodeConnection = $"SELECT * FROM [{DbCatalog}].[Admin].[NodeConnection] WHERE IsDeleted=0";
        public static readonly string GetLastNodeConnection = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[NodeConnection] ORDER BY NodeConnectionId DESC";
        public static readonly string InsertMovement = $"INSERT [{DbCatalog}].[Admin].[Movement](MessageTypeId, SystemTypeId, SourceSystem, EventType, MovementId, MovementTypeId, OperationalDate,GrossStandardVolume, NetStandardVolume, MeasureMentUnit, Scenario,Observations, [Classification], IsDeleted, CreatedBy, CreatedDate) VALUES (2, 2, 'SINOPER', 'CREATE', @MovementId, 23, GETDATE(), 23, 23, 'Volume', 'test', 'obs', 'Movement', 0, 'SYSTEM', GETDATE())";
        public static readonly string GetLastMovement = $"SELECT TOP 1 * FROM [{DbCatalog}].[OffChain].[Movement] ORDER BY MovementTransactionID DESC";
        public static readonly string InsertMovementPeriod = $"INSERT [{DbCatalog}].[Admin].[MovementPeriod](MovementTransactionID, StartTime, EndTime, CreatedBy, CreatedDate) VALUES (@MovementTransactionID, GETDATE(), GETDATE(), 'SYSTEM', GETDATE())";
        public static readonly string InsertMovementSource = $"INSERT [{DbCatalog}].[Admin].[MovementSource](MovementTransactionID, SourceNodeID, SourceStorageLocationID, SourceProductID, SourceProductTypeID, CreatedBy, CreatedDate) VALUES (@MovementTransactionID, @SourceNodeId, @SourceStorageLocationID, '10000002049', 23, 'SYSTEM', GETDATE())";
        public static readonly string InsertMovementDestination = $"INSERT [{DbCatalog}].[Admin].[MovementDestination](MovementTransactionID, DestinationNodeID, DestinationStorageLocationID, DestinationProductID, DestinationProductTypeID, CreatedBy, CreatedDate) VALUES (@MovementTransactionID, @DestinationNodeID, @SourceStorageLocationID, '10000002049', 24, 'SYSTEM', GETDATE())";
        public static readonly string GetDestinationValueBySourceValue = $"SELECT DestinationValue FROM [{DbCatalog}].[Admin].[HomologationDataMapping] WHERE SourceValue = @value";
        public static readonly string GetSourceValueByDestinationValue = $"SELECT SourceValue FROM [{DbCatalog}].[Admin].[HomologationDataMapping] WHERE DestinationValue = @value";
        public static readonly string GetPendingTransactions = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[PendingTransaction] WHERE MessageId = @messageCode";
        public static readonly string GetCreatedInventory = $"SELECT * FROM [{DbCatalog}].[Admin].[Inventory] WHERE EventType = @eventType ORDER BY CreatedDate DESC";
        public static readonly string GetInventoryProductVolume = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[InventoryProduct] WHERE ProductVolume LIKE '%-%' ORDER BY CreatedDate DESC";
        public static readonly string GetCreatedMovement = $"SELECT * FROM [Admin].[Movement] WHERE EventType = @eventType ORDER BY CreatedDate DESC";
        public static readonly string GetMovementProductVolume = $"SELECT TOP 1 * FROM [Admin].[Movement] WHERE NetStandardVolume LIKE '%-%' ORDER BY CreatedDate DESC";
        public static readonly string GetLastInventory = $"SELECT TOP 1 * FROM [{DbCatalog}].[OffChain].[InventoryProduct] ORDER BY InventoryProductId DESC";
        public static readonly string GetHomologationBySourceAndDestination = $"SELECT * FROM [{DbCatalog}].[Admin].[Homologation] WHERE SourceSystemId=1 AND DestinationSystemId=2";
        public static readonly string GetHomologationBySourceAndDestinationForXml = $"SELECT * FROM [{DbCatalog}].[Admin].[Homologation] WHERE SourceSystemId=2 AND DestinationSystemId=1";
        public static readonly string GetHomologationBySourceAndDestinationForXL = $"SELECT * FROM [{DbCatalog}].[Admin].[Homologation] WHERE SourceSystemId=3 AND DestinationSystemId=1";
        public static readonly string GetHomologationGroupByHomologationId = $"SELECT * FROM [{DbCatalog}].[Admin].[HomologationGroup] WHERE HomologationId=@homologationId";
        public static readonly string GetNodeStorageLocationByNodeId = $"SELECT * FROM [{DbCatalog}].[Admin].[NodeStorageLocation] WHERE NodeId=@nodeId";
        public static readonly string GetNodeStorageLocationByStorageLocationId = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[NodeStorageLocation] WHERE StorageLocationId=@storageLocationId ORDER BY CreatedDate Desc";
        public static readonly string GetCategoryName = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Category] ORDER BY CreatedDate DESC";
        public static readonly string GetNodeConnectionProductById = $"SELECT * FROM [{DbCatalog}].[Admin].[NodeConnectionProduct] WHERE NodeConnectionId=@nodeConnectionId AND IsDeleted='False'";
        public static readonly string GetNodeStorageLocationId = $"Select NodeStorageLocationId from [Admin].[NodeStorageLocation] where NodeId=@NodeId";
        public static readonly string GetStorageLocationProductId = $"Select StorageLocationProductId from [Admin].[StorageLocationProduct] where ProductId=@ProductId' and NodeStorageLocationId = @NodeStorageLocationId";
        public static readonly string GetOwnerDetails = $"Select * from [Admin].[StorageLocationProductOwner] where  StorageLocationProductId=@StorageLocationProductId";
        public static readonly string GetElementByCategory = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE CategoryId=1 ORDER BY ElementId DESC";
        public static readonly string DeleteHomologationObjectAndDataMapping = $"Update [{DbCatalog}].[Admin].[HomologationGroup] SET LastModifiedBy = 'Automation' WHERE HomologationGroupId = @homologationGroupId;"
                                                        + $"DELETE FROM [{DbCatalog}].[Admin].[HomologationObject] WHERE HomologationGroupId = @homologationGroupId;" +
                                                     $"DELETE FROM [{DbCatalog}].[Admin].[HomologationDataMapping] WHERE HomologationGroupId = @homologationGroupId;";

        public static readonly string DeleteHomologationAndGroup = $"Update [{DbCatalog}].[Admin].[HomologationGroup] SET LastModifiedBy = 'Automation' WHERE HomologationId = @homologationId;" + $"DELETE FROM [{DbCatalog}].[Admin].[HomologationGroup] WHERE HomologationId = @homologationId;" +
                                                     $"DELETE FROM [{DbCatalog}].[Admin].[Homologation] WHERE HomologationId = @homologationId";

        public static readonly string DeleteDataMapping = $"DELETE FROM [{DbCatalog}].[Admin].[HomologationDataMapping];" +
                                                     $"DELETE FROM [{DbCatalog}].[Admin].[HomologationObject]";

        public static readonly string GetInventoryByTankName = $"SELECT * FROM [{DbCatalog}].[Admin].[Inventory] WHERE TankName=@tankName";
        public static readonly string GetInventoryProductByInventoryId = $"SELECT * FROM [{DbCatalog}].[OffChain].[InventoryProduct] WHERE FileRegistrationTransactionId=@fileRegistrationTransactionId";
        public static readonly string GetLastFileRegistrationError = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[FileRegistrationError] ORDER BY FileRegistrationErrorID DESC";
        public static readonly string GetUploadId = $"SELECT TOP 1 * from [{DbCatalog}].[Admin].[FileRegistration] WHERE SystemTypeId = 3 ORDER BY CreatedDate DESC";
        public static readonly string GetLastTwoDaysData = $"SELECT * FROM [{DbCatalog}].[Admin].[FileRegistration] WHERE CreatedDate BETWEEN CAST (GETDATE()-2 AS DATE) AND CAST (GETDATE( ) AS DATE)";
        public static readonly string GetCategoryByCategoryId = $"SELECT Name FROM [{DbCatalog}].[Admin].[Category] WHERE CategoryId = @categoryId";
        public static readonly string GetCategoryElementNameByCategoryElementId = $"SELECT Name FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE ElementId = @elementId";
        public static readonly string GetRandomProductId = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Product] ORDER BY NEWID()";
        public static readonly string GetInventoryId = $"SELECT * FROM [{DbCatalog}].[offchain].[InventoryProduct] where InventoryId = @inventoryId";
        public static readonly string GetMovementId = $"SELECT * FROM [{DbCatalog}].[offchain].[Movement] where MovementId = @movementId ORDER BY CreatedDate DESC";
        public static readonly string GetLastInventoryId = $"SELECT TOP 1 * FROM [{DbCatalog}].[offchain].[Inventory] ORDER BY CreatedDate DESC";
        public static readonly string GetLastMovementId = $"SELECT TOP 1 * FROM [{DbCatalog}].[offchain].[Movement] ORDER BY CreatedDate DESC";
        public static readonly string DeleteInventoryId = $"DELETE FROM [{DbCatalog}].[Admin].[InventoryProduct] WHERE InventoryPrimaryKeyId = @inventoryTransactionId;" +
                                                 $"DELETE FROM [{DbCatalog}].[Admin].[Inventory] WHERE InventoryTransactionId = @inventoryTransactionId";

        public static readonly string DeleteMovementId = $"DELETE FROM [{DbCatalog}].[Admin].[MovementSource] WHERE MovementTransactionId = @movementTransactionId;" +
                                                $"DELETE FROM [{DbCatalog}].[Admin].[MovementDestination] WHERE MovementTransactionId = @movementTransactionId;" +
                                                $"DELETE FROM[{DbCatalog}].[Admin].[Attribute] WHERE MovementTransactionId = @movementTransactionId;" +
                                                $"DELETE FROM [{DbCatalog}].[Admin].[MovementPeriod] WHERE MovementTransactionId = @movementTransactionId;" +
                                                $"DELETE FROM [{DbCatalog}].[Admin].[Owner] WHERE MovementTransactionId = @movementTransactionId;" +
                                                $"DELETE FROM [{DbCatalog}].[Admin].[Movement] WHERE MovementTransactionId = @movementTransactionId";

        public static readonly string GetInventoryProduct = $"SELECT * FROM [{DbCatalog}].[Admin].[InventoryProduct] where InventoryPrimaryKeyId = @inventoryTransactionId";
        public static readonly string GetNotesForUnbalances = $"SELECT Comment FROM [{DbCatalog}].[Admin].[UnbalanceComment] WHERE NodeId = @nodeId";
        public static readonly string GetCategoryElementsByNameDesc = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[CategoryElement] ORDER BY Name DESC";
        public static readonly string GetCategoryElementsByDescriptionDesc = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[CategoryElement] ORDER BY Description DESC";
        public static readonly string GetCategoryElementsByCreatedDateDesc = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[CategoryElement] ORDER BY CreatedDate DESC";
        public static readonly string GetCategoryElementsByCategoryDesc = $"Select TOP 1 ele.* from [{DbCatalog}].[Admin].[CategoryElement] ele JOIN [{DbCatalog}].[Admin].Category cat on ele.CategoryId = cat.CategoryId order by cat.Name DESC";
        public static readonly string GetCategoryElementsByIsActiveDesc = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[CategoryElement] ORDER BY IsActive DESC,Elementid";
        public static readonly string GetTicketsByIdDesc = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].Ticket ORDER BY TicketId DESC";
        public static readonly string GetTicketsBySegmentDesc = $"SELECT TOP 1 tkt.* FROM[{DbCatalog}].[Admin].[Ticket] tkt JOIN [{DbCatalog}].[Admin].CategoryElement ce ON tkt.CategoryElementId = ce.ElementId ORDER BY ce.Name DESC";
        public static readonly string GetTicketsByStartDateDesc = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].Ticket ORDER BY StartDate DESC";
        public static readonly string GetTicketsByEndDateDesc = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].Ticket ORDER BY EndDate DESC";
        public static readonly string GetTicketsByCreatedDateDesc = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].Ticket ORDER BY CreatedDate DESC";
        public static readonly string GetTicketsByCreatedByDesc = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].Ticket ORDER BY CreatedBy DESC";
        public static readonly string GetTicketsByStateDesc = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].Ticket ORDER BY Status DESC,TicketId";
        public static readonly string UpdateNodeTagStartDateByNodeId = $"UPDATE [{DbCatalog}].[Admin].[NodeTag] SET StartDate=GETDATE()-3, LastModifiedBy='System', LastModifiedDate=GETDATE() WHERE NodeId=@nodeId";
        public static readonly string DeleteAllInventoryIds = $"DELETE FROM [{DbCatalog}].[Admin].[InventoryProduct] WHERE InventoryId IN ('TK 12130', 'TK 12131','TK 12132','TK 12133',TK 12134', 'TK 12135');" +
                                                 $"DELETE FROM [{DbCatalog}].[Admin].[Inventory] WHERE InventoryTransactionId = @inventoryTransactionId";

        public static readonly string GetMultipleInventories = $"SELECT * FROM [{DbCatalog}].[Admin].[Inventory] where InventoryId in (@inventoryId1, @inventoryId2, @inventoryId3)";
        public static readonly string GetMultipleMovements = $"SELECT * FROM [{DbCatalog}].[Admin].[Movement] where MovementId in (@movementId1, @movementId2, @movementId3)";

        public static readonly string GetLastFileRegistration = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[FileRegistration] ORDER BY CreatedDate DESC";
        public static readonly string GetFileRegistrationById = $"SELECT * FROM [{DbCatalog}].[Admin].[FileRegistration] WHERE FileRegistrationId = @fileRegistrationId";
        public static readonly string GetPendingTransactionByMessageId = $"SELECT * FROM [{DbCatalog}].[Admin].[PendingTransaction] WHERE MESSAGEID = @messageId";
        public static readonly string GetInventoryByTicketId = $"SELECT * FROM [{DbCatalog}].[Admin].[Inventory] WHERE TicketId = @ticketId";
        public static readonly string GetMovementsByTicketId = $"SELECT * FROM [{DbCatalog}].[Admin].[Movement] WHERE TicketId = @ticketId";
        public static readonly string GetMovementsByTicketIdAndInterface = $"SELECT * FROM [{DbCatalog}].[Admin].[Movement] WHERE TicketId = @ticketId AND VariableTypeId='1'";
        public static readonly string GetMovementsByTicketIdAndTolerance = $"SELECT * FROM [{DbCatalog}].[Admin].[Movement] WHERE TicketId = @ticketId AND VariableTypeId='2'";
        public static readonly string GetMovementsByTicketIdAndPNI = $"SELECT * FROM [{DbCatalog}].[Admin].[Movement] WHERE TicketId = @ticketId AND VariableTypeId='3'";
        public static readonly string GetElementId = $"SELECT * FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE name = @categoryName";
        public static readonly string GetOtherElementIds = $"SELECT * FROM [{DbCatalog}].[Admin].[NodeTag] WHERE ElementId NOT IN (@elementId) and NodeId = @nodeId";
        public static readonly string UpdateNodeTagDate = $"UPDATE [{DbCatalog}].[Admin].[NodeTag] SET StartDate = CAST (GETDATE()-@date AS DATE) ,LastModifiedBy='SYSTEM', LastModifiedDate=GETDATE() WHERE NodeId = @nodeId";
        public static readonly string UpdateElementId1 = $"UPDATE [{DbCatalog}].[Admin].[NodeTag] SET ElementId = 2 ,LastModifiedBy='System', LastModifiedDate=GETDATE() WHERE NodeTagId = @nodeTagId";
        public static readonly string UpdateElementId2 = $"UPDATE [{DbCatalog}].[Admin].[NodeTag] SET ElementId = 3 ,LastModifiedBy='System', LastModifiedDate=GETDATE() WHERE NodeTagId = @nodeTagId";
        public static readonly string GetCompletedTicket = $"SELECT TOP 1 * FROM Admin.Ticket WHERE errormessage IS NULL AND Status = 0 AND TickettypeId=1 and CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";
        public static readonly string GetErrorTicket = $"SELECT TOP 1 * FROM Admin.Ticket WHERE errormessage IS NOT NULL AND Status = 2 AND TickettypeId=1 and CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";
        public static readonly string GetInProgressTicket = $"SELECT TOP 1 * FROM Admin.Ticket WHERE TickettypeId=1 AND Status = 1 and CREATEDDATE > GETDATE()-40 ORDER BY ticketid DESC";
        public static readonly string GetLastTicket = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Ticket] WHERE Tickettypeid=1 and CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";
        public static readonly string GetTickets = $"SELECT * FROM [{DbCatalog}].[Admin].[Ticket]";
        public static readonly string GetRules = $"SELECT * FROM [{DbCatalog}].[Admin].[RuleName]";
        public static readonly string GetProducts = $"SELECT * FROM [{DbCatalog}].[Admin].[Product]";
        public static readonly string GetScenarios = $"SELECT * FROM [{DbCatalog}].[Admin].[Scenario]";
        public static readonly string NodeConnectionProduct = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[NodeConnectionProduct] ORDER BY NodeConnectionProductId DESC";
        public static readonly string NodeConnectionProductOwner = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[NodeConnectionProductOwner] ORDER BY NodeConnectionProductOwnerId DESC";
        public static readonly string GetNodeTags = $"SELECT * FROM [{DbCatalog}].[Admin].[NodeTag]";
        public static readonly string GetRandomFileId = $"SELECT Top(1) FileRegistrationId FROM [{DbCatalog}].[Admin].[FileRegistration]";
        public static readonly string GetFileRegistrations = $"SELECT TOP(1) * FROM [{DbCatalog}].[Admin].[FileRegistration] ORDER BY FileRegistrationId DESC ";

        public static readonly string GetNodeInformation = $"Declare @Today DateTime = GETUTCDATE()" +
                                                 $" Select CS.Name as Segment," +
                                                 $" N.Name as NodeName,CN.Name as NodeType,CO.Name as Operator" +
                                                 $" From [{DbCatalog}].[Admin].[Node] N" +
                                                 $" Inner Join [{DbCatalog}].[Admin].[NodeTag] Segment" +
                                                 $" On N.NodeID = Segment.NodeId" +
                                                 $" Inner Join [{DbCatalog}].[Admin].[CategoryElement] CS" +
                                                 $" On Segment.ElementId = CS.ElementId" +
                                                 $" Inner Join [{DbCatalog}].[Admin].[NodeTag] Operator" +
                                                 $" On N.NodeID = Operator.NodeId" +
                                                 $" Inner Join [{DbCatalog}].[Admin].[CategoryElement] CO" +
                                                 $" On Operator.ElementId = CO.ElementId" +
                                                 $" Inner Join [{DbCatalog}].[Admin].[NodeTag] NodeType" +
                                                 $" On N.NodeID = NodeType.NodeId" +
                                                 $" Inner Join [{DbCatalog}].[Admin].[CategoryElement] CN" +
                                                 $" On NodeType.ElementId = CN.ElementId" +
                                                 $" Where Segment.ElementId = @segmentElementId" +
                                                 $" And Operator.ElementId = @operatorElementId" +
                                                 $" And NodeType.ElementId = @nodeTypeElementId" +
                                                 $" And @Today Between Segment.StartDate  And Segment.EndDate" +
                                                 $" And @Today Between Operator.StartDate  And Operator.EndDate" +
                                                 $" And @Today Between NodeType.StartDate  And NodeType.EndDate";

        public static readonly string GetFirstNode = $"SELECT TOP 1  * FROM [{DbCatalog}].[Admin].[Node]";
        public static readonly string GetSecondPageTickets = $"SELECT TOP 1 * FROM Admin.Ticket WHERE ticketid NOT IN (SELECT TOP 10 ticketid FROM Admin.Ticket WHERE TICKETTYPEID=1 ORDER BY CREATEDDATE DESC) AND TICKETTYPEID=1 and CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";
        public static readonly string GetSecondPageTicketsOfOwnershipCalculation = $"SELECT TOP 1 * FROM Admin.Ticket WHERE ticketid NOT IN (SELECT TOP 10 ticketid FROM Admin.Ticket WHERE TICKETTYPEID=2 ORDER BY CREATEDDATE DESC) AND TICKETTYPEID=2 and CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";
        public static readonly string GetSecondPageExceptions = $"SELECT TOP 1 * FROM Admin.PendingTransactionError WHERE ErrorId NOT IN (SELECT TOP 10 ErrorId FROM Admin.PendingTransactionError ORDER BY CREATEDDATE DESC) AND CREATEDDATE > GETDATE()-40 AND COMMENT IS NULL ORDER BY CREATEDDATE DESC";
        public static readonly string GetSegmentNamewithoutMovementsandInventories = $"SELECT TOP 1 [CE].Name from [{DbCatalog}].[Admin].[CategoryElement] [CE]" +
                                                                           "INNER JOIN [{DbCatalog}].[Admin].[NodeTag] [NT]" +
                                                                           "ON [NT].ElementId = [CE].ElementId" +
                                                                           "WHERE [CE].CategoryId=2" +
                                                                           "AND [NT].NodeId NOT IN (SELECT [MS].SourceNodeId FROM [{DbCatalog}].[Admin].[MovementSource] [MS])" +
                                                                           "AND [NT].NodeId NOT IN (SELECT [MD].DestinationNodeId FROM [{DbCatalog}].[Admin].[MovementDestination] [MD])" +
                                                                           "AND [NT].NodeId NOT IN (SELECT [Inv].NodeID FROM [{DbCatalog}].[Admin].[Inventory] [Inv])";

        public static readonly string GetSortedSegmentNodeInformation = $"Declare @date DateTime = GETUTCDATE()" +
                                                               " Select TOP 1 CS.Name as Segment, N.Name as NodeName, CN.Name as NodeType,CO.Name as Operator" +
                                                               " From    [{DbCatalog}].[Admin].[Node] N" +
                                                               " Inner Join [{DbCatalog}].[Admin].[NodeTag] NT" +
                                                               " On NT.NodeId = N.NodeId" +
                                                               " Inner Join [{DbCatalog}].[Admin].[CategoryElement] CS" +
                                                               " On NT.ElementId = CS.ElementId" +
                                                               " Inner Join [{DbCatalog}].[Admin].[NodeTag] NT1" +
                                                               " On NT1.NodeId = N.NodeId" +
                                                               " Inner Join [{DbCatalog}].[Admin].[CategoryElement] CN" +
                                                               " On NT1.ElementId = CN.ElementId" +
                                                               " Inner Join [{DbCatalog}].[Admin].[NodeTag] NT2" +
                                                               " On NT2.NodeId = N.NodeId" +
                                                               " Inner Join [{DbCatalog}].[Admin].[CategoryElement] CO" +
                                                               " On NT2.ElementId = CO.ElementId" +
                                                               " Where NT.ElementId = 10 And @date between NT.StartDate and NT.EndDate" +
                                                               " And NT1.ElementId in (Select ElementId From Admin.CategoryElement Where CategoryId = 1 and IsActive = 1)" +
                                                               " And NT2.ElementId in (Select ElementId From Admin.CategoryElement Where CategoryId = 3 and IsActive = 1)" +
                                                               " And @date between NT1.StartDate and NT1.EndDate" +
                                                               " And @date between NT2.StartDate and NT2.EndDate order by CS.Name DESC,NT.CreatedDate DESC";

        public static readonly string GetSortedNodeInformation = $"Declare @date DateTime = GETUTCDATE()" +
                                                       " Select TOP 1 CS.Name as Segment, N.Name as NodeName, CN.Name as NodeType,CO.Name as Operator" +
                                                       " From    [{DbCatalog}].[Admin].[Node] N" +
                                                       " Inner Join [{DbCatalog}].[Admin].[NodeTag] NT" +
                                                       " On NT.NodeId = N.NodeId" +
                                                       " Inner Join [{DbCatalog}].[Admin].[CategoryElement] CS" +
                                                       " On NT.ElementId = CS.ElementId" +
                                                       " Inner Join [{DbCatalog}].[Admin].[NodeTag] NT1" +
                                                       " On NT1.NodeId = N.NodeId" +
                                                       " Inner Join [{DbCatalog}].[Admin].[CategoryElement] CN" +
                                                       " On NT1.ElementId = CN.ElementId" +
                                                       " Inner Join [{DbCatalog}].[Admin].[NodeTag] NT2" +
                                                       " On NT2.NodeId = N.NodeId" +
                                                       " Inner Join [{DbCatalog}].[Admin].[CategoryElement] CO" +
                                                       " On NT2.ElementId = CO.ElementId" +
                                                       " Where NT.ElementId = 10 And @date between NT.StartDate and NT.EndDate" +
                                                       " And NT1.ElementId in (Select ElementId From Admin.CategoryElement Where CategoryId = 1 and IsActive = 1)" +
                                                       " And NT2.ElementId in (Select ElementId From Admin.CategoryElement Where CategoryId = 3 and IsActive = 1)" +
                                                       " And @date between NT1.StartDate and NT1.EndDate" +
                                                       " And @date between NT2.StartDate and NT2.EndDate order by N.Name DESC";

        public static readonly string GetSortedNodeTypeInformation = $"Declare @date DateTime = GETUTCDATE()" +
                                                           " Select TOP 1 CS.Name as Segment, N.Name as NodeName, CN.Name as NodeType,CO.Name as Operator" +
                                                           " From    [{DbCatalog}].[Admin].[Node] N" +
                                                           " Inner Join [{DbCatalog}].[Admin].[NodeTag] NT" +
                                                           " On NT.NodeId = N.NodeId" +
                                                           " Inner Join [{DbCatalog}].[Admin].[CategoryElement] CS" +
                                                           " On NT.ElementId = CS.ElementId" +
                                                           " Inner Join [{DbCatalog}].[Admin].[NodeTag] NT1" +
                                                           " On NT1.NodeId = N.NodeId" +
                                                           " Inner Join [{DbCatalog}].[Admin].[CategoryElement] CN" +
                                                           " On NT1.ElementId = CN.ElementId" +
                                                           " Inner Join [{DbCatalog}].[Admin].[NodeTag] NT2" +
                                                           " On NT2.NodeId = N.NodeId" +
                                                           " Inner Join [{DbCatalog}].[Admin].[CategoryElement] CO" +
                                                           " On NT2.ElementId = CO.ElementId" +
                                                           " Where NT.ElementId = 10 And @date between NT.StartDate and NT.EndDate" +
                                                           " And NT1.ElementId in (Select ElementId From Admin.CategoryElement Where CategoryId = 1 and IsActive = 1)" +
                                                           " And NT2.ElementId in (Select ElementId From Admin.CategoryElement Where CategoryId = 3 and IsActive = 1)" +
                                                           " And @date between NT1.StartDate and NT1.EndDate" +
                                                           " And @date between NT2.StartDate and NT2.EndDate order by CN.Name DESC,NT1.CreatedDate DESC";

        public static readonly string GetSortedOperatorNodeInformation = $"Declare @date DateTime = GETUTCDATE()" +
                                                               " Select TOP 1 CS.Name as Segment, N.Name as NodeName, CN.Name as NodeType,CO.Name as Operator" +
                                                               " From    [{DbCatalog}].[Admin].[Node] N" +
                                                               " Inner Join [{DbCatalog}].[Admin].[NodeTag] NT" +
                                                               " On NT.NodeId = N.NodeId" +
                                                               " Inner Join [{DbCatalog}].[Admin].[CategoryElement] CS" +
                                                               " On NT.ElementId = CS.ElementId" +
                                                               " Inner Join [{DbCatalog}].[Admin].[NodeTag] NT1" +
                                                               " On NT1.NodeId = N.NodeId" +
                                                               " Inner Join [{DbCatalog}].[Admin].[CategoryElement] CN" +
                                                               " On NT1.ElementId = CN.ElementId" +
                                                               " Inner Join [{DbCatalog}].[Admin].[NodeTag] NT2" +
                                                               " On NT2.NodeId = N.NodeId" +
                                                               " Inner Join [{DbCatalog}].[Admin].[CategoryElement] CO" +
                                                               " On NT2.ElementId = CO.ElementId" +
                                                               " Where NT.ElementId = 10 And @date between NT.StartDate and NT.EndDate" +
                                                               " And NT1.ElementId in (Select ElementId From Admin.CategoryElement Where CategoryId = 1 and IsActive = 1)" +
                                                               " And NT2.ElementId in (Select ElementId From Admin.CategoryElement Where CategoryId = 3 and IsActive = 1)" +
                                                               " And @date between NT1.StartDate and NT1.EndDate" +
                                                               " And @date between NT2.StartDate and NT2.EndDate order by CO.Name DESC,NT2.CreatedDate DESC";

        public static readonly string GetNodeInformationForTransporteSegment = $"Declare @date DateTime = GETUTCDATE()" +
                                                                     " Select CS.Name as Segment, N.Name as NodeName, CN.Name as NodeType,CO.Name as Operator" +
                                                                     " From    [{DbCatalog}].[Admin].[Node] N" +
                                                                     " Inner Join [{DbCatalog}].[Admin].[NodeTag] NT" +
                                                                     " On NT.NodeId = N.NodeId" +
                                                                     " Inner Join [{DbCatalog}].[Admin].[CategoryElement] CS" +
                                                                     " On NT.ElementId = CS.ElementId" +
                                                                     " Inner Join [{DbCatalog}].[Admin].[NodeTag] NT1" +
                                                                     " On NT1.NodeId = N.NodeId" +
                                                                     " Inner Join [{DbCatalog}].[Admin].[CategoryElement] CN" +
                                                                     " On NT1.ElementId = CN.ElementId" +
                                                                     " Inner Join [{DbCatalog}].[Admin].[NodeTag] NT2" +
                                                                     " On NT2.NodeId = N.NodeId" +
                                                                     " Inner Join [{DbCatalog}].[Admin].[CategoryElement] CO" +
                                                                     " On NT2.ElementId = CO.ElementId" +
                                                                     " Where NT.ElementId = 10 And @date between NT.StartDate and NT.EndDate" +
                                                                     " And NT1.ElementId in (Select ElementId From Admin.CategoryElement Where CategoryId = 1 and IsActive = 1)" +
                                                                     " And NT2.ElementId in (Select ElementId From Admin.CategoryElement Where CategoryId = 3 and IsActive = 1)" +
                                                                     " And @date between NT1.StartDate and NT1.EndDate" +
                                                                     " And @date between NT2.StartDate and NT2.EndDate";

        public static readonly string GetAllStorageLocations = $"SELECT * from [{DbCatalog}].[Admin].[LogisticCenter]";
        public static readonly string GetProductByStorageLocation = $"SELECT * from [{DbCatalog}].[Admin].[Product] WHERE StorageLocationId = @storageLocationId and Name = @name";
        public static readonly string GetStorageLocationOwner = $"SELECT * FROM [{DbCatalog}].[Admin].[StorageLocationProduct] WHERE NodeStorageLocationId IN ( select NodeStorageLocationId from [{DbCatalog}].[Admin].[NodeStorageLocation] where NodeId = @nodeId)";
        public static readonly string GetOwnerhsipCalcTickets = $"SELECT * FROM [{DbCatalog}].[Admin].[Ticket] where CategoryElementId IN (SELECT ElementId from [{DbCatalog}].[Admin].[CategoryElement] WHERE Name = @CategorySegmentName) ORDERBY EndDate DESC";
        public static readonly string GetConnectionProducts = $"SELECT * FROM [{DbCatalog}].[Admin].NodeConnectionProduct NCP JOIN [{DbCatalog}].[Admin].NodeConnection NC ON NCP.NodeConnectionId = NC.NodeConnectionId WHERE SourceNodeId = @sourceNodeId";

        public static readonly string GetAllCalculatedMovementsOfOperationalCutoff = $"SELECT DISTINCT MOVEMENTID FROM [{DbCatalog}].[OFFCHAIN].[MOVEMENT] WHERE TICKETID = @ticketId";
        public static readonly string GetLatestOwnershipCalculationTicket = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Ticket] WHERE Tickettypeid=2 and CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";
        public static readonly string GetOwnershipCalculationTickets = $"SELECT * FROM [{DbCatalog}].[Admin].[Ticket] WHERE Tickettypeid=2 and CREATEDDATE > GETDATE()-40";
        public static readonly string GetOperationalCutoffTickets = $"SELECT * FROM [{DbCatalog}].[Admin].[Ticket] WHERE Tickettypeid=1 and CREATEDDATE > GETDATE()-40";
        public static readonly string GetLastException = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[PendingTransactionError] WHERE CREATEDDATE > GETDATE()-40 AND COMMENT IS NULL ORDER BY CREATEDDATE DESC";
        public static readonly string GetExceptions = $"SELECT * FROM [{DbCatalog}].[Admin].[PendingTransactionError] WHERE CREATEDDATE > GETDATE()-40 AND COMMENT IS NULL";
        public static readonly string GetTicketSortedByTicketIdForOwnershipForSegmentsGrid = $"SELECT TOP 1 T.TicketId,CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate," +
                                                                                   " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate," +
                                                                                   " CONVERT(VARCHAR(10), T.CreatedDate, 3)+ ' '+ " +
                                                                                   " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate," +
                                                                                   " T.CreatedBy as CreatedBy,CE.Name," +
                                                                                   " CASE" +
                                                                                   " WHEN T.[Status] = 0 THEN 'Finalizado'" +
                                                                                   " WHEN T.[Status] = 1 THEN 'Enviado'" +
                                                                                   " WHEN T.[Status] = 2 THEN 'Fallido'" +
                                                                                   " END AS Status" +
                                                                                   " FROM [{DbCatalog}].[Admin].[Ticket] T join" +
                                                                                   " [{DbCatalog}].[Admin].[CategoryElement] CE on T.CategoryElementId = CE.ElementId" +
                                                                                   " where T.Tickettypeid=2 and T.CREATEDDATE > GETDATE()-40" +
                                                                                   " ORDER BY T.TicketId DESC";

        public static readonly string GetTicketSortedByStartDateForOwnershipForSegmentsGrid = $"SELECT TOP 1 T.TicketId,CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate," +
                                                                                    " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate," +
                                                                                    " CONVERT(VARCHAR(10), T.CreatedDate, 3)+ ' '+ " +
                                                                                    " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate," +
                                                                                    " T.CreatedBy as CreatedBy,CE.Name," +
                                                                                    " CASE" +
                                                                                    " WHEN T.[Status] = 0 THEN 'Finalizado'" +
                                                                                    " WHEN T.[Status] = 1 THEN 'Enviado'" +
                                                                                    " WHEN T.[Status] = 2 THEN 'Fallido'" +
                                                                                    " END AS Status" +
                                                                                    " FROM [{DbCatalog}].[Admin].[Ticket] T join" +
                                                                                    " [{DbCatalog}].[Admin].[CategoryElement] CE on T.CategoryElementId = CE.ElementId" +
                                                                                    " where T.Tickettypeid=2 and T.CREATEDDATE > GETDATE()-40" +
                                                                                    " ORDER BY T.StartDate DESC";

        public static readonly string GetTicketSortedByEndDateForOwnershipForSegmentsGrid = $"SELECT TOP 1 T.TicketId,CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate," +
                                                                                  " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate," +
                                                                                  " CONVERT(VARCHAR(10), T.CreatedDate, 3)+ ' '+ " +
                                                                                  " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate," +
                                                                                  " T.CreatedBy as CreatedBy,CE.Name," +
                                                                                  " CASE" +
                                                                                  " WHEN T.[Status] = 0 THEN 'Finalizado'" +
                                                                                  " WHEN T.[Status] = 1 THEN 'Enviado'" +
                                                                                  " WHEN T.[Status] = 2 THEN 'Fallido'" +
                                                                                  " END AS Status" +
                                                                                  " FROM [{DbCatalog}].[Admin].[Ticket] T join" +
                                                                                  " [{DbCatalog}].[Admin].[CategoryElement] CE on T.CategoryElementId = CE.ElementId" +
                                                                                  " where T.Tickettypeid=2 and T.CREATEDDATE > GETDATE()-40" +
                                                                                  " ORDER BY T.EndDate DESC";

        public static readonly string GetTicketSortedByCreatedTicketDateForOwnershipForSegmentsGrid = $"SELECT TOP 1 T.TicketId,CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate," +
                                                                                            " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate," +
                                                                                            " CONVERT(VARCHAR(10), T.CreatedDate, 3)+ ' '+ " +
                                                                                            " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate," +
                                                                                            " T.CreatedBy as CreatedBy,CE.Name," +
                                                                                            " CASE" +
                                                                                            " WHEN T.[Status] = 0 THEN 'Finalizado'" +
                                                                                            " WHEN T.[Status] = 1 THEN 'Enviado'" +
                                                                                            " WHEN T.[Status] = 2 THEN 'Fallido'" +
                                                                                            " END AS Status" +
                                                                                            " FROM [{DbCatalog}].[Admin].[Ticket] T join" +
                                                                                            " [{DbCatalog}].[Admin].[CategoryElement] CE on T.CategoryElementId = CE.ElementId" +
                                                                                            " where T.Tickettypeid=2 and T.CREATEDDATE > GETDATE()-40" +
                                                                                            " ORDER BY T.CreatedDate DESC";

        public static readonly string GetTicketSortedByCreatedByForOwnershipForSegmentsGrid = $"SELECT TOP 1 T.TicketId,CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate," +
                                                                                   " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate," +
                                                                                   " CONVERT(VARCHAR(10), T.CreatedDate, 3)+ ' '+ " +
                                                                                   " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate," +
                                                                                   " T.CreatedBy as CreatedBy,CE.Name," +
                                                                                   " CASE" +
                                                                                   " WHEN T.[Status] = 0 THEN 'Finalizado'" +
                                                                                   " WHEN T.[Status] = 1 THEN 'Enviado'" +
                                                                                   " WHEN T.[Status] = 2 THEN 'Fallido'" +
                                                                                   " END AS Status" +
                                                                                   " FROM [{DbCatalog}].[Admin].[Ticket] T join" +
                                                                                   " [{DbCatalog}].[Admin].[CategoryElement] CE on T.CategoryElementId = CE.ElementId" +
                                                                                   " where T.Tickettypeid=2 and T.CREATEDDATE > GETDATE()-40" +
                                                                                   " ORDER BY T.CreatedBy DESC";

        public static readonly string GetTicketSortedBySegmentNameForOwnershipForSegmentsGrid = $"SELECT TOP 1 T.TicketId,CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate," +
                                                                                      " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate," +
                                                                                      " CONVERT(VARCHAR(10), T.CreatedDate, 3)+ ' '+ " +
                                                                                      " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate," +
                                                                                      " T.CreatedBy as CreatedBy,CE.Name," +
                                                                                      " CASE" +
                                                                                      " WHEN T.[Status] = 0 THEN 'Finalizado'" +
                                                                                      " WHEN T.[Status] = 1 THEN 'Enviado'" +
                                                                                      " WHEN T.[Status] = 2 THEN 'Fallido'" +
                                                                                      " END AS Status" +
                                                                                      " FROM [{DbCatalog}].[Admin].[Ticket] T join" +
                                                                                      " [{DbCatalog}].[Admin].[CategoryElement] CE on T.CategoryElementId = CE.ElementId" +
                                                                                      " where T.Tickettypeid=2 and T.CREATEDDATE > GETDATE()-40" +
                                                                                      " ORDER BY CE.Name DESC";

        public static readonly string GetTicketSortedByStatusForOwnershipForSegmentsGrid = $"SELECT TOP 1 T.TicketId,CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate," +
                                                                                 " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate," +
                                                                                 " CONVERT(VARCHAR(10), T.CreatedDate, 3)+ ' '+ " +
                                                                                 " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate," +
                                                                                 " T.CreatedBy as CreatedBy,CE.Name," +
                                                                                 " CASE" +
                                                                                 " WHEN T.[Status] = 0 THEN 'Finalizado'" +
                                                                                 " WHEN T.[Status] = 1 THEN 'Enviado'" +
                                                                                 " WHEN T.[Status] = 2 THEN 'Fallido'" +
                                                                                 " END AS Status" +
                                                                                 " FROM [{DbCatalog}].[Admin].[Ticket] T join" +
                                                                                 " [{DbCatalog}].[Admin].[CategoryElement] CE on T.CategoryElementId = CE.ElementId" +
                                                                                 " where T.Tickettypeid=2 and T.CREATEDDATE > GETDATE()-40" +
                                                                                 " ORDER BY T.Status DESC";

        public static readonly string GetSubmittedTicket = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Ticket] WHERE TickettypeId=2 AND Status = 1 and CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";
        public static readonly string GetCompletedTicketOfOwnershipCalculationGrid = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Ticket] WHERE errormessage IS NULL AND Status = 0 AND TickettypeId=2 and CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";
        public static readonly string GetErrorTicketOfOwnershipCalculationGrid = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Ticket] WHERE errormessage IS NOT NULL AND Status = 2 AND TickettypeId=2 and CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";
        public static readonly string UpdateTicketIdwithErrorStatus = $"UPDATE [{DbCatalog}].[Admin].[Ticket] SET STATUS=2, ErrorMessage = 'ErrorMessage', LastModifiedBy='System', LastModifiedDate=GETDATE() WHERE TICKETID=@ticketId";

        public static readonly string GetLastOwnershipRule = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE categoryid=10 ORDER BY ElementId DESC";

        public static readonly string GetOwnershipCalculationTicketsForNode = $"SELECT * FROM [{DbCatalog}].[Admin].[OwnerShipNode] WHERE CREATEDDATE > GETDATE()-40";
        public static readonly string UpdateTicketIdwithProcessingStatus = $"UPDATE [{DbCatalog}].[Admin].[Ticket] SET STATUS=1, ErrorMessage = NULL, LastModifiedBy='System', LastModifiedDate=GETDATE() WHERE TICKETID=@ticketId";
        public static readonly string UpdateTicketIdwithProcessedStatus = $"UPDATE [{DbCatalog}].[Admin].[Ticket] SET STATUS=0, ErrorMessage = NULL, LastModifiedBy='System', LastModifiedDate=GETDATE() WHERE TICKETID=@ticketId";
        public static readonly string GetSubmittedTicketInOwnershipPerNodeGrid = $"SELECT TOP 1 T.TicketId FROM [{DbCatalog}].[Admin].[OwnerShipNode] [OCN] JOIN [{DbCatalog}].[Admin].[Ticket] [T] ON OCN.TicketId = T.TicketId WHERE OCN.Status = 1 and T.CREATEDDATE > GETDATE()-40 ORDER BY T.CREATEDDATE DESC";
        public static readonly string UpdateTicketIdwithProcessingStatusInOwnershipCalculationPerNodeGrid = $"UPDATE [{DbCatalog}].[Admin].[OwnerShipNode] SET STATUS=1, LastModifiedBy='System', LastModifiedDate=GETDATE() WHERE TICKETID=@ticketId";
        public static readonly string GetErrorTicketOfOwnershipCalculationPerNodeGrid = $"SELECT TOP 1 T.TicketId FROM [{DbCatalog}].[Admin].[OwnerShipNode] [OCN] JOIN [{DbCatalog}].[Admin].[Ticket] [T] ON OCN.TicketId = T.TicketId WHERE OCN.Status = 2 and T.CREATEDDATE > GETDATE()-40 ORDER BY T.CREATEDDATE DESC";
        public static readonly string UpdateTicketIdwithErrorStatusInOwnershipCalculationPerNodeGrid = $"UPDATE [{DbCatalog}].[Admin].[OwnerShipNode] SET STATUS=2, LastModifiedBy='System', LastModifiedDate=GETDATE() WHERE TICKETID=@ticketId";
        public static readonly string GetCompletedTicketOfOwnershipCalculationPerNodeGrid = $"SELECT TOP 1 T.TicketId FROM [{DbCatalog}].[Admin].[OwnerShipNode] [OCN] JOIN [{DbCatalog}].[Admin].[Ticket] [T] ON OCN.TicketId = T.TicketId WHERE OCN.Status = 0 and T.CREATEDDATE > GETDATE()-40 ORDER BY T.CREATEDDATE DESC";
        public static readonly string UpdateTicketIdwithProcessedStatusInOwnershipCalculationPerNodeGrid = $"UPDATE [{DbCatalog}].[Admin].[OwnerShipNode] SET STATUS=0, LastModifiedBy='System', LastModifiedDate=GETDATE() WHERE TICKETID=@ticketId";
        public static readonly string GetSecondPageTicketsOfOwnershipCalculationForNode = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[OwnerShipNode] WHERE Ticketid NOT IN (SELECT TOP 10 ticketid FROM [{DbCatalog}].[Admin].[OwnerShipNode] WHERE CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC) AND CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";
        public static readonly string GetPreviousCompletedRecordInOwnershipCalculationForSegments = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Ticket] where status=0 and tickettypeid=2 and ticketid not in (Select TOP 1 TicketId FROM [{DbCatalog}].[Admin].[Ticket] where status=0 and tickettypeid=2 ORDER BY createddate DESC) ORDER BY createddate DESC";
        public static readonly string GetSecondTicketWithCompletedStatusInTicketTable = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Ticket] WHERE tickettypeid=2 and Ticketid not in (Select TOP 1 TicketId FROM [{DbCatalog}].[Admin].[Ticket] where tickettypeid=2 and CREATEDDATE > GETDATE()-40 ORDER BY createddate DESC) and CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";

        public static readonly string GetTicketSortedByTicketIdForOwnershipForNodesGrid = $"SELECT TOP 1 OCN.TicketId, CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate," +
                                                                                " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate," +
                                                                                " CONVERT(VARCHAR(10), T.CreatedDate, 3) + ' ' +" +
                                                                                " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate," +
                                                                                " T.CreatedBy,N.Name AS Node,CE.Name AS Segment," +
                                                                                " CASE" +
                                                                                " WHEN OCN.[Status] = 0 THEN 'Finalizado'" +
                                                                                " WHEN OCN.[Status] = 1 THEN 'Enviado'" +
                                                                                " WHEN OCN.[Status] = 2 THEN 'Finalizado'" +
                                                                                " END AS Status" +
                                                                                " FROM [{DbCatalog}].[Admin].[OwnerShipNode] OCN" +
                                                                                " JOIN [{DbCatalog}].[Admin].[Ticket] T ON OCN.TicketId = T.TicketId" +
                                                                                " JOIN [{DbCatalog}].[Admin].[Node] N ON OCN.NodeId = N.NodeId" +
                                                                                " JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryElementId = CE.ElementId" +
                                                                                " WHERE OCN.CREATEDDATE > GETDATE()-40" +
                                                                                " ORDER BY OCN.TicketId DESC";

        public static readonly string GetTicketSortedByStartDateForOwnershipForNodesGrid = $"SELECT TOP 1 OCN.TicketId, CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate," +
                                                                                 " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate," +
                                                                                 " CONVERT(VARCHAR(10), T.CreatedDate, 3) + ' ' +" +
                                                                                 " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate," +
                                                                                 " T.CreatedBy,N.Name AS Node,CE.Name AS Segment," +
                                                                                 " CASE" +
                                                                                 " WHEN OCN.[Status] = 0 THEN 'Finalizado'" +
                                                                                 " WHEN OCN.[Status] = 1 THEN 'Enviado'" +
                                                                                 " WHEN OCN.[Status] = 2 THEN 'Finalizado'" +
                                                                                 " END AS Status" +
                                                                                 " FROM [{DbCatalog}].[Admin].[OwnerShipNode] OCN" +
                                                                                 " JOIN [{DbCatalog}].[Admin].[Ticket] T ON OCN.TicketId = T.TicketId" +
                                                                                 " JOIN [{DbCatalog}].[Admin].[Node] N ON OCN.NodeId = N.NodeId" +
                                                                                 " JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryElementId = CE.ElementId" +
                                                                                 " WHERE OCN.CREATEDDATE > GETDATE()-40" +
                                                                                 " ORDER BY T.StartDate DESC";

        public static readonly string GetTicketSortedByEndDateForOwnershipForNodesGrid = $"SELECT TOP 1 OCN.TicketId, CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate," +
                                                                               " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate," +
                                                                               " CONVERT(VARCHAR(10), T.CreatedDate, 3) + ' ' +" +
                                                                               " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate," +
                                                                               " T.CreatedBy,N.Name AS Node,CE.Name AS Segment," +
                                                                               " CASE" +
                                                                               " WHEN OCN.[Status] = 0 THEN 'Finalizado'" +
                                                                               " WHEN OCN.[Status] = 1 THEN 'Enviado'" +
                                                                               " WHEN OCN.[Status] = 2 THEN 'Finalizado'" +
                                                                               " END AS Status" +
                                                                               " FROM [{DbCatalog}].[Admin].[OwnerShipNode] OCN" +
                                                                               " JOIN [{DbCatalog}].[Admin].[Ticket] T ON OCN.TicketId = T.TicketId" +
                                                                               " JOIN [{DbCatalog}].[Admin].[Node] N ON OCN.NodeId = N.NodeId" +
                                                                               " JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryElementId = CE.ElementId" +
                                                                               " WHERE OCN.CREATEDDATE > GETDATE()-40" +
                                                                               " ORDER BY T.EndDate DESC";

        public static readonly string GetTicketSortedByCreatedTicketDateForOwnershipForNodesGrid = $"SELECT TOP 1 OCN.TicketId, CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate," +
                                                                                        " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate," +
                                                                                        " CONVERT(VARCHAR(10), T.CreatedDate, 3) + ' ' +" +
                                                                                        " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate," +
                                                                                        " T.CreatedBy,N.Name AS Node,CE.Name AS Segment," +
                                                                                        " CASE" +
                                                                                        " WHEN OCN.[Status] = 0 THEN 'Finalizado'" +
                                                                                        " WHEN OCN.[Status] = 1 THEN 'Enviado'" +
                                                                                        " WHEN OCN.[Status] = 2 THEN 'Finalizado'" +
                                                                                        " END AS Status" +
                                                                                        " FROM [{DbCatalog}].[Admin].[OwnerShipNode] OCN" +
                                                                                        " JOIN [{DbCatalog}].[Admin].[Ticket] T ON OCN.TicketId = T.TicketId" +
                                                                                        " JOIN [{DbCatalog}].[Admin].[Node] N ON OCN.NodeId = N.NodeId" +
                                                                                        " JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryElementId = CE.ElementId" +
                                                                                        " WHERE OCN.CREATEDDATE > GETDATE()-40" +
                                                                                        " ORDER BY OCN.CREATEDDATE DESC";

        public static readonly string GetTicketSortedByCreatedByForOwnershipForNodesGrid = $"SELECT TOP 1 OCN.TicketId, CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate," +
                                                                                " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate," +
                                                                                " CONVERT(VARCHAR(10), T.CreatedDate, 3) + ' ' +" +
                                                                                " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate," +
                                                                                " T.CreatedBy,N.Name AS Node,CE.Name AS Segment," +
                                                                                " CASE" +
                                                                                " WHEN OCN.[Status] = 0 THEN 'Finalizado'" +
                                                                                " WHEN OCN.[Status] = 1 THEN 'Enviado'" +
                                                                                " WHEN OCN.[Status] = 2 THEN 'Finalizado'" +
                                                                                " END AS Status" +
                                                                                " FROM [{DbCatalog}].[Admin].[OwnerShipNode] OCN" +
                                                                                " JOIN [{DbCatalog}].[Admin].[Ticket] T ON OCN.TicketId = T.TicketId" +
                                                                                " JOIN [{DbCatalog}].[Admin].[Node] N ON OCN.NodeId = N.NodeId" +
                                                                                " JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryElementId = CE.ElementId" +
                                                                                " WHERE OCN.CREATEDDATE > GETDATE()-40" +
                                                                                " ORDER BY T.CreatedBy DESC";

        public static readonly string GetTicketSortedBySegmentNameForOwnershipForNodesGrid = $"SELECT TOP 1 OCN.TicketId, CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate," +
                                                                                   " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate," +
                                                                                   " CONVERT(VARCHAR(10), T.CreatedDate, 3) + ' ' +" +
                                                                                   " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate," +
                                                                                   " T.CreatedBy,N.Name AS Node,CE.Name AS Segment," +
                                                                                   " CASE" +
                                                                                   " WHEN OCN.[Status] = 0 THEN 'Finalizado'" +
                                                                                   " WHEN OCN.[Status] = 1 THEN 'Enviado'" +
                                                                                   " WHEN OCN.[Status] = 2 THEN 'Finalizado'" +
                                                                                   " END AS Status" +
                                                                                   " FROM [{DbCatalog}].[Admin].[OwnerShipNode] OCN" +
                                                                                   " JOIN [{DbCatalog}].[Admin].[Ticket] T ON OCN.TicketId = T.TicketId" +
                                                                                   " JOIN [{DbCatalog}].[Admin].[Node] N ON OCN.NodeId = N.NodeId" +
                                                                                   " JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryElementId = CE.ElementId" +
                                                                                   " WHERE OCN.CREATEDDATE > GETDATE()-40" +
                                                                                   " ORDER BY CE.Name DESC";

        public static readonly string GetTicketSortedByNodeNameForOwnershipForNodesGrid = $"SELECT TOP 1 OCN.TicketId, CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate," +
                                                                                " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate," +
                                                                                " CONVERT(VARCHAR(10), T.CreatedDate, 3) + ' ' +" +
                                                                                " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate," +
                                                                                " T.CreatedBy,N.Name AS Node,CE.Name AS Segment," +
                                                                                " CASE" +
                                                                                " WHEN OCN.[Status] = 0 THEN 'Finalizado'" +
                                                                                " WHEN OCN.[Status] = 1 THEN 'Enviado'" +
                                                                                " WHEN OCN.[Status] = 2 THEN 'Finalizado'" +
                                                                                " END AS Status" +
                                                                                " FROM [{DbCatalog}].[Admin].[OwnerShipNode] OCN" +
                                                                                " JOIN [{DbCatalog}].[Admin].[Ticket] T ON OCN.TicketId = T.TicketId" +
                                                                                " JOIN [{DbCatalog}].[Admin].[Node] N ON OCN.NodeId = N.NodeId" +
                                                                                " JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryElementId = CE.ElementId" +
                                                                                " WHERE OCN.CREATEDDATE > GETDATE()-40" +
                                                                                " ORDER BY N.Name DESC";

        public static readonly string GetTicketSortedByStatusForOwnershipForNodesGrid = $"SELECT TOP 1 OCN.TicketId, CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate," +
                                                                              " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate," +
                                                                              " CONVERT(VARCHAR(10), T.CreatedDate, 3) + ' ' +" +
                                                                              " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate," +
                                                                              " T.CreatedBy,N.Name AS Node,CE.Name AS Segment," +
                                                                              " CASE" +
                                                                              " WHEN OCN.[Status] = 0 THEN 'Finalizado'" +
                                                                              " WHEN OCN.[Status] = 1 THEN 'Enviado'" +
                                                                              " WHEN OCN.[Status] = 2 THEN 'Finalizado'" +
                                                                              " END AS Status" +
                                                                              " FROM [{DbCatalog}].[Admin].[OwnerShipNode] OCN" +
                                                                              " JOIN [{DbCatalog}].[Admin].[Ticket] T ON OCN.TicketId = T.TicketId" +
                                                                              " JOIN [{DbCatalog}].[Admin].[Node] N ON OCN.NodeId = N.NodeId" +
                                                                              " JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryElementId = CE.ElementId" +
                                                                              " WHERE OCN.CREATEDDATE > GETDATE()-40" +
                                                                              " ORDER BY OCN.Status DESC";

        public static readonly string GetLastTransformation = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Transformation] ORDER BY TransformationId DESC";
        public static readonly string GetProductById = $"SELECT * FROM [{DbCatalog}].[Admin].[Product] WHERE ProductId = @productId";
        public static readonly string GetNodeById = $"SELECT * FROM [{DbCatalog}].[Admin].[Node] WHERE NodeId = @nodeId";

        public static readonly string GetAlgorithms = $"SELECT ModelName FROM [{DbCatalog}].[Admin].[Algorithm]";

        public static readonly string GetMovementTransformationRecord = $"SELECT ms.SourceNodeId,ms.SourceProductId,md.DestinationNodeId,md.DestinationProductId FROM offchain.movement m JOIN offchain.movementsource ms ON ms.MovementTransactionId = m.MovementTransactionId JOIN offchain.movementdestination md ON md.MovementTransactionId = m.MovementTransactionId WHERE m.MovementId = @movementId";
        public static readonly string GetInventoryTransformationRecord = $"SELECT i.NodeId, ip.ProductId FROM offchain.inventory i JOIN offchain.inventoryproduct ip ON i.InventoryTransactionId = ip.InventoryPrimaryKeyId WHERE i.InventoryId = @inventoryId";
        public static readonly string GetLastButOneTransformation = $"SELECT TOP 1 * From (SELECT TOP 2 * FROM [{DbCatalog}].[Admin].[Transformation] ORDER BY TransformationId DESC) x ORDER BY TransformationId";

        public static readonly string GetExceptionsByComment = $"SELECT * FROM [{DbCatalog}].[Admin].[PendingTransactionError] WHERE Comment = @comment ORDER BY lastmodifieddate DESC";

        public static readonly string GetHomologationBySourceAndDestinationForEvent = $"SELECT * FROM [{DbCatalog}].[Admin].[Homologation] WHERE SourceSystemId = 5 AND DestinationSystemId = 1";

        public static readonly string GetLastEvent = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Event] ORDER BY CREATEDDATE DESC";

        public static readonly string GetLatestUploadedEventRecordInFileUploadGrid = $"SELECT TOP 1 * from [{DbCatalog}].[Admin].[view_FileRegistrationStatus] WHERE SystemTypeId = 5 and CREATEDDATE > GETDATE()-30 ORDER BY CREATEDDATE DESC";

        public static readonly string GetAllUploadedRecordsInFileUploadGrid = $"SELECT * from [{DbCatalog}].[Admin].[FileRegistration] where SystemTypeId in (4,5) and CREATEDDATE > @startDate AND CREATEDDATE < @endDate and CREATEDDATE > GETDATE()-30 ORDER BY CREATEDDATE DESC";

        public static readonly string GetCompletedUploadedEventRecordInFileUploadGrid = $"Select TOP 1 CONVERT(VARCHAR(10), CreatedDate, 103) as CreatedDate,Name," +
                                                                               " CASE" +
                                                                               " WHEN SystemTypeId = 4 THEN 'Contratos de Compras y Ventas'" +
                                                                               " WHEN SystemTypeId = 5 THEN 'Planeación, Programación y Acuerdos'" +
                                                                               " END AS SystemTypeId,FileActionType,CreatedBy,Status,RecordsProcessed from[Admin].[view_FileRegistrationStatus]" +
                                                                               " WHERE SystemTypeId = 5 and Status = 'Finalizado' and CREATEDDATE > GETDATE()-30 ORDER BY CREATEDDATE DESC";

        public static readonly string GetProcessingUploadedEventRecordInFileUploadGrid = $"Select TOP 1 CONVERT(VARCHAR(10), CreatedDate, 103) as CreatedDate,Name," +
                                                                               " CASE" +
                                                                               " WHEN SystemTypeId = 4 THEN 'Contratos de Compras y Ventas'" +
                                                                               " WHEN SystemTypeId = 5 THEN 'Planeación, Programación y Acuerdos'" +
                                                                               " END AS SystemTypeId,FileActionType,CreatedBy,Status,RecordsProcessed from[Admin].[view_FileRegistrationStatus]" +
                                                                               " WHERE SystemTypeId = 5 and Status = 'Procesando' and CREATEDDATE > GETDATE()-30 ORDER BY CREATEDDATE DESC";

        public static readonly string GetFailedUploadedEventRecordInFileUploadGrid = $"Select TOP 1 CONVERT(VARCHAR(10), CreatedDate, 103) as CreatedDate,Name," +
                                                                               " CASE" +
                                                                               " WHEN SystemTypeId = 4 THEN 'Contratos de Compras y Ventas'" +
                                                                               " WHEN SystemTypeId = 5 THEN 'Planeación, Programación y Acuerdos'" +
                                                                               " END AS SystemTypeId,FileActionType,CreatedBy,Status,RecordsProcessed from[Admin].[view_FileRegistrationStatus]" +
                                                                               " WHERE SystemTypeId = 5 and Status = 'Fallido' and CREATEDDATE > GETDATE()-30 ORDER BY CREATEDDATE DESC";

        public static readonly string UpdateFileRegistrationIdwithProcessingStatus = $"UPDATE [{DbCatalog}].[Admin].[FileRegistration] SET STATUS=1, LastModifiedBy='System', LastModifiedDate=GETDATE() WHERE FileRegistrationId=@fileRegistrationId";

        public static readonly string UpdateFileRegistrationIdwithFailedStatus = $"UPDATE [{DbCatalog}].[Admin].[FileRegistration] SET STATUS=2, LastModifiedBy='System', LastModifiedDate=GETDATE() WHERE FileRegistrationId=@fileRegistrationId";

        public static readonly string UpdateFileRegistrationIdwithCompletedStatus = $"UPDATE [{DbCatalog}].[Admin].[FileRegistration] SET STATUS=0, LastModifiedBy='System', LastModifiedDate=GETDATE() WHERE FileRegistrationId=@fileRegistrationId";

        public static readonly string GetAllUploadedRecordsInFileUploadGridWithinDateRange = $"SELECT * from [{DbCatalog}].[Admin].[FileRegistration] where SystemTypeId in (4,5) and CreatedDate between @startDate and @endDate and CREATEDDATE > GETDATE()-30 ORDER BY CREATEDDATE DESC";

        public static readonly string GetLatestLogisticTicket = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Ticket] WHERE Tickettypeid=3 and CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";
        public static readonly string GetLogisticTickets = $"SELECT * FROM [{DbCatalog}].[Admin].[Ticket] WHERE Tickettypeid=3 and CREATEDDATE > GETDATE()-40";
        public static readonly string DeleteLogisticTickets = $"DELETE FROM [{DbCatalog}].[Admin].[Ticket] where TicketTypeId=3";
        public static readonly string GetProcessingLogisticTicket = $"SELECT TOP 1 * FROM Admin.Ticket WHERE TickettypeId=3 AND Status = 1 and CREATEDDATE > GETDATE()-40 ORDER BY ticketid DESC";
        public static readonly string GetErrorLogisticTicket = $"SELECT TOP 1 * FROM Admin.Ticket WHERE TickettypeId=3 AND Status = 2 and CREATEDDATE > GETDATE()-40 ORDER BY ticketid DESC";
        public static readonly string GetCompletedLogisticTicket = $"SELECT TOP 1 * FROM Admin.Ticket WHERE TickettypeId=3 AND Status = 0 and CREATEDDATE > GETDATE()-40 ORDER BY ticketid DESC";
        public static readonly string GetOfficialLogisticTicket = $"SELECT TOP 1 * FROM Admin.Ticket WHERE TickettypeId=6 AND Status = @status and CREATEDDATE > GETDATE()-40 ORDER BY ticketid DESC";
        public static readonly string GetTicketSortedBySegmentForReportLogisticGrid = $"SELECT TOP 1 CE.Name as Segment,CE1.Name as Owner,CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate, " +
                                                                            " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate, " +
                                                                            " T.CreatedBy as CreatedBy," +
                                                                            " CONVERT(VARCHAR(10), T.CreatedDate, 3) + ' ' + " +
                                                                            " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate, " +
                                                                            " CASE " +
                                                                            " WHEN T.[Status] = 0 THEN 'Finalizado' " +
                                                                            " WHEN T.[Status] = 1 THEN 'Procesando' " +
                                                                            " WHEN T.[Status] = 2 THEN 'Fallido' " +
                                                                            " END AS Status " +
                                                                            " FROM [{DbCatalog}].[Admin].[Ticket] T " +
                                                                            " join " +
                                                                            " [{DbCatalog}].[Admin].[CategoryElement] CE on T.CategoryElementId = CE.ElementId " +
                                                                            " join " +
                                                                            " [{DbCatalog}].[Admin].[CategoryElement] CE1 on T.OwnerId = CE1.ElementId" +
                                                                            " where T.Tickettypeid=3 and T.CREATEDDATE > GETDATE()-40" +
                                                                            " ORDER BY CE.Name DESC, T.CreatedDate DESC";

        public static readonly string GetTicketSortedByOwnerForReportLogisticGrid = $"SELECT TOP 1 CE.Name as Segment,CE1.Name as Owner,CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate, " +
                                                                            " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate, " +
                                                                            " T.CreatedBy as CreatedBy," +
                                                                            " CONVERT(VARCHAR(10), T.CreatedDate, 3) + ' ' + " +
                                                                            " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate, " +
                                                                            " CASE " +
                                                                            " WHEN T.[Status] = 0 THEN 'Finalizado' " +
                                                                            " WHEN T.[Status] = 1 THEN 'Procesando' " +
                                                                            " WHEN T.[Status] = 2 THEN 'Fallido' " +
                                                                            " END AS Status " +
                                                                            " FROM [{DbCatalog}].[Admin].[Ticket] T " +
                                                                            " join " +
                                                                            " [{DbCatalog}].[Admin].[CategoryElement] CE on T.CategoryElementId = CE.ElementId " +
                                                                            " join " +
                                                                            " [{DbCatalog}].[Admin].[CategoryElement] CE1 on T.OwnerId = CE1.ElementId" +
                                                                            " where T.Tickettypeid=3 and T.CREATEDDATE > GETDATE()-40" +
                                                                            " ORDER BY CE1.Name DESC, T.CreatedDate DESC";

        public static readonly string GetTicketSortedByStartDateForReportLogisticGrid = $"SELECT TOP 1 CE.Name as Segment,CE1.Name as Owner,CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate, " +
                                                                            " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate, " +
                                                                            " T.CreatedBy as CreatedBy," +
                                                                            " CONVERT(VARCHAR(10), T.CreatedDate, 3) + ' ' + " +
                                                                            " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate, " +
                                                                            " CASE " +
                                                                            " WHEN T.[Status] = 0 THEN 'Finalizado' " +
                                                                            " WHEN T.[Status] = 1 THEN 'Procesando' " +
                                                                            " WHEN T.[Status] = 2 THEN 'Fallido' " +
                                                                            " END AS Status " +
                                                                            " FROM [{DbCatalog}].[Admin].[Ticket] T " +
                                                                            " join " +
                                                                            " [{DbCatalog}].[Admin].[CategoryElement] CE on T.CategoryElementId = CE.ElementId " +
                                                                            " join " +
                                                                            " [{DbCatalog}].[Admin].[CategoryElement] CE1 on T.OwnerId = CE1.ElementId" +
                                                                            " where T.Tickettypeid=3 and T.CREATEDDATE > GETDATE()-40" +
                                                                            " ORDER BY T.StartDate DESC, T.CreatedDate DESC";

        public static readonly string GetTicketSortedByEndDateForReportLogisticGrid = $"SELECT TOP 1 CE.Name as Segment,CE1.Name as Owner,CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate, " +
                                                                            " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate, " +
                                                                            " T.CreatedBy as CreatedBy," +
                                                                            " CONVERT(VARCHAR(10), T.CreatedDate, 3) + ' ' + " +
                                                                            " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate, " +
                                                                            " CASE " +
                                                                            " WHEN T.[Status] = 0 THEN 'Finalizado' " +
                                                                            " WHEN T.[Status] = 1 THEN 'Procesando' " +
                                                                            " WHEN T.[Status] = 2 THEN 'Fallido' " +
                                                                            " END AS Status " +
                                                                            " FROM [{DbCatalog}].[Admin].[Ticket] T " +
                                                                            " join " +
                                                                            " [{DbCatalog}].[Admin].[CategoryElement] CE on T.CategoryElementId = CE.ElementId " +
                                                                            " join " +
                                                                            " [{DbCatalog}].[Admin].[CategoryElement] CE1 on T.OwnerId = CE1.ElementId" +
                                                                            " where T.Tickettypeid=3 and T.CREATEDDATE > GETDATE()-40" +
                                                                            " ORDER BY T.EndDate DESC, T.CreatedDate DESC";

        public static readonly string GetTicketSortedByTicketCreatedDateForReportLogisticGrid = $"SELECT TOP 1 CE.Name as Segment,CE1.Name as Owner,CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate, " +
                                                                                      " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate, " +
                                                                                      " T.CreatedBy as CreatedBy," +
                                                                                      " CONVERT(VARCHAR(10), T.CreatedDate, 3) + ' ' + " +
                                                                                      " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate, " +
                                                                                      " CASE " +
                                                                                      " WHEN T.[Status] = 0 THEN 'Finalizado' " +
                                                                                      " WHEN T.[Status] = 1 THEN 'Procesando' " +
                                                                                      " WHEN T.[Status] = 2 THEN 'Fallido' " +
                                                                                      " END AS Status " +
                                                                                      " FROM [{DbCatalog}].[Admin].[Ticket] T " +
                                                                                      " join " +
                                                                                      " [{DbCatalog}].[Admin].[CategoryElement] CE on T.CategoryElementId = CE.ElementId " +
                                                                                      " join " +
                                                                                      " [{DbCatalog}].[Admin].[CategoryElement] CE1 on T.OwnerId = CE1.ElementId" +
                                                                                      " where T.Tickettypeid=3 and T.CREATEDDATE > GETDATE()-40" +
                                                                                      " ORDER BY T.CreatedDate DESC";

        public static readonly string GetTicketSortedByUsernameForReportLogisticGrid = $"SELECT TOP 1 CE.Name as Segment,CE1.Name as Owner,CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate, " +
                                                                             " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate, " +
                                                                             " T.CreatedBy as CreatedBy," +
                                                                             " CONVERT(VARCHAR(10), T.CreatedDate, 3) + ' ' + " +
                                                                             " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate, " +
                                                                             " CASE " +
                                                                             " WHEN T.[Status] = 0 THEN 'Finalizado' " +
                                                                             " WHEN T.[Status] = 1 THEN 'Procesando' " +
                                                                             " WHEN T.[Status] = 2 THEN 'Fallido' " +
                                                                             " END AS Status " +
                                                                             " FROM [{DbCatalog}].[Admin].[Ticket] T " +
                                                                             " join " +
                                                                             " [{DbCatalog}].[Admin].[CategoryElement] CE on T.CategoryElementId = CE.ElementId " +
                                                                             " join " +
                                                                             " [{DbCatalog}].[Admin].[CategoryElement] CE1 on T.OwnerId = CE1.ElementId" +
                                                                             " where T.Tickettypeid=3 and T.CREATEDDATE > GETDATE()-40" +
                                                                             " ORDER BY T.CreatedBy DESC, T.CreatedDate DESC";

        public static readonly string GetTicketSortedByStatusForReportLogisticGrid = $"SELECT TOP 1 CE.Name as Segment,CE1.Name as Owner,CONVERT(VARCHAR(10), T.StartDate, 3) as StartDate, " +
                                                                            " CONVERT(VARCHAR(10), T.EndDate, 3) as EndDate, " +
                                                                            " T.CreatedBy as CreatedBy," +
                                                                            " CONVERT(VARCHAR(10), T.CreatedDate, 3) + ' ' + " +
                                                                            " CONVERT(VARCHAR(5), T.CreatedDate, 108)  as CreatedDate, " +
                                                                            " CASE " +
                                                                            " WHEN T.[Status] = 0 THEN 'Finalizado' " +
                                                                            " WHEN T.[Status] = 1 THEN 'Procesando' " +
                                                                            " WHEN T.[Status] = 2 THEN 'Fallido' " +
                                                                            " END AS Status " +
                                                                            " FROM [{DbCatalog}].[Admin].[Ticket] T " +
                                                                            " join " +
                                                                            " [{DbCatalog}].[Admin].[CategoryElement] CE on T.CategoryElementId = CE.ElementId " +
                                                                            " join " +
                                                                            " [{DbCatalog}].[Admin].[CategoryElement] CE1 on T.OwnerId = CE1.ElementId" +
                                                                            " where T.Tickettypeid=3 and T.CREATEDDATE > GETDATE()-40" +
                                                                            " ORDER BY T.Status DESC, T.CreatedDate DESC";

        public static readonly string GetSecondPageTicketsOfReportLogisticGrid = $"SELECT TOP 1 * FROM Admin.Ticket WHERE ticketid NOT IN (SELECT TOP 10 ticketid FROM Admin.Ticket WHERE TICKETTYPEID=3 ORDER BY CREATEDDATE DESC) AND TICKETTYPEID=3 and CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";

        public static readonly string GetLatestCompletedOwnershipTicket = $"SELECT TOP 1 TicketId, CategoryElementId, CONVERT(VARCHAR(10), StartDate, 103) as StartDate, CONVERT(VARCHAR(10), EndDate, 103) as EndDate FROM [{DbCatalog}].[Admin].[Ticket] WHERE errormessage IS NULL AND Status = 0 AND TickettypeId=2 and CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";

        public static readonly string GetSecondPageTicketsOfEventsFileUploadGrid = $"SELECT TOP 1 * from [{DbCatalog}].[Admin].[FileRegistration] where FileRegistrationId NOT IN (SELECT TOP 10 FileRegistrationId FROM [{DbCatalog}].[Admin].[FileRegistration]" +
                                                                         " WHERE SystemTypeId in (4,5) and CREATEDDATE > GETDATE()-30 ORDER BY CREATEDDATE DESC) AND SystemTypeId in (4,5)" +
                                                                         " and CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";

        public static readonly string GetLatestMovementRecord = $"SELECT TOP 1 * FROM [{DbCatalog}].[offchain].[movement] ORDER BY Operationaldate DESC";

        public static readonly string GetLatestOperativeMovementsPeriodicRecord = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[view_OperativeMovementsPeriodic] WHERE NetStandardVolume = @netStandardVolume ORDER BY Operationaldate DESC";

        public static readonly string GetLatestExpectedMovementsPeriodicRecord = $"SELECT	DISTINCT TOP 1" +
                                                                       " Mov.OperationalDate					AS	OperationalDate" +
                                                                       " ,Mov.DestinationNodeName			AS	DestinationNode" +
                                                                       " ,CeDestNodeType.Name				AS	DestinationNodeType" +
                                                                       " ,Mov.MovementTypeName				AS	MovementType" +
                                                                       " ,Mov.SourceNodeName					AS	SourceNode" +
                                                                       " ,CeSourceNodeType.Name				AS	SourceNodeType" +
                                                                       " ,Mov.SourceProductName				AS	SourceProduct" +
                                                                       " ,Ce.Name							AS	SourceProductType" +
                                                                       " ,Mov.NetStandardVolume				AS	NetStandardVolume" +
                                                                       " FROM Admin.view_MovementInformation Mov" +
                                                                       " INNER JOIN [Admin].[CategoryElement] Ce" +
                                                                       " ON Mov.[SourceProductTypeId] = Ce.ElementId" +
                                                                       " INNER JOIN [Admin].[NodeTag] NTSource" +
                                                                       " ON NTSource.NodeId = Mov.SourceNodeId" +
                                                                       " INNER JOIN [Admin].[NodeTag] NTDest" +
                                                                       " ON NTDest.NodeId = Mov.DestinationNodeId" +
                                                                       " INNER JOIN [Admin].[CategoryElement] CeSourceNodeType" +
                                                                       " ON CeSourceNodeType.ElementId = NTSource.ElementId" +
                                                                       " INNER JOIN [Admin].[CategoryElement] CeDestNodeType" +
                                                                       " ON CeDestNodeType.ElementId = NTDest.ElementId" +
                                                                       " WHERE Ce.CategoryId = 11" +
                                                                       " AND CeSourceNodeType.CategoryId = 1" +
                                                                       " AND CeDestNodeType.CategoryId = 1" +
                                                                       " AND NetStandardVolume = @netStandardVolume" +
                                                                       " ORDER BY Operationaldate DESC";

        public static readonly string GetEventPendingTransactionError = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[PendingTransaction] WHERE MessageTypeId =9 ORDER BY CreatedDate DESC";

        public static readonly string GetExceptionDetails = $"SELECT PT.SystemName as OriginSystem,CONVERT(VARCHAR(10), PT.CreatedDate, 5) + ' ' + CONVERT(VARCHAR(5), PT.CreatedDate, 108) as CreatedDate,PTE.ErrorMessage as Error,PT.Identifier as Identifier," +
                                                  " CASE" +
                                                  " WHEN PT.ActionTypeId = 1 THEN 'Insertar'" +
                                                  " WHEN PT.ActionTypeId = 2 THEN 'Actualizar'" +
                                                  " WHEN PT.ActionTypeId = 3 THEN 'Eliminar'" +
                                                  " END AS Type," +
                                                  " CE.Name as Segment," +
                                                  " CASE" +
                                                  " WHEN PT.MessageTypeId = 1 THEN 'Registro de movimientos'" +
                                                  " WHEN PT.MessageTypeId = 4 THEN 'Registro de inventarios'" +
                                                  " WHEN PT.MessageTypeId = 9 THEN 'Eventos planeación, programación y acuerdos'" +
                                                  " END AS Process," +
                                                  " PT.Volume as Volume,CE1.Name as Units,CONVERT(VARCHAR(10), PT.StartDate, 5) as StartDate,CONVERT(VARCHAR(10), PT.EndDate, 5) as EndDate,FR.Name as FileName," +
                                                  " N.Name as OriginNode," +
                                                  " CASE" +
                                                  " WHEN PT.DestinationNodeId IS NOT NULL THEN N1.Name" +
                                                  " WHEN PT.DestinationNodeId IS NULL THEN NULL" +
                                                  " END AS DestinationNode," +
                                                  " P.Name as OriginProduct," +
                                                  " CASE" +
                                                  " WHEN PT.DestinationProductId IS NOT NULL THEN P1.Name" +
                                                  " WHEN PT.DestinationProductId IS NULL THEN NULL" +
                                                  " END AS DestinationProduct" +
                                                  " FROM Admin.PendingTransaction PT" +
                                                  " JOIN Admin.PendingTransactionError PTE ON PT.TransactionId=PTE.TransactionId" +
                                                  " JOIN Admin.FileRegistration FR ON PT.MessageId = FR.UploadId" +
                                                  " JOIN Admin.CategoryElement CE ON PT.SegmentId = CE.ElementId" +
                                                  " JOIN Admin.CategoryElement CE1 ON PT.Units = CE1.ElementId" +
                                                  " JOIN Admin.Node N ON PT.SourceNodeId = N.NodeId" +
                                                  " JOIN Admin.Node N1 ON PT.DestinationNodeId = N1.NodeId" +
                                                  " JOIN Admin.Product P ON PT.SourceProductId = P.ProductId" +
                                                  " JOIN Admin.Product P1 ON PT.DestinationProductId = P1.ProductId" +
                                                  " WHERE PTE.ErrorId = @errorId";

        public static readonly string GetAllNodeTagsForGivenNodeId = $"SELECT * FROM [{DbCatalog}].[ADMIN].[NODETAG] NT JOIN [{DbCatalog}].[ADMIN].[CategoryElement] CE ON NT.ElementId = CE.ElementId WHERE NT.NODEID=@nodeId and CE.CategoryId=2";

        public static readonly string UpdateElementIdInNodeTag = $"UPDATE [{DbCatalog}].[Admin].[NodeTag] SET ElementId = @elementId, StartDate=@startDate, EndDate=@endDate, LastModifiedBy='System', LastModifiedDate=GETDATE() WHERE NodeTagId=@nodeTagId";

        public static readonly string GetLastTicketAndElement = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Ticket] T JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryElementId = CE.ElementId WHERE T.Tickettypeid=2 and T.CREATEDDATE > GETDATE()-40 ORDER BY T.TicketId DESC";

        public static readonly string GetTopProcessedEvent = $"Select TOP 1 * from [{DbCatalog}].[Admin].[Event] where EndDate BETWEEN GETDATE()-1 AND GETDATE() ORDER BY CREATEDDATE DESC";

        public static readonly string LastHomologationGroupId = $"SELECT TOP 1 HomologationGroupId from [{DbCatalog}].[Admin].[HomologationGroup] ORDER BY CreatedDate DESC";
        public static readonly string LastHomologationObjectId = $"SELECT TOP 1 HomologationObjectId FROM [{DbCatalog}].[Admin].[HomologationObject] ORDER BY CreatedDate DESC";
        public static readonly string LastHomologationDataMappingId = $"SELECT TOP 1 HomologationDataMappingId FROM [{DbCatalog}].[Admin].[HomologationDataMapping] ORDER BY CreatedDate DESC";
        public static readonly string GetNewNodeTag = $"SELECT * FROM [{DbCatalog}].[Admin].[NodeTag] WHERE NodeId = @nodeId AND ElementId = 1";
        public static readonly string GetInventoryBySegmentId = $"SELECT TOP 1 * FROM [{DbCatalog}].[Offchain].[InventoryProduct] WHERE SegmentId = @segmentId ORDER BY InventoryProductID DESC";
        public static readonly string GetInventoryBySegmentIdWithOrder = $"SELECT TOP 1 * FROM [{DbCatalog}].[Offchain].[InventoryProduct] WHERE SegmentId = @segmentId ORDER BY InventoryProductID";
        public static readonly string GetMovementBySegmentId = $"SELECT TOP 1 * FROM [{DbCatalog}].[Offchain].[Movement] WHERE SegmentId = @segmentId ORDER BY MovementTransactionId DESC";
        public static readonly string GetMovementBySegmentIdWithInsertEvent = $"SELECT TOP 1 * FROM [{DbCatalog}].[Offchain].[Movement] WHERE SegmentId = @segmentId and EventType = 'Insert' ORDER BY MovementTransactionId DESC";
        public static readonly string GetMovementSource = $"SELECT TOP 1 * FROM [{DbCatalog}].[Offchain].[MovementSource] WHERE MovementTransactionId = @movementTransactionId ORDER BY CREATEDDATE DESC";
        public static readonly string GetMovementDestination = $"SELECT TOP 1 * FROM [{DbCatalog}].[Offchain].[MovementDestination] WHERE MovementTransactionId = @movementTransactionId ORDER BY CREATEDDATE DESC";

        public static readonly string GetOperativeMovements = $"SELECT TOP 1 * FROM [{DbCatalog}].[Analytics].[OperativeMovements]";
        public static readonly string GetOperativeMovementsWithOwnership = $"SELECT TOP 1 * FROM [{DbCatalog}].[Analytics].[OperativeMovementswithOwnership]";
        public static readonly string GetOperativeNodeRelationship = $"SELECT TOP 1 * FROM [{DbCatalog}].[Analytics].[OperativeNodeRelationship]";
        public static readonly string GetOperativeNodeRelationshipWithOwnership = $"SELECT TOP 1 * FROM [{DbCatalog}].[Analytics].[OperativeNodeRelationshipWithOwnership]";
        public static readonly string GetOperativeMovementsCount = $"SELECT COUNT(*) FROM [{DbCatalog}].[Analytics].[OperativeMovements]";
        public static readonly string GetOperativeMovementsWithOwnershipCount = $"SELECT COUNT(*) FROM [{DbCatalog}].[Analytics].[OperativeMovementswithOwnership]";
        public static readonly string GetOperativeNodeRelationshipCount = $"SELECT COUNT(*) FROM [{DbCatalog}].[Analytics].[OperativeNodeRelationship]";
        public static readonly string GetOperativeNodeRelationshipWithOwnershipCount = $"SELECT COUNT(*) FROM [{DbCatalog}].[Analytics].[OperativeNodeRelationshipWithOwnership]";
        public static readonly string GetOperativeNodeRelationshipWithOwnershipOnLogisticSourceCenter = $"SELECT COUNT(*) FROM [{DbCatalog}].[Analytics].[OperativeNodeRelationshipWithOwnership] WHERE LogisticSourceCenter= @sourceCenter";
        public static readonly string GetOwnerId = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE categoryId = 7";
        public static readonly string GetLastFileRegistrationId = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[FileRegistration] ORDER BY FileRegistrationId DESC";
        public static readonly string UpdateOwnershipStatusId = $"UPDATE [{DbCatalog}].[Admin].[OwnershipNode] SET OwnershipStatusId = (SELECT OwnershipNodeStatusTypeId FROM [{DbCatalog}].[Admin].OwnershipNodestatusType WHERE Name = @name) WHERE TicketId = @ticketId";
        public static readonly string UpdateStatusWithSuccessful = $"UPDATE [{DbCatalog}].[Admin].[Ticket] SET Status = (SELECT OwnershipNodeStatusTypeId FROM [{DbCatalog}].[Admin].OwnershipNodestatusType WHERE Name = 'Propiedad') WHERE TicketId = @ticketId";
        public static readonly string GetTicketIdOfPreviousRecord = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[OwnerShipNode] WHERE Ticketid NOT IN (SELECT TOP 1 ticketid FROM [{DbCatalog}].[Admin].[OwnerShipNode] WHERE CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC) AND CREATEDDATE > GETDATE()-40 ORDER BY CREATEDDATE DESC";
        public static readonly string GetLastUpdatedNode = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Node] order by LastModifiedDate DESC";
        public static readonly string GetProductByNodeStorageLocation = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[StorageLocationProduct] WHERE NodeStroageLocationId = @nodeStroageLocationId";
        public static readonly string GetProductNameById = $"SELECT * FROM [{DbCatalog}].[Admin].[Product] WHERE ProductId = @productId";
        public static readonly string GetUnabalnceByTicketId = $"SELECT * FROM [{DbCatalog}].[Admin].[Unbalance] WHERE TicketId = @ticketId";
        public static readonly string GetNodeConnectionProductByNodeConnnectionID = $"SELECT * FROM [{DbCatalog}].[Admin].[NodeConnectionProduct] WHERE NodeConnectionId= @nodeConnectionId";
        public static readonly string GetLastUpdatedTransformation = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Transformation] order by LastModifiedDate DESC";
        public static readonly string GetHomologationBySourceAndDestinationForContracts = $"SELECT * FROM [{DbCatalog}].[Admin].[Homologation] WHERE SourceSystemId = 4 AND DestinationSystemId = 1";
        public static readonly string GetHomologationBySourceAndDestinationForSAP = $"SELECT * FROM [{DbCatalog}].[Admin].[Homologation] WHERE SourceSystemId = 7 AND DestinationSystemId = 1";
        public static readonly string GetOwnerShipNodeByTicket = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[OwnershipNode] WHERE TicketId = @ticketid";
        public static readonly string GetLastContractId = $"SELECT TOP 1 DocumentNumber from [{DbCatalog}].[Admin].[Contract] WHERE DocumentNumber = @documentNumber ORDER BY CreatedDate DESC";
        public static readonly string GetLastTransactionError = $"SELECT TOP 1 ErrorMessage FROM [{DbCatalog}].[Admin].[PendingTransactionError] ORDER BY CreatedDate DESC";
        public static readonly string UpdateAcceptableBalancePercentage = $"UPDATE [{DbCatalog}].[Admin].[Node] SET AcceptableBalancePercentage = @acceptableBalancePercentage,LastModifiedBy='System' where NodeId = @nodeId";
        public static readonly string UpdateOwnershipNodeState = $"UPDATE [{DbCatalog}].[Admin].[OwnershipNode] SET OwnershipStatusId = (SELECT OwnershipNodeStatusTypeId from [{DbCatalog}].[Admin].[OwnershipNodeStatusType] WHERE [NAME] = @name) where NodeId = @nodeId and TicketId = @ticketId";
        public static readonly string UpdateInputVolumeForNodeWithZero = $"Update [{DbCatalog}].[Admin].[OwnershipCalculation] SET InputVolume = 0 Where ownershipticketid = @ticketId  and nodeid= @nodeId";
        public static readonly string GetEditOwnershipNodeCalculationInformation = $"DECLARE @tempTable AS TABLE" +
                                                              " (ProductId NVARCHAR(60)," +
                                                              " Product  NVARCHAR(50)," +
                                                              " Owner NVARCHAR(50)," +
                                                              " InitialInventory DECIMAL(29,2), Inputs DECIMAL(29,2), Outputs DECIMAL(29,2), IdentifiedLosses DECIMAL(29,2), Interface DECIMAL(29,2), Tolerance DECIMAL(29,2)," +
                                                              " UnidentifiedLosses DECIMAL(29,2), FinalInventory DECIMAL(29,2), Volume DECIMAL(29,2), MeasurementUnit NVARCHAR(10), Control DECIMAL(29,2))" +
                                                              " INSERT INTO  @tempTable" +
                                                              " EXEC [{DbCatalog}].[Admin].[usp_BalanceSummaryWithOwnership] @ownershipNodeId" +
                                                              " SELECT Sum(InitialInventory) as InitialInventory,Sum(Inputs) as Inputs,Sum(Outputs) as Outputs," +
                                                              " Sum(IdentifiedLosses) as IdentifiedLosses,Sum(Interface) as Interface,Sum(Tolerance)as Tolerance," +
                                                              " Sum(UnidentifiedLosses) as UnidentifiedLosses,Sum(FinalInventory) as FinalInventory," +
                                                              " Sum(Volume)as Volume,'Bbl' as MeasurementUnit,Sum([Control]) as [Control] from @tempTable" +
                                                              " and M.ContractId IS NOT NULL AND M.MovementTypeId IS NOT NULL AND M.NetStandardVolume IS NOT NULL AND " +
                                                              " M.MeasurementUnit IS NOT NULL AND M.SourceSystem IS NOT NULL AND  M.OperationalDate IS NOT NULL AND " +
                                                              " O.OwnerId IS NOT NULL AND  O.OwnershipValue IS NOT NULL AND O.OwnershipValueUnit IS NOT NULL AND M.SegmentId IS NOT NULL";

        public static readonly string GetOwnershipNodeId = $"SELECT OWNERSHIPNODEID FROM [{DbCatalog}].[Admin].[OwnershipNode] WHERE TicketId = @ticketId AND NodeId = @nodeId";
        public static readonly string GetMovementOfContractDaily = $"SELECT M.ContractId,MS.SourceNodeId,MD.DestinationNodeId,MS.SourceProductId,MD.DestinationProductId, M.MovementTypeId," +
                                                         " M.NetStandardVolume, M.MeasurementUnit,M.SourceSystem, M.OperationalDate, O.OwnerId, O.OwnershipValue,O.OwnershipValueUnit," +
                                                         $" M.SegmentId FROM [{DbCatalog}].[Offchain].[Movement] M " +
                                                         $" JOIN [{DbCatalog}].[Admin].[Contract] C ON M.ContractId = C.ContractId" +
                                                         $" JOIN [{DbCatalog}].[Offchain].[Owner] O ON M.MovementTransactionId = O.MovementTransactionId" +
                                                         $" LEFT JOIN [{DbCatalog}].[offchain].[MovementSource] MS ON M.MovementTransactionId = MS.MovementTransactionId" +
                                                         $" LEFT JOIN [{DbCatalog}].[offchain].[MovementDestination] MD ON M.MovementTransactionId = MD.MovementTransactionId" +
                                                         $" WHERE M.ContractId in (Select Top 1 ContractId from[{DbCatalog}].[Admin].[Contract] CO where CO.Frequency = @frequency" +
                                                         $" and CO.StartDate <= Convert(date,Admin.udf_GETTRUEDATE()) and CO.EndDate >= Convert(date,Admin.udf_GETTRUEDATE())) " +
                                                         $" and M.OperationalDate = Convert(date,Admin.udf_GETTRUEDATE())" +
                                                         $" and M.ContractId IS NOT NULL AND M.MovementTypeId IS NOT NULL AND M.NetStandardVolume IS NOT NULL AND " +
                                                         $" M.MeasurementUnit IS NOT NULL AND M.SourceSystem IS NOT NULL AND  M.OperationalDate IS NOT NULL AND " +
                                                         $" O.OwnerId IS NOT NULL AND  O.OwnershipValue IS NOT NULL AND O.OwnershipValueUnit IS NOT NULL AND M.SegmentId IS NOT NULL";

        public static readonly string GetMovementOfContractWeekly = $"SELECT M.ContractId,MS.SourceNodeId,MD.DestinationNodeId,MS.SourceProductId,MD.DestinationProductId, M.MovementTypeId," +
                                                          " M.NetStandardVolume, M.MeasurementUnit,M.SourceSystem, M.OperationalDate, O.OwnerId, O.OwnershipValue,O.OwnershipValueUnit," +
                                                          " M.SegmentId FROM [{DbCatalog}].[Offchain].[Movement] M " +
                                                          " JOIN [{DbCatalog}].[Admin].[Contract] C ON M.ContractId = C.ContractId" +
                                                          " JOIN [{DbCatalog}].[Offchain].[Owner] O ON M.MovementTransactionId = O.MovementTransactionId" +
                                                          " LEFT JOIN [{DbCatalog}].[offchain].[MovementSource] MS ON M.MovementTransactionId = MS.MovementTransactionId" +
                                                          " LEFT JOIN [{DbCatalog}].[offchain].[MovementDestination] MD ON M.MovementTransactionId = MD.MovementTransactionId" +
                                                          " WHERE M.ContractId in (Select Top 1 ContractId from[{DbCatalog}].[Admin].[Contract] CO where CO.Frequency = @frequency" +
                                                          " and CO.StartDate <= Convert(date,Admin.udf_GETTRUEDATE()) and CO.EndDate >= Convert(date,Admin.udf_GETTRUEDATE())) " +
                                                          " and (Convert(varchar(10), M.OperationalDate, 105) = @firstWeek or Convert(varchar(10), M.OperationalDate, 105) = @secondWeek or" +
                                                          " Convert(varchar(10), M.OperationalDate, 105) = @thirdWeek or Convert(varchar(10), M.OperationalDate, 105) = @lastWeek or" +
                                                          " Convert(varchar(10), M.OperationalDate, 105) = @lastDayOfMonth or Convert(varchar(10), M.OperationalDate, 105) = @febLastDay or Convert(varchar(10), M.OperationalDate, 105) = @leapYearFebLastDay)" +
                                                          " and M.ContractId IS NOT NULL AND M.MovementTypeId IS NOT NULL AND M.NetStandardVolume IS NOT NULL AND " +
                                                          " M.MeasurementUnit IS NOT NULL AND M.SourceSystem IS NOT NULL AND  M.OperationalDate IS NOT NULL AND " +
                                                          " O.OwnerId IS NOT NULL AND  O.OwnershipValue IS NOT NULL AND O.OwnershipValueUnit IS NOT NULL AND M.SegmentId IS NOT NULL";

        public static readonly string GetMovementOfContractBiWeekly = $"SELECT M.ContractId,MS.SourceNodeId,MD.DestinationNodeId,MS.SourceProductId,MD.DestinationProductId, M.MovementTypeId," +
                                                            " M.NetStandardVolume, M.MeasurementUnit,M.SourceSystem, M.OperationalDate, O.OwnerId, O.OwnershipValue,O.OwnershipValueUnit," +
                                                            " M.SegmentId FROM [{DbCatalog}].[Offchain].[Movement] M " +
                                                            " JOIN [{DbCatalog}].[Admin].[Contract] C ON M.ContractId = C.ContractId" +
                                                            " JOIN [{DbCatalog}].[Offchain].[Owner] O ON M.MovementTransactionId = O.MovementTransactionId" +
                                                            " LEFT JOIN [{DbCatalog}].[offchain].[MovementSource] MS ON M.MovementTransactionId = MS.MovementTransactionId" +
                                                            " LEFT JOIN [{DbCatalog}].[offchain].[MovementDestination] MD ON M.MovementTransactionId = MD.MovementTransactionId" +
                                                            " WHERE M.ContractId in (Select Top 1 ContractId from[{DbCatalog}].[Admin].[Contract] CO where CO.Frequency = @frequency" +
                                                            " and CO.StartDate <= Convert(date,Admin.udf_GETTRUEDATE()) and CO.EndDate >= Convert(date,Admin.udf_GETTRUEDATE())) " +
                                                            " and (Convert(varchar(10), M.OperationalDate, 105) = @lastDayOfMonth or Convert(varchar(10), M.OperationalDate, 105) = @febLastDay or" +
                                                            " Convert(varchar(10), M.OperationalDate, 105) = @thirdWeek or Convert(varchar(10), M.OperationalDate, 105) = @lastWeek or Convert(varchar(10), M.OperationalDate, 105) = @leapYearFebLastDay)" +
                                                            " and M.ContractId IS NOT NULL AND M.MovementTypeId IS NOT NULL AND M.NetStandardVolume IS NOT NULL AND " +
                                                            " M.MeasurementUnit IS NOT NULL AND M.SourceSystem IS NOT NULL AND  M.OperationalDate IS NOT NULL AND " +
                                                            " O.OwnerId IS NOT NULL AND  O.OwnershipValue IS NOT NULL AND O.OwnershipValueUnit IS NOT NULL AND M.SegmentId IS NOT NULL";

        public static readonly string GetMovementOfContractMonthly = $"SELECT M.ContractId,MS.SourceNodeId,MD.DestinationNodeId,MS.SourceProductId,MD.DestinationProductId, M.MovementTypeId," +
                                                           " M.NetStandardVolume, M.MeasurementUnit,M.SourceSystem, M.OperationalDate, O.OwnerId, O.OwnershipValue,O.OwnershipValueUnit," +
                                                           " M.SegmentId FROM [{DbCatalog}].[Offchain].[Movement] M " +
                                                           " JOIN [{DbCatalog}].[Admin].[Contract] C ON M.ContractId = C.ContractId" +
                                                           " JOIN [{DbCatalog}].[Offchain].[Owner] O ON M.MovementTransactionId = O.MovementTransactionId" +
                                                           " LEFT JOIN [{DbCatalog}].[offchain].[MovementSource] MS ON M.MovementTransactionId = MS.MovementTransactionId" +
                                                           " LEFT JOIN [{DbCatalog}].[offchain].[MovementDestination] MD ON M.MovementTransactionId = MD.MovementTransactionId" +
                                                           " WHERE M.ContractId in (Select Top 1 ContractId from[{DbCatalog}].[Admin].[Contract] CO where CO.Frequency = @frequency" +
                                                           " and CO.StartDate <= Convert(date,Admin.udf_GETTRUEDATE()) and CO.EndDate >= Convert(date,Admin.udf_GETTRUEDATE())) " +
                                                           " and (Convert(varchar(10), M.OperationalDate, 105) = @lastDayOfMonth or Convert(varchar(10), M.OperationalDate, 105) = @febLastDay or" +
                                                           " Convert(varchar(10), M.OperationalDate, 105) = @leapYearFebLastDay or Convert(varchar(10), M.OperationalDate, 105) = @lastWeek)" +
                                                           " and M.ContractId IS NOT NULL AND M.MovementTypeId IS NOT NULL AND M.NetStandardVolume IS NOT NULL AND " +
                                                           " M.MeasurementUnit IS NOT NULL AND M.SourceSystem IS NOT NULL AND  M.OperationalDate IS NOT NULL AND " +
                                                           " O.OwnerId IS NOT NULL AND  O.OwnershipValue IS NOT NULL AND O.OwnershipValueUnit IS NOT NULL AND M.SegmentId IS NOT NULL";

        public static readonly string GetNodeConnectionByNodeName = $"SELECT NC.NodeConnectionId,NCP.NodeConnectionProductId,Ncp.ProductId,NC.SourceNodeId,Nc.DestinationNodeId,NOD.NodeId FROM ADMIN.NodeConnectionProduct NCP INNER JOIN ADMIN.NodeConnection NC ON NCP.NodeConnectionId = NC.NodeConnectionId INNER JOIN ADMIN.Node nod ON NOD.NodeId = NC.SOURCENODEID WHERE NOD.Name = @nodeName";
        public static readonly string GetHomologationBySourceAndDestinationForSIV = $"SELECT * FROM [{DbCatalog}].[Admin].[Homologation] WHERE SourceSystemId=1 AND DestinationSystemId=6";
        public static readonly string GetExcelFileNameInFileRegistration = $"select Top 1 * from {DbCatalog}.Admin.FileRegistration WHERE CREATEDDATE > GETDATE()-40 and SegmentId != 10 ORDER BY CREATEDDATE DESC";
        public static readonly string GetEAllExcelFileNamesInFileRegistration = $"select * from [{DbCatalog}].[Admin].[FileRegistration] WHERE CREATEDDATE > GETDATE()-40 and SegmentId != 10 ORDER BY CREATEDDATE DESC";
        public static readonly string CategoryElementCountforCategory = $"Select Count(*) from[Admin].[CategoryElement] where CategoryId = (Select CategoryId from[Admin].[Category] where Name = @CategoryElement)";
        public static readonly string GetCategoryByCategoryName = $"SELECT * FROM [{DbCatalog}].[Admin].[Category] WHERE Name = @categoryName";
        public static readonly string GetIconCount = $"SELECT COUNT(*) FROM [{DbCatalog}].[Admin].[Icon]";
        public static readonly string GetIconByName = $"SELECT * FROM [{DbCatalog}].[Admin].[Icon] WHERE Name = @iconName";
        public static readonly string GetCategoryElementByName = $"SELECT * FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE Name = @name";
        public static readonly string DeleteCategoryElementByName = $"DELETE FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE Name = @name";
        public static readonly string DeletePreviouslyloadAnalticsDataWithOwnership = $"DELETE from [{DbCatalog}].[Analytics].[OperativeMovementsWithOwnership] where MovementType like '%HistoricalMovements%'";
        public static readonly string DeletePreviouslyloadAnalticsDataWithoutOwnership = $"DELETE from [{DbCatalog}].[Analytics].[OperativeMovements] where DestinationNode like '%HistoricalMovements%'";
        public static readonly string GetTopInputConnectionsToNode = $"Select TOP 1 [NodeId],[Name] from [{DbCatalog}].[Admin].[Node] where NodeId in (Select SourceNodeid from[{DbCatalog}].[Admin].[NodeConnection] where DestinationNodeId = @nodeId)";
        public static readonly string GetTopOutputConnectionsToNode = $"Select TOP 1 [NodeId],[Name] from [{DbCatalog}].[Admin].[Node] where NodeId in (Select DestinationNodeId from [{DbCatalog}].[Admin].[NodeConnection] where SourceNodeId=@nodeId)";
        public static readonly string GetAllInputConnectionsToNode = $"Select [NodeId],[Name] from [{DbCatalog}].[Admin].[Node] where NodeId in (Select SourceNodeid from[{DbCatalog}].[Admin].[NodeConnection] where DestinationNodeId = @nodeId)";
        public static readonly string GetAllOutputConnectionsToNode = $"Select [NodeId],[Name] from [{DbCatalog}].[Admin].[Node] where NodeId in (Select DestinationNodeId from [{DbCatalog}].[Admin].[NodeConnection] where SourceNodeId=@nodeId)";
        public static readonly string GetProductsForGivenNode = $"Select Name from [{DbCatalog}].[Admin].[Product] where ProductId in (Select ProductId from [{DbCatalog}].[Admin].[StorageLocationProduct] where NodeStorageLocationId in (Select NodeStorageLocationId from [{DbCatalog}].[Admin].[NodeStorageLocation] where NodeId = @nodeId))";
        public static readonly string GetAllUnits = $"Select Name from [{DbCatalog}].[Admin].[CategoryElement] where Categoryid=6 order by elementid";
        public static readonly string GetAllMovementTypes = $"Select Name from [{DbCatalog}].[Admin].[CategoryElement] where Categoryid=9 order by elementid";
        public static readonly string GetAllReasonForChanges = $"Select Name from [{DbCatalog}].[Admin].[CategoryElement] where Categoryid=16 order by elementid";
        public static readonly string GetOwnershipNodeStatus = $"Select TOP 1 ONS.Name from [{DbCatalog}].[Admin].[OwnershipNode] O" +
                                                     " JOIN[{DbCatalog}].[Admin].[OwnershipNodeStatusType] ONS" +
                                                     " ON O.Ownershipstatusid = ONS.OwnershipNodeStatusTypeId" +
                                                     " Where O.NodeId = @nodeId Order by O.CreatedDate desc";

        public static readonly string GetTopProcessedCutoffTicket = $"SELECT TOP 1 CONVERT(VARCHAR(10), T.CreatedDate, 105) as ExecutionDate, CONVERT(VARCHAR(10), T.StartDate, 105) as StartDate, CE.Name,CONVERT(VARCHAR(10), T.EndDate, 105) as EndDate from [{DbCatalog}].[Admin].[Ticket] T JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryELementId = CE.ElementId where T.TicketTypeId = 1 and T.Status = 0 and T.CreatedDate < GETDATE()-2 and CE.Name not like '%Automation%' order by T.TicketId desc";
        public static readonly string GetTopPendingTransaction = $"SELECT TOP 1 * from [{DbCatalog}].[Admin].[PendingTransaction] order by createddate desc";

        public static readonly string GetOwnershipCalculationSegmentTicket = $"SELECT TOP 1 T.TicketId,CE.Name as OwnershipSegmentName,ST.StatusType as StatusName," +
                                                                   " FORMAT (T.StartDate, 'd','us') as StartDate,FORMAT (T.EndDate, 'd','us') as EndDate from [{DbCatalog}].[Admin].[Ticket] T" +
                                                                   " JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryElementId = CE.ElementId" +
                                                                   " JOIN [{DbCatalog}].[Admin].[StatusType] ST ON T.[Status] = ST.StatusTypeId" +
                                                                   " WHERE T.TicketTypeId = 2 and T.Status = 0 AND T.CREATEDDATE > GETDATE() - 40 ORDER BY T.CREATEDDATE";

        public static readonly string GetSAPDetailsOfANode = $"Select TOP 1 N.Name, N.SendToSAP from [{DbCatalog}].[Admin].[CategoryElement] CE" +
                                                   " JOIN [{DbCatalog}].[Admin].[NodeTag] NT ON CE.ElementId = NT.ElementId" +
                                                   " JOIN [{DbCatalog}].[Admin].[Node] N ON NT.NodeId = N.NodeId" +
                                                   " WHERE CE.Name = @segment AND N.SendToSAP = 1";

        public static readonly string GetNodeBySegment = $"Select TOP 1 N.Name, N.SendToSAP from [{DbCatalog}].[Admin].[CategoryElement] CE" +
                                               " JOIN [{DbCatalog}].[Admin].[NodeTag] NT ON CE.ElementId = NT.ElementId" +
                                               " JOIN [{DbCatalog}].[Admin].[Node] N ON NT.NodeId = N.NodeId" +
                                               " WHERE CE.Name = @segment";

        public static readonly string UpdateNodeSendToSAPStatus = $"UPDATE [{DbCatalog}].[Admin].[Node] SET SendToSAP = @status,LastModifiedBy= 'System' WHERE Name = @name";

        public static readonly string NodeOwnershipRules = $"SELECT RuleName as estrategia , RuleId as idEstrategia from [{DbCatalog}].Admin.NodeOwnershipRule where isActive = 1 order by RuleId";

        public static readonly string ProductOwnershipRules = $"SELECT RuleName as estrategia , RuleId as idEstrategia from [{DbCatalog}].Admin.NodeProductRule where isActive = 1 order by RuleId";

        public static readonly string ConnectionOwnershipRules = $"SELECT RuleName as estrategia , RuleId as idEstrategia from [{DbCatalog}].Admin.NodeConnectionProductRule where isActive = 1 order by RuleId";

        public static readonly string UpdateNodeApprovalStatus = $"UPDATE [{DbCatalog}].[Admin].[OwnershipNode] SET OwnershipStatusId = @status WHERE TicketId = @ticketId";

        public static readonly string RangeIsMoreThan60DaysForOwnershipCalculatedSegment = $"SELECT TOP 1 T.TicketId,CE.Name as OwnershipSegmentName,ST.StatusType as StatusName," +
                                                                                 " FORMAT (DATEADD(DAY, -61, T.StartDate), 'd','us') as StartDate,FORMAT (T.EndDate, 'd','us') as EndDate from [{DbCatalog}].[Admin].[Ticket] T" +
                                                                                 " JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryElementId = CE.ElementId" +
                                                                                 " JOIN [{DbCatalog}].[Admin].[StatusType] ST ON T.[Status] = ST.StatusTypeId" +
                                                                                 " WHERE T.TicketTypeId = 2 and T.Status = 0 AND T.CREATEDDATE > GETDATE() - 40 ORDER BY T.CREATEDDATE";

        public static readonly string GetOwnershipStrategyInformation = $"SELECT TOP 1 RuleName AS OwnershipStrategyInformation from[{DbCatalog}].[Admin].[NodeOwnershipRule] WHERE IsActive = 1";

        public static readonly string GetLastNodeName = $"Select Name from [{DbCatalog}].[Admin].[Node] where NodeId =@nodeId";
        public static readonly string GetOperativeMovementsCountwithHistoricalInformation = $"SELECT COUNT(*) FROM [{DbCatalog}].[Analytics].[OperativeMovements] where DestinationNode like '%HistoricalMovements%' and OperationalDate between @startDate and @endDate and SourceSystem='CSV' and cast(LoadDate as Date) = cast(getdate() as Date)";
        public static readonly string GetOperativeMovementsWithOwnershipCountwithHistoricalInformation = $"SELECT COUNT(*) FROM [{DbCatalog}].[Analytics].[OperativeMovementsWithOwnership] where MovementType like '%HistoricalMovements%' and OperationalDate between @startDate and @endDate and SourceSystem='CSV' and cast(LoadDate as Date) = cast(getdate() as Date)";

        public static readonly string GetOperativeMovementsCountwithdifferentHistoricalInformation = $"SELECT TOP 5 OperationalDate as [STARTTICK_DT], DestinationNode as [DESTINO] , DestinationNodeType as [DESTINO_TP]," +
                                                                                           "MovementType as [MOVEMENT_TP], SourceNode as [ORIGEN],SourceNodeType as [ORIGEN_TP],SourceProduct as [PRODUCTO],SourceProductType as [PRODUCTO_TP],TransferPoint as [RELACION_TT]," +
                                                                                           "FieldWaterProduction as [Agua_Campo],SourceField as [Campo],RelatedSourceField as [Casos Correlacionados],NetStandardVolume as [VALUE_FL] FROM [{DbCatalog}].[Analytics].[OperativeMovements]" +
                                                                                           "where DestinationNode like '%HistoricalMovements%' and OperationalDate not between @startDate and @endDate ORDER BY operativeMovementsId";

        public static readonly string GetOperativeMovementsWithOwnershipCountwithdifferentHistoricalInformation = $"SELECT TOP 5 OperationalDate as [FECHA_CONT], MovementType as [MOVEMENT_TP] , SourceProduct as [PDTO_ORIG], SourceStorageLocation as [ALM_ORG]," +
                                                                                                        "DestinationProduct as [PDTO_DEST], DestinationStorageLocation as [ALM_DEST],OwnershipVolume as [VALOR],TransferPoint as [RELACION_TT],Month as [Mes]," +
                                                                                                        "Year as [Año],DayofMonth as [DiasMes] FROM[{DbCatalog}].[Analytics].[OperativeMovementsWithOwnership] where MovementType like '%HistoricalMovements%'" +
                                                                                                        "and OperationalDate not between @startDate and @endDate ORDER BY operativeMovementsWithOwnershipId";

        public static readonly string NodeOwnershipRuleStrategies = $"Select * from [{DbCatalog}].[Admin].NodeOwnershipRule where isActive = 1";

        public static readonly string UpdateNodeWithOwnershipStrategy = $"Update [{DbCatalog}].[Admin].[Node] SET NodeOwnershipRuleId = 1, LastModifiedBy='System'" +
                                                              $" Where NodeId In(Select NodeId from [{DbCatalog}].[Admin].[NodeTag] where elementid in" +
                                                              $" (Select elementid from [{DbCatalog}].[Admin].[CategoryElement] where name = @segment))";

        public static readonly string UpdateNodeProductsWithOwnershipStrategy = $"Update [{DbCatalog}].[Admin].[StorageLocationProduct] SET NodeProductRuleId = 1, LastModifiedBy = 'System'" +
                                                                      $" where StorageLocationProductId in (Select StorageLocationProductId from [{DbCatalog}].[Admin].[StorageLocationProduct] where NodeStorageLocationId" +
                                                                      $" in(Select NodeStorageLocationId from [{DbCatalog}].[Admin].[NodeStorageLocation] where nodeid in(Select NodeId from [{DbCatalog}].[Admin].[NodeTag] where ElementId" +
                                                                      $" in(Select Elementid from [{DbCatalog}].[Admin].[CategoryElement] where name = @segment))))";

        public static readonly string UpdateNodeConnectionsWithOwnershipStrategy = $"Update [{DbCatalog}].[Admin].[NodeConnectionProduct] SET NodeConnectionProductRuleId = 1,Priority=1, LastModifiedBy='System'" +
                                                                         $" where nodeconnectionid in (Select NodeConnectionId from [{DbCatalog}].[Admin].[NodeConnection] where sourcenodeid in" +
                                                                         $" (Select NodeId from [{DbCatalog}].[Admin].[NodeTag] where elementid in(Select elementid from [{DbCatalog}].[Admin].[CategoryElement] where name = @segment))" +
                                                                         $" or DestinationNodeid in(Select NodeId from [{DbCatalog}].[Admin].[NodeTag] where elementid in(Select elementid from [{DbCatalog}].[Admin].[CategoryElement] where name = @segment)))";

        public static readonly string DeleteNodeOwnershipStrategy = $"Update [{DbCatalog}].[Admin].[Node] SET NodeOwnershipRuleId = null, LastModifiedBy='System'" +
                                                              $" Where NodeId In(Select NodeId from [{DbCatalog}].[Admin].[NodeTag] where elementid in" +
                                                              $" (Select elementid from [{DbCatalog}].[Admin].[CategoryElement] where name = @segment))";

        public static readonly string DeleteNodeProductsOwnershipStrategy = $"Update [{DbCatalog}].[Admin].[StorageLocationProduct] SET NodeProductRuleId = null, LastModifiedBy = 'System'" +
                                                                      $" where StorageLocationProductId in (Select StorageLocationProductId from [{DbCatalog}].[Admin].[StorageLocationProduct] where NodeStorageLocationId" +
                                                                      $" in(Select NodeStorageLocationId from [{DbCatalog}].[Admin].[NodeStorageLocation] where nodeid in(Select NodeId from [{DbCatalog}].[Admin].[NodeTag] where ElementId" +
                                                                      $" in(Select Elementid from [{DbCatalog}].[Admin].[CategoryElement] where name = @segment))))";

        public static readonly string DeleteNodeConnectionsOwnershipStrategy = $"Update [{DbCatalog}].[Admin].[NodeConnectionProduct] SET NodeConnectionProductRuleId = null,Priority=1, LastModifiedBy='System'" +
                                                                         $" where nodeconnectionid in (Select NodeConnectionId from [{DbCatalog}].[Admin].[NodeConnection] where sourcenodeid in" +
                                                                         $" (Select NodeId from [{DbCatalog}].[Admin].[NodeTag] where elementid in(Select elementid from [{DbCatalog}].[Admin].[CategoryElement] where name = @segment))" +
                                                                         $" or DestinationNodeid in(Select NodeId from [{DbCatalog}].[Admin].[NodeTag] where elementid in(Select elementid from [{DbCatalog}].[Admin].[CategoryElement] where name = @segment)))";

        public static readonly string GetNodeNamesOfSegment = $"select Name from [{DbCatalog}].[Admin].Node where NodeId" +
                                                    $" in(select NodeId from [{DbCatalog}].[Admin].NodeTag where ElementId" +
                                                    $" in(select ElementId from [{DbCatalog}].[Admin].CategoryElement where name = @segment))";

        public static readonly string GetAllNodeConnectionsOfSegment = $"select * from [{DbCatalog}].[Admin].NodeConnection where sourcenodeid in " +
                                                             $" (select nodeid from [{DbCatalog}].[Admin].NodeTag where elementid =" +
                                                             $" (select ElementId from [{DbCatalog}].[Admin].CategoryElement where name = @segment))";

        public static readonly string GetNodeNameFromNodeId = $"select Name from [{DbCatalog}].[Admin].Node where NodeId = @nodeId";
        public static readonly string GetFileRegistrationUsingUploadId = $"SELECT * FROM [{DbCatalog}].[Admin].[FileRegistration] WHERE UploadId= @uploadId";

        public static readonly string GetTopCategorySegment = $"SELECT TOP 1 CE.Name from [{DbCatalog}].[Admin].[NodeTag] NT" +
                                                    " JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON CE.ElementId = NT.ElementId" +
                                                    " JOIN [{DbCatalog}].[Admin].[Category] C ON C.CategoryId = CE.CategoryId" +
                                                    " JOIN [{DbCatalog}].[Admin].[Node] N ON N.NodeId = Nt.NodeId" +
                                                    " WHERE C.CategoryId = 2 AND N.[Order] != 1" +
                                                    " and NT.ElementId NOT IN" +
                                                    " (" +
                                                    "     Select NT.ElementId from [Admin].NodeTag NT" +
                                                    "     Join [Admin].CategoryElement CE ON CE.ElementId = NT.ElementId" +
                                                    "     JOIN [Admin].Category C ON C.CategoryId = CE.CategoryId" +
                                                    "     JOIN [Admin].Node N ON N.NodeId = Nt.NodeId" +
                                                    "     WHERE C.CategoryId = 2 AND N.[Order] = 1" +
                                                    " ) ORDER BY CE.CreatedDate DESC";

        public static readonly string UpdateNodeState = $"UPDATE [{DbCatalog}].[Admin].node set isActive = 0 where NodeId = @NodeID";

        public static readonly string GetLastUpdatedRuleRefreshHistory = $"SELECT TOP 1* from [{DbCatalog}].Admin.OwnershipRuleRefreshHistory where status =0 ORDER BY OwnershipRuleRefreshHistoryId Desc";

        public static readonly string UpdateLastCreatedRefreshHistoryStatus = $"UPDATE [{DbCatalog}].Admin.OwnershipRuleRefreshHistory set status = @status where OwnershipRuleRefreshHistoryId = (Select Top 1 OwnershipRuleRefreshHistoryID from [{DbCatalog}].Admin.OwnershipRuleRefreshHistory ORDER BY OwnershipRuleRefreshHistoryId Desc)";

        public static readonly string BuildLogisticCenter = $"SELECT TOP 1 pr.Name AS productName, CONCAT(lc.Name,':',ns.Name) AS LogisticCenter FROM [{DbCatalog}].[ADMIN].[Node] no INNER JOIN [{DbCatalog}].[ADMIN].[LogisticCenter] lc ON no.LogisticCenterId=lc.LogisticCenterId INNER JOIN [{DbCatalog}].[ADMIN].[NodeStorageLocation] ns ON no.NodeId = ns.NodeId INNER JOIN [{DbCatalog}].[ADMIN].[StorageLocationProduct] slp on slp.NodeStorageLocationId = ns.NodeStorageLocationId INNER JOIN [{DbCatalog}].[Admin].[Product] pr ON slp.ProductId = pr.ProductId WHERE no.nodeid = @nodeId";
        public static readonly string GetOwnershipPercentageValues = $"SELECT TOP 1 * FROM [{DbCatalog}].[Analytics].[OwnershipPercentageValues]";
        public static readonly string GetOwnershipPercentageValuesCount = $"SELECT COUNT(*) FROM [{DbCatalog}].[Analytics].[OwnershipPercentageValues] WHERE FORMAT(LoadDate, 'd', 'us') = FORMAT (Admin.udf_GetTrueDate(), 'd','us') AND LastModifiedBy IS NULL";
        public static readonly string GetUpdatedOwnershipPercentageValuesCount = $"SELECT COUNT(*) FROM [{DbCatalog}].[Analytics].[OwnershipPercentageValues] WHERE FORMAT(LoadDate, 'd', 'us') = FORMAT (Admin.udf_GetTrueDate(), 'd','us') AND LastModifiedBy='ADF'";
        public static readonly string GetOwnershipPercentageValuesCountWhereSourceSystemIsNotCSV = $"SELECT COUNT(*) FROM [{DbCatalog}].[Analytics].[OwnershipPercentageValues] WHERE SourceSystem != @sourceSystem";
        public static readonly string GetTrueDate = $"SELECT FORMAT(Admin.udf_GetTrueDate(), 'd','us') AS TRUEDATE";
        public static readonly string UpdateOwnerColor = $"UPDATE [{DbCatalog}].[Admin].[CategoryElement] SET color = '#00903B' WHERE elementid = 30";
        public static readonly string UpdateOwnerColorWithNull = $"UPDATE [{DbCatalog}].[Admin].[CategoryElement] SET color = null WHERE elementid = 30";
        public static readonly string UpdateNodeStartdateofSegement = $"UPDATE [{DbCatalog}].[Admin].[NodeTag] SET StartDate=GETDATE()-3, LastModifiedBy='System', LastModifiedDate=GETDATE() WHERE" +
                                                                      $" ElementId = (select ElementId from [{DbCatalog}].[Admin].[CategoryElement] where Name = @element";

        public static readonly string GetNodeStartDateFromNodeTag = $"SELECT StartDate FROM [{DbCatalog}].[Admin].[NodeTag] WHERE NodeId = (Select NodeId from [{DbCatalog}].[Admin].[Node] Where Name = @nodeName) order by NodeTagId";
        public static readonly string GetDestinationProductNameOfMovement = $"SELECT [Name] FROM [{DbCatalog}].[ADMIN].[Product] WHERE ProductId IN" +
                                                                            $" (SELECT DestinationProductId FROM[{DbCatalog}].[Offchain].[MovementDestination]" +
                                                                            $" WHERE MovementTransactionId IN(SELECT MovementTransactionId FROM[{DbCatalog}].[Offchain].[Movement] WHERE MOVEMENTID = @movementId))";

        public static readonly string GetDestinationProductTypeIdOfMovement = $"SELECT * FROM [{DbCatalog}].[Offchain].[MovementDestination]" +
                                                                             $" WHERE MovementTransactionId IN(SELECT MovementTransactionId FROM [{DbCatalog}].[Offchain].[Movement] WHERE MOVEMENTID = @movementId)";

        public static readonly string InsertInactiveNodeOwnershipStrategy = $"INSERT INTO [{DbCatalog}].[Admin].NodeOwnershipRule (RuleId, RuleName, IsActive, CreatedBy) VALUES (@ruleId, @ruleName, 0, 'System')";

        public static readonly string UpdateInactiveNodeStrategy = $"Update [{DbCatalog}].[Admin].[Node] SET NodeOwnershipRuleId = @ruleId, LastModifiedBy='System'" +
                                                              $" Where NodeId In(Select NodeId from [{DbCatalog}].[Admin].[NodeTag] where elementid in" +
                                                              $" (Select elementid from [{DbCatalog}].[Admin].[CategoryElement] where name = @segment))";

        public static readonly string DeleteInactiveNodeOwnershipStrategy = $"Delete from [{DbCatalog}].[Admin].NodeOwnershipRule Where RuleId = @ruleId";

        public static readonly string InsertInactiveConnectioneOwnershipStrategy = $"INSERT INTO [{DbCatalog}].[Admin].NodeConnectionProductRule (RuleId, RuleName, IsActive, CreatedBy) VALUES (@ruleId, @ruleName, 0, 'System')";

        public static readonly string UpdateInactiveConnectionStrategy = $"Update [{DbCatalog}].[Admin].[NodeConnectionProduct] SET NodeConnectionProductRuleId = @ruleId,Priority=1, LastModifiedBy='System'" +
                                                                         $" where nodeconnectionid in (Select NodeConnectionId from [{DbCatalog}].[Admin].[NodeConnection] where sourcenodeid in" +
                                                                         $" (Select NodeId from [{DbCatalog}].[Admin].[NodeTag] where elementid in(Select elementid from [{DbCatalog}].[Admin].[CategoryElement] where name = @segment))" +
                                                                         $" or DestinationNodeid in(Select NodeId from [{DbCatalog}].[Admin].[NodeTag] where elementid in(Select elementid from [{DbCatalog}].[Admin].[CategoryElement] where name = @segment)))";

        public static readonly string DeleteInactiveConnectionOwnershipStrategy = $"Delete from [{DbCatalog}].[Admin].NodeConnectionProductRule Where RuleId = @ruleId";
        public static readonly string SetNodeToInactive = $"Update [{DbCatalog}].[Admin].Node Set IsACtive = 0, LastModifiedBy= 'System' where NodeId = @NodeId";

        public static readonly string InsertInactiveProductOwnershipStrategy = $"INSERT INTO [{DbCatalog}].[Admin].NodeProductRule (RuleId, RuleName, IsActive, CreatedBy) VALUES (@ruleId, @ruleName, 0, 'System')";

        public static readonly string UpdateInactiveProductStrategy = $"Update [{DbCatalog}].[Admin].[StorageLocationProduct] SET NodeProductRuleId = @ruleId, LastModifiedBy = 'System'" +
                                                                      $" where StorageLocationProductId in (Select StorageLocationProductId from [{DbCatalog}].[Admin].[StorageLocationProduct] where NodeStorageLocationId" +
                                                                      $" in(Select NodeStorageLocationId from [{DbCatalog}].[Admin].[NodeStorageLocation] where nodeid in(Select NodeId from [{DbCatalog}].[Admin].[NodeTag] where ElementId" +
                                                                      $" in(Select Elementid from [{DbCatalog}].[Admin].[CategoryElement] where name = @segment))))";

        public static readonly string DeleteInactiveProductOwnershipStrategy = $"Delete from [{DbCatalog}].[Admin].NodeProductRule Where RuleId = @ruleId";

        public static readonly string GetNodeDetials = $"SELECT * from [{DbCatalog}].Admin.Node where NodeId=@NodeId";

        public static readonly string GetNodeProductOwnershipStrategyInformation = $"SELECT TOP 1 RuleName AS OwnershipStrategyInformation from [{DbCatalog}].[Admin].[NodeProductRule] WHERE IsActive = 1";

        public static readonly string GetExceptionCountLastHour = $"SELECT COUNT(*) FROM [{DbCatalog}].[Admin].[PendingTransactions] WHERE WHERE CreatedDate <  GETDATE() AND CreatedDate > dateadd(minute, -30, GETDATE())";

        public static readonly string GetLastPurchaseContract = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Contract] WHERE MovementTypeId = 49 ORDER BY CREATEDDATE DESC";

        public static readonly string GetLastSaleContract = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Contract] WHERE MovementTypeId = 50 ORDER BY CREATEDDATE DESC";

        public static readonly string GetContractPendingTransactionError = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[PendingTransaction] WHERE MessageTypeId = 8 ORDER BY CreatedDate DESC";

        public static readonly string GetInventoryProductByNodeIdAndBatchId = $"SELECT COUNT(*) FROM [{DbCatalog}].[Offchain].[InventoryProduct] WHERE NodeId = @nodeId AND EventType=@eventType AND BatchId = @batchId";

        public static readonly string GetInventoryProductByNodeId = $"SELECT COUNT(*) FROM [{DbCatalog}].[Offchain].[InventoryProduct] WHERE NodeId = @nodeId AND EventType=@eventType";

        public static readonly string GetCountOfInitialInventory = $"SELECT COUNT(*) FROM [{DbCatalog}].[Admin].[Unbalance] WHERE TicketId = @ticketId AND ToleranceInitialInventory='100000.00' AND NodeId = @nodeId AND ProductId = '10000002049' AND CalculationDate=@calculationDate";

        public static readonly string GetCountOfFinalInventory = $"SELECT COUNT(*) FROM [{DbCatalog}].[Admin].[Unbalance] WHERE TicketId = @ticketId AND ToleranceFinalInventory='100000.00' AND NodeId = @nodeId AND ProductId = '10000002049'";

        public static readonly string GetTicketStatusOfSegment = $"SELECT TOP 1 FORMAT (DATEADD(DAY, -1, StartDate), 'yyyy-MM-dd') as PreviousDayOfTicketStartDate,FORMAT (StartDate, 'yyyy-MM-dd') as FormattedStartDate,* FROM [{DbCatalog}].[Admin].[Ticket] WHERE CategoryElementId=@segmentId ORDER BY CreatedDate DESC";

        public static readonly string UpdateTicketStatusToFail = $"UPDATE [{DbCatalog}].[Admin].[Ticket] SET Status=2 WHERE TicketId=@ticketId";

        public static readonly string UpdateInventoryDateOfInventoryProduct = $"UPDATE [{DbCatalog}].[Offchain].[Inventory] SET InventoryDate = @PreviousDayOfTicketStartDate WHERE FileRegistrationTransactionId = @fileRegistrationTransactionId";

        public static readonly string UpdateInventoryWithTrueDate = $"UPDATE [{DbCatalog}].[Offchain].[Inventory] SET InventoryDate = CONVERT(varchar, [{DbCatalog}].[Admin].udf_GetTrueDate()-2, 23)  WHERE FileRegistrationTransactionId = @fileRegistrationTransactionId";

        public static readonly string GetVariablesCountOfNode = $"Select count(*) as count from [{DbCatalog}].[Admin].[StorageLocationProductVariable] where StorageLocationProductId in" +
                                                                $" (Select StorageLocationProductId from [{DbCatalog}].[Admin].[StorageLocationProduct] where NodeStorageLocationId in" +
                                                                $" (Select NodeStorageLocationId from [{DbCatalog}].[Admin].[NodeStorageLocation] where NodeId in" +
                                                                $" (Select NodeId from [{DbCatalog}].[Admin].[Node] where Name = @nodeName)))";

        public static readonly string GetInventoryWithMultipleProducts = $"SELECT * FROM [{DbCatalog}].[Offchain].[InventoryProduct] WHERE InventoryId = @inventoryId";

        public static readonly string GetSegmentNodeDetailsForTicket = "SELECT TOP 1 T.CategoryElementId, N.NodeId, T.StartDate, T.EndDate," +
                                                                       $" N.Name AS NodeName, CE.Name AS SegmentName FROM [{DbCatalog}].[Admin].[Ticket] T" +
                                                                       $" JOIN [{DbCatalog}].[Admin].[NodeTag] NT ON T.CategoryElementId = NT.ElementId" +
                                                                       $" JOIN [{DbCatalog}].[Admin].[Node] N ON NT.NodeId = N.NodeId" +
                                                                       $" JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryElementId = CE.ElementId" +
                                                                       $" WHERE T.TicketTypeId=@ticketType AND CE.CategoryId = 2 ORDER BY TicketId";

        public static readonly string GetFailedTicketBySegment = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Ticket] WHERE TickettypeId=@ticketType AND Status=2 AND CategoryElementid=@elementId AND CreatedDate >= Convert(date,[{DbCatalog}].[Admin].udf_GETTRUEDATE()) ORDER BY TicketId DESC";

        public static readonly string GetCountOfProcessedTickets = $"SELECT COUNT(*) FROM [{DbCatalog}].[Admin].[Ticket] WHERE (Status=0 OR Status=2) AND TicketId=@ticketId AND CreatedDate >= Convert(date,[{DbCatalog}].[Admin].udf_GETTRUEDATE())";

        public static readonly string GetCountOfMultipleProcessedTickets = $"SELECT COUNT(*) FROM [{DbCatalog}].[Admin].[Ticket] WHERE (Status=0 OR Status=2) AND (TicketId=@ticketId OR TicketId=@ticketId1) AND CreatedDate >= Convert(date,[{DbCatalog}].[Admin].udf_GETTRUEDATE())";

        public static readonly string GetCountOfProcessedFileRegistationRecords = $"SELECT COUNT(*) FROM [{DbCatalog}].[Admin].[FileRegistrationTransaction] WHERE (StatusTypeId=0 OR StatusTypeId=2) AND FileRegistrationId IN (SELECT FileRegistrationId FROM [{DbCatalog}].[Admin].[FileRegistration] WHERE UploadId=@uploadId)";

        public static readonly string GetCountOfMultipleProcessedFileRegistationRecords = $"SELECT COUNT(*) FROM [{DbCatalog}].[Admin].[FileRegistrationTransaction] WHERE (StatusTypeId=0 OR StatusTypeId=2) AND FileRegistrationId IN (SELECT FileRegistrationId FROM [{DbCatalog}].[Admin].[FileRegistration] WHERE (UploadId=@uploadId) OR UploadId=@uploadId1)";

        public static readonly string GetDeadLetteredMessages = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[DeadLetteredMessage] WHERE Status=1 AND TicketId=@ticketId AND QueueName=@queue AND CreatedDate >= Convert(date,[{DbCatalog}].[Admin].udf_GETTRUEDATE()) ORDER BY DeadletteredMessageId DESC";

        public static readonly string GetDeadLetteredMessagesByQueue = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[DeadLetteredMessage] WHERE Status=1 AND QueueName=@queue AND CreatedDate >= Convert(date,[{DbCatalog}].[Admin].udf_GETTRUEDATE()) ORDER BY DeadletteredMessageId DESC";

        public static readonly string GetCountOfDeadLetteredMessagesByBlobPath = $"SELECT COUNT(*) FROM [{DbCatalog}].[Admin].[DeadLetteredMessage] WHERE BlobPath=@blobPath";

        public static readonly string GetCountOfProcessedDeadLetteredMessages = $"SELECT COUNT(*) FROM [{DbCatalog}].[Admin].[DeadLetteredMessage] WHERE DeadletteredMessageId=@messageId AND Status=0";

        public static readonly string GetCountOfMultipleProcessedDeadLetteredMessages = $"SELECT COUNT(*) FROM [{DbCatalog}].[Admin].[DeadLetteredMessage] WHERE (DeadletteredMessageId=@messageId1 OR DeadletteredMessageId=@messageId2) AND Status=0";

        public static readonly string GetFileRegistrationDetailsBySystem = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[FileRegistration] WHERE Action=1 AND SystemTypeId=@systemTypeId ORDER BY FileRegistrationId DESC";

        public static readonly string GetCountOfInventoriesByInventoryId = $"SELECT COUNT(*) FROM [{DbCatalog}].[Offchain].[InventoryProduct] WHERE InventoryId=@inventoryId";

        public static readonly string GetCountOfInventoriesByInventoryIdAndNodeId = $"SELECT COUNT(*) FROM [{DbCatalog}].[Offchain].[InventoryProduct] WHERE InventoryId=@inventoryId AND NodeId=@nodeId";

        public static readonly string GetCountOfMultipleInventoriesByInventoryId = $"SELECT COUNT(*) FROM [{DbCatalog}].[Offchain].[InventoryProduct] WHERE (InventoryId=@inventoryId OR InventoryId=@inventoryId1)";

        public static readonly string GetCountOfMultipleInventoriesByInventoryIdAndNodeId = $"SELECT COUNT(*) FROM [{DbCatalog}].[Offchain].[InventoryProduct] WHERE (InventoryId=@inventoryId OR InventoryId=@inventoryId1) AND NodeId=@nodeId";

        public static readonly string GetCountOfMultipleMovementsByMovementId = $"SELECT COUNT(*) FROM [{DbCatalog}].[Offchain].[Movement] WHERE (MovementId=@movementId OR MovementId=@movementId1)";

        public static readonly string GetCountOfMovementsByMovementId = $"SELECT COUNT(*) FROM [{DbCatalog}].[Offchain].[Movement] WHERE MovementId=@movementId";

        public static readonly string InsertFileRegistrationForSinoper = $"INSERT INTO [{DbCatalog}].[Admin].[FileRegistration] VALUES (@uploadFileId,[{DbCatalog}].[Admin].udf_GETTRUEDATE(), @uploadFileId,1,1,2,10,@blobPath,null,null,null,1,'System',[{DbCatalog}].[Admin].udf_GETTRUEDATE(),null,null)";

        public static readonly string DeleteDataInHomologationObjectAndDataMapping = $"DELETE FROM [{DbCatalog}].[ADMIN].[HOMOLOGATIONDATAMAPPING] WHERE HOMOLOGATIONGROUPID IN (" +
                                                                                     $" SELECT HOMOLOGATIONGROUPID FROM [{DbCatalog}].[ADMIN].[HOMOLOGATIONGROUP] WHERE HomologationId IN (" +
                                                                                     $" SELECT HomologationId from [{DbCatalog}].[Admin].[Homologation] Where SourceSystemId = @sourceSytem AND DestinationSystemId = @destinationSystem))";

        public static readonly string GetStorageLocationProduct = $"SELECT StorageLocationProductId from [{DbCatalog}].Admin.StorageLocationProduct where NodeStorageLocationId in (select NodeStorageLocationId from [{DbCatalog}].Admin.NodeStorageLocation where NodeId = @node)";

        public static readonly string GetVariableIds = $"SELECT VariableTypeId from [{DbCatalog}].[Admin].[VariableType] where FicoName = @fico";

        public static readonly string InsertvariableintoStorage = $"INSERT into [{DbCatalog}].Admin.StorageLocationProductVariable (StorageLocationProductId, VariableTypeId,CreatedBy) VALUES (@storageid ,@variableid,'System')";

        public static readonly string StorageLocationOwnershipowner = $"SELECT Top 1 * from [{DbCatalog}].Admin.StorageLocationProductOwner where StorageLocationProductId = (SELECT Top 1 StorageLocationProductId from [{DbCatalog}].Admin.StorageLocationProduct where NodeStorageLocationId in" +
                                                                      $"(SELECT NodeStorageLocationId from [{DbCatalog}].Admin.NodeStorageLocation where NodeId = @nodeId)) ORDER by StorageLocationProductOwnerId DESC";

        public static readonly string GetEvacuationMovement = $"SELECT * from [{DbCatalog}].Offchain.Movement where SourceSystem = 'FICO' and movementtypeid in (153,154) and OperationalDate = @OperationDate";

        public static readonly string GetCancellationMovements = $"SELECT * from [{DbCatalog}].Offchain.Movement where SourceSystem ='FICO' and OperationalDate = @OperationDate and movementtypeid in (SELECT ElementId from [{DbCatalog}].Admin.CategoryElement where Name in (@Category1,@Category2))";

        public static readonly string GetCancellationSourceDetails = $"Select * from [{DbCatalog}].Offchain.MovementSource where MovementTransactionId = @MovementTransactionID";

        public static readonly string GetCancellationDestinationDetails = $"Select * from [{DbCatalog}].Offchain.MovementDestination where MovementTransactionId = @MovementTransactionID";

        public static readonly string GetNewContractMovements = $"SELECT TOP 1 * from Offchain.Movement where SourceSystem ='FICO' and OperationalDate = @OperationDate and MovementTypeId in (50) and SegmentId = (SELECT ElementId from Admin.CategoryElement where Name = @SegmentName) ";

        public static readonly string GetNewMovementSource = $"Select * from [{DbCatalog}].[Offchain].[MovementSource] where MovementTransactionId = @MovementTransactionId";

        public static readonly string GetNewMovementDestination = $"SELECT * from [{DbCatalog}].[Offchain].MovementDestination where MovementTransactionId = @MovementTransactionId";

        public static readonly string GetNewCollaborationEventMovements = $"SELECT TOP 1 * from [{DbCatalog}].Offchain.Movement where SourceSystem ='FICO' and OperationalDate = @OperationDate and MovementTypeId in (158) and SegmentId = (SELECT ElementId from [{DbCatalog}].Admin.CategoryElement where Name = @SegmentName) ";

        public static readonly string GetNewEvacuationEventMovements = $"SELECT TOP 1 * from [{DbCatalog}].Offchain.Movement where SourceSystem ='FICO' and OperationalDate = @OperationDate and MovementTypeId in (154) and SegmentId = (SELECT ElementId from [{DbCatalog}].Admin.CategoryElement where Name = @SegmentName) ";

        public static readonly string GetTicketIdwithTicketDesc = $"SELECT Top 1 TicketId from [{DbCatalog}].Admin.Ticket where status= 0 order by TicketId Desc";

        public static readonly string GetTicketIdwithCreatedDateDesc = $"SELECT Top 1 TicketId from [{DbCatalog}].Admin.Ticket where status = 0 order by CreatedDate Desc";

        public static readonly string GetSegmentWithOwnership = $"Select Name from [{DbCatalog}].[Admin].[CategoryElement] where ElementId = (Select  top 1 CategoryElementId from [{DbCatalog}].[Admin].[Ticket])";

        public static readonly string GetSegmentForDeltaCalReport = $"select Name from [{DbCatalog}].Admin.CategoryElement where ElementId  in (select top 1 CategoryElementId from [{DbCatalog}].Admin.Ticket where  TicketTypeId=5 and Status=4 and TicketGroupId is not null)";

        public static readonly string GetSegmentNotForDeltaCalReport = $"select Name from [{DbCatalog}].Admin.CategoryElement where ElementId  in (select top 1 CategoryElementId from [{DbCatalog}].Admin.Ticket where  TicketTypeId=2 and Status=2 and TicketGroupId is not null)";

        public static readonly string GetEnddateOfTicket = $"select top 1 EndDate as Date from [{DbCatalog}].[Admin].[Ticket]";

        public static readonly string GetStartdateOfTicket = $"select top 1 StartDate as Date from [{DbCatalog}].[Admin].[Ticket]";

        public static readonly string UpateTicketEndDate = $"Update [{DbCatalog}].[Admin].[Ticket] set EndDate = GETDATE()+1, LastModifiedBy = 'System' WHERE TicketId = (Select Top 1 TicketId FROM [{DbCatalog}].[Admin].[Ticket] )";

        public static readonly string UpdateInventoryDateToSixDaysBefore = $"UPDATE [{DbCatalog}].[Offchain].[InventoryProduct] SET InventoryDate=[{DbCatalog}].[Admin].udf_GetTrueDate()-6, LastModifiedBy='System' where SegmentId=@segmentId";

        public static readonly string GetTopOneInventoryProduct = $"Select TOP 1 * from [{DbCatalog}].[Offchain].[InventoryProduct] where SegmentId=@segmentId ORDER BY INVENTORYDATE ASC";

        public static readonly string GetTicketAndSegmentDetailsByTicketId = $"SELECT T.TicketId,CE.Name FROM [{DbCatalog}].[Admin].[Ticket] T JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryElementId=CE.ElementId WHERE T.TicketId=@ticketId";

        public static readonly string GetInventoryProductDetailsFromOwnership = $"SELECT TOP 1 * FROM [{DbCatalog}].[Offchain].[Ownership] WHERE TicketId=@ticketId AND InventoryProductId IS NOT NULL";

        public static readonly string GetInventoryProductByInventoryProductId = $"SELECT TOP 1 * FROM [{DbCatalog}].[Offchain].[InventoryProduct] WHERE InventoryProductId=@invProdId";

        public static readonly string GetRulesAndVersionFromOwnership = $"SELECT COUNT(*) FROM [{DbCatalog}].[Offchain].[Ownership] WHERE TicketId=@ticketId AND InventoryProductId IS NOT NULL AND AppliedRule=@rule AND RuleVersion=@version";

        public static readonly string GetLastInventoryTransactionId = $"Select top 1 BlockchainInventoryProductTransactionId,BatchId,TankName,SourceSystemId,NodeId,ProductId,ProductType,SegmentId,ScenarioId,EventType,Version,InventoryProductUniqueId,MeasurementUnit,CreatedDate,InventoryDate,ProductVolume,UncertaintyPercentage,InventoryId from [{DbCatalog}].[Offchain].[InventoryProduct] where InventoryId not like 'DataGenerator%' order by CreatedDate desc";

        public static readonly string GetLastMovementTransactionDetails = $"Select top 1 BlockchainMovementTransactionId,MovementId,MeasurementUnit,OperationalDate,MovementTransactionId,NetStandardVolume,MovementTypeId,CreatedDate,EventType,GlobalMovementId,IsOfficial,ScenarioId,MovementEventId,MovementContractId,SegmentId,SourceSystemId,Version,UncertaintyPercentage,BackupMovementId from [{DbCatalog}].[Offchain].[Movement] where Blockchainstatus = 1 order by CreatedDate desc";

        public static readonly string GetLastMovementSourceDetails = $"Select SourceNodeId,SourceProductId from [{DbCatalog}].Offchain.MovementSource where MovementTransactionId=@MovementTransactionId";

        public static readonly string GetLastMovementDestinationDetails = $"Select DestinationNodeId,DestinationProductId from [{DbCatalog}].Offchain.MovementDestination where MovementTransactionId=@MovementTransactionId";

        public static readonly string GetLastMovementPeriodDetails = $"Select StartTime,EndTime from [{DbCatalog}].Offchain.MovementPeriod where MovementTransactionId=@MovementTransactionId";

        public static readonly string TagSystemElementWithNode = $"INSERT INTO [{DbCatalog}].[Admin].[NodeTag] VALUES (@nodeId, @elementId, cast(Getdate()-@date as date), '9999-12-31 00:00:00.000', 'Automation', cast(Getdate() as date), NULL, NULL)";

        public static readonly string GetAttributeByInventoryProductId = $"SELECT * FROM [{DbCatalog}].[Offchain].[Attribute] WHERE InventoryProductId = @inventoryProductId";

        public static readonly string GetOwnerByInventoryProductId = $"SELECT * FROM [{DbCatalog}].[Offchain].[Owner] WHERE InventoryProductId = @inventoryProductId";

        public static readonly string UpdateActiveElementToInactive = $"UPDATE [{DbCatalog}].[Admin].[CategoryElement] SET isActive= 0 WHERE Name = @name";

        public static readonly string UpdateInActiveElementToActive = $"UPDATE [{DbCatalog}].[Admin].[CategoryElement] SET isActive= 1 WHERE Name = @name";

        public static readonly string UpdateChainSegmentAsASonSegment = $"UPDATE [{DbCatalog}].[Admin].[CategoryElement] SET IsOperationalSegment = 1 WHERE Name = @name";

        public static readonly string GetIsOperationalSegmentValue = $"Select IsOperationalSegment from [{DbCatalog}].[Admin].[CategoryElement] where Name = @name";

        public static readonly string GetListOfActiveChainSegments = $"Select Name from [{DbCatalog}].[Admin].[CategoryElement] where (IsOperationalSegment IS NULL OR IsOperationalSegment = 0) AND CategoryId = 2 AND IsActive = 1 order by Name";

        public static readonly string GetListOfActiveSonSegments = $"Select Name from [{DbCatalog}].[Admin].[CategoryElement] where IsOperationalSegment = 1 AND IsActive = 1 order by Name";

        public static readonly string GetInventoryProductByBatchId = $"SELECT COUNT(*) FROM [{DbCatalog}].[Offchain].[InventoryProduct] WHERE BatchId = @batchId";

        public static readonly string GetScenarioTypeDetails = $"SELECT * FROM [{DbCatalog}].[Admin].[ScenarioType]";

        public static readonly string GetInventoriesOtherthanOperativoScenarioForOldRecords = $"SELECT * FROM [{DbCatalog}].[Offchain].[InventoryProduct] WHERE ScenarioId != @scenarioId AND CreatedDate < '2020-05-26'";

        public static readonly string GetMovementsOtherthanOperativoScenarioForOldRecords = $"SELECT * FROM [{DbCatalog}].[Offchain].[Movement] WHERE ScenarioId != @scenarioId AND CreatedDate < '2020-05-26'";

        public static readonly string GetInventoryProductOfInsertEventType = $"SELECT TOP 1 * FROM [{DbCatalog}].[Offchain].[InventoryProduct] WHERE EventType = @eventType";

        public static readonly string GetCountOfInventoriesByEventType = $"SELECT COUNT(*) FROM [{DbCatalog}].[Offchain].[InventoryProduct] WHERE EventType = @eventType AND InventoryId = @inventoryId";

        public static readonly string GetMovementByBatchId = $"SELECT COUNT(*) FROM [{DbCatalog}].[offchain].[Movement] WHERE BatchId = @batchId";

        public static readonly string GetErrorfromPendingTransaction = $"SELECT * from [{DbCatalog}].Admin.PendingTransaction where segmentId = @SegmentID and Identifier = @Identifier";

        public static readonly string GetMovementDetails = $"SELECT * from [{DbCatalog}].offchain.movement where segmentid = @segmentId order by MovementTransactionId desc";

        public static readonly string EventBasedMovementDetails = $"SELECT Top 1* from [{DbCatalog}].offchain.movement where segmentid =  @segmentId and Eventtype = @Event";

        public static readonly string GetInventoryDetails = $"SELECT * from [{DbCatalog}].[Offchain].[InventoryProduct] where segmentid = @segmentId order by InventoryProductId desc";

        public static readonly string EventBasedInventoryDetails = $"SELECT Top 1* from [{DbCatalog}].[Offchain].[InventoryProduct] where segmentid =  @segmentId and Eventtype = @Event";

        public static readonly string GetCountOfMovementsByEventType = $"SELECT COUNT(*) FROM [{DbCatalog}].[Offchain].[Movement] WHERE EventType = @eventType AND MovementId = @MovementId";

        public static readonly string GetInventoryAttribute = $"SELECT * from [{DbCatalog}].Admin.Attribute where InventoryProductId = (SELECT TOP 1 InventoryProductId FROM [{DbCatalog}].[Offchain].[InventoryProduct] WHERE SegmentId = @segmentId ORDER BY CREATEDDATE DESC)";

        public static readonly string GetMovementAttribute = $"SELECT * from [{DbCatalog}].Admin.Attribute where MovementTransactionId = (SELECT TOP 1 MovementTransactionId FROM [{DbCatalog}].[Offchain].[Movement] WHERE SegmentId = @segmentId ORDER BY CREATEDDATE DESC)";

        public static readonly string GetAllOriginType = $"SELECT Name FROM [{DbCatalog}].[Admin].[OriginType] ORDER BY OriginTypeID";

        public static readonly string GetFirstOriginType = $"SELECT TOP 1 Name FROM [{DbCatalog}].[Admin].[OriginType] ORDER BY OriginTypeID";

        public static readonly string GetSecondOriginType = $"SELECT TOP 1 Name FROM [{DbCatalog}].[Admin].[OriginType] WHERE [OriginTypeID] in (SELECT TOP 2 OriginTypeID FROM [Admin].[OriginType] ORDER BY OriginTypeID ASC) ORDER BY OriginTypeID DESC";

        public static readonly string GetLastOriginType = $"SELECT TOP 1 Name FROM [{DbCatalog}].[Admin].[OriginType] ORDER BY OriginTypeID DESC";

        public static readonly string GetSelectedCancellationType = $"SELECT TOP 1 CE.Name FROM [{DbCatalog}].[Admin].[CategoryElement] CE JOIN [Admin].[Annulation] A on CE.ElementId=A.AnnulationMovementTypeId WHERE [Categoryid]=9 ORDER BY CE.Name ASC";

        public static readonly string GetNotSelectedMovementType = $"SELECT TOP 1 Name FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE [CategoryId]=9 and ElementId not in(select SourceMovementTypeId FROM [Admin].[Annulation]) ORDER BY Name DESC";

        public static readonly string GetMovementTypeofSelectedCancellationType = $"SELECT C.Name FROM [{DbCatalog}].[Admin].[Annulation] AN JOIN [Admin].[CategoryElement] C on AN.SourceMovementTypeId=C.ElementId" +
                                                                                  $" WHERE AN.AnnulationMovementTypeId in (Select Top 1 CE.ElementId FROM [{DbCatalog}].[Admin].[CategoryElement] CE" +
                                                                                  $" JOIN [{DbCatalog}].[Admin].[Annulation] A on CE.ElementId= A.AnnulationMovementTypeId WHERE CE.[CategoryId]= 9 ORDER BY CE.Name ASC)";

        public static readonly string GetSelectedMovementType = $"SELECT TOP 1 CE.Name FROM [{DbCatalog}].[Admin].[CategoryElement] CE JOIN [Admin].[Annulation] A on CE.ElementId=A.SourceMovementTypeId WHERE [Categoryid]=9 ORDER BY Name";

        public static readonly string GetNotSelectedCancellationType = $"SELECT TOP 1 Name FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE [CategoryId]=9 and ElementId not in(select AnnulationMovementTypeId FROM [Admin].[Annulation]) ORDER BY name DESC";

        public static readonly string GetCancellationTypeofSelectedMovementType = $"SELECT C.Name FROM [{DbCatalog}].[Admin].[Annulation] AN JOIN [Admin].[CategoryElement] C on AN.AnnulationMovementTypeId=C.ElementId" +
                                                                                  $" WHERE AN.SourceMovementTypeId in (SELECT TOP 1 CE.ElementId FROM [{DbCatalog}].[Admin].[CategoryElement] CE" +
                                                                                  $" JOIN [{DbCatalog}].[Admin].[Annulation] A on CE.ElementId= A.SourceMovementTypeId WHERE CE.[CategoryId]= 9 ORDER BY Name)";

        public static readonly string CancellationType = $"SELECT TOP 1 Name FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE Categoryid=9 ORDER BY elementid DESC";
        public static readonly string MovementTypes = $"SELECT TOP 1 Name FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE Categoryid=9 ORDER BY elementid";

        public static readonly string GetCountOfMovementsWhenDestinationProductIdIsSameAsSourceProductId = $"SELECT COUNT(*) FROM Offchain.Movement M " +
                                                                                                            " JOIN Offchain.MovementSource MS ON M.MovementTransactionId = MS.MovementTransactionId" +
                                                                                                            " JOIN Offchain.MovementDestination MD ON M.MovementTransactionId = MD.MovementTransactionId" +
                                                                                                            " WHERE MD.DestinationProductId = MS.SourceProductId AND MovementId = @movementId";

        public static readonly string GetSourceSystemElements = $"SELECT TOP 4 * FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE CategoryId = 22 ORDER BY ELEMENTID";

        public static readonly string UpdateNodeTagSegmentDate = $"UPDATE [{DbCatalog}].[Admin].[NodeTag] SET StartDate = CAST (GETDATE()-@date AS DATE) ,LastModifiedBy='SYSTEM', LastModifiedDate=GETDATE() WHERE NodeId = @nodeId AND ElementId = @elementId";

        public static readonly string GetLatestAnnulationRecord = $"SELECT TOP 1 CE.Name as MovementType, CE1.Name as CancellationType, OT.Name as Source,OT1.Name as Destination,OT2.Name as SourceProduct,OT3.Name as DestinationProduct, R.IsActive, R.CreatedDate" +
                                                                    $" FROM [{DbCatalog}].[Admin].[Annulation] R" +
                                                                    $" JOIN[Admin].[CategoryElement] CE ON R.SourceMovementTypeId=CE.ElementId" +
                                                                    $" JOIN[Admin].[CategoryElement] CE1 ON R.AnnulationMovementTypeId=CE1.ElementId" +
                                                                    $" JOIN[Admin].[OriginType] OT ON R.SourceNodeId=OT.OriginTypeId" +
                                                                    $" JOIN[Admin].[OriginType] OT1 ON R.DestinationNodeId=OT1.OriginTypeId" +
                                                                    $" JOIN[Admin].[OriginType] OT2 ON R.SourceProductId=OT2.OriginTypeId" +
                                                                    $" JOIN[Admin].[OriginType] OT3 ON R.DestinationProductId=OT3.OriginTypeId" +
                                                                    $" ORDER BY R.CreatedDate DESC";

        public static readonly string GetAnnulationRecordsByMovementTypeDesc = $" SELECT TOP 1 CE.Name as MovementType, CE1.Name as CancellationType, OT.Name as Source,OT1.Name as Destination,OT2.Name as SourceProduct,OT3.Name as DestinationProduct, R.IsActive, R.CreatedDate FROM [{DbCatalog}].[Admin].[Annulation] R" +
                                                                                $" JOIN[Admin].[CategoryElement] CE ON R.SourceMovementTypeId=CE.ElementId" +
                                                                                $" JOIN[Admin].[CategoryElement] CE1 ON R.AnnulationMovementTypeId=CE1.ElementId" +
                                                                                $" JOIN[Admin].[OriginType] OT ON R.SourceNodeId=OT.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT1 ON R.DestinationNodeId=OT1.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT2 ON R.SourceProductId=OT2.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT3 ON R.DestinationProductId=OT3.OriginTypeId" +
                                                                                $" ORDER BY CE.Name DESC";

        public static readonly string GetAnnulationRecordsByCancellationTypeDesc = $" SELECT TOP 1 CE.Name as MovementType, CE1.Name as CancellationType, OT.Name as Source,OT1.Name as Destination,OT2.Name as SourceProduct,OT3.Name as DestinationProduct, R.IsActive, R.CreatedDate FROM [{DbCatalog}].[Admin].[Annulation] R" +
                                                                                $" JOIN[Admin].[CategoryElement] CE ON R.SourceMovementTypeId=CE.ElementId" +
                                                                                $" JOIN[Admin].[CategoryElement] CE1 ON R.AnnulationMovementTypeId=CE1.ElementId" +
                                                                                $" JOIN[Admin].[OriginType] OT ON R.SourceNodeId=OT.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT1 ON R.DestinationNodeId=OT1.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT2 ON R.SourceProductId=OT2.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT3 ON R.DestinationProductId=OT3.OriginTypeId" +
                                                                                $" ORDER BY CE1.Name DESC";

        public static readonly string GetAnnulationRecordsBySourceNodeDesc = $" SELECT TOP 1 CE.Name as MovementType, CE1.Name as CancellationType, OT.Name as Source,OT1.Name as Destination,OT2.Name as SourceProduct,OT3.Name as DestinationProduct, R.IsActive, R.CreatedDate FROM [{DbCatalog}].[Admin].[Annulation] R" +
                                                                                $" JOIN[Admin].[CategoryElement] CE ON R.SourceMovementTypeId=CE.ElementId" +
                                                                                $" JOIN[Admin].[CategoryElement] CE1 ON R.AnnulationMovementTypeId=CE1.ElementId" +
                                                                                $" JOIN[Admin].[OriginType] OT ON R.SourceNodeId=OT.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT1 ON R.DestinationNodeId=OT1.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT2 ON R.SourceProductId=OT2.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT3 ON R.DestinationProductId=OT3.OriginTypeId" +
                                                                                $" ORDER BY OT.Name DESC";

        public static readonly string GetAnnulationRecordsByDestinationNodeDesc = $" SELECT TOP 1 CE.Name as MovementType, CE1.Name as CancellationType, OT.Name as Source,OT1.Name as Destination,OT2.Name as SourceProduct,OT3.Name as DestinationProduct, R.IsActive, R.CreatedDate FROM [{DbCatalog}].[Admin].[Annulation] R" +
                                                                                $" JOIN[Admin].[CategoryElement] CE ON R.SourceMovementTypeId=CE.ElementId" +
                                                                                $" JOIN[Admin].[CategoryElement] CE1 ON R.AnnulationMovementTypeId=CE1.ElementId" +
                                                                                $" JOIN[Admin].[OriginType] OT ON R.SourceNodeId=OT.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT1 ON R.DestinationNodeId=OT1.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT2 ON R.SourceProductId=OT2.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT3 ON R.DestinationProductId=OT3.OriginTypeId" +
                                                                                $" ORDER BY OT1.Name DESC";

        public static readonly string GetAnnulationRecordsBySourceProductDesc = $" SELECT TOP 1 CE.Name as MovementType, CE1.Name as CancellationType, OT.Name as Source,OT1.Name as Destination,OT2.Name as SourceProduct,OT3.Name as DestinationProduct, R.IsActive, R.CreatedDate FROM [{DbCatalog}].[Admin].[Annulation] R" +
                                                                                $" JOIN[Admin].[CategoryElement] CE ON R.SourceMovementTypeId=CE.ElementId" +
                                                                                $" JOIN[Admin].[CategoryElement] CE1 ON R.AnnulationMovementTypeId=CE1.ElementId" +
                                                                                $" JOIN[Admin].[OriginType] OT ON R.SourceNodeId=OT.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT1 ON R.DestinationNodeId=OT1.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT2 ON R.SourceProductId=OT2.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT3 ON R.DestinationProductId=OT3.OriginTypeId" +
                                                                                $" ORDER BY OT2.Name DESC";

        public static readonly string GetAnnulationRecordsByDestinationProductDesc = $" SELECT TOP 1 CE.Name as MovementType, CE1.Name as CancellationType, OT.Name as Source,OT1.Name as Destination,OT2.Name as SourceProduct,OT3.Name as DestinationProduct, R.IsActive, R.CreatedDate FROM [{DbCatalog}].[Admin].[Annulation] R" +
                                                                                $" JOIN[Admin].[CategoryElement] CE ON R.SourceMovementTypeId=CE.ElementId" +
                                                                                $" JOIN[Admin].[CategoryElement] CE1 ON R.AnnulationMovementTypeId=CE1.ElementId" +
                                                                                $" JOIN[Admin].[OriginType] OT ON R.SourceNodeId=OT.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT1 ON R.DestinationNodeId=OT1.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT2 ON R.SourceProductId=OT2.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT3 ON R.DestinationProductId=OT3.OriginTypeId" +
                                                                                $" ORDER BY OT3.Name DESC";

        public static readonly string GetMovementTransactionId = $"SELECT MovementTransactionId FROM [{DbCatalog}].[OffChain].[Movement] WHERE MovementId = @movementId";

        public static readonly string GetMovementDetailsByMovementId = $"SELECT TOP 1 * FROM [{DbCatalog}].[OffChain].[Movement] WHERE MovementId = @movementId ORDER BY MovementTransactionId DESC";

        public static readonly string GetLastButOneMovementDetailsByMovementId = $"SELECT TOP 1 * FROM [{DbCatalog}].[OffChain].[Movement] WHERE MovementTransactionId NOT IN (SELECT TOP 1 MovementTransactionId FROM [{DbCatalog}].[OffChain].[Movement] WHERE MovementId = @movementId ORDER BY MovementTransactionId DESC) AND MovementId = @movementId ORDER BY MovementTransactionId DESC";

        public static readonly string UpdateMovementEventType = $"UPDATE [{DbCatalog}].[OffChain].[Movement] SET EventType = 'Delete' WHERE MovementTransactionId IN (SELECT TOP 1 MovementTransactionId FROM [{DbCatalog}].[OffChain].[Movement] WHERE MovementId = @movementId ORDER BY MovementTransactionId DESC)";

        public static readonly string InsertIntoSapTracking = $"INSERT INTO [{DbCatalog}].[Admin].[SapTracking] (MovementTransactionId, FileRegistrationId, StatusTypeId, OperationalDate, ErrorMessage, SessionId, Comment, CreatedBy, CreatedDate) VALUES (@movementTransactionId,null,0,Getdate(),null,null,'test','System',Getdate())";

        public static readonly string GetRecordByMovementName = $" SELECT TOP 1 CE.Name as MovementType, CE1.Name as CancellationType, OT.Name as Source,OT1.Name as Destination,OT2.Name as SourceProduct,OT3.Name as DestinationProduct, R.IsActive, R.CreatedDate FROM [{DbCatalog}].[Admin].[Annulation] R" +
                                                                                $" JOIN[Admin].[CategoryElement] CE ON R.SourceMovementTypeId=CE.ElementId" +
                                                                                $" JOIN[Admin].[CategoryElement] CE1 ON R.AnnulationMovementTypeId=CE1.ElementId" +
                                                                                $" JOIN[Admin].[OriginType] OT ON R.SourceNodeId=OT.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT1 ON R.DestinationNodeId=OT1.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT2 ON R.SourceProductId=OT2.OriginTypeId" +
                                                                                $" JOIN[Admin].[OriginType] OT3 ON R.DestinationProductId=OT3.OriginTypeId" +
                                                                                $" WHERE CE.Name IN (SELECT TOP 1 CE.Name FROM [Admin].[CategoryElement] CE JOIN [Admin].[Annulation] A on CE.ElementId=A.SourceMovementTypeId WHERE [Categoryid]=9 ORDER BY Name)";

        public static readonly string GetAllActiveMovementTypes = $" SELECT Name FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE[Categoryid]=9 And IsActive = 1 and Name NOT IN(SELECT TOP 1 CE.Name FROM [Admin].[CategoryElement] CE JOIN [Admin].[Annulation] A on CE.ElementId=A.SourceMovementTypeId WHERE [Categoryid]=9 ORDER BY Name)";

        public static readonly string GetLatestUpdatedAnnulationAuditLogField = $" SELECT TOP 1 field FROM [{DbCatalog}].[audit].[auditlog] WHERE entity='admin.annulation' AND logtype='update' ORDER BY logdate DESC";

        public static readonly string GetCountOfOperativeMovementswithdifferentHistoricalInformation = $"SELECT COUNT(*) FROM [{DbCatalog}].[Analytics].[OperativeMovements] WHERE DestinationNode like '%HistoricalMovements%' AND OperationalDate NOT BETWEEN @startDate AND @endDate AND SourceSystem='CSV' AND cast(LoadDate as Date) = cast(getdate() as Date)";

        public static readonly string GetCountOfOperativeMovementsWithOwnershipwithdifferentHistoricalInformation = $"SELECT COUNT(*) FROM [{DbCatalog}].[Analytics].[OperativeMovementsWithOwnership] WHERE MovementType like '%HistoricalMovements%' AND OperationalDate NOT BETWEEN @startDate AND @endDate AND SourceSystem='CSV' AND cast(LoadDate as Date) = cast(getdate() as Date)";

        public static readonly string GetTopTicketBySegmentForDeltaGrid = $"SELECT T.TicketID, CE.Name,Replace(CONVERT(VARCHAR(10), T.StartDate,6) ,' ', '-') as StartDate,Replace(CONVERT(VARCHAR(10), T.EndDate,6) ,' ', '-') as EndDate, " +
                                                                          $"Replace(CONVERT(VARCHAR(10), T.CreatedDate,6) ,' ', '-') as CreatedDate,T.CreatedBy, CASE WHEN T.[Status] = 0 THEN 'Finalizado' WHEN T.[Status] = 1 THEN 'Procesando' WHEN " +
                                                                          $"T.[Status] = 2 THEN 'Fallido' END AS Status FROM [{DbCatalog}].Admin.Ticket T join[{DbCatalog}].Admin.CategoryElement CE on T.CategoryElementId = CE.ElementId " +
                                                                          $"WHERE TicketId = (SELECT TOP 1 TicketId FROM [{DbCatalog}].Admin.Ticket WHERE TicketTypeId = 4 ORDER BY createddate desc)";

        public static readonly string GetAllDeltaTickets = $"SELECT Replace(CONVERT(VARCHAR(10),CreatedDate,6) ,' ', '-') as ExecutionDate, * FROM [{DbCatalog}].Admin.Ticket where TicketTypeId = 4 and CREATEDDATE > GETDATE() - 40 ORDER BY CreatedDate desc";

        public static readonly string GetOfficialDeltas = $"select distinct * from [{DbCatalog}].admin.ticket where tickettypeid = 5";

        public static readonly string GetSONSegments = $"SELECT * FROM [{DbCatalog}].Admin.CategoryElement where CategoryId = 2 and IsOperationalSegment = 1";

        public static readonly string GetAllActiveSegments = $"SELECT * FROM [{DbCatalog}].Admin.CategoryElement where CategoryId = 2 and IsActive = 1";

        public static readonly string SetSegmentStatus = $"update [{DbCatalog}].admin.CategoryElement set IsActive=@Status where Name=@SegmentName";

        public static readonly string GetNonSONSegments = $"SELECT * FROM [{DbCatalog}].Admin.CategoryElement where CategoryId = 2 and IsOperationalSegment = 0";

        public static readonly string UpdateIsOperationalForSegnement = $"UPDATE [{DbCatalog}].Admin.CategoryElement set IsOperationalSegment = '1' WHERE ElementId = @elementId";

        public static readonly string GetSegmentforDeltaCalculation = $"SELECT * FROM [{DbCatalog}].Admin.CategoryElement WHERE elementid = (SELECT TOP 1 CategoryElementId FROM [{DbCatalog}].Admin.Ticket WHERE TicketTypeId = 2  and status = 0 order by createddate DESC)";

        public static readonly string GetSegmentWithoutOwnerhsip = $"SELECT TOP 1 * from [{DbCatalog}].Admin.CategoryElement where CategoryId = 2 and IsOperationalSegment = 1 and ElementId NOT IN (SELECT CategoryElementId from [{DbCatalog}].Admin.Ticket) ORDER BY CreatedDate DESC";

        public static readonly string GetActiveSegment = $"select distinct top 1 Name,ElementId from [{DbCatalog}].admin.CategoryElement ce inner join [{DbCatalog}].Offchain.Movement M on ce.ElementId=m.SegmentId  where IsActive=1";

        public static readonly string GetActiveSegmentwithMovementDetails = $"select top 1 * from [{DbCatalog}].Offchain.Movement M  inner join [{DbCatalog}].offchain.MovementPeriod MP on M.MovementTransactionId=MP.MovementTransactionId  inner join [{DbCatalog}].admin.categoryElement ce on ce.elementId =m.segmentId where YEAR(MP.StartTime) = @Year and M.scenarioId=2 and ce.IsActive=1";

        public static readonly string GetSegmentDetails = $"SELECT * from [{DbCatalog}].Admin.CategoryElement where ElementId = @Element";

        public static readonly string UpdateTicketForProcessingCutoff = $"UPDATE [{DbCatalog}].admin.ticket set status = 1 where TicketId = @Ticket";

        public static readonly string UpdateTicketForSuccessCutoff = $"UPDATE [{DbCatalog}].admin.ticket set status = 0 where TicketId = @Ticket";

        public static readonly string GetSegmentWithRunningCutoff = $"SELECT TOP 1 * FROM [{DbCatalog}].Admin.ticket where TicketTypeId = 1 and status=0 and CategoryElementId = (SELECT Top 1 CategoryElementId from [{DbCatalog}].Admin.Ticket where TicketTypeId = 2 and Status in (0, 2)  ORDER BY createddate desc)";

        public static readonly string GetSegmentWithoutDeltaCalculation = $"SELECT TOP 1 tkt.* from [{DbCatalog}].Admin.ticket tkt JOIN [{DbCatalog}].Offchain.Movement mv on tkt.CategoryElementId = mv.SegmentId where tkt.TicketTypeId = 1 and tkt.status= 0 and mv.TicketId is not null and tkt.CategoryElementId = (SELECT TOP 1 CategoryElementId from [{DbCatalog}].Admin.Ticket WHERE TicketTypeId = 2 and Status in (0,2) ORDER BY createddate desc)";

        public static readonly string GetDeltaCalculationRunning = $"select count(*) from [{DbCatalog}].admin.Ticket t inner join [{DbCatalog}].admin.statustype st on st.StatusTypeId=t.Status where TicketTypeId=5 ";

        public static readonly string GetLastDateForDeltaTicket = $"SELECT TOP 1 Replace(CONVERT(VARCHAR(10),EndDate,6) ,' ', '-')  AS Date,* from [{DbCatalog}].Admin.ticket WHERE CategoryElementId = @Element and TicketTypeId = 2 ORDER BY EndDate DESC";

        public static readonly string GetDeltaTickets = $"SELECT* FROM [{DbCatalog}].[Admin].[Ticket] WHERE Tickettypeid = 4 and CREATEDDATE > GETDATE()-40";

        public static readonly string GetSegmentWithRunningDeltaTicket = $"SELECT TOP 1 * FROM [{DbCatalog}].Admin.ticket where TicketTypeId = 4 and status = 0 ORDER BY CreatedDate DESC";

        public static readonly string GetSegmentDeltaIsProcessing = $"SELECT TOP 1 CE.NAME FROM [{DbCatalog}].ADMIN.CategoryElement CE JOIN [{DbCatalog}].Admin.Ticket T ON CE.ElementId = T.CategoryElementId WHERE T.[Status] = 1 AND T.TicketTypeId = 5";

        public static readonly string GetSegmentwithMovements = $" select top 1 mp.StartTime,mp.EndTime,ce.Name,SegmentId from [{DbCatalog}].Offchain.Movement M  inner join [{DbCatalog}].offchain.MovementPeriod MP  on M.MovementTransactionId=MP.MovementTransactionId inner join [{DbCatalog}].admin.categoryElement ce on ce.elementId =M.segmentId inner join admin.scenarioType st on st.scenarioTypeId= m.scenarioId where M.segmentId in (select top 1 SegmentId from [{DbCatalog}].Offchain.Movement M inner join [{DbCatalog}].offchain.MovementPeriod MP on M.MovementTransactionId= MP.MovementTransactionId inner join [{DbCatalog}].admin.categoryElement ce on ce.elementId = m.segmentId where YEAR(MP.StartTime) = 2020 and M.scenarioId= 2 and ce.IsActive= 1) and M.scenarioId=2";

        public static readonly string GetSegmentNodesWithDeltasPreviousPeriod = $"select top 1 ce.Name from [{DbCatalog}].admin.DeltaNode dn inner join admin.node n on n.nodeid = dn.NodeId inner join [{DbCatalog}].admin.Ticket t on t.TicketId=dn.TicketId inner join [{DbCatalog}].admin.OwnershipNodeStatusType s on s.OwnershipNodeStatusTypeId= dn.status inner join [{DbCatalog}].admin.CategoryElement ce on ce.elementId= t.categoryelementId where dn.status!=9 and t.EndDate< = (select top 1 mp.StartTime from Offchain.Movement M  inner join offchain.MovementPeriod MP  on M.MovementTransactionId= MP.MovementTransactionId inner join [{DbCatalog}].admin.categoryElement ce on ce.elementId = M.segmentId inner join [{DbCatalog}].admin.scenarioType st on st.scenarioTypeId= m.scenarioId where M.segmentId in (select top 1 SegmentId from Offchain.Movement M inner join [{DbCatalog}].offchain.MovementPeriod MP on M.MovementTransactionId= MP.MovementTransactionId inner join [{DbCatalog}].admin.categoryElement ce on ce.elementId = m.segmentId where YEAR(MP.StartTime) = 2020 and M.scenarioId= 2 and ce.IsActive= 1) and M.scenarioId=2 )";

        public static readonly string GetSegmentWithoutOfficailInfo = $"select top 1 ce.Name from [{DbCatalog}].Offchain.Movement m join [{DbCatalog}].admin.CategoryElement ce on m.SegmentId=ce.ElementId where m.segmentid not in ( select SegmentId from [{DbCatalog}].offchain.Movement where ScenarioId =2)";

        public static readonly string GetUnapprovedNodesCount = $"Select Count(*) as Total from [{DbCatalog}].Admin.OwnershipNode WHERE TicketId IN (SELECT TicketId FROM [{DbCatalog}].Admin.Ticket WHERE CategoryElementId = (SELECT ElementId FROM [{DbCatalog}].Admin.CategoryElement WHERE NAME = @Name) and (StartDate >= @StartDate AND EndDate <= @EndDate) AND TicketTypeId = 2)";

        public static readonly string GetSegmentDeltaForValidMovements = $"SELECT TOP 1 tkt.* from [{DbCatalog}].Admin.ticket tkt JOIN [{DbCatalog}].Offchain.Movement mv on tkt.CategoryElementId = mv.SegmentId where tkt.TicketTypeId = 2 and tkt.status= 0 and mv.DeltaTicketId is null and mv.TicketId is null";

        public static readonly string GetSegmentDeltaPendingMovementDetails = $"Select * from [{DbCatalog}].offchain.movement where DeltaTicketId = NULL and SegmentId = (Select ElementId from [{DbCatalog}].Admin.CategoryElement where Name = @Element)";

        public static readonly string InsertTicketForDeltaError = $"INSERT INTO [{DbCatalog}].Admin.Ticket (CategoryElementId, StartDate, EndDate, Status, TicketTypeId, TicketGroupId, ErrorMessage, CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate) VALUES ('1','2020-06-19 01:07:35.670','2020-06-22 01:07:35.670','2','4', NULL,'Cutoff/Ownership tickets for the selected segments are already processing.', 'trueadmin', '2020-06-23 03:56:50.067', 'System', GetDate())";
        public static readonly string InsertTicketForDeltaBusinessError = $"INSERT INTO [{DbCatalog}].Admin.Ticket (CategoryElementId, StartDate, EndDate, Status, TicketTypeId, TicketGroupId, CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate) VALUES ('1','2020-06-19 01:07:35.670','2020-06-22 01:07:35.670','2','4', NULL, 'trueadmin', '2020-06-23 03:56:50.067', 'System', GetDate())";
        public static readonly string GetMovementTransId = $"SELECT TOP 1 * from [{DbCatalog}].Offchain.movement ORDER BY MovementTransactionId DESC";
        public static readonly string GetInventoryProdId = $"SELECT TOP 1 * from [{DbCatalog}].Offchain.InventoryProduct order by InventoryProductId DESC";
        public static readonly string InsertIntoDeltaErrorTable = $"INSERT INTO [{DbCatalog}].Admin.DeltaError (MovementTransactionId, InventoryProductId, TicketId, ErrorMessage, CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate) VALUES (@MovementTransID, @InventoryProdID, @Ticket,'TestAutomation','System2','2020-06-16 02:01:18.873',NULL,NULL)";

        public static readonly string UpdateSegmentAsSon = $"UPDATE [{DbCatalog}].Admin.CategoryElement SET IsOperationalSegment = '1' WHERE Name = @Segment";

        public static readonly string GetNoteFromSAPTrackingTable = $"SELECT * FROM [{DbCatalog}].Admin.SapTracking WHERE MovementTransactionId in (SELECT MovementTransactionId from [{DbCatalog}].offchain.movement WHERE SegmentID = (SELECT ElementId FROM Admin.CategoryElement WHERE Name = @SegmentName))";

        public static readonly string GetTheMovementDetails = $"SELECT * FROM [{DbCatalog}].offchain.movement WHERE ScenarioId = 1 and IsTransferPoint = 1 and SegmentID = (SELECT ElementId FROM [{DbCatalog}].Admin.CategoryElement WHERE Name = @SegmentName);";

        public static readonly string UpdateGlobalMovementID = $"UPDATE [{DbCatalog}].offchain.movement SET GlobalMovementId = @GlobalMovementId , IsOfficial = 1 WHERE MovementId = @MovementID";

        public static readonly string GetMovementsByEventType = $"SELECT Top 1 * FROM [{DbCatalog}].offchain.movement WHERE MovementId = @MovementId AND eventtype = @EventType ORDER BY MovementTransactionId";

        public static readonly string UpdateGlobalMovementId = $"UPDATE [{DbCatalog}].[Offchain].[Movement] SET GlobalMovementId='12345' Where MovementId=@movementId";

        public static readonly string UpdateErrorInSapTracking = $"UPDATE [{DbCatalog}].[Admin].[SapTracking] SET ErrorMessage='Test' Where MovementTransactionId=@movementTransactionId";

        public static readonly string GetSapTrackingByMovementTransactionId = $"SELECT * FROM [{DbCatalog}].[Admin].[SapTracking] WHERE MovementTransactionId=@movementTransactionId";

        public static readonly string GetRegistrationDetailsWithProcessIdentifier = $"SELECT * FROM [{DbCatalog}].[Admin].[FileRegistration] WHERE UploadId=@processIdentifier AND CreatedDate IS NOT NULL";

        public static readonly string GetPendingTransactionDetailsWithProcessIdentifier = $"SELECT * FROM [{DbCatalog}].[Admin].[PendingTransaction] WHERE MessageId=@processIdentifier";

        public static readonly string GetSapProcessResultDetails = $"Exec [Admin].[usp_GetUploadDetailsSAP] @uploadId";

        public static readonly string UpdateSegmentToSON = $"UPDATE [{DbCatalog}].[Admin].[CategoryElement] SET IsOperationalSegment = '1' WHERE name = @name";

        public static readonly string UpdateTicketStatusDelta = $"UPDATE [{DbCatalog}].[Admin].[ticket] SET TicketTypeId = '4' WHERE ticketId =@ticketId";

        public static readonly string UpdateTicketStatusProcessing = $"UPDATE [{DbCatalog}].[Admin].[ticket] SET status = 1 WHERE ticketId = (SELECT TOP 1 ticketId FROM [{DbCatalog}].[Admin].[ticket] T JOIN [{DbCatalog}].[Admin].[CategoryElement] C ON T.CategoryElementId=C.ElementId WHERE C.Name=@name)";

        public static readonly string UpdateTicketStatusFailed = $"UPDATE [{DbCatalog}].[Admin].[ticket] SET status = 2 WHERE ticketId = (SELECT TOP 1 ticketId FROM [{DbCatalog}].[Admin].[ticket] T JOIN [{DbCatalog}].[Admin].[CategoryElement] C ON T.CategoryElementId=C.ElementId WHERE C.Name=@name)";

        public static readonly string GetInventoryDetailsWithId = $"SELECT TOP 1 * FROM [{DbCatalog}].offchain.InventoryProduct WHERE Inventoryid = @InventoryId ORDER BY InventoryProductid DESC";

        public static readonly string InsertAnnulationMovementForDeltaCalculation = $"INSERT INTO [{DbCatalog}].Admin.Annulation (SourceMovementTypeId, AnnulationMovementTypeId, SourceNodeId, DestinationNodeID, SourceProductId, DestinationProductID, IsActive, CreatedBy, CreatedDate) VALUES (@MovementTypeId,@AnnulationMovementTypeId,1,2,1,3,@isActive,'trueadmin', GETDATE())";

        public static readonly string GetDeltaTicketStatus = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Ticket] T JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryElementId = CE.ElementId WHERE T.Tickettypeid=4 and T.CREATEDDATE > GETDATE()-40 ORDER BY T.TicketId DESC";

        public static readonly string GetOwnershipDetails = $"Select OwnershipStatusId, OwnershipAnalyticsStatus, OwnershipAnalyticsErrorMessage from [{DbCatalog}].[Admin].[OwnershipNode] where OwnershipNodeId = @ownershipNodeId";

        public static readonly string GetFicoSourceMovement = $"SELECT * FROM [{DbCatalog}].offchain.movement WHERE SourceMovementTransactionId = @SourceMovementTransactionId";

        public static readonly string GetDeltaTicketIdFromMovement = $"SELECT DeltaTicketId FROM [{DbCatalog}].offchain.movement WHERE SourceMovementTransactionId = @SourceMovementTransactionId";

        public static readonly string GetSourceNodeOfOperationalMovement = $"SELECT SourceNodeId FROM [{DbCatalog}].offchain.MovementSource WHERE MovementTransactionId = @movementTransactionId";

        public static readonly string GetDestinationNodeOfOperationalMovement = $"SELECT DestinationNodeId FROM [{DbCatalog}].offchain.MovementDestination WHERE MovementTransactionId = @movementTransactionId";

        public static readonly string GetMovementDestinationNodeAndProduct = $"SELECT DestinationNodeId, DestinationProductId FROM [{DbCatalog}].offchain.MovementDestination WHERE MovementTransactionId = @movementTransactionId";

        public static readonly string GetMovementSourceNodeAndProduct = $"SELECT SourceNodeId, SourceNodeId FROM [{DbCatalog}].offchain.MovementSource WHERE MovementTransactionId = @movementTransactionId";

        public static readonly string GetMovementTypeIdOperational = $"SELECT MovementTypeId FROM [{DbCatalog}].offchain.Movement WHERE MovementTransactionId = @movementTransactionId";

        public static readonly string GetMovementTypeIdFICO = $"SELECT MovementTypeId FROM [{DbCatalog}].offchain.Movement WHERE SourceMovementTransactionId = @sourceMovementTransactionId";

        public static readonly string GetSourceProductId = $"SELECT SourceProductId FROM [{DbCatalog}].offchain.MovementSource WHERE MovementTransactionId = @movementTransactionId";

        public static readonly string GetDestinationProductId = $"SELECT DestinationProductId FROM [{DbCatalog}].offchain.MovementDestination WHERE MovementTransactionId = @movementTransactionId";

        public static readonly string GetSegSceUnitOperational = $"SELECT SegmentId,MeasurementUnit,ScenarioId FROM [{DbCatalog}].offchain.Movement WHERE MovementTransactionId = @movementTransactionId";

        public static readonly string GetSegSceUnitInventory = $"SELECT SegmentId,MeasurementUnit,ScenarioId FROM [{DbCatalog}].offchain.InventoryProduct WHERE InventoryProductId = @inventoryProductId";

        public static readonly string GetSegSceUnitFICO = $"SELECT SegmentId,MeasurementUnit,ScenarioId FROM [{DbCatalog}].offchain.Movement WHERE SourceMovementTransactionId = @sourceMovementTransactionId";

        public static readonly string GetDeltaAndSourceMovementTrans = $"SELECT NetStandardVolume, SourceMovementTransactionId FROM [{DbCatalog}].offchain.Movement WHERE SourceMovementTransactionId = @sourceMovementTransactionId";

        public static readonly string GetDeltaAndSourceInventoryId = $"SELECT NetStandardVolume, SourceInventoryProductId FROM [{DbCatalog}].offchain.Movement WHERE SourceInventoryProductId = @sourceInventoryProductId";

        public static readonly string GetSourceSystemFromNewMovement = $"SELECT SourceSystem FROM [{DbCatalog}].offchain.Movement WHERE SourceMovementTransactionId = @sourceMovementTransactionId";

        public static readonly string GetSourceSystemFromNewInventoryProdId = $"SELECT SourceSystem FROM [{DbCatalog}].offchain.Movement WHERE SourceInventoryProductId = @sourceInventoryProductId";

        public static readonly string GetNewMovementTransactionId = $"SELECT MovementTransactionId from FROM [{DbCatalog}].offchain.movement WHERE SourceMovementTransactionId = @sourceMovementTransactionId";

        public static readonly string GetLastOfficialDeltaTicket = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[ticket] WHERE TicketTypeId = 5 AND Status = 4 ORDER BY TicketId Desc";

        public static readonly string GetMovementwithSourceMovementId = $"SELECT TOP 1 MovementTransactionId ,NetStandardVolume , MovementTypeId , ScenarioId , SourceMovementTransactionId , SourceSystemId , OfficialDeltaTicketId , OperationalDate , OfficialDeltaMessageTypeId FROM [{DbCatalog}].offchain.movement WHERE SourceMovementTransactionId = @SourceMovementTransactionId ORDER BY CreatedDate DESC";

        public static readonly string GetMovementwithConsolidatedMovementId = $"SELECT TOP 1 MovementTransactionId, ConsolidatedMovementTransactionID ,NetStandardVolume , MovementTypeId , ScenarioId , SourceSystemId , OfficialDeltaTicketId , OperationalDate , OfficialDeltaMessageTypeId FROM [{DbCatalog}].offchain.movement WHERE ConsolidatedMovementTransactionID = @ConsolidatedMovementTransactionID ORDER BY CreatedDate DESC";

        public static readonly string GetOwnershipOfMovement = $"SELECT * from [{DbCatalog}].Offchain.Owner WHERE MovementTransactionId = @MovementTransactionId";

        public static readonly string GetMovementwithSourceInventoryId = $"SELECT TOP 1 OfficialDeltaTicketId,MovementTransactionId ,SourceInventoryProductId,NetStandardVolume , MovementTypeId , ScenarioId , SourceSystemId , DeltaTicketId , OperationalDate, OfficialDeltaMessageTypeId FROM [{DbCatalog}].offchain.movement WHERE SourceInventoryProductId = @SourceInventoryProductId ORDER BY CreatedDate DESC";

        public static readonly string GetMovementwithConsolidatedInventoryId = $"SELECT TOP 1 ConsolidatedInventoryProductId,OfficialDeltaTicketId,MovementTransactionId ,SourceInventoryProductId,NetStandardVolume , MovementTypeId , ScenarioId , SourceSystemId , DeltaTicketId , OperationalDate, OfficialDeltaMessageTypeId FROM [{DbCatalog}].offchain.movement WHERE ConsolidatedInventoryProductId = @ConsolidatedInventoryProductId ORDER BY CreatedDate DESC";

        public static readonly string GetLastNodeId = $"Select top (1) NodeId  from [{DbCatalog}].[Admin].[Node] order by CreatedDate desc";

        public static readonly string GetOffchainNode = $"Select top (1)* from [{DbCatalog}].[Offchain].[Node] where NodeId=@NodeId order by CreatedDate desc";

        public static readonly string GetLatestOfficialDeltaNode = $"SELECT TOP 1 TicketId, Replace(CONVERT(VARCHAR(10),StartDate,6) ,' ', '-')  AS StartDate, Replace(CONVERT(VARCHAR(10),EndDate,6) ,' ', '-')  AS EndDate, ExecutionDate, CreatedBy, NodeName, Segment, Status FROM [{DbCatalog}].[Admin].[view_DeltaNode] WHERE ExecutionDate >= (GetDate() - 90) ORDER BY ticketid, NodeName";

        public static readonly string GetOfficialDeltaNodeRecordCount = $"SELECT COUNT(*) as DeltaRecords FROM [{DbCatalog}].[Admin].[view_DeltaNode] WHERE ExecutionDate >= (GetDate() - 90)";

        public static readonly string GetAllOfficialDeltaNodeRecords = $"SELECT * FROM [{DbCatalog}].[Admin].[view_DeltaNode] WHERE ExecutionDate >= (GetDate() - 90)";

        public static readonly string GetOfficialDeltaNodeByTicketId = $"SELECT TOP 1 TicketId, Replace(CONVERT(VARCHAR(10),StartDate,6) ,' ', '-')  AS StartDate, Replace(CONVERT(VARCHAR(10),EndDate,6) ,' ', '-')  AS EndDate, ExecutionDate, CreatedBy, NodeName, Segment, Status FROM [{DbCatalog}].[Admin].[view_DeltaNode] WHERE ExecutionDate >= (GetDate() - 90) AND TicketId=@TicketId ORDER BY ticketid, NodeName";

        public static readonly string GetDeltaStatusRecordfromOfficialDeltaNode = $"SELECT TOP 1 TicketId, Replace(CONVERT(VARCHAR(10),StartDate,6) ,' ', '-')  AS StartDate, Replace(CONVERT(VARCHAR(10),EndDate,6) ,' ', '-')  AS EndDate, ExecutionDate, CreatedBy, NodeName, Segment, TicketStatus FROM [{DbCatalog}].[Admin].[view_DeltaNode] WHERE ExecutionDate >= (GetDate() - 90) AND TicketStatus='Deltas' ORDER BY ticketid, NodeName";

        public static readonly string GetFailedStatusRecordfromOfficialDeltaNode = $"SELECT TOP 1 TicketId, Replace(CONVERT(VARCHAR(10),StartDate,6) ,' ', '-')  AS StartDate, Replace(CONVERT(VARCHAR(10),EndDate,6) ,' ', '-')  AS EndDate, ExecutionDate, CreatedBy, NodeName, Segment, Status FROM [{DbCatalog}].[Admin].[view_DeltaNode] WHERE ExecutionDate >= (GetDate() - 90) AND Status='Fallido' ORDER BY ticketid, NodeName";

        public static readonly string GetDeltaStatusNamefromOfficialDeltaNode = $"SELECT TOP 1 Status FROM [{DbCatalog}].[Admin].[view_DeltaNode] WHERE Status='Deltas'";

        public static readonly string GetDeltaStatusRecordfromOfficialAdjustment = $"SELECT TOP 1 TicketId FROM [{DbCatalog}].[Admin].[ticket] WHERE tickettypeid = 5 AND status=4 AND createddate > getdate()-365 ORDER BY TicketId DESC";

        public static readonly string ConsolidatedMovementInformationWithSegmentName = $"SELECT * FROM [{DbCatalog}].ADMIN.CONSOLIDATEDMOVEMENT WHERE SegmentId = (SELECT ELEMENTID FROM [{DbCatalog}].ADMIN.CATEGORYELEMENT WHERE NAME = @segmentName)";

        public static readonly string ConsolidatedInventoryProductInformationWithSegmentName = $"SELECT * FROM [{DbCatalog}].ADMIN.CONSOLIDATEDINVENTORYPRODUCT WHERE SegmentId = (SELECT ELEMENTID FROM [{DbCatalog}].ADMIN.CATEGORYELEMENT WHERE NAME = @segmentName)";

        public static readonly string ConsolidatedMovementsInformation = $"SELECT * FROM [{DbCatalog}].Admin.ConsolidatedMovement WHERE SourceNodeId = @sourceNodeId AND DestinationNodeId = @destinationNodeId AND SourceProductId = @sourceProductId AND DestinationProductId = @destinationProductId AND MovementTypeId = @movementTypeId";

        public static readonly string ConsolidatedMovementInformationWithoutMovementSourceDetails = $"SELECT Top 1 * FROM [{DbCatalog}].Admin.ConsolidatedMovement WHERE SourceNodeId IS NULL AND DestinationNodeId = @destinationNodeId AND SourceProductId IS NULL AND DestinationProductId = @destinationProductId AND MovementTypeId = @movementTypeId";

        public static readonly string ConsolidatedInventoryInformationAsc = $"SELECT  TOP 1 * FROM [{DbCatalog}].Admin.ConsolidatedInventoryProduct WHERE NodeId = @NodeId AND ProductId = @ProductId AND TicketId = @officialDeltaTicketId ORDER BY InventoryDate ASC";

        public static readonly string ConsolidatedInventoryInformationDesc = $"SELECT TOP 1 * FROM [{DbCatalog}].Admin.ConsolidatedInventoryProduct WHERE NodeId = @NodeId AND ProductId = @ProductId AND TicketId = @officialDeltaTicketId ORDER BY InventoryDate DESC";

        public static readonly string ConsolidatedMovementOwnerInformation = $"SELECT * FROM [{DbCatalog}].Admin.ConsolidatedOwner WHERE ConsolidatedMovementId = @consolidatedMovementId";

        public static readonly string ConsolidatedInventoryOwnerInformation = $"SELECT * FROM [{DbCatalog}].Admin.ConsolidatedOwner WHERE ConsolidatedInventoryProductId = @consolidatedProductId";

        public static readonly string ConsolidatedInformationAlreadyExistsForSONSegment = $"SELECT Top 1 CE.Name as SegmentName, CONVERT(varchar, CM.EndDate, 23) as EndDate FROM [{DbCatalog}].Admin.ConsolidatedMovement CM JOIN [{DbCatalog}].Admin.CategoryElement CE ON CE.ElementId = CM.SegmentId WHERE CE.IsOperationalSegment = 1 ORDER BY CM.CreatedDate DESC";

        public static readonly string ConsolidatedInformationAlreadyExistsForNOSONSegment = $"SELECT Top 1 CE.Name as SegmentName, CONVERT(varchar, CM.EndDate, 23) as EndDate FROM [{DbCatalog}].Admin.ConsolidatedMovement CM JOIN [{DbCatalog}].Admin.CategoryElement CE ON CE.ElementId = CM.SegmentId WHERE CE.IsOperationalSegment IS NULL ORDER BY CM.CreatedDate DESC";

        public static readonly string ConsolidatedMovementInformationByTicketId = $"SELECT * FROM [{DbCatalog}].ADMIN.ConsolidatedMovement WHERE TicketId = @officialDeltaTicketId";

        public static readonly string ConsolidatedInventoryProductInformationByTicketId = $"SELECT * FROM [{DbCatalog}].ADMIN.ConsolidatedInventoryProduct WHERE TicketId = @officialDeltaTicketId";

        public static readonly string GetOfficialDeltaTicketStatus = $"SELECT TOP 1 * FROM [{DbCatalog}].[Admin].[Ticket] T JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON T.CategoryElementId = CE.ElementId WHERE T.Tickettypeid=5 and T.CREATEDDATE > GETDATE()-40 ORDER BY T.TicketId DESC";

        public static readonly string GetProcessingOfficialDeltaTickets = $"SELECT * FROM [{DbCatalog}].Admin.Ticket WHERE TicketTypeId=5 AND [Status] = 1 AND CategoryElementId = (SELECT ElementId FROM [{DbCatalog}].Admin.CategoryElement WHERE NAME = @segmentName)";

        public static readonly string UpdateOfficialDeltaTicketsToFailed = $"UPDATE [{DbCatalog}].ADMIN.Ticket SET [Status] = 2, LastModifiedBy = 'System' WHERE TicketTypeId = 5 AND TicketId = @ticketId";

        public static readonly string CreateTechnicalErrorOfGivenType = $"Insert Into [{DbCatalog}].[Admin].Ticket" +
                                                                        $"(CategoryElementId,StartDate,EndDate,Status,TicketTypeId,ErrorMessage,CreatedBy,CreatedDate)" +
                                                                        $"Values" +
                                                                        $"(@CategoryElementId,@StartDate,@EndDate,2,5,@TechnicalError,'Admin',	Admin.udf_GETTRUEDATE())" +
                                                                        $"Select top 1 * from [{DbCatalog}].[Admin].ticket order by ticketid desc";

        public static readonly string GetSegmentNameUsingSegmentId = $"select * from [{DbCatalog}].[Admin].categoryelement where elementId = @CategoryElementId";

        public static readonly string Query1 = $"Insert Into [{DbCatalog}].[Admin].Ticket" +
                                               $"(CategoryElementId, StartDate, EndDate, Status, TicketTypeId, ErrorMessage, CreatedBy, CreatedDate)" +
                                               $"Values" +
                                               $"(@SegmentId, @StartDate,	@EndDate,	2,	5,    NULL,	'Admin',    Admin.udf_GETTRUEDATE())" +
                                               $"Select top 1 * from [{DbCatalog}].[Admin].ticket order by ticketid desc";

        public static readonly string Query2 = $"Insert Into [{DbCatalog}].[Admin].DeltaNode" +
                                               $"(TicketId    , NodeId    , Status    , CreatedBy    , CreatedDate)" +
                                               $"Values" +
                                               $"(@TicketId,	@NodeId,	3,	'Admin',    Admin.udf_GETTRUEDATE())" +
                                               $"Select top 1 * from [{DbCatalog}].[Admin].DeltaNode order by DeltaNodeId desc";

        public static readonly string Query3 = $"Insert Into [{DbCatalog}].[Admin].DeltaNodeError" +
                                               $"(DeltaNodeId    , InventoryProductId    , MovementTransactionId    , ConsolidatedMovementId    , ConsolidatedInventoryProductId    , ErrorMessage    , CreatedBy    , CreatedDate)" +
                                               $"Values" +
                                               $"(@DeltaNodeId,	@InventoryProductId,    Null,    Null,    Null,	@BusinessError,	'Admin',    Admin.udf_GETTRUEDATE())" +

                                               $"Insert Into [{DbCatalog}].[Admin].DeltaNodeError" +
                                               $"(DeltaNodeId    , InventoryProductId    , MovementTransactionId    , ConsolidatedMovementId    , ConsolidatedInventoryProductId    , ErrorMessage    , CreatedBy    , CreatedDate)" +
                                               $"Values" +
                                               $"(@DeltaNodeId,    Null,	@MovementTransactionId,   Null,    Null,	@BusinessError,	'Admin',    Admin.udf_GETTRUEDATE())";

        public static readonly string GetInventoryProductIdUsingSegmentId = $"select top 1 * from [{DbCatalog}].offchain.inventoryproduct where SegmentId = @SegmentId";

        public static readonly string GetMovementTransactionIdUsingSegmentId = $"select top 1 * from [{DbCatalog}].offchain.movement where SegmentId = @SegmentId";

        public static readonly string GetOperationallyUnapprovedNode = $"SELECT TOP 1 CE.NAME FROM [{DbCatalog}].[Admin].[Ticket] T JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON CE.ElementId = T.CategoryElementId JOIN [{DbCatalog}].[Admin].[OwnershipNode] OP ON OP.TicketId = T.TicketId JOIN [{DbCatalog}].[Offchain].[InventoryProduct] [IP] ON [IP].SegmentId = [CE].ElementId JOIN [{DbCatalog}].[Offchain].[Movement] [M] ON [M].SegmentId = [CE].ElementId WHERE OP.OwnershipStatusId != 9 AND T.TicketTypeId = 2 AND T.[STATUS] = 0 AND ([IP].ScenarioId = 2 OR [M].ScenarioId = 2)";

        public static readonly string GetSegmentsNoUnapporvedNodes = $"SELECT Top 1 ce.Name FROM [{DbCatalog}].[Admin].[Ticket] T JOIN [{DbCatalog}].[Admin].[CategoryElement] CE ON CE.ElementId = T.CategoryElementId JOIN [{DbCatalog}].[Admin].[OwnershipNode] OP ON OP.TicketId = T.TicketId JOIN [{DbCatalog}].[Offchain].[InventoryProduct] [IP] ON [IP].SegmentId = [CE].ElementId JOIN [{DbCatalog}].[Offchain].[Movement] [M] ON [M].SegmentId = [CE].ElementId WHERE OP.OwnershipStatusId = 9 AND T.TicketTypeId = 2 AND T.[STATUS] = 0 AND ([IP].ScenarioId = 2 OR [M].ScenarioId = 2)";

        public static readonly string SetDeltaCalculationRunning = $"update [{DbCatalog}].admin.Ticket set Status=@Status where CategoryElementId=@SegmentId and TicketTypeId=5";

        public static readonly string UpdateOwnershipNodeStatusBasedOnSegmentName = $"UPDATE [{DbCatalog}].[Admin].[OwnershipNode] SET OwnershipStatusId = @ownershipStatusId WHERE TicketId IN(SELECT TicketId FROM [{DbCatalog}].[Admin].[Ticket] WHERE CategoryElementId = (SELECT ElementId FROM [{DbCatalog}].[Admin].[CategoryElement] WHERE NAME = @segment))";

        public static readonly string OFficialMovementInformationByTicketId = $"SELECT Mov.* , Own.* FROM [{DbCatalog}].offchain.Movement Mov FULL OUTER JOIN [{DbCatalog}].Offchain.Owner Own ON Mov.MovementTransactionId = Own.MovementTransactionId WHERE Mov.OfficialDeltaTicketId = @officialDeltaTicketId and Mov.SystemTypeId = 3";

        public static readonly string OFficialInventoryProductInformationByTicketId = $"SELECT Inv.* , Own.* FROM [{DbCatalog}].offchain.InventoryProduct Inv FULL OUTER JOIN [{DbCatalog}].Offchain.Owner Own ON Inv.InventoryProductId = Own.InventoryProductId WHERE Inv.OfficialDeltaTicketId = @officialDeltaTicketId";

        public static readonly string ConsolidationInventoryProductInformationByTicketId = $"SELECT Inv.* , Own.* FROM [{DbCatalog}].Admin.ConsolidatedInventoryProduct Inv FULL OUTER JOIN [{DbCatalog}].Admin.ConsolidatedOwner Own ON Inv.ConsolidatedInventoryProductId = Own.ConsolidatedInventoryProductId WHERE Inv.TicketID = @officialDeltaTicketId";

        public static readonly string ConsolidationMovementInformationByTicketId = $"SELECT Mov.* , Own.* FROM [{DbCatalog}].Admin.ConsolidatedMovement Mov FULL OUTER JOIN [{DbCatalog}].Admin.ConsolidatedOwner Own ON Mov.ConsolidatedMovementId = Own.ConsolidatedMovementId WHERE Mov.TicketID = @officialDeltaTicketId";

        public static readonly string GetTicketDetailsWithTicketId = $"SELECT * FROM [{DbCatalog}].Admin.Ticket where TicketID = @TicketId";

        public static readonly string UpdateTheNodeTagForOfficialDelta = $"Update [{DbCatalog}].[Admin].[NodeTag] set StartDate = '2020-01-20 00:00:00.000', LastModifiedBy = 'System' where NodeId =@NodeId";

        public static readonly string GetNodeIdsForAGivenTicketId = $"select NodeId from [{DbCatalog}].[Admin].DeltaNode where ticketid = @TicketId";

        public static readonly string GetSegmentForSentToApproveStatus = $"SELECT TOP 1 Segment FROM [{DbCatalog}].[Admin].[view_DeltaNode] WHERE Status='Enviado a aprobación' AND ExecutionDate >= (GetDate() - 90)";

        public static readonly string GetNodeNameForSentToApproveStatus = $"SELECT TOP 1 NodeName FROM [{DbCatalog}].[Admin].[view_DeltaNode] WHERE Status='Enviado a aprobación' AND ExecutionDate >= (GetDate() - 90)";

        public static readonly string GetStatusNamefromOfficialDeltaNode = $"SELECT TOP 1 Status FROM [{DbCatalog}].[Admin].[view_DeltaNode] WHERE NodeName=@nodename AND ExecutionDate >= (GetDate() - 90)";

        public static readonly string InsertAnnulationMovementForOfficialBalanceFile = $"INSERT INTO [{DbCatalog}].Admin.Annulation (SourceMovementTypeId, AnnulationMovementTypeId, SourceNodeId, DestinationNodeID, SourceProductId, DestinationProductID, IsActive, CreatedBy, CreatedDate) VALUES (@movementTypeId,@annulationMovementTypeId,@sourceNodeId,@destinationNodeId,@sourceProductId,@destinationProductId,@isActive,'Automation', [{DbCatalog}].[Admin].udf_GETTRUEDATE())";

        public static readonly string UpdateL1ApproverValue = $"update [{DbCatalog}].admin.DeltaNodeApproval set Approvers=@email where NodeId =@nodeId";

        public static readonly string GetNodeWithDeltas = $"select top 1 TicketId,NodeId,NodeName from [{DbCatalog}].admin.View_DeltaNode where status='Deltas'";

        public static readonly string GetNodeWithSendForApproval = $"select top 1 TicketId,DeltaNodeId,NodeId,NodeName from [{DbCatalog}].admin.View_DeltaNode where status='Enviado a aprobación'";

        public static readonly string GetDeltaApproverAndRequestorPerNode = $"select dna.Approvers,dn.Editor from [{DbCatalog}].admin.DeltaNodeApproval dna inner join [{DbCatalog}].admin.DeltaNode dn on dn.NodeId =dna.NodeId where dna.NodeId=@nodeid";

        public static readonly string GetApprovalCommentAndDate = $"select LastApprovedDate,Comment, LastModifiedDate from [{DbCatalog}].admin.DeltaNode where NodeId=@nodeId";

        public static readonly string GetAnnulationMovementIds = $"SELECT AnnulationMovementTypeId FROM [{DbCatalog}].Admin.Annulation ORDER BY CreatedDate DESC";

        public static readonly string GetNodeStatus = $"select ns.Name from [{DbCatalog}].admin.DeltaNode dn inner join [{DbCatalog}].Admin.OwnershipNodeStatusType ns on ns.OwnershipNodeStatusTypeId=dn.Status where NodeId=@nodeid";

        public static readonly string DeleteAnnulationForSystemGeneratedMovementTypes = $"DELETE FROM [{DbCatalog}].Admin.Annulation WHERE SourceMovementTypeId IN (42,43,44)";

        public static readonly string DeleteOfficialLogisiticMovementTypes = $"DELETE FROM [{DbCatalog}].Admin.CategoryElement WHERE NAME IN ('Anul. Tr. Material a material', 'Anul. Tr.trasladar ce a ce', 'Anul. Tr. Almacen a Almacen','Tr. Material a material', 'Tr.trasladar ce a ce', 'Tr. Almacen a Almacen')";

        public static readonly string InsertOfficialLogisiticMovementTypes = $"INSERT INTO [{DbCatalog}].Admin.CategoryElement(Name,Description,CategoryId,IsActive,IconId,Color,IsOperationalSegment,CreatedBy,CreatedDate)" +
                                                                             $" VALUES('Anul. Tr. Material a material','Anul. Tr. Material a material',9,1,null,null,null,'system', [{DbCatalog}].[Admin].udf_GETTRUEDATE())," +
                                                                             $" ('Anul. Tr.trasladar ce a ce','Anul. Tr.trasladar ce a ce',9,1,null,null,null,'system',[{DbCatalog}].[Admin].udf_GETTRUEDATE())," +
                                                                             $" ('Anul. Tr. Almacen a Almacen','Anul. Tr. Almacen a Almacen',9,1,null,null,null,'system',[{DbCatalog}].[Admin].udf_GETTRUEDATE())," +
                                                                             $" ('Tr. Material a material','Tr. Material a material',9,1,null,null,null,'system',[{DbCatalog}].[Admin].udf_GETTRUEDATE())," +
                                                                             $" ('Tr.trasladar ce a ce','Tr.trasladar ce a ce',9,1,null,null,null,'system',[{DbCatalog}].[Admin].udf_GETTRUEDATE())," +
                                                                             $" ('Tr. Almacen a Almacen','Tr. Almacen a Almacen',9,1,null,null,null,'system',[{DbCatalog}].[Admin].udf_GETTRUEDATE())";

        public static readonly string UpdateOfficialNodeApprovalStatus = $"UPDATE [{DbCatalog}].Admin.DeltaNode SET [Status] = @status WHERE TicketId = @officialDeltaTicket";

        public static readonly string CountOfRecordsInOfficialBalanceFile = $"SELECT COUNT(*) FROM [{DbCatalog}].Offchain.Movement M" +
                                                                            $" LEFT JOIN [{DbCatalog}].Admin.CategoryElement CE ON M.MovementTypeId = CE.ElementId" +
                                                                            $" WHERE SegmentId = (SELECT ElementId FROM [{DbCatalog}].Admin.CategoryElement WHERE [Name] = @segment)" +
                                                                             " AND (OfficialDeltaMessageTypeId IN (3,4) or M.SourceSystemId = 190) AND ScenarioId = 2";

        public static readonly string CountOfRecordsInOfficialBalanceFileForSingleNode = $"SELECT COUNT(*) FROM [{DbCatalog}].Offchain.Movement M" +
                                                                                         $" LEFT JOIN [{DbCatalog}].Admin.CategoryElement CE ON M.MovementTypeId = CE.ElementId" +
                                                                                         $" LEFT JOIN [{DbCatalog}].Offchain.MovementSource MS ON M.MovementTransactionId = MS.MovementTransactionId" +
                                                                                         $" LEFT JOIN [{DbCatalog}].Offchain.MovementDestination MD ON M.MovementTransactionId = MD.MovementTransactionId" +
                                                                                         $" WHERE SegmentId = (SELECT ElementId FROM [{DbCatalog}].Admin.CategoryElement WHERE [Name] = @segment)" +
                                                                                         $" AND (MS.SourceNodeId = @nodeId OR MD.DestinationNodeId = @nodeId)" +
                                                                                         $" AND (OfficialDeltaMessageTypeId IN (3,4) or M.SourceSystemId = 190) AND ScenarioId = 2";

        public static readonly string CountOfRecordsInOfficialBalanceFileWithTrasnformations = $"SELECT COUNT(*) FROM [{DbCatalog}].Offchain.Movement M" +
                                                                                               $" LEFT JOIN [{DbCatalog}].Admin.CategoryElement CE ON M.MovementTypeId = CE.ElementId" +
                                                                                               $" WHERE SegmentId = (SELECT ElementId FROM [{DbCatalog}].Admin.CategoryElement WHERE [Name] = @segment)" +
                                                                                               $" AND NetStandardVolume = '-350010' AND (OfficialDeltaMessageTypeId IN (3,4) or M.SourceSystemId = 190) AND ScenarioId = 2";

        public static readonly string GetTicketBySegmentName = $"SELECT TOP 1 * FROM [{DbCatalog}].Admin.Ticket T JOIN [{DbCatalog}].Admin.CategoryElement CE ON T.CategoryElementid = CE.ElementId WHERE CE.Name = @segment AND T.TicketTypeId = @ticketType ORDER BY T.TicketId DESC";

        public static readonly string SetDeltaNodeApprover = $"INSERT INTO [{DbCatalog}].[Admin].[DeltaNodeApproval] values(@ticketID, 1, 'trueadmin@ecopetrol.com.co', 'ADF', Admin.udf_GETTRUEDATE(), NULL, NULL)";

        public static readonly string ApproveDeltaNodeTicket = $"UPDATE [{DbCatalog}].[Admin].[DeltaNode] SET LastApprovedDate='2020-08-20 07:43:54.263', status=9 where ticketid=@ticketID";

        public static readonly string UpdateOperationalDateForMovements = $"UPDATE [{DbCatalog}].[offchain].[movement] SET OperationalDate='2020-03-31 00:00:00.000' WHERE SegmentId = @segmentId and OperationalDate='2020-04-01 00:00:00.000'";

        public static readonly string ApproveDeltaNodeTicketWithPreviousPeriod = $"UPDATE [{DbCatalog}].[Admin].[DeltaNode] SET LastApprovedDate='2020-08-08 07:43:54.263', status=9 where ticketid=@ticketID";

        public static readonly string GetSapTrackingDetails = $"SELECT COUNT(*) FROM [{DbCatalog}].Admin.SapTracking WHERE MovementTransactionId IN " +
                                                              $" (SELECT MovementTransactionId FROM [{DbCatalog}].Offchain.Movement WHERE" +
                                                              $" SegmentId = (SELECT ElementId FROM [{DbCatalog}].Admin.CategoryElement WHERE Name = @segment) " +
                                                              $" AND EventType = @action AND NetStandardVolume NOT LIKE '%-%') AND StatusTypeId = 0";

        public static readonly string GetInventoriesBySegmentAndVolume = $"SELECT * FROM [{DbCatalog}].Offchain.InventoryProduct WHERE SegmentId = (SELECT ElementId FROM [{DbCatalog}].Admin.CategoryElement WHERE[NAME] = @segment) AND ProductVolume = @value";

        public static readonly string UnbalanceByTicketId = $"SELECT TOP 1 * FROM [{DbCatalog}].Offchain.Unbalance WHERE TicketId = @ticketId";

        public static readonly string GetAnnulationBySourceMovementTypeId = $"SELECT * FROM [{DbCatalog}].Admin.Annulation WHERE SourceMovementTypeId = @sourceMovementTypeId";

        public static readonly string DeleteAnnulationBySourceMovementTypeId = $"DELETE FROM [{DbCatalog}].Admin.Annulation WHERE SourceMovementTypeId = @sourceMovementTypeId";

        // Write SQL Queries before this line
        private static string DbCatalog => ConfigurationManager.AppSettings[nameof(DbCatalog)];
    }
}