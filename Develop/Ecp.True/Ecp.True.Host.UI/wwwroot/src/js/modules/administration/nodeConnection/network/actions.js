import { apiService } from '../../../../common/services/apiService.js';
import { resourceProvider } from '../../../../common/services/resourceProvider.js';
import { networkBuilderService } from '../../../../common/services/networkBuilderService';

export const CREATE_UNSAVED_CONNECTION = 'CREATE_UNSAVED_CONNECTION';
export const CREATE_NODE_CONNECTION = 'CREATE_NODE_CONNECTION';
export const RECEIVE_CREATE_CONNECTION = 'RECEIVE_CREATE_CONNECTION';
export const FAIL_CREATE_CONNECTION = 'FAIL_CREATE_CONNECTION';
export const FAIL_UPDATE_CONNECTION_DETAIL = 'FAIL_UPDATE_CONNECTION_DETAIL';
export const REMOVE_CONNECTION = 'REMOVE_CONNECTION';
export const UPDATE_NODE_CONNECTION = 'UPDATE_NODE_CONNECTION';
export const UPDATE_MODEL_CONNECTIONS = 'UPDATE_MODEL_CONNECTIONS';
export const UPDATE_MODEL_NODES = 'UPDATE_MODEL_NODES';
export const CLEAR_ERROR_MESSAGE = 'CLEAR_ERROR_MESSAGE';
export const CLEAR_UNSAVED_CONNECTION = 'CLEAR_UNSAVED_CONNECTION';
export const CLEAR_CONNECTION_TO_DELETE = 'CLEAR_CONNECTION_TO_DELETE';
export const RESET_STATE = 'RESET_STATE';
export const SHOW_NODE_PANEL = 'SHOW_NODE_PANEL';
export const PERSIST_CURRENT_GRAPHICAL_NETWORK = 'PERSIST_CURRENT_GRAPHICAL_NETWORK';
export const SET_UNSAVED_NODE = 'SET_UNSAVED_NODE';
export const REMOVE_UNSAVED_NODE = 'REMOVE_UNSAVED_NODE';
export const FAIL_GETTING_NODEDETAILS = 'FAIL_GETTING_NODEDETAILS';
export const UPDATE_NETWORK_MODEL = 'UPDATE_NETWORK_MODEL';
export const TOGGLE_BG = 'TOGGLE_BG';
export const SELECT_NODE = 'SELECT_NODE';

export const REQUEST_GET_GRAPHICALNODE = 'REQUEST_GET_GRAPHICALNODE';
export const RECEIVE_GET_GRAPHICALNODE = 'RECEIVE_GET_GRAPHICALNODE';
export const REQUEST_GET_GRAPHICALNETWORK = 'REQUEST_GET_GRAPHICALNETWORK';
export const RECEIVE_GET_GRAPHICALNETWORK = 'RECEIVE_GET_GRAPHICALNETWORK';
export const REQUEST_DELETE_NODECONNECTION = 'REQUEST_DELETE_NODECONNECTION';
export const RECEIVE_DELETE_NODECONNECTION = 'RECEIVE_DELETE_NODECONNECTION';
export const UPDATE_NODE_CONNECTION_DETAIL = 'UPDATE_NODE_CONNECTION_DETAIL';
export const RECEIVE_UPDATE_CONNECTION_DETAIL = 'RECEIVE_UPDATE_CONNECTION_DETAIL';
export const SET_NODE_CONNECTION = 'SET_NODE_CONNECTION';
export const CREATE_CONNECTION_TO_DELETE = 'CREATE_CONNECTION_TO_DELETE';
export const REMOVE_DELETED_CONNECTION = 'REMOVE_DELETED_CONNECTION';
export const RESET_CONNECTION_TO_DELETE = 'RESET_CONNECTION_TO_DELETE';
export const UPDATE_CONNECTION_STATE = 'UPDATE_CONNECTION_STATE';
export const FAIL_DELETE_CONNECTION = 'FAIL_DELETE_CONNECTION';
export const RECEIVE_ENABLE_CONNECTION = 'RECEIVE_ENABLE_CONNECTION';

