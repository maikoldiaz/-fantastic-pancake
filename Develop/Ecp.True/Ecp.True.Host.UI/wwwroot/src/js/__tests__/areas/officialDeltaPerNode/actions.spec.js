import * as actions from '../../../modules/dailyBalance/officialDeltaPerNode/actions';

describe('Actions for Official Delta Per Node', () => {
    it('should initialize node error detail', () => {
        const SET_NODE_ERROR = 'SET_NODE_ERROR';
        const node = { id: 100 };
        const action = actions.initializeNodeError(node);

        expect(action.type).toEqual(SET_NODE_ERROR);
        expect(action.node).toEqual(node);
    });

    it('should call set report toggler', () => {
        const action = actions.setReportTriggerToggler({});
        expect(action.type).toEqual('SET_REPORT_TRIGGER_TOGGLER');
    });

    it('should call set grid values', () => {
        const action = actions.setGridValues({});
        expect(action.type).toEqual('SET_GRID_VALUES');
    });

    it('should call refresh status', () => {
        const action = actions.refreshStatus({});
        expect(action.type).toEqual('REFRESH_STATUS');
    });
});
