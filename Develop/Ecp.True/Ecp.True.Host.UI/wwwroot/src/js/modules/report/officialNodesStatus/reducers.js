import {
    RECEIVE_OFFICIAL_NODE_STATUS_PERIODS,
    RESET_OFFICIAL_NODE_STATUS_PERIODS,
    SAVE_OFFICIAL_NODE_STATUS_FILTER,
    RECEIVE_SAVE_OFFICIAL_NODE_STATUS_REPORT,
    FAILURE_SAVE_OFFICIAL_NODE_STATUS_REPORT
} from './actions';

const officialNodesStatusInitialState = {
    officialPeriods: {
        defaultYear: null,
        officialPeriods: []
    }
};

export const officialNodeStatusReport = (state = officialNodesStatusInitialState, action = {}) => {
    switch (action.type) {
    case '@@redux-form/CHANGE': {
        if (action.meta.form === 'officialNodeStatusReport') {
            if (action.meta.field === 'segment') {
                return Object.assign({}, state, { segment: action.payload, segmentChangeToggler: !state.segmentChangeToggler });
            }
        }
        return state;
    }
    case SAVE_OFFICIAL_NODE_STATUS_FILTER: {
        return Object.assign({}, state, {
            filters: action.filters
        });
    }
    case RESET_OFFICIAL_NODE_STATUS_PERIODS: {
        return Object.assign({}, state, {
            officialPeriods: {
                defaultYear: null,
                officialPeriods: []
            }
        });
    }
    case RECEIVE_OFFICIAL_NODE_STATUS_PERIODS: {
        return Object.assign({}, state, {
            officialPeriods: action.periods
        });
    }
    case RECEIVE_SAVE_OFFICIAL_NODE_STATUS_REPORT: {
        return Object.assign({}, state, { receiveOfficialNodeStatusToggler: !state.receiveOfficialNodeStatusToggler });
    }
    case FAILURE_SAVE_OFFICIAL_NODE_STATUS_REPORT: {
        return Object.assign({}, state, { failureOfficialNodeStatusToggler: !state.failureOfficialNodeStatusToggler });
    }
    default:
        return state;
    }
};
