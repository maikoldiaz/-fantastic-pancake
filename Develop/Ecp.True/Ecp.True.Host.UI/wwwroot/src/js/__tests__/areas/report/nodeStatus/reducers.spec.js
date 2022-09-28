import * as actions from '../../../../modules/report/nodeStatus/actions';
import { nodeStatusReport } from '../../../../modules/report/nodeStatus/reducers';

describe('Reducers for nodeStatusReport', () => {
    const initialState = {
        filterSettings: {},
        ticket: null
    };
    it('should handle action NODE_STATUS_REPORT_SAVE_REPORT_FILTER',
        function () {
            const action = {
                type: actions.NODE_STATUS_REPORT_SAVE_REPORT_FILTER,
                filters: { nodeName: 'Todos' }
            };
            const newState = Object.assign({}, initialState, { filters: { nodeName: 'Todos' } });
            expect(nodeStatusReport(initialState, action)).toEqual(newState);
        }
    );
    it('should handle action NODE_STATUS_REPORT_RECEIVE_FINAL_STARTDATE_TICKET',
        function () {
            const action = {
                type: actions.NODE_STATUS_REPORT_RECEIVE_FINAL_STARTDATE_TICKET,
                ticket: { ticketId: 1 }
            };
            const newState = Object.assign({}, initialState, { startDateTicket: { ticketId: 1 } });
            expect(nodeStatusReport(initialState, action)).toEqual(newState);
        }
    );
    it('should handle action NODE_STATUS_REPORT_RECEIVE_FINAL_ENDDATE_TICKET',
        function () {
            const action = {
                type: actions.NODE_STATUS_REPORT_RECEIVE_FINAL_ENDDATE_TICKET,
                ticket: { ticketId: 1 }
            };
            const newState = Object.assign({}, initialState, { endDateTicket: { ticketId: 1 } });
            expect(nodeStatusReport(initialState, action)).toEqual(newState);
        }
    );

    it('should return default state',
        function () {
            const action = {
                type: 'dummy Value'
            };
            const newState = Object.assign({}, initialState);
            expect(nodeStatusReport(initialState, action)).toEqual(newState);
        });
});
