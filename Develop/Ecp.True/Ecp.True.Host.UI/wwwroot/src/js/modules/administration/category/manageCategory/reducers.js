import {
    RECEIVE_GET_CATEGORIES,
    RECEIVE_ADD_CATEGORY,
    RECEIVE_UPDATE_CATEGORY,
    REQUEST_EDIT_INITIALIZE_CATEGORY
} from './actions.js';

export const manageCategory = (state = {}, action = {}) => {
    switch (action.type) {
    case RECEIVE_GET_CATEGORIES: {
        return Object.assign({},
            state,
            {
                categories: action.categories.value
            });
    }
    case RECEIVE_ADD_CATEGORY:
    case RECEIVE_UPDATE_CATEGORY:
        return Object.assign({},
            state,
            {
                status: action.status,
                refreshToggler: !state.refreshToggler
            });
    case REQUEST_EDIT_INITIALIZE_CATEGORY:
        return Object.assign({},
            state,
            {
                category: action.category
            });
    default:
        return state;
    }
};
