import { apiService } from '../../../../common/services/apiService';
import { navigationService } from '../../../../common/services/navigationService';

const GET_CONNECTION = 'GET_CONNECTION';
export const RECEIVE_GET_CONNECTION = 'RECEIVE_GET_CONNECTION';

const UPDATE_CONNECTION = 'UPDATE_CONNECTION';
export const RECEIVE_UPDATE_CONNECTION = 'RECEIVE_UPDATE_CONNECTION';

export const INIT_PRODUCT = 'INIT_PRODUCT';
const UPDATE_PRODUCT = 'UPDATE_PRODUCT';
export const RECEIVE_UPDATE_PRODUCT = 'RECEIVE_UPDATE_PRODUCT';

const GET_OWNERS = 'GET_OWNERS';
export const RECEIVE_GET_OWNERS = 'RECEIVE_GET_OWNERS';

const UPDATE_NODE_CONNECTION_OWNERS = 'UPDATE_NODE_CONNECTION_OWNERS';
export const RECEIVE_UPDATE_NODE_CONNECTION_OWNERS = 'RECEIVE_UPDATE_NODE_CONNECTION_OWNERS';

const LOAD_ALGORITHMS_LIST = 'LOAD_ALGORITHMS_LIST';
export const RECEIVE_ALGORITHM_LIST = 'RECEIVE_ALGORITHM_LIST';

const QUERY_CONNECTION_BY_SOURCE_DESTINATION_NODE = 'QUERY_CONNECTION_BY_SOURCE_DESTINATION_NODE';
export const RECEIVE_CONNECTION_BY_NODES = 'RECEIVE_CONNECTION_BY_NODES';

export const SET_CONTROL_LIMIT_SOURCE = 'SET_CONTROL_LIMIT_SOURCE';
export const CLEAR_CONNECTION_ATTRIBUTES_OWNERS = 'CLEAR_CONNECTION_ATTRIBUTES_OWNERS';

const REQUEST_DELETE_CONNECTION_ATTRIBUTES = 'REQUEST_DELETE_CONNECTION_ATTRIBUTES';
export const RECEIVE_DELETE_CONNECTION_ATTRIBUTES = 'RECEIVE_DELETE_CONNECTION_ATTRIBUTES';

export const CLEAR_STATUS_CONNECTION_ATTRIBUTES = 'CLEAR_STATUS_CONNECTION_ATTRIBUTES';

export const CHANGE_STATUS_NODE_CONNECTION_ATTRIBUTES = 'CHANGE_STATUS_NODE_CONNECTION_ATTRIBUTES';

export const receiveGetConnection = result => {
    return {
        type: RECEIVE_GET_CONNECTION,
        connection: result.value
    };
};

export const getConnection = connectionId => {
    return {
        type: GET_CONNECTION,
        fetchConfig: {
            path: apiService.nodeConnection.queryById(connectionId),
            success: receiveGetConnection,
            notFound: true
        }
    };
};

export const setControlLimitSource = controlLimitSource => {
    return {
        type: SET_CONTROL_LIMIT_SOURCE,
        controlLimitSource
    };
};

export const receiveUpdateConnection = () => {
    return {
        type: RECEIVE_UPDATE_CONNECTION,
        status: true
    };
};

export const receiveUpdateConnectionFailed = () => {
    return {
        type: RECEIVE_UPDATE_CONNECTION,
        status: false
    };
};

export const updateConnection = connection => {
    return {
        type: UPDATE_CONNECTION,
        fetchConfig: {
            path: apiService.nodeConnection.createOrUpdate(),
            method: 'PUT',
            success: receiveUpdateConnection,
            failure: receiveUpdateConnectionFailed,
            body: connection
        }
    };
};


export const initProduct = connectionProduct => {
    return {
        type: INIT_PRODUCT,
        connectionProduct
    };
};

export const receiveUpdateProduct = () => {
    return {
        type: RECEIVE_UPDATE_PRODUCT
    };
};

export const updateProduct = product => {
    return {
        type: UPDATE_PRODUCT,
        fetchConfig: {
            path: apiService.nodeConnection.updateProduct(),
            method: 'PUT',
            success: receiveUpdateProduct,
            body: product
        }
    };
};

export const receiveGetOwners = json => {
    return {
        type: RECEIVE_GET_OWNERS,
        owners: json.value
    };
};

export const getOwners = () => {
    return {
        type: GET_OWNERS,
        fetchConfig: {
            path: apiService.nodeConnection.getOwners(),
            success: receiveGetOwners
        }
    };
};

export const receiveUpdateOwners = () => {
    return {
        type: RECEIVE_UPDATE_NODE_CONNECTION_OWNERS
    };
};

export const updateOwners = data => {
    return {
        type: UPDATE_NODE_CONNECTION_OWNERS,
        fetchConfig: {
            path: apiService.nodeConnection.updateOwners(),
            method: 'PUT',
            body: data,
            success: receiveUpdateOwners
        }
    };
};

