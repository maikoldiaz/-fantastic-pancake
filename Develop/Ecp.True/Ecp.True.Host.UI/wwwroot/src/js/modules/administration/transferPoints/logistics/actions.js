import { apiService } from '../../../../common/services/apiService';
import { constants } from '../../../../common/services/constants';

export const INIT_TRANSFER_POINT_ROW = 'INIT_TRANSFER_POINT_ROW';
export const RECEIVE_DELETE_TRANSFER_POINT = 'RECEIVE_DELETE_TRANSFER_POINT';
const REQUEST_UPDATE_TRANSFER_POINT = 'REQUEST_UPDATE_TRANSFER_POINT';
export const RECEIVE_TRANSFER_SOURCE_NODES = 'RECEIVE_TRANSFER_SOURCE_NODES';
const GET_TRANSFER_SOURCE_NODES = 'GET_TRANSFER_SOURCE_NODES';
export const RECEIVE_TRANSFER_DESTINATION_NODES = 'RECEIVE_TRANSFER_DESTINATION_NODES';
const GET_TRANSFER_DESTINATION_NODES = 'GET_TRANSFER_DESTINATION_NODES';
export const RECEIVE_SOURCE_TRANSFER_LOCATIONS = 'RECEIVE_SOURCE_TRANSFER_LOCATIONS';
export const RECEIVE_DESTINATION_TRANSFER_LOCATIONS = 'RECEIVE_DESTINATION_TRANSFER_LOCATIONS';
const GET_TRANSFER_LOCATIONS = 'GET_TRANSFER_LOCATIONS';
export const RECEIVE_TRANSFER_DESTINATION_PRODUCTS = 'RECEIVE_TRANSFER_DESTINATION_PRODUCTS';
export const RECEIVE_TRANSFER_SOURCE_PRODUCTS = 'RECEIVE_TRANSFER_SOURCE_PRODUCTS';
const GET_TRANSFER_PRODUCTS = 'GET_TRANSFER_PRODUCTS';
export const RECEIVE_SOURCE_LOGISTIC_CENTER = 'RECEIVE_SOURCE_LOGISTIC_CENTER';
export const RECEIVE_DESTINATION_LOGISTIC_CENTER = 'RECEIVE_DESTINATION_LOGISTIC_CENTER';
const GET_LOGISTIC_CENTER = 'GET_LOGISTIC_CENTER';
export const SET_SOURCE_STORAGE_LOCATION = 'SET_SOURCE_STORAGE_LOCATION';
export const SET_DESTINATION_STORAGE_LOCATION = 'SET_DESTINATION_STORAGE_LOCATION';
export const RECEIVE_CREATE_TRANSFER_POINT = 'RECEIVE_CREATE_TRANSFER_POINT';
export const RESET_ON_DESTINATION_NODE_CHANGE = 'RESET_ON_DESTINATION_NODE_CHANGE';
export const RESET_ON_SOURCE_NODE_CHANGE = 'RESET_ON_SOURCE_NODE_CHANGE';

export const initTransferPointRow = row => {
    return {
        type: INIT_TRANSFER_POINT_ROW,
        row
    };
};

export const receiveCreateTransferPoint = success => {
    return {
        type: RECEIVE_CREATE_TRANSFER_POINT,
        success
    };
};

export const receiveDeleteTransferPoint = success => {
    return {
        type: RECEIVE_DELETE_TRANSFER_POINT,
        success
    };
};

export const requestUpdateTransferPoint = (values, method) => {
    let successAction = null;
    if (method === 'POST') {
        successAction = json => receiveCreateTransferPoint(json);
    } else if (method === 'DELETE') {
        successAction = json => receiveDeleteTransferPoint(json);
    }
    return {
        type: REQUEST_UPDATE_TRANSFER_POINT,
        fetchConfig: {
            path: apiService.nodeRelationship.update(constants.NodeRelationship.Logistics),
            body: values,
            method: method,
            success: successAction
        }
    };
};

export const receiveTransferSourceNodes = nodes => {
    return {
        type: RECEIVE_TRANSFER_SOURCE_NODES,
        nodes
    };
};

export const getTransferSourceNodes = () => {
    return {
        type: GET_TRANSFER_SOURCE_NODES,
        fetchConfig: {
            path: apiService.nodeRelationship.queryTransferSourceNodes(true),
            success: json => receiveTransferSourceNodes(json)
        }
    };
};

export const receiveTransferDestinationNodes = nodes => {
    return {
        type: RECEIVE_TRANSFER_DESTINATION_NODES,
        nodes
    };
};

export const getTransferDestinationNodes = sourceNodeId => {
    return {
        type: GET_TRANSFER_DESTINATION_NODES,
        fetchConfig: {
            path: apiService.nodeRelationship.queryTransferDestinationNodes(sourceNodeId, true),
            success: json => receiveTransferDestinationNodes(json)
        }
    };
};

export const receiveSourceNodeStorageLocations = storageLocations => {
    return {
        type: RECEIVE_SOURCE_TRANSFER_LOCATIONS,
        storageLocations
    };
};

export const receiveDestinationNodeStorageLocations = storageLocations => {
    return {
        type: RECEIVE_DESTINATION_TRANSFER_LOCATIONS,
        storageLocations
    };
};

export const getNodeStorageLocations = (nodeId, isSource) => {
    return {
        type: GET_TRANSFER_LOCATIONS,
        fetchConfig: {
            path: apiService.node.getNodeStorageLocations(nodeId),
            success: isSource ? json => receiveSourceNodeStorageLocations(json) : json => receiveDestinationNodeStorageLocations(json)
        }
    };
};

export const receiveTransferSourceProducts = products => {
    return {
        type: RECEIVE_TRANSFER_SOURCE_PRODUCTS,
        products
    };
};

export const receiveTransferDestinationProducts = products => {
    return {
        type: RECEIVE_TRANSFER_DESTINATION_PRODUCTS,
        products
    };
};

export const getTransferProducts = (nodeId, isSource) => {
    return {
        type: GET_TRANSFER_PRODUCTS,
        fetchConfig: {
            path: apiService.node.queryNodeProducts(nodeId),
            success: isSource ? json => receiveTransferSourceProducts(json) : json => receiveTransferDestinationProducts(json)
        }
    };
};

export const receiveSourceLogisticCenter = logisticCenter => {
    return {
        type: RECEIVE_SOURCE_LOGISTIC_CENTER,
        logisticCenter
    };
};

export const receiveDestinationLogisticCenter = logisticCenter => {
    return {
        type: RECEIVE_DESTINATION_LOGISTIC_CENTER,
        logisticCenter
    };
};

export const getLogisticCenter = (nodeName, isSource) => {
    return {
        type: GET_LOGISTIC_CENTER,
        fetchConfig: {
            path: apiService.node.getLogisticCenter(nodeName),
            success: isSource ? json => receiveSourceLogisticCenter(json) : json => receiveDestinationLogisticCenter(json)
        }
    };
};

export const setStorageLocation = (storageLocation, isSource) => {
    return {
        type: isSource ? SET_SOURCE_STORAGE_LOCATION : SET_DESTINATION_STORAGE_LOCATION,
        storageLocation
    };
};

export const resetOnDestinationNodeChange = () => {
    return {
        type: RESET_ON_DESTINATION_NODE_CHANGE
    };
};

export const resetOnSourceNodeChange = () => {
    return {
        type: RESET_ON_SOURCE_NODE_CHANGE
    };
};
