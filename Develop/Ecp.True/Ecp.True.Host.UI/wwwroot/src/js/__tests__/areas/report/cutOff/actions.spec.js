import * as actions from '../../../../modules/report/cutOff/actions';
import { apiService } from '../../../../common/services/apiService';
import { systemConfigService } from '../../../../common/services/systemConfigService';

it('should get selected category', () => {
    const CUTOFF_REPORT_FILTER_ON_SELECT_CATEGORY = 'CUTOFF_REPORT_FILTER_ON_SELECT_CATEGORY';
    const selectedCategory = 'category1';
    const action = actions.cutOffReportFilterOnSelectCategory(selectedCategory);
    expect(action.type).toEqual(CUTOFF_REPORT_FILTER_ON_SELECT_CATEGORY);
    expect(action.selectedCategory).toEqual(selectedCategory);
});

it('should get selected element', () => {
    const CUTOFF_REPORT_FILTER_ON_SELECT_ELEMENT = 'CUTOFF_REPORT_FILTER_ON_SELECT_ELEMENT';
    const selectedElement = 'Campo';
    const action = actions.cutOffReportFilterOnSelectElement(selectedElement);
    expect(action.type).toEqual(CUTOFF_REPORT_FILTER_ON_SELECT_ELEMENT);
    expect(action.selectedElement).toEqual(selectedElement);
});

it('should save cutoff report filter', () => {
    const SAVE_CUTOFF_REPORT_FILTER = 'SAVE_CUTOFF_REPORT_FILTER';
    const filters = { nodeId: 84, name: 'CASTILLA' };
    const action = actions.saveCutOffReportFilter(filters);
    expect(action.type).toEqual(SAVE_CUTOFF_REPORT_FILTER);
    expect(action.filters).toEqual(filters);
});

it('should request search nodes', () => {
    const elementId = 1;
    const searchText = 'CASTILLA';
    const REQUEST_SEARCH_NODES = 'REQUEST_SEARCH_NODES';
    systemConfigService.getAutocompleteItemsCount = jest.fn(() => 5);
    const action = actions.requestSearchNodes(elementId, searchText);
    expect(action.type).toEqual(REQUEST_SEARCH_NODES);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.node.searchNodeTags(elementId, searchText));
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const RECEIVE_SEARCH_NODES = 'RECEIVE_SEARCH_NODES';
    expect(receiveAction.type).toEqual(RECEIVE_SEARCH_NODES);
});

it('should receive operational data without cutoff', () => {
    const action = actions.receiveOperationalDataWithoutCutOff({});
    expect(action.type).toEqual('RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF');
});

it('should request operational data without cutoff', () => {
    const REQUEST_OPERATIONAL_DATA_WITHOUT_CUTOFF = 'REQUEST_OPERATIONAL_DATA_WITHOUT_CUTOFF';
    const action = actions.requestOperationalDataWithoutCutOff();
    expect(action.type).toEqual(REQUEST_OPERATIONAL_DATA_WITHOUT_CUTOFF);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.ticket.postOperationalDataWithoutCutOff());
    expect(action.fetchConfig.success).toBeDefined();
});

it('should request non operational segment ownership', () => {
    const REQUEST_NONOPERATIONAL_SEGMENT_OWNERSHIP = 'REQUEST_NONOPERATIONAL_SEGMENT_OWNERSHIP';
    const action = actions.requestNonOperationalSegmentOwnership();
    expect(action.type).toEqual(REQUEST_NONOPERATIONAL_SEGMENT_OWNERSHIP);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.ticket.postNonOperationalSegmentOwnership());
    expect(action.fetchConfig.success).toBeDefined();
});

it('should receive operational data without cutoff status', () => {
    const action = actions.receiveOperationalDataWithoutCutOffStatus({});
    expect(action.type).toEqual('RECEIVE_OPERATIONAL_DATA_WITHOUT_CUTOFF_STATUS');
});

it('should request operational data without cutoff status', () => {
    const executionId = 1;
    const reportType = 1;
    const REQUEST_OPERATIONAL_DATA_WITHOUT_CUTOFF_STATUS = 'REQUEST_OPERATIONAL_DATA_WITHOUT_CUTOFF_STATUS';
    const action = actions.requestOperationalDataWithoutCutoffStatus(executionId, reportType);
    expect(action.type).toEqual(REQUEST_OPERATIONAL_DATA_WITHOUT_CUTOFF_STATUS);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.ticket.requestReportExecutionStatus(executionId, reportType));
    expect(action.fetchConfig.success).toBeDefined();
});

it('should refresh status', () => {
    const action = actions.refreshStatus({});
    expect(action.type).toEqual('REFRESH_STATUS');
});

it('should clear status', () => {
    const action = actions.clearStatus({});
    expect(action.type).toEqual('CLEAR_STATUS');
});

it('should receive search nodes', () => {
    const action = actions.receiveSearchNodes({});
    expect(action.type).toEqual('RECEIVE_SEARCH_NODES');
    expect(action.nodes).toEqual([]);
});

it('should receive search nodes', () => {
    const node = {
        value: [{
            node: 1
        }]
    };
    const output = [node.value[0].node];
    const action = actions.receiveSearchNodes(node);
    expect(action.type).toEqual('RECEIVE_SEARCH_NODES');
    expect(action.nodes).toEqual(output);
});

