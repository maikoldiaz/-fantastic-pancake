import React from 'react';
import { mount } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
// eslint-disable-next-line no-unused-vars
import setup from '../../../areas/setup';
import { CategoryElementFilterComponent } from '../../../../common/components/filters/categoryElementFilter.jsx';

const defaultValue = {
    categoryElements: [
        {
            category: null, element: null
        }
    ]
};
const initCategoryElementFilter = {
    selectedCategories: {},
    values: defaultValue,
    defaultValues: defaultValue
};
const sharedInitialState = {
    categories: [],
    categoryElements: [],
    progressStatus: {
        categoryElements: 0
    }
};

function mountWithRealStore() {
    const categoryElementFilter = {
        defaultValues: {
            categoryElements: [
                {
                    category: null, element: null
                }
            ]
        }
    };
    const reducers = {
        form: formReducer,
        categoryElementFilter: jest.fn(() => initCategoryElementFilter),
        saveCategoryElementFilter: jest.fn(() => 'nodeAttributes'),
        shared: jest.fn(() => sharedInitialState),
        flyout: jest.fn(() => 'nodeAttributes')
    };

    const props = {
        fields: { _isFieldArray: true, length: 1, name: 'categoryElements' },
        meta: { dirty: false, form: 'nodeAttributes', invalid: false, pristine: true, valid: true },
        name: 'nodeAttributes',
        categoryOptions: [], categoryElementOptions: [], selectedCategories: {}, showTrash: true,
        categoryElementFilter: categoryElementFilter,
        onSubmit: jest.fn()
    };
    const store = createStore(combineReducers(reducers));
    const enzymeWrapper = mount(<Provider store={store}><CategoryElementFilterComponent {...props} /></Provider>);
    return { enzymeWrapper, props, store };
}

describe('category element filter', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(CategoryElementFilterComponent).length).toEqual(1);
    });
});
