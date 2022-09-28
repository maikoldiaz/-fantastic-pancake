import { apiService } from '../../../common/services/apiService';

export const RECEIVE_REPORT_EXECUTION_DETAILS = 'RECEIVE_REPORT_EXECUTION_DETAILS';
export const GET_REPORT_EXECUTION_DETAILS = 'GET_REPORT_EXECUTION_DETAILS';

export const receiveReportDetails = json => {
    return {
        type: RECEIVE_REPORT_EXECUTION_DETAILS,
        execution: json.value.length > 0 ? json.value[0] : null
    };
};

export const getReportDetails = executionId => {
    return {
        type: GET_REPORT_EXECUTION_DETAILS,
        fetchConfig: {
            path: apiService.reports.queryById(executionId),
            success: receiveReportDetails
        }
    };
};
