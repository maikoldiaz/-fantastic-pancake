import * as actions from '../../../../modules/report/cutOff/actions';
import { cutOffReport } from '../../../../modules/report/cutOff/reducers';
import { dateService } from '../../../../common/services/dateService';
import { constants } from '../../../../common/services/constants';

describe('Reducers for cutOffReport', () => {
    const initialState = {
        selectedCategory: 'y',
        selectedElement: 'y',
        searchedNodes: ['node']
    };

    it('should handle action CUTOFF_REPORT_FILTER_ON_SELECT_CATEGORY',
        function () {
            const action = {
                type: actions.CUTOFF_REPORT_FILTER_ON_SELECT_CATEGORY,
                selectedCategory: 'x'
            };
            const newState = Object.assign({}, initialState, { selectedCategory: 'x' });
            expect(cutOffReport(initialState, action)).toEqual(newState);
        });

    it('should handle action CUTOFF_REPORT_FILTER_ON_SELECT_ELEMENT',
        function () {
            const action = {
                type: actions.CUTOFF_REPORT_FILTER_ON_SELECT_ELEMENT,
                selectedElement: 'x'
            };
            const newState = Object.assign({}, initialState, { selectedElement: 'x' });
            expect(cutOffReport(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_SEARCH_NODES',
        function () {
            const action = {
                type: actions.RECEIVE_SEARCH_NODES,
                nodes: ['newNode']
            };
            const newState = Object.assign({}, initialState, { searchedNodes: ['newNode'] });
            expect(cutOffReport(initialState, action)).toEqual(newState);
        });

    it('should handle action CLEAR_SEARCH_NODES',
        function () {
            const action = {
                type: actions.CLEAR_SEARCH_NODES
            };
            const newState = Object.assign({}, initialState, { searchedNodes: [] });
            expect(cutOffReport(initialState, action)).toEqual(newState);
        });

    it('should handle action SAVE_CUTOFF_REPORT_FILTER',
        function () {
            const action = {
                type: actions.SAVE_CUTOFF_REPORT_FILTER,
                filters: { nodeId: 1 }
            };
            const newState = Object.assign({}, initialState, { filters: { nodeId: 1 } });
            expect(cutOffReport(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_FINAL_TICKET',
        function () {
            const action = {
                type: actions.RECEIVE_FINAL_TICKET,
                ticket: { ticketId: 1 }
            };
            const newState = Object.assign({}, initialState, { ticket: { ticketId: 1 } });
            expect(cutOffReport(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_NONOPERATIONAL_SEGMENT_OWNERSHIP',
        function () {
            const action = {
                type: actions.RECEIVE_NONOPERATIONAL_SEGMENT_OWNERSHIP,
                executionId: 23
            };
            const newState = Object.assign({}, initialState, { selectedCategory: 'y', selectedElement: 'y', searchedNodes: ['node'] });
            expect(cutOffReport(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF',
        function () {
            const action = {
                type: actions.RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF,
                executionId: 23
            };
            const newState = Object.assign({}, initialState, { reportToggler: !initialState.reportToggler, executionId: 23, filters: { executionId: 23 } });
            expect(cutOffReport(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF_STATUS',
        function () {
            const action = {
                type: actions.RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF_STATUS,
                status: 1
            };
            const newState = Object.assign({}, initialState, { receiveStatusToggler: !initialState.receiveStatusToggler, status: 1 });
            expect(cutOffReport(initialState, action)).toEqual(newState);
        });

    it('should handle action REFRESH_STATUS',
        function () {
            const action = {
                type: actions.REFRESH_STATUS
            };
            const newState = Object.assign({}, initialState, { refreshStatusToggler: !initialState.refreshStatusToggler });
            expect(cutOffReport(initialState, action)).toEqual(newState);
        });

    it('should handle action CLEAR_STATUS',
        function () {
            const action = {
                type: actions.CLEAR_STATUS
            };
            const newState = Object.assign({}, initialState, { operationalDataWithoutCutoffStatus: null });
            expect(cutOffReport(initialState, action)).toEqual(newState);
        });

    it('should handle action CUTOFF_REPORT_RESET_FIELDS',
        function () {
            const action = {
                type: actions.CUTOFF_REPORT_RESET_FIELDS
            };
            const newState = Object.assign({},
                initialState,
                {
                    selectedCategory: null,
                    selectedElement: null,
                    searchedNodes: []
                });
            expect(cutOffReport(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_OWNERSHIP_NODE_DETAILS',
        function () {
            dateService.parse = jest.fn(() => '2020-06-14');
            const action = {
                type: actions.RECEIVE_OWNERSHIP_NODE_DETAILS,
                node: {}
            };
            const newState = Object.assign({}, initialState, {
                filters: {
                    categoryName: action.node.categoryName,
                    elementName: action.node.segment,
                    nodeName: action.node.nodeName,
                    initialDate: '2020-06-14',
                    finalDate: '2020-06-14',
                    reportType: constants.Report.WithOwner
                }
            });
            expect(cutOffReport(initialState, action)).toEqual(newState);
        });

    it('should return default state',
        function () {
            const action = {
                type: 'dummy Value'
            };
            const newState = Object.assign({}, initialState);
            expect(cutOffReport(initialState, action)).toEqual(newState);
        });
});

