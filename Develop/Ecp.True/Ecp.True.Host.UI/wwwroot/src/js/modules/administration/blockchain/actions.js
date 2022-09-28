import { apiService } from '../../../common/services/apiService';
export const REQUEST_VIEW_INITIALIZE_TRANSACTIONS = 'REQUEST_VIEW_INITIALIZE_TRANSACTIONS';
export const RECEIVE_BLOCKCHAIN_TRANSACTIONS = 'RECEIVE_BLOCKCHAIN_TRANSACTIONS';
export const REQUEST_VIEW_TRANSACTION = 'REQUEST_VIEW_TRANSACTION';
export const BLOCKCHAIN_PAGE_CHANGE = 'BLOCKCHAIN_PAGE_CHANGE';
export const REQUEST_BLOCKCHAIN_TRANSACTIONS = 'REQUEST_BLOCKCHAIN_TRANSACTIONS';
export const RECEIVE_BLOCKCHAIN_TRANSACTION = 'RECEIVE_BLOCKCHAIN_TRANSACTION';
export const BLOCKCHAIN_TRANSACTIONS_RECEIVE_FAILURE = 'BLOCKCHAIN_TRANSACTIONS_RECEIVE_FAILURE';
export const RESET_BLOCKCHAIN_STATE = 'RESET_BLOCKCHAIN_STATE';
export const RESET_BLOCKCHAIN_TRANSACTION = 'RESET_BLOCKCHAIN_TRANSACTION';
export const SAVE_BLOCK_RANGE = 'SAVE_BLOCK_RANGE';
export const REQUEST_BLOCKCHAIN_RANGE_EVENTS = 'REQUEST_BLOCKCHAIN_RANGE_EVENTS';
export const RECEIVE_BLOCKCHAIN_RANGE_EVENTS = 'RECEIVE_BLOCKCHAIN_RANGE_EVENTS';
export const BLOCKCHAIN_RANGE_EVENTS_FAILURE = 'BLOCKCHAIN_RANGE_EVENTS_FAILURE';

export const receiveTransactions = (transactions, head) => {
    return {
        type: RECEIVE_BLOCKCHAIN_TRANSACTIONS,
        transactions,
        head
    };
};

export const receiveFailure = name => {
    return {
        type: BLOCKCHAIN_TRANSACTIONS_RECEIVE_FAILURE,
        name
    };
};

export const requestTransactions = (path, pageSize, lastHead, name) => {
    return {
        type: REQUEST_VIEW_INITIALIZE_TRANSACTIONS,
        fetchConfig: {
            path,
            body: { pageSize, lastHead },
            success: json => receiveTransactions(json, lastHead),
            failure: () => receiveFailure(name)
        }
    };
};

export const viewTransaction = transaction => {
    return {
        type: REQUEST_VIEW_TRANSACTION,
        transaction
    };
};

export const onPageChange = isNext => {
    return {
        type: BLOCKCHAIN_PAGE_CHANGE,
        isNext
    };
};

export const receiveTransaction = transaction => {
    return {
        type: RECEIVE_BLOCKCHAIN_TRANSACTION,
        transaction
    };
};

export const requestTransaction = body => {
    return {
        type: REQUEST_BLOCKCHAIN_TRANSACTIONS,
        fetchConfig: {
            path: apiService.blockchain.getTransaction(),
            body,
            success: receiveTransaction
        }
    };
};

export const resetState = () => {
    return {
        type: RESET_BLOCKCHAIN_STATE
    };
};

export const resetTransaction = () => {
    return {
        type: RESET_BLOCKCHAIN_TRANSACTION
    };
};

export const saveBlockRange = blockRange => {
    return {
        type: SAVE_BLOCK_RANGE,
        blockRange
    };
};

export const receiveRangeEvents = events => {
    return {
        type: RECEIVE_BLOCKCHAIN_RANGE_EVENTS,
        events
    };
};

export const receiveRangeFailure = name => {
    return {
        type: BLOCKCHAIN_RANGE_EVENTS_FAILURE,
        name
    };
};

export const requestRangeEvents = (path, body) => {
    return {
        type: REQUEST_BLOCKCHAIN_RANGE_EVENTS,
        fetchConfig: {
            path,
            body,
            success: json => receiveRangeEvents(json),
            failure: () => receiveRangeFailure()
        }
    };
};
