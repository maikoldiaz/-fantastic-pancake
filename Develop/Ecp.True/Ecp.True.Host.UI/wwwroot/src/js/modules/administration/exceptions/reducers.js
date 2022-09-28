import {
    INIT_ERROR_ADD_COMMENT,
    RECEIVE_ERROR_COMMENT,
    SET_SELECTED_DATA,
    SET_ERROR_DETAIL,
    SAVE_PAGE_FILTERS,
    RECEIVE_RETRY_RECORDS,
    RESET_GRID_FILTER
} from './actions';

const errorInitialState = {
    refreshToggler: false,
    errorDetail: [{}],
    pageFilters: {}
};

export const controlException = function (state = errorInitialState, action = {}) {
    switch (action.type) {
    case INIT_ERROR_ADD_COMMENT:
        return Object.assign({},
            state,
            {
                name: action.name,
                [action.name]: Object.assign({}, state[action.name] ? state[action.name] : {}, {
                    comment: '',
                    preText: action.preText,
                    postText: action.postText,
                    count: action.count
                })
            });
    case RECEIVE_ERROR_COMMENT: {
        return Object.assign({}, state,
            {
                status: action.status,
                refreshToggler: !state.refreshToggler
            });
    }
    case SET_SELECTED_DATA: {
        return Object.assign({}, state,
            {
                selectedData: action.error
            });
    }
    case SET_ERROR_DETAIL: {
        return Object.assign({}, state,
            {
                errorDetail: action.errorDetail
            });
    }
    case SAVE_PAGE_FILTERS: {
        return Object.assign({}, state,
            {
                pageFilters: action.pageFilters,
                loadGridEmpty: true
            });
    }
    case RECEIVE_RETRY_RECORDS: {
        return Object.assign({}, state,
            {
                retryToggler: !state.retryToggler
            });
    }
    case RESET_GRID_FILTER: {
        return Object.assign({}, state,
            {
                reloadGridToggler: !state.reloadGridToggler,
                loadGridEmpty: false
            });
    }
    default:
        return state;
    }
};
