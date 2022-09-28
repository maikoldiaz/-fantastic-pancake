import * as actions from '../../../../modules/report/eventContract/actions';
import { eventContractReport } from '../../../../modules/report/eventContract/reducers';

describe('Reducers for eventContractReport', () => {
    const initialState = {
        selectedCategory: 'y',
        selectedElement: 'y',
        searchedNodes: ['node'],
        filters: {}
    };
    it('should handle action EVENT_CONTRACT_REPORT_SAVE_REPORT_FILTER',
        function () {
            const action = {
                type: actions.EVENT_CONTRACT_REPORT_SAVE_REPORT_FILTER,
                filters: { nodeId: 1 }
            };
            const newState = Object.assign({}, initialState, { filters: { nodeId: 1 } });
            expect(eventContractReport(initialState, action)).toEqual(newState);
        });

    it('should handle action EVENT_CONTRACT_REPORT_RECEIVE_FINAL_TICKET',
        function () {
            const action = {
                type: actions.EVENT_CONTRACT_REPORT_RECEIVE_FINAL_TICKET,
                ticket: { ticketId: 1 }
            };
            const newState = Object.assign({}, initialState, { ticket: { ticketId: 1 } });
            expect(eventContractReport(initialState, action)).toEqual(newState);
        });

    it('should return default state',
        function () {
            const action = {
                type: 'dummy Value'
            };
            const newState = Object.assign({}, initialState);
            expect(eventContractReport(initialState, action)).toEqual(newState);
        });
});
