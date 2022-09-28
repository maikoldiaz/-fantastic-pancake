import { apiService } from '../../../common/services/apiService';

export const RESET_OFFICIAL_NODE_STATUS_PERIODS = 'RESET_OFFICIAL_NODE_STATUS_PERIODS';
export const SAVE_OFFICIAL_NODE_STATUS_FILTER = 'SAVE_OFFICIAL_NODE_STATUS_FILTER';
export const REQUEST_OFFICIAL_NODE_STATUS_PERIODS = 'REQUEST_OFFICIAL_NODE_STATUS_PERIODS';
export const RECEIVE_OFFICIAL_NODE_STATUS_PERIODS = 'RECEIVE_OFFICIAL_NODE_STATUS_PERIODS';
export const RECEIVE_SAVE_OFFICIAL_NODE_STATUS_REPORT = 'RECEIVE_SAVE_OFFICIAL_NODE_STATUS_REPORT';
export const FAILURE_SAVE_OFFICIAL_NODE_STATUS_REPORT = 'FAILURE_SAVE_OFFICIAL_NODE_STATUS_REPORT';
export const REQUEST_SAVE_OFFICIAL_NODE_STATUS_REPORT = 'REQUEST_SAVE_OFFICIAL_NODE_STATUS_REPORT';

export const resetOfficialNodeStatusPeriods = () => {
    return {
        type: RESET_OFFICIAL_NODE_STATUS_PERIODS
    };
};

export const saveOfficialNodeStatusFilter = filters => {
    return {
        type: SAVE_OFFICIAL_NODE_STATUS_FILTER,
        filters
    };
};

const receiveOfficialPeriods = periods => {
    return {
        type: RECEIVE_OFFICIAL_NODE_STATUS_PERIODS,
        periods
    };
};
export const getOfficialPeriods = (segmentId, years) => {
    return {
        type: REQUEST_OFFICIAL_NODE_STATUS_PERIODS,
        fetchConfig: {
            path: apiService.officialDelta.getOfficialPeriods(segmentId, years, false),
            success: receiveOfficialPeriods
        }
    };
};

const receiveOfficialNodeStatusReport = () => {
    return {
        type: RECEIVE_SAVE_OFFICIAL_NODE_STATUS_REPORT
    };
};
const failureOfficialNodeStatusReport = () => {
    return {
        type: FAILURE_SAVE_OFFICIAL_NODE_STATUS_REPORT
    };
};
export const requestOfficialNodeStatusReport = body => {
    return {
        type: REQUEST_SAVE_OFFICIAL_NODE_STATUS_REPORT,
        fetchConfig: {
            path: apiService.reports.requestOfficialNodeStatusReport(),
            method: 'POST',
            body,
            success: receiveOfficialNodeStatusReport,
            failure: failureOfficialNodeStatusReport
        }
    };
};
