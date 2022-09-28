import * as actions from '../../../modules/transportBalance/cutOff/actions';
import { ticketInfo } from '../../../modules/transportBalance/cutOff/reducers';
describe('Reducers for Ticket Chart', () => {
    const initialState = {
        ticket: {}, total: [], processed: [], generated: []
    };

    it('should receive ticket info detail',
        function () {
            const action = {
                ticket: { ticket: { ticketId: 1000 }, movements: [], inventories: [], generatedMovements: [] },
                type: actions.RECEIVE_TICKET_INFO_DETAILS
            };
            const newState = Object.assign({}, initialState, { ticket: {}, total: [], processed: [], generated: [] });

            const expectedState = ticketInfo(initialState, action);
            expect(expectedState).toEqual(newState);
        });

    it('should not change receive ticket info detail',
        function () {
            const action = {
                ticket: { ticketId: 1000 },
                type: 'raw_value'
            };
            expect(ticketInfo(initialState, action)).toEqual(initialState);
        });

    it('should reset ticket info detail',
        function () {
            const action = {
                type: 'RESET_TICKET_INFO_DETAILS'
            };

            expect(ticketInfo(initialState, action)).toEqual(initialState);
        });

    it('should handle default',
        function () {
            const action = {
                type: 'Default'
            };

            expect(ticketInfo(initialState, action)).toEqual(initialState);
        });
});
