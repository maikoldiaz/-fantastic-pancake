import { apiService } from '../../../common/services/apiService';

export const EVENT_CONTRACT_REPORT_REQUEST_FINAL_TICKET = 'EVENT_CONTRACT_REPORT_REQUEST_FINAL_TICKET';
export const EVENT_CONTRACT_REPORT_RECEIVE_FINAL_TICKET = 'EVENT_CONTRACT_REPORT_RECEIVE_FINAL_TICKET';

export const eventContractReportReceiveFinalTicket = json => {
    let ticket = null;
    if (json.value.length) {
        ticket = json.value[0];
        if (!ticket.endDate) {
            ticket.endDate = ticket.ownershipTicket ? ticket.ownershipTicket.endDate : ticket.unbalanceTicket.endDate;
        }
    }
    return {
        type: EVENT_CONTRACT_REPORT_RECEIVE_FINAL_TICKET,
        ticket: ticket
    };
};

export const eventContractReportRequestFinalTicket = (elementId, ticketTypeId) => {
    return {
        type: EVENT_CONTRACT_REPORT_REQUEST_FINAL_TICKET,
        fetchConfig: {
            path: apiService.ticket.getFinalSegmentTicket(elementId, ticketTypeId),
            success: eventContractReportReceiveFinalTicket
        }
    };
};

export const EVENT_CONTRACT_REPORT_SAVE_REPORT_FILTER = 'EVENT_CONTRACT_REPORT_SAVE_REPORT_FILTER';
export const eventContractReportSaveReportFilter = filters => {
    return {
        type: EVENT_CONTRACT_REPORT_SAVE_REPORT_FILTER,
        filters
    };
};
