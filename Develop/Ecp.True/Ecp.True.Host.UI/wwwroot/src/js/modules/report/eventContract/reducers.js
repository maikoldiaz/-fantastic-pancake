import { EVENT_CONTRACT_REPORT_RECEIVE_FINAL_TICKET, EVENT_CONTRACT_REPORT_SAVE_REPORT_FILTER } from './actions';

const filterInitialState = {
    filterSettings: {}
};

export const eventContractReport = (state = filterInitialState, action = {}) => {
    switch (action.type) {
    case EVENT_CONTRACT_REPORT_RECEIVE_FINAL_TICKET:
        return Object.assign({},
            state,
            {
                ticket: action.ticket
            });
    case EVENT_CONTRACT_REPORT_SAVE_REPORT_FILTER:
        return Object.assign({},
            state,
            {
                filters: action.filters
            });
    default:
        return state;
    }
};
