import * as actions from '../../../../modules/report/settingsAudit/actions';

it('should save setting audit report', () => {
    const SETTINGS_AUDIT_REPORT_SAVE_FILTER = 'SETTINGS_AUDIT_REPORT_SAVE_FILTER';
    const stateValues = { nodeId: 84, name: 'CASTILLA' };
    const action = actions.settingsAuditReportSaveReportFilter(stateValues);

    expect(action.type).toEqual(SETTINGS_AUDIT_REPORT_SAVE_FILTER);
    expect(action.filters).toEqual(stateValues);
});

it('should reset setting audit report', () => {
    const SETTINGS_AUDIT_REPORT_RESET_FILTER = 'SETTINGS_AUDIT_REPORT_RESET_FILTER';
    const action = actions.settingsAuditReportResetReportFilter();

    expect(action.type).toEqual(SETTINGS_AUDIT_REPORT_RESET_FILTER);
});

it('should set setting audit report back navigation', () => {
    const SET_BACK_NAVIGATION_SETTINGS_AUDIT = 'SET_BACK_NAVIGATION_SETTINGS_AUDIT';
    const action = actions.setBackNavigation();

    expect(action.type).toEqual(SET_BACK_NAVIGATION_SETTINGS_AUDIT);
});

it('should reset setting audit report back navigation', () => {
    const RESET_BACK_NAVIGATION_SETTINGS_AUDIT = 'RESET_BACK_NAVIGATION_SETTINGS_AUDIT';
    const action = actions.resetBackNavigation();

    expect(action.type).toEqual(RESET_BACK_NAVIGATION_SETTINGS_AUDIT);
});
