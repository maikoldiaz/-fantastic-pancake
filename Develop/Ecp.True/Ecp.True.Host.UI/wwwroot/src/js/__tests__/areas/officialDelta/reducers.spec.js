import * as actions from '../../../modules/dailyBalance/officialDelta/actions';
import { officialDelta } from '../../../modules/dailyBalance/officialDelta/reducers';

describe('Reducers for Official Delta', () => {
    const initialState = {};

    it('should handle action INIT_OFFICIAL_DELTA_TICKET_PROCESS',
        function () {
            const action = {
                type: actions.INIT_OFFICIAL_DELTA_TICKET_PROCESS
            };
            const newState = Object.assign({}, initialState, {
                step: 0,
                segmentId: 0,
                isValid: false,
                unapprovedNodes: [],
                periods: { officialPeriods: [] }
            });
            expect(officialDelta(initialState, action)).toEqual(newState);
        });

    it('should handle action SET_IS_VALID',
        function () {
            const action = {
                type: actions.SET_IS_VALID,
                isValid: true
            };
            const newState = Object.assign({}, initialState, { isValid: action.isValid });
            expect(officialDelta(initialState, action)).toEqual(newState);
        });

    it('should handle action SET_OFFICIAL_DELTA_TICKET',
        function () {
            const action = {
                type: actions.SET_OFFICIAL_DELTA_TICKET,
                ticket: {}
            };
            const newState = Object.assign({}, initialState, { ticket: action.ticket });
            expect(officialDelta(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_OFFICIAL_DELTA_VALIDATION_DATA',
        function () {
            const action = {
                type: actions.RECEIVE_OFFICIAL_DELTA_VALIDATION_DATA,
                unapprovedNodes: {}
            };
            const newState = Object.assign({}, initialState, { unapprovedNodes: action.unapprovedNodes });
            expect(officialDelta(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_OFFICIAL_PERIODS',
        function () {
            const action = {
                type: actions.RECEIVE_OFFICIAL_PERIODS,
                periods: {}
            };
            const newState = Object.assign({}, initialState, { periods: action.periods });
            expect(officialDelta(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_OFFICIAL_DELTA_TICKET',
        function () {
            const action = {
                type: actions.RECEIVE_OFFICIAL_DELTA_TICKET
            };
            const newState = Object.assign({}, initialState, { receiveToggler: !initialState.receiveToggler });
            expect(officialDelta(initialState, action)).toEqual(newState);
        });

    it('should handle action RESET_OFFICIAL_PERIODS',
        function () {
            const action = {
                type: actions.RESET_OFFICIAL_PERIODS
            };
            const newState = Object.assign({}, initialState, { periods: { officialPeriods: [] } });
            expect(officialDelta(initialState, action)).toEqual(newState);
        });

    it('should handle segment change',
        function () {
            const action = {
                type: '@@redux-form/CHANGE',
                meta: {
                    form: 'initOfficialDeltaTicket',
                    field: 'segment',
                    payload: { elementId: 1 }
                }
            };
            const newState = Object.assign({}, initialState, { segment: action.payload, segmentChangeToggler: !initialState.segmentChangeToggler, isValid: false });
            expect(officialDelta(initialState, action)).toEqual(newState);
        });

    it('should handle other field change',
        function () {
            const action = {
                type: '@@redux-form/CHANGE',
                meta: {
                    form: 'initOfficialDeltaTicket',
                    field: 'period',
                    payload: { elementId: 1 }
                }
            };
            const newState = Object.assign({}, initialState, { isValid: false });
            expect(officialDelta(initialState, action)).toEqual(newState);
        });

    it('should handle other form',
        function () {
            const action = {
                type: '@@redux-form/CHANGE',
                meta: {
                    form: 'form',
                    field: 'period',
                    payload: { elementId: 1 }
                }
            };
            expect(officialDelta(initialState, action)).toEqual(initialState);
        });

    it('should handle default case',
        function () {
            const action = {
                type: 'test'
            };
            expect(officialDelta(initialState, action)).toEqual(initialState);
        });
});
