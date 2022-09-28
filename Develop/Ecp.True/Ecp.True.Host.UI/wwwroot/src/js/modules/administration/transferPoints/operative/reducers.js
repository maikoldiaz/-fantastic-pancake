import {
    INIT_DELETE_TRANSFER_POINT_ROW,
    INIT_UPDATE_TRANSFER_POINT_ROW,
    REFRESH_TRANSFER_POINT_ROW,
    RECEIVE_TRANSFER_SOURCE_NODES,
    RECEIVE_TRANSFER_DESTINATION_NODES,
    RECEIVE_CREATE_TRANSFER_POINT,
    RECEIVE_TRANSFER_SOURCE_PRODUCTS,
    RECEIVE_SOURCE_NODE_TYPE,
    RECEIVE_DESTINATION_NODE_TYPE,
    RESET_NODE_TYPE,
    RECEIVE_DELETE_TRANSFER_POINT,
    RECEIVE_UPDATE_TRANSFER_POINT,
    REFRESH_CREATE_SUCCESS,
    RECEIVE_NODE_TYPE_NOT_FOUND
} from './actions';

export const transferPointsOperational = (state = {
    transferPoint: {},
    sourceNodes: {},
    sourceNodeType: null,
    destinationNodeType: null,
    nodeTypeFailure: false,
    fieldChange: { fieldChangeToggler: false }
}, action = {}) => {
    switch (action.type) {
    case INIT_DELETE_TRANSFER_POINT_ROW: {
        return Object.assign({}, state, { transferPoint: action.row });
    }
    case INIT_UPDATE_TRANSFER_POINT_ROW: {
        return Object.assign({}, state, {
            initialValues: {
                operativeNodeRelationshipId: action.row.operativeNodeRelationshipId,
                transferPoint: { name: action.row.transferPoint },
                movementType: { name: action.row.movementType },
                sourceProduct: { product: { name: action.row.sourceProduct } },
                sourceProductType: { name: action.row.sourceProductType },
                sourceNode: { sourceNode: { name: action.row.sourceNode } },
                destinationNode: { destinationNode: { name: action.row.destinationNode } },
                sourceField: action.row.sourceField,
                fieldWaterProduction: action.row.fieldWaterProduction,
                relatedSourceField: action.row.relatedSourceField,
                sourceNodeType: action.row.sourceNodeType,
                destinationNodeType: action.row.destinationNodeType,
                rowVersion: action.row.rowVersion
            }
        });
    }
    case REFRESH_TRANSFER_POINT_ROW: {
        return Object.assign({}, state, { transferPoint: {} });
    }
    case RECEIVE_TRANSFER_SOURCE_NODES: {
        return Object.assign({}, state, { sourceNodes: action.nodes.value });
    }
    case RECEIVE_TRANSFER_DESTINATION_NODES: {
        return Object.assign({}, state, { destinationNodes: action.nodes.value });
    }
    case RECEIVE_CREATE_TRANSFER_POINT: {
        return Object.assign({}, state, { createSuccess: !state.createSuccess });
    }
    case RECEIVE_TRANSFER_SOURCE_PRODUCTS: {
        return Object.assign({}, state, { sourceProducts: action.products.value });
    }
    case RECEIVE_SOURCE_NODE_TYPE: {
        return Object.assign({}, state, { sourceNodeType: action.nodeType.name });
    }
    case RECEIVE_DESTINATION_NODE_TYPE: {
        return Object.assign({}, state, { destinationNodeType: action.nodeType.name });
    }
    case RESET_NODE_TYPE: {
        return Object.assign({}, state, {
            destinationNodeType: null,
            sourceNodeType: null,
            destinationNodes: [],
            sourceProducts: [],
            nodeTypeFailure: false
        });
    }
    case RECEIVE_DELETE_TRANSFER_POINT: {
        return Object.assign({}, state, { deleteSuccess: !state.deleteSuccess });
    }
    case RECEIVE_UPDATE_TRANSFER_POINT: {
        return Object.assign({}, state, { updateSuccess: !state.updateSuccess });
    }
    case REFRESH_CREATE_SUCCESS: {
        return Object.assign({}, state, { createSuccess: {} });
    }
    case RECEIVE_NODE_TYPE_NOT_FOUND: {
        return Object.assign({}, state, { nodeTypeFailure: true });
    }
    case '@@redux-form/CHANGE': {
        if (action.meta.form === 'createTransferPointOperational') {
            if (action.meta.field === 'sourceNode') {
                return Object.assign({}, state, {
                    destinationNodeType: null,
                    sourceNodeType: null,
                    destinationNodes: [],
                    sourceProducts: [],
                    nodeTypeFailure: false,
                    fieldChange: {
                        fieldChangeToggler: !state.fieldChange.fieldChangeToggler,
                        currentModifiedField: action.meta.field,
                        currentModifiedValue: action.payload
                    }
                });
            }
            return Object.assign({}, state, {
                fieldChange: {
                    fieldChangeToggler: !state.fieldChange.fieldChangeToggler,
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
