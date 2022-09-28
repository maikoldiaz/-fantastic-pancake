import { BALANCE_CONTROL_RECEIVE_FINAL_TICKET, BALANCE_CONTROL_SAVE_REPORT_FILTER } from './actions';

const filterInitialState = {
    filterSettings: {},
    ticket: null
};

export const balanceControlChart = (state = filterInitialState, action = {}) => {
    switch (action.type) {
    case BALANCE_CONTROL_RECEIVE_FINAL_TICKET:
        return Object.assign({},
            state,
            {
                ticket: action.ticket
            });
    case BALANCE_CONTROL_SAVE_REPORT_FILTER:
        return Object.assign({},
            state,
            {
                filters: action.filters
            });
    default:
        return state;
    }
};
