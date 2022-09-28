import {
    INIT_OFFICIAL_DELTA_TICKET_PROCESS,
    SET_IS_VALID,
    SET_OFFICIAL_DELTA_TICKET,
    RECEIVE_OFFICIAL_DELTA_VALIDATION_DATA,
    RECEIVE_OFFICIAL_PERIODS,
    RECEIVE_OFFICIAL_DELTA_TICKET, RECEIVE_OFFICIAL_DELTA_TICKET_FAILURE,
    RESET_OFFICIAL_PERIODS } from './actions';

const initialState = {};

export const officialDelta = function (state = initialState, action = {}) {
    switch (action.type) {
    case INIT_OFFICIAL_DELTA_TICKET_PROCESS:
        return Object.assign({}, state, {
            step: 0,
            segmentId: 0,
            isValid: false,
            unapprovedNodes: [],
            periods: { officialPeriods: [] }
        });
    case '@@redux-form/CHANGE': {
        if (action.meta.form === 'initOfficialDeltaTicket') {
            if (action.meta.field === 'segment') {
                return Object.assign({}, state, { segment: action.payload, segmentChangeToggler: !state.segmentChangeToggler, isValid: false });
            }
            return Object.assign({}, state, { isValid: false });
        }
        return state;
    }
    case SET_IS_VALID:
        return Object.assign({}, state, { isValid: action.isValid });
    case SET_OFFICIAL_DELTA_TICKET:
        return Object.assign({}, state, { ticket: action.ticket });
    case RECEIVE_OFFICIAL_DELTA_VALIDATION_DATA:
        return Object.assign({}, state, { unapprovedNodes: action.unapprovedNodes });
    case RECEIVE_OFFICIAL_PERIODS:
        return Object.assign({}, state, { periods: action.periods });
    case RECEIVE_OFFICIAL_DELTA_TICKET:
        return Object.assign({}, state, { receiveToggler: !state.receiveToggler });
    case RECEIVE_OFFICIAL_DELTA_TICKET_FAILURE:
        return Object.assign({}, state, { failureToggler: !state.failureToggler });
    case RESET_OFFICIAL_PERIODS:
        return Object.assign({}, state, { periods: { officialPeriods: [] } });
    default:
        return state;
    }
};
