import {
    RECEIVE_BLOCKCHAIN_TRANSACTIONS,
    REQUEST_VIEW_TRANSACTION,
    BLOCKCHAIN_PAGE_CHANGE,
    RECEIVE_BLOCKCHAIN_TRANSACTION,
    BLOCKCHAIN_TRANSACTIONS_RECEIVE_FAILURE,
    RESET_BLOCKCHAIN_STATE,
    RESET_BLOCKCHAIN_TRANSACTION,
    SAVE_BLOCK_RANGE,
    RECEIVE_BLOCKCHAIN_RANGE_EVENTS,
    BLOCKCHAIN_RANGE_EVENTS_FAILURE
} from './actions.js';
import { utilities } from '../../../common/services/utilities';

const blockchainInitialState = {
    nextPageHead: null,
    previousHead: [],
    transactions: { events: [] },
    isNext: true,
    currentPage: 1,
    transactionDetails: {}
};

export const blockchain = (state = blockchainInitialState, action = {}) => {
    switch (action.type) {
    case RECEIVE_BLOCKCHAIN_TRANSACTIONS:
        if (state.transactions.events.length) {
            return Object.assign({},
                state,
                {
                    currentHead: action.head,
                    transactions: {
                        events: action.transactions.events,
                        headBlock: action.transactions.headBlock
                    },
                    nextPageHead: action.transactions.headBlock,
                    transactionsToggler: !state.transactionsToggler
                });
        }
        return Object.assign({},
            state,
            {
                transactions: action.transactions,
                nextPageHead: action.transactions.headBlock,
                currentHead: null,
                previousHead: [],
                transactionsToggler: !state.transactionsToggler
            });
    case REQUEST_VIEW_TRANSACTION:
        return Object.assign({}, state, { transaction: action.transaction, transactionDetails: {} });
    case RECEIVE_BLOCKCHAIN_TRANSACTION:
        return Object.assign({}, state, { transactionDetails: action.transaction });
    case BLOCKCHAIN_PAGE_CHANGE: {
        const previousHead = utilities.isNullOrUndefined(state.currentHead) ? -1 : state.currentHead;
        const page = { page: state.currentPage, head: previousHead };
        return Object.assign({}, state, {
            isNext: action.isNext,
            currentPage: action.isNext ? state.currentPage + 1 : state.currentPage - 1,
            previousHead: action.isNext && state.previousHead.indexOf(page) === -1 ? state.previousHead.concat(page) : state.previousHead });
    }
    case BLOCKCHAIN_TRANSACTIONS_RECEIVE_FAILURE:
        return Object.assign({}, state, { transactionFailure: !state.transactionFailure });
    case RESET_BLOCKCHAIN_STATE:
        return Object.assign({}, state, { transactions: { events: [] }, nextPageHead: null, currentHead: null,
            previousHead: [], currentPage: 1, isNext: true, transaction: null, transactionDetails: null });
    case RESET_BLOCKCHAIN_TRANSACTION:
        return Object.assign({}, state, { transaction: null, transactionDetails: null });
    case SAVE_BLOCK_RANGE:
        return Object.assign({}, state, { blockRange: action.blockRange });
    case RECEIVE_BLOCKCHAIN_RANGE_EVENTS:
        return Object.assign({}, state, { blockRangeEvents: action.events.events, blockRangeEventsToggler: !state.blockRangeEventsToggler });
    case BLOCKCHAIN_RANGE_EVENTS_FAILURE:
        return Object.assign({}, state, { blockRangeEventsFailureToggler: !state.blockRangeEventsFailureToggler });
    default:
        return state;
    }
};
