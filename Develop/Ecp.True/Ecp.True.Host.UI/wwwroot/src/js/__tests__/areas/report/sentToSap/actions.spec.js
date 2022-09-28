import * as actions from '../../../../modules/report/sentToSap/actions';
import { apiService } from '../../../../common/services/apiService';

describe('Actions for sentToSap report', () => {
    it('should call BUILD_SEND_TO_SAP_REPORT_EXECUTION_FILTERS action', () => {
        const execution = {
            executionId: 10
        };

        const action = actions.buildReportFilters(execution);
        expect(action.type).toEqual(actions.BUILD_SEND_TO_SAP_REPORT_EXECUTION_FILTERS);
        expect(action.execution).toEqual(execution);
    });

    it('should execute report', () => {
        const REQUEST_SENT_TO_SAP_REPORT = 'REQUEST_SENT_TO_SAP_REPORT';
        const action = actions.requestSendToSapReport();
        const m_action = {
            type: REQUEST_SENT_TO_SAP_REPORT,
            fetchConfig: {
                path: apiService.reports.requestSendToSapReport(),
                method: 'POST',
                body: {},
                success: actions.receivedSendToSapReport()
            }
        };

        expect(action).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.path).toEqual(m_action.path);
        expect(action.method).toEqual(m_action.method);
        expect(action.body).toEqual(m_action.body);
        expect(action.success).toEqual(m_action.success);

    });

    it('should receive confirm execute report', () => {
        const RECEIVE_SENT_TO_SAP_REPORT = 'RECEIVE_SENT_TO_SAP_REPORT';
        const action = actions.receivedSendToSapReport();
        const m_action = {
            type: RECEIVE_SENT_TO_SAP_REPORT,
            status: true
        };

        expect(action).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.status).toEqual(m_action.status);
    });

    it('should receive failed execute report', () => {
        const FAILED_RECEIVE_SENT_TO_SAP_REPORT = 'FAILED_RECEIVE_SENT_TO_SAP_REPORT';
        const action = actions.failedReceivedSendToSapReport();
        const m_action = {
            type: FAILED_RECEIVE_SENT_TO_SAP_REPORT,
            status: false
        };

        expect(action).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.status).toEqual(m_action.status);
    });
});


