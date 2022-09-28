import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { reducer as formReducer } from 'redux-form';
import { mount } from 'enzyme';
import React from 'react';
// eslint-disable-next-line no-unused-vars
import setup from '../../areas/setup';
import Shared, { Shared as SharedComponent } from '../../../common/components/shared.jsx';

function mountWithRealStore() {
    const shared = {
        categoryElementsToggler: true,
        allCategoriesToggler: {},
        storageLocationsToggler: {},
        systemTypesToggler: {},
        variableTypesToggler: {}
    };

    const reducers = {
        form: formReducer,
        shared: jest.fn(() => shared)
    };
    const store = createStore(combineReducers(reducers));
    const props = {
        requestSystemTypes: jest.fn(),
        requestCategoryElements: jest.fn(),
        requestCategories: jest.fn(),
        requestLogisticCenters: jest.fn(),
        requestStorageLocations: jest.fn(),
        requestVariableTypes: jest.fn()
    };

    const enzymeWrapper = mount(<Provider store={store}><Shared {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('shared', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should request category elements, when the props change.', () => {
        const props = {
            requestSystemTypes: jest.fn(),
            requestCategoryElements: jest.fn(),
            requestCategories: jest.fn(),
            requestLogisticCenters: jest.fn(),
            requestStorageLocations: jest.fn(),
            requestVariableTypes: jest.fn(),
            categoryElementsToggler: true
        };
        const wrapper = mount(<SharedComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { categoryElementsToggler: false }));
        expect(props.requestCategoryElements.mock.calls).toHaveLength(1);
    });

    it('should request categories, when the props change.', () => {
        const props = {
            requestSystemTypes: jest.fn(),
            requestCategoryElements: jest.fn(),
            requestCategories: jest.fn(),
            requestLogisticCenters: jest.fn(),
            requestStorageLocations: jest.fn(),
            requestVariableTypes: jest.fn(),
            allCategoriesToggler: true
        };
        const wrapper = mount(<SharedComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { allCategoriesToggler: false }));
        expect(props.requestCategories.mock.calls).toHaveLength(1);
    });

    it('should request logistic center toggler, when the props change.', () => {
        const props = {
            requestSystemTypes: jest.fn(),
            requestCategoryElements: jest.fn(),
            requestCategories: jest.fn(),
            requestLogisticCenters: jest.fn(),
            requestStorageLocations: jest.fn(),
            requestVariableTypes: jest.fn(),
            logisticCentersToggler: true
        };
        const wrapper = mount(<SharedComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { logisticCentersToggler: false }));
        expect(props.requestLogisticCenters.mock.calls).toHaveLength(1);
    });

    it('should request storage locations toggler, when the props change.', () => {
        const props = {
            requestSystemTypes: jest.fn(),
            requestCategoryElements: jest.fn(),
            requestCategories: jest.fn(),
            requestLogisticCenters: jest.fn(),
            requestStorageLocations: jest.fn(),
            requestVariableTypes: jest.fn(),
            storageLocationsToggler: true
        };
        const wrapper = mount(<SharedComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { storageLocationsToggler: false }));
        expect(props.requestStorageLocations.mock.calls).toHaveLength(1);
    });

    it('should request system types toggler, when the props change.', () => {
        const props = {
            requestSystemTypes: jest.fn(),
            requestCategoryElements: jest.fn(),
            requestCategories: jest.fn(),
            requestLogisticCenters: jest.fn(),
            requestStorageLocations: jest.fn(),
            requestVariableTypes: jest.fn(),
            systemTypesToggler: true
        };
        const wrapper = mount(<SharedComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { systemTypesToggler: false }));
        expect(props.requestSystemTypes.mock.calls).toHaveLength(1);
    });

    it('should variable types toggler, when the props change.', () => {
        const props = {
            requestSystemTypes: jest.fn(),
            requestCategoryElements: jest.fn(),
            requestCategories: jest.fn(),
            requestLogisticCenters: jest.fn(),
            requestStorageLocations: jest.fn(),
            requestVariableTypes: jest.fn(),
            variableTypesToggler: true
        };
        const wrapper = mount(<SharedComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { variableTypesToggler: false }));
        expect(props.requestVariableTypes.mock.calls).toHaveLength(1);
    });
});
