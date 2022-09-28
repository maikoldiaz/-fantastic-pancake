import * as actions from '../../../../modules/report/settingsAudit/actions';
import { settingsAuditReport } from '../../../../modules/report/settingsAudit/reducers';
import { dateService } from '../../../../common/services/dateService';

describe('Reducers for settingsAuditReport', () => {
    const initialState = {
        filters: {}
    };
    it('should handle action SETTINGS_AUDIT_REPORT_SAVE_FILTER',
        function () {
            const action = {
                type: actions.SETTINGS_AUDIT_REPORT_SAVE_FILTER,
                filters: {
                    initialDate: dateService.now(),
                    finalDate: dateService.now(),
                    reportType: '10.10.09AuditoriaDeConfiguraciones09'
                }
            };
            const filters = {
                initialDate: dateService.convertToColombian(action.filters.initialDate),
                finalDate: dateService.convertToColombian(action.filters.finalDate),
                reportType: action.filters.reportType
            };
            const formValues = {
                initialDate: action.filters.initialDate,
                finalDate: action.filters.finalDate
            };
            const newState = Object.assign({}, initialState, { filters, formValues });
            expect(settingsAuditReport(initialState, action)).toEqual(newState);
        });
    it('should handle action SETTINGS_AUDIT_REPORT_RESET_FILTER',
        function () {
            const action = {
                type: actions.SETTINGS_AUDIT_REPORT_RESET_FILTER,
                filters: { initialDate: null, finalDate: null, reportType: null },
                initialValues: { initialDate: null, finalDate: null }
            };
            const newState = Object.assign({}, initialState, { filters: { initialDate: null, finalDate: null, reportType: null },
                initialValues: { initialDate: null, finalDate: null } });
            expect(settingsAuditReport(initialState, action)).toEqual(newState);
        });

    it('should handle action SET_BACK_NAVIGATION_SETTINGS_AUDIT',
        function () {
            const action = {
                type: actions.SET_BACK_NAVIGATION_SETTINGS_AUDIT
            };
            const newState = Object.assign({}, initialState, { backNavigation: true, initialValues: initialState.formValues });
            expect(settingsAuditReport(initialState, action)).toEqual(newState);
        });

    it('should handle action RESET_BACK_NAVIGATION_SETTINGS_AUDIT',
        function () {
            const action = {
                type: actions.RESET_BACK_NAVIGATION_SETTINGS_AUDIT
            };
            const newState = Object.assign({}, initialState, { backNavigation: false });
            expect(settingsAuditReport(initialState, action)).toEqual(newState);
        });

    it('should return default state',
        function () {
            const action = {
                type: 'dummy Value'
            };
            const newState = Object.assign({}, initialState);
            expect(settingsAuditReport(initialState, action)).toEqual(newState);
        });
});
