import * as actions from '../../../modules/transportBalance/cutOff/actions';
import { apiService } from '../../../common/services/apiService.js';

describe('ticket chart actions', () => {
    it('should request ticket details', () => {
        const REQUEST_TICKET_DETAILS = 'REQUEST_TICKET_DETAILS';
        const ticketId = 1000;
        const ticketObject = { ticketId: 1000 };
        const action = actions.requestTicketDetails(ticketId);

        expect(action.type).toEqual(REQUEST_TICKET_DETAILS);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ticket.getTicketInformation(ticketId));
        expect(action.fetchConfig.success).toBeDefined();
        const receiveAction = action.fetchConfig.success(ticketObject);
        expect(receiveAction.type).toEqual(actions.RECEIVE_TICKET_DETAILS);
        expect(receiveAction.ticket).toEqual(ticketObject);
    });

    it('should receive ticket details', () => {
        const RECEIVE_TICKET_DETAILS = 'RECEIVE_TICKET_DETAILS';
        const ticketObject = { ticketId: 1000 };
        const action = actions.receiveTicketDetails(ticketObject);

        expect(action.type).toEqual(RECEIVE_TICKET_DETAILS);
        expect(action.ticket).toEqual(ticketObject);
    });

    it('should reset ticket details', () => {
        const RESET_TICKET_DETAILS = 'RESET_TICKET_DETAILS';
        const action = actions.resetTicketDetails();
        expect(action.type).toEqual(RESET_TICKET_DETAILS);
    });

    it('should initialize ticket error', () => {
        const SET_TICKET_ERROR = 'SET_TICKET_ERROR';
        const ticket = { ticketId: 1000 };
        const action = actions.initializeTicketError(ticket);
        expect(action.type).toEqual(SET_TICKET_ERROR);
        expect(action.ticket).toEqual(ticket);
    });
});
