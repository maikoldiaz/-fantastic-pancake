import * as actions from '../../../../modules/report/nodeStatus/actions';

it('should save node status filter', () => {
    const NODE_STATUS_REPORT_SAVE_REPORT_FILTER = 'NODE_STATUS_REPORT_SAVE_REPORT_FILTER';
    const filters = { nodeId: 84, name: 'CASTILLA' };
    const action = actions.nodeStatusReportSaveReportFilter(filters);

    expect(action.type).toEqual(NODE_STATUS_REPORT_SAVE_REPORT_FILTER);
    expect(action.filters).toEqual(filters);
});
it('should request final startdate ticket', () => {
    const NODE_STATUS_REPORT_REQUEST_FINAL_TICKET = 'NODE_STATUS_REPORT_REQUEST_FINAL_TICKET';
    const action = actions.nodeStatusReportRequestFinalTicket(1, true);
    expect(action.type).toEqual(NODE_STATUS_REPORT_REQUEST_FINAL_TICKET);
});
it('should request final enddate ticket', () => {
    const NODE_STATUS_REPORT_REQUEST_FINAL_TICKET = 'NODE_STATUS_REPORT_REQUEST_FINAL_TICKET';
    const action = actions.nodeStatusReportRequestFinalTicket(1, false);
    expect(action.type).toEqual(NODE_STATUS_REPORT_REQUEST_FINAL_TICKET);
});
it('should receive final startdate ticket', () => {
    const NODE_STATUS_REPORT_RECEIVE_FINAL_STARTDATE_TICKET = 'NODE_STATUS_REPORT_RECEIVE_FINAL_STARTDATE_TICKET';
    const ticketJson = {
        value: [{
            ownershipTicket: {
                startDate: '2017-02-14'
            }
        }]
    };
    const action = actions.nodeStatusReportReceiveFinalStartDateTicket(ticketJson);
    expect(action.type).toEqual(NODE_STATUS_REPORT_RECEIVE_FINAL_STARTDATE_TICKET);
    expect(action.ticket).toEqual(ticketJson.value[0]);
});
it('should receive final enddate ticket', () => {
    const NODE_STATUS_REPORT_RECEIVE_FINAL_ENDDATE_TICKET = 'NODE_STATUS_REPORT_RECEIVE_FINAL_ENDDATE_TICKET';
    const ticketJson = {
        value: [{
            ownershipTicket: {
                endDate: '2017-02-14'
            }
        }]
    };
    const action = actions.nodeStatusReportReceiveFinalEndDateTicket(ticketJson);
    expect(action.type).toEqual(NODE_STATUS_REPORT_RECEIVE_FINAL_ENDDATE_TICKET);
    expect(action.ticket).toEqual(ticketJson.value[0]);
});
