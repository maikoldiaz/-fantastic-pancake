import { apiService } from './../../../common/services/apiService';

const REQUEST_ADD_LOGISTICS_TICKET = 'REQUEST_ADD_LOGISTICS_TICKET';
const REQUEST_LAST_OWNERSHIP_PERFORMED_DATE = 'REQUEST_LAST_OWNERSHIP_PERFORMED_DATE';

export const RECEIVE_ADD_LOGISTICS_TICKET = 'RECEIVE_ADD_LOGISTICS_TICKET';
export const RECEIVE_DATE_FOR_SEGMENT = 'RECEIVE_DATE_FOR_SEGMENT';
export const SET_INITIAL_DATE = 'SET_INITIAL_DATE';
export const CLEAR_LOGISTICS_OWNERSHIP_REQUEST_DATA = 'CLEAR_LOGISTICS_OWNERSHIP_REQUEST_DATA';
export const CLEAR_SEARCH_NODES = 'CLEAR_SEARCH_NODES';
export const ON_SEGMENT_SELECT = 'ON_SEGMENT_SELECT';
export const REQUEST_SEARCH_NODES = 'REQUEST_SEARCH_NODES';
export const RECEIVE_SEARCH_NODES = 'RECEIVE_SEARCH_NODES';
export const SET_LOGISTICS_INFO = 'SET_LOGISTICS_INFO';
export const REQUEST_LOGISTICS_VALIDATION_DATA = 'REQUEST_LOGISTICS_VALIDATION_DATA';
export const RECEIVE_LOGISTICS_VALIDATION_DATA = 'RECEIVE_LOGISTICS_VALIDATION_DATA';
export const RECEIVE_LOGISTICS_FAILURE = 'RECEIVE_LOGISTICS_FAILURE';

const receiveLogisticTicket = (status, name) => {
    return {
        type: RECEIVE_ADD_LOGISTICS_TICKET,
        status,
        name
    };
};

const receiveLogisticFailure = name=> {
    return {
        type: RECEIVE_LOGISTICS_FAILURE,
        name
    };
};

const receivedDate = (date, name) => {
    return {
        type: RECEIVE_DATE_FOR_SEGMENT,
        date,
        name
    };
};

export const createLogisticTicket = (operationalCutOff, name) => {
    return {
        type: REQUEST_ADD_LOGISTICS_TICKET,
        fetchConfig: {
            path: apiService.operationalCutOff.saveOperationalCutOff(),
            body: operationalCutOff,
            success: status => receiveLogisticTicket(status, name),
            failure: () => receiveLogisticFailure(name)
        }
    };
};

export const getLastOwnershipPerformedDateForSelectedSegment = (selectedSegmentId, name) => {
    return {
        type: REQUEST_LAST_OWNERSHIP_PERFORMED_DATE,
        fetchConfig: {
            path: apiService.ownership.getLastOwnershipPerformedDateForSelectedSegment(selectedSegmentId),
            success: date => receivedDate(date, name)
        }
    };
};

export const setInitialDate = date => {
    return {
        type: SET_INITIAL_DATE,
        date
    };
};

export const clearLogisticsOwnershipRequestData = () => {
    return {
        type: CLEAR_LOGISTICS_OWNERSHIP_REQUEST_DATA
    };
};

export const clearSearchNodes = name => {
    return {
        type: CLEAR_SEARCH_NODES,
        name
    };
};

export const onSegmentSelect = (selectedSegment, name) => {
    return {
        type: ON_SEGMENT_SELECT,
        selectedSegment,
        name
    };
};

const receiveSearchNodes = (json, name) => {
    const nodes = json.value ? json.value.map(x => x.node) : [];
    return {
        type: RECEIVE_SEARCH_NODES,
        nodes,
        name
    };
};

export const requestSearchNodes = (elementId, searchText, name) => {
    return {
        type: REQUEST_SEARCH_NODES,
        fetchConfig: {
            path: apiService.node.searchSendToSapNodes(elementId, searchText),
            success: json => receiveSearchNodes(json, name),
            failure: () => clearSearchNodes(name)
        }
    };
};

export const setLogisticsInfo = (logisticsInfo, name) => {
    return {
        type: SET_LOGISTICS_INFO,
        logisticsInfo,
        name
    };
};

const receiveLogisticsValidationData = (validationData, name) => {
    return {
        type: RECEIVE_LOGISTICS_VALIDATION_DATA,
        validationData,
        name
    };
};

export const getLogisticsValidationData = (ticket, name, isOfficialLogistics) => {
    return {
        type: REQUEST_LOGISTICS_VALIDATION_DATA,
        fetchConfig: {
            path: isOfficialLogistics ? apiService.ticket.getOfficialLogisticsValidationData() : apiService.ticket.getLogisticsValidationData(),
            body: ticket,
            success: json => receiveLogisticsValidationData(json, name)
        }
    };
};

export const REQUEST_LOGISTICS_OFFICIAL_PERIODS = 'REQUEST_LOGISTICS_OFFICIAL_PERIODS';
export const RECEIVE_LOGISTICS_OFFICIAL_PERIODS = 'RECEIVE_LOGISTICS_OFFICIAL_PERIODS';

const receiveLogisticsOfficialPeriods = (periods, name) => {
    return {
        type: RECEIVE_LOGISTICS_OFFICIAL_PERIODS,
        periods,
        name
    };
};

export const getLogisticsOfficialPeriods = (segmentId, years, name) => {
    return {
        type: REQUEST_LOGISTICS_OFFICIAL_PERIODS,
        fetchConfig: {
            path: apiService.officialDelta.getOfficialPeriods(segmentId, years, true),
            success: json => receiveLogisticsOfficialPeriods(json, name)
        }
    };
};


export const SET_IS_PERIOD_VALID = 'SET_IS_PERIOD_VALID';

export const setIsPeriodValid = (isPeriodValid, name) => {
    return {
        type: SET_IS_PERIOD_VALID,
        isPeriodValid,
        name
    };
};