export const receiveAlgorithmList = result => {
    return {
        type: RECEIVE_ALGORITHM_LIST,
        result
    };
};

export const getAlgorithmList = () => {
    return {
        type: LOAD_ALGORITHMS_LIST,
        fetchConfig: {
            path: apiService.nodeConnection.getAlgorithmList(),
            success: receiveAlgorithmList
        }
    };
};

export const receiveConnectionByNodeId = connection => {
    return {
        type: RECEIVE_CONNECTION_BY_NODES,
        connection
    };
};

export const queryConnectionBySourceAndDestinationNodeId = (sourceNodeId, destinationNodeId) => {
    return {
        type: QUERY_CONNECTION_BY_SOURCE_DESTINATION_NODE,
        fetchConfig: {
            path: apiService.nodeConnection.queryBySourceAndDestinationNodeId(sourceNodeId, destinationNodeId),
            success: receiveConnectionByNodeId
        }
    };
};

export const clearOwners = () => {
    return {
        type: CLEAR_CONNECTION_ATTRIBUTES_OWNERS
    };
};

export const receiveDeleteConnectionAttributes = statusDelete => {
    return {
        type: RECEIVE_DELETE_CONNECTION_ATTRIBUTES,
        statusDelete: statusDelete
    };
};


export const deleteConnectionAttributes = (sourceNodeId, destinationNodeId) => {
    return {
        type: REQUEST_DELETE_CONNECTION_ATTRIBUTES,
        fetchConfig: {
            path: apiService.nodeConnection.deleteNodeConnection(sourceNodeId, destinationNodeId),
            method: 'DELETE',
            success: () => receiveDeleteConnectionAttributes(true),
            failure: () => receiveDeleteConnectionAttributes(false)
        }
    };
};

export const clearStatusConnectionAttributes = () => {
    return {
        type: CLEAR_STATUS_CONNECTION_ATTRIBUTES,
        statusDelete: undefined
    };
};

export const changeStatusNodeConnectionAttributes = e => {
    return {
        type: CHANGE_STATUS_NODE_CONNECTION_ATTRIBUTES,
        status: e
    };
};


// NODE COST CENTER
export const INIT_NODE_COST_CENTER = 'INIT_NODE_COST_CENTER';

const REQUEST_GET_NODES = 'REQUEST_GET_NODES';
export const RECEIVE_GET_NODES = 'RECEIVE_GET_NODES';

const REQUEST_GET_DESTINATION_NODES = 'REQUEST_GET_DESTINATION_NODES';
export const RECEIVE_GET_DESTINATION_NODES = 'RECEIVE_GET_DESTINATION_NODES';

const REQUEST_CREATE_NODE_COST_CENTER = 'REQUEST_CREATE_NODE_COST_CENTER';
export const RECEIVE_CREATE_NODE_COST_CENTER = 'REQUEST_CREATE_NODE_COST_CENTER';

export const RECEIVE_CREATE_NODE_COST_CENTER_FAILURE = 'RECEIVE_CREATE_NODE_COST_CENTER_FAILURE';

export const NOTIFY_NODE_COST_CENTER_DUPLICATES = 'NOTIFY_NODE_COST_CENTER_DUPLICATES';

export const INIT_NODE_COST_CENTER_DUPLICATES = 'INIT_NODE_COST_CENTER_DUPLICATES';

export const SAVE_NODE_COST_CENTER = 'SAVE_NODE_COST_CENTER';
export const RECEIVE_SAVE_NODE_COST_CENTER = 'RECEIVE_SAVE_NODE_COST_CENTER';

export const REQUEST_DELETE_NODE_COST_CENTER = 'REQUEST_DELETE_NODE_COST_CENTER';
export const RECEIVE_DELETE_NODE_COST_CENTER = 'RECEIVE_DELETE_NODE_COST_CENTER';

export const BLUR_NODE_COST_CENTER = 'BLUR_NODE_COST_CENTER';

export const CLEAR_STATUS_NODE_COST_CENTER = 'CLEAR_STATUS_NODE_COST_CENTER';

export const initNodeCostCenter = initialValues => {
    return {
        type: INIT_NODE_COST_CENTER,
        initialValues
    };
};

export const receiveActiveNodes = response => {
    const nodes = response ? response.value : [];
    return {
        type: RECEIVE_GET_NODES,
        nodes
    };
};

export const getActiveNodes = () => {
    return {
        type: REQUEST_GET_NODES,
        fetchConfig: {
            path: apiService.node.queryActive(),
            success: receiveActiveNodes
        }
    };
};


export const receiveDestinationNodesNyNodeId = (nodes, nodeIdSelected) => {
    return {
        type: RECEIVE_GET_DESTINATION_NODES,
        nodes,
        nodeIdSelected
    };
};

