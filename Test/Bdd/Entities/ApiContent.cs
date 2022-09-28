// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiContent.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Entities
{
    using System;
    using System.Collections.Generic;

    public static class ApiContent
    {
        public const string CreateCategoryElement = "{ name: 'Category_Test', description: 'CategoryElementAutomation', isActive: true, categoryId: 10 }";
        public const string UpdateCategoryElement = "{ elementId: '10', name: 'Category_Test', description: 'CategoryElementAutomation', isActive: true, categoryId: 10 }";
        public const string CreateCategory = "{ name: 'TestAutomationCategory9', description: 'TestAutomationCategory', isActive: true, isGrouper: true }";
        public const string UpdateCategory = "{ categoryId: '3', name: 'TestAutomationCategory9', description: 'Category1', isActive: true, isGrouper: true }";
        public const string CreateNode = "{ name: 'Node202', description:'Nodeone', isActive: true, nodeTypeId: 2, segmentId: 3, operatorId: 4, ownershipRule: 5, sendToSap: false, logisticCenterId: 4033, order: 2, nodeStorageLocations:[{ name:'Location1', description: 'North', isActive: true, sendToSap: true, storageLocationId: '1004:M001', storageLocationTypeId: 5, products: [{ nodeStorageLocationId: 0, productId: '10000002049', isActive: true}] }]}";
        public const string UpdateNode = "{ nodeId: '7', name: 'Node202', description:'Nodeone',rowVersion:'AAAAAAACSIA=', isActive: true, nodeTypeId: 5, segmentId: 6, operatorId: 9, ownershipRule: 5, sendToSap: false, order: 2, nodeStorageLocations:[{ nodeId: '7', nodeStorageLocationId: 0, name:'Location1', description: 'North', isActive: true, sendToSap: true, storageLocationId: '1004:M001', storageLocationTypeId: 5, products: [{ nodeStorageLocationId: 0, storageLocationProductId: 0, productId: '10000002049', isActive: true}] }^^placeholder^^]}";
        public const string AddStorageLocation = ",{ nodeId: '800', nodeStorageLocationId: 0, name: 'Location3', description: 'North', isActive: true, sendToSap: true, storageLocationId: '1004:M001', storageLocationTypeId: 5, products: [{ nodeStorageLocationId: 0, storageLocationProductId: 0, productId: '10000002372', isActive: false }]}";
        public const string CreateHomologation = "{ sourceSystemId: 1, destinationSystemId: 2, homologationGroups: [{ groupTypeId: 13, homologationObjects: [{ isRequiredMapping: true, homologationObjectTypeId: 1 }], homologationDataMapping: [{ sourceValue: '2', destinationValue: 'AutomationNode' }]}]}";
        public const string CreateHomologationWithTwoGroups = "{ sourceSystemId: 1, destinationSystemId: 2, homologationGroups: [{ groupTypeId: 14, homologationObjects: [{ isRequiredMapping: true, homologationObjectTypeId: 11 }], homologationDataMapping: [{ sourceValue: '10000002318', destinationValue: 'CRUDO CAMPO MAMBO' }]}]}";
        public const string CreateConnection = "{ sourceNodeId : 25, destinationNodeId : 22, description : 'NodeConnectionAutomation', 'isTransfer' : false, 'algorithmId' : null, 'isActive' : true, 'controlLimit' : 0.35, 'products' : [{ 'productId' : '10000002049', 'uncertaintyPercentage' : 1.33, 'owners' : [{ 'ownerId' : 103, 'ownershipPercentage' : 50, }, { 'ownerId' : 104, 'ownershipPercentage' : 50, }] }] }";
        public const string UpdateConnection = "{ sourceNodeId : 25, destinationNodeId : 22, description : 'NodeConnectionAutomation', 'isActive' : true,'rowVersion' : 'AAAAAAACSmM=', 'controlLimit' : 0.35}";
        public const string CreateConnectionWithFourProducts = "{ sourceNodeId : 25, destinationNodeId : 22, description : 'NodeConnectionAutomation', 'isActive' : true, 'controlLimit' : 0.35, isTransfer: true, algorithmId: '1', 'products' : [{ 'productId' : '10000002049', 'uncertaintyPercentage' : 1.33, 'owners' : [{ 'ownerId' : 103, 'ownershipPercentage' : 50, }, { 'ownerId' : 104, 'ownershipPercentage' : 50, }] }, { 'productId' : '10000002441', 'uncertaintyPercentage' : 2.55, 'owners' : [{ 'ownerId' : 103, 'ownershipPercentage' : 50, }, { 'ownerId' : 104, 'ownershipPercentage' : 50, }] }, { 'productId' : '10000003000', 'uncertaintyPercentage' : 1.33, 'owners' : [{ 'ownerId' : 103, 'ownershipPercentage' : 50, }, { 'ownerId' : 104, 'ownershipPercentage' : 50, }] }, { 'productId' : '10000003002', 'uncertaintyPercentage' : 9.11, 'owners' : [{ 'ownerId' : 103, 'ownershipPercentage' : 50, }, { 'ownerId' : 104, 'ownershipPercentage' : 50, }] }] }";
        public const string UpdateConnectionWithFiveProducts = "{ sourceNodeId : 25, destinationNodeId : 22, description : 'NodeConnectionAutomation', 'isActive' : true, 'controlLimit' : 0.35, 'products' : [{ 'productId' : '10000002049', 'uncertaintyPercentage' : 1.33, 'owners' : [{ 'ownerId' : 103, 'ownershipPercentage' : 50, }, { 'ownerId' : 104, 'ownershipPercentage' : 50, }] }, { 'productId' : '10000002441', 'uncertaintyPercentage' : 2.55, 'owners' : [{ 'ownerId' : 103, 'ownershipPercentage' : 50, }, { 'ownerId' : 104, 'ownershipPercentage' : 50, }] }, { 'productId' : '10000003000', 'uncertaintyPercentage' : 1.33, 'owners' : [{ 'ownerId' : 103, 'ownershipPercentage' : 50, }, { 'ownerId' : 104, 'ownershipPercentage' : 50, }] }, { 'productId' : '10000003002', 'uncertaintyPercentage' : 9.11, 'owners' : [{ 'ownerId' : 103, 'ownershipPercentage' : 50, }, { 'ownerId' : 104, 'ownershipPercentage' : 50, }] }, { 'productId' : '10000003078', 'uncertaintyPercentage' : 2.11 }] }";
        public const string UpdateConnectionWithFourProducts = "{ sourceNodeId : 25, destinationNodeId : 22, description : 'NodeConnectionAutomation', 'isActive' : true, 'controlLimit' : 0.35, 'products' : [{ 'productId' : '10000002049', 'uncertaintyPercentage' : 1.33, 'owners' : [{ 'ownerId' : 103, 'ownershipPercentage' : 50, }, { 'ownerId' : 104, 'ownershipPercentage' : 50, }] }, { 'productId' : '10000002441', 'uncertaintyPercentage' : 2.55, 'owners' : [{ 'ownerId' : 103, 'ownershipPercentage' : 50, }, { 'ownerId' : 104, 'ownershipPercentage' : 50, }] }, { 'productId' : '10000003000', 'uncertaintyPercentage' : 1.33, 'owners' : [{ 'ownerId' : 103, 'ownershipPercentage' : 50, }, { 'ownerId' : 104, 'ownershipPercentage' : 50, }] }, { 'productId' : '10000003078', 'uncertaintyPercentage' : 2.11 }] }";
        public const string SetupHomologation = "{ sourceSystemId: 2, destinationSystemId: 1, homologationGroups: [{ groupTypeId: 1, homologationObjects: [{ isRequiredMapping: true, homologationObjectTypeId: 5 }, { isRequiredMapping: true, homologationObjectTypeId: 6 }, { isRequiredMapping: true, homologationObjectTypeId: 1 }, { isRequiredMapping: true, homologationObjectTypeId: 36 }], homologationDataMapping: [{ sourceValue: 'GALAN', destinationValue: '476' }, { sourceValue: 'SALGAR', destinationValue: '477' }, { sourceValue: 'LIMON', destinationValue: '476' }, { sourceValue: '1500', destinationValue: '421' }, { sourceValue: '2000', destinationValue: '422' }]}, { groupTypeId: 2, homologationObjects: [{ isRequiredMapping: true, homologationObjectTypeId: 31 }], homologationDataMapping: [{ sourceValue: 'CASTILLA', destinationValue: '10000002049' }, { sourceValue: 'CAST', destinationValue: '10000002093' }]}]}";
        public const string SetupHomologationWithoutTank = "{ sourceSystemId: 2, destinationSystemId: 1, homologationGroups: [{ groupTypeId: 1, homologationObjects: [{ isRequiredMapping: true, homologationObjectTypeId: 5 }, { isRequiredMapping: true, homologationObjectTypeId: 6 }, { isRequiredMapping: true, homologationObjectTypeId: 1 }], homologationDataMapping: [{ sourceValue: 'GALAN', destinationValue: '476' }, { sourceValue: 'SALGAR', destinationValue: '477' }, { sourceValue: 'LIMON', destinationValue: '476' }]}, { groupTypeId: 2, homologationObjects: [{ isRequiredMapping: true, homologationObjectTypeId: 31 }], homologationDataMapping: [{ sourceValue: 'CASTILLA', destinationValue: '10000002049' }, { sourceValue: 'CAST', destinationValue: '10000002093' }]}]}";
        public const string SetupHomologationForXL = "{sourceSystemId: 3, destinationSystemId:1, homologationGroups: [{groupTypeId: 1, homologationObjects :[{isRequiredMapping: true, HomologationObjectTypeId: 5},{isRequiredMapping: true,HomologationObjectTypeId: 6},{isRequiredMapping: true,HomologationObjectTypeId: 1},{isRequiredMapping: true, HomologationObjectTypeId: 29},{ isRequiredMapping: true, HomologationObjectTypeId: 30}], homologationDataMapping:[{sourceValue: 'ESTACION 1', destinationValue: '130'},{sourceValue: 'ESTACION 2', destinationValue: '132'},{ sourceValue: 'TK 12122', destinationValue: '100'},{ sourceValue: 'DILUYENTE', destinationValue: '83'},{ sourceValue: 'CRUDO', destinationValue: '98'}]},{groupTypeId: 2, homologationObjects :[{ isRequiredMapping: true, homologationObjectTypeId: 31},{isRequiredMapping: true, homologationObjectTypeId: 7},{isRequiredMapping: true, homologationObjectTypeId: 8},],homologationDataMapping:[{sourceValue: 'CRUDO MEZCL', destinationValue:'10000002049'},{sourceValue: 'NAFTA VIRGEN', destinationValue: '10000003004'},{sourceValue: 'CRUDO MEZCLA', destinationValue: '10000003006'}],}]}";
        public const string UpdateNodeUncertaininty = "{storageLocationProductId:{StorageLocationProductId},productId: {ProductId},isActive: true,uncertaintyPercentage: {UncertainityPercentage}}";
        public const string UpdateNodeControlLimitandAcceptablePercentage = "{\"name\":\"{Name}\",\"nodeId\":{NodeId},\"controlLimit\":{ControlLimit},\"acceptableBalancePercentage\":{AcceptablePercentage},\"description\":\"{Description}\",\"isActive\":true,\"nodeTypeId\":{NodeTypeId},\"segmentId\":{SegmentId},\"operatorId\":{OperatorId},\"sendToSap\":true,\"isAuditable\":true,\"logisticCenterId\":\"{LogisticCenterId}\",\"nodeStorageLocations\":[{\"name\":\"{NodeStorageLocationName}\",\"description\":\"{NodeStorageDescription}\",\"isActive\":true,\"sendToSap\":true,\"storageLocationId\":\"{NodeStorageLocationId}\",\"storageLocationTypeId\":{NodeStorageLocationTypeId},\"products\":[{\"nodeStorageLocationId\":{NodeProductStorageLocationId},\"productId\":\"{ProductId}\",\"isActive\":true}]}]}";
        public const string UpdateOwnerInformation = "[{\"isAuditable\":true,\"storageLocationProductId\":{StorageLocationProductId},\"ownerId\":{OwnerId},\"ownershipPercentage\":{OwnershipPercentage}}]";
        public const string CreateConnectionWithTwoProducts = "{ sourceNodeId: '2025', destinationNodeId: '2026', description: 'Automation_Connection_2Products', isActive: true, isTransfer: true, algorithmId: '1', products: [{ productId: '10000002318', uncertaintyPercentage: 1.33 }, { productId: '10000002372', uncertaintyPercentage: 1.33 }]}";
        public const string CreateConnectionWithOneProduct = "{ sourceNodeId: '2025', destinationNodeId: '2026', description: 'Automation_Connection_1Product', isActive: true, isTransfer: true, algorithmId: '1', products: [{ productId: '10000002318', uncertaintyPercentage: 1.33 }]}";
        public const string CreateNodeWithMutipleProducts = "{ name: 'Node202', description:'Nodeone', isActive: true, nodeTypeId: 2, segmentId: 3, operatorId: 4, nodeOwnershipRuleId: 2, sendToSap: true, order: 2, logisticCenterId: 4033, controlLimit: 1.96, acceptableBalancePercentage: 0.1, nodeStorageLocations:[{name:'Location1', description:'North', isActive:true, sendToSap:true, storageLocationId:'1004:M001', storageLocationTypeId:5, products:[{nodeStorageLocationId:0, productId:'10000002318', uncertaintyPercentage:0.04, isActive:true},{nodeStorageLocationId:0,productId:'10000002372',uncertaintyPercentage:0.08,isActive:true}]}]}";
        public const string CreateNodeWithOneProduct = "{ name: 'Node202', description:'Nodeone', isActive: true, nodeTypeId: 2, segmentId: 3, operatorId: 4, nodeOwnershipRuleId: 2, sendToSap: false, order: 2, logisticCenterId: 4033, controlLimit: 1.96, acceptableBalancePercentage: 0.1, nodeStorageLocations:[{name:'Location1', description:'North', isActive:true, sendToSap:true, storageLocationId:'1004:M001', storageLocationTypeId:5, products:[{nodeStorageLocationId:0, productId:'10000002318', uncertaintyPercentage:0.04, isActive:true}]}]}";
        public const string SetupHomologationForCutoff = "{sourceSystemId:3,destinationSystemId:1,homologationGroups:[{groupTypeId:13,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:24},{isRequiredMapping:true,HomologationObjectTypeId:27},{isRequiredMapping:true,HomologationObjectTypeId:4}],homologationDataMapping:[{sourceValue:'AYACUCHO',destinationValue:3741},{sourceValue:'REBOMDEOS',destinationValue:3742},{sourceValue:'DESPACHOS',destinationValue:3743},{sourceValue:'RECIBOS',destinationValue:3744}]},{groupTypeId:14,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:25},{isRequiredMapping:true,HomologationObjectTypeId:28},{isRequiredMapping:true,HomologationObjectTypeId:7}],homologationDataMapping:[{sourceValue:'CRUDO CAMPO MAMBO',destinationValue:'10000002318'},{sourceValue:'CRUDO CAMPO CUSUCO',destinationValue:'10000002372'}]},{groupTypeId:6,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:9}],homologationDataMapping:[{sourceValue:'Bbl',destinationValue:31}]},{groupTypeId:9,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:33}],homologationDataMapping:[{sourceValue:'DESPA',destinationValue:8290}]},{groupTypeId:11,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:26},{isRequiredMapping:true,HomologationObjectTypeId:29},{isRequiredMapping:true,HomologationObjectTypeId:8}],homologationDataMapping:[{sourceValue:'CRUDO',destinationValue:8291},{sourceValue:'DILUYENTE',destinationValue:8292},{sourceValue:'CRUDOR',destinationValue:8293}]},{groupTypeId:7,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:16}],homologationDataMapping:[{sourceValue:'ECOPETROL',destinationValue:8294}]}]}";
        public const string FilterNodes = "{segmentId: 0, nodeTypeIds: [ 0 ], operatorIds: [ 0 ]}";
        public const string UpdateConnectionProduct = "{'createdBy':'Automation','createdDate':'2019-11-12T01:05:20.823Z','isAuditable':true,'nodeConnectionProductId':2,'nodeConnectionId':2,'productId':'10000002049','uncertaintyPercentage':1.28,'isDeleted':false,'productName':'string','priority':4,'ruleId':5,'owners':[{'createdBy':'string','createdDate':'2019-11-12T01:05:20.823Z','isAuditable':true,'nodeConnectionProductOwnerId':0,'nodeConnectionProductId':0,'ownerId':0,'ownershipPercentage':0,'isDeleted':true}]}";
        public const string UpdateConnectionProductOwner = "{'createdBy':'System','createdDate':'2019-11-12T04:26:29.791Z','isAuditable':true,'nodeConnectionProductOwnerId':224,'nodeConnectionProductId':2,'ownerId':30,'ownershipPercentage':100,'isDeleted':false}";
        public const string AssociateNodes = "{'operationalType':2,'elementId':3397,'inputDate':'2019-11-12T04:41:32.313Z','taggedNodes':[{'createdBy':'System','createdDate':'2019-11-12T04:41:32.313Z','nodeTagId':6717,'nodeId':2490}]}";
        public const string OperationalCutoff = "{'unbalances':[{'createdBy':'string','createdDate':'2019-11-08T10:34:30.037Z','unbalanceId':0,'ticketId':0,'nodeId':0,'productId':'string','unbalance':0,'initialInventory':0,'inputs':0,'outputs':0,'finalInventory':0,'identifiedLosses':0,'units':'string','unbalancePercentage':0,'controlLimit':0,'comment':'string','status':true,'calculationDate':'2019-11-08T10:34:30.037Z'}],'pendingTransactionErrors':[{'createdBy':'string','createdDate':'2019-11-08T10:34:30.037Z','errorId':0,'transactionId':0,'errorMessage':'string','comment':'string'}]}";
        public const string RegisterFile = "{'systemTypeId':1,'uploadId':'0bda1d82-1859-4ad1-bcad-3a0640395532','messageDate':'2019-11-12T11:57:26.493Z','name':'TestDataCutOff_Daywise.xlsx','actionType':2,'blobPath':'excel/0bda1d82-1859-4ad1-bcad-3a0640395531','homologationInventoryBlobPath':'string','homologationMovementBlobPath':'string','isActive':true,'isHomologated':true,'previousUploadId':null,'isParsed':true}";
        public const string PendingTransactions = "{'createdBy':'string','createdDate':'2019-11-12T05:39:11.220Z','ticketId':0,'categoryElementId':0,'startDate':'2019-11-12T05:39:11.220Z','endDate':'2019-11-12T05:39:11.220Z','status':true,'errorMessage':'string','movements':[{'createdBy':'string','createdDate':'2019-11-12T05:39:11.220Z','movementTransactionId':0,'messageTypeId':0,'systemTypeId':0,'sourceSystem':'string','eventType':'string','movementId':'string','movementTypeId':'string','ticketId':0,'operationalDate':'2019-11-12T05:39:11.220Z','grossStandardVolume':0,'netStandardVolume':0,'uncertaintyPercentage':0,'measurementUnit':'string','scenario':'string','observations':'string','classification':'string','isDeleted':true,'fileRegistrationTransactionId':0,'attributes':[{'createdBy':'string','createdDate':'2019-11-12T05:39:11.220Z','id':0,'attributeId':'string','attributeValue':'string','valueAttributeUnit':'string','attributeDescription':'string','inventoryProductId':0,'movementTransactionId':0}],'period':{'createdBy':'string','createdDate':'2019-11-12T05:39:11.220Z','movementPeriodId':0,'movementTransactionId':0,'startTime':'2019-11-12T05:39:11.220Z','endTime':'2019-11-12T05:39:11.220Z'},'owners':[{'createdBy':'string','createdDate':'2019-11-12T05:39:11.220Z','id':0,'ownerId':'string','ownershipValue':0,'ownershipValueUnit':'string','inventoryProductId':0,'movementTransactionId':0}]}],'inventories':[{'createdBy':'string','createdDate':'2019-11-12T05:39:11.220Z','inventoryTransactionId':0,'systemTypeId':0,'sourceSystem':'string','destinationSystem':'string','eventType':'string','tankName':'string','inventoryId':'string','ticketId':0,'inventoryDate':'2019-11-12T05:39:11.220Z','nodeId':81,'observations':'string','scenario':'string','isDeleted':true,'fileRegistrationTransactionId':0,'products':[{'createdBy':'string','createdDate':'2019-11-12T05:39:11.220Z','inventoryProductId':0,'productId':'string','productType':'string','productVolume':0,'measurementUnit':'string','inventoryPrimaryKeyId':0,'uncertaintyPercentage':0,'attributes':[null],'owners':[null]}]}],'pendingTransactions':[{'createdBy':'string','createdDate':'2019-11-12T05:39:11.220Z','transactionId':0,'sourceNode':'string','destinationNode':'string','sourceProduct':'string','destinationProduct':'string','volume':'string','units':'string','startDate':'2019-11-12T05:39:11.220Z','endDate':'2019-11-12T05:39:11.220Z','ticketId':0,'messageTypeId':'Movement','blobName':'string','messageId':'string','errorJson':'string'}],'unbalanceComments':[{'createdBy':'string','createdDate':'2019-11-12T05:39:11.220Z','unbalanceId':0,'ticketId':0,'nodeId':81,'productId':'string','unbalance':0,'initialInventory':0,'inputs':0,'outputs':0,'finalInventory':0,'identifiedLosses':0,'units':'string','unbalancePercentage':0,'controlLimit':0,'comment':'string','status':true,'calculationDate':'2019-11-12T05:39:11.220Z'}]}";
        public const string Unbalances = "{'createdBy':'string','createdDate':'2019-11-12T10:45:43.786Z','ticketId':0,'categoryElementId':0,'startDate':'2019-11-12T10:45:43.787Z','endDate':'2019-11-12T10:45:43.787Z','status':true,'errorMessage':'string','movements':[{'createdBy':'string','createdDate':'2019-11-12T10:45:43.787Z','movementTransactionId':0,'messageTypeId':0,'systemTypeId':0,'sourceSystem':'string','eventType':'string','movementId':'string','movementTypeId':'string','ticketId':0,'operationalDate':'2019-11-12T10:45:43.787Z','grossStandardVolume':0,'netStandardVolume':0,'uncertaintyPercentage':0,'measurementUnit':'string','scenario':'string','observations':'string','classification':'string','isDeleted':true,'fileRegistrationTransactionId':0,'attributes':[{'createdBy':'string','createdDate':'2019-11-12T10:45:43.787Z','id':0,'attributeId':'string','attributeValue':'string','valueAttributeUnit':'string','attributeDescription':'string','inventoryProductId':0,'movementTransactionId':0}],'period':{'createdBy':'string','createdDate':'2019-11-12T10:45:43.787Z','movementPeriodId':0,'movementTransactionId':0,'startTime':'2019-11-12T10:45:43.787Z','endTime':'2019-11-12T10:45:43.787Z'},'owners':[{'createdBy':'string','createdDate':'2019-11-12T10:45:43.787Z','id':0,'ownerId':'string','ownershipValue':0,'ownershipValueUnit':'string','inventoryProductId':0,'movementTransactionId':0}]}],'inventories':[{'createdBy':'string','createdDate':'2019-11-12T10:45:43.787Z','inventoryTransactionId':0,'systemTypeId':0,'sourceSystem':'string','destinationSystem':'string','eventType':'string','tankName':'string','inventoryId':'string','ticketId':0,'inventoryDate':'2019-11-12T10:45:43.787Z','nodeId':81,'observations':'string','scenario':'string','isDeleted':true,'fileRegistrationTransactionId':0,'products':[{'createdBy':'string','createdDate':'2019-11-12T10:45:43.787Z','inventoryProductId':0,'productId':'string','productType':'string','productVolume':0,'measurementUnit':'string','inventoryPrimaryKeyId':0,'uncertaintyPercentage':0,'attributes':[null],'owners':[null]}]}],'pendingTransactions':[{'createdBy':'string','createdDate':'2019-11-12T10:45:43.787Z','transactionId':0,'sourceNode':'string','destinationNode':'string','sourceProduct':'string','destinationProduct':'string','volume':'string','units':'string','startDate':'2019-11-12T10:45:43.787Z','endDate':'2019-11-12T10:45:43.787Z','ticketId':0,'messageTypeId':'Movement','blobName':'string','messageId':'string','errorJson':'string'}],'unbalanceComments':[{'createdBy':'string','createdDate':'2019-11-12T10:45:43.787Z','unbalanceId':0,'ticketId':0,'nodeId':81,'productId':'string','unbalance':0,'initialInventory':0,'inputs':0,'outputs':0,'finalInventory':0,'identifiedLosses':0,'units':'string','unbalancePercentage':0,'controlLimit':0,'comment':'string','status':true,'calculationDate':'2019-11-12T10:45:43.787Z'}]}";
        public const string UpdateNodeOwners = "{'isAuditable':true,'storageLocationProductId':'512','isActive':true,'productId':'10000002372','nodeStorageLocationId':'334','rowVersion':'AAAAAAAAVC0=','owners':[{'ownerId':29,'ownershipPercentage':12},{'ownerId':27,'ownershipPercentage':88}]}";
        public const string UpdateOwnershipRule = "{'ownershipRuleType':'StorageLocationProduct','ids':[{'id':'1661','rowVersion':'AAAAAAAAZHg='}],'ownershipRuleId':2}";
        public const string HomologationDataMapping = "{'sourceValue':'81','destinationValue': 'AddedValue' }";
        public const string HomologationObject = "{'homologationObjectTypeId':2,'isRequiredMapping':true}";
        public const string UpdateHomologation = "{'sourceSystemId':3,'destinationSystemId':1,'homologationGroups':[{'groupTypeId':1,'homologationObjects':[{'homologationObjectTypeId':2,'isRequiredMapping':true}],'homologationDataMapping':[{'sourceValue':'r3_2','destinationValue':1643}]}]}";
        public const string HomologationForNodes = "{sourceSystemId:3,destinationSystemId:1,homologationGroups:[{groupTypeId:13,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:2},{isRequiredMapping:true,HomologationObjectTypeId:10},{isRequiredMapping:true,HomologationObjectTypeId:13}],homologationDataMapping:[{sourceValue:'AYACUCHO',destinationValue:3741},{sourceValue:'REBOMDEOS',destinationValue:3742},{sourceValue:'DESPACHOS',destinationValue:3743},{sourceValue:'RECIBOS',destinationValue:3744}]}";
        public const string HomologationForProduct = "{sourceSystemId:3,destinationSystemId:1,homologationGroups:[{groupTypeId:14,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:11},{isRequiredMapping:true,HomologationObjectTypeId:14},{isRequiredMapping:true,HomologationObjectTypeId:3}],homologationDataMapping:[{sourceValue:'CRUDO CAMPO MAMBO',destinationValue:10000002318},{sourceValue:'CRUDO CAMPO CUSUCO',destinationValue:10000002372},{sourceValue:'CRUDOS IMPORTADOS',destinationValue:10000002049},{sourceValue:'CRUDO CAMPO PINOCHO',destinationValue:10000003085}]}]}";
        public const string HomologationForUnit = "{sourceSystemId:3,destinationSystemId:1,homologationGroups:[{groupTypeId:6,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:5},{isRequiredMapping:true,HomologationObjectTypeId:7}],homologationDataMapping:[{sourceValue:'Bbl',destinationValue:31},{sourceValue:'Kg',destinationValue:147},{sourceValue:'%',destinationValue:159}]}]}";
        public const string HomologationForMovementTypeId = "{sourceSystemId:3,destinationSystemId:1,homologationGroups:[{groupTypeId:9,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:18}],homologationDataMapping:[{sourceValue:'DESPA',destinationValue:9128}]}]}";
        public const string HomologationForProductTypeId = "{sourceSystemId:3,destinationSystemId:1,homologationGroups:[{groupTypeId:11,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:4},{isRequiredMapping:true,HomologationObjectTypeId:12},{isRequiredMapping:true,HomologationObjectTypeId:15}],homologationDataMapping:[{sourceValue:'CRUDO',destinationValue:9129},{sourceValue:'DILUYENTE',destinationValue:9130},{sourceValue:'CRUDOR',destinationValue:9131}]}]}";
        public const string HomologationForContractNodes = "{sourceSystemId:4,destinationSystemId:1,homologationGroups:[{groupTypeId:13,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:2},{isRequiredMapping:true,HomologationObjectTypeId:10},{isRequiredMapping:true,HomologationObjectTypeId:13}],homologationDataMapping:[{sourceValue:'AYACUCHO',destinationValue:3741},{sourceValue:'REBOMDEOS',destinationValue:3742},{sourceValue:'DESPACHOS',destinationValue:3743},{sourceValue:'RECIBOS',destinationValue:3744}]}";
        public const string HomologationForContractProduct = "{sourceSystemId:4,destinationSystemId:1,homologationGroups:[{groupTypeId:14,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:11},{isRequiredMapping:true,HomologationObjectTypeId:14},{isRequiredMapping:true,HomologationObjectTypeId:3}],homologationDataMapping:[{sourceValue:'CRUDO CAMPO MAMBO',destinationValue:10000002318},{sourceValue:'CRUDO CAMPO CUSUCO',destinationValue:10000002372}]}]}";
        public const string HomologationForContractUnit = "{sourceSystemId:4,destinationSystemId:1,homologationGroups:[{groupTypeId:6,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:5}],homologationDataMapping:[{sourceValue:'Bbl',destinationValue:31}]}]}";
        public const string HomologationForContractMovementTypeId = "{sourceSystemId:4,destinationSystemId:1,homologationGroups:[{groupTypeId:9,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:18}],homologationDataMapping:[{sourceValue:'Compra',destinationValue:49},{sourceValue:'Venta',destinationValue:50}]}]}";
        public const string HomologationForContractCommercial = "{sourceSystemId:4,destinationSystemId:1,homologationGroups:[{groupTypeId:17,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:19}],homologationDataMapping:[{sourceValue:'REFICAR',destinationValue:52},{sourceValue:'EQUION',destinationValue:53}]}]}";
        public const string HomologationForOwner = "{sourceSystemId:3,destinationSystemId:1,homologationGroups:[{groupTypeId:7,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:8},{isRequiredMapping:true,HomologationObjectTypeId:21},{isRequiredMapping:true,HomologationObjectTypeId:22}],homologationDataMapping:[{sourceValue:'ECOPETROL',destinationValue:30},{sourceValue:'EQUION',destinationValue:29}]}]}";
        public const string HomologationForSourceSystem = "{sourceSystemId:7,destinationSystemId:1,homologationGroups:[{groupTypeId:22,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:27}],homologationDataMapping:[{sourceValue:'SAPS4',destinationValue:162}]}]}";
        public const string HomologationForSourceSystemExcel = "{sourceSystemId:3,destinationSystemId:1,homologationGroups:[{groupTypeId:22,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:27}],homologationDataMapping:[{sourceValue:'EXCEL',destinationValue:164}]}]}";
        public const string HomologationForSegment = "{sourceSystemId:3,destinationSystemId:1,homologationGroups:[{groupTypeId:2,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:20}],homologationDataMapping:[{sourceValue:'Automation_Segment',destinationValue:9132}]}]}";
        public const string HomologationForOperator = "{sourceSystemId:7,destinationSystemId:1,homologationGroups:[{groupTypeId:3,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:24}],homologationDataMapping:[{sourceValue:'Automation_Operator',destinationValue:18}]}]}";
        public const string HomologationForAttributeId = "{sourceSystemId:7,destinationSystemId:1,homologationGroups:[{groupTypeId:20,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:6}],homologationDataMapping:[{sourceValue:'Automation_AttributeId',destinationValue:18}]}]}";
        public const string HomologationForValueAttributeUnit = "{sourceSystemId:7,destinationSystemId:1,homologationGroups:[{groupTypeId:21,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:7}],homologationDataMapping:[{sourceValue:'Automation_ValueAttributeUnit',destinationValue:18}]}]}";
        public const string HomologationForStorageLocation = "{sourceSystemId:7,destinationSystemId:1,homologationGroups:[{groupTypeId:15,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:25},{isRequiredMapping:true,HomologationObjectTypeId:26}],homologationDataMapping:[{sourceValue:'Automation_SourceStorageLocationId',destinationValue:9130},{sourceValue:'Automation_DestinationStorageLocationId',destinationValue:9131}]}]}";
        public const string HomologationForSystem = "{sourceSystemId:7,destinationSystemId:1,homologationGroups:[{groupTypeId:8,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:23}],homologationDataMapping:[{sourceValue:'Automation_System',destinationValue:9132}]}]}";
        public const string HomologationForEventType = "{sourceSystemId:5,destinationSystemId:1,homologationGroups:[{groupTypeId:12,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:1}],homologationDataMapping:[{sourceValue:'EventoPlaneacion',destinationValue:45},{sourceValue:'AcuerdoColaboracion',destinationValue:47}]}]}";
        public const string HomologationForEventNodes = "{sourceSystemId:5,destinationSystemId:1,homologationGroups:[{groupTypeId:13,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:10},{isRequiredMapping:true,HomologationObjectTypeId:13}],homologationDataMapping:[{sourceValue:'AYACUCHO',destinationValue:3741},{sourceValue:'REBOMDEOS',destinationValue:3742}]}]}";
        public const string HomologationForEventOwner = "{sourceSystemId:5,destinationSystemId:1,homologationGroups:[{groupTypeId:7,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:8}],homologationDataMapping:[{sourceValue:'ECOPETROL',destinationValue:30}]}]}";
        public const string OwnershipDetails = "{'algorithmId': '1','movementType': 'DESPACHO A LINEA','sourceNode': 'BATERIA CARICARE','sourceNodeType': 'Limite','destinationNode': 'CAÑO LIMON - COVEÑAS','destinationNodeType': 'Oleoducto','sourceProduct': 'CRUDO CAÑO LIMON','sourceProductType': 'CRUDO','startDate': '2019-04-01','endDate': '2019-07-31'}";
        public const string HomologationForNodesWithTank = "{sourceSystemId:2,destinationSystemId:1,homologationGroups:[{groupTypeId:13,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:2},{isRequiredMapping:true,HomologationObjectTypeId:10},{isRequiredMapping:true,HomologationObjectTypeId:13}],homologationDataMapping:[{sourceValue:'AYACUCHO',destinationValue:3741},{sourceValue:'REBOMDEOS',destinationValue:3742},{sourceValue:'DESPACHOS',destinationValue:3743},{sourceValue:'RECIBOS',destinationValue:3744},{sourceValue:'1500',destinationValue:3744},{sourceValue:'2000',destinationValue:3744}]}";
        public const string UpdateNodeConnectionProductOwners = "{'owners':[{'ownerId':27,'ownershipPercentage':12},{'ownerId':29,'ownershipPercentage':88}],'productId':2562,'rowVersion':'AAAAAAACUt8='}";
        public const string NodeWithSendToSAPTrue = "{'name':'NodeForSAP1','description':'Nodeone','isActive':true,'nodeTypeId':7,'segmentId':10,'operatorId':15,'sendToSap':true,'logisticCenterId':'1000','order': 2,'nodeStorageLocations':[{'name':'Locan1','description':'North','isActive':true,'sendToSap':true,'storageLocationId':'1000:M001','storageLocationTypeId':1,'products':[{'nodeStorageLocationId':0,'productId':'10000003001','isActive':true}]}]}";
        public const string HomologationForProductSAP = "{sourceSystemId:3,destinationSystemId:1,homologationGroups:[{groupTypeId:14,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:11},{isRequiredMapping:true,HomologationObjectTypeId:14},{isRequiredMapping:true,HomologationObjectTypeId:3}],homologationDataMapping:[{sourceValue:'CRUDO CAMPO MAMBO',destinationValue:10000002318},{sourceValue:'CRUDO HCT',destinationValue:10000003001}]}]}";
        public const string HomologationForMovementTypeIdForSIV = "{sourceSystemId:1,destinationSystemId:6,homologationGroups:[{groupTypeId:9,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:18}],homologationDataMapping:[{sourceValue:25,destinationValue:'DESPA'}]}]}";
        public const string ProcessMovement = "{'sourceSystem': 'SAPS4','eventType': 'INSERT','movementId': '56031P132','batchId': '999','movementTypeId': 'DESPA','operationalDate': '2020-05-17T00:00:00','system': '','period': {'startTime': '2020-05-17T00:00:00','endTime': '2020-05-17T00:00:00'},'movementSource': {'sourceNodeId': 'REBOMDEOS','sourceStorageLocationId': 'Automation_SourceStorageLocationId','sourceProductId': 'CRUDO CAMPO MAMBO','sourceProductTypeId': 'CRUDO'},'movementDestination': {'destinationNodeId': 'AYACUCHO','destinationStorageLocationId': 'Automation_DestinationStorageLocationId','destinationProductId': 'CRUDO CAMPO MAMBO','destinationProductTypeid': 'DILUYENTE'},'grossStandardQuantity': '1594','netStandardQuantity': '2450','measurementUnit': 'Bbl','uncertainty': '2.5','segmentId': 'Automation_Segment','operatorId': 'Automation_Operator','attributes': [{'attributeId': 'Automation_AttributeId','attributeType': 'General','attributeValue': 'ECOPETROL S.A.','valueAttributeUnit': 'Automation_ValueAttributeUnit','attributeDescription': 'Mayorista'}],'owners': [{'ownerId': 'ECOPETROL','ownershipValue': '2450','ownerShipValueUnit': 'Bbl'}],'scenarioId': '1','version': '1','observations': 'Mandatory fields example - Obs.','classification': 'Movimiento','officialInformation': {'backupMovementId': '56031P134','globalMovementId': '112233','isOfficial': 'true'},'sapProcessStatus': ''}";
        public const string ProcessInventory = "{'sourceSystem': 'SAPS4','destinationSystem': 'TRUE','eventType': 'INSERT','inventoryId': '20190821','inventoryDate': '2019-08-21T00:00:00','nodeId': 'AYACUCHO','tank': '','scenarioId': 1,'segmentId': 'Automation_Segment','operatorId': 'Automation_Operator','system': 'Automation_System','observations': 'Mandatory fields example - Obs.','version': '1','uncertainty': '0.01','products': [{'productId': 'CRUDO CAMPO MAMBO','productType': 'CRUDO','netStandardQuantity': '2343','grossStandardQuantity': '2000','measurementUnit': 'Bbl','batchId': '1','attributes': [{'attributeId': 'Automation_AttributeId','attributeType': 'Test','attributeValue': '0.78','valueAttributeUnit': 'Automation_ValueAttributeUnit','attributeDescription': 'Test'}],'owners': [{'ownerId': 'ECOPETROL','ownershipValue': '2343','ownerShipValueUnit': 'Bbl'}]}]}";
        public const string ProcessInventoryWithMultipleProducts = "{'sourceSystem':'SAPS4','destinationSystem':'TRUE','eventType':'INSERT','inventoryId':'20190821','inventoryDate':'2019-08-21T00:00:00','nodeId':'AYACUCHO','tank':'','scenarioId':1,'segmentId':'Automation_Segment','operatorId':'Automation_Operator','system':'Automation_System','observations':'Mandatory fields example - Obs.','version':'1','uncertainty':'0.01','products':[{'productId':'CRUDO CAMPO MAMBO','productType':'CRUDO','netStandardQuantity':'2343','grossStandardQuantity':'2000','measurementUnit':'Bbl','batchId':'','attributes':[{'attributeId':'Automation_AttributeId','attributeType':'Test','attributeValue':'0.78','valueAttributeUnit':'Automation_ValueAttributeUnit','attributeDescription':'Test'}],'owners':[{'ownerId':'ECOPETROL','ownershipValue':'2343','ownerShipValueUnit':'Bbl'}]},{'productId':'CRUDO CAMPO CUSUCO','productType':'CRUDO','netStandardQuantity':'2343','grossStandardQuantity':'2000','measurementUnit':'Bbl','batchId':'','attributes':[{'attributeId':'Automation_AttributeId','attributeType':'Test','attributeValue':'0.78','valueAttributeUnit':'Automation_ValueAttributeUnit','attributeDescription':'Test'}],'owners':[{'ownerId':'ECOPETROL','ownershipValue':'2343','ownerShipValueUnit':'Bbl'}]}]}";
        public const string ProcessInventoryWithHomologated = "{'sourceSystem': 'SAPS4','destinationSystem': 'TRUE','eventType': 'INSERT','inventoryId': '20190821','inventoryDate': '2019-08-21T00:00:00','nodeId': 'AYACUCHO','tank': '','scenarioId': 1,'segmentId': 'Automation_Segment','operatorId': 'Automation_Operator','system': 'Automation_System','observations': 'Mandatory fields example - Obs.','version': '1','uncertainty': '0.01','products': [{'productId': 'CRUDO CAMPO MAMBO','productType': 'CRUDO','netStandardQuantity': '2343','grossStandardQuantity': '2000','measurementUnit': 'Bbl','batchId': '1','attributes': [{'attributeId': 'Automation_AttributeId','attributeType': 'Test','attributeValue': '0.78','valueAttributeUnit': 'Automation_ValueAttributeUnit','attributeDescription': 'Test'}],'owners': [{'ownerId': 'ECOPETROL','ownershipValue': '2343','ownerShipValueUnit': 'Bbl'}]}]}";
        public const string ProcessMovementWithHomologated = "{'sourceSystem': 'SAPS4','eventType': 'INSERT','movementId': '56031P132','batchId': '999','movementTypeId': 'DESPA','operationalDate': '2020-05-17T00:00:00','system': '','period': {'startTime': '2020-05-17T00:00:00','endTime': '2020-05-17T00:00:00'},'movementSource': {'sourceNodeId': 'REBOMDEOS','sourceStorageLocationId': '','sourceProductId': 'CRUDO CAMPO MAMBO','sourceProductTypeId': 'CRUDO'},'movementDestination': {'destinationNodeId': 'AYACUCHO','destinationStorageLocationId': '','destinationProductId': 'CRUDO CAMPO MAMBO','destinationProductTypeid': 'DILUYENTE'},'grossStandardQuantity': '1594','netStandardQuantity': '2450','measurementUnit': 'Bbl','uncertainty': '2.5','segmentId': 'Automation_Segment','operatorId': 'Automation_Operator','attributes': [{'attributeId': 'Automation_AttributeId','attributeType': 'General','attributeValue': 'ECOPETROL S.A.','valueAttributeUnit': 'Automation_ValueAttributeUnit','attributeDescription': 'Mayorista'}],'owners': [{'ownerId': 'ECOPETROL','ownershipValue': '2450','ownerShipValueUnit': 'Bbl'}],'scenarioId': '1','version': '1','observations': 'Mandatory fields example - Obs.','classification': 'Movimiento','officialInformation': {'backupMovementId': '56031P134','globalMovementId': '112233','isOfficial': 'true'},'sapProcessStatus': ''}";
        public const string CreateFicoConnection = "{'volPayload':{'volInput':{'tipoLlamada':'tipolamadavalue','estrategiasActivas':true}}}";
        public const string UpdateNodeConnectionProduct = "{'isAuditable':true,'nodeConnectionId':489,'rowVersion':'AAAAAAAAZh4=','nodeConnectionProductId':923,'productId':'10000002318','uncertaintyPercentage':0.8,'priority':1}";
        public const string CreateNodeWithMultipleProduct = "{ name: 'Node202', description:'Nodeone', isActive: true, nodeTypeId: 2, segmentId: 3, operatorId: 4, ownershipRule: 5, sendToSap: false, logisticCenterId: 4033, order: 2, nodeStorageLocations:[{ name:'Location1', description: 'North', isActive: true, sendToSap: true, storageLocationId: '1004:M001', storageLocationTypeId: 5, products: [{ nodeStorageLocationId: 0, productId: '10000002049', isActive: true}, { nodeStorageLocationId: 0, productId: '10000002318', isActive: true}] }]}";
        public const string OperationalCutoffTicketRequest = "{ ticket:{ startDate: '2020-04-01T00:00:00.000Z', endDate: '2020-04-15T00:00:00.000Z', categoryElementId: 1161, ticketTypeId: 1 }, pendingTransactionErrors: [], unbalances: [] }";
        public const string OwnershipTicketRequest = "{ ticket:{ startDate: '2020-04-01T00:00:00.000Z', endDate: '2020-04-15T00:00:00.000Z', categoryElementId: 1161, ticketTypeId: 2, ownerid: 30, nodeid: 1767 }, pendingTransactionErrors: [], unbalances: [] }";
        public const string OfficialDeltaTicketRequest = "{ ticket:{ startDate: '2020-04-01T00:00:00.000Z', endDate: '2020-04-15T00:00:00.000Z', categoryElementId: 1161, ticketTypeId: 5 } }";
        public const string ExcelMovementAndInventoryRequest = "{ uploadId: '0db14061-b575-4a63-a6da-a89fb51e86cf',	name: '0db14061-b575-4a63-a6da-a89fb51e86cf.xlsx', actionType: 1, segmentId: null, systemTypeId: 2,	blobPath: '/true/sinoper/xml/inventory/QU1RIEVBSTAyLkQuUU0gIF5Yh0wg90h0' }";
        public const string ServiceBusMessageForSinoper = "{ systemTypeId: 'Sinoper', uploadFileId: 'QU1RIEVBSTAxLlEuUU0gIF3uT9shBdLq' }";
        public const string ProcessInventoryWithTwoSameProduct = "{'sourceSystem':'SAPS4','destinationSystem':'TRUE','eventType':'INSERT','inventoryId':'20190821','inventoryDate':'2019-08-21T00:00:00','nodeId':'AYACUCHO','tank':'','scenarioId':1,'segmentId':'Automation_Segment','operatorId':'Automation_Operator','system':'Automation_System','observations':'Mandatory fields example - Obs.','version':'1','uncertainty':'0.01','products':[{'productId':'CRUDO CAMPO MAMBO','productType':'CRUDO','netStandardQuantity':'2343','grossStandardQuantity':'2000','measurementUnit':'Bbl','batchId':'','attributes':[{'attributeId':'Automation_AttributeId','attributeType':'Test','attributeValue':'0.78','valueAttributeUnit':'Automation_ValueAttributeUnit','attributeDescription':'Test'}],'owners':[{'ownerId':'ECOPETROL','ownershipValue':'2343','ownerShipValueUnit':'Bbl'}]},{'productId':'CRUDO CAMPO MAMBO','productType':'CRUDO','netStandardQuantity':'2343','grossStandardQuantity':'2000','measurementUnit':'Bbl','batchId':'','attributes':[{'attributeId':'Automation_AttributeId','attributeType':'Test','attributeValue':'0.78','valueAttributeUnit':'Automation_ValueAttributeUnit','attributeDescription':'Test'}],'owners':[{'ownerId':'ECOPETROL','ownershipValue':'2343','ownerShipValueUnit':'Bbl'}]}]}";
        public const string UpdateNodeProductOwners = "{ productId: 41720, rowVersion: 'AAAAAAAA6BA=', owners: [{ownerId: 29,ownershipPercentage: 12},{ownerId: 27,ownershipPercentage: 88}] }";
        public const string CreateNodeWithMutipleProductsForOfficialBalanceFile = "{ name: 'Node202', description:'Nodeone', isActive: true, nodeTypeId: 2, segmentId: 3, operatorId: 4, nodeOwnershipRuleId: 1, sendToSap: true, order: 2, logisticCenterId: 1088, controlLimit: 1.96, acceptableBalancePercentage: 0.1, nodeStorageLocations:[{name:'LogisticLocation1', description:'North', isActive:true, sendToSap:true, storageLocationId:'1088:M001', storageLocationTypeId:5, products:[{nodeStorageLocationId:0, productId:'10000002373', uncertaintyPercentage:0.04, isActive:true},{nodeStorageLocationId:0,productId:'10000002372',uncertaintyPercentage:0.08,isActive:true}]},{name:'LogisticLocation2', description:'North', isActive:true, sendToSap:true, storageLocationId:'1088:C001', storageLocationTypeId:5, products:[{nodeStorageLocationId:0, productId:'40000003009', uncertaintyPercentage:0.04, isActive:true}]}]}";
        public const string CreateNodeWithOneProductForOfficialBalanceFile = "{ name: 'Node202', description:'Nodeone', isActive: true, nodeTypeId: 2, segmentId: 3, operatorId: 4, nodeOwnershipRuleId: 1, sendToSap: true, order: 2, logisticCenterId: 1900, controlLimit: 1.96, acceptableBalancePercentage: 0.1, nodeStorageLocations:[{name:'LogisticLocation1', description:'North', isActive:true, sendToSap:true, storageLocationId:'1900:M001', storageLocationTypeId:5, products:[{nodeStorageLocationId:0, productId:'10000002318', uncertaintyPercentage:0.04, isActive:true}]}]}";
        public const string UpdateNodeProductOwnersForOfficialBalanceFile = "{ productId: 41720, rowVersion: 'AAAAAAAA6BA=', owners: [{ownerId: 30,ownershipPercentage: 100}]}";
        public const string CreateConnectionForOfficialBalanceFile = "{ sourceNodeId: '2025', destinationNodeId: '2026', description: 'Automation_Connection', isActive: true, isTransfer: true, algorithmId: '1', products: []}";
        public const string UpdateNodeConnectionProductOwnersForOfficialBalanceFile = "{'owners':[{'ownerId':30,'ownershipPercentage':100}],'productId':2562,'rowVersion':'AAAAAAACUt8='}";
        public const string HomologationForLogisticProducts = "{sourceSystemId:3,destinationSystemId:1,homologationGroups:[{groupTypeId:14,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:11},{isRequiredMapping:true,HomologationObjectTypeId:14},{isRequiredMapping:true,HomologationObjectTypeId:3}],homologationDataMapping:[{sourceValue:'CRUDO CAMPO MAMBO',destinationValue:10000002318},{sourceValue:'CRUDO CAMPO CUSUCO',destinationValue:10000002372},{sourceValue:'CRUDO CAMPO PASTINACA',destinationValue:10000002373},{sourceValue:'GAS GUAJIRA',destinationValue:40000003009}]}]}";
        public const string HomologationForExcelWithOfficialSourceSystem = "{sourceSystemId:3,destinationSystemId:1,homologationGroups:[{groupTypeId:22,homologationObjects:[{isRequiredMapping:true,HomologationObjectTypeId:27}],homologationDataMapping:[{sourceValue:'ManualInvOficial',destinationValue:189}, {sourceValue:'ManualMovOficial',destinationValue:190}]}]}";
        public const string SaveApprovalRequestResult = "{deltaNodeId:'19',approverAlias:'trueadmin',comment:'Approving request',status:'APPROVED'}";

        public static Dictionary<string, string> Routes { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Categories", "categories" },
            { "Category Elements", "categoryelements" },
            { "Category Element", "categoryelements" },
            { "Category's Elements", "categories/{0}/elements" },
            { "Category Name", "categories/{categoryName}/exists" },
            { "Element Name", "categories/{categoryId}/elements/{elementName}/exists" },
            { "Logistic Centers", "logisticcenters" },
            { "Node", "nodes" },
            { "Node Name", "nodes/{nodeName}/exists" },
            { "Nodes", "nodes" },
            { "Change Delta Node Status", "nodes/delta" },
            { "Get Delta Node details", "nodes/{deltanNodeId}/delta" },
            { "node-connection", "nodeconnections" },
            { "Transport Nodes", "nodes" },
            { "Transport Node", "nodes" },
            { "Storage Locations", "storagelocations" },
            { "Transport Category", "categories" },
            { "Transport Categories", "categories" },
            { "StorageLocation Name", "nodes/{nodeId}/storagelocations/{StorageLocationName}/exists" },
            { "Homologations", "homologations" },
            { "GroupTypeId", "homologations/{homologationId}/groups/{groupTypeId}" },
            { "HomologationGroupName", "homologations/{homologationId}/groups/name/{groupName}" },
            { "Homologation", "homologations" },
            { "HomologationGroup", "homologations" },
            { "HomologationWithTwoGroups", "homologations" },
            { "Movements", "movements" },
            { "MovementsHomologated", "movements" },
            { "Inventory", "inventory" },
            { "Inventories", "inventories" },
            { "InventoriesHomologated", "inventories" },
            { "NodeProductUncertainity", "nodes/locations/products" },
            { "NodeAddProperties", "nodes?$expand=nodestoragelocations($expand=products)&$filter=NodeId eq {NodeId}" },
            { "NodeUpdateOwner", "nodes/locations/products/{storageLocationProductId}/owners" },
            { "Node Tags", "nodetags" },
            { "Node Tag", "nodetags" },
            { "Tickets", "tickets" },
            { "File Registrations", "fileregistrations" },
            { "File Registration", "fileregistrations" },
            { "Rules", "rules" },
            { "Products", "products" },
            { "Scenarios", "scenarios" },
            { "FilterNodes", "nodes/filter" },
            { "TicketInfo", "ticketinfo" },
            { "node-connection products", "nodeconnections/{connectionId}/products" },
            { "node-connection product", "/nodeconnections/products" },
            { "node-connection productowner", "/nodeconnections/products/{productId}/owners" },
            { "nodetags", "nodetag" },
            { "StorageLocation", "nodes/{nodeId}/storagelocations/" },
            { "StorageLocation Products", "nodes/{nodeId}/storagelocations/products" },
            { "operationalcutoff", "/operationalcutoff" },
            { "fileregistration", "/fileregistration" },
            { "fileregistration status",  "/fileregistration/status" },
            { "BlobFileName", "/fileregistrations/{blobFileName}/uploadaccessinfo" },
            { "TransactionErrors", "/transactions/pending" },
            { "Unbalances", "/unbalances" },
            { "ownership details", "ownership" },
            { "ReadAccessInfo", "/fileregistration/readaccessinfo" },
            { "UpdateOwnershipRule", "nodes/rules" },
            { "CreatedHomologationWithTwoGroups", "homologations/{homologationId}/groups/{homologationGroupId}" },
            { "HomologationGroups", "homologationGroups?$expand=homologation($expand=sourceSystem,destinationSystem),group,homologationObjects,homologationDataMapping" },
            { "HomologationGroupByGroupType", "homologationGroups?$expand=homologation($expand=sourceSystem,destinationSystem),group,homologationObjects,homologationDataMapping&$filter=groupTypeId eq 13 and homologation/sourceSystemId eq 1 and homologation/destinationSystemId eq 2" },
            { "HomologationDataMapping", "HomologationDataMapping" },
            { "HomologationObject", "HomologationObject" },
            { "UpdateNodeProducts", "/nodes/locations/products" },
            { "TicketRequest", "operationalcutoff" },
            { "DeadletteredMessages", "failures" },
            { "ExcelMovementAndInventoryRequest", "fileregistration" },
            { "Official", "movements/official" },
            { "UpdateNodeProductOwners", "nodes/locations/products/owners" },
        };

        public static Dictionary<string, string> InputFolder { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Movements", "movements" },
            { "Inventory", "inventory" },
        };

        public static Dictionary<string, string> OutFolder { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Movements", "movements-out" },
            { "Inventory", "inventory-out" },
        };

        public static Dictionary<string, string> OutputFolder { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Movements", "movement" },
            { "Inventory", "inventory" },
        };

        public static Dictionary<string, string> Creates { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Category Element", CreateCategoryElement },
            { "Category Elements", CreateCategoryElement },
            { "Node", CreateNode },
            { "Nodes", CreateNode },
            { "node-connection", CreateConnection },
            { "Approve Node Status", SaveApprovalRequestResult },
            { "Transport Node", CreateNode },
            { "Transport Nodes", CreateNode },
            { "Transport Categories", CreateCategory },
            { "Transport Category", CreateCategory },
            { "Homologation",  CreateHomologation },
            { "Homologations",  CreateHomologation },
            { "HomologationGroup",  CreateHomologation },
            { "HomologationWithTwoGroups",  CreateHomologationWithTwoGroups },
            { "SetupHomologation",  SetupHomologation },
            { "SetupHomologationWithoutTank",  SetupHomologationWithoutTank },
            { "SetupHomologationForXL",  SetupHomologationForXL },
            { "node-connection-1Product", CreateConnectionWithOneProduct },
            { "node-connection-2Products", CreateConnectionWithTwoProducts },
            { "CreateNodeWithMutipleProducts", CreateNodeWithMutipleProducts },
            { "CreateNodeWithOneProduct", CreateNodeWithOneProduct },
            { "CreateNodeWithOwnership", UpdateOwnershipRule },
            { "SetupHomologationForCutoff", SetupHomologationForCutoff },
            { "FilterNodes", FilterNodes },
            { "operationalcutoff", OperationalCutoff },
            { "fileregistration", RegisterFile },
            { "CreateOwnership", UpdateOwnershipRule },
            { "HomologationForNodes", HomologationForNodes },
            { "HomologationForProduct", HomologationForProduct },
            { "HomologationForUnit", HomologationForUnit },
            { "HomologationForMovementTypeId", HomologationForMovementTypeId },
            { "HomologationForProductTypeId", HomologationForProductTypeId },
            { "HomologationForOwner", HomologationForOwner },
            { "HomologationForEventNodes", HomologationForEventNodes },
            { "HomologationForEventType", HomologationForEventType },
            { "ownershipdetails", OwnershipDetails },
            { "HomologationForEventOwner", HomologationForEventOwner },
            { "HomologationForOperator", HomologationForOperator },
            { "HomologationForNodesWithTank", HomologationForNodesWithTank },
            { "HomologationForContractNodes", HomologationForContractNodes },
            { "HomologationForContractProduct", HomologationForContractProduct },
            { "HomologationForContractUnit", HomologationForContractUnit },
            { "HomologationForContractMovementTypeId", HomologationForContractMovementTypeId },
            { "HomologationForContractCommercial", HomologationForContractCommercial },
            { "HomologationForSegment", HomologationForSegment },
            { "NodeWithSendToSAPTrue", NodeWithSendToSAPTrue },
            { "HomologationForProductSAP",  HomologationForProductSAP },
            { "HomologationForMovementTypeIdForSIV", HomologationForMovementTypeIdForSIV },
            { "Movements", ProcessMovement },
            { "Inventories", ProcessInventory },
            { "MovementsHomologated", ProcessMovementWithHomologated },
            { "InventoriesHomologated", ProcessInventoryWithHomologated },
            { "FicoConnection", CreateFicoConnection },
            { "UpdateNodeConnectionProduct", UpdateNodeConnectionProduct },
            { "UpdateNodeOwners", UpdateNodeOwners },
            { "InventoriesWithMultipleProducts", ProcessInventoryWithMultipleProducts },
            { "NodeWithMultipleProduct", CreateNodeWithMultipleProduct },
            { "OperationalCutoffTicketRequest", OperationalCutoffTicketRequest },
            { "OwnershipTicketRequest", OwnershipTicketRequest },
            { "OfficialDeltaTicketRequest", OfficialDeltaTicketRequest },
            { "ExcelMovementAndInventoryRequest", ExcelMovementAndInventoryRequest },
            { "ServiceBusMessageForSinoper", ServiceBusMessageForSinoper },
            { "HomologationForAttributeId", HomologationForAttributeId },
            { "HomologationForValueAttributeUnit", HomologationForValueAttributeUnit },
            { "HomologationForStorageLocation", HomologationForStorageLocation },
            { "HomologationForSystem", HomologationForSystem },
            { "HomologationForSourceSystem", HomologationForSourceSystem },
            { "InventoriesWithTwoSameProduct", ProcessInventoryWithTwoSameProduct },
            { "HomologationForSourceSystemExcel", HomologationForSourceSystemExcel },
            { "UpdateNodeProductOwners", UpdateNodeProductOwners },
            { "CreateNodeWithMutipleProductsForOfficialBalanceFile", CreateNodeWithMutipleProductsForOfficialBalanceFile },
            { "CreateNodeWithOneProductForOfficialBalanceFile", CreateNodeWithOneProductForOfficialBalanceFile },
            { "UpdateNodeProductOwnersForOfficialBalanceFile", UpdateNodeProductOwnersForOfficialBalanceFile },
            { "CreateConnectionForOfficialBalanceFile", CreateConnectionForOfficialBalanceFile },
            { "HomologationForLogisticProducts", HomologationForLogisticProducts },
            { "HomologationForExcelWithOfficialSourceSystem", HomologationForExcelWithOfficialSourceSystem },
            { "HomologationDataMapping", HomologationDataMapping },
        };

        public static Dictionary<string, string> Updates { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Category Element", UpdateCategoryElement },
            { "Category Elements", UpdateCategoryElement },
            { "Category's Elements", UpdateCategoryElement },
            { "Transport Category", UpdateCategory },
            { "Transport Categories", UpdateCategory },
            { "Node", UpdateNode },
            { "Nodes", UpdateNode },
            { "node-connection", UpdateConnection },
            { "Transport Node", UpdateNode },
            { "Transport Nodes", UpdateNode },
            { "node-connection product", UpdateConnectionProduct },
            { "node-connection productowner", UpdateConnectionProductOwner },
            { "nodetags", AssociateNodes },
            { "TransactionErrors", PendingTransactions },
            { "Unbalances", Unbalances },
            { "UpdateNodeConnectionProductOwners", UpdateNodeConnectionProductOwners },
            { "UpdateNodeConnectionProductOwnersForOfficialBalanceFile", UpdateNodeConnectionProductOwnersForOfficialBalanceFile },
        };

        public static Dictionary<string, string> Ids { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Category Element", "elementId" },
            { "Category Elements", "elementId" },
            { "Category's Elements", "categoryId" },
            { "Transport Category", "categoryId" },
            { "Transport Categories", "categoryId" },
            { "Node", "nodeId" },
            { "Nodes", "nodeId" },
            { "Transport Node", "nodeId" },
            { "Transport Nodes", "nodeId" },
            { "StorageLocation NodeStorageLocationId", "nodeStorageLocationId" },
            { "Products", "productId" },
            { "StorageLocations", "nodeStorageLocationId" },
            { "Categories", "elementId" },
            { "Owners", "elementId" },
            { "SourceNodeId", "SourceNodeId" },
            { "DestinationNodeId", "DestinationNodeId" },
            { "node-connection", "nodeConnectionId" },
            { "Homologations", "homologationId" },
            { "GroupTypeId", "groupTypeId" },
            { "Inventory", "FileRegistrationTransactionId" },
            { "Movements", "FileRegistrationTransactionId" },
            { "TicketInfo", "TicketId" },
            { "HomologationWithTwoGroups", "homologationGroupId" },
            { "HomologationGroup", "homologationGroupId" },
            { "HomologationObjectType", "homologationObjectTypeId" },
        };

        public static Dictionary<string, string> Homologation { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Nodes", "HomologationGroup GroupTypeId" },
            { "StorageLocations", "HomologationGroup GroupTypeId" },
            { "Products", "HomologationGroup GroupTypeId" },
            { "Owners", "HomologationGroup GroupTypeId" },
            { "Source", "HomologationDataMapping DestinationValue" },
            { "Destination", "HomologationDataMapping SourceValue" },
        };

        public static Dictionary<string, string> Pipeline { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "OperativeMovements", "ADF_OperativeMovementsHistory" },
            { "OperativeMovementsWithOwnership", "ADF_OperativeMovementswithOwnerShip" },
            { "OperativeNodeRelationship", "ADF_OperativeNodeRelationShipHistory" },
            { "OperativeNodeRelationshipWithOwnership", "ADF_OperativeNodeRelationShipwithOwnerShipHistory" },
            { "OwnershipPercentageValues", "ADF_OwnershipPercentageValuesHistory" },
        };

        public static Dictionary<string, string> LastCreated { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Category Element", SqlQueries.GetLastCategoryElement },
            { "Category Elements", SqlQueries.GetLastCategoryElement },
            { "Category's Elements", SqlQueries.GetLastCategoryElement },
            { "Transport Category", SqlQueries.GetLastCategory },
            { "Transport Categories", SqlQueries.GetLastCategory },
            { "Node", SqlQueries.GetLastNode },
            { "Nodes", SqlQueries.GetLastNode },
            { "Node Details", SqlQueries.GetNode },
            { "Transport Node", SqlQueries.GetLastNode },
            { "Transport Nodes", SqlQueries.GetLastNode },
            { "Storage Locations", SqlQueries.GetLastStorageLocation },
            { "Storage Location Status", SqlQueries.GetNodeStorageLocationStatus },
            { "Product Location Status", SqlQueries.GetProductLocationStatus },
            { "Audit Details", SqlQueries.GetAuditLogDetails },
            { "Homologation Audit Details", SqlQueries.GetAuditLogDetailsForHomologation },
            { "Product Locations", SqlQueries.GetLastStorageLocationProduct },
            { "StorageLocation Name", SqlQueries.GetStorageLocationName },
            { "NewAuditStorageLocation Name", SqlQueries.GetNewAuditStorageLocationName },
            { "Product Audit Details", SqlQueries.GetAuditLogDetailsForProduct },
            { "True System Id", SqlQueries.GetTrueId },
            { "Sinoper System Id", SqlQueries.GetSinoperId },
            { "Excel Id", SqlQueries.GetExcelId },
            { "Group Type Id", SqlQueries.GetGroupTypeId },
            { "node-connection", SqlQueries.GetLastNodeConnection },
            { "Movements", SqlQueries.GetLastMovement },
            { "Homologations", SqlQueries.GetLastHomologation },
            { "HomologationWithTwoGroups", SqlQueries.GetLastHomologationGroup },
            { "HomologationGroup", SqlQueries.GetLastHomologationGroup },
            { "GetOwnershipData", SqlQueries.GetOwnerDetails },
            { "Inventory", SqlQueries.GetLastInventory },
            { "FileRegistrationError", SqlQueries.GetLastFileRegistrationError },
            { "fileregistration", SqlQueries.GetLastFileRegistration },
            { "TicketInfo", SqlQueries.GetLastTicket },
            { "GetNodeInformation", SqlQueries.GetNodeInformation },
            { "GetSortedSegmentNodeInformation", SqlQueries.GetSortedSegmentNodeInformation },
            { "GetSortedNodeInformation", SqlQueries.GetSortedNodeInformation },
            { "GetSortedNodeTypeInformation", SqlQueries.GetSortedNodeTypeInformation },
            { "GetSortedOperatorNodeInformation", SqlQueries.GetSortedOperatorNodeInformation },
            { "GetTicketSortedByTicketIdForOwnershipForSegmentsGrid", SqlQueries.GetTicketSortedByTicketIdForOwnershipForSegmentsGrid },
            { "GetTicketSortedByStartDateForOwnershipForSegmentsGrid", SqlQueries.GetTicketSortedByStartDateForOwnershipForSegmentsGrid },
            { "GetTicketSortedByEndDateForOwnershipForSegmentsGrid", SqlQueries.GetTicketSortedByEndDateForOwnershipForSegmentsGrid },
            { "GetTicketSortedByCreatedTicketDateForOwnershipForSegmentsGrid", SqlQueries.GetTicketSortedByCreatedTicketDateForOwnershipForSegmentsGrid },
            { "GetTicketSortedByUsernameForOwnershipForSegmentsGrid", SqlQueries.GetTicketSortedByCreatedByForOwnershipForSegmentsGrid },
            { "GetTicketSortedBySegmentNameForOwnershipForSegmentsGrid", SqlQueries.GetTicketSortedBySegmentNameForOwnershipForSegmentsGrid },
            { "GetTicketSortedByStatusForOwnershipForSegmentsGrid", SqlQueries.GetTicketSortedByStatusForOwnershipForSegmentsGrid },
            { "GetTicketSortedByTicketIdForOwnershipForNodesGrid", SqlQueries.GetTicketSortedByTicketIdForOwnershipForNodesGrid },
            { "GetTicketSortedByStartDateForOwnershipForNodesGrid", SqlQueries.GetTicketSortedByStartDateForOwnershipForNodesGrid },
            { "GetTicketSortedByEndDateForOwnershipForNodesGrid", SqlQueries.GetTicketSortedByEndDateForOwnershipForNodesGrid },
            { "GetTicketSortedByCreatedTicketDateForOwnershipForNodesGrid", SqlQueries.GetTicketSortedByCreatedTicketDateForOwnershipForNodesGrid },
            { "GetTicketSortedByUsernameForOwnershipForNodesGrid", SqlQueries.GetTicketSortedByCreatedByForOwnershipForNodesGrid },
            { "GetTicketSortedBySegmentNameForOwnershipForNodesGrid", SqlQueries.GetTicketSortedBySegmentNameForOwnershipForNodesGrid },
            { "GetTicketSortedByNodeNameForOwnershipForNodesGrid", SqlQueries.GetTicketSortedByNodeNameForOwnershipForNodesGrid },
            { "GetTicketSortedByStatusForOwnershipForNodesGrid", SqlQueries.GetTicketSortedByStatusForOwnershipForNodesGrid },
            { "GetTicketSortedBySegmentForReportLogisticGrid", SqlQueries.GetTicketSortedBySegmentForReportLogisticGrid },
            { "GetTicketSortedByOwnerForReportLogisticGrid", SqlQueries.GetTicketSortedByOwnerForReportLogisticGrid },
            { "GetTicketSortedByStartDateForReportLogisticGrid", SqlQueries.GetTicketSortedByStartDateForReportLogisticGrid },
            { "GetTicketSortedByEndDateForReportLogisticGrid", SqlQueries.GetTicketSortedByEndDateForReportLogisticGrid },
            { "GetTicketSortedByTicketCreatedDateForReportLogisticGrid", SqlQueries.GetTicketSortedByTicketCreatedDateForReportLogisticGrid },
            { "GetTicketSortedByUsernameForReportLogisticGrid", SqlQueries.GetTicketSortedByUsernameForReportLogisticGrid },
            { "GetTicketSortedByStatusForReportLogisticGrid", SqlQueries.GetTicketSortedByStatusForReportLogisticGrid },
            { "Contract", SqlQueries.GetLastContractId },
        };

        public static Dictionary<string, string> GetRow { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "StorageLocation", SqlQueries.GetStorageLocation },
            { "OperationalBalance", SqlQueries.GetOperationalBalance },
            { "Inputs", SqlQueries.GetInputData },
            { "Outputs", SqlQueries.GetOutputData },
            { "Losses", SqlQueries.GetLosses },
            { "Nodes", SqlQueries.GetNodeId },
            { "GetDeltaNode", SqlQueries.GetNodeWithDeltas },
            { "Products", SqlQueries.GetProductId },
            { "StorageLocations", SqlQueries.GetStorageLocationId },
            { "Owners", SqlQueries.GetOwnerId },
            { "Categories", SqlQueries.GetCategoryElementId },
            { "PendingTranasations", SqlQueries.GetPendingTransactions },
            { "Inventory",  SqlQueries.GetCreatedInventory },
            { "Inventory_ProductVolume",  SqlQueries.GetInventoryProductVolume },
            { "Movement_ProductVolume",  SqlQueries.GetMovementProductVolume },
            { "MovementIdentifier_ProductVolume",  SqlQueries.GetMovementProductVolume },
            { "GroupTypeId", SqlQueries.GetHomologationGroupId },
            { "HomologationGroupName", SqlQueries.GetHomologationGroupName },
            { "HomologationId", SqlQueries.GetHomologationBySourceAndDestinationForXml },
            { "HomologationIdForXL", SqlQueries.GetHomologationBySourceAndDestinationForXL },
            { "HomologationIdForSIV", SqlQueries.GetHomologationBySourceAndDestinationForSIV },
            { "HomologationGroupId", SqlQueries.GetHomologationGroupByHomologationId },
            { "NodeStorageLocationByNodeId", SqlQueries.GetNodeStorageLocationByNodeId },
            { "StorageLocationProductByNodeStorageLocationId", SqlQueries.GetNodeStorageLocationByStorageLocationId },
            { "StorageLocationProductId", SqlQueries.GetStorageLocationProductId },
            { "InventoryProduct", SqlQueries.GetInventoryProductByInventoryId },
            { "InventoryByIds", SqlQueries.GetMultipleInventories },
            { "MovementsByIds", SqlQueries.GetMultipleMovements },
            { "NodeConnection", SqlQueries.GetNodeConnection },
            { "node-connection product", SqlQueries.NodeConnectionProduct },
            { "FileId", SqlQueries.GetRandomFileId },
            { "HomologationIdForEvent", SqlQueries.GetHomologationBySourceAndDestinationForEvent },
            { "HomologationIdForContracts", SqlQueries.GetHomologationBySourceAndDestinationForContracts },
            { "OperativeMovements", SqlQueries.GetOperativeMovements },
            { "OperativeMovementsWithOwnership", SqlQueries.GetOperativeMovementsWithOwnership },
            { "OperativeNodeRelationship", SqlQueries.GetOperativeNodeRelationship },
            { "OperativeNodeRelationshipWithOwnership", SqlQueries.GetOperativeNodeRelationshipWithOwnership },
            { "NodeConnectionProductByNodeConnnectionID", SqlQueries.GetNodeConnectionProductByNodeConnnectionID },
            { "CategoryByName", SqlQueries.GetCategoryByCategoryName },
            { "SAPHomologation", SqlQueries.GetHomologationBySourceAndDestinationForSAP },
            { "FileRegistrationUsingUploadID", SqlQueries.GetFileRegistrationUsingUploadId },
            { "OwnershipPercentageValues", SqlQueries.GetOwnershipPercentageValues },
            { "IventoryWithMultipleProducts", SqlQueries.GetInventoryWithMultipleProducts },
            { "MovementById", SqlQueries.GetMovementId },
            { "InventoryById", SqlQueries.GetInventoryId },
            { "AttributeByInventoryProductId", SqlQueries.GetAttributeByInventoryProductId },
            { "OwnerByInventoryProductId", SqlQueries.GetOwnerByInventoryProductId },
            { "GetNodeWithSendForApproval", SqlQueries.GetNodeWithSendForApproval },
            { "GetDeltaApproverAndRequestorPerNode", SqlQueries.GetDeltaApproverAndRequestorPerNode },
            { "GetNodeApprovalCommentAndDate", SqlQueries.GetApprovalCommentAndDate },
        };

        public static Dictionary<string, string> InsertRow { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Movements", SqlQueries.InsertMovement },
            { "MovementPeriod", SqlQueries.InsertMovementPeriod },
            { "MovementSource", SqlQueries.InsertMovementSource },
            { "MovementDestination", SqlQueries.InsertMovementDestination },
            { "TagSystemElementWithNode", SqlQueries.TagSystemElementWithNode },
        };

        public static Dictionary<string, string> Counts { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "OperativeMovements", SqlQueries.GetOperativeMovementsCount },
            { "OperativeMovementsWithOwnership", SqlQueries.GetOperativeMovementsWithOwnershipCount },
            { "OperativeNodeRelationship", SqlQueries.GetOperativeNodeRelationshipCount },
            { "OperativeNodeRelationshipWithOwnership", SqlQueries.GetOperativeNodeRelationshipWithOwnershipCount },
            { "Icons", SqlQueries.GetIconCount },
            { "OwnershipPercentageValues", SqlQueries.GetOwnershipPercentageValuesCount },
            { "ADFUpdatedOwnershipPercentageValuesRecords", SqlQueries.GetUpdatedOwnershipPercentageValuesCount },
        };

        public static Dictionary<string, string> FileNames { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "OperativeMovements", "MOV_OP_CONSOLIDADOS_TRUE" },
            { "OperativeMovementsWithOwnership", "MOV_TT_CONSOLIDADOS_TRUE" },
            { "OperativeNodeRelationship", "RELACION_NODOS_OP_TRUE" },
            { "OperativeNodeRelationshipWithOwnership", "RELACION_NODOS_TT_TRUE" },
            { "OperativeMovementsHistory", "MOV_OP_CONSOLIDADOS_TRUE" },
            { "OperativeMovementsWithOwnershipHistory", "MOV_TT_CONSOLIDADOS_TRUE" },
            { "OwnershipPercentageValues", "OwnershipPercentageValues" },
        };

        public static Dictionary<string, string> ContainerNames { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "OperativeMovements", "operativemovementshistory" },
            { "OperativeMovementsWithOwnership", "operativemovementswithownershiphistory" },
            { "OperativeNodeRelationship", "nodes" },
            { "OperativeNodeRelationshipWithOwnership", "nodes" },
            { "OperativeMovementsHistory", "operativemovementshistory" },
            { "Ownership", "ownership" },
            { "OwnershipPercentageValues", "workfiles" },
        };

        public static Dictionary<string, string> UpdateRow { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "UpdateNodeTagDate", SqlQueries.UpdateNodeTagStartDateByNodeId },
            { "UpdateSegmentToSONSegment", SqlQueries.UpdateIsOperationalForSegnement },
        };

        public static Dictionary<string, string> LogType { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Create", "Insert" },
            { "Update", "Update" },
            { "Delete", "Update" },
        };

        public static Dictionary<string, string> HomologateLogType { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Create", "Insert" },
            { "Update", "Update" },
            { "Delete", "Delete" },
            { "Insert", "Insert" },
        };

        public static Dictionary<string, string> UpdateXML { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "MOVEMENT DATE", "INTERNALMOVEMENTS" },
            { "MovementSystem", "INTERNALMOVEMENTS" },
            { "MOVEMENTID", "INTERNALMOVEMENTS/INTERNALMOVEMENT" },
            { "MovementType", "INTERNALMOVEMENTS/INTERNALMOVEMENT" },
            { "MovementCommodity", "INTERNALMOVEMENTS/INTERNALMOVEMENT" },
            { "MovementOwnerId", "INTERNALMOVEMENTS/INTERNALMOVEMENT/OWNERS/OWNER" },
            { "MovementOwnerType", "INTERNALMOVEMENTS/INTERNALMOVEMENT/OWNERS/OWNER" },
            { "MovementValue", "INTERNALMOVEMENTS/INTERNALMOVEMENT/OWNERS/OWNER/QUANTITY" },
            { "Movement", "INTERNALMOVEMENTS/INTERNALMOVEMENT/OWNERS/OWNER/QUANTITY" },
            { "MovementProperty", "INTERNALMOVEMENTS/INTERNALMOVEMENT/OWNERS/OWNER/QUANTITY" },
            { "BatchId", "INTERNALMOVEMENTS/INTERNALMOVEMENT/BATCHES/BATCH" },
            { "MovementCommodityName", "INTERNALMOVEMENTS/INTERNALMOVEMENT/BATCHES/BATCH" },
            { "MovementCommodityType", "INTERNALMOVEMENTS/INTERNALMOVEMENT/BATCHES/BATCH" },
            { "StartTime", "INTERNALMOVEMENTS/INTERNALMOVEMENT/PERIOD" },
            { "EndTime", "INTERNALMOVEMENTS/INTERNALMOVEMENT/PERIOD" },
            { "MovementCase", "INTERNALMOVEMENTS/INTERNALMOVEMENT" },
            { "Source", "INTERNALMOVEMENTS/INTERNALMOVEMENT/LOCATION" },
            { "SourceType", "INTERNALMOVEMENTS/INTERNALMOVEMENT/LOCATION" },
            { "Destination", "INTERNALMOVEMENTS/INTERNALMOVEMENT/LOCATION" },
            { "DestinationType", "INTERNALMOVEMENTS/INTERNALMOVEMENT/LOCATION" },
            { "MovementCriterionValue", "INTERNALMOVEMENTS/INTERNALMOVEMENT/CRITERIA/CRITERION" },
            { "MovementCriterionValueType", "INTERNALMOVEMENTS/INTERNALMOVEMENT/CRITERIA/CRITERION" },
            { "MovementCriterionProperty", "INTERNALMOVEMENTS/INTERNALMOVEMENT/CRITERIA/CRITERION" },
            { "MovementCriterionUom", "INTERNALMOVEMENTS/INTERNALMOVEMENT/CRITERIA/CRITERION" },
            { "MovementCriterionDescription", "INTERNALMOVEMENTS/INTERNALMOVEMENT/CRITERIA/CRITERION" },
            { "InventoriesDate", "INVENTORIES" },
            { "InventorySystem", "INVENTORIES" },
            { "Inventory", "INVENTORIES" },
            { "LocationId", "INVENTORIES/INVENTORY" },
            { "LocationType", "INVENTORIES/INVENTORY" },
            { "InventoryCase", "INVENTORIES/INVENTORY" },
            { "DATE", "INVENTORIES/INVENTORY" },
            { "COMMODITYNAME", "INVENTORIES/INVENTORY/COMMODITIES/COMMODITY" },
            { "InventoryCommodityType", "INVENTORIES/INVENTORY/COMMODITIES/COMMODITY" },
            { "InventoryBatchId", "INVENTORIES/INVENTORY/COMMODITIES/COMMODITY" },
            { "Orden", "INVENTORIES/INVENTORY/COMMODITIES/COMMODITY" },
            { "InventoryCriterionValue", "INVENTORIES/INVENTORY/COMMODITIES/COMMODITY/CRITERIA/CRITERION" },
            { "Inventory UOM", "INVENTORIES/INVENTORY/COMMODITIES/COMMODITY/CRITERIA/CRITERION" },
            { "InventoryCriterionProperty", "INVENTORIES/INVENTORY/COMMODITIES/COMMODITY/CRITERIA/CRITERION" },
            { "InventoryOwnerId", "INVENTORIES/INVENTORY/COMMODITIES/COMMODITY/OWNERS/OWNER" },
            { "InventoryOwnerType", "INVENTORIES/INVENTORY/COMMODITIES/COMMODITY/OWNERS/OWNER" },
            { "InventoryOwnerValue", "INVENTORIES/INVENTORY/COMMODITIES/COMMODITY/OWNERS/OWNER/QUANTITY" },
            { "InventoryOwnerUom", "INVENTORIES/INVENTORY/COMMODITIES/COMMODITY/OWNERS/OWNER/QUANTITY" },
            { "InventoryOwnerProperty", "INVENTORIES/INVENTORY/COMMODITIES/COMMODITY/OWNERS/OWNER/QUANTITY" },
            { "Movement UOM", "INTERNALMOVEMENTS/INTERNALMOVEMENT/CRITERIA/CRITERION" },
            { "BATCHID_1", "INVENTORIES/INVENTORY/COMMODITIES/COMMODITY[1]" },
            { "BATCHID_2", "INVENTORIES/INVENTORY/COMMODITIES/COMMODITY[2]" },
        };

        public static Dictionary<string, string> Conversion { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Tanque Name", "LocationId" },
            { "Product", "CommodityName" },
            { "EventType", "Inventory" },
        };

        public static Dictionary<string, string> CountsWithDifferentOperationalDate { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "OperativeMovements", SqlQueries.GetOperativeMovementsCountwithHistoricalInformation },
            { "OperativeMovementsWithOwnership", SqlQueries.GetOperativeMovementsWithOwnershipCountwithHistoricalInformation },
        };

        public static Dictionary<string, string> CountWithoutHistoricalInformations { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "OperativeMovements", SqlQueries.GetOperativeMovementsCountwithdifferentHistoricalInformation },
            { "OperativeMovementsWithOwnership", SqlQueries.GetOperativeMovementsWithOwnershipCountwithdifferentHistoricalInformation },
        };

        public static Dictionary<string, string> CountsWithOtherOperationalDate { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "OperativeMovements", SqlQueries.GetCountOfOperativeMovementswithdifferentHistoricalInformation },
            { "OperativeMovementsWithOwnership", SqlQueries.GetCountOfOperativeMovementsWithOwnershipwithdifferentHistoricalInformation },
        };
    }
}