import * as actions from '../../../../modules/report/nodeConfiguration/actions';

it('should save node configuration filter', () => {
    const NODE_CONFIGURATION_REPORT_SAVE_REPORT_FILTER = 'NODE_CONFIGURATION_REPORT_SAVE_REPORT_FILTER';
    const filters = { nodeId: 84, name: 'CASTILLA' };
    const action = actions.nodeConfigurationReportSaveReportFilter(filters);

    expect(action.type).toEqual(NODE_CONFIGURATION_REPORT_SAVE_REPORT_FILTER);
    expect(action.filters).toEqual(filters);
});
