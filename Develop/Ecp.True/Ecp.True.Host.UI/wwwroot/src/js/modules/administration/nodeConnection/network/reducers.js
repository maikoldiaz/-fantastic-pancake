import { utilities } from '../../../../common/services/utilities.js';
import { constants } from '../../../../common/services/constants.js';
import { networkBuilderService } from '../../../../common/services/networkBuilderService';

import {
    RECEIVE_GET_GRAPHICALNETWORK,
    RECEIVE_GET_ALLSOURCENODESDETAILS_GRAPHICALNETWORK,
    RECEIVE_GET_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK,
    CREATE_UNSAVED_CONNECTION,
    RECEIVE_CREATE_CONNECTION,
    FAIL_CREATE_CONNECTION,
    UPDATE_NODE_CONNECTION,
    UPDATE_MODEL_CONNECTIONS,
    UPDATE_MODEL_NODES,
    REMOVE_CONNECTION,
    CLEAR_ERROR_MESSAGE,
    CLEAR_UNSAVED_CONNECTION,
    REQUEST_GET_GRAPHICALNETWORK,
    RESET_STATE,
    HIDE_ALLSOURCENODESDETAILS_GRAPHICALNETWORK,
    HIDE_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK,
    FAIL_GETTING_NODEDETAILS,
    SHOW_NODE_PANEL,
    PERSIST_CURRENT_GRAPHICAL_NETWORK,
    SET_UNSAVED_NODE,
    REMOVE_UNSAVED_NODE,
    REQUEST_GET_GRAPHICALNODE,
    RECEIVE_GET_GRAPHICALNODE,
    TOGGLE_BG,
    SELECT_NODE,
    UPDATE_NETWORK_MODEL,
    RECEIVE_DELETE_NODECONNECTION,
    RECEIVE_UPDATE_CONNECTION_DETAIL,
    FAIL_UPDATE_CONNECTION_DETAIL,
    SET_NODE_CONNECTION,
    CREATE_CONNECTION_TO_DELETE,
    CLEAR_CONNECTION_TO_DELETE,
    REMOVE_DELETED_CONNECTION,
    RESET_CONNECTION_TO_DELETE,
    UPDATE_CONNECTION_STATE,
    FAIL_DELETE_CONNECTION,
    RECEIVE_ENABLE_CONNECTION,
    RECEIVE_CONNECTION_AFTER_UPDATE,
    UPDATE_ROW_VERSION_FOR_CONNECTION,
    CONFIRM_DELETE,
    CONFIRM_DISABLE,
    CONFIRM_ENABLE,
    CONNECTION_CREATE_SUCCESS,
    RECEIVE_NEW_CONNECTION_DETAIL,
    INPUT_OUTPUT_CLICKED,
    SAVE_GRAPHICAL_FILTER
} from './actions.js';

const initialState = {
    graphicalNetworkBgEnabled: true,
    graphicalNetwork: null,
    errorMessage: null,
    unsavedConnection: null,
    modelConnections: {},
    modelNodes: {},
    showCreateNodePanel: false,
    receiveGraphicalNetworkToggler: false,
    getErrorNodeDetailsToggler: false,
    receiveEnableButtonToggler: false,
    connectionAfterUpdate: null,
    connectionToDelete: null
};

function getNodeId(portId) {
    return parseInt(portId.split('_')[1], 10);
}

function buildSourceNode(existingGraphicalNetwork, unsavedConnection, createdConnection) {
    const copyOfGraphicalNetwork = Object.assign({}, existingGraphicalNetwork);
    const sourcePortId = unsavedConnection.sourcePortId;
    const sourceNodeObject = copyOfGraphicalNetwork[getNodeId(unsavedConnection.sourcePortId)];
    const outputConnections = (sourceNodeObject.outputConnections) + 1;
    const destinationNodeArray = sourceNodeObject[sourcePortId];
    destinationNodeArray.push({
        sourceNodeId: getNodeId(sourcePortId),
        destinationNodeId: getNodeId(unsavedConnection.targetPortId), state: constants.NodeConnectionState.Active,
        rowVersion: createdConnection.rowVersion
    });
    return Object.assign({}, sourceNodeObject, { outputConnections, [sourcePortId]: destinationNodeArray });
}

