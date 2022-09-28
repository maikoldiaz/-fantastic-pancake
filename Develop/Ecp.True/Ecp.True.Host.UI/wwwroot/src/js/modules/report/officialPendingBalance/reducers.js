import {
    OFFICIAL_PENDING_BALANCE_SAVE_FILTER,
    OFFICIAL_PENDING_BALANCE_SELECT_ELEMENT,
    RESET_OFFICIAL_PENDING_BALANCE_FILTER
} from './actions';

const pendingBalanceInitialState = {
    filterSettings: {}
};

export const pendingBalance = (state = pendingBalanceInitialState, action = {}) => {
    switch (action.type) {
    case OFFICIAL_PENDING_BALANCE_SELECT_ELEMENT:
        return Object.assign({},
            state,
            {
                formValues: {
                    element: action.element
                }
            });
    case OFFICIAL_PENDING_BALANCE_SAVE_FILTER:
        return Object.assign({},
            state,
            {
                filters: action.data,
                formValues: {
                    element: action.data.element
                }
            });
    case RESET_OFFICIAL_PENDING_BALANCE_FILTER:
        return Object.assign({},
            state,
            {
                filters: {
                    elementId: null
                }
            });
    default:
        return state;
    }
};
