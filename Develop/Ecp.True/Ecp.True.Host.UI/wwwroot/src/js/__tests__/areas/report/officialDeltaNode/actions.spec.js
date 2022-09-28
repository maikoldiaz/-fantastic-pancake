import * as actions from '../../../../modules/report/officialDeltaNode/actions';
import { apiService } from '../../../../common/services/apiService';

it('should save official node balance', () => {
    const OFFICIAL_DELTA_SAVE_FILTER = 'OFFICIAL_DELTA_SAVE_FILTER';
    const data = 'Data';
    const action = actions.saveOfficialDeltaFilter(data);
    expect(action.type).toEqual(OFFICIAL_DELTA_SAVE_FILTER);
    expect(action.data).toEqual('Data');
});

it('should select segment', () => {
    const OFFICIAL_DELTA_SELECT_ELEMENT = 'OFFICIAL_DELTA_SELECT_ELEMENT';
    const element = 'element';
    const action = actions.onSelectedElement(element);
    expect(action.type).toEqual(OFFICIAL_DELTA_SELECT_ELEMENT);
    expect(action.element).toEqual('element');
});

it('should reset official node balance report filters', () => {
    const RESET_OFFICIAL_DELTA_FILTER = 'RESET_OFFICIAL_DELTA_FILTER';
    const action = actions.resetBalancePerNodeFilter();
    expect(action.type).toEqual(RESET_OFFICIAL_DELTA_FILTER);
});

it('receive submit for approval', () => {
    const action = actions.receiveSubmitForApproval({});
    expect(action.type).toEqual('RECEIVE_SUBMIT_FOR_APPROVAL');
});

it('request submit for approval', () => {
    const action = actions.submitForApproval({});
    expect(action.type).toEqual('REQUEST_SUBMIT_FOR_APPROVAL');
});

it('receive submit for reopen', () => {
    const action = actions.receiveSubmitForReopen({});
    expect(action.type).toEqual('RECEIVE_SUBMIT_FOR_REOPEN');
});

it('receive reopen', () => {
    const action = actions.receiveReopen({});
    expect(action.type).toEqual('RECEIVE_REOPEN');
});

it('request submit for reopen', () => {
    const action = actions.submitForReopen({});
    expect(action.type).toEqual('REQUEST_SUBMIT_FOR_REOPEN');
});

it('submit reopen', () => {
    const action = actions.reopenDeltaNode({});
    expect(action.type).toEqual('SUBMIT_REOPEN');
});

it('submission type', () => {
    const action = actions.setSubmissionType({});
    expect(action.type).toEqual('SUBMISSION_TYPE');
});

it('set report toggler', () => {
    const action = actions.setReportToggler({});
    expect(action.type).toEqual('SET_REPORT_TOGGLER');
});

it('reset Node Filter', () => {
    const action = actions.resetNodeFilter({});
    expect(action.type).toEqual('RESET_OFFICIAL_DELTA_FILTER');
});

it('reset Flow Node Filter', () => {
    const action = actions.resetFlowNodeFilter({});
    expect(action.type).toEqual('RESET_FLOW_NODE_FILTER');
});

it('reset Flow Report Toggler', () => {
    const action = actions.resetFlowReportToggler({});
    expect(action.type).toEqual('RESET_FLOW_REPORT_TRIGGER');
});

it('receive Delta Node', () => {
    const action = actions.receiveDeltaNode({ value: [] });
    expect(action.type).toEqual('RECEIVE_DELTA_NODE_DETAILS');
    expect(action.node).toEqual({});
});

it('reset Delta Node Source', () => {
    const action = actions.resetDeltaNodeSource({});
    expect(action.type).toEqual('RESET_DELTA_NODE_SOURCE');
    expect(action.source).toEqual({});
});

it('set Flow Trigger Toggler', () => {
    const action = actions.setFlowTriggerToggler(true);
    expect(action.type).toEqual('SET_FLOW_TRIGGER_TOGGLER');
    expect(action.flowReportToggler).toEqual(true);
});

it('request Delta Node', () => {
    const action = actions.requestDeltaNode(1);
    expect(action.fetchConfig.path).toEqual(apiService.officialDelta.queryByDeltaNodeId(1));
    expect(action.fetchConfig.notFound).toEqual(true);
    expect(action.fetchConfig.success).toBeDefined();
});

it('request Delta Node Manual Movements', () => {
    const action = actions.requestDeltaNodeMovements('2020-01-01', '2020-01-31', 1);
    expect(action.fetchConfig.path).toEqual(apiService.officialDelta.queryManualMovementsByDeltaNodeId('2020-01-01', '2020-01-31', 1));
    expect(action.fetchConfig.success).toBeDefined();
})

it('receive Delta Node Manual Movements With Array Empty', () => {
    const action = actions.receiveDeltaNodeMovements({ value: [] });
    expect(action.type).toEqual('RECEIVE_DELTA_NODE_MOVEMENTS');
    expect(action.data).toEqual([]);
});

it('receive Delta Node Manual Movements With Values', () => {
    const value = JSON.parse(`{"movementTransactionId":278702,"ticketId":null,"ownershipTicketId":null,"netStandardVolume":401475.74,"movementSource":{"sourceNode":{"name":"Automation_e2uj7"},"sourceProduct":{"name":"CRUDO CAMPO CUSUCO"}},"movementDestination":{"destinationNode":{"name":"Automation_r0xae"},"destinationProduct":{"name":"GAS GUAJIRA"}},"movementType":{"name":"Automation_mezlx"},"segment":{"name":"Automation_h2k6u"},"measurementUnitElement":{"name":"Bbl","description":"Barriles"},"owners":[{"ownerElement":{"name":"ECOPETROL","description":"ECOPETROL"}}]}`);
    const action = actions.receiveDeltaNodeMovements({ value: [value] });
    expect(action.type).toEqual('RECEIVE_DELTA_NODE_MOVEMENTS');
    expect(action.data).toEqual([value]);
});

it('should be able to save manual movements delta node', () => {
    const action = actions.saveManualMovements(1, [1]);
    expect(action.fetchConfig.path).toEqual(apiService.officialDelta.setManualMovementsByTicketAndConsolidateMovements(1, [1]));
    expect(action.fetchConfig.method).toEqual(`PUT`);
    expect(action.fetchConfig.body).toEqual([1]);
    expect(action.fetchConfig.success).toBeDefined();
})

it('receive OK response of save manual movements delta node', () => {
    const action = actions.receiveSaveManualMovementsOk();
    expect(action.type).toEqual('RECEIVE_SAVE_DELTA_NODE_MOVEMENTS');
    expect(action.status).toEqual(true);
})

it('reset form save manual movements delta node', () => {
    const action = actions.receiveSaveManualMovementsReset();
    expect(action.type).toEqual('RECEIVE_SAVE_DELTA_NODE_MOVEMENTS');
    expect(action.status).toEqual(null);
})

it('receive FAILURE form save manual movements delta node', () => {
    const action = actions.receiveSaveManualMovementsFail();
    expect(action.type).toEqual('RECEIVE_SAVE_DELTA_NODE_MOVEMENTS');
    expect(action.status).toEqual(false);
})