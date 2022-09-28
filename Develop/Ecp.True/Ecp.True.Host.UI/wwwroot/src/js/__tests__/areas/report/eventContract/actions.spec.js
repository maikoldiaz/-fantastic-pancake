import * as actions from '../../../../modules/report/eventContract/actions';
import { apiService } from '../../../../common/services/apiService.js';

it('should save balance control chart', () => {
    const EVENT_CONTRACT_REPORT_RECEIVE_FINAL_TICKET = 'EVENT_CONTRACT_REPORT_REQUEST_FINAL_TICKET';
    const action = actions.eventContractReportRequestFinalTicket(123, 234);
    expect(action.type).toEqual(EVENT_CONTRACT_REPORT_RECEIVE_FINAL_TICKET);
});

it('should save balance control chart', () => {
    const EVENT_CONTRACT_REPORT_SAVE_REPORT_FILTER = 'EVENT_CONTRACT_REPORT_SAVE_REPORT_FILTER';
    const filters = { nodeId: 84, name: 'CASTILLA' };
    const action = actions.eventContractReportSaveReportFilter(filters);

    expect(action.type).toEqual(EVENT_CONTRACT_REPORT_SAVE_REPORT_FILTER);
    expect(action.filters).toEqual(filters);
});

it('should get final ticket report', () => {
    const eventContractReportRequestFinalTicket = 'EVENT_CONTRACT_REPORT_REQUEST_FINAL_TICKET';
    const elementId = 'someId';
    const ticketId = 'someTicketId';
    const ticketJson = {
        value: {
            ownershipTicket: {
                endDate: '2017-02-14'
            }
        }
    };
    const apiServiceMock = apiService.ticket.getFinalSegmentTicket = jest.fn(() => {
        return ticketJson;
    });
    const action = actions.eventContractReportRequestFinalTicket(elementId, ticketId);
    expect(action.type).toEqual(eventContractReportRequestFinalTicket);
    expect(apiServiceMock.mock.calls).toHaveLength(1);
});

it('should receive final ticket report', () => {
    const eventContractReportReceiveFinalTicket = 'EVENT_CONTRACT_REPORT_RECEIVE_FINAL_TICKET';
    const ticketJson = {
        value: [{
            ownershipTicket: {
                endDate: '2017-02-14'
            }
        }]
    };
    const ticket = ticketJson.value[0];
    ticket.endDate = '2017-02-14';
    const action = actions.eventContractReportReceiveFinalTicket(ticketJson);
    expect(action.type).toEqual(eventContractReportReceiveFinalTicket);
    expect(action.ticket).toEqual(ticket);
});

it('should receive final ticket conditional clause', () => {
    const EVENT_CONTRACT_REPORT_RECEIVE_FINAL_TICKET = 'EVENT_CONTRACT_REPORT_RECEIVE_FINAL_TICKET';
    const ticketJson = {
        value: [{
            ownershipTicket: {
                startDate: '2017-02-14',
                endDate: '2017-02-15'
            }
        }]
    };
    const endDate = '2017-02-15';
    const action = actions.eventContractReportReceiveFinalTicket(ticketJson);
    expect(action.type).toEqual(EVENT_CONTRACT_REPORT_RECEIVE_FINAL_TICKET);
    expect(action.ticket.endDate).toEqual(endDate);
});
