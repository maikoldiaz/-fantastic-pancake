import * as actions from '../../../../modules/report/officialInitialBalance/actions';
import { initialBalance } from '../../../../modules/report/officialInitialBalance/reducers.js';
import { dateService } from '../../../../common/services/dateService';
import { constants } from '../../../../common/services/constants';


describe('Reducers for InitialBalance', () => {
    const initialState = {
        filterSettings: {}
    };

    it('should handle action OFFICIAL_INITIAL_BALANCE_SELECT_ELEMENT',
        function () {
            const action = {
                type: actions.OFFICIAL_INITIAL_BALANCE_SELECT_ELEMENT,
                element: 'element'
            };
            const newState = Object.assign({}, initialState, { formValues: { element: 'element' } });
            expect(initialBalance(initialState, action)).toEqual(newState);
        });

    it('should handle action OFFICIAL_INITIAL_BALANCE_SAVE_FILTER',
        function () {
            const action = {
                type: actions.OFFICIAL_INITIAL_BALANCE_SAVE_FILTER,
                data: {
                    element: {}
                }
            };
            const newState = Object.assign({}, initialState, { filters: { element: {} }, formValues: { element: {} } });
            expect(initialBalance(initialState, action)).toEqual(newState);
        });

    it('should handle action RESET_OFFICIAL_INITIAL_BALANCE_FILTER',
        function () {
            const action = {
                type: actions.RESET_OFFICIAL_INITIAL_BALANCE_FILTER
            };
            const newState = Object.assign({}, initialState, { filterSettings: {}, filters: { elementId: null } });
            expect(initialBalance(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_OFFICIAL_INITIAL_BALANCE',
        function () {
            const action = {
                type: actions.RECEIVE_OFFICIAL_INITIAL_BALANCE,
                executionId: 23
            };
            const newState = Object.assign({}, initialState, { reportToggler: true, executionId: 23, filters: { executionId: 23 } });
            expect(initialBalance(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_OFFICIAL_INITIAL_BALANCE_STATUS',
        function () {
            const action = {
                type: actions.RECEIVE_OFFICIAL_INITIAL_BALANCE_STATUS,
                status: true
            };
            const newState = Object.assign({}, initialState, { receiveStatusToggler: true, status: true });
            expect(initialBalance(initialState, action)).toEqual(newState);
        });

    it('should handle action REFRESH_STATUS',
        function () {
            const action = {
                type: actions.REFRESH_STATUS
            };
            const newState = Object.assign({}, initialState, { refreshStatusToggler: true });
            expect(initialBalance(initialState, action)).toEqual(newState);
        });

    it('should handle action CLEAR_STATUS',
        function () {
            const action = {
                type: actions.CLEAR_STATUS
            };
            const newState = Object.assign({}, initialState, { officialInitialBalanceStatus: null });
            expect(initialBalance(initialState, action)).toEqual(newState);
        });
});
