import * as actions from '../../../../modules/administration/category/manageCategory/actions';
import { apiService } from '../../../../common/services/apiService';

it('should request get Categories', () => {
    const REQUEST_GET_CATEGORIES = 'REQUEST_GET_CATEGORIES';
    const action = actions.getCategories();

    expect(action.type).toEqual(REQUEST_GET_CATEGORIES);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.category.getAll(true));

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_GET_CATEGORIES = 'RECEIVE_GET_CATEGORIES';
    expect(receiveAction.type).toEqual(RECEIVE_GET_CATEGORIES);
});

describe('Actions for Category', () => {
    it('should request create Category', () => {
        const categoryObject = {
            name: 'test',
            description: 'description',
            isActive: true,
            isGroup: true
        };
        const REQUEST_ADD_CATEGORY = 'REQUEST_ADD_CATEGORY';
        const action = actions.createCategory(categoryObject);

        expect(action.type).toEqual(REQUEST_ADD_CATEGORY);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.category.createOrUpdate());
        expect(action.fetchConfig.body).toEqual(categoryObject);

        expect(action.fetchConfig.success).toBeDefined();
        const receiveAction = action.fetchConfig.success(true);

        const RECEIVE_ADD_CATEGORY = 'RECEIVE_ADD_CATEGORY';
        expect(receiveAction.type).toEqual(RECEIVE_ADD_CATEGORY);
        expect(receiveAction.status).toEqual(true);
    });

    it('should request update Category', () => {
        const categoryObject = {
            categoryId: 1,
            name: 'test',
            description: 'description',
            isActive: true,
            isGroup: true
        };
        const REQUEST_UPDATE_CATEGORY = 'REQUEST_UPDATE_CATEGORY';
        const action = actions.updateCategory(categoryObject);

        expect(action.type).toEqual(REQUEST_UPDATE_CATEGORY);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.category.createOrUpdate());
        expect(action.fetchConfig.body).toEqual(categoryObject);

        expect(action.fetchConfig.method).toEqual('PUT');
        expect(action.fetchConfig.success).toBeDefined();
        const receiveAction = action.fetchConfig.success(true);

        const RECEIVE_UPDATE_CATEGORY = 'RECEIVE_UPDATE_CATEGORY';
        expect(receiveAction.type).toEqual(RECEIVE_UPDATE_CATEGORY);
        expect(receiveAction.status).toEqual(true);
    });

    it('should request initialize edit', () => {
        const categoryObject = {
            categoryId: 1,
            name: 'test',
            description: 'description',
            isActive: true,
            isGroup: true
        };

        const action = actions.initializeEdit(categoryObject);
        const REQUEST_EDIT_INITIALIZE_CATEGORY = 'REQUEST_EDIT_INITIALIZE_CATEGORY';
        const m_action = {
            type: REQUEST_EDIT_INITIALIZE_CATEGORY,
            category: categoryObject
        };

        expect(action).toEqual(m_action);
    });
});
