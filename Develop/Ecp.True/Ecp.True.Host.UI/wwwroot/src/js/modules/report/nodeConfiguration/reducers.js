import { NODE_CONFIGURATION_REPORT_SAVE_REPORT_FILTER } from './actions';

const filterInitialState = { filterSettings: {} };

export const nodeConfigurationReport = (state = filterInitialState, action = {}) => {
    if (action.type === NODE_CONFIGURATION_REPORT_SAVE_REPORT_FILTER) {
        return Object.assign({}, state, { filters: action.filters });
    }
    return state;
};
