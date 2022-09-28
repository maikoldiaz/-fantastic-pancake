import * as actions from '../../../../modules/report/nodeConfiguration/actions';
import { nodeConfigurationReport } from '../../../../modules/report/nodeConfiguration/reducers';

describe('Reducers for nodeConfigurationReport', () => {
    const initialState = {
        filterSettings: {},
        ticket: null
    };
    it('should handle action NODE_CONFIGURATION_REPORT_SAVE_REPORT_FILTER',
        function () {
            const action = {
                type: actions.NODE_CONFIGURATION_REPORT_SAVE_REPORT_FILTER,
                filters: { nodeId: 1 }
            };
            const newState = Object.assign({}, initialState, { filters: { nodeId: 1 } });
            expect(nodeConfigurationReport(initialState, action)).toEqual(newState);
        });

    it('should return default state',
        function () {
            const action = {
                type: 'dummy Value'
            };
            const newState = Object.assign({}, initialState);
            expect(nodeConfigurationReport(initialState, action)).toEqual(newState);
        });
});
