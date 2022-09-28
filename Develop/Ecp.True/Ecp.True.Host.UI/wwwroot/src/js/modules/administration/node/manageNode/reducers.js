import {
    INIT_EDIT_NODE_STORAGE_LOCATION,
    SAVE_NODES_GRID_FILTER,
    INIT_NODE_STORAGE_LOCATION_PRODUCTS,
    RECEIVE_SEARCH_PRODUCTS,
    RECEIVE_NODE_STORAGE_LOCATIONS,
    UPDATE_NODE_STORAGE_LOCATIONS,
    REMOVE_NODE_STORAGE_LOCATIONS,
    RECEIVE_CREATE_UPDATE_NODE,
    CLEAR_SEARCH_PRODUCTS,
    INIT_CREATE_NODE,
    INIT_UPDATE_NODE,
    CHANGE_NODES_FILTER_PERSISTANCE,
    RESET_NODES_FILTER,
    SUBMIT_WITH_AUTO_REORDER,
    RECEIVE_SAME_ORDER_NODE,
    RECEIVE_FAILURE_CREATE_UPDATE_NODE,
    SET_FAILURE_STATE,
    RECEIVE_CREATE_UPDATE_GRAPHICAL_NODE,
    RECEIVE_GRAPHICAL_UPDATE_NODE,
    INIT_GRAPHICAL_UPDATE_NODE,
    VALIDATE_NODE,
    RESET_AUTO_REORDER,
    CLEAR_PRODUCTS
} from './actions';
import { utilities } from '../../../../common/services/utilities';
import { constants } from '../../../../common/services/constants';
import { dateService } from '../../../../common/services/dateService';

const nodesGridFilterInitialState = {
    defaultFilterValues: {
        segment: null, nodeTypes: [], operators: []
    },
    node: { isActive: true },
    nodeStorageLocations: [],
    isActive: false,
    searchedProducts: [],
    mode: constants.Modes.Create,
    isAsyncValid: true,
    persist: false,
    autoOrderNode: false,
    validNode: true,
    graphicalNodeUpdate: false
};

function buildNodeStorageLocations(existingItems, itemToBeUpdated) {
    if (utilities.isNullOrUndefined(itemToBeUpdated)) {
        return existingItems;
    }

    const index = existingItems.findIndex(x => x.nodeStorageLocationId === itemToBeUpdated.nodeStorageLocationId);
    const items = [...existingItems];
    if (index < 0) {
        items.push(itemToBeUpdated);
    } else {
        items[index] = itemToBeUpdated;
    }

    return items;
}

function removeNodeStorageLocations(existing, removed) {
    const filtered = existing.filter(x => x.nodeStorageLocationId !== removed.nodeStorageLocationId);
    return [...filtered];
}

function rebuildNodeStorageLocations(nodeStorageLocations, existingLogisticCenterId, newLogisticCenter) {
    const newToCompare = utilities.isNullOrUndefined(newLogisticCenter) ? { logisticCenterId: null } : newLogisticCenter;
    const updatedNodeStorageLocations = [...nodeStorageLocations];
    if (existingLogisticCenterId !== newToCompare.logisticCenterId) {
        updatedNodeStorageLocations.forEach(x => {
            x.storageLocation = null;
            x.products = [];
        });
    }

    return updatedNodeStorageLocations;
}

function validate(node, nodeStorageLocations) {
    if (utilities.isNullOrUndefined(nodeStorageLocations) || nodeStorageLocations.length === 0 || nodeStorageLocations.filter(x => x.products.length === 0).length > 0) {
        return false;
    }

    // Node validation can be done through @@redux-form/UPDATE_SYNC_ERRORS form action
    if (!(utilities.isNotEmpty(node.nodeType) && utilities.isNotEmpty(node.segment) && utilities.isNotEmpty(node.operator))) {
        return false;
    }

    if (node.sendToSap && !utilities.isNotEmpty(node.logisticCenter)) {
        return false;
    }

    return !utilities.isNullOrWhitespace(node.name);
}

