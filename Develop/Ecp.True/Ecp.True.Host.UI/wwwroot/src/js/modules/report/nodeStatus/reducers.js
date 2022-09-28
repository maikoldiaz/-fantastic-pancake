import { NODE_STATUS_REPORT_SAVE_REPORT_FILTER, NODE_STATUS_REPORT_RECEIVE_FINAL_STARTDATE_TICKET, NODE_STATUS_REPORT_RECEIVE_FINAL_ENDDATE_TICKET } from './actions';

const filterInitialState = {
    filterSettings: {}
};

export const nodeStatusReport = (state = filterInitialState, action = {}) => {
    switch (action.type) {
    case NODE_STATUS_REPORT_RECEIVE_FINAL_STARTDATE_TICKET:
        return Object.assign({},
            state,
            {
                startDateTicket: action.ticket
            });
    case NODE_STATUS_REPORT_RECEIVE_FINAL_ENDDATE_TICKET:
        return Object.assign({},
            state,
            {
                endDateTicket: action.ticket
            });
    case NODE_STATUS_REPORT_SAVE_REPORT_FILTER:
        return Object.assign({},
            state,
            {
                filters: action.filters
            });
    default:
        return state;
    }
};