it('should clear search nodes', () => {
    const action = actions.clearSearchNodes({});
    expect(action.type).toEqual('CLEAR_SEARCH_NODES');
});

it('should reset cutoff report filter', () => {
    const action = actions.cutOffReportResetFields({});
    expect(action.type).toEqual('CUTOFF_REPORT_RESET_FIELDS');
});

it('should receive final ticket report for ownership', () => {
    const RECEIVE_FINAL_TICKET = 'RECEIVE_FINAL_TICKET';
    const ticketJson = {
        value: [{
            ownershipTicket: {
                endDate: '2017-02-14'
            }
        }]
    };
    const ticket = ticketJson.value[0];
    ticket.endDate = '2017-02-14';
    const action = actions.receiveFinalTicket(ticketJson);
    expect(action.type).toEqual(RECEIVE_FINAL_TICKET);
    expect(action.ticket).toEqual(ticket);
});

it('should receive final ticket report for unbalancedticket', () => {
    const RECEIVE_FINAL_TICKET = 'RECEIVE_FINAL_TICKET';
    const ticketJson = {
        value: [{
            unbalanceTicket: {
                endDate: '2017-02-14'
            }
        }]
    };
    const ticket = ticketJson.value[0];
    const action = actions.receiveFinalTicket(ticketJson);
    expect(action.type).toEqual(RECEIVE_FINAL_TICKET);
    expect(action.ticket).toEqual(ticket);
});

it('should receive final ticket report for empty ticket', () => {
    const RECEIVE_FINAL_TICKET = 'RECEIVE_FINAL_TICKET';
    const ticketJson = {
        value: []
    };
    const ticket = null;
    const action = actions.receiveFinalTicket(ticketJson);
    expect(action.type).toEqual(RECEIVE_FINAL_TICKET);
    expect(action.ticket).toEqual(ticket);
});


it('should receive final ticket report without date', () => {
    const RECEIVE_FINAL_TICKET = 'RECEIVE_FINAL_TICKET';
    const ticketJson = {
        value: [{
            ownershipTicket: {
                endDate: '2017-02-14'
            }
        }]
    };
    const ticket = ticketJson.value[0];
    const action = actions.receiveFinalTicket(ticketJson);
    expect(action.type).toEqual(RECEIVE_FINAL_TICKET);
    ticket.endDate = '2017-02-14';
    expect(action.ticket).toEqual(ticket);
});

it('should request final ticket for systemticket', () => {
    const categoryId = 8;
    const elementId = 1;
    const ticketTypeId = 1;
    const REQUEST_FINAL_TICKET = 'REQUEST_FINAL_TICKET';
    const action = actions.requestFinalTicket(categoryId, elementId, ticketTypeId);
    expect(action.type).toEqual(REQUEST_FINAL_TICKET);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.ticket.getFinalSystemTicket(elementId, ticketTypeId));
    expect(action.fetchConfig.success).toBeDefined();
});

it('should request final ticket for segmentticket', () => {
    const categoryId = 9;
    const elementId = 1;
    const ticketTypeId = 1;
    const REQUEST_FINAL_TICKET = 'REQUEST_FINAL_TICKET';
    const action = actions.requestFinalTicket(categoryId, elementId, ticketTypeId);
    expect(action.type).toEqual(REQUEST_FINAL_TICKET);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.ticket.getFinalSegmentTicket(elementId, ticketTypeId));
    expect(action.fetchConfig.success).toBeDefined();
});


it('should receive ownership node', () => {
    const RECEIVE_OWNERSHIP_NODE_DETAILS = 'RECEIVE_OWNERSHIP_NODE_DETAILS';
    const ownershipNodes = {
        value: [{ ownershipNodeId: 1 }]
    };
    const action = actions.receiveOwnershipNode(ownershipNodes);
    expect(action.type).toEqual(RECEIVE_OWNERSHIP_NODE_DETAILS);
    expect(action.node).toEqual(ownershipNodes.value[0]);
});

it('should receive ownership node', () => {
    const RECEIVE_OWNERSHIP_NODE_DETAILS = 'RECEIVE_OWNERSHIP_NODE_DETAILS';
    const ownershipNodes = {
        value: []
    };
    const action = actions.receiveOwnershipNode(ownershipNodes);
    expect(action.type).toEqual(RECEIVE_OWNERSHIP_NODE_DETAILS);
    expect(action.node).toEqual({});
});

it('should request search nodes', () => {
    const REQUEST_OWNERSHIP_NODE_DETAILS = 'REQUEST_OWNERSHIP_NODE_DETAILS';
    const ownershipNodes = {
        value: [{ ownershipNodeId: 1 }]
    };
    const nodeId = 1;
    const action = actions.requestOwnershipNode(nodeId);
    expect(action.type).toEqual(REQUEST_OWNERSHIP_NODE_DETAILS);
    expect(action.fetchConfig).toBeDefined();
    expect(action.fetchConfig.path).toEqual(apiService.ownershipNode.queryByOwnershipNodeId(nodeId));
    expect(action.fetchConfig.success).toBeDefined();
});


