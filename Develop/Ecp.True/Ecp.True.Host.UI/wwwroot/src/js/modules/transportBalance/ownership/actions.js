import { apiService } from './../../../common/services/apiService';

export const REQUEST_OWNERSHIP_CALCULATION_DATES = 'REQUEST_OWNERSHIP_CALCULATION_DATES';
export const RECEIVE_OWNERSHIP_CALCULATION_DATES = 'RECEIVE_OWNERSHIP_CALCULATION_DATES';
export const REQUEST_OWNERSHIP_CALCULATION_VALIDATIONS = 'REQUEST_OWNERSHIP_CALCULATION_VALIDATIONS';
export const RECEIVE_OWNERSHIP_CALCULATION_VALIDATIONS = 'RECEIVE_OWNERSHIP_CALCULATION_VALIDATIONS';
export const SET_OWNERSHIP_CALCULATION_INFO = 'SET_OWNERSHIP_CALCULATION_INFO';
export const EXECUTE_OWNERSHIP_CALCULATION = 'CREATE_OWNERSHIP_CALCULATION';
export const RECEIVE_CREATE_OWNERSHIP_CALCULATION = 'RECEIVE_CREATE_OWNERSHIP_CALCULATION';
export const CLEAR_SEGMENT_AND_DATE = 'CLEAR_SEGMENT_AND_DATE';
export const RECEIVE_OWNERSHIP_FAILURE = 'RECEIVE_OWNERSHIP_FAILURE';
export const SAVE_SELECTED_OWNERSHIP_TICKET = 'SAVE_SELECTED_OWNERSHIP_TICKET';
export const RECEIVE_CONCILIATION_TICKET = 'RECEIVE_CONCILIATION_TICKET';
export const FAILURE_CONCILIATION_TICKET = 'FAILURE_CONCILIATION_TICKET';
export const REQUEST_CONCILIATION_TICKET = 'REQUEST_CONCILIATION_TICKET';
export const RECEIVE_OWNERSHIPNODE_TICKET_DATA = 'RECEIVE_OWNERSHIPNODE_TICKET_DATA';
export const FAILURE_OWNERSHIPNODE_TICKET_DATA = 'FAILURE_OWNERSHIPNODE_TICKET_DATA';
export const REQUEST_OWNERSHIPNODE_TICKET_DATA = 'REQUEST_OWNERSHIPNODE_TICKET_DATA';
export const RECEIVE_LAST_OPERATIONAL_CONCILIATION_TICKET = 'RECEIVE_LAST_OPERATIONAL_CONCILIATION_TICKET';
export const REQUEST_LAST_OPERATIONAL_CONCILIATION_TICKET = 'REQUEST_LAST_OPERATIONAL_CONCILIATION_TICKET';

const receiveOwnershipCalculationDates = dates => {
    return {
        type: RECEIVE_OWNERSHIP_CALCULATION_DATES,
        dates
    };
};

export const requestOwnershipCalculationDates = segmentId =>{
    const path = apiService.ownership.getDates(segmentId);
    return {
        type: REQUEST_OWNERSHIP_CALCULATION_DATES,
        fetchConfig: {
            path,
            success: json => receiveOwnershipCalculationDates(json)
        }
    };
};

export const setOwnershipCalculationInfo = ticket => {
    return {
        type: SET_OWNERSHIP_CALCULATION_INFO,
        ticket
    };
};


const receiveOwnershipCalculationValidations = validations => {
    return {
        type: RECEIVE_OWNERSHIP_CALCULATION_VALIDATIONS,
        validations
    };
};

export const requestOwnershipCalculationValidations = data =>{
    const path = apiService.ownership.validateNodes();
    return {
        type: REQUEST_OWNERSHIP_CALCULATION_VALIDATIONS,
        fetchConfig: {
            path,
            method: 'POST',
            body: data,
            success: json => receiveOwnershipCalculationValidations(json)
        }
    };
};

const receiveCreateOwnershipCalculation = status => {
    return {
        type: RECEIVE_CREATE_OWNERSHIP_CALCULATION,
        status
    };
};

const receiveOwnershipFailure = () => {
    return {
        type: RECEIVE_OWNERSHIP_FAILURE
    };
};

export const executeOwnershipCalculation = ticket => {
    return {
        type: EXECUTE_OWNERSHIP_CALCULATION,
        fetchConfig: {
            path: apiService.ownership.saveOwnershipCalculation(),
            body: ticket,
            success: json => receiveCreateOwnershipCalculation(json),
            failure: receiveOwnershipFailure
        }
    };
};

export const clearSegmentAndDate = () => {
    return {
        type: CLEAR_SEGMENT_AND_DATE
    };
};

const receiveConciliation = () => {
    return {
        type: RECEIVE_CONCILIATION_TICKET
    };
};
const failureConciliation = response => {
    return {
        type: FAILURE_CONCILIATION_TICKET,
        response
    };
};
export const requestConciliation = data => {
    return {
        type: REQUEST_CONCILIATION_TICKET,
        fetchConfig: {
            path: apiService.conciliation.requestConciliation(),
            method: 'POST',
            body: data,
            success: receiveConciliation,
            failure: failureConciliation
        }
    };
};

export const saveSelectedTicket = ticket => {
    return {
        type: SAVE_SELECTED_OWNERSHIP_TICKET,
        ticket
    };
};

const receiveOwnershipNodeData = ownershipNodesData => {
    return {
        type: RECEIVE_OWNERSHIPNODE_TICKET_DATA,
        ownershipNodesData
    };
};
const failureOwnershipNodeData = () => {
    return {
        type: FAILURE_OWNERSHIPNODE_TICKET_DATA
    };
};
export const requestOwnershipNodeData = ticketId => {
    return {
        type: REQUEST_OWNERSHIPNODE_TICKET_DATA,
        fetchConfig: {
            path: apiService.ownershipNode.queryByTicketId(ticketId),
            success: receiveOwnershipNodeData,
            failure: failureOwnershipNodeData
        }
    };
};

export const receiveLastOperationalTicket = ticket => {
    return {
        type: RECEIVE_LAST_OPERATIONAL_CONCILIATION_TICKET,
        ticket
    };
};
export const requestLastOperationalTicket = filter => {
    return {
        type: 'REQUEST_LAST_OPERATIONAL_CONCILIATION_TICKET',
        fetchConfig: {
            path: apiService.ticket.getLastOperationalTicketsPerSegmentWithStatus(filter),
            success: receiveLastOperationalTicket
        }
    };
};
