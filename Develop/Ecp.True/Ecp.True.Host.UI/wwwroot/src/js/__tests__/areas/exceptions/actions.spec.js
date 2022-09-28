import * as actions from '../../../modules/administration/exceptions/actions';
import { apiService } from '../../../common/services/apiService';

describe('Actions for ownership balance', () => {
    it('should initialize error add comment', () => {
        const name = 'name';
        const preText = 'text';
        const postText = 'name';
        const count = 1;
        const action = actions.initErrorAddComment(name, preText, postText, count);
        const INIT_ERROR_ADD_COMMENT = 'INIT_ERROR_ADD_COMMENT';
        const mock_action = {
            type: INIT_ERROR_ADD_COMMENT,
            name,
            preText,
            postText,
            count
        };
        expect(action).toEqual(mock_action);
    });

    it('should save error comment', () => {
        const SAVE_ERROR_COMMENT = 'SAVE_ERROR_COMMENT';
        const action = actions.saveErrorComment();
        expect(action.type).toEqual(SAVE_ERROR_COMMENT);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.error.saveErrorComment());

        expect(action.fetchConfig.method).toEqual('POST');
        expect(action.fetchConfig.success).toBeDefined();
        const receiveAction = action.fetchConfig.success(true);

        const RECEIVE_ERROR_COMMENT = 'RECEIVE_ERROR_COMMENT';
        expect(receiveAction.type).toEqual(RECEIVE_ERROR_COMMENT);
        expect(receiveAction.status).toEqual(true);
    });

    it('should get error detail', () => {
        const GET_ERROR_DETAIL = 'GET_ERROR_DETAIL';
        const action = actions.getErrorDetail('1');
        expect(action.type).toEqual(GET_ERROR_DETAIL);

        expect(action.fetchConfig.path).toEqual(apiService.error.getErrorDetails('1'));

        expect(action.fetchConfig.success).toBeDefined();
        const receiveAction = action.fetchConfig.success(true);

        const SET_ERROR_DETAIL = 'SET_ERROR_DETAIL';
        expect(receiveAction.type).toEqual(SET_ERROR_DETAIL);
        expect(receiveAction.errorDetail).toEqual(true);
    });

    it('should request retry record', () => {
        const REQUEST_RETRY_RECORDS = 'REQUEST_RETRY_RECORDS';
        const action = actions.requestRetryRecord({});
        expect(action.type).toEqual(REQUEST_RETRY_RECORDS);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.error.retryErrors());
        expect(action.fetchConfig.body).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();
        const receiveAction = action.fetchConfig.success(true);

        const RECEIVE_RETRY_RECORDS = 'RECEIVE_RETRY_RECORDS';
        expect(receiveAction.type).toEqual(RECEIVE_RETRY_RECORDS);
        expect(receiveAction.success).toEqual(true);
    });

    it('should save page filters', () => {
        const pageFilters = {};
        const action = actions.savePageFilters(pageFilters);
        const SAVE_PAGE_FILTERS = 'SAVE_PAGE_FILTERS';
        const mock_action = {
            type: SAVE_PAGE_FILTERS,
            pageFilters
        };
        expect(action).toEqual(mock_action);
    });

    it('should set selected data', () => {
        const error = {};
        const action = actions.setSelectedData(error);
        const SET_SELECTED_DATA = 'SET_SELECTED_DATA';
        const mock_action = {
            type: SET_SELECTED_DATA,
            error
        };
        expect(action).toEqual(mock_action);
    });

    it('should receive error detail', () => {
        const result = {};
        const action = actions.receiveErrorDetail(result);
        const SET_ERROR_DETAIL = 'SET_ERROR_DETAIL';
        const mock_action = {
            type: SET_ERROR_DETAIL,
            errorDetail: result
        };
        expect(action).toEqual(mock_action);
    });

    it('should receive error detail', () => {
        const result = {};
        const action = actions.receiveErrorDetail(result);
        const SET_ERROR_DETAIL = 'SET_ERROR_DETAIL';
        const mock_action = {
            type: SET_ERROR_DETAIL,
            errorDetail: result
        };
        expect(action).toEqual(mock_action);
    });

    it('should receive error comment', () => {
        const status = {};
        const action = actions.receiveErrorComment(status);
        const RECEIVE_ERROR_COMMENT = 'RECEIVE_ERROR_COMMENT';
        const mock_action = {
            type: RECEIVE_ERROR_COMMENT,
            status
        };
        expect(action).toEqual(mock_action);
    });

    it('should receive filtered record', () => {
        const status = {};
        const action = actions.receiveFilteredRecord(status);
        const RECEIVE_FILTERED_RECORD = 'RECEIVE_FILTERED_RECORD';
        const mock_action = {
            type: RECEIVE_FILTERED_RECORD,
            status
        };
        expect(action).toEqual(mock_action);
    });
});
