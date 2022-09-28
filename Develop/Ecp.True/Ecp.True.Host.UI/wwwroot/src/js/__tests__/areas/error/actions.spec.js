import * as actions from '../../../modules/administration/exceptions/actions';
import { apiService } from '../../../common/services/apiService';

describe('Actions for control errors', () => {
    it('should initialize add comment modal', () => {
        const actionType = 'INIT_ERROR_ADD_COMMENT';
        const name = 'pendingTransactionErrors';
        const preText = 'Some Pre Text';
        const postText = 'Some Post Text';
        const count = 1;
        const action = actions.initErrorAddComment(name, preText, postText, count);

        expect(action.type).toEqual(actionType);
    });

    it('should save error comments', () => {
        const actionType = 'SAVE_ERROR_COMMENT';
        const errorComments = { errorId: [1], comment: 'Test Comment' };
        const action = actions.saveErrorComment(errorComments);
        const status = null;
        expect(action.type).toEqual(actionType);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.error.saveErrorComment());
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(status);
        expect(receiveAction.type).toEqual(actions.RECEIVE_ERROR_COMMENT);
        expect(receiveAction.status).toEqual(status);
    });
    it('should set selected data', () => {
        const actionType = 'SET_SELECTED_DATA';
        const error = null;
        const action = actions.setSelectedData(error);

        expect(action.type).toEqual(actionType);
    });
    it('should get error detail', () => {
        const actionType = 'GET_ERROR_DETAIL';
        const errorId = '1_P';
        const action = actions.getErrorDetail(errorId);
        const result = null;
        expect(action.type).toEqual(actionType);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.error.getErrorDetails(errorId));
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(result);
        expect(receiveAction.type).toEqual(actions.SET_ERROR_DETAIL);
        expect(receiveAction.errorDetail).toEqual(result);
    });
    it('should reset grid filter', () => {
        const action = actions.resetGridFilter();
        const RESET_GRID_FILTER = 'RESET_GRID_FILTER';
        const mock_action = {
            type: RESET_GRID_FILTER
        };
        expect(action).toEqual(mock_action);
    });
});
