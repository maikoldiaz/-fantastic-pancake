import {
    RECEIVE_SAVE_PRODUCT,
    INIT_PRODUCT,
    RECEIVE_UPDATE_PRODUCT,
    CLEAR_SUCCESS,
    RECEIVE_DELETE_PRODUCT,
    RECEIVE_PRODUCTS,
    RECEIVE_STORAGE_BY_LOGISTIC_CENTER,
    RECEIVE_DELETE_ASSOCIATION,
    CLEAR_STORAGE_LIST_BY_POSITION,
    CLEAR_ALL_STORAGE_LIST,
    RECEIVE_CREATE_ASSOCIATION
} from './actions.js';

const initialState = {
    newAssociation: {},
    deleteToggler: false,
    deleted: false,
    associationsCreated: [],
    createToggler: false,
    updateToggler: false
};

export const products = function (state = initialState, action = {}) {
    switch (action.type) {
    case RECEIVE_SAVE_PRODUCT: {
        return Object.assign({}, state, {
            saveSuccess: action.status,
            updateToggler: !state.updateToggler,
            isActive: action.isActive
        });
    }
    case INIT_PRODUCT : {
        return Object.assign({}, state, {
            initialValues: action.initialValues
        });
    }
    case RECEIVE_UPDATE_PRODUCT: {
        return Object.assign({}, state, {
            updateSuccess: action.status,
            updateToggler: !state.updateToggler
        });
    }
    case RECEIVE_DELETE_PRODUCT: {
        return Object.assign({}, state, {
            deleteSuccess: action.status,
            updateToggler: !state.updateToggler
        });
    }
    case CLEAR_SUCCESS: {
        return Object.assign({}, state, {
            updateSuccess: action.updateSuccess,
            saveSuccess: action.saveSuccess,
            deleteSuccess: action.deleteSuccess,
            isActive: action.isActive
        });
    }
    case RECEIVE_PRODUCTS: {
        return Object.assign({}, state, {
            products: action.data.value
        });
    }
    case RECEIVE_STORAGE_BY_LOGISTIC_CENTER: {
        let result = {
            [`storageLocation`]: action.storages,
            logisticCenterId: action.logisticCenterId
        };

        result = { ...state.newAssociation[action.position], ...result };

        return Object.assign({}, state, {
            newAssociation: { ...state.newAssociation, [action.position]: result }
        });
    }
    case RECEIVE_DELETE_ASSOCIATION: {
        return Object.assign({}, state, {
            deleteToggler: !state.deleteToggler,
            deleted: action.deleted
        });
    }
    case CLEAR_STORAGE_LIST_BY_POSITION: {
        let result = {
            [`storageLocation`]: []
        };

        result = { ...state.newAssociation[action.position], ...result };

        return Object.assign({}, state, {
            newAssociation: { ...state.newAssociation, [action.position]: result }
        });
    }
    case CLEAR_ALL_STORAGE_LIST: {
        return Object.assign({}, state, {
            newAssociation: []
        });
    }
    case RECEIVE_CREATE_ASSOCIATION: {
        return Object.assign({}, state, {
            createToggler: !state.createToggler,
            associationsCreated: action.response
        });
    }
    default:
        return state;
    }
};
