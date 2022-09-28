import * as actions from '../../../../modules/report/officialNodesStatus/actions';
import { officialNodeStatusReport } from '../../../../modules/report/officialNodesStatus/reducers';

describe('Reducers for OfficialNodeStatusReport report', () => {
    const initialState = {
        officialPeriods: {
            defaultYear: 1010,
            officialPeriods: []
        }
    };

    it('should return status unchanged', () => {
        expect(officialNodeStatusReport(initialState)).toEqual(initialState);
    });

    it('should handle action RESET_OFFICIAL_NODE_STATUS_PERIODS', () => {
        const action = {
            type: actions.RESET_OFFICIAL_NODE_STATUS_PERIODS
        };
        const newState = Object.assign({}, initialState, {
            officialPeriods: {
                defaultYear: null,
                officialPeriods: []
            }
        });
        expect(officialNodeStatusReport(initialState, action)).toEqual(newState);
    });

    it('should handle action RECEIVE_OFFICIAL_NODE_STATUS_PERIODS', () => {
        const action = {
            type: actions.RECEIVE_OFFICIAL_NODE_STATUS_PERIODS,
            periods: {
                defaultYear: 2010,
                officialPeriods: [{
                    month: 10,
                    years: 2020
                }]
            }
        };
        const newState = Object.assign({}, initialState, {
            officialPeriods: {
                defaultYear: 2010,
                officialPeriods: [{
                    month: 10,
                    years: 2020
                }]
            }
        });
        expect(officialNodeStatusReport(initialState, action)).toEqual(newState);
    });

    it('should handle action RECEIVE_SAVE_OFFICIAL_NODE_STATUS_REPORT', () => {
        const action = {
            type: actions.RECEIVE_SAVE_OFFICIAL_NODE_STATUS_REPORT
        };
        const newState = Object.assign({}, initialState, {
            receiveOfficialNodeStatusToggler: true
        });
        expect(officialNodeStatusReport(initialState, action)).toEqual(newState);
    });

    it('should handle action FAILURE_SAVE_OFFICIAL_NODE_STATUS_REPORT', () => {
        const action = {
            type: actions.FAILURE_SAVE_OFFICIAL_NODE_STATUS_REPORT
        };
        const newState = Object.assign({}, initialState, {
            failureOfficialNodeStatusToggler: true
        });
        expect(officialNodeStatusReport(initialState, action)).toEqual(newState);
    });

    it('should handle action SAVE_OFFICIAL_NODE_STATUS_FILTER', () => {
        const action = {
            type: actions.SAVE_OFFICIAL_NODE_STATUS_FILTER,
            filters: { executionId: 'execution-id-1' }
        };
        const newState = Object.assign({}, initialState, {
            filters: { executionId: 'execution-id-1' }
        });
        expect(officialNodeStatusReport(initialState, action)).toEqual(newState);
    });

    it('should update segmentChangeToggler when segment field changes', () => {
        const action = {
            type: '@@redux-form/CHANGE',
            meta: {
                form: 'officialNodeStatusReport',
                field: 'segment'
            },
            payload: { id: 'segment-id-1' }
        };
        const newState = Object.assign({}, initialState, {
            segment: { id: 'segment-id-1' },
            segmentChangeToggler: true
        });
        expect(officialNodeStatusReport(initialState, action)).toEqual(newState);
    });

    it('should return the initial state when the field updated is not segment', () => {
        const action = {
            type: '@@redux-form/CHANGE',
            meta: {
                form: 'officialNodeStatusReport',
                field: 'category'
            }
        };
        const newState = Object.assign({}, initialState);
        expect(officialNodeStatusReport(initialState, action)).toEqual(newState);
    });
});

