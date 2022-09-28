import { apiService } from '../../../common/services/apiService';
import { systemConfigService } from '../../../common/services/systemConfigService';
import { constants } from '../../../common/services/constants';

describe('api service',
    () => {
        beforeAll(() => {
            systemConfigService.initialize({
                autocompleteItemsCount: 50
            });
        });

        it('should build api for query nodeConnection',
            () => {
                expect(apiService.nodeConnection.query())
                    .toMatch('$expand=SourceNode($select=name, isActive),DestinationNode($select=name, isActive),Algorithm&$select=nodeConnectionId,sourceNodeId,destinationNodeId,controlLimit,isActive,isTransfer,algorithmId');
            });
        it('should build api for queryById nodeConnection',
            () => {
                expect(apiService.nodeConnection.queryById())
                    .toMatch('$expand=SourceNode($select=name, isActive),DestinationNode($select=name, isActive),Algorithm&$select=nodeConnectionId,sourceNodeId,destinationNodeId,controlLimit,isActive,isTransfer,algorithmId');
            });
        it('should build api for update nodeConnection',
            () => {
                expect(apiService.nodeConnection.createOrUpdate()).toMatch('nodeconnections');
            });
        it('should build api for queryProducts nodeConnection',
            () => {
                expect(apiService.nodeConnection.queryProducts(1))
                    .toMatch('$expand=Product($select=name),Owners($select=ownerId,ownershipPercentage;$expand=Owner($select=name,color))');
            });
        it('should build api for updateProduct nodeConnection',
            () => {
                expect(apiService.nodeConnection.updateProduct()).toMatch('nodeconnections/products');
            });
        it('should build api for getOwners nodeConnection',
            () => {
                expect(apiService.nodeConnection.getOwners()).toMatch('categories/7/elements');
            });
        it('should build api for updateOwners nodeConnection',
            () => {
                expect(apiService.nodeConnection.updateOwners()).toMatch('owners');
            });
        it('should build api for query node',
            () => {
                expect(apiService.node.query()).toMatch('$select=nodeId,name,controlLimit,acceptableBalancePercentage,isActive');
            });
        it('should build api for getById node',
            () => {
                expect(apiService.node.getById()).toMatch('nodes');
            });
        it('should build api for update node',
            () => {
                expect(apiService.node.update()).toMatch('nodes/attributes');
            });
        it('should build api for queryProducts node',
            () => {
                expect(apiService.node.queryProducts())
                    .toMatch('?$expand=NodeStorageLocation($select=name),Product($select=name),Owners($select=ownerId,ownershipPercentage;$expand=Owner($select=name,color))');
            });
        it('should build api for updateProduct node',
            () => {
                expect(apiService.node.updateProduct()).toMatch('nodes/locations/products');
            });
        it('should build api for updateOwners node',
            () => {
                expect(apiService.node.updateOwners()).toMatch('nodes/locations/products/owners');
            });
        it('should build api for query nodeTagging',
            () => {
                expect(apiService.node.query()).toMatch('nodes?$expand=NodeOwnershipRule($select=ruleName)&$select=nodeId,name,controlLimit,acceptableBalancePercentage,isActive');
            });
        it('should build api for queryTags nodeTagging',
            () => {
                expect(apiService.node.queryTags()).toMatch('$expand=categoryElement($expand=category),node');
            });
        it('should build api for tagNode nodeTagging',
            () => {
                expect(apiService.node.tagNode()).toMatch('nodetag');
            });
        it('should build api to get nodes',
            () => {
                expect(apiService.node.searchNodesOfSegment(123, 'test')).toMatch(`nodeTags?$filter=elementId eq 123 and contains(Node/name,'test')&$expand=node&$orderby=Node/name`);
            });
        it('should build api for query category',
            () => {
                expect(apiService.category.query()).toMatch('categories');
            });
        it('should build api for query all category',
            () => {
                expect(apiService.category.getAll()).toMatch('categories');
            });
        it('should build api for createOrUpdate category',
            () => {
                expect(apiService.category.createOrUpdate()).toMatch('categories');
            });
        it('should build api for validateCategoryName category',
            () => {
                expect(apiService.category.validateCategoryName()).toMatch('categories');
            });
        it('should build api for query fileUpload',
            () => {
                expect(apiService.fileUpload.query()).toMatch('fileregistrations');
            });
        it('should build api for create fileUpload',
            () => {
                expect(apiService.fileUpload.create()).toMatch('fileregistration');
            });
        it('should build api for getUploadAccessInfo fileUpload',
            () => {
                expect(apiService.fileUpload.getUploadAccessInfo()).toMatch('fileregistrations/undefined/3/uploadaccessinfo');
            });
        it('should build api for getUploadAccessInfo fileUpload with CONTRACT type',
            () => {
                expect(apiService.fileUpload.getUploadAccessInfo('filename', constants.SystemType.CONTRACT)).toMatch('fileregistrations/filename/4/uploadaccessinfo');
            });
        it('should build api for getReadAccessInfo fileUpload',
            () => {
                expect(apiService.fileUpload.getReadAccessInfo()).toMatch('fileregistration/readaccessinfo');
            });

        it('should build api for getAppInfo',
            () => {
                expect(apiService.bootstrap()).toMatch('bootstrap');
            });
        it('should build api for getCategoryElements',
            () => {
                expect(apiService.getCategoryElements()).toMatch('categoryelements?$expand=category');
            });
        it('should build api for getElements',
            () => {
                expect(apiService.getElements()).toMatch('categoryelements');
            });
        it('should build api for get last operational ticket',
            () => {
                expect(apiService.ticket.getLastOperationalTicketsPerSegment()).toMatch(`tickets?$filter=ticketTypeId eq 'Ownership'&$apply=groupby((categoryElementId,ticketTypeId), aggregate(ticketId with max as ticketId))`);
            });
        it('should build api for get last operational ticket with aditional filter',
            () => {
                expect(apiService.ticket.getLastOperationalTicketsPerSegmentWithStatus(['APPROVED'])).toMatch(`tickets?$filter=ticketTypeId eq 'Ownership'&$apply=filter(status eq 'APPROVED')/groupby((categoryElementId,ticketTypeId), aggregate(ticketId with max as ticketId))`);
            });
        it('should build api for getTickets',
            () => {
                expect(apiService.ticket.query()).toMatch('tickets?$expand=CategoryElement');
            });
        it('should build api for get last ticket',
            () => {
                expect(apiService.ticket.getLastTicket(1)).toMatch(`$top=1&$orderby=ticketId desc&$filter=ticketTypeId eq 'Cutoff' and categoryElementId eq 1 and status ne 'FAILED'`);
            });
        it('should build api for get ticket Information',
            () => {
                const ticketId = 10;
                expect(apiService.ticket.getTicketInformation(ticketId)).toMatch(`ticketinfo/${ticketId}`);
            });

        it('should build api for operationalCutOff for getPendingTransactionErrors',
            () => {
                expect(apiService.operationalCutOff.getPendingTransactionErrors()).toMatch(`transactions/pending`);
            });
        it('should build api for operationalCutOff for getUnbalances',
            () => {
                expect(apiService.operationalCutOff.getUnbalances()).toMatch(`unbalances`);
            });
        it('should build api for operationalCutOff for saveOperationalCutOff',
            () => {
                expect(apiService.operationalCutOff.saveOperationalCutOff()).toMatch(`operationalcutoff`);
            });
        it('should build api for operationalCutOff for validate',
            () => {
                expect(apiService.operationalCutOff.validate()).toMatch('cutoff/validate');
            });
        it('should build api for transformation for create',
            () => {
                expect(apiService.transformation.create()).toMatch('transformations');
            });
        it('should build api for transformation for query',
            () => {
                expect(apiService.transformation.query()).toMatch('transformations?$expand=OriginSourceNode');
            });
        it('should build api for transformation for getInfo',
            () => {
                expect(apiService.transformation.getInfo()).toMatch('info');
            });
        it('should build api for transformation for validate',
            () => {
                expect(apiService.transformation.validate()).toMatch('transformations/exists');
            });
        it('should build api for transformation for delete',
            () => {
                expect(apiService.transformation.delete()).toMatch('transformations');
            });
        it('should build api for getOwnershipNode',
            () => {
                expect(apiService.ownershipNode.query()).toMatch('ownershipnodes');
            });
        it('should build api for getOwnershipNodeErrors',
            () => {
                expect(apiService.ownershipNode.getErrors()).toMatch('ownershiperrors');
            });
        it('should build api for getById',
            () => {
                expect(apiService.ownershipNode.getById()).toMatch('nodeownership');
            });
        it('should build api for getOwnershipNodeBalance',
            () => {
                expect(apiService.ownershipNode.getOwnershipNodeBalance(32)).toMatch('ownershipnodes/32/balance');
            });
        it('should build api for getOwnershipMovementInventoryData',
            () => {
                expect(apiService.ownershipNode.getOwnershipMovementInventoryData(12)).toMatch('ownershipnodes/12/details');
            });
        it('should build api for getOwnersForMovement',
            () => {
                expect(apiService.ownershipNode.getOwnersForMovement()).toMatch('movements');
            });
        it('should build api for reopenTicket',
            () => {
                expect(apiService.ownershipNode.reopenTicket()).toMatch('reopenticket');
            });
        it('should build api for getOwnersForInventory',
            () => {
                expect(apiService.ownershipNode.getOwnersForInventory()).toMatch('nodeownership');
            });
        it('should build api for publishOwnerships',
            () => {
                expect(apiService.ownershipNode.publishOwnerships()).toMatch('ownershipnodes/publish');
            });
        it('should build api for getContractsForNewMovementsForNodeOwnership',
            () => {
                const selectedData = {
                    movementType: {
                        elementId: 1
                    },
                    sourceProduct: {
                        productId: 1
                    },
                    destinationProduct: {
                        productId: 1
                    },
                    sourceNodes: {
                        nodeId: 1
                    },
                    destinationNodes: {
                        nodeId: 1
                    }
                };
                expect(apiService.ownershipNode.getContractsForNewMovementsForNodeOwnership(selectedData, null)).toMatch('contracts');
            });
        it('should build api for editContracts',
            () => {
                const movement = {
                    sourceProductId: 1,
                    destinationProductId: 34,
                    destinationNodeId: 12,
                    operationalDate: 12,
                    contractId: 1,
                    movementTypeId: 12
                };
                expect(apiService.ownershipNode.editContracts(movement)).toMatch('contracts');
            });
        it('should build api for sendOwnershipNodeForApproval',
            () => {
                expect(apiService.ownershipNode.sendOwnershipNodeForApproval()).toMatch('nodeownership/submitforapproval');
            });
        it('should build api for getIcons',
            () => {
                expect(apiService.category.getIcons()).toMatch('icons');
            });
        it('should query operative transfer point relationships',
            () => {
                const route = 'operatives';
                expect(apiService.nodeRelationship.query(route)).toMatch(`operatives`);
            });
        it('should query logistic transfer point relationships',
            () => {
                const route = 'logistics';
                expect(apiService.nodeRelationship.query(route)).toMatch(`logistics`);
            });
        it('should perform crud on operative transfer point relationships',
            () => {
                const route = 'operatives';
                expect(apiService.nodeRelationship.update(route)).toMatch(`nodes/relationships/operatives`);
            });
        it('should perform crud on logistic transfer point relationships',
            () => {
                const route = 'logistics';
                expect(apiService.nodeRelationship.update(route)).toMatch(`nodes/relationships/logistics`);
            });
        it('should query node storage locations ',
            () => {
                expect(apiService.node.getNodeStorageLocations()).toMatch('nodestoragelocations');
            });
        it('should query segment ticket startdate ',
            () => {
                expect(apiService.ticket.getFinalSegmentTicket(1, 'Ownership', true)).
                    toMatch(`tickets?$top=1&$orderby=startdate desc&$filter=ticketTypeId eq 'Ownership' and categoryElementId eq 1 and status ne 'FAILED'`);
            });
        it('should query segment ticket enddate ',
            () => {
                expect(apiService.ticket.getFinalSegmentTicket(1, 'Ownership', false)).
                    toMatch(`tickets?$top=1&$orderby=enddate desc&$filter=ticketTypeId eq 'Ownership' and categoryElementId eq 1 and status ne 'FAILED'`);
            });
        it('should query ownership system tickets ',
            () => {
                expect(apiService.ticket.getFinalSystemTicket(1, 'Ownership')).
                    toMatch(`systemownershipcalculations?$expand=OwnershipTicket($select=endDate, ticketTypeId, status, ticketId)&$orderby=OwnershipTicket/enddate desc&$filter=systemId eq 1 and OwnershipTicket/ticketTypeId eq 'Ownership' and OwnershipTicket/status ne 'FAILED'`);
            });
        it('should query cutoff system tickets ',
            () => {
                expect(apiService.ticket.getFinalSystemTicket(1, 'Cutoff')).
                    toMatch(`systemunbalances?$expand=UnbalanceTicket($select=endDate, ticketTypeId, status, ticketId)&$orderby=UnbalanceTicket/enddate desc&$filter=systemId eq 1 and UnbalanceTicket/ticketTypeId eq 'Cutoff' and UnbalanceTicket/status ne 'FAILED'`);
            });
        it('should query postOperationalDataWithoutCutOff ',
            () => {
                expect(apiService.ticket.postOperationalDataWithoutCutOff()).toMatch('operationaldatawithoutcutoff');
            });
        it('should query postTicketNodeStatus ',
            () => {
                expect(apiService.ticket.postTicketNodeStatus()).toMatch('saveticketnodestatus');
            });
        it('should query postValidateNodesLogisticAvailables',
            () => {
                expect(apiService.ticket.postValidateNodesLogisticAvailables()).toMatch('logistics/validateNodesLogisticAvailables');
            });
        it('should query getOfficialLogisticsValidationData',
            () => {
                expect(apiService.ticket.getOfficialLogisticsValidationData()).toMatch('officialdelta/nodes/unapproved');
            });
        it('should query postNodeConfigurationReportRequest',
            () => {
                expect(apiService.ticket.postNodeConfigurationReportRequest()).toMatch('savenodeconfigurationreportrequest');
            });
        it('should query postOfficialNodeBalanceReport',
            () => {
                expect(apiService.ticket.postOfficialNodeBalanceReport()).toMatch('saveofficialnodebalance');
            });
        it('should build api for getLogisticCenters',
            () => {
                expect(apiService.getLogisticCenters()).toMatch('logisticcenters');
            });
        it('should build api for getReportConfig',
            () => {
                expect(apiService.getReportConfig('unbalanceReport')).toMatch('reportConfigs/unbalanceReport');
            });
        it('should build api for getSystemTypes',
            () => {
                expect(apiService.getSystemTypes()).toMatch('systemTypes');
            });
        it('should build api for getVariableTypes',
            () => {
                expect(apiService.getVariableTypes()).toMatch('variabletypes');
            });
        it('should build api for getUsers',
            () => {
                expect(apiService.getUsers()).toMatch('users');
            });
        it('should build api for getFlowConfig',
            () => {
                expect(apiService.getFlowConfig('config')).toMatch('flowConfigs/config');
            });
        it('should build api for getFilterCategoryElements',
            () => {
                expect(apiService.getFilterCategoryElements('$filter=id eq 2')).toMatch('categoryelements?$filter=id eq 2&$orderby=name asc');
            });
        it('should build api for updateDeviationPercentage',
            () => {
                expect(apiService.updateDeviationPercentage()).toMatch('segments/deviationpercentage');
            });
        it('should build api for getAlgorithmList',
            () => {
                expect(apiService.nodeConnection.getAlgorithmList()).toMatch('algorithms');
            });
        it('should build api for getSourceNodesByDestinationNode',
            () => {
                expect(apiService.nodeConnection.getSourceNodesByDestinationNode(1)).toMatch('nodeconnections?$expand=SourceNode($select=name,nodeId)&$select=SourceNode&$filter=destinationNodeId eq 1');
            });
        it('should build api for getDestinationNodesBySourceNode',
            () => {
                expect(apiService.nodeConnection.getDestinationNodesBySourceNode(1)).toMatch('nodeconnections?$expand=DestinationNode($select=name,nodeId)&$select=DestinationNode&$filter=sourceNodeId eq 1');
            });
        it('should build api for getOwnershipRules',
            () => {
                expect(apiService.nodeConnection.getOwnershipRules()).toMatch('nodeconnectionproductrules');
            });
        it('should build api for deleteNodeConnection',
            () => {
                expect(apiService.nodeConnection.deleteNodeConnection(1, 2)).toMatch('nodeconnections/1/2');
            });
        it('should build api for getProductRules',
            () => {
                expect(apiService.nodeConnection.getProductRules()).toMatch('nodeconnections/products/rules');
            });
        it('should build api for get delta exceptions details',
            () => {
                const ticket = {
                    ticketId: 10,
                    deltaNodeId: 11,
                    ticketTypeId: 'Delta'
                };
                expect(apiService.ticket.getDeltaExceptionsDetails(ticket.ticketId, ticket.ticketTypeId)).toMatch(`deltaExceptions/${10}`);
            });
        it('should build api for query report execution',
            () => {
                expect(apiService.reports.query()).toMatch('reportexecutions');
            });
    });
