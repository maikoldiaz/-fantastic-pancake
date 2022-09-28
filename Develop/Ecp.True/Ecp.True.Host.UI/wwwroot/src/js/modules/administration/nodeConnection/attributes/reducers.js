import {
    RECEIVE_GET_CONNECTION,
    RECEIVE_UPDATE_CONNECTION,
    INIT_PRODUCT,
    RECEIVE_UPDATE_PRODUCT,
    RECEIVE_GET_OWNERS,
    RECEIVE_UPDATE_NODE_CONNECTION_OWNERS,
    RECEIVE_ALGORITHM_LIST,
    RECEIVE_CONNECTION_BY_NODES,
    SET_CONTROL_LIMIT_SOURCE,
    CLEAR_CONNECTION_ATTRIBUTES_OWNERS,
    RECEIVE_GET_NODES,
    RECEIVE_GET_DESTINATION_NODES,
    INIT_NODE_COST_CENTER,
    RECEIVE_CREATE_NODE_COST_CENTER,
    NOTIFY_NODE_COST_CENTER_DUPLICATES,
    INIT_NODE_COST_CENTER_DUPLICATES,
    RECEIVE_DELETE_NODE_COST_CENTER,
    BLUR_NODE_COST_CENTER,
    RECEIVE_DELETE_CONNECTION_ATTRIBUTES,
    RECEIVE_NODES_BY_SEGMENT,
    RECEIVE_CREATE_NODE_CONNECTION,
    CLEAR_STATUS_CONNECTION_ATTRIBUTES,
    CLEAR_STATUS_NODE_COST_CENTER,
    CHANGE_STATUS_NODE_CONNECTION_ATTRIBUTES

} from './actions';

const connectionAttributesInitialState = {
    connection: {
        sourceNode: {},
        destinationNode: {}
    },
    connectionProduct: { owners: [] },
    algorithms: [],
    controlLimitSource: '',
    newConnections: {},
    status: false
};
export const attributes = (state = connectionAttributesInitialState, action = {}) => {
    switch (action.type) {
    case RECEIVE_GET_CONNECTION: {
        const isTransfer = state.controlLimitSource === 'graphicconfigurationnetwork' ? true : action.connection[0].isTransfer;
        const connection = Object.assign({}, action.connection[0], { isTransfer });
        return Object.assign({}, state, {
            connection,
            receiveConnectionToggler: !state.receiveConnectionToggler,
            isTransfer
        });
    }
    case RECEIVE_UPDATE_CONNECTION: {
        return Object.assign({}, state, {
            connectionToggler: !state.connectionToggler,
            status: action.status

        });
    }
    case INIT_PRODUCT: {
        return Object.assign({}, state, { connectionProduct: action.connectionProduct });
    }
    case RECEIVE_UPDATE_PRODUCT: {
        return Object.assign({}, state, { productToggler: !state.productToggler });
    }
    case RECEIVE_GET_OWNERS: {
        return Object.assign({}, state, { owners: action.owners, ownersToggler: !state.ownersToggler });
    }
    case RECEIVE_UPDATE_NODE_CONNECTION_OWNERS: {
        return Object.assign({}, state, { ownersUpdateToggler: !state.ownersUpdateToggler });
    }
    case RECEIVE_ALGORITHM_LIST: {
        return Object.assign({}, state, { algorithms: action.result });
    }
    case RECEIVE_CONNECTION_BY_NODES: {
        return Object.assign({}, state, { nodeConnectionId: action.connection.nodeConnectionId, receiveConnectionByNodeIDToggler: !state.receiveConnectionByNodeIDToggler });
    }
    case SET_CONTROL_LIMIT_SOURCE:
        return Object.assign({}, state, { controlLimitSource: action.controlLimitSource });
    case CLEAR_CONNECTION_ATTRIBUTES_OWNERS: {
        return Object.assign({}, state, {
            owners: []
        });
    }
    case RECEIVE_DELETE_CONNECTION_ATTRIBUTES: {
        return Object.assign({},
            state,
            {
                statusDelete: action.statusDelete
            });
    }
    case CLEAR_STATUS_CONNECTION_ATTRIBUTES: {
        return Object.assign({},
            state,
            {
                statusDelete: action.statusDelete
            });
    }
    case RECEIVE_GET_NODES: {
        return Object.assign({}, state,
            { nodes: action.nodes }
        );
    }
    case RECEIVE_GET_DESTINATION_NODES: {
        const currentNodes = action.nodes ? action.nodes : [];
        return Object.assign({}, state, {
            destinationNodes: {
                nodes: currentNodes.map(n => n.destinationNode),
                nodeIdSelected: action.nodeIdSelected
            }
        });
    }
    case BLUR_NODE_COST_CENTER: {
        return Object.assign({}, state, {
            destinationNodes: {
                nodes: [],
                nodeIdSelected: undefined
            }
        });
    }
    case RECEIVE_NODES_BY_SEGMENT: {
        let result = {
            [`${action.isSource ? 'source' : 'destination'}Nodes`]: action.nodes,
            segmentIdSelected: action.segmentIdSelected
        };

        result = { ...state.newConnections[action.position], ...result };

        return Object.assign({}, state, {
            newConnections: { ...state.newConnections, [action.position]: result }
        });
    }
    case CHANGE_STATUS_NODE_CONNECTION_ATTRIBUTES: {
        return Object.assign({}, state, {
            status: action.status
        });
    }
    default:
        return state;
    }
};

// NODE COST CENTER

export const nodeCostCenters = function (state = {}, action = {}) {
    switch (action.type) {
    case INIT_NODE_COST_CENTER : {
        return Object.assign({}, state, {
            initialValues: { ...action.initialValues }
        });
    }
    case RECEIVE_CREATE_NODE_COST_CENTER:
    case RECEIVE_CREATE_NODE_CONNECTION: {
        return Object.assign({}, state, {
            duplicates: action.duplicates,
            isDuplicatedNotified: action.isDuplicatedNotified
        });
    }
    case NOTIFY_NODE_COST_CENTER_DUPLICATES: {
        return Object({}, state, {
            isDuplicatedNotified: action.isDuplicatedNotified
        });
    }
    case INIT_NODE_COST_CENTER_DUPLICATES: {
        return Object.assign({}, state, {
            flatDuplicates: action.duplicates.result,
            totalUnsaved: action.duplicates.totalUnSaved,
            totalToSaved: action.duplicates.totalToSaved
        });
    }
    case RECEIVE_DELETE_NODE_COST_CENTER: {
        return Object.assign({},
            state,
            {
                status: action.status
            });
    }
    case CLEAR_STATUS_NODE_COST_CENTER: {
        return Object.assign({},
            state,
            {
                status: action.status
            });
    }
    case '@@redux-form/CHANGE': {
        if (action.meta.form === 'nodeCostCenter') {
            return Object.assign({}, state, {
                fieldChange: {
                    currentModifiedField: action.meta.field,
                    currentModifiedValue: action.payload
                }
            });
        }
        return state;
    }
    default:
        return state;
    }
};