function buildDestinationNode(existingGraphicalNetwork, unsavedConnection, createdConnection) {
    const copyOfGraphicalNetwork = Object.assign({}, existingGraphicalNetwork);
    const targetPortId = unsavedConnection.targetPortId;
    const destinationNodeObject = copyOfGraphicalNetwork[getNodeId(targetPortId)];
    const inputConnections = (destinationNodeObject.inputConnections) + 1;
    const sourceNodeArray = destinationNodeObject[targetPortId];
    sourceNodeArray.push({
        sourceNodeId: getNodeId(unsavedConnection.sourcePortId),
        destinationNodeId: getNodeId(targetPortId), state: constants.NodeConnectionState.Active,
        rowVersion: createdConnection.rowVersion
    });
    return Object.assign({}, destinationNodeObject, { inputConnections, [targetPortId]: sourceNodeArray });
}
function moreGraphicalNodeConnectionCases(action, state) {
    switch (action.type) {
    case REQUEST_GET_GRAPHICALNODE:
    case REQUEST_GET_GRAPHICALNETWORK:
        return { requested: true };
    case CREATE_UNSAVED_CONNECTION:
        return { unsavedConnection: action.unsavedConnection, createUnsavedConnectionToggler: !state.createUnsavedConnectionToggler };
    case CONFIRM_ENABLE:
        return { hasConfirmedEnableToggler: !state.hasConfirmedEnableToggler };
    case CONFIRM_DISABLE:
        return { hasConfirmedDisableToggler: !state.hasConfirmedDisableToggler };
    case CONFIRM_DELETE:
        return { hasConfirmedDeleteToggler: !state.hasConfirmedDeleteToggler };
    case REMOVE_CONNECTION:
        return { removeConnectionToggler: !state.removeConnectionToggler };
    case UPDATE_NODE_CONNECTION:
        return { updateConnectionToggler: !state.updateConnectionToggler };
    case RECEIVE_UPDATE_CONNECTION_DETAIL:
        return { updateConnectionDetailToggler: !state.updateConnectionDetailToggler };
    case UPDATE_MODEL_CONNECTIONS:
        return { modelConnections: action.modelConnections };
    case UPDATE_MODEL_NODES:
        return { modelNodes: action.modelNodes };
    case SELECT_NODE:
        return { isSelected: action.nodeId };
    case CONNECTION_CREATE_SUCCESS:
        return { connectionCreatedToggler: !state.connectionCreatedToggler };
    case RECEIVE_NEW_CONNECTION_DETAIL:
        return { receiveNewConnectionToggler: !state.receiveNewConnectionToggler, createdConnection: action.connection };
    case INPUT_OUTPUT_CLICKED:
        return { inputOutputClickedToggler: !state.inputOutputClickedToggler };
    case SAVE_GRAPHICAL_FILTER:
        return { filters: action.filters };
    case CLEAR_ERROR_MESSAGE:
        return { errorMessage: null };
    case CLEAR_UNSAVED_CONNECTION:
        return { unsavedConnection: null };
    case SHOW_NODE_PANEL:
        return { showCreateNodePanel: action.show };
    default:
        return state;
    }
}
export const nodeGraphicalConnection = (state = initialState, action = {}) => {
    switch (action.type) {
    case RECEIVE_GET_GRAPHICALNETWORK: {
        const graphicalNetwork = utilities.normalize(action.graphicalNetwork.graphicalNodes, 'nodeId');
        let normalizedSourceNodeConnections = {};
        let normalizedDestinationNodeConnections = {};
        if (action.graphicalNetwork.graphicalNodeConnections) {
            normalizedSourceNodeConnections = utilities.normalizedGroupBy(action.graphicalNetwork.graphicalNodeConnections, 'destinationNodeId');
            normalizedDestinationNodeConnections = utilities.normalizedGroupBy(action.graphicalNetwork.graphicalNodeConnections, 'sourceNodeId');
        }
        action.graphicalNetwork.graphicalNodes.forEach(v => {
            const node = graphicalNetwork[v.nodeId];
            node[constants.NodeSection.In + v.nodeId] = normalizedSourceNodeConnections[v.nodeId] ? normalizedSourceNodeConnections[v.nodeId] : [];
            node[constants.NodeSection.Out + v.nodeId] = normalizedDestinationNodeConnections[v.nodeId] ? normalizedDestinationNodeConnections[v.nodeId] : [];
            node.name = v.nodeName;
        });


        return Object.assign({},
            state,
            {
                graphicalNetwork,
                receiveGraphicalNetworkToggler: !state.receiveGraphicalNetworkToggler
            });
    }
    case RECEIVE_GET_ALLSOURCENODESDETAILS_GRAPHICALNETWORK: {
        networkBuilderService.getAllGraphicalNetworkSourceNodeDetails(action.sourceNodesDetailsgraphicalNetwork, action.destinationNodeId, action.nodeGraphicalNetwork);
        return Object.assign({},
            state,
            {
                cloneGraphicalNetwork: action.nodeGraphicalNetwork,
                selectedNodeId: action.destinationNodeId,
                receiveGraphicalNetworkDataToggler: !state.receiveGraphicalNetworkDataToggler
            });
    }
    case RECEIVE_GET_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK: {
        networkBuilderService.getAllGraphicalNetworkDestinationNodeDetails(action.destinationNodesDetailsgraphicalNetwork, action.sourceNodeId, action.nodeGraphicalNetwork);
        return Object.assign({},
            state,
            {
                cloneGraphicalNetwork: action.nodeGraphicalNetwork,
                selectedNodeId: action.sourceNodeId,
                receiveGraphicalNetworkDataToggler: !state.receiveGraphicalNetworkDataToggler
            });
    }
    case HIDE_ALLSOURCENODESDETAILS_GRAPHICALNETWORK: {
        const nodesToRemove = [];
        action.nodeGraphicalNetwork[action.destinationNodeId][constants.NodeSection.In + action.destinationNodeId].forEach(
            x => networkBuilderService.getNodesToRemove(nodesToRemove, action.destinationNodeId, action.nodeGraphicalNetwork, x.sourceNodeId));

        // deleting nodes and node connections from the current node
        networkBuilderService.deleteNodesAndCollections(nodesToRemove, action.destinationNodeId, action.nodeGraphicalNetwork);

        return Object.assign({}, state,
            {
                graphicalNetwork: action.nodeGraphicalNetwork,
                selectedNodeId: action.destinationNodeId,
                receiveGraphicalNetworkToggler: !state.receiveGraphicalNetworkToggler
            });
    }
    case HIDE_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK: {
        const nodesToRemove = [];
        action.nodeGraphicalNetwork[action.sourceNodeId][constants.NodeSection.Out + action.sourceNodeId].forEach(
            x => networkBuilderService.getNodesToRemove(nodesToRemove, action.sourceNodeId, action.nodeGraphicalNetwork, x.destinationNodeId));

        // deleting nodes and node connections from the current node
        networkBuilderService.deleteNodesAndCollections(nodesToRemove, action.sourceNodeId, action.nodeGraphicalNetwork);

        return Object.assign({}, state,
            {
                graphicalNetwork: action.nodeGraphicalNetwork,
                selectedNodeId: action.sourceNodeId,
                receiveGraphicalNetworkToggler: !state.receiveGraphicalNetworkToggler
            });
    }
    case RECEIVE_GET_GRAPHICALNODE: {
        const savedGraphicalNetwork = utilities.normalize(action.graphicalNetwork.graphicalNodes, 'nodeId');
        action.graphicalNetwork.graphicalNodes.forEach(v => {
            const node = savedGraphicalNetwork[v.nodeId];
            node[constants.NodeSection.In + v.nodeId] = [];
            node[constants.NodeSection.Out + v.nodeId] = [];
        });

        return Object.assign({},
            state,
            {
                graphicalNetwork: { ...savedGraphicalNetwork, ...state.unsavedgraphicalNetwork }
            });
    }
    case CREATE_CONNECTION_TO_DELETE:
        return Object.assign({},
            state,
            {
                connectionToDelete: action.connectionToDelete,
                sourceNodeId: action.sourceNode,
                destinationNodeId: action.targetNode,
                errorMessage: null
            });
    case RECEIVE_CREATE_CONNECTION: {
        let graphicalNetwork = Object.assign({},
            state.graphicalNetwork, {
                [getNodeId(state.unsavedConnection.sourcePortId)]: buildSourceNode(state.graphicalNetwork, state.unsavedConnection, state.createdConnection)
            });

        graphicalNetwork = Object.assign({},
            graphicalNetwork, {
                [getNodeId(state.unsavedConnection.targetPortId)]: buildDestinationNode(graphicalNetwork, state.unsavedConnection, state.createdConnection)
            });

        return Object.assign({}, state,
            {
                createConnectionToggler: !state.createConnectionToggler,
                graphicalNetwork
            });
    }
    case FAIL_CREATE_CONNECTION: {
        return Object.assign({}, state,
            {
                createConnectionToggler: !state.createConnectionToggler,
                errorMessage: action.errorMessage
            });
    }
    case FAIL_UPDATE_CONNECTION_DETAIL: {
        return Object.assign({}, state,
            {
                updateConnectionDetailToggler: !state.updateConnectionDetailToggler,
                errorMessage: action.errorMessage
            });
    }
    case PERSIST_CURRENT_GRAPHICAL_NETWORK: {
        return Object.assign({}, state,
            {
                unsavedgraphicalNetwork: Object.assign({}, state.graphicalNetwork)
            });
    }
    case CLEAR_CONNECTION_TO_DELETE: {
        return Object.assign({}, state,
            {
                connectionToDelete: null
            });
    }
    case SET_NODE_CONNECTION: {
        return Object.assign({}, state, {
            connectionToActive: action.connectionToActive
        });
    }
    case RESET_STATE: {
        return Object.assign({}, state, initialState);
    }
    case FAIL_GETTING_NODEDETAILS: {
        return Object.assign({}, state,
            {
                getErrorNodeDetailsToggler: !state.getErrorNodeDetailsToggler
            });
    }
    case UPDATE_NETWORK_MODEL: {
        return Object.assign({}, state,
            {
                graphicalNetwork: action.nodeGraphicalNetwork,
                cloneGraphicalNetwork: null,
                receiveGraphicalNetworkToggler: !state.receiveGraphicalNetworkToggler
            });
    }
    case SET_UNSAVED_NODE: {
        const unsavedGraphicalNode = {
            nodeId: 0,
            nodeName: 'Nodo',
            acceptableBalancePercentage: '####',
            controlLimit: '####',
            segment: 'XXXX',
            operator: 'XXXX',
            nodeType: null,
            segmentColor: '#66666B',
            nodeTypeIcon: null,
            isActive: true,
            isUnsaved: true,
            inputConnections: '###',
            outputConnections: '###',
            createdBy: null,
            createdDate: null
        };
        return Object.assign({}, state,
            {
                graphicalNetwork: { 0: unsavedGraphicalNode, ...state.unsavedgraphicalNetwork },
                showCreateNodePanel: true
            });
    }
    case REMOVE_UNSAVED_NODE: {
        return Object.assign({}, state,
            {
                graphicalNetwork: state.unsavedgraphicalNetwork,
                showCreateNodePanel: false
            });
    }
    case TOGGLE_BG: {
        return Object.assign({}, state,
            {
                graphicalNetworkBgEnabled: !state.graphicalNetworkBgEnabled
            });
    }
    case RECEIVE_DELETE_NODECONNECTION: {
        return Object.assign({},
            state,
            {
                removeConnectionDetailToggler: !state.removeConnectionDetailToggler
            });
    }
    case UPDATE_CONNECTION_STATE: {
        const destinationNetwork = networkBuilderService.getupdatedDestinationConnection(state.graphicalNetwork, state.sourceNodeId, state.destinationNodeId, action.state);
        const sourceNetwork = networkBuilderService.getupdatedSourceConnection(state.graphicalNetwork, state.sourceNodeId, state.destinationNodeId, action.state);

        return Object.assign({}, state, {
            graphicalNetwork: Object.assign({}, state.graphicalNetwork, {
                [state.destinationNodeId]: destinationNetwork,
                [state.sourceNodeId]: sourceNetwork
            })
        });
    }

    case REMOVE_DELETED_CONNECTION: {
        const port = `out_${state.sourceNodeId}-in_${state.destinationNodeId}`;
        const modelConnections = state.modelConnections;
        delete modelConnections[port];
        let destinationNetwork = null;
        let sourceNetwork = null;

        if (state.sourceNodeId !== state.destinationNodeId) {
            destinationNetwork = networkBuilderService.getDestinationNodeAfterDelete(state.graphicalNetwork, state.sourceNodeId, state.destinationNodeId);
            sourceNetwork = networkBuilderService.getSourceNodeAfterDelete(state.graphicalNetwork, state.sourceNodeId, state.destinationNodeId);
        } else {
            destinationNetwork = networkBuilderService.getNodeAfterDelete(state.graphicalNetwork, state.destinationNodeId);
            sourceNetwork = destinationNetwork;
        }


        return Object.assign({},
            state,
            {
                modelConnections: modelConnections,
                graphicalNetwork: Object.assign({}, state.graphicalNetwork, {
                    [state.destinationNodeId]: destinationNetwork,
                    [state.sourceNodeId]: sourceNetwork
                })
            }
        );
    }
    case UPDATE_ROW_VERSION_FOR_CONNECTION: {
        const in_destinationNode = `in_${state.destinationNodeId}`;
        const out_sourceNode = `out_${state.sourceNodeId}`;
        const sourceNodes = [...state.graphicalNetwork[state.destinationNodeId][in_destinationNode]];
        const destinationNodes = [...state.graphicalNetwork[state.sourceNodeId][out_sourceNode]];
        const destinationNetwork = Object.assign({}, state.graphicalNetwork[state.destinationNodeId]);
        const sourceNetwork = Object.assign({}, state.graphicalNetwork[state.sourceNodeId]);

        sourceNodes.filter(x => x.sourceNodeId === Number(state.sourceNodeId))[0].rowVersion = state.connectionAfterUpdate.rowVersion;
        destinationNodes.filter(x => x.destinationNodeId === Number(state.destinationNodeId))[0].rowVersion = state.connectionAfterUpdate.rowVersion;
        destinationNetwork[in_destinationNode] = sourceNodes;
        sourceNetwork[out_sourceNode] = destinationNodes;

        return Object.assign({},
            state,
            {
                graphicalNetwork: Object.assign({}, state.graphicalNetwork, {
                    [state.destinationNodeId]: destinationNetwork,
                    [state.sourceNodeId]: sourceNetwork
                })
            }
        );
    }
    case FAIL_DELETE_CONNECTION: {
        return Object.assign({}, state, {
            removeConnectionDetailToggler: !state.removeConnectionDetailToggler,
            errorMessage: action.errorMessage
        });
    }
    case RESET_CONNECTION_TO_DELETE:
        return Object.assign({},
            state, {
                sourceNodeId: null,
                destinationNodeId: null,
                connectionToDelete: null
            });
    case RECEIVE_ENABLE_CONNECTION:
        return Object.assign({},
            state,
            {
                receiveEnableButtonToggler: !state.receiveEnableButtonToggler
            });
    case RECEIVE_CONNECTION_AFTER_UPDATE: {
        return Object.assign({}, state, { connectionAfterUpdate: action.connection, receiveConnectionAfterUpdateToggler: !state.receiveConnectionAfterUpdateToggler });
    }
    default:
        return Object.assign({}, state, moreGraphicalNodeConnectionCases(action, state));
    }
};