export const REQUEST_GET_ALLSOURCENODESDETAILS_GRAPHICALNETWORK = 'REQUEST_GET_ALLSOURCENODESDETAILS_GRAPHICALNETWORK';
export const RECEIVE_GET_ALLSOURCENODESDETAILS_GRAPHICALNETWORK = 'RECEIVE_GET_ALLSOURCENODESDETAILS_GRAPHICALNETWORK';

export const REQUEST_GET_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK = 'REQUEST_GET_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK';
export const RECEIVE_GET_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK = 'RECEIVE_GET_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK';

export const HIDE_ALLSOURCENODESDETAILS_GRAPHICALNETWORK = 'HIDE_ALLSOURCENODESDETAILS_GRAPHICALNETWORK';
export const HIDE_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK = 'HIDE_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK';

const GET_NODE_CONNECTION_BY_NODE_ID = 'GET_NODE_CONNECTION_BY_NODE_ID';
export const RECEIVE_CONNECTION_AFTER_UPDATE = 'RECEIVE_CONNECTION_AFTER_UPDATE';

export const UPDATE_ROW_VERSION_FOR_CONNECTION = 'UPDATE_ROW_VERSION_FOR_CONNECTION';
export const CONFIRM_DELETE = 'CONFIRM_DELETE';
export const CONFIRM_ENABLE = 'CONFIRM_ENABLE';
export const CONFIRM_DISABLE = 'CONFIRM_DISABLE';
export const CONNECTION_CREATE_SUCCESS = 'CONNECTION_CREATE_SUCCESS';
export const RECEIVE_NEW_CONNECTION_DETAIL = 'RECEIVE_NEW_CONNECTION_DETAIL';
export const GET_NEW_CONNECTION_DETAIL = 'GET_NEW_CONNECTION_DETAIL';
export const INPUT_OUTPUT_CLICKED = 'INPUT_OUTPUT_CLICKED';
export const SAVE_GRAPHICAL_FILTER = 'SAVE_GRAPHICAL_FILTER';

export const receiveNodeNetwork = graphicalNetwork => {
    return {
        type: RECEIVE_GET_GRAPHICALNETWORK,
        graphicalNetwork
    };
};

export const receiveAllSourceNodesDetailsNetwork = (sourceNodesDetailsgraphicalNetwork, nodeGraphicalNetwork, destinationNodeId) => {
    return {
        type: RECEIVE_GET_ALLSOURCENODESDETAILS_GRAPHICALNETWORK,
        sourceNodesDetailsgraphicalNetwork,
        nodeGraphicalNetwork,
        destinationNodeId
    };
};

export const receiveAllDestinationNodesDetailsNetwork = (destinationNodesDetailsgraphicalNetwork, nodeGraphicalNetwork, sourceNodeId) => {
    return {
        type: RECEIVE_GET_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK,
        destinationNodesDetailsgraphicalNetwork,
        nodeGraphicalNetwork,
        sourceNodeId
    };
};

export const failedGettingNodeDetails = () => {
    return {
        type: FAIL_GETTING_NODEDETAILS
    };
};

export const receiveNode = graphicalNetwork => {
    return {
        type: RECEIVE_GET_GRAPHICALNODE,
        graphicalNetwork
    };
};

export const getGraphicalNetwork = (elementId, nodeId) => {
    return {
        type: REQUEST_GET_GRAPHICALNETWORK,
        fetchConfig: {
            path: apiService.node.getGraphicalNetwork(elementId, nodeId),
            success: receiveNodeNetwork
        }
    };
};

export const getGraphicalNode = (elementId, nodeId) => {
    return {
        type: REQUEST_GET_GRAPHICALNODE,
        fetchConfig: {
            path: apiService.node.getGraphicalNetwork(elementId, nodeId),
            success: receiveNode
        }
    };
};

export const setUnsavedNode = () => {
    return {
        type: SET_UNSAVED_NODE
    };
};

export const removeUnsavedNode = () => {
    return {
        type: REMOVE_UNSAVED_NODE
    };
};

export const createUnsavedConnection = unsavedConnection => {
    return {
        type: CREATE_UNSAVED_CONNECTION,
        unsavedConnection
    };
};