export const getDestinationNodeByNodeId = nodeId => {
    return {
        type: REQUEST_GET_DESTINATION_NODES,
        fetchConfig: {
            path: apiService.nodeConnection.getDestinationNodesBySourceNode(nodeId),
            success: json => receiveDestinationNodesNyNodeId(json.value, nodeId)
        }

    };
};

export const receiveCreatedNodeCostCenter = response => {
    const currentResponse = response ? response : [];
    const duplicates = currentResponse.filter(r => r.status === 'Duplicated').map(rf => rf.nodeCostCenter);
    const duplicatesHasNoData = duplicates.length < 1;

    if (duplicatesHasNoData) {
        navigationService.navigateTo('manage');
    }


    return {
        type: RECEIVE_CREATE_NODE_COST_CENTER,
        duplicates,
        isDuplicatedNotified: false
    };
};

export const receiveCreatedNodeCostCenterException = response => ({
    type: RECEIVE_CREATE_NODE_COST_CENTER_FAILURE,
    data: response
});

export const createNodeCostCenter = body => ({
    type: REQUEST_CREATE_NODE_COST_CENTER,
    fetchConfig: {
        path: apiService.nodeConnection.createNodeCostCenter(),
        method: 'POST',
        body,
        success: receiveCreatedNodeCostCenter,
        failure: receiveCreatedNodeCostCenterException
    }
});

export const notifyNodeCostCenterDuplicates = () => {
    return {
        type: NOTIFY_NODE_COST_CENTER_DUPLICATES,
        isDuplicatedNotified: true
    };
};

export const initCostCenterDuplicates = duplicates => {
    return {
        type: INIT_NODE_COST_CENTER_DUPLICATES,
        duplicates
    };
};


export const receiveSaveNodeCostCenter = status => {
    return {
        type: RECEIVE_SAVE_NODE_COST_CENTER,
        status
    };
};

export const saveNodeCostCenter = body => {
    return {
        type: SAVE_NODE_COST_CENTER,
        fetchConfig: {
            path: apiService.nodeConnection.updateNodeCostCenter(),
            method: 'PUT',
            body,
            success: receiveSaveNodeCostCenter
        }
    };
};

const receiveDeleteNodeCostCenter = () => {
    return {
        type: RECEIVE_DELETE_NODE_COST_CENTER,
        status: true
    };
};

const receiveDeleteNodeCostCenterFailed = () => {
    return {
        type: RECEIVE_DELETE_NODE_COST_CENTER,
        status: false
    };
};


export const deleteNodeCostCenter = data => {
    return {
        type: REQUEST_DELETE_NODE_COST_CENTER,
        fetchConfig: {
            path: apiService.nodeConnection.deleteNodeCostCenter(data),
            method: 'DELETE',
            success: receiveDeleteNodeCostCenter,
            failure: receiveDeleteNodeCostCenterFailed
        }
    };
};

export const blurSourceNode = () => {
    return {
        type: BLUR_NODE_COST_CENTER
    };
};

export const clearStatusNodeCostCenter = () => {
    return {
        type: CLEAR_STATUS_NODE_COST_CENTER,
        status: undefined
    };
};

// New connection

const REQUEST_NODE_BY_SEGMENT_ID = 'REQUEST_NODE_BY_SEGMENT_ID';
export const RECEIVE_NODES_BY_SEGMENT = 'RECEIVE_NODES_BY_SEGMENT';

const REQUEST_CREATE_NODE_CONNECTION = 'REQUEST_CREATE_NODE_CONNECTION';
export const RECEIVE_CREATE_NODE_CONNECTION = 'RECEIVE_CREATE_NODE_CONNECTION';

export const receiveNodesBySegment = (nodes, segmentIdSelected, position, isSource = true) => {
    return {
        type: RECEIVE_NODES_BY_SEGMENT,
        nodes: nodes,
        segmentIdSelected,
        position,
        isSource
    };
};

export const getNodesBySegmentId = (segmentIdSelected, position, isSource = true) => {
    return {
        type: REQUEST_NODE_BY_SEGMENT_ID,
        fetchConfig: {
            path: apiService.nodeConnection.getNodesBySegmentId(segmentIdSelected),
            success: response => receiveNodesBySegment(response.value, segmentIdSelected, position, isSource)
        }

    };
};


export const receiveCreateNodeConnection = response => {
    const currentResponse = response ? response : [];
    const duplicates = currentResponse.filter(r => r.status === 'Duplicated').map(rf => rf.nodeConnection);
    const duplicatesHasNoData = duplicates.length < 1;

    if (duplicatesHasNoData) {
        navigationService.navigateTo('manage');
    }

    return {
        type: RECEIVE_CREATE_NODE_CONNECTION,
        duplicates,
        isDuplicatedNotified: false
    };
};

export const createNodeConnection = body => {
    return {
        type: REQUEST_CREATE_NODE_CONNECTION,
        fetchConfig: {
            path: apiService.nodeConnection.createUsingList(),
            method: 'POST',
            body,
            success: receiveCreateNodeConnection
        }
    };
};
