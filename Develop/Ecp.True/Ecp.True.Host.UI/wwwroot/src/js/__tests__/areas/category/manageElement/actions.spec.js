import * as actions from '../../../../modules/administration/category/manageElement/actions';
import { apiService } from '../../../../common/services/apiService';

describe('Actions for Element', () => {
    it('should request create Element', () => {
        const categoryElementObject = {
            categoryId: 1,
            name: 'test',
            description: 'description',
            isActive: true
        };
        const REQUEST_ADD_CATEGORYELEMENT = 'REQUEST_ADD_CATEGORYELEMENT';
        const action = actions.createElement(categoryElementObject);

        expect(action.type).toEqual(REQUEST_ADD_CATEGORYELEMENT);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.category.createOrUpdateElements());
        expect(action.fetchConfig.body).toEqual(categoryElementObject);

        expect(action.fetchConfig.success).toBeDefined();
        const receiveAction = action.fetchConfig.success(true);

        const RECEIVE_ADD_CATEGORYELEMENT = 'RECEIVE_ADD_CATEGORYELEMENT';
        expect(receiveAction.type).toEqual(RECEIVE_ADD_CATEGORYELEMENT);
        expect(receiveAction.status).toEqual(true);
    });

    it('should request update Element', () => {
        const categoryElementObject = {
            elementId: 1,
            categoryId: 1,
            name: 'test',
            description: 'description',
            isActive: true
        };
        const REQUEST_UPDATE_CATEGORYELEMENT = 'REQUEST_UPDATE_CATEGORYELEMENT';
        const action = actions.updateElement(categoryElementObject);

        expect(action.type).toEqual(REQUEST_UPDATE_CATEGORYELEMENT);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.category.createOrUpdateElements());
        expect(action.fetchConfig.body).toEqual(categoryElementObject);

        expect(action.fetchConfig.method).toEqual('PUT');
        expect(action.fetchConfig.success).toBeDefined();
        const receiveAction = action.fetchConfig.success(true);

        const RECEIVE_UPDATE_CATEGORYELEMENT = 'RECEIVE_UPDATE_CATEGORYELEMENT';
        expect(receiveAction.type).toEqual(RECEIVE_UPDATE_CATEGORYELEMENT);
        expect(receiveAction.status).toEqual(true);
    });

    it('should request initialize edit', () => {
        const categoryElementObject = {
            elementId: 1,
            categoryId: 1,
            name: 'test',
            description: 'description',
            isActive: true
        };

        const action = actions.initializeEdit(categoryElementObject);
        const REQUEST_EDIT_INITIALIZE_CATEGORYELEMENT = 'REQUEST_EDIT_INITIALIZE_CATEGORYELEMENT';
        const m_action = {
            type: REQUEST_EDIT_INITIALIZE_CATEGORYELEMENT,
            categoryElement: categoryElementObject
        };

        expect(action).toEqual(m_action);
    });

    it('should request icons', () => {
        const REQUEST_ICONS = 'REQUEST_ICONS';
        const action = actions.getIcons();

        expect(action.type).toEqual(REQUEST_ICONS);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.category.getIcons());

        expect(action.fetchConfig.success).toBeDefined();
        const receiveAction = action.fetchConfig.success(true);

        const RECEIVE_ICONS = 'RECEIVE_ICONS';
        expect(receiveAction.type).toEqual(RECEIVE_ICONS);
        expect(receiveAction.icons).toEqual(true);
    });

    it('should open icon modal', () => {
        const OPEN_ICON_MODAL = 'OPEN_ICON_MODAL';
        const action = actions.openIconModal();
        expect(action.type).toEqual(OPEN_ICON_MODAL);
    });

    it('should refresh status', () => {
        const REFRESH_STATUS = 'REFRESH_STATUS';
        const action = actions.refreshStatus();
        expect(action.type).toEqual(REFRESH_STATUS);
    });
});
