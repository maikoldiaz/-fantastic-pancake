import { apiService } from '../../../common/services/apiService';

export const NODE_STATUS_REPORT_REQUEST_FINAL_TICKET = 'NODE_STATUS_REPORT_REQUEST_FINAL_TICKET';
export const NODE_STATUS_REPORT_RECEIVE_FINAL_STARTDATE_TICKET = 'NODE_STATUS_REPORT_RECEIVE_FINAL_STARTDATE_TICKET';
export const NODE_STATUS_REPORT_RECEIVE_FINAL_ENDDATE_TICKET = 'NODE_STATUS_REPORT_RECEIVE_FINAL_ENDDATE_TICKET';

export const nodeStatusReportReceiveFinalStartDateTicket = json => {
    let ticket = null;
    if (json.value.length) {
        ticket = json.value[0];
    }
    return {
        type: NODE_STATUS_REPORT_RECEIVE_FINAL_STARTDATE_TICKET,
        ticket: ticket
    };
};

export const nodeStatusReportReceiveFinalEndDateTicket = json => {
    let ticket = null;
    if (json.value.length) {
        ticket = json.value[0];
    }
    return {
        type: NODE_STATUS_REPORT_RECEIVE_FINAL_ENDDATE_TICKET,
        ticket: ticket
    };
};

export const nodeStatusReportRequestFinalTicket = (elementId, start) => {
    return {
        type: NODE_STATUS_REPORT_REQUEST_FINAL_TICKET,
        fetchConfig: {
            path: apiService.ticket.getFinalSegmentTicket(elementId, 'Ownership', start),
            success: start ? nodeStatusReportReceiveFinalStartDateTicket : nodeStatusReportReceiveFinalEndDateTicket
        }
    };
};

export const NODE_STATUS_REPORT_SAVE_REPORT_FILTER = 'NODE_STATUS_REPORT_SAVE_REPORT_FILTER';
export const nodeStatusReportSaveReportFilter = filters => {
    return {
        type: NODE_STATUS_REPORT_SAVE_REPORT_FILTER,
        filters
    };
};
