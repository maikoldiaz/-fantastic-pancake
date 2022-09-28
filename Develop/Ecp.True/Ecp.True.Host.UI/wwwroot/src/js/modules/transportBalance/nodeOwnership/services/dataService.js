import { constants } from '../../../../common/services/constants';
import { gridUtils } from '../../../../common/components/grid/gridUtils';
import { utilities } from '../../../../common/services/utilities';
import { change } from 'redux-form';
import { requestSourceNodes, requestSourceProducts, requestDestinationProducts, requestDestinationNodes } from '../actions';
import classNames from 'classnames/bind';
import { dateService } from '../../../../common/services/dateService';

const dataService = (function () {
    function buildOwnershipObject(values, ticketId, isMovement) {
        const ownershipObject = values.map(x => {
            return {
                ticketId: ticketId,
                ownerId: x.ownerId,
                ownershipPercentage: x.ownershipPercentage,
                ownershipVolume: x.ownershipVolume,
                appliedRule: x.ownershipFunction,
                ruleVersion: x.ruleVersion
            };
        });
        if (isMovement) {
            return ownershipObject.map((x, index) => {
                return Object.assign({}, x, { movementTransactionId: values[index].transactionId, executionDate: dateService.now().toDate() });
            });
        }
        return ownershipObject.map((x, index) => {
            return Object.assign({}, x, { inventoryProductId: values[index].transactionId });
        });
    }

    const statusValue = {
        [constants.Modes.Create]: constants.EventType.Insert,
        [constants.Modes.Update]: constants.EventType.Update
    };

    const columnsPerVariableType = {
        [constants.VariableType.Interface]: [{ name: 'movementType' }, { name: 'operationalDate', isDate: true }, { name: 'sourceProduct' }, { name: 'destinationProduct' }],
        [constants.VariableType.Tolerance]: [{ name: 'movementType' }, { name: 'operationalDate', isDate: true },
            { name: 'sourceNode' }, { name: 'destinationNode' }, { name: 'sourceProduct' }, { name: 'destinationProduct' }],
        [constants.VariableType.UnidentifiedLoss]: [{ name: 'movementType' }, { name: 'operationalDate', isDate: true },
            { name: 'sourceNode' }, { name: 'destinationNode' }, { name: 'sourceProduct' }, { name: 'destinationProduct' }],
        [constants.VariableType.Output]: [{ name: 'movementType' }, { name: 'operationalDate', isDate: true }, { name: 'destinationNode' }, { name: 'destinationProduct' }],
        [constants.VariableType.IdentifiedLoss]: [{ name: 'movementType' }, { name: 'operationalDate', isDate: true }, { name: 'destinationNode' }, { name: 'destinationProduct' }],
        [constants.VariableType.Input]: [{ name: 'movementType' }, { name: 'operationalDate', isDate: true }, { name: 'sourceNode' }, { name: 'sourceProduct' }],
        [constants.VariableType.InitialInventory]: [{ name: 'operationalDate', isDate: true }, { name: 'tankName' }],
        [constants.VariableType.FinalInventory]: [{ name: 'operationalDate', isDate: true }, { name: 'tankName' }]
    };

    const dispatchActions = (fieldName, params) => {
        let additional = [];
        if (!utilities.isNullOrUndefined(params.selection) && params.selection.variableTypeId === constants.VariableType.Input) {
            additional = [
                requestSourceNodes(params.nodeId),
                change('createMovement', `sourceNodes`, null),
                change('createMovement', `destinationNodes`, params.destinationNode[0]),
                requestDestinationProducts(params.nodeId)
            ];
        } else if (!utilities.isNullOrUndefined(params.selection) && params.selection.variableTypeId === constants.VariableType.Output) {
            additional = [
                change('createMovement', `sourceNodes`, params.sourceNode[0]),
                requestSourceProducts(params.nodeId),
                requestDestinationNodes(params.nodeId),
                change('createMovement', `destinationNodes`, null)
            ];
        } else if (!utilities.isNullOrUndefined(params.selection) && params.selection.variableTypeId === constants.VariableType.IdentifiedLoss) {
            additional = [
                requestSourceProducts(params.nodeId),
                change('createMovement', `sourceNodes`, params.sourceNode[0]),
                change('createMovement', `destinationNodes`, null)
            ];
        } else if (!utilities.isNullOrUndefined(params.selection) && params.selection.variableTypeId === constants.VariableType.Interface) {
            additional = [
                requestSourceProducts(params.nodeId),
                change('createMovement', `sourceNodes`, params.sourceNode[0]),
                requestDestinationProducts(params.nodeId),
                change('createMovement', `destinationNodes`, params.destinationNode[0])
            ];
        } else if (!utilities.isNullOrUndefined(params.selection) &&
        (params.selection.variableTypeId === constants.VariableType.Tolerance ||
            params.selection.variableTypeId === constants.VariableType.UnidentifiedLoss
        )) {
            additional = [
                change('createMovement', `sourceNodes`, null),
                change('createMovement', `destinationNodes`, null)
            ];
        }
        const actions = {
            netVolume: [],
            unit: [],
            variable: [
                change('createMovement', `sourceProduct`, null),
                change('createMovement', `destinationProduct`, null),
                change('createMovement', `movementType`, null),
                change('createMovement', `contract`, null),
                ...additional
            ],
            sourceNodes: [
                requestSourceProducts(params.selection.nodeId),
                change('createMovement', `sourceProduct`, null)
            ],
            destinationNodes: [
                requestDestinationProducts(params.selection.nodeId),
                change('createMovement', `destinationProduct`, null)
            ],
            sourceProduct: [],
            destinationProduct: [],
            movementType: [],
            contract: []
        };
        return actions[fieldName];
    };

    return {
        buildMovement: (values, ticketId) => {
            const movement = {
                ticketId,
                movementTransactionId: values[0].transactionId,
                movementTypeId: parseInt(utilities.getValueOrDefault(values[0].movementTypeId, -1), 10),
                operationalDate: values[0].operationalDate,
                netStandardVolume: values[0].netVolume,
                measurementUnit: !utilities.isNullOrWhitespace(values[0].unitId) ? parseInt(values[0].unitId, 10) : null,
                variableTypeId: values[0].variableTypeId,
                reasonId: values[0].reasonId,
                comment: values[0].comment,
                eventType: utilities.getValue(statusValue, values[0].status) || constants.EventType.Delete,
                movementId: utilities.isNullOrWhitespace(values[0].movementId) ? utilities.getSessionId() : values[0].movementId,
                movementContractId: values[0].contractId,
                period: {
                    movementTransactionId: values[0].transactionId
                },
                ownerships: buildOwnershipObject(values, ticketId, true)
            };
            if (!utilities.isNullOrUndefined(values[0].sourceNodeId) && !utilities.isNullOrUndefined(values[0].sourceProductId)) {
                movement.movementSource = {
                    sourceNodeId: values[0].sourceNodeId,
                    sourceProductId: values[0].sourceProductId
                };
            }
            if (!utilities.isNullOrUndefined(values[0].destinationNodeId) && !utilities.isNullOrUndefined(values[0].destinationProductId)) {
                movement.movementDestination = {
                    destinationNodeId: values[0].destinationNodeId,
                    destinationProductId: values[0].destinationProductId
                };
            }
            return movement;
        },

        buildInventory: (values, ticketId) => {
            return {
                reasonId: values[0].reasonId,
                comment: values[0].comment,
                ownership: {
                    ticketId,
                    inventoryProductId: values[0].transactionId,
                    eventType: utilities.getValue(statusValue, values[0].status) || constants.EventType.Delete,
                    ownerships: buildOwnershipObject(values, ticketId, false)
                }
            };
        },

        compareStatus: (statusToCheck, statusArray, comparator, operator) => {
            let isEnabled = false;
            if (comparator === 'eq') {
                if (operator === 'or') {
                    statusArray.forEach(status => {
                        isEnabled = isEnabled || statusToCheck === status;
                    });
                } else if (operator === 'and') {
                    isEnabled = true;
                    statusArray.forEach(status => {
                        isEnabled = isEnabled && statusToCheck === status;
                    });
                }
            } else if (comparator === 'ne') {
                if (operator === 'or') {
                    statusArray.forEach(status => {
                        isEnabled = isEnabled || statusToCheck !== status;
                    });
                } else if (operator === 'and') {
                    isEnabled = true;
                    statusArray.forEach(status => {
                        isEnabled = isEnabled && statusToCheck !== status;
                    });
                }
            }
            return isEnabled;
        },

        groupByArray: (xs, key) => {
            return xs.reduce(function (rv, x) {
                const v = key instanceof Function ? key(x) : x[key];
                const el = rv.find(r => r && r.key === v);
                if (el) {
                    el.values.push(x);
                } else {
                    rv.push({ key: v, values: [x] });
                }
                return rv;
            }, []);
        },

        buildVariableTypeColumns: (variableType, date, props) => {
            const columns = [];
            if (!utilities.isNullOrUndefined(columnsPerVariableType[variableType])) {
                columnsPerVariableType[variableType].forEach(column => {
                    if (column.isDate) {
                        columns.push(gridUtils.buildDateColumn(column.name, props, date, column.name));
                    } else {
                        columns.push(gridUtils.buildTextColumn(column.name, props, null, column.name));
                    }
                });
            }
            columns.push(gridUtils.buildDecimalColumn('netVolume', props, 'volume'));
            columns.push(gridUtils.buildTextColumn('unit', props, null, 'unit'));
            columns.push(gridUtils.buildTextColumn('ownershipFunction', props, null, 'ownershipFunction'));
            columns.push(gridUtils.buildDecimalColumn('ownershipVolume', props, 'ownershipVolume'));
            columns.push(gridUtils.buildDecimalColumn('ownershipPercentage', props, 'ownershipPercentage'));
            return columns;
        },

        buildNodeDetailsFilter: value => {
            if (!utilities.isNullOrUndefined(utilities.getValue(value, 'productId'))) {
                return {
                    product: {
                        productId: value.productId,
                        product: value.product
                    }
                };
            }
            if (!utilities.isNullOrUndefined(utilities.getValue(value, 'variableTypeId'))) {
                return {
                    variableType: {
                        variableTypeId: value.variableTypeId,
                        name: value.name
                    }
                };
            }
            return {
                owner: {
                    elementId: value.elementId,
                    name: value.name
                }
            };
        },

        buildErrorCheckObject: values => {
            return {
                sourceNodeId: utilities.getValue(values.sourceNodes, 'sourceNode.nodeId', null),
                sourceNode: utilities.getValue(values.sourceNodes, 'sourceNode.name', null),
                destinationNodeId: utilities.getValue(values.destinationNodes, 'destinationNode.nodeId', null),
                destinationNode: utilities.getValue(values.destinationNodes, 'destinationNode.name', null),
                sourceProductId: utilities.getValue(values.sourceProduct, 'product.productId', null),
                sourceProduct: utilities.getValue(values.sourceProduct, 'product.name', null),
                destinationProductId: utilities.getValue(values.destinationProduct, 'product.productId', null),
                destinationProduct: utilities.getValue(values.destinationProduct, 'product.name', null)
            };
        },

        buildMovementInventoryOwnershipArray: (movementInventoryOwnershipData, values) => {
            return movementInventoryOwnershipData.map(item => {
                return {
                    reasonId: values.reasonForChange.elementId,
                    reason: values.reasonForChange.name,
                    comment: values.comment,
                    status: constants.Modes.Create,
                    movementType: values.movementType.name,
                    unit: values.unit.name,
                    unitId: values.unit.elementId,
                    variableTypeId: values.variable.variableTypeId,
                    movementTypeId: values.movementType.elementId,
                    netVolume: item.netVolume,
                    ownershipVolume: parseFloat(item.ownershipVolume).toFixed(2),
                    ownershipPercentage: parseFloat(item.ownershipPercentage).toFixed(2),
                    ownerId: item.ownerId,
                    ownerName: item.ownerName,
                    color: item.color,
                    contractId: !utilities.isNullOrUndefined(values.contract) ? values.contract.contractId : null,
                    documentNumber: !utilities.isNullOrUndefined(values.contract) ? values.contract.documentNumber : null,
                    position: !utilities.isNullOrUndefined(values.contract) ? values.contract.position : null
                };
            });
        },

        buildMovementInventoryOwnershipObject: (ownership, isMovement) => {
            const movementInventoryOwnershipObject = {
                transactionId: ownership.transactionId,
                movementType: ownership.movementType,
                operationalDate: ownership.operationalDate,
                sourceNode: ownership.sourceNode,
                destinationNode: ownership.destinationNode,
                sourceProduct: ownership.sourceProduct,
                destinationProduct: ownership.destinationProduct,
                netVolume: ownership.netVolume,
                unit: ownership.unit,
                sourceProductId: ownership.sourceProductId,
                destinationProductId: ownership.destinationProductId,
                sourceNodeId: ownership.sourceNodeId,
                destinationNodeId: ownership.destinationNodeId,
                variableTypeId: ownership.variableTypeId
            };
            if (isMovement) {
                return Object.assign({}, movementInventoryOwnershipObject, {
                    movementId: ownership.movementId,
                    documentNumber: ownership.documentNumber,
                    position: ownership.position,
                    contractId: ownership.contractId
                });
            }
            return Object.assign({}, movementInventoryOwnershipObject, {
                tankName: ownership.tankName
            });
        },

        buildMovementDataTable: (operativeInformation, isContractEdit, selectedContract) => {
            const movementDataFirstTable = [
                { key: 'operationDate', value: operativeInformation.operationalDate, isDate: true },
                { key: 'sourceNode', value: operativeInformation.sourceNode },
                { key: 'destinationNode', value: operativeInformation.destinationNode },
                { key: 'prodOrigen', value: operativeInformation.sourceProduct },
                { key: 'prodDestino', value: operativeInformation.destinationProduct }];

            const movementDataSecondTable = [
                { key: 'documentNumber', value: operativeInformation.documentNumber, isDashedGridCell: true },
                { key: 'position', value: operativeInformation.position, isDashedGridCell: true },
                { key: 'volOperativo', value: operativeInformation.netVolume, isVolume: true },
                { key: 'units', value: operativeInformation.unit },
                { key: 'ilFunction', value: operativeInformation.ownershipFunction, isOwnershipFunction: true }];

            let movementDataThirdTable = [];
            if (isContractEdit) {
                movementDataThirdTable = [
                    { key: 'initialDate', value: selectedContract && selectedContract.startDate, isDate: true },
                    { key: 'finalDate', value: selectedContract && selectedContract.endDate, isDate: true },
                    { key: 'owner1', value: selectedContract && selectedContract.owner1.name },
                    { key: 'owner2', value: selectedContract && selectedContract.owner2.name },
                    { key: 'value', value: selectedContract && selectedContract.volume },
                    { key: 'units', value: selectedContract && selectedContract.measurementUnitDetail.name }];
            }
            return [ movementDataFirstTable, movementDataSecondTable, movementDataThirdTable ];
        },

        buildInventoryDataTable: operativeInformation => {
            return [
                { key: 'operationDate', value: operativeInformation.operationalDate, isDate: true },
                { key: 'node', value: operativeInformation.sourceNode },
                { key: 'tank', value: operativeInformation.tankName },
                { key: 'product', value: operativeInformation.sourceProduct },
                { key: 'volume', value: operativeInformation.netVolume, isVolume: true },
                { key: 'units', value: operativeInformation.unit },
                { key: 'ownershipFunction', value: operativeInformation.ownershipFunction, isOwnershipFunction: true }
            ];
        },

        updateMovementInventoryOwnershipObject: (values, movementInventoryOwnershipData, isMovement, mode) => {
            movementInventoryOwnershipData.forEach(item => {
                item.reasonId = values.reasonForChange.elementId;
                item.reason = values.reasonForChange.name;
                item.comment = values.comment;
            });
            if (isMovement) {
                return movementInventoryOwnershipData.map(item => {
                    return Object.assign({}, item, {
                        status: item.transactionId < 0 && mode !== constants.Modes.Delete ? constants.Modes.Create : mode
                    });
                });
            }
            return movementInventoryOwnershipData.map(item => {
                return Object.assign({}, item, {
                    status: !item.status ? mode : item.status
                });
            });
        },

        getDispatchActions: function (fieldName, params) {
            return dispatchActions(fieldName, params);
        },

        getStatusIconClass: status => {
            return classNames('fas m-r-1', {
                ['fa-check-circle fas--success ']: status === constants.OwnershipNodeStatus.OWNERSHIP || status === constants.OwnershipNodeStatus.RECONCILED,
                ['fa-lock']: status === constants.OwnershipNodeStatus.LOCKED,
                ['fa-unlock']: status === constants.OwnershipNodeStatus.UNLOCKED,
                ['fa-spinner']: status === constants.OwnershipNodeStatus.PUBLISHING,
                ['fa-upload']: status === constants.OwnershipNodeStatus.PUBLISHED,
                ['fa-file-signature']: status === constants.OwnershipNodeStatus.SUBMITFORAPPROVAL,
                ['fa-check-circle fas--success']: status === constants.OwnershipNodeStatus.APPROVED,
                ['fa-times-circle fas--error']: status === constants.OwnershipNodeStatus.REJECTED || status === constants.OwnershipNodeStatus.CONCILIATIONFAILED,
                ['fa-redo']: status === constants.OwnershipNodeStatus.REOPENED,
                ['fa-file-import']: status === constants.OwnershipNodeStatus.SENT,
                ['fa-exclamation-circle fas--warning']: status === constants.OwnershipNodeStatus.NOTRECONCILED
            });
        }
    };
}());

export { dataService };