export const createConnectionToDelete = (connectionToDelete, sourceNode, targetNode) => {
    return {
        type: CREATE_CONNECTION_TO_DELETE,
        connectionToDelete,
        sourceNode,
        targetNode
    };
};

export const receiveCreateConnection = () => {
    return {
        type: RECEIVE_CREATE_CONNECTION
    };
};

export const receiveUpdateConnection = json => {
    return {
        type: RECEIVE_UPDATE_CONNECTION_DETAIL,
        json
    };
};

export const failedCreateConnection = json => {
    return {
        type: FAIL_CREATE_CONNECTION,
        errorMessage: json && json.errorCodes && json.errorCodes.length > 0 ? json.errorCodes[0].message : resourceProvider.read('systemErrorMessage')
    };
};

export const failedUpdateConnection = json => {
    return {
        type: FAIL_UPDATE_CONNECTION_DETAIL,
        errorMessage: json && json.errorCodes && json.errorCodes.length > 0 ? json.errorCodes[0].message : resourceProvider.read('systemErrorMessage')
    };
};

export const connectionCreated = () => {
    return {
        type: CONNECTION_CREATE_SUCCESS
    };
};

export const createNodeConnection = connection => {
    return {
        type: CREATE_NODE_CONNECTION,
        fetchConfig: {
            path: apiService.nodeConnection.createOrUpdate(),
            method: 'POST',
            success: connectionCreated,
            failure: json => failedCreateConnection(json),
            body: connection
        }
    };
};

export const updateNodeConnectionDetail = connection => {
    return {
        type: UPDATE_NODE_CONNECTION_DETAIL,
        fetchConfig: {
            path: apiService.nodeConnection.createOrUpdate(),
            method: 'PUT',
            success: receiveUpdateConnection,
            failure: json => failedUpdateConnection(json),
            body: connection
        }
    };
};

export const updateConnectionState = state => {
    return {
        type: UPDATE_CONNECTION_STATE,
        state
    };
};

export const setNodeConnection = connectionToActive => {
    return {
        type: SET_NODE_CONNECTION,
        connectionToActive
    };
};

export const updateNodeConnection = connection => {
    return {
        type: UPDATE_NODE_CONNECTION,
        connection
    };
};

export const removeConnection = connection => {
    return {
        type: REMOVE_CONNECTION,
        connection
    };
};

export const setErrorMessage = errorMessage => {
    return {
        type: FAIL_CREATE_CONNECTION,
        errorMessage
    };
};

export const clearErrorMessage = () => {
    return {
        type: CLEAR_ERROR_MESSAGE
    };
};

export const updateModelConnections = modelConnections => {
    return {
        type: UPDATE_MODEL_CONNECTIONS,
        modelConnections
    };
};

export const updateModelNodes = modelNodes => {
    return {
        type: UPDATE_MODEL_NODES,
        modelNodes
    };
};

export const clearUnsavedConnection = () => {
    return {
        type: CLEAR_UNSAVED_CONNECTION
    };
};

export const removeDeletedConnection = () => {
    return {
        type: REMOVE_DELETED_CONNECTION
    };
};

export const resetConnectionToDelete = () => {
    return {
        type: RESET_CONNECTION_TO_DELETE
    };
};

export const confirmDelete = () => {
    return {
        type: CONFIRM_DELETE
    };
};

export const confirmEnable = () => {
    return {
        type: CONFIRM_ENABLE
    };
};

export const confirmDisable = () => {
    return {
        type: CONFIRM_DISABLE
    };
};

export const clearConnectionToDelete = () => {
    return {
        type: CLEAR_CONNECTION_TO_DELETE
    };
};

export const resetState = () => {
    return {
        type: RESET_STATE
    };
};

export const toggleBG = () => {
    return {
        type: TOGGLE_BG
    };
};
export const showAllSourceNodesDetails = (destinationNodeId, nodeGraphicalNetwork) => {
    return {
        type: REQUEST_GET_ALLSOURCENODESDETAILS_GRAPHICALNETWORK,
        fetchConfig: {
            path: apiService.node.showAllSourceNodesDetails(destinationNodeId),
            success: json => receiveAllSourceNodesDetailsNetwork(json, nodeGraphicalNetwork, destinationNodeId),
            failure: failedGettingNodeDetails
        }
    };
};

