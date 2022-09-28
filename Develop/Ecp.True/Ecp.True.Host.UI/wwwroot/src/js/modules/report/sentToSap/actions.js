import { apiService } from '../../../common/services/apiService';

export const BUILD_SEND_TO_SAP_REPORT_EXECUTION_FILTERS = 'BUILD_SEND_TO_SAP_REPORT_EXECUTION_FILTERS';
export const RESET_PERIOD_SEND_TO_SAP_REPORT = 'RESET_PERIOD_SEND_TO_SAP_REPORT';
export const REQUEST_SAP_REPORT_OFFICIAL_PERIODS = 'REQUEST_SAP_REPORT_OFFICIAL_PERIODS';
export const RECEIVE_SAP_REPORT_OFFICIAL_PERIODS = 'RECEIVE_SAP_REPORT_OFFICIAL_PERIODS';
export const REQUEST_SENT_TO_SAP_REPORT = 'REQUEST_SENT_TO_SAP_REPORT';
export const RECEIVE_SENT_TO_SAP_REPORT = 'RECEIVE_SENT_TO_SAP_REPORT';
export const FAILED_RECEIVE_SENT_TO_SAP_REPORT = 'FAILED_RECEIVE_SENT_TO_SAP_REPORT';

export const buildReportFilters = execution => {
    return {
        type: BUILD_SEND_TO_SAP_REPORT_EXECUTION_FILTERS,
        execution
    };
};

export const resetPeriodSendToSapReport = () => {
    return {
        type: RESET_PERIOD_SEND_TO_SAP_REPORT
    };
};

const receiveOfficialPeriods = periods => {
    return {
        type: RECEIVE_SAP_REPORT_OFFICIAL_PERIODS,
        periods
    };
};
export const getOfficialPeriods = (segmentId, years) => {
    return {
        type: REQUEST_SAP_REPORT_OFFICIAL_PERIODS,
        fetchConfig: {
            path: apiService.officialDelta.getOfficialPeriods(segmentId, years, false),
            success: receiveOfficialPeriods
        }
    };
};

export const receivedSendToSapReport = executionId => {
    return {
        type: RECEIVE_SENT_TO_SAP_REPORT,
        status: true,
        executionId
    };
};

export const failedReceivedSendToSapReport = () => {
    return {
        type: FAILED_RECEIVE_SENT_TO_SAP_REPORT,
        status: false
    };
};

export const requestSendToSapReport = body => {
    return {
        type: REQUEST_SENT_TO_SAP_REPORT,
        fetchConfig: {
            path: apiService.reports.requestSendToSapReport(),
            method: 'POST',
            body,
            success: receivedSendToSapReport,
            failure: failedReceivedSendToSapReport
        }
    };
};
