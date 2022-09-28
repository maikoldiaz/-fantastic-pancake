import { apiService } from '../../../common/services/apiService';

export const OFFICIAL_INITIAL_BALANCE_SAVE_FILTER = 'OFFICIAL_INITIAL_BALANCE_SAVE_FILTER';
export const saveOfficialInitialBalanceFilter = data => {
    return {
        type: OFFICIAL_INITIAL_BALANCE_SAVE_FILTER,
        data
    };
};

export const clearOfficialInitialBalanceFilter = () => {
    return {
        type: OFFICIAL_INITIAL_BALANCE_SAVE_FILTER,
        data: null
    };
};

export const OFFICIAL_INITIAL_BALANCE_SELECT_ELEMENT = 'OFFICIAL_INITIAL_BALANCE_SELECT_ELEMENT';
export const onSelectedElement = element => {
    return {
        type: OFFICIAL_INITIAL_BALANCE_SELECT_ELEMENT,
        element
    };
};

export const RESET_OFFICIAL_INITIAL_BALANCE_FILTER = 'RESET_OFFICIAL_INITIAL_BALANCE_FILTER';
export const resetOfficialInitialBalanceFilter = () => {
    return {
        type: RESET_OFFICIAL_INITIAL_BALANCE_FILTER
    };
};

export const RECEIVE_OFFICIAL_INITIAL_BALANCE = 'RECEIVE_OFFICIAL_INITIAL_BALANCE';
export const receiveOfficialInitialBalance = executionId => {
    return {
        type: RECEIVE_OFFICIAL_INITIAL_BALANCE,
        executionId
    };
};

export const RECEIVE_ERROR_OFFICIAL_INITIAL_BALANCE = 'RECEIVE_ERROR_OFFICIAL_INITIAL_BALANCE';
export const receiveErrorOfficialInitialBalance = () => {
    return {
        type: RECEIVE_ERROR_OFFICIAL_INITIAL_BALANCE
    };
};

export const SAVE_OFFICIAL_INITIAL_BALANCE = 'SAVE_OFFICIAL_INITIAL_BALANCE';
export const saveOfficialInitialBalance = body => {
    return {
        type: SAVE_OFFICIAL_INITIAL_BALANCE,
        fetchConfig: {
            path: apiService.ticket.postOfficialInitialBalanceReport(),
            body,
            success: receiveOfficialInitialBalance,
            failure: receiveErrorOfficialInitialBalance
        }
    };
};

export const RECEIVE_OFFICIAL_INITIAL_BALANCE_STATUS = 'RECEIVE_OFFICIAL_INITIAL_BALANCE_STATUS';
export const receiveOfficialInitialBalanceStatus = status => {
    return {
        type: RECEIVE_OFFICIAL_INITIAL_BALANCE_STATUS,
        status
    };
};

export const REQUEST_OFFICIAL_INITIAL_BALANCE_STATUS = 'REQUEST_OFFICIAL_INITIAL_BALANCE_STATUS';
export const requestOfficialInitialBalanceStatus = executionId => {
    return {
        type: REQUEST_OFFICIAL_INITIAL_BALANCE_STATUS,
        fetchConfig: {
            path: apiService.ticket.requestReportExecutionStatus(executionId),
            success: receiveOfficialInitialBalanceStatus,
            showProgress: false
        }
    };
};

export const REFRESH_STATUS = 'REFRESH_STATUS';
export const refreshStatus = () => {
    return {
        type: REFRESH_STATUS
    };
};

export const CLEAR_STATUS = 'CLEAR_STATUS';
export const clearStatus = () => {
    return {
        type: CLEAR_STATUS
    };
};

export const BUILD_INITIAL_REPORT_EXECUTION_FILTERS = 'BUILD_INITIAL_REPORT_EXECUTION_FILTERS';
export const buildReportFilters = execution => {
    return {
        type: BUILD_INITIAL_REPORT_EXECUTION_FILTERS,
        execution
    };
};

export const CLEAR_OFFICIAL_INITIAL_NODES = 'CLEAR_OFFICIAL_INITIAL_NODES';
export const clearSelectedNode = status => {
    return {
        type: CLEAR_OFFICIAL_INITIAL_NODES,
        status
    };
};

