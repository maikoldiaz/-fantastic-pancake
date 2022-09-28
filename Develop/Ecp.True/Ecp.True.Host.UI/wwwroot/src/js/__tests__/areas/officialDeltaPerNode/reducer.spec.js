import * as actions from '../../../modules/dailyBalance/officialDeltaPerNode/actions';
import { officialDeltaPerNode } from '../../../modules/dailyBalance/officialDeltaPerNode/reducers';

describe('Reducer for node ownership cut', () => {
    const initialState = {
        deltaTicket: {
            segment: null,
            startDate: null,
            endDate: null
        },
        isFinalDateReceivedToggler: true,
        refreshDeltaCalculationGridToggler: false,
        isReady: true
    };

    it('should call set grid values', () => {
        const action = {
            type: actions.SET_GRID_VALUES,
            data: {}
        };

        const newState = Object.assign({}, initialState, { nodeGridValues: action.data });
        expect(officialDeltaPerNode(initialState, action)).toEqual(newState);
    });

    it('should call set report trigger toggler', () => {
        const action = {
            type: actions.SET_REPORT_TRIGGER_TOGGLER,
            nodeGridToggler: true
        };

        const newState = Object.assign({}, initialState, { nodeGridToggler: action.nodeGridToggler });
        expect(officialDeltaPerNode(initialState, action)).toEqual(newState);
    });
});
