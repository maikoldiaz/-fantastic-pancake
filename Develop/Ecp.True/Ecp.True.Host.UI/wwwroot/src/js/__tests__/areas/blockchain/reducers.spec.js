import * as actions from '../../../modules/administration/blockchain/actions';
import { blockchain } from '../../../modules/administration/blockchain/reducers';

describe('Reducers for transactionGrid', () => {
    const initialState = {
        transaction: {},
        transactions: { events: [] },
        transactionDetails: {},
        nextPageHead: null, previousHead: null, nextHead: null
    };
    const initialStatewithevents = {
        transaction: {},
        transactionDetails: {},
        transactions: { events: [{ type: 1 }] }

    };
    it('should handle action RECEIVE_CURRENT_HEAD',
        function () {
            const action = {
                type: actions.RECEIVE_CURRENT_HEAD,
                transactions: { events: [] }
            };
            const newState = Object.assign({}, initialState);
            expect(blockchain(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_BLOCKCHAIN_TRANSACTIONS',
        function () {
            const action = {
                type: actions.RECEIVE_BLOCKCHAIN_TRANSACTIONS,
                transaction: {},
                transactions: { headblock: 1, events: [{ type: 1 }] },
                nextPageHead: 1, previousHead: 2, transactionsToggler: true
            };
            const newState = Object.assign({}, initialStatewithevents, { transactions: { events: [{ type: 1 }] }, transactionsToggler: true });

            expect(blockchain(initialStatewithevents, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_BLOCKCHAIN_TRANSACTIONS when state empty',
        function () {
            const action = {
                type: actions.RECEIVE_BLOCKCHAIN_TRANSACTIONS,
                transactions: { headblock: 1, events: [{ type: 3 }] },
                nextPageHead: null, previousHead: 2, transactionsToggler: true
            };
            const newState = Object.assign({}, initialState, { transactions: { events: [{ type: 3 }], headblock: 1 }, currentHead: null, previousHead: [], transactionsToggler: true });
            expect(blockchain(initialState, action));
        });


    it('should handle action REQUEST_VIEW_TRANSACTION',
        function () {
            const transactionObject = {
                type: 'test',
                transactionHash: 'description',
                blockNumber: true
            };
            const action = {
                type: actions.REQUEST_VIEW_TRANSACTION,
                transaction: transactionObject
            };
            const newState = Object.assign({}, initialState, { transaction: transactionObject });
            expect(blockchain(initialState, action)).toEqual(newState);
        });

    it('should handle action SAVE_BLOCK_RANGE',
        function () {
            const blockRange = {
                headBlock: '1',
                tailBlock: '2',
                event: 3
            };
            const action = {
                type: actions.SAVE_BLOCK_RANGE,
                blockRange
            };
            const newState = Object.assign({}, initialState, { blockRange: blockRange });
            expect(blockchain(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_BLOCKCHAIN_RANGE_EVENTS',
        function () {
            const events = {
                events: []
            };
            const action = {
                type: actions.RECEIVE_BLOCKCHAIN_RANGE_EVENTS,
                events
            };
            const newState = Object.assign({}, initialState, { blockRangeEvents: events.events, blockRangeEventsToggler: true });
            expect(blockchain(initialState, action)).toEqual(newState);
        });

    it('should handle action BLOCKCHAIN_RANGE_EVENTS_FAILURE',
        function () {
            const action = {
                type: actions.BLOCKCHAIN_RANGE_EVENTS_FAILURE
            };
            const newState = Object.assign({}, initialState, { blockRangeEventsFailureToggler: true });
            expect(blockchain(initialState, action)).toEqual(newState);
        });
    it('should handle action default',
        function () {
            const action = {
                type: 'default'
            };
            expect(blockchain(initialState, action)).toEqual(initialState);
        });
});
