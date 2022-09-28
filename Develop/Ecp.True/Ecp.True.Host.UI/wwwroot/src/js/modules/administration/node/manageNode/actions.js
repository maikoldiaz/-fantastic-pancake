import { apiService } from '../../../../common/services/apiService';
import { receiveGridData } from '../../../../common/components/grid/actions';

export const INIT_EDIT_NODE_STORAGE_LOCATION = 'INIT_EDIT_NODE_STORAGE_LOCATION';
export const REQUEST_FILTERED_NODES = 'REQUEST_FILTERED_NODES';
export const INIT_NODES_GRID_FILTER = 'INIT_NODES_GRID_FILTER';
export const SAVE_NODES_GRID_FILTER = 'SAVE_NODES_GRID_FILTER';
export const INIT_NODE_STORAGE_LOCATION_PRODUCTS = 'INIT_NODE_STORAGE_LOCATION_PRODUCTS';

export const INIT_CREATE_NODE = 'INIT_CREATE_NODE';
export const INIT_UPDATE_NODE = 'INIT_UPDATE_NODE';
export const GET_UPDATE_NODE = 'GET_UPDATE_NODE';

export const REQUEST_NODE_STORAGE_LOCATIONS = 'REQUEST_NODE_STORAGE_LOCATIONS';
export const UPDATE_NODE_STORAGE_LOCATIONS = 'UPDATE_NODE_STORAGE_LOCATIONS';
export const RECEIVE_NODE_STORAGE_LOCATIONS = 'RECEIVE_NODE_STORAGE_LOCATIONS';
export const REQUEST_CREATE_UPDATE_NODE = 'REQUEST_CREATE_UPDATE_NODE';
export const RECEIVE_CREATE_UPDATE_NODE = 'RECEIVE_CREATE_UPDATE_NODE';
export const REMOVE_NODE_STORAGE_LOCATIONS = 'REMOVE_NODE_STORAGE_LOCATIONS';
export const RESET_NODES_FILTER = 'RESET_NODES_FILTER';
export const CHANGE_NODES_FILTER_PERSISTANCE = 'CHANGE_NODES_FILTER_PERSISTANCE';
export const RECEIVE_SAME_ORDER_NODE = 'RECEIVE_SAME_ORDER_NODE';
export const SUBMIT_WITH_AUTO_REORDER = 'SUBMIT_WITH_AUTO_REORDER';
export const RECEIVE_FAILURE_CREATE_UPDATE_NODE = 'RECEIVE_FAILURE_CREATE_UPDATE_NODE';
export const SET_FAILURE_STATE = 'SET_FAILURE_STATE';
export const RECEIVE_CREATE_UPDATE_GRAPHICAL_NODE = 'RECEIVE_CREATE_UPDATE_GRAPHICAL_NODE';
export const REQUEST_UPDATE_NODE = 'REQUEST_UPDATE_NODE';
export const RECEIVE_GRAPHICAL_UPDATE_NODE = 'RECEIVE_GRAPHICAL_UPDATE_NODE';
export const INIT_GRAPHICAL_UPDATE_NODE = 'INIT_GRAPHICAL_UPDATE_NODE';
export const VALIDATE_NODE = 'VALIDATE_NODE';
export const RESET_AUTO_REORDER = 'RESET_AUTO_REORDER';
export const CLEAR_PRODUCTS = 'CLEAR_PRODUCTS';

export const initializeNodeStorageLocationEdit = nodeStorageLocation => {
    return {
        type: INIT_EDIT_NODE_STORAGE_LOCATION,
        nodeStorageLocation
    };
};

export const requestFilteredNodes = (name, filter) => {
    return {
        type: REQUEST_FILTERED_NODES,
        fetchConfig: {
            path: apiService.node.QuerygetFilterNode(filter),
            method: 'GET',
            success: json => receiveGridData(json, name)
        }
    };
};


export const saveNodesGridFilter = filterValues => {
    return {
        type: SAVE_NODES_GRID_FILTER,
        filterValues
    };
};

export const initCreateNode = node => {
    return {
        type: INIT_CREATE_NODE,
        node
    };
};

export const initUpdateNode = node => {
    return {
        type: INIT_UPDATE_NODE,
        node
    };
};

export const initGraphicalUpdateNode = node => {
    return {
        type: INIT_GRAPHICAL_UPDATE_NODE,
        node
    };
};

