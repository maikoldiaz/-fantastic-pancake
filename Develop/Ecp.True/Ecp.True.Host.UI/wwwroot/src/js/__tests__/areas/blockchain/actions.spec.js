import * as actions from '../../../modules/administration/blockchain/actions';
import { apiService } from '../../../common/services/apiService';

it('should request Transactions success', () => {
    const REQUEST_VIEW_INITIALIZE_TRANSACTIONS = 'REQUEST_VIEW_INITIALIZE_TRANSACTIONS';
    const action = actions.requestTransactions('apiurl', '', '', '');
    expect(action.type).toEqual(REQUEST_VIEW_INITIALIZE_TRANSACTIONS);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual('apiurl');
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const RECEIVE_BLOCKCHAIN_TRANSACTIONS = 'RECEIVE_BLOCKCHAIN_TRANSACTIONS';
    expect(receiveAction.type).toEqual(RECEIVE_BLOCKCHAIN_TRANSACTIONS);
});

it('should request Transactions failed', () => {
    const REQUEST_VIEW_INITIALIZE_TRANSACTIONS = 'REQUEST_VIEW_INITIALIZE_TRANSACTIONS';
    const action = actions.requestTransactions('apiurl', '', '', '');
    expect(action.type).toEqual(REQUEST_VIEW_INITIALIZE_TRANSACTIONS);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual('apiurl');
    expect(action.fetchConfig.failure).toBeDefined();
    const receiveActionfailed = action.fetchConfig.failure(true);
    const BLOCKCHAIN_TRANSACTIONS_RECEIVE_FAILURE = 'BLOCKCHAIN_TRANSACTIONS_RECEIVE_FAILURE';
    expect(receiveActionfailed.type).toEqual(BLOCKCHAIN_TRANSACTIONS_RECEIVE_FAILURE);
});

it('should view Transaction', () => {
    const REQUEST_VIEW_TRANSACTION = 'REQUEST_VIEW_TRANSACTION';
    const transactionObject = {
        type: 'test',
        transactionHash: 'description',
        blockNumber: true
    };
    const action = actions.viewTransaction(transactionObject);
    expect(action.type).toEqual(REQUEST_VIEW_TRANSACTION);
});

it('should receive current head', () => {
    const BLOCKCHAIN_PAGE_CHANGE = 'BLOCKCHAIN_PAGE_CHANGE';
    const action = actions.onPageChange(true);
    expect(action.type).toEqual(BLOCKCHAIN_PAGE_CHANGE);
});

it('should receive transactions', () => {
    const RECEIVE_BLOCKCHAIN_TRANSACTIONS = 'RECEIVE_BLOCKCHAIN_TRANSACTIONS';
    const transactionObject = { headblock: 1, events: [{ type: 1 }] };
    const action = actions.receiveTransactions(transactionObject);
    expect(action.type).toEqual(RECEIVE_BLOCKCHAIN_TRANSACTIONS);
});

it('should request Transactions details', () => {
    const REQUEST_BLOCKCHAIN_TRANSACTIONS = 'REQUEST_BLOCKCHAIN_TRANSACTIONS';
    const action = actions.requestTransaction({});
    expect(action.type).toEqual(REQUEST_BLOCKCHAIN_TRANSACTIONS);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.blockchain.getTransaction());
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const RECEIVE_BLOCKCHAIN_TRANSACTION = 'RECEIVE_BLOCKCHAIN_TRANSACTION';
    expect(receiveAction.type).toEqual(RECEIVE_BLOCKCHAIN_TRANSACTION);
});

it('should reset state', () => {
    const RESET_BLOCKCHAIN_STATE = 'RESET_BLOCKCHAIN_STATE';
    const action = actions.resetState();
    expect(action.type).toEqual(RESET_BLOCKCHAIN_STATE);
});

it('should request events success', () => {
    const REQUEST_BLOCKCHAIN_RANGE_EVENTS = 'REQUEST_BLOCKCHAIN_RANGE_EVENTS';
    const action = actions.requestRangeEvents('apiurl', {});
    expect(action.type).toEqual(REQUEST_BLOCKCHAIN_RANGE_EVENTS);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual('apiurl');
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const RECEIVE_BLOCKCHAIN_RANGE_EVENTS = 'RECEIVE_BLOCKCHAIN_RANGE_EVENTS';
    expect(receiveAction.type).toEqual(RECEIVE_BLOCKCHAIN_RANGE_EVENTS);
});

it('should request events failed', () => {
    const REQUEST_BLOCKCHAIN_RANGE_EVENTS = 'REQUEST_BLOCKCHAIN_RANGE_EVENTS';
    const action = actions.requestRangeEvents('apiurl', {});
    expect(action.type).toEqual(REQUEST_BLOCKCHAIN_RANGE_EVENTS);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual('apiurl');
    expect(action.fetchConfig.failure).toBeDefined();
    const receiveActionfailed = action.fetchConfig.failure(true);
    const BLOCKCHAIN_RANGE_EVENTS_FAILURE = 'BLOCKCHAIN_RANGE_EVENTS_FAILURE';
    expect(receiveActionfailed.type).toEqual(BLOCKCHAIN_RANGE_EVENTS_FAILURE);
});

it('should save block range', () => {
    const SAVE_BLOCK_RANGE = 'SAVE_BLOCK_RANGE';
    const action = actions.saveBlockRange({});
    expect(action.type).toEqual(SAVE_BLOCK_RANGE);
    expect(action.blockRange).toEqual({});
});
