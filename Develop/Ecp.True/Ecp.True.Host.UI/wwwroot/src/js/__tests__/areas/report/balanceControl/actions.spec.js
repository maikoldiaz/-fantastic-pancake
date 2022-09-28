import * as actions from '../../../../modules/report/balanceControl/actions';

it('should save balance control chart', () => {
    const BALANCE_CONTROL_SAVE_REPORT_FILTER = 'BALANCE_CONTROL_SAVE_REPORT_FILTER';
    const filters = { nodeId: 84, name: 'CASTILLA' };
    const action = actions.balanceControlSaveReportFilter(filters);

    expect(action.type).toEqual(BALANCE_CONTROL_SAVE_REPORT_FILTER);
    expect(action.filters).toEqual(filters);
});

it('should receive final ticket report', () => {
    const BALANCE_CONTROL_RECEIVE_FINAL_TICKET = 'BALANCE_CONTROL_RECEIVE_FINAL_TICKET';
    const ticketJson = {
        value: [{
            ownershipTicket: {
                endDate: '2017-02-14'
            }
        }]
    };
    const ticket = ticketJson.value[0];
    ticket.endDate = '2017-02-14';
    const action = actions.balanceControlReceiveFinalTicket(ticketJson);
    expect(action.type).toEqual(BALANCE_CONTROL_RECEIVE_FINAL_TICKET);
    expect(action.ticket).toEqual(ticket);
});

it('should request final ticket report', () => {
    const BALANCE_CONTROL_REQUEST_FINAL_TICKET = 'BALANCE_CONTROL_REQUEST_FINAL_TICKET';
    const elementId = 1;
    const ticketTypeId = 2;
    const action = actions.balanceControlRequestFinalTicket(elementId, ticketTypeId);
    expect(action.type).toEqual(BALANCE_CONTROL_REQUEST_FINAL_TICKET);
});

it('should receive final ticket conditional clause', () => {
    const BALANCE_CONTROL_RECEIVE_FINAL_TICKET = 'BALANCE_CONTROL_RECEIVE_FINAL_TICKET';
    const ticketJson = {
        value: [{
            ownershipTicket: {
                startDate: '2017-02-14',
                endDate: '2017-02-15'
            }
        }]
    };
    const action = actions.balanceControlReceiveFinalTicket(ticketJson);
    expect(action.type).toEqual(BALANCE_CONTROL_RECEIVE_FINAL_TICKET);
    expect(action.ticket).toEqual(ticketJson.value[0]);
});
