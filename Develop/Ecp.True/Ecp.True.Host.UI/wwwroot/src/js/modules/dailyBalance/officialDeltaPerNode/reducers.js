import { SET_GRID_VALUES, SET_REPORT_TRIGGER_TOGGLER } from './actions';

const initialState = {
    deltaTicket: {
        segment: null,
        startDate: null,
        endDate: null
    },
    isFinalDateReceivedToggler: true,
    refreshDeltaCalculationGridToggler: false,
    isReady: true
};

export const officialDeltaPerNode = (state = initialState, action = {}) => {
    switch (action.type) {
    case SET_GRID_VALUES:
        return Object.assign({},
            state,
            {
                nodeGridValues: action.data
            });
    case SET_REPORT_TRIGGER_TOGGLER:
        return Object.assign({},
            state,
            {
                nodeGridToggler: action.nodeGridToggler
            });
    default:
        return state;
    }
};
