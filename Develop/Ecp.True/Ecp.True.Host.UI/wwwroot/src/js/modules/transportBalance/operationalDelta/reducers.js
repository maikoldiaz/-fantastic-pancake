import {
    INIT_DELTA_TICKET,
    INIT_DELTA_SEGMENT,
    INIT_DELTA_START_DATE,
    SET_DELTA_END_DATE,
    SET_DELTA_TICKET,
    RECEIVE_END_DATE,
    RECEIVE_SAVE_DELTA_CALCULATION,
    SET_IS_READY,
    RECEIVE_DELTA_TICKET_FAILURE
} from './actions';

import { dateService } from '../../../common/services/dateService';

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

export const operationalDelta = function (state = initialState, action = {}) {
    switch (action.type) {
    case INIT_DELTA_TICKET:
        return Object.assign({},
            state,
            {
                deltaTicket: Object.assign({}, initialState.deltaTicket)
            });
    case INIT_DELTA_SEGMENT:
        return Object.assign({},
            state,
            {
                deltaTicket: Object.assign({}, state.deltaTicket, { segment: action.segment })
            });
    case INIT_DELTA_START_DATE:
        return Object.assign({},
            state,
            {
                deltaTicket: Object.assign({}, state.deltaTicket, { startDate: action.startDate })
            });
    case SET_DELTA_END_DATE:
        return Object.assign({},
            state,
            {
                deltaTicket: Object.assign({}, state.deltaTicket, { endDate: action.endDate })
            });

    case SET_DELTA_TICKET:
        return Object.assign({},
            state,
            {
                deltaTicket: Object.assign({}, state.deltaTicket, action.deltaTicket)
            });
    case RECEIVE_END_DATE:
        return Object.assign({},
            state,
            {
                deltaTicket: Object.assign({}, state.deltaTicket, { endDate: dateService.format(action.date) }),
                isFinalDateReceivedToggler: !state.isFinalDateReceivedToggler
            });
    case RECEIVE_SAVE_DELTA_CALCULATION:
        return Object.assign({},
            state,
            {
                status: action.status,
                refreshDeltaCalculationGridToggler: !state.refreshDeltaCalculationGridToggler
            });
    case SET_IS_READY:
        return Object.assign({}, state, { isReady: action.isReady });
    case RECEIVE_DELTA_TICKET_FAILURE:
        return Object.assign({}, state, { failureToggler: !state.failureToggler });
    default:
        return state;
    }
};
