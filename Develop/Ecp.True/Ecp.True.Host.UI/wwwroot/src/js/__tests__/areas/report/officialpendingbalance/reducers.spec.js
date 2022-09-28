import * as actions from '../../../../modules/report/officialInitialBalance/actions';
import { pendingBalance } from '../../../../modules/report/officialPendingBalance/reducers.js';


describe('Reducers for pendingBalance', () => {
    const initialState = {
        filterSettings: {},
        "filters": {
            "element": {},
        },
        "formValues": {
            "element": {},
        },
    };


    it('should handle action OFFICIAL_PENDING_BALANCE_SELECT_ELEMENT',
        function () {
            const action = {
                type: actions.OFFICIAL_PENDING_BALANCE_SELECT_ELEMENT,
                element: 'element'
            };
            const newState = Object.assign({}, initialState, { filterSettings: {}, filters: { element: {} }, formValues: { element: {} } });
            expect(pendingBalance(initialState, action)).toEqual(newState);
        });

    it('should handle action OFFICIAL_PENDING__BALANCE_SAVE_FILTER',
        function () {
            const action = {
                type: actions.OFFICIAL_PENDING_BALANCE_SAVE_FILTER,
                data: {
                    element: {}
                }
            };
            const newState = Object.assign({}, initialState, { filters: { element: {} }, formValues: { element: {} } });
            expect(pendingBalance(initialState, action)).toEqual(newState);
        });

    it('should handle action RESET_OFFICIAL_PENDING_BALANCE_FILTER',
        function () {
            const action = {
                type: actions.RESET_OFFICIAL_PENDING_BALANCE_FILTER,
            };
            const newState = Object.assign({}, initialState, { filterSettings: {}, filters: { element: {} }, formValues: { element: {} } });
            expect(pendingBalance(initialState, action)).toEqual(newState);
        });
});
