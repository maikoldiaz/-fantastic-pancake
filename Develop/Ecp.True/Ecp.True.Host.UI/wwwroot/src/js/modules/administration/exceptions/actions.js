import { apiService } from '../../../common/services/apiService';

export const INIT_ERROR_ADD_COMMENT = 'INIT_ERROR_ADD_COMMENT';
export const SAVE_ERROR_COMMENT = 'SAVE_ERROR_COMMENT';
export const RECEIVE_ERROR_COMMENT = 'RECEIVE_ERROR_COMMENT';
export const SET_SELECTED_DATA = 'SET_SELECTED_DATA';
export const GET_ERROR_DETAIL = 'GET_ERROR_DETAIL';
export const SET_ERROR_DETAIL = 'SET_ERROR_DETAIL';
export const RECEIVE_FILTERED_RECORD = 'RECEIVE_FILTERED_RECORD';

export const REQUEST_RETRY_RECORDS = 'REQUEST_RETRY_RECORDS';
export const RECEIVE_RETRY_RECORDS = 'RECEIVE_RETRY_RECORDS';

export const SAVE_PAGE_FILTERS = 'SAVE_PAGE_FILTERS';
export const RESET_GRID_FILTER = 'RESET_GRID_FILTER';

export const initErrorAddComment = (name, preText, postText, count) => {
    return {
        type: INIT_ERROR_ADD_COMMENT,
        name,
        preText,
        postText,
        count
    };
};

export const receiveErrorComment = status => {
    return {
        type: RECEIVE_ERROR_COMMENT,
        status
    };
};

export const receiveFilteredRecord = status => {
    return {
        type: RECEIVE_FILTERED_RECORD,
        status
    };
};

export const saveErrorComment = errorComments => {
    const path = apiService.error.saveErrorComment();
    return {
        type: SAVE_ERROR_COMMENT,
        fetchConfig: {
            path,
            method: 'POST',
            body: errorComments,
            success: json => receiveErrorComment(json)
        }
    };
};

export const getFilteredRecord = messageId => {
    const path = apiService.error.getFilteredError(messageId);
    return {
        type: RECEIVE_FILTERED_RECORD,
        fetchConfig: {
            path,
            method: 'GET',
            success: json => receiveFilteredRecord(json)
        }
    };
};

export const setSelectedData = error =>{
    return {
        type: SET_SELECTED_DATA,
        error
    };
};

export const receiveErrorDetail = errorDetail => {
    return {
        type: SET_ERROR_DETAIL,
        errorDetail
    };
};

export const getErrorDetail = errorId => {
    return {
        type: GET_ERROR_DETAIL,
        fetchConfig: {
            path: apiService.error.getErrorDetails(errorId),
            success: receiveErrorDetail,
            notFound: true
        }
    };
};

export const receiveRetryRecord = success => {
    return {
        type: RECEIVE_RETRY_RECORDS,
        success
    };
};

export const savePageFilters = pageFilters => {
    return {
        type: SAVE_PAGE_FILTERS,
        pageFilters
    };
};

export const requestRetryRecord = pendingTransactionIds => {
    return {
        type: REQUEST_RETRY_RECORDS,
        fetchConfig: {
            path: apiService.error.retryErrors(),
            body: pendingTransactionIds,
            success: receiveRetryRecord
        }
    };
};

export const resetGridFilter = () => {
    return {
        type: RESET_GRID_FILTER
    };
};


