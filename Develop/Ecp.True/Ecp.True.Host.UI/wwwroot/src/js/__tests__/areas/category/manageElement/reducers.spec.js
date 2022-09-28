import * as actions from '../../../../modules/administration/category/manageElement/actions';
import { manageElement } from '../../../../modules/administration/category/manageElement/reducers';

describe('Reducers for Element', () => {
    const initialState = {
        refreshToggler: false,
        categoryElement: {},
        iconModal: false
    };

    it('should handle action RECEIVE_ADD_CATEGORYELEMENT',
        function () {
            const elementObject = {
                message: 'Element created successfully'
            };
            const action = {
                type: actions.RECEIVE_ADD_CATEGORYELEMENT,
                status: elementObject
            };
            const newState = Object.assign({}, initialState, { status: elementObject, refreshToggler: !initialState.refreshToggler });
            expect(manageElement(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_UPDATE_CATEGORYELEMENT',
        function () {
            const elementObject = {
                message: 'Element updated successfully'
            };
            const action = {
                type: actions.RECEIVE_UPDATE_CATEGORYELEMENT,
                status: elementObject
            };
            const newState = Object.assign({}, initialState, { status: elementObject, refreshToggler: !initialState.refreshToggler });
            expect(manageElement(initialState, action)).toEqual(newState);
        });


    it('should handle action REQUEST_EDIT_INITIALIZE_CATEGORYELEMENT',
        function () {
            const categoryElementObject = {
                elementId: 1,
                categoryId: 1,
                name: 'test',
                description: 'description',
                isActive: true
            };
            const action = {
                type: actions.REQUEST_EDIT_INITIALIZE_CATEGORYELEMENT,
                categoryElement: categoryElementObject
            };
            const newState = Object.assign({}, initialState, { categoryElement: categoryElementObject });
            expect(manageElement(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_ICONS',
        function () {
            const icons = {};
            const action = {
                type: actions.RECEIVE_ICONS,
                icons: icons
            };
            const newState = Object.assign({}, initialState, { icons: action.icons });
            expect(manageElement(initialState, action)).toEqual(newState);
        });

    it('should handle action OPEN_ICON_MODAL',
        function () {
            const action = {
                type: actions.OPEN_ICON_MODAL
            };
            const newState = Object.assign({}, initialState, { iconModal: !initialState.iconModal });
            expect(manageElement(initialState, action)).toEqual(newState);
        });

    it('should handle action REFRESH_STATUS',
        function () {
            const action = {
                type: actions.REFRESH_STATUS
            };
            const newState = Object.assign({}, initialState, { status: { message: '' } });
            expect(manageElement(initialState, action)).toEqual(newState);
        });


    it('should return default state',
        function () {
            const action = {
                type: 'dummy Value'
            };
            const newState = Object.assign({}, initialState);
            expect(manageElement(initialState, action)).toEqual(newState);
        });
});
