import * as actions from '../../../../modules/report/balanceControl/actions';
import { balanceControlChart } from '../../../../modules/report/balanceControl/reducers';

describe('Reducers for balanceControlChart', () => {
    const initialState = {
        selectedCategory: 'y',
        selectedElement: 'y',
        searchedNodes: ['node'],
        filters: {}
    };
    it('should handle action BALANCE_CONTROL_SAVE_REPORT_FILTER',
        function () {
            const action = {
                type: actions.BALANCE_CONTROL_SAVE_REPORT_FILTER,
                filters: { nodeId: 1 }
            };
            const newState = Object.assign({}, initialState, { filters: { nodeId: 1 } });
            expect(balanceControlChart(initialState, action)).toEqual(newState);
        });

    it('should handle action BALANCE_CONTROL_RECEIVE_FINAL_TICKET',
        function () {
            const action = {
                type: actions.BALANCE_CONTROL_RECEIVE_FINAL_TICKET,
                ticket: { ticketId: 1 }
            };
            const newState = Object.assign({}, initialState, { ticket: { ticketId: 1 } });
            expect(balanceControlChart(initialState, action)).toEqual(newState);
        });

    it('should return default state',
        function () {
            const action = {
                type: 'dummy Value'
            };
            const newState = Object.assign({}, initialState);
            expect(balanceControlChart(initialState, action)).toEqual(newState);
        });
});
