import { apiService } from '../../../../common/services/apiService';
import { constants } from '../../../../common/services/constants';

export const INIT_DELETE_TRANSFER_POINT_ROW = 'INIT_DELETE_TRANSFER_POINT_ROW';
export const INIT_UPDATE_TRANSFER_POINT_ROW = 'INIT_UPDATE_TRANSFER_POINT_ROW';
export const REFRESH_TRANSFER_POINT_ROW = 'REFRESH_TRANSFER_POINT_ROW';
export const RECEIVE_DELETE_TRANSFER_POINT = 'RECEIVE_DELETE_TRANSFER_POINT';
export const RECEIVE_UPDATE_TRANSFER_POINT = 'RECEIVE_UPDATE_TRANSFER_POINT';
const REQUEST_UPDATE_TRANSFER_POINT = 'REQUEST_UPDATE_TRANSFER_POINT';
export const RECEIVE_CREATE_TRANSFER_POINT = 'RECEIVE_CREATE_TRANSFER_POINT';
export const RECEIVE_TRANSFER_SOURCE_NODES = 'RECEIVE_TRANSFER_SOURCE_NODES';
const GET_TRANSFER_SOURCE_NODES = 'GET_TRANSFER_SOURCE_NODES';
export const RECEIVE_TRANSFER_DESTINATION_NODES = 'RECEIVE_TRANSFER_DESTINATION_NODES';
const GET_TRANSFER_DESTINATION_NODES = 'GET_TRANSFER_DESTINATION_NODES';
export const RECEIVE_TRANSFER_SOURCE_PRODUCTS = 'RECEIVE_TRANSFER_SOURCE_PRODUCTS';
const GET_TRANSFER_SOURCE_PRODUCTS = 'GET_TRANSFER_SOURCE_PRODUCTS';
export const RECEIVE_SOURCE_NODE_TYPE = 'RECEIVE_SOURCE_NODE_TYPE';
export const RECEIVE_DESTINATION_NODE_TYPE = 'RECEIVE_DESTINATION_NODE_TYPE';
const GET_NODE_TYPE = 'GET_NODE_TYPE';
export const RESET_NODE_TYPE = 'RESET_NODE_TYPE';
export const RESET_DELETE_SUCCESS = 'RESET_DELETE_SUCCESS';
export const REFRESH_CREATE_SUCCESS = 'REFRESH_CREATE_SUCCESS';
export const RECEIVE_NODE_TYPE_NOT_FOUND = 'RECEIVE_NODE_TYPE_NOT_FOUND';

export const initTransferPointRow = (row, isUpdate) => {
    return {
        type: isUpdate ? INIT_UPDATE_TRANSFER_POINT_ROW : INIT_DELETE_TRANSFER_POINT_ROW,
        row,
        isUpdate
    };
};

export const refreshTransferPointRow = () => {
    return {
        type: REFRESH_TRANSFER_POINT_ROW
    };
};

export const receiveUpdateTransferPoint = success => {
    return {
        type: RECEIVE_UPDATE_TRANSFER_POINT,
        success
    };
};

export const receiveDeleteTransferPoint = success => {
    return {
        type: RECEIVE_DELETE_TRANSFER_POINT,
        success
    };
};

export const receiveCreateTransferPoint = success => {
    return {
        type: RECEIVE_CREATE_TRANSFER_POINT,
        success
    };
};

export const requestUpdateTransferPoint = (values, method) => {
    let successAction = null;
    if (method === 'PUT') {
        successAction = json => receiveUpdateTransferPoint(json);
    } else if (method === 'POST') {
        successAction = json => receiveCreateTransferPoint(json);
    } else if (method === 'DELETE') {
        successAction = json => receiveDeleteTransferPoint(json);
    }
    return {
        type: REQUEST_UPDATE_TRANSFER_POINT,
        fetchConfig: {
            path: apiService.nodeRelationship.update(constants.NodeRelationship.Operative),
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
            path: apiService.nodeRelationship.queryTransferSourceNodes(false),
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
            path: apiService.nodeRelationship.queryTransferDestinationNodes(sourceNodeId, false),
            success: json => receiveTransferDestinationNodes(json)
        }
    };
};

export const receiveTransferSourceProducts = products => {
    return {
        type: RECEIVE_TRANSFER_SOURCE_PRODUCTS,
        products
    };
};

export const getTransferSourceProducts = sourceNodeId => {
    return {
        type: GET_TRANSFER_SOURCE_PRODUCTS,
        fetchConfig: {
            path: apiService.node.queryNodeProducts(sourceNodeId),
            success: json => receiveTransferSourceProducts(json)
        }
    };
};

export const receiveNodeTypeFailure = value => {
    return {
        type: RECEIVE_NODE_TYPE_NOT_FOUND,
        value
    };
};

export const receiveSourceNodeType = nodeType => {
    return {
        type: RECEIVE_SOURCE_NODE_TYPE,
        nodeType
    };
};

export const receiveDestinationNodeType = nodeType => {
    return {
        type: RECEIVE_DESTINATION_NODE_TYPE,
        nodeType
    };
};

export const getNodeType = (sourceNodeId, isSource) => {
    return {
        type: GET_NODE_TYPE,
        fetchConfig: {
            path: apiService.nodeRelationship.getNodeType(sourceNodeId),
            success: json => isSource ? receiveSourceNodeType(json) : receiveDestinationNodeType(json),
            failure: json => receiveNodeTypeFailure(json)
        }
    };
};

export const resetNodeType = () => {
    return {
        type: RESET_NODE_TYPE
    };
};

export const refreshCreateSuccess = () => {
    return {
        type: REFRESH_CREATE_SUCCESS
    };
};

