import { RECEIVE_REPORT_EXECUTION_DETAILS } from './actions.js';

export const reportExecution = function (state = { execution: null }, action = {}) {
    if (action.type === RECEIVE_REPORT_EXECUTION_DETAILS) {
        return Object.assign({}, state, { execution: action.execution, reportToggler: !state.reportToggler });
    }
    return state;
};
