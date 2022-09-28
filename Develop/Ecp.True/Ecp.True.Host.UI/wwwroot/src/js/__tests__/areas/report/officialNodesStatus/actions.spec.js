import * as actions from '../../../../modules/report/officialNodesStatus/actions';
import { apiService } from '../../../../common/services/apiService';

describe('Actions for OfficialNodeStatusReport report', () => {
    it('should call RESET_OFFICIAL_NODE_STATUS_PERIODS action', () => {
        const action = actions.resetOfficialNodeStatusPeriods();
        expect(action.type).toEqual(actions.RESET_OFFICIAL_NODE_STATUS_PERIODS);
    });

    it('should call SAVE_OFFICIAL_NODE_STATUS_FILTER action', () => {
        const action = actions.saveOfficialNodeStatusFilter();
        expect(action.type).toEqual(actions.SAVE_OFFICIAL_NODE_STATUS_FILTER);
    });

    it('should execute getOfficialPeriods - REQUEST_OFFICIAL_NODE_STATUS_PERIODS', () => {
        const periods = { defaultYear: 2021 };

        const action = actions.getOfficialPeriods(1020, 2021);
        expect(action).toBeDefined();
        expect(action.type).toEqual(actions.REQUEST_OFFICIAL_NODE_STATUS_PERIODS);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();

        const successAction = action.fetchConfig.success(periods);
        expect(successAction.type).toEqual(actions.RECEIVE_OFFICIAL_NODE_STATUS_PERIODS);
        expect(successAction.periods).toEqual(periods);
    });

    it('should execute requestOfficialNodeStatusReport - REQUEST_SAVE_OFFICIAL_NODE_STATUS_REPORT', () => {
        const action = actions.requestOfficialNodeStatusReport({});
        expect(action).toBeDefined();
        expect(action.type).toEqual(actions.REQUEST_SAVE_OFFICIAL_NODE_STATUS_REPORT);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();
        expect(action.fetchConfig.failure).toBeDefined();

        const successAction = action.fetchConfig.success();
        expect(successAction.type).toEqual(actions.RECEIVE_SAVE_OFFICIAL_NODE_STATUS_REPORT);

        const failureAction = action.fetchConfig.failure();
        expect(failureAction.type).toEqual(actions.FAILURE_SAVE_OFFICIAL_NODE_STATUS_REPORT);
    });
});
