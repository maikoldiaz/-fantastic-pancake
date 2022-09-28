import * as actions from '../../../modules/administration/exceptions/actions';
import { controlException } from '../../../modules/administration/exceptions/reducers';

describe('Reducer for control errors', () => {
    const initialState = {
        refreshToggler: false,
        errorDetail: [{}],
        pendingTransactionErrors: {
            comment: '',
            count: 0
        },
        retryToggler: false
    };

    it('should initialize add error comment', () => {
        const action = {
            type: actions.INIT_ERROR_ADD_COMMENT,
            name: 'pendingTransactionErrors',
            preText: 'Some pre text',
            postText: 'Some post Text',
            count: 1
        };
        const newState = Object.assign({}, initialState,
            { name: 'pendingTransactionErrors', pendingTransactionErrors: { comment: '', preText: 'Some pre text', postText: 'Some post Text', count: 1 } });
        expect(controlException(initialState, action)).toEqual(newState);
    });

    it('should receive error comment', () => {
        const action = {
            type: actions.RECEIVE_ERROR_COMMENT,
            status: '',
            refreshToggler: true
        };
        const newState = Object.assign({}, initialState, { status: '', refreshToggler: true });
        expect(controlException(initialState, action)).toEqual(newState);
    });
    it('should set selected data', () => {
        const action = {
            type: actions.SET_SELECTED_DATA,
            error: { errorId: 1 }
        };
        const newState = Object.assign({}, initialState, { selectedData: { errorId: 1 } });
        expect(controlException(initialState, action)).toEqual(newState);
    });
    it('should set error detail', () => {
        const action = {
            type: actions.SET_ERROR_DETAIL,
            errorDetail: { errorId: 1, error: 'Some text' }
        };
        const newState = Object.assign({}, initialState, { errorDetail: { errorId: 1, error: 'Some text' } });
        expect(controlException(initialState, action)).toEqual(newState);
    });
    it('should return default state', () => {
        const action = {
            type: 'SET_SELECTED_DATA1'
        };
        const state = Object.assign({}, initialState, { step: 0 });

        expect(controlException(state, action)).toEqual(state);
    });
    it('should save page filters', () => {
        const action = {
            type: 'SAVE_PAGE_FILTERS',
            pageFilters: 'test page filter'
        };
        const state = Object.assign({}, initialState, {
            pageFilters: 'test page filter'
        });

        expect(controlException(state, action).pageFilters).toEqual(state.pageFilters);
    });
    it('should receive retry records', () => {
        const action = {
            type: 'RECEIVE_RETRY_RECORDS'
        };
        const state = Object.assign({}, initialState, {
            retryToggler: true
        });
        const newState = Object.assign({}, initialState, {
            retryToggler: false
        });

        expect(controlException(state, action).retryToggler).toEqual(newState.retryToggler);
    });
    it('should reset grid filter', () => {
        const action = {
            type: actions.RESET_GRID_FILTER,
            loadGridEmpty: false,
            reloadGridToggler: true
        };
        const newState = Object.assign({}, initialState, { loadGridEmpty: false, reloadGridToggler: true });
        expect(controlException(initialState, action)).toEqual(newState);
    });
});
