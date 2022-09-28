import { utilities } from './utilities';
import { constants } from './constants';
import { systemConfigService } from './systemConfigService';

const apiService = (function () {
    const API_ROOT = window.location.origin;
    function createApiUrl(baseUrl) {
        return `${API_ROOT}/v1/${baseUrl}`;
    }

    function createQueryUrl(route, query) {
        // For V2 create a new function below
        const baseUrl = `${API_ROOT}/v1/odata/${route}`;
        if (query) {
            return `${baseUrl}?${query}`;
        }

        return baseUrl;
    }

    return {
        nodeConnection: {
            query: () => {
                const expand = '$expand=SourceNode($select=name, isActive),DestinationNode($select=name, isActive),Algorithm';
                const select = '$select=nodeConnectionId,sourceNodeId,destinationNodeId,controlLimit,isActive,isTransfer,algorithmId,rowVersion';
                return createQueryUrl('nodeconnections', `${expand}&${select}`);
            },
            queryById: connectionId => {
                const expand = '$expand=SourceNode($select=name, isActive),DestinationNode($select=name, isActive),Algorithm';
                const select = '$select=nodeConnectionId,sourceNodeId,destinationNodeId,controlLimit,isActive,isTransfer,algorithmId,rowVersion';
                const filter = `$filter=nodeConnectionId eq ${connectionId}`;

                return createQueryUrl(`nodeconnections`, `${expand}&${select}&${filter}`);
            },
            queryBySourceAndDestinationNodeId: (sourceNodeId, destinationNodeId) => {
                return createApiUrl(`nodeconnections/${sourceNodeId}/${destinationNodeId}`);
            },
            getDestinationNodes: sourceNodeId => {
                return createApiUrl(`nodeconnections/${sourceNodeId}/destinationnodes`);
            },
            createOrUpdate: () => {
                return createApiUrl('nodeconnections');
            },
            createUsingList: () => {
                return createApiUrl('nodeconnections/list');
            },
            queryProducts: connectionId => {
                const expand = `$expand=Product($select=name),Owners($select=ownerId,ownershipPercentage;$expand=Owner($select=name,color)),NodeConnectionProductRule($select=ruleId,ruleName)`;
                const select = '$select=nodeConnectionId,nodeConnectionProductId,productId,uncertaintyPercentage,priority,rowVersion,nodeConnectionProductRuleId';
                return createQueryUrl(`nodeconnections/${connectionId}/products`, `${expand}&${select}`);
            },
            updateProduct: () => {
                return createApiUrl('nodeconnections/products');
            },
            getOwners: () => {
                return createQueryUrl('categories/7/elements');
            },
            updateOwners: () => {
                return createApiUrl(`nodeconnections/products/owners`);
            },
            getAlgorithmList: () => {
                return createApiUrl('algorithms');
            },
            getSourceNodesByDestinationNode: destinationNodeId => {
                const expand = '$expand=SourceNode($select=name,nodeId)';
                const select = '$select=SourceNode';
                const filter = `$filter=destinationNodeId eq ${destinationNodeId}`;

                return createQueryUrl(`nodeconnections`, `${expand}&${select}&${filter}`);
            },
            getDestinationNodesBySourceNode: sourceNodeId => {
                const expand = '$expand=DestinationNode($select=name,nodeId)';
                const select = '$select=DestinationNode';
                const filter = `$filter=sourceNodeId eq ${sourceNodeId}`;

                return createQueryUrl(`nodeconnections`, `${expand}&${select}&${filter}`);
            },
            getOwnershipRules: () => {
                return createQueryUrl('nodeconnectionproductrules');
            },
            deleteNodeConnection: (sourceNodeId, destinationNodeId) => {
                return createApiUrl(`nodeconnections/${sourceNodeId}/${destinationNodeId}`);
            },
            getProductRules: () => {
                return createApiUrl('nodeconnections/products/rules');
            },
            getNodeCostCenter: nodeCostCenterId => {
                return createApiUrl(`nodecostcenters/${nodeCostCenterId}`);
            },
            getAllNodeCostCenter: () => {
                const expand = '$expand=SourceNode($select=name),DestinationNode($select=name),'
                    + 'CostCenterCategoryElement($select=name),MovementTypeCategoryElement($select=name)';
                return createQueryUrl(`nodecostcenters`, `${expand}`);
            },
            createNodeCostCenter: () => {
                return createApiUrl('nodecostcenters');
            },
            updateNodeCostCenter: () => {
                return createApiUrl('nodecostcenters');
            },
            deleteNodeCostCenter: nodeCostCenterId => {
                return createApiUrl(`nodecostcenters/${nodeCostCenterId}`);
            },
            getNodesBySegmentId: segmentId => {
                const filter = `$filter=NodeTags/any(x:x/CategoryElement/ElementId%20eq%20${segmentId})`;
                return createQueryUrl('nodes', filter);
            }

        },
        node: {
            query: () => {
                const expand = `$expand=NodeOwnershipRule($select=ruleName)`;
                const select = '$select=nodeId,name,controlLimit,acceptableBalancePercentage,isActive';
                return createQueryUrl('nodes', `${expand}&${select}`);
            },
            queryActive: () => {
                return createQueryUrl('nodes', `$filter=isActive eq true`);
            },
            getById: nodeId => {
                const expand = `$expand=nodeOwnershipRule($select=ruleId,ruleName)`;
                const filter = `$filter=nodeId eq ${nodeId}`;
                return createQueryUrl('nodes', `${expand}&${filter}`);
            },
            update: () => {
                return createApiUrl('nodes/attributes');
            },
            createOrUpdate: () => {
                return createApiUrl('nodes');
            },
            createGraphicalNode: () => {
                return createApiUrl('graphicalNode');
            },
            getUpdateNode: nodeId => {
                const expand = '$expand=NodeTags($expand=CategoryElement($select=elementId,name,categoryId,isActive)),' +
                    'logisticCenter($select=logisticCenterId,name),Unit($select=elementId,name,categoryId,isActive)';
                const filter = `$filter=nodeId eq ${nodeId}`;
                return createQueryUrl('nodes', `${expand}&${filter}`);
            },
            queryProducts: nodeId => {
                const expand = '$expand=NodeStorageLocation($select=name),Product($select=name),Owners($select=ownerId,ownershipPercentage;$expand=Owner($select=name,color)),' +
                    'nodeProductRule($select=ruleId,ruleName),storageLocationProductVariables($select=storageLocationProductVariableId,variableTypeId,rowVersion;' +
                    '$expand=variableType($select=name,shortName,ficoName,isConfigurable))';
                const select = `$select=storageLocationProductId,nodeStorageLocationId,productId,uncertaintyPercentage,isActive,rowVersion`;

                const filter = `$filter=NodeStorageLocation/Node/NodeId eq ${nodeId}`;

                return createQueryUrl(`storagelocationproducts`, `${expand}&${select}&${filter}`);
            },
            queryNodeProducts: nodeId => {
                const expand = `$expand=Product($select=name,productId)`;
                const select = `$select=productId,isActive`;

                const filter = `$filter=NodeStorageLocation/Node/NodeId eq ${nodeId}`;

                return createQueryUrl(`storagelocationproducts`, `${expand}&${select}&${filter}`);
            },
            queryRules: () => {
                return createQueryUrl(`nodeownershiprules`);
            },
            searchProducts: (searchText, storageLocationId) => {
                const order = `$orderby=name`;
                const filter = utilities.isNullOrWhitespace(storageLocationId)
                    ? `$filter=contains(name,'${encodeURIComponent(searchText)}') and isActive eq true` :
                    `$filter=StorageLocationProductMappings/any(x:x/StorageLocationId eq '${storageLocationId}') and contains(name,'${encodeURIComponent(searchText)}') and isActive eq true`;
                return createQueryUrl('products', `${filter}&${order}&$top=${systemConfigService.getAutocompleteItemsCount()}`);
            },
            searchNodeTags: (elementId, searchText) => {
                const order = `$orderby=node/name`;
                const apply = `$apply=filter(elementId eq ${elementId})/filter(contains(node/name,'${encodeURIComponent(searchText)}'))/groupby((node/nodeid, node/name))`;
                return createQueryUrl('nodeTags', `${apply}&${order}&&$top=${systemConfigService.getAutocompleteItemsCount()}`);
            },
            updateProduct: () => {
                return createApiUrl('nodes/locations/products');
            },
            updateOwners: () => {
                return createApiUrl(`nodes/locations/products/owners`);
            },
            queryTags: () => {
                return createQueryUrl('nodetags', `$expand=categoryElement($expand=category),node`);
            },
            tagNode: () => {
                return createApiUrl('nodetag');
            },
            getFilteredNodes: () => {
                return createApiUrl('nodes/filter');
            },

            QuerygetFilterNode: segmentId=>{
                const filter = `$filter=NodeTags/any(x:x/CategoryElement/ElementId eq ${segmentId})`;
                const expand = `$expand=NodeTags($expand=CategoryElement($expand=Category)),&$top=10`;
                const order = `?$apply=orderby((category/categoryId)`;
                return createQueryUrl(`nodes`, `${filter}&${order}&${expand}`);
            },

            getNodeStorageLocations: nodeId => {
                return createQueryUrl(`nodes/${nodeId}/nodestoragelocations`, `$expand=storageLocationType,products($expand=product,Owners,StorageLocationProductVariables),storageLocation`);
            },
            validateNodeName: name => {
                return createApiUrl(`nodes/${name}/exists`);
            },
            validateStorageLocationName: (nodeId, name) => {
                return createApiUrl(`nodes/${nodeId}/storageLocations/${name}/exists`);
            },
            validateNodeOrder: (nodeId, segmentId, order) => {
                return createApiUrl(`nodes/${nodeId}/${segmentId}/${order}`);
            },
            searchNodes: searchText => {
                const order = `$orderby=name`;
                const filter = `$filter=contains(name,'${encodeURIComponent(searchText)}') and isActive eq true`;
                return createQueryUrl('nodes', `${filter}&${order}&$top=${systemConfigService.getAutocompleteItemsCount()}`);
            },
            searchSendToSapNodes: (elementId, searchText) => {
                const order = `$orderby=Node/name`;
                const filter = `$filter=elementId eq ${elementId} and Node/SendToSAP eq true and contains(Node/name,'${encodeURIComponent(searchText)}')`;
                return createQueryUrl('nodeTags', `${filter}&$expand=node&${order}&$top=${systemConfigService.getAutocompleteItemsCount()}`);
            },
            searchNodesOfSegment: (elementId, searchText) => {
                const order = `$orderby=Node/name`;
                const filter = `$filter=elementId eq ${elementId} and contains(Node/name,'${encodeURIComponent(searchText)}')`;
                return createQueryUrl('nodeTags', `${filter}&$expand=node&${order}&$top=${systemConfigService.getAutocompleteItemsCount()}`);
            },
            getGraphicalNetwork: (elementId, nodeId) => {
                return createApiUrl(`nodeconnections/${nodeId}/elements/${elementId}`);
            },
            getLogisticCenter: nodeName => {
                return createApiUrl(`nodes/logisticcenter/${nodeName}`);
            },
            showAllSourceNodesDetails: destinationNodeId => {
                return createApiUrl(`nodeconnections/${destinationNodeId}/sourcenodesdetails`);
            },
            showAllDestinationNodesDetails: sourceNodeId => {
                return createApiUrl(`nodeconnections/${sourceNodeId}/destinationnodesdetails`);
            },
            getOwnershipRules: () => {
                return createQueryUrl('nodeproductrules');
            },
            getRules: () => {
                return createApiUrl('nodes/rules');
            },
            getProductRules: () => {
                return createApiUrl('nodes/products/rules');
            },
            bulkUpdateRules: () => {
                return createApiUrl(`nodes/rules`);
            }
        },
        blockchain: {
            getEvents: () => {
                return createApiUrl(`blockchain/events`);
            },
            getEventsInRange: () => {
                return createApiUrl(`blockchain/events/range`);
            },
            getTransaction: () => {
                return createApiUrl(`blockchain/transactions`);
            },
            transactionExists: () => {
                return createApiUrl(`blockchain/transactions/exists`);
            },
            rangeExists: () => {
                return createApiUrl(`blockchain/transactions/exists/range`);
            }
        },
        category: {
            query: () => {
                return createQueryUrl('categories');
            },
            getAll: isActive => {
                return createQueryUrl('categories', `$filter=isActive eq ${isActive} and isHomologation eq false`);
            },
            createOrUpdate: () => {
                return createApiUrl('categories');
            },
            validateCategoryName: name => {
                return createApiUrl(`categories/${name}/exists`);
            },
            queryElements: () => {
                return createQueryUrl('categoryelements', '$expand=category,icon');
            },
            createOrUpdateElements: () => {
                return createApiUrl('categoryelements');
            },
            validateElementName: () => {
                return createApiUrl(`categories/elements/exists`);
            },
            getIcons: () => {
                return createApiUrl('icons');
            },
            updateCategoryElements: () => {
                return createApiUrl('segments/operational');
            }
        },
        fileUpload: {
            query: () => {
                return createQueryUrl('fileregistrations');
            },
            create: () => {
                return createApiUrl('fileregistration');
            },
            getUploadAccessInfo: (fileName, selectedFileType) => {
                const type = utilities.isNullOrUndefined(selectedFileType) ? constants.SystemType.EXCEL : selectedFileType;
                return createApiUrl(`fileregistrations/${fileName}/${type}/uploadaccessinfo`);
            },
            getReadAccessInfo: () => {
                return createApiUrl('fileregistration/readaccessinfo');
            },
            getReadAccessInfoByContainer: container => {
                return createApiUrl('fileregistration/readaccessinfo/' + container);
            }
        },
        ticket: {
            getLastTicket: segmentId => {
                const order = `$orderby=ticketId desc`;
                const filter = `$filter=ticketTypeId eq 'Cutoff' and categoryElementId eq ${segmentId} and status ne 'FAILED'`;
                const top = `$top=1`;
                return createQueryUrl('tickets', `${top}&${order}&${filter}`);
            },
            query: () => {
                return createQueryUrl('tickets', '$expand=CategoryElement($expand=Category),Owner($select=name)');
            },
            getTicketInformation: ticketId => {
                return createApiUrl(`ticketinfo/${ticketId}`);
            },
            getTicketWithNodes: ticketId => {
                const expand = '$expand=TicketNodes($expand=Node)';
                const filter = `$filter=ticketId eq ${ticketId}`;
                return createQueryUrl('tickets', `${expand}&${filter}`);
            },
            getDeltaExceptionsDetails: (ticketId, ticketTypeId) => {
                return createApiUrl(`deltaExceptions/${ticketId}/${ticketTypeId}`);
            },
            getLastOperationalTicketsPerSegment: () => {
                return createQueryUrl('tickets', `$filter=ticketTypeId eq 'Ownership'&$apply=groupby((categoryElementId,ticketTypeId), aggregate(ticketId with max as ticketId))`);
            },
            getLastOperationalTicketsPerSegmentWithStatus: (statusFilter = []) => {
                const filter = statusFilter.map(status => `status eq '${status}'`).join(' or ');
                const apply = `$apply=filter(${filter})/groupby((categoryElementId,ticketTypeId), aggregate(ticketId with max as ticketId))`;
                return createQueryUrl('tickets', `$filter=ticketTypeId eq 'Ownership'&${apply}`);
            },
            getTickets: () => {
                return createQueryUrl('ticketentities');
            },
            getTicketByTicketId: ticketId => {
                const filter = `$filter=ticketId eq ${ticketId}`;
                return createQueryUrl('ticketentities', `${filter}`);
            },
            getFinalSegmentTicket: (segmentId, ticketTypeId, start = false) => {
                const date = start ? 'startdate' : 'enddate';
                return createQueryUrl('tickets', `$top=1&$orderby=${date} desc&$filter=ticketTypeId eq '${ticketTypeId}' and categoryElementId eq ${segmentId} and status ne 'FAILED'`);
            },
            getFinalSystemTicket: (systemId, ticketTypeId) => {
                const table = ticketTypeId === 'Ownership' ? 'SystemOwnershipCalculation' : 'SystemUnbalance';
                const ticketProperty = ticketTypeId === 'Ownership' ? 'OwnershipTicket' : 'UnbalanceTicket';
                const expand = `$expand=${ticketProperty}($select=endDate, ticketTypeId, status, ticketId)`;
                const order = `$orderby=${ticketProperty}/enddate desc`;
                const filter = `$filter=systemId eq ${systemId} and ${ticketProperty}/ticketTypeId eq '${ticketTypeId}' and ${ticketProperty}/status ne 'FAILED'`;

                return createQueryUrl(utilities.toLowerCase(table) + 's', `${expand}&${order}&${filter}`);
            },
            postOperationalDataWithoutCutOff: () => {
                return createApiUrl('operationaldatawithoutcutoff');
            },
            postValidateNodesLogisticAvailables: () => {
                return createApiUrl('logistics/validateNodesLogisticAvailables');
            },
            postNonOperationalSegmentOwnership: () => {
                return createApiUrl('savenonoperationalsegmentownership');
            },
            requestReportExecutionStatus: executionId => {
                return createApiUrl(`reports/${executionId}/status`);
            },
            getLogisticsValidationData: () => {
                return createApiUrl('logistics/validate');
            },
            getOfficialLogisticsValidationData: () => {
                return createApiUrl('officialdelta/nodes/unapproved');
            },
            postTicketNodeStatus: () => {
                return createApiUrl('saveticketnodestatus');
            },
            postEventContractReportRequest: () => {
                return createApiUrl('saveeventcontractreportrequest');
            },
            postNodeConfigurationReportRequest: () => {
                return createApiUrl('savenodeconfigurationreportrequest');
            },
            postOfficialInitialBalanceReport: () => {
                return createApiUrl('saveofficialinitialbalance');
            },
            postOfficialNodeBalanceReport: () => {
                return createApiUrl('saveofficialnodebalance');
            },
            sendDeltaNodeForApproval: () => {
                return createApiUrl('deltanode/submitforapproval');
            },
            sendDeltaNodeForReopen: deltaNodeId => {
                return createApiUrl(`deltanode/${deltaNodeId}/reopen`);
            },
            reopenDeltaNode: () => {
                return createApiUrl('deltanode/reopen');
            },
            requestDateRanges: (elementId, years, isPerNodeReport) => {
                return createApiUrl(`officialdelta/periods/${elementId}/${years}/${isPerNodeReport}`);
            }
        },
        operationalCutOff: {
            getPendingTransactionErrors: () => {
                return createApiUrl(`transactions/pending`);
            },
            getUnbalances: () => {
                return createApiUrl(`unbalances`);
            },
            saveOperationalCutOff: () => {
                return createApiUrl(`operationalcutoff`);
            },
            validate: () => {
                return createApiUrl('cutoff/validate');
            },
            validateInitialInventory: () => {
                return createApiUrl('cutoff/initial');
            },
            validateUniqueSegmentTicket: () => {
                return createApiUrl(`tickets/exists`);
            },
            validateDeltaTicket: segmentId => {
                return createApiUrl(`tickets/${segmentId}/delta/exists`);
            },
            getTransferPointMovements: () => {
                return createApiUrl(`transferpointmovements`);
            },
            updateComment: () => {
                return createApiUrl(`updatecomment`);
            },
            getSapTrackingErrors: sapTrackingId => {
                return createApiUrl(`saptracking/errors/${sapTrackingId}`);
            },
            getFirstTimeNodes: () => {
                return createApiUrl('cutoff/firsttimenodes');
            }
        },
        operationalDelta: {
            getPendingInventories: () => {
                return createApiUrl(`cutoff/deltainventories`);
            },
            getPendingMovements: () => {
                return createApiUrl(`cutoff/deltamovements`);
            },
            validateDeltaProcessingStatus: (segmentId, isOwnershipCalculation) => {
                return createApiUrl(`segments/${segmentId}/${isOwnershipCalculation}/ticketprocessingstatus`);
            },
            saveOperationalDelta: () => {
                return createApiUrl(`operationalcutoff`);
            }
        },
        officialDelta: {
            getOfficialDeltaPerNode: () => {
                return createQueryUrl(`deltanodes`);
            },
            validateTicketProcessingStatus: segmentId => {
                return createApiUrl(`officialdelta/segments/${segmentId}/ticketprocessingstatus`);
            },
            validatePreviousOfficialPeriod: () => {
                return createApiUrl(`officialdelta/previousperiods/validate`);
            },
            saveOfficialDelta: () => {
                return createApiUrl(`operationalcutoff`);
            },
            getSonUnapproveNodes: () => {
                return createApiUrl('logistics/validate');
            },
            getOfficialPeriods: (segmentId, years, isPerNodeReport) => {
                return createApiUrl(`officialdelta/periods/${segmentId}/${years}/${isPerNodeReport}`);
            },
            queryByDeltaNodeId: deltaNodeId => {
                return createQueryUrl('deltanodes', `$filter=deltaNodeId eq ${deltaNodeId}`);
            },
            queryManualMovementsByDeltaNodeId: (startTime, endTime, nodeId) => {
                const select = '$select=movementTransactionId,ticketId,netStandardVolume,ownershipTicketId&$orderby=movementTransactionId%20desc';
                let expand = '$expand=movementSource($select=sourceNode,sourceProduct;$expand=sourceNode($select=name),';
                expand = expand + 'sourceProduct($select=name)),movementDestination($select=destinationNode,destinationProduct;';
                expand = expand + '$expand=destinationNode($select=name),destinationProduct($select=name)),movementType($select=name),';
                expand = expand + 'segment($select=name),measurementUnitElement($select=name,description),owners($select=ownerElement,ownershipValue,ownershipValueUnit;';
                expand = expand + '$expand=ownerElement($select=name,description))';
                return createQueryUrl('movements', `${select}&${expand}&startTime=${startTime}&endTime=${endTime}&nodeId=${nodeId}`);
            },
            setManualMovementsByTicketAndConsolidateMovements: deltaNodeId => {
                return createApiUrl(`deltaNodes/${deltaNodeId}/manualMovements`);
            }
        },
        ownership: {
            getDates: segmentId => {
                return createApiUrl(`segments/${segmentId}/ownership`);
            },
            validate: () => {
                return createApiUrl('ownership/exists');
            },
            validateNodes: () => {
                return createApiUrl('ownership/nodes/exists');
            },
            saveOwnershipCalculation: () => {
                return createApiUrl(`operationalcutoff`);
            },
            getLastOwnershipPerformedDateForSelectedSegment: segmentId => {
                return createApiUrl(`segments/${segmentId}/logistics/ownershipLastPerformed`);
            },
            getSyncProgress: () => {
                return createApiUrl(`strategies/progress`);
            },
            syncRules: () => {
                return createApiUrl(`strategies/refresh`);
            }
        },
        conciliation: {
            requestConciliation: () => {
                return createApiUrl(`conciliation/initialize`);
            }
        },
        ownershipNode: {
            query: () => {
                return createQueryUrl('viewownershipnodes');
            },
            queryByTicketId: ticketId => {
                return createQueryUrl('viewownershipnodes', `$filter=ticketId eq ${ticketId}`);
            },
            queryByOwnershipNodeId: ownershipNodeId => {
                return createQueryUrl('viewownershipnodes', `$filter=ownershipNodeId eq ${ownershipNodeId}`);
            },
            queryById: ownershipNodeId => {
                const expand = `$expand=ticket($expand=categoryElement($expand=category)),node($select=name,acceptableBalancePercentage)`;
                const filter = `$filter=ownershipNodeId eq ${ownershipNodeId}`;
                return createQueryUrl('ownershipnodes', `${expand}&${filter}`);
            },
            getById: ownershipNodeId => {
                return createApiUrl(`nodeownership/${ownershipNodeId}`);
            },
            getErrors: () => {
                return createQueryUrl(`ownershiperrors`);
            },
            getOwnershipNodeBalance: ownershipNodeId => {
                return createApiUrl(`ownershipnodes/${ownershipNodeId}/balance`);
            },
            getOwnershipMovementInventoryData: ownershipNodeId => {
                return createApiUrl(`ownershipnodes/${ownershipNodeId}/details`);
            },
            getOwnersForMovement: (sourceNodeId, destinationNodeId, productId) => {
                return createApiUrl(`movements/${sourceNodeId}/${destinationNodeId}/products/${productId}/ownership`);
            },
            reopenTicket: () => {
                return createApiUrl(`reopenticket`);
            },
            getOwnersForInventory: (nodeId, productId) => {
                return createApiUrl(`nodeownership/${nodeId}/${productId}`);
            },
            publishOwnerships: () => {
                return createApiUrl('ownershipnodes/publish');
            },
            sendOwnershipNodeForApproval: ownershipNodeId => {
                return createApiUrl(`nodeownership/submitforapproval/${ownershipNodeId}`);
            },
            getContractsForNewMovementsForNodeOwnership: (selectedData, date) => {
                let filter = `$filter=movementTypeId eq ${selectedData.movementType.elementId} and startdate le ${date} and endDate ge ${date}`;

                if (selectedData.sourceProduct.productId !== null && selectedData.destinationProduct.productId !== null) {
                    filter = `${filter} and (productId eq '${selectedData.sourceProduct.productId}'` +
                        ` or productId eq '${selectedData.destinationProduct.productId}')` +
                        ` and (destinationNodeId eq ${selectedData.sourceNodes.nodeId}` +
                        ` or destinationNodeId eq ${selectedData.destinationNodes.nodeId})`;
                } else if (selectedData.sourceProduct.productId !== null) {
                    filter = `${filter} and productId eq '${selectedData.sourceProduct.productId}' and destinationNodeId eq ${selectedData.sourceNodes.nodeId}`;
                } else {
                    filter = `${filter} and productId eq '${selectedData.destinationProduct.productId}' and destinationNodeId eq ${selectedData.destinationNodes.nodeId}`;
                }
                const expand = `$expand=Owner1($select=name),Owner2($select=name),MeasurementUnitDetail($select=name)`;
                const select = `$select=ContractId, DocumentNumber,Position,StartDate,EndDate,Owner1Id,Owner2Id,MeasurementUnit,Volume`;

                return createQueryUrl(`contracts`, `${expand}&${select}&${filter}`);
            },
            editContracts: movement => {
                const expand = `$expand=Owner1($select=name),Owner2($select=name),MeasurementUnitDetail($select=name)`;
                const select = `$select=contractId,DocumentNumber,Position,StartDate,EndDate,Owner1Id,Owner2Id,MeasurementUnit,Volume`;
                const filter = `$filter=(productId eq '${movement.sourceProductId}'` +
                    ` or productId eq '${movement.destinationProductId}') and (destinationNodeId eq ${movement.destinationNodeId} or destinationNodeId eq ${movement.sourceNodeId})` +
                    ` and ${movement.operationalDate}Z ge startdate and ${movement.operationalDate}Z le endDate` +
                    ` and ${movement.contractId} ne contractId and movementTypeId eq ${movement.movementTypeId}`;
                return createQueryUrl(`contracts`, `${expand}&${select}&${filter}`);
            }
        },
        nodeRelationship: {
            query: route => {
                return createQueryUrl(route);
            },
            update: type => {
                return createApiUrl(`nodes/relationships/${type}`);
            },
            queryTransferSourceNodes: isSendToSap => {
                const expand = '$expand=SourceNode($select=name)';
                const select = '$select=sourceNodeId';
                const filter = isSendToSap ? `$filter=isTransfer eq true and SourceNode/SendToSap eq true` : `$filter=isTransfer eq true`;

                return createQueryUrl(`nodeconnections`, `${expand}&${select}&${filter}`);
            },
            queryTransferDestinationNodes: (sourceNodeId, isSendToSap) => {
                const expand = '$expand=DestinationNode($select=name,nodeId)';
                const select = '$select=DestinationNode';
                const filter = isSendToSap ? `$filter=sourceNodeId eq ${sourceNodeId} and isTransfer eq true and DestinationNode/SendToSap eq true` :
                    `$filter=sourceNodeId eq ${sourceNodeId} and isTransfer eq true`;

                return createQueryUrl(`nodeconnections`, `${expand}&${select}&${filter}`);
            },
            getNodeType: sourceNodeId => {
                return createApiUrl(`nodes/type/${sourceNodeId}`);
            },
            logisticsTransferPointExists: () => {
                return createApiUrl(`nodes/relationships/logistics/exists`);
            },
            operativeTransferPointExists: () => {
                return createApiUrl(`nodes/relationships/operative/exists`);
            }
        },
        transformation: {
            create: () => {
                return createApiUrl('transformations');
            },
            query: () => {
                const expand = `$expand=OriginSourceNode($select=name),OriginDestinationNode($select=name),OriginSourceProduct($select=name),
                OriginDestinationProduct($select=name),DestinationSourceProduct($select=name),
                DestinationDestinationProduct($select=name),DestinationSourceNode($select=name),
                DestinationDestinationNode($select=name),OriginMeasurement($select=name),DestinationMeasurement($select=name)`;
                return createQueryUrl(`transformations`, expand);
            },
            getInfo: transformationId => {
                return createApiUrl(`transformations/${transformationId}/info`);
            },
            validate: () => {
                return createApiUrl('transformations/exists');
            },
            delete: () => {
                return createApiUrl(`transformations`);
            }
        },
        error: {
            getErrors: () => {
                return createQueryUrl('pendingtransactionerrors');
            },
            getFilteredErrors: filter => {
                return createQueryUrl('pendingtransactionerrors', filter);
            },
            saveErrorComment: () => {
                return createApiUrl('transactions/saveComment');
            },
            getErrorDetails: errorId => {
                const parts = errorId.split('_');
                const suffix = `${parts[0]}${utilities.equalsIgnoreCase(parts[1], 'P') ? '/pending' : ''}`;
                return createApiUrl(`transactions/${suffix}`);
            },
            retryErrors: () => {
                return createApiUrl(`retrypendingtransactions`);
            }
        },
        homologation: {
            getHomologationGroups: () => {
                const expand = '$expand=homologation($expand=sourceSystem,destinationSystem),group($select=name),homologationDataMapping($select=homologationDataMappingId)';

                return createQueryUrl('homologationGroups', expand);
            },
            getHomologationGroup: homologationGroupId => {
                const expand = '$expand=homologation($expand=sourceSystem,destinationSystem),group,homologationObjects($expand=homologationObjectType)';
                const filter = `$filter=homologationGroupId eq ${homologationGroupId}`;

                return createQueryUrl(`homologationGroups`, `${expand}&${filter}`);
            },
            deleteHomologationGroup: () => {
                return createApiUrl(`homologations/groups`);
            },
            getHomologationObjects: () => {
                return createApiUrl('homologationObjectTypes');
            },
            getHomologationDataMappingsByGroup: groupId => {
                return createApiUrl(`homologations/groups/${groupId}/mappings`);
            },
            validateHomologationGroup: homologationGroup => {
                return createApiUrl(`groups/${homologationGroup.groupId}/source/${homologationGroup.sourceSystemId}/destination/${homologationGroup.destinationSystemId}/exists`);
            },
            searchHomologationGroupData: (searchText, path, categoryId) => {
                let filter = '';
                if (utilities.isNullOrUndefined(categoryId)) {
                    filter = `$filter=contains(name,'${encodeURIComponent(searchText)}') and isActive eq true`;
                } else {
                    filter = `$filter=contains(name,'${encodeURIComponent(searchText)}') and isActive eq true and categoryId eq ${categoryId}`;
                }
                return createQueryUrl(path, `${filter}&$top=${systemConfigService.getAutocompleteItemsCount()}&$orderBy=name asc`);
            },
            saveCreateUpdateHomologationGroup: () => {
                return createApiUrl('homologations');
            }
        },
        annulation: {
            query: () => {
                const expand = '$expand=sapTransactionCode($select=name,elementId),sourceCategoryElement($select=name,elementId)' +
                    ',annulationCategoryElement($select=name,elementId),sourceNodeOriginType($select=name,originTypeId),' +
                    'destinationNodeOriginType($select=name,originTypeId),sourceProductOriginType($select=name,originTypeId),destinationProductOriginType($select=name,originTypeId)';
                return createQueryUrl('annulations', expand);
            },
            create: () => {
                return createApiUrl('annulations');
            },
            exists: () => {
                return createApiUrl('annulations/exists');
            }
        },
        reports: {
            query: () => {
                return createQueryUrl('reportexecutions');
            },
            exists: type => {
                return createApiUrl(`report/${type}/exists`);
            },
            queryById: executionId => {
                const filter = `$filter=ExecutionId eq ${executionId}`;
                return createQueryUrl('reportexecutions', `${filter}`);
            },
            requestSendToSapReport: () => {
                return createApiUrl('savemovementsendtosapreportrequest');
            },
            requestOfficialNodeStatusReport: () => {
                return createApiUrl('saveofficialnodestatusreportrequest');
            },
            requestUserRolePermissionReport: () => {
                return createApiUrl('saveuserrolesandpermissionsreportrequest');
            }
        },
        logistic: {
            confirmMovements: () => {
                return createApiUrl(`logistics/confirmmovements`);
            },
            cancelBatch: ticketId => {
                return createApiUrl(`logistics/${ticketId}`);
            },
            postValidateNodesAvailables: () => {
                return createApiUrl('logistics/validatenodesavailables');
            },
            getMovementsDetail: ticketId => {
                return createQueryUrl(`logisticmovement?ticketId=${ticketId}`);
            },
            getFailMovements: urlParams => {
                const { categoryElementId, startDate, endDate, scenarioTypeId, ownerId, nodes = [] } = urlParams;
                const nodesResolved = nodes.map(n => `&ticketNodes=${n.nodeId}`).join('');
                return createQueryUrl(
                    `failedlogisticmovement?categoryElementId=${categoryElementId}&startDate=${startDate}&endDate=${endDate}&scenarioTypeId=${scenarioTypeId}&ownerId=${ownerId}${nodesResolved}`
                );
            },
            forwardMovements: () => {
                return createApiUrl(`logistics/forward`);
            }
        },
        product: {
            getProducts: () =>{
                const select = '$select=productId,name,isActive,rowVersion';

                return createQueryUrl(`products`, `${select}`);
            },
            createProduct: () => {
                return createApiUrl('products');
            },
            updateProduct: productId => {
                return createApiUrl(`products/${productId}`);
            },
            deleteProduct: productId => {
                return createApiUrl(`products/${productId}`);
            },
            getProductById: productId => {
                return createApiUrl(`products/${productId}`);
            }

        },
        integration: {
            getIntegrationManagement: () => {
                return createQueryUrl(`integrationmanagements`);
            }
        },
        bootstrap: () => {
            return createApiUrl('bootstrap');
        },
        getCategoryElements: () => {
            return createQueryUrl('categoryelements', `$expand=category`);
        },
        getFilterCategoryElements: filter => {
            const order = '$orderby=name asc';
            return createQueryUrl('categoryelements', `${filter}&${order}`);
        },
        updateDeviationPercentage: () => {
            return createApiUrl('segments/deviationpercentage');
        },
        getElements: () => {
            return createQueryUrl('categoryelements');
        },
        getLogisticCenters: () => {
            return createApiUrl('logisticcenters');
        },
        getStorageLocations: () => {
            return createQueryUrl('storagelocations');
        },
        getReportConfig: type => {
            return createApiUrl(`reportConfigs/${type}`);
        },
        getSystemTypes: () => {
            return createQueryUrl('systemTypes');
        },
        getVariableTypes: () => {
            return createApiUrl('variabletypes');
        },
        getUsers: () => {
            return createApiUrl('users');
        },
        getFlowConfig: type => {
            return createApiUrl(`flowConfigs/${type}`);
        },
        getOriginTypes: () => {
            return createApiUrl('origintypes');
        },
        getStorageLocationProductMappings: () => {
            const expand = `$expand=Product($select=productId,name),StorageLocation($select=storageLocationId,name;$expand=LogisticCenter($select=logisticCenterId,name))`;
            return createQueryUrl('storageLocationProductMappings', `${expand}`);
        },
        getStorageLocationsByLogisticCenter: logisticCenterId => {
            const filter = `$filter=logisticCenterId eq '${logisticCenterId}'`;
            return createQueryUrl('storagelocations', `${filter}`);
        },
        deleteAssociationCenterStorageProduct: associationId => {
            return createApiUrl(`storageLocationProductMappings/${associationId}`);
        },
        createAssociationCenterStorageProduct: () => {
            return createApiUrl('storageLocationProductMappings');
        }
    };
}());

export { apiService };
