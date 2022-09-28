import { apiService } from '../../../common/services/apiService';

export const INIT_OFFICIAL_DELTA_TICKET_PROCESS = 'INIT_OFFICIAL_DELTA_TICKET_PROCESS';
export const SET_IS_VALID = 'SET_IS_VALID';
export const SET_OFFICIAL_DELTA_TICKET = 'SET_OFFICIAL_DELTA_TICKET';
export const REQUEST_OFFICIAL_DELTA_VALIDATION_DATA = 'REQUEST_OFFICIAL_DELTA_VALIDATION_DATA';
export const RECEIVE_OFFICIAL_DELTA_VALIDATION_DATA = 'RECEIVE_OFFICIAL_DELTA_VALIDATION_DATA';
export const REQUEST_OFFICIAL_DELTA_TICKET = 'REQUEST_OFFICIAL_DELTA_TICKET';
export const RECEIVE_OFFICIAL_DELTA_TICKET = 'RECEIVE_OFFICIAL_DELTA_TICKET';
export const RESET_OFFICIAL_PERIODS = 'RESET_OFFICIAL_PERIODS';
export const RECEIVE_OFFICIAL_DELTA_TICKET_FAILURE = 'RECEIVE_OFFICIAL_DELTA_TICKET_FAILURE';

export const initOfficialDelta = () => {
    return {
        type: INIT_OFFICIAL_DELTA_TICKET_PROCESS
    };
};

export const setIsValid = isValid => {
    return {
        type: SET_IS_VALID,
        isValid
    };
};

export const setOfficialDeltaTicketInfo = ticket => {
    return {
        type: SET_OFFICIAL_DELTA_TICKET,
        ticket
    };
};

const receiveOfficialDeltaValidationData = unapprovedNodes => {
    return {
        type: RECEIVE_OFFICIAL_DELTA_VALIDATION_DATA,
        unapprovedNodes
    };
};

export const getOfficialDeltaValidationData = ticket => {
    return {
        type: REQUEST_OFFICIAL_DELTA_VALIDATION_DATA,
        fetchConfig: {
            path: apiService.officialDelta.getSonUnapproveNodes(),
            body: ticket,
            success: receiveOfficialDeltaValidationData
        }
    };
};

const receiveOfficialDelta = () => {
    return {
        type: RECEIVE_OFFICIAL_DELTA_TICKET
    };
};

const receiveOfficialDeltaFailure = () => {
    return {
        type: RECEIVE_OFFICIAL_DELTA_TICKET_FAILURE
    };
};

export const saveOfficialDelta = ticket => {
    return {
        type: REQUEST_OFFICIAL_DELTA_TICKET,
        fetchConfig: {
            path: apiService.officialDelta.saveOfficialDelta(),
            body: ticket,
            success: receiveOfficialDelta,
            failure: receiveOfficialDeltaFailure
        }
    };
};

export const REQUEST_OFFICIAL_PERIODS = 'REQUEST_OFFICIAL_PERIODS';
export const RECEIVE_OFFICIAL_PERIODS = 'RECEIVE_OFFICIAL_PERIODS';

const receiveOfficialPeriods = periods => {
    return {
        type: RECEIVE_OFFICIAL_PERIODS,
        periods
    };
};

export const getOfficialPeriods = (segmentId, years) => {
    return {
        type: REQUEST_OFFICIAL_PERIODS,
        fetchConfig: {
            path: apiService.officialDelta.getOfficialPeriods(segmentId, years, false),
            success: receiveOfficialPeriods
        }
    };
};

export const resetOfficialPeriods = () => {
    return {
        type: RESET_OFFICIAL_PERIODS
    };
};
