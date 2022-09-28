import { apiService } from '../../../common/services/apiService';

export const BALANCE_CONTROL_REQUEST_FINAL_TICKET = 'BALANCE_CONTROL_REQUEST_FINAL_TICKET';
export const BALANCE_CONTROL_RECEIVE_FINAL_TICKET = 'BALANCE_CONTROL_RECEIVE_FINAL_TICKET';

export const balanceControlReceiveFinalTicket = json => {
    let ticket = null;
    if (json.value.length) {
        ticket = json.value[0];
        if (!ticket.endDate) {
            ticket.endDate = ticket.ownershipTicket ? ticket.ownershipTicket.endDate : ticket.unbalanceTicket.endDate;
        }
    }
    return {
        type: BALANCE_CONTROL_RECEIVE_FINAL_TICKET,
        ticket: ticket
    };
};

export const balanceControlRequestFinalTicket = (elementId, ticketTypeId) => {
    return {
        type: BALANCE_CONTROL_REQUEST_FINAL_TICKET,
        fetchConfig: {
            path: apiService.ticket.getFinalSegmentTicket(elementId, ticketTypeId),
            success: balanceControlReceiveFinalTicket
        }
    };
};

export const BALANCE_CONTROL_SAVE_REPORT_FILTER = 'BALANCE_CONTROL_SAVE_REPORT_FILTER';
export const balanceControlSaveReportFilter = filters => {
    return {
        type: BALANCE_CONTROL_SAVE_REPORT_FILTER,
        filters
    };
};