export const showAllDestinationNodesDetails = (sourceNodeId, nodeGraphicalNetwork) => {
    return {
        type: REQUEST_GET_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK,
        fetchConfig: {
            path: apiService.node.showAllDestinationNodesDetails(sourceNodeId),
            success: json => receiveAllDestinationNodesDetailsNetwork(json, nodeGraphicalNetwork, sourceNodeId),
            failure: failedGettingNodeDetails
        }
    };
};
export const hideAllSourceNodeDetails = (destinationNodeId, nodeGraphicalNetwork) => {
    return {
        type: HIDE_ALLSOURCENODESDETAILS_GRAPHICALNETWORK,
        destinationNodeId,
        nodeGraphicalNetwork
    };
};

export const hideAllDestinationNodeDetails = (sourceNodeId, nodeGraphicalNetwork) => {
    return {
        type: HIDE_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK,
        sourceNodeId,
        nodeGraphicalNetwork
    };
};

export const updateNetworkModel = nodeGraphicalNetwork => {
    return {
        type: UPDATE_NETWORK_MODEL,
        nodeGraphicalNetwork
    };
};

export const showNodePanel = show => {
    return {
        type: SHOW_NODE_PANEL,
        show
    };
};
export const persistCurrentGraphicalNetwork = () => {
    return {
        type: PERSIST_CURRENT_GRAPHICAL_NETWORK
    };
};
export const selectNode = nodeId => {
    return {
        type: SELECT_NODE,
        nodeId
    };
};

const receiveDeleteNodeConnection = status => {
    return {
        type: RECEIVE_DELETE_NODECONNECTION,
        status
    };
};

export const receiveEnableConnection = () => {
    return {
        type: RECEIVE_ENABLE_CONNECTION
    };
};

export const failedDeleteConnection = json => {
    return {
        type: FAIL_DELETE_CONNECTION,
        errorMessage: json && json.errorCodes && json.errorCodes.length > 0 ? json.errorCodes[0].message : resourceProvider.read('systemErrorMessage')
    };
};

export const deleteNodeConnection = (sourceNodeId, destinationNodeId) => {
    return {
        type: REQUEST_DELETE_NODECONNECTION,
        fetchConfig: {
            path: apiService.nodeConnection.deleteNodeConnection(sourceNodeId, destinationNodeId),
            method: 'DELETE',
            success: receiveDeleteNodeConnection,
            failure: json => failedDeleteConnection(json)
        }
    };
};

export const receiveNewConnectionDetail = connection =>{
    return {
        type: RECEIVE_NEW_CONNECTION_DETAIL,
        connection
    };
};


export const getNewConnectionDetail = (sourcePortId, destinationPortId) => {
    const sourceNodeId = networkBuilderService.getNodeId(sourcePortId);
    const destinationNodeId = networkBuilderService.getNodeId(destinationPortId);
    return {
        type: GET_NEW_CONNECTION_DETAIL,
        fetchConfig: {
            path: apiService.nodeConnection.queryBySourceAndDestinationNodeId(sourceNodeId, destinationNodeId),
            success: receiveNewConnectionDetail
        }
    };
};

export const receiveConnectionAfterUpdate = connection => {
    return {
        type: RECEIVE_CONNECTION_AFTER_UPDATE,
        connection
    };
};

export const getNodeConnectionByNodeIds = (sourceNodeId, destinationNodeId) => {
    return {
        type: GET_NODE_CONNECTION_BY_NODE_ID,
        fetchConfig: {
            path: apiService.nodeConnection.queryBySourceAndDestinationNodeId(sourceNodeId, destinationNodeId),
            success: receiveConnectionAfterUpdate
        }
    };
};

export const updateRowVersionForConnectionUpdate = connection => {
    return {
        type: UPDATE_ROW_VERSION_FOR_CONNECTION,
        connection
    };
};

export const onInputAndOutputClicked = () => {
    return {
        type: INPUT_OUTPUT_CLICKED
    };
};

export const saveGraphicalFilter = filters => {
    return {
        type: SAVE_GRAPHICAL_FILTER,
        filters
    };
};
