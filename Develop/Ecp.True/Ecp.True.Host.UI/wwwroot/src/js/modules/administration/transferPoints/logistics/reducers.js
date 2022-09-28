import {
    INIT_TRANSFER_POINT_ROW,
    RECEIVE_DELETE_TRANSFER_POINT,
    RECEIVE_TRANSFER_SOURCE_NODES,
    RECEIVE_TRANSFER_DESTINATION_NODES,
    RECEIVE_SOURCE_TRANSFER_LOCATIONS,
    RECEIVE_DESTINATION_TRANSFER_LOCATIONS,
    RECEIVE_TRANSFER_SOURCE_PRODUCTS,
    RECEIVE_TRANSFER_DESTINATION_PRODUCTS,
    RECEIVE_SOURCE_LOGISTIC_CENTER,
    RECEIVE_DESTINATION_LOGISTIC_CENTER,
    SET_SOURCE_STORAGE_LOCATION,
    SET_DESTINATION_STORAGE_LOCATION,
    RECEIVE_CREATE_TRANSFER_POINT,
    RESET_ON_DESTINATION_NODE_CHANGE,
    RESET_ON_SOURCE_NODE_CHANGE
} from './actions';


export const transferPointsLogistics = (state = { transferPoint: {}, sourceNodes: {}, fieldChange: { fieldChangeToggler: false } }, action = {}) => {
    switch (action.type) {
    case INIT_TRANSFER_POINT_ROW: {
        return Object.assign({}, state, { transferPoint: Object.assign({}, action.row, { notes: null }) });
    }
    case RECEIVE_DELETE_TRANSFER_POINT: {
        return Object.assign({}, state, { deleteToggler: !state.deleteToggler });
    }
    case RECEIVE_CREATE_TRANSFER_POINT: {
        return Object.assign({}, state, { createToggler: !state.createToggler });
    }
    case RECEIVE_TRANSFER_SOURCE_NODES: {
        return Object.assign({}, state, { sourceNodes: Array.from(new Set(action.nodes.value.map(node => node.sourceNodeId))).map(id => action.nodes.value.find(node => node.sourceNodeId === id)) });
    }
    case RECEIVE_TRANSFER_DESTINATION_NODES: {
        return Object.assign({}, state, { destinationNodes: action.nodes.value });
    }
    case RECEIVE_SOURCE_TRANSFER_LOCATIONS: {
        return Object.assign({}, state, { sourceStorageLocations: action.storageLocations.value });
    }
    case RECEIVE_DESTINATION_TRANSFER_LOCATIONS: {
        return Object.assign({}, state, { destinationStorageLocations: action.storageLocations.value });
    }
    case RECEIVE_TRANSFER_SOURCE_PRODUCTS: {
        return Object.assign({}, state, { sourceProducts: action.products.value });
    }
    case RECEIVE_TRANSFER_DESTINATION_PRODUCTS: {
        return Object.assign({}, state, { destinationProducts: action.products.value });
    }
    case RECEIVE_SOURCE_LOGISTIC_CENTER: {
        return Object.assign({}, state, { sourceLogisticCenter: action.logisticCenter.logisticCenter ? action.logisticCenter.logisticCenter.storageLocations[0].name : 'na' });
    }
    case RECEIVE_DESTINATION_LOGISTIC_CENTER: {
        return Object.assign({}, state, { destinationLogisticCenter: action.logisticCenter.logisticCenter ? action.logisticCenter.logisticCenter.storageLocations[0].name : 'na' });
    }
    case SET_SOURCE_STORAGE_LOCATION: {
        return Object.assign({}, state, { sourceStorageLocation: action.storageLocation.storageLocation.name });
    }
    case SET_DESTINATION_STORAGE_LOCATION: {
        return Object.assign({}, state, { destinationStorageLocation: action.storageLocation.storageLocation.name });
    }
    case RESET_ON_DESTINATION_NODE_CHANGE: {
        return Object.assign({}, state, { destinationStorageLocation: null, destinationLogisticCenter: null });
    }
    case RESET_ON_SOURCE_NODE_CHANGE: {
        return Object.assign({}, state, {
            destinationStorageLocations: [],
            destinationNodes: [],
            sourceStorageLocations: [],
            sourceProducts: [],
            destinationProducts: [],
            sourceStorageLocation: null,
            sourceLogisticCenter: null,
            destinationStorageLocation: null,
            destinationLogisticCenter: null
        });
    }
    case '@@redux-form/CHANGE': {
        if (action.meta.form === 'createTransferPointLogistics') {
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
