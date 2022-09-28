import * as actions from '../../../../modules/administration/category/manageCategory/actions';
import { manageCategory } from '../../../../modules/administration/category/manageCategory/reducers';

describe('Reducers for Category', () => {
    const initialState = {
        refreshToggler: false,
        category: {},
        categories: []
    };

    it('should handle action RECEIVE_GET_CATEGORIES',
        function () {
            const action = {
                type: actions.RECEIVE_GET_CATEGORIES,
                categories: { value: [] }
            };
            const newState = Object.assign({}, initialState);
            expect(manageCategory(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_ADD_CATEGORY',
        function () {
            const categoryObject = {
                message: 'Category created successfully'
            };
            const action = {
                type: actions.RECEIVE_ADD_CATEGORY,
                status: categoryObject
            };
            const newState = Object.assign({}, initialState, { status: categoryObject, refreshToggler: !initialState.refreshToggler });
            expect(manageCategory(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_UPDATE_CATEGORY',
        function () {
            const categoryObject = {
                message: 'Category updated successfully'
            };
            const action = {
                type: actions.RECEIVE_UPDATE_CATEGORY,
                status: categoryObject
            };
            const newState = Object.assign({}, initialState, { status: categoryObject, refreshToggler: !initialState.refreshToggler });
            expect(manageCategory(initialState, action)).toEqual(newState);
        });


    it('should handle action REQUEST_EDIT_INITIALIZE_CATEGORY',
        function () {
            const categoryObject = {
                name: 'test',
                description: 'description',
                isActive: true,
                isGroup: true
            };
            const action = {
                type: actions.REQUEST_EDIT_INITIALIZE_CATEGORY,
                category: categoryObject
            };
            const newState = Object.assign({}, initialState, { category: categoryObject });
            expect(manageCategory(initialState, action)).toEqual(newState);
        });

    it('should return default state',
        function () {
            const action = {
                type: 'dummy Value'
            };
            const newState = Object.assign({}, initialState);
            expect(manageCategory(initialState, action)).toEqual(newState);
        });
});