export const manageNode = (state = nodesGridFilterInitialState, action = {}) => {
    switch (action.type) {
    case INIT_UPDATE_NODE:
        return Object.assign({},
            nodesGridFilterInitialState,
            {
                node: Object.assign({}, action.node, {
                    existingName: action.node.nodeName ? action.node.nodeName : action.node.name,
                    existingLogisticCenter: action.node.logisticCenter,
                    existingLogisticCenterId: action.node.logisticCenter ? action.node.logisticCenter.logisticCenterId : null,
                    name: action.node.nodeName ? action.node.nodeName : action.node.name
                }),
                nodeStorageLocations: action.node.nodeStorageLocations ? action.node.nodeStorageLocations : [],
                mode: constants.Modes.Update,
                filterValues: state.filterValues,
                unit: action.node.unit,
                capacity: action.node.capacity,
                updateToggler: !state.updateToggler
            });
    case INIT_GRAPHICAL_UPDATE_NODE:
        return Object.assign({},
            state,
            {
                node: Object.assign({}, action.node, {
                    existingName: action.node.nodename,
                    existingLogisticCenter: action.node.logisticCenter,
                    existingLogisticCenterId: action.node.logisticCenter ? action.node.logisticCenter.logisticCenterId : null
                }),
                nodeStorageLocations: action.node.nodeStorageLocations ? action.node.nodeStorageLocations : [],
                mode: constants.Modes.Update,
                unit: action.node.unit,
                capacity: action.node.capacity,
                graphicalNodeUpdate: true
            });
    case RECEIVE_GRAPHICAL_UPDATE_NODE: {
        const today = dateService.parseToPBIString(dateService.nowAsString());
        const node = action.node.value[0];
        const nodeType = node.nodeTags.find(
            t => t.categoryElement.categoryId === constants.Category.NodeType &&
            today >= t.startDate && today <= t.endDate);
        const segment = node.nodeTags.find(
            t => t.categoryElement.categoryId === constants.Category.Segment &&
            today >= t.startDate && today <= t.endDate);
        const operator = node.nodeTags.find(
            t => t.categoryElement.categoryId === constants.Category.Operator &&
            today >= t.startDate && today <= t.endDate);
        return Object.assign({},
            state,
            {
                node: Object.assign({}, node, {
                    existingName: node.name,
                    existingLogisticCenter: node.logisticCenter,
                    existingLogisticCenterId: node.logisticCenter ? node.logisticCenter.logisticCenterId : null,
                    name: node.name,
                    nodeType: nodeType ? nodeType.categoryElement : {},
                    segment: segment ? segment.categoryElement : {},
                    operator: operator ? operator.categoryElement : {}
                }),
                nodeStorageLocations: node.nodeStorageLocations ? node.nodeStorageLocations : state.nodeStorageLocations,
                mode: constants.Modes.Update,
                capacity: node.capacity,
                updateToggler: !state.updateToggler
            });
    }
    case INIT_CREATE_NODE:
        return Object.assign({}, nodesGridFilterInitialState, {
            filterValues: state.filterValues
        });
    case INIT_EDIT_NODE_STORAGE_LOCATION:
        return Object.assign({},
            state,
            {
                nodeStorageLocation: action.nodeStorageLocation
            });
    case SAVE_NODES_GRID_FILTER: {
        return Object.assign({},
            state,
            {
                filterValues: Object.assign({}, action.filterValues)
            });
    }
    case INIT_NODE_STORAGE_LOCATION_PRODUCTS:
        return Object.assign({},
            state,
            {
                productNodeStorageLocation: action.productNodeStorageLocation
            });
    case RECEIVE_SEARCH_PRODUCTS:
        return Object.assign({},
            state,
            {
                searchedProducts: action.items
            });
    case CLEAR_SEARCH_PRODUCTS:
        return Object.assign({},
            state,
            {
                searchedProducts: []
            });
    case UPDATE_NODE_STORAGE_LOCATIONS: {
        const existingItems = action.nodeStorageLocations ? action.nodeStorageLocations : state.nodeStorageLocations;
        const updatedNodeStorageLocations = buildNodeStorageLocations(existingItems, action.nodeStorageLocation);
        return Object.assign({},
            state,
            {
                nodeStorageLocations: updatedNodeStorageLocations,
                isValid: validate(state.node, updatedNodeStorageLocations)
            });
    }
    case RECEIVE_NODE_STORAGE_LOCATIONS: {
        const nodeStorageLocations = action.nodeStorageLocations ? action.nodeStorageLocations : [];
        return Object.assign({},
            state,
            {
                nodeStorageLocations,
                isValid: validate(state.node, nodeStorageLocations)
            });
    }
    case REMOVE_NODE_STORAGE_LOCATIONS: {
        const updatedNodeStorageLocations = removeNodeStorageLocations(state.nodeStorageLocations, action.nodeStorageLocation);
        return Object.assign({},
            state,
            {
                nodeStorageLocations: updatedNodeStorageLocations,
                isValid: validate(state.node, updatedNodeStorageLocations)
            });
    }
    case RECEIVE_CREATE_UPDATE_NODE:
        return Object.assign({},
            state,
            {
                refreshToggler: !state.refreshToggler,
                persist: true,
                existingNode: null
            });
    case RECEIVE_CREATE_UPDATE_GRAPHICAL_NODE:
        return Object.assign({},
            state,
            {
                persist: true,
                nodeSavedToggler: !state.nodeSavedToggler,
                savedNodeId: parseInt(action.status.message, 10)
            });
    case SUBMIT_WITH_AUTO_REORDER:
        return Object.assign({},
            state,
            {
                autoOrder: !state.autoOrder,
                autoOrderNode: true
            });
    case RESET_AUTO_REORDER:
        return Object.assign({},
            state,
            {
                autoOrderNode: false
            });
    case CHANGE_NODES_FILTER_PERSISTANCE:
        return Object.assign({},
            state,
            {
                persist: action.persist
            });
    case RESET_NODES_FILTER:
        return Object.assign({},
            state,
            {
                filterValues: nodesGridFilterInitialState.defaultFilterValues
            });
    case RECEIVE_SAME_ORDER_NODE:
        return Object.assign({},
            state,
            {
                existingNode: action.existingNode
            });
    case RECEIVE_FAILURE_CREATE_UPDATE_NODE:
        return Object.assign({},
            state,
            {
                saveNodeFailureToggler: !state.saveNodeFailureToggler,
                saveNodeFailed: true
            });
    case SET_FAILURE_STATE:
        return Object.assign({},
            state,
            {
                saveNodeFailed: action.status
            });
    case VALIDATE_NODE: {
        let validNode = true;
        const operatorExists = action.node.operador ? utilities.isNullOrUndefined(action.node.operador.name) : utilities.isNullOrUndefined(action.node.operator.name);
        if (utilities.isNullOrUndefined(action.node.nodeType.name) || utilities.isNullOrUndefined(action.node.segment.name)
            || operatorExists || action.node.order === 0) {
            validNode = false;
        }
        return Object.assign({},
            state,
            {
                validNode,
                validNodeToggler: !state.validNodeToggler
            });
    }
    case CLEAR_PRODUCTS: {
        const modifiedNodeStorageLocations = rebuildNodeStorageLocations(state.nodeStorageLocations, state.node.existingLogisticCenterId, action.logisticCenter);
        return Object.assign({}, state, {
            nodeStorageLocations: modifiedNodeStorageLocations,
            isValid: validate(state.node, modifiedNodeStorageLocations),
            node: Object.assign({}, state.node, {
                existingLogisticCenterId: action.logisticCenter ? action.logisticCenter.logisticCenterId : null
            })
        });
    }
    case '@@redux-form/CHANGE': {
        if (action.meta.form === 'createNode') {
            const updatedNode = Object.assign({}, state.node, {
                [action.meta.field]: action.payload
            });
            return Object.assign({}, state, {
                node: updatedNode,
                isValid: validate(updatedNode, state.nodeStorageLocations)
            });
        }
        return state;
    }
    case '@@redux-form/STOP_ASYNC_VALIDATION': {
        if (action.meta.form === 'createNode') {
            return Object.assign({}, state, {
                isAsyncValid: !action.error
            });
        }
        return state;
    }
    default:
        return state;
    }
};