export const initializeNodeStorageLocationProducts = productNodeStorageLocation => {
    return {
        type: INIT_NODE_STORAGE_LOCATION_PRODUCTS,
        productNodeStorageLocation
    };
};

export const receiveNodeStorageLocations = nodeStorageLocations => {
    return {
        type: RECEIVE_NODE_STORAGE_LOCATIONS,
        nodeStorageLocations
    };
};

export const updateNodeStorageLocations = (nodeStorageLocation, nodeStorageLocations) => {
    return {
        type: UPDATE_NODE_STORAGE_LOCATIONS,
        nodeStorageLocation,
        nodeStorageLocations
    };
};

export const removeNodeStorageLocations = nodeStorageLocation => {
    return {
        type: REMOVE_NODE_STORAGE_LOCATIONS,
        nodeStorageLocation
    };
};

export const REQUEST_SEARCH_PRODUCTS = 'REQUEST_SEARCH_PRODUCTS';
export const RECEIVE_SEARCH_PRODUCTS = 'RECEIVE_SEARCH_PRODUCTS';
export const CLEAR_SEARCH_PRODUCTS = 'CLEAR_SEARCH_PRODUCTS';

export const clearSearchProducts = () => {
    return {
        type: CLEAR_SEARCH_PRODUCTS
    };
};

const receiveSearchProducts = json => {
    return {
        type: RECEIVE_SEARCH_PRODUCTS,
        items: json.value
    };
};

export const requestSearchProducts = (searchText, storageLocationId) => {
    return {
        type: REQUEST_SEARCH_PRODUCTS,
        fetchConfig: {
            path: apiService.node.searchProducts(searchText, storageLocationId),
            success: receiveSearchProducts,
            failure: clearSearchProducts
        }
    };
};

export const requestNodeStorageLocations = nodeId => {
    const path = apiService.node.getNodeStorageLocations(nodeId);
    return {
        type: REQUEST_NODE_STORAGE_LOCATIONS,
        fetchConfig: {
            path,
            success: json => receiveNodeStorageLocations(json.value)
        }
    };
};
const receiveUpdateNode = node => {
    return {
        type: RECEIVE_GRAPHICAL_UPDATE_NODE,
        node
    };
};
const receiveCreateUpdateNode = status => {
    return {
        type: RECEIVE_CREATE_UPDATE_NODE,
        status
    };
};
const receiveCreateUpdateGraphicalNode = status => {
    return {
        type: RECEIVE_CREATE_UPDATE_GRAPHICAL_NODE,
        status
    };
};

const receiveFailureCreateUpdateNode = () => {
    return {
        type: RECEIVE_FAILURE_CREATE_UPDATE_NODE
    };
};

export const requestCreateUpdateNode = (node, isCreate, isGraphical) => {
    return {
        type: REQUEST_CREATE_UPDATE_NODE,
        fetchConfig: {
            path: isGraphical && isCreate ? apiService.node.createGraphicalNode() : apiService.node.createOrUpdate(),
            method: isCreate ? 'POST' : 'PUT',
            success: isGraphical ? receiveCreateUpdateGraphicalNode : receiveCreateUpdateNode,
            failure: receiveFailureCreateUpdateNode,
            body: node
        }
    };
};

export const requestUpdateNode = nodeId => {
    return {
        type: REQUEST_UPDATE_NODE,
        fetchConfig: {
            path: apiService.node.getUpdateNode(nodeId),
            method: 'GET',
            success: receiveUpdateNode,
            failure: receiveFailureCreateUpdateNode
        }
    };
};

export const setFailureState = status => {
    return {
        type: SET_FAILURE_STATE,
        status
    };
};

export const submitWithAutoReOrder = () => {
    return {
        type: SUBMIT_WITH_AUTO_REORDER
    };
};

export const resetReorder = () => {
    return {
        type: RESET_AUTO_REORDER
    };
};

export const changeNodesFilterPersistance = persist => {
    return {
        type: CHANGE_NODES_FILTER_PERSISTANCE,
        persist
    };
};

export const resetNodesFilters = () => {
    return {
        type: RESET_NODES_FILTER
    };
};

export const updateSameOrderNode = existingNode => {
    return {
        type: RECEIVE_SAME_ORDER_NODE,
        existingNode
    };
};

export const isValidNode = node => {
    return {
        type: VALIDATE_NODE,
        node
    };
};

export const clearProducts = logisticCenter => {
    return {
        type: CLEAR_PRODUCTS,
        logisticCenter
    };
};
