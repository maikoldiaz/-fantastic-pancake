import { apiService } from '../../../common/services/apiService';
import { receiveGridData } from '../../../common/components/grid/actions';

export const INIT_DELTA_SEGMENT = 'INIT_DELTA_SEGMENT';
export const INIT_DELTA_START_DATE = 'INIT_DELTA_START_DATE';
export const SET_DELTA_END_DATE = 'SET_DELTA_END_DATE';
export const INIT_DELTA_TICKET = 'INIT_DELTA_TICKET';
export const SET_DELTA_TICKET = 'SET_DELTA_TICKET';
export const REQUEST_END_DATE = 'REQUEST_END_DATE';
export const RECEIVE_END_DATE = 'RECEIVE_END_DATE';
export const REQUEST_PENDING_INVENTORIES = 'REQUEST_PENDING_INVENTORIES';
export const RECEIVE_PENDING_INVENTORIES = 'RECEIVE_PENDING_INVENTORIES';
export const REQUEST_PENDING_MOVEMENTS = 'REQUEST_PENDING_MOVEMENTS';
export const RECEIVE_PENDING_MOVEMENTS = 'RECEIVE_PENDING_MOVEMENTS';
export const SAVE_DELTA_CALCULATION = 'SAVE_DELTA_CALCULATION';
export const RECEIVE_SAVE_DELTA_CALCULATION = 'RECEIVE_SAVE_DELTA_CALCULATION';
export const SET_IS_READY = 'SET_IS_READY';
export const RECEIVE_DELTA_TICKET_FAILURE = 'RECEIVE_DELTA_TICKET_FAILURE';

export const initDeltaSegment = segment => {
    return {
        type: INIT_DELTA_SEGMENT,
        segment
    };
};

export const initDeltaStartDate = startDate => {
    return {
        type: INIT_DELTA_START_DATE,
        startDate
    };
};

export const setDeltaEndDate = endDate => {
    return {
        type: SET_DELTA_END_DATE,
        endDate
    };
};

export const receivedEndDate = date => {
    return {
        type: RECEIVE_END_DATE,
        date
    };
};

export const requestEndDate = segmentId => {
    return {
        type: REQUEST_END_DATE,
        fetchConfig: {
            path: apiService.ownership.getLastOwnershipPerformedDateForSelectedSegment(segmentId),
            success: receivedEndDate
        }
    };
};

export const initDeltaTicket = () =>{
    return {
        type: INIT_DELTA_TICKET
    };
};

export const setDeltaTicket = deltaTicket =>{
    return {
        type: SET_DELTA_TICKET,
        deltaTicket
    };
};

export const requestPendingInventories = (apiURL, deltaTicket, name) => {
    return {
        type: REQUEST_PENDING_INVENTORIES,
        fetchConfig: {
            path: apiURL,
            method: 'PUT',
            success: json => receiveGridData(json, name),
            body: deltaTicket
        }
    };
};


export const requestPendingMovements = (apiURL, deltaTicket, name) => {
    return {
        type: REQUEST_PENDING_MOVEMENTS,
        fetchConfig: {
            path: apiURL,
            method: 'PUT',
            success: json => receiveGridData(json, name),
            body: deltaTicket
        }
    };
};


export const receiveSaveDeltaCalculation = status => {
    return {
        type: RECEIVE_SAVE_DELTA_CALCULATION,
        status
    };
};

const receiveDeltaFailure = () => {
    return {
        type: RECEIVE_DELTA_TICKET_FAILURE
    };
};

export const saveDeltaCalculation = ticket => {
    return {
        type: SAVE_DELTA_CALCULATION,
        fetchConfig: {
            path: apiService.operationalDelta.saveOperationalDelta(),
            body: ticket,
            success: receiveSaveDeltaCalculation,
            failure: receiveDeltaFailure
        }
    };
};

export const setIsReady = isReady => {
    return {
        type: SET_IS_READY,
        isReady
    };
};
