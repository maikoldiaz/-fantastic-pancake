import * as actions from '../../../../modules/report/officialDeltaNode/actions';
import { officialDeltaNode } from '../../../../modules/report/officialDeltaNode/reducers.js';
import * as deltaActions from '../../../../modules/dailyBalance/officialDeltaPerNode/actions';

describe('Reducers for balancePerNode', () => {
    const initialState = {
        filterSettings: {}, isSaveForm: false
    };

    it('should handle action OFFICIAL_DELTA_SELECT_ELEMENT',
        function () {
            const action = {
                type: actions.OFFICIAL_DELTA_SELECT_ELEMENT,
                element: 'element'
            };
            const newState = Object.assign({}, initialState, { formValues: { element: 'element' } });
            expect(officialDeltaNode(initialState, action)).toEqual(newState);
        });

    it('should handle action OFFICIAL_DELTA_SAVE_FILTER',
        function () {
            const action = {
                type: actions.OFFICIAL_DELTA_SAVE_FILTER,
                data: { element: {} }
            };
            const newState = Object.assign({}, initialState, { filters: { element: {} }, formValues: { element: {} } });
            expect(officialDeltaNode(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_DELTA_NODE_DETAILS',
        function () {
            const action = {
                type: actions.RECEIVE_DELTA_NODE_DETAILS,
                executionId: 23,
                node: { startDate: '10-Abr-2020', endDate: '11-Abr-2020' }
            };
            const newState = Object.assign({}, initialState, { reportToggler: true, filters: {
                deltaNodeId: undefined,
                elementId: undefined,
                elementName: undefined,
                nodeId: undefined,
                nodeName: undefined,
                nodeStatus: undefined,
                reportType: '10.10.12BalanceOficialPorNodo12',
                ticketStatus: undefined
            } });
            expect(officialDeltaNode(initialState, action).reportToggler).toEqual(true);
        });

    it('should handle action RESET_DELTA_NODE_SOURCE',
        function () {
            const action = {
                type: actions.RESET_DELTA_NODE_SOURCE
            };
            const newState = Object.assign({}, initialState, { source: null });
            expect(officialDeltaNode(initialState, action)).toEqual(newState);
        });

    it('should handle action RESET_OFFICIAL_DELTA_FILTER',
        function () {
            const action = {
                type: actions.RESET_OFFICIAL_DELTA_FILTER
            };
            const newState = Object.assign({}, initialState, { filters: null });
            expect(officialDeltaNode(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_SUBMIT_FOR_APPROVAL',
        function () {
            const action = {
                type: actions.RECEIVE_SUBMIT_FOR_APPROVAL,
                sendForApprovalResponse: { isApproverExist: true },
                approveToggler: true
            };
            const newState = Object.assign({}, initialState, { sendForApprovalResponse: { isApproverExist: true }, approveToggler: true });
            expect(officialDeltaNode(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_SUBMIT_FOR_REOPEN',
        function () {
            const action = {
                type: actions.RECEIVE_SUBMIT_FOR_REOPEN,
                reopenNodesList: [],
                reopenToggler: true
            };
            const newState = Object.assign({}, initialState, { reopenNodesList: [], reopenToggler: true });
            expect(officialDeltaNode(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_REOPEN',
        function () {
            const action = {
                type: actions.RECEIVE_REOPEN,
                reopenConfirmation: null,
                reopenConfirmationToggler: true
            };
            const newState = Object.assign({}, initialState, { reopenConfirmation: null, reopenConfirmationToggler: true });
            expect(officialDeltaNode(initialState, action)).toEqual(newState);
        });

    it('should handle action SUBMISSION_TYPE',
        function () {
            const action = {
                type: actions.SUBMISSION_TYPE,
                submissionType: 1
            };
            const newState = Object.assign({}, initialState, { submissionType: 1 });
            expect(officialDeltaNode(initialState, action)).toEqual(newState);
        });

    it('should handle SET_DELTA_NODE_SOURCE',
        function () {
            const action = {
                type: deltaActions.SET_DELTA_NODE_SOURCE,
                source: {}
            };
            const newState = Object.assign({}, initialState, { source: action.source });
            expect(officialDeltaNode(initialState, action)).toEqual(newState);
        });

    it('should handle default',
        function () {
            const action = {
                type: 'test',
                submissionType: 1
            };
            expect(officialDeltaNode(initialState, action)).toEqual(initialState);
        });

    it('should handle action RECEIVE_DELTA_NODE_MOVEMENTS',
        function () {
            const action = {
                type: actions.RECEIVE_DELTA_NODE_MOVEMENTS,
                data: []
            };
            const newState = Object.assign({}, initialState, { manualMovements: action.data });
            expect(officialDeltaNode(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_SAVE_DELTA_NODE_MOVEMENTS',
        function () {
            const action = {
                type: actions.RECEIVE_SAVE_DELTA_NODE_MOVEMENTS,
                status: true
            };
            const newState = Object.assign({}, initialState, { isSaveForm: true });
            expect(officialDeltaNode(initialState, action)).toEqual(newState);
        });
});
