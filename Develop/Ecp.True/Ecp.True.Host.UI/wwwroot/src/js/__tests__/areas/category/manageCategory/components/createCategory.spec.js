import setup from '../../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import CreateCategory from '../../../../../modules/administration/category/manageCategory/components/createCategory.jsx';

const initialValues = {
    category: {
        manageCategory: {
            category: {},
            isActive: true,
            isGrouper: true,
            refreshToggler: false
        }
    }
};

function mountWithRealStore() {
    const reducers = {
        closeModal: jest.fn(() => Promise.resolve),
        category: jest.fn(() => initialValues.category)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <CreateCategory initialValues={initialValues} />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('CreateCategory', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('find Control', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#txt_category_name')).toBe(true);
        expect(enzymeWrapper.exists('#txtarea_category_description')).toBe(true);
        expect(enzymeWrapper.exists('#tog_category_active')).toBe(true);
        expect(enzymeWrapper.exists('#tog_category_grouper')).toBe(true);
        expect(enzymeWrapper.find('#btn_category_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find('#btn_category_submit').text()).toEqual('submit');
    });

    it('should give error when click on save and name is not given', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_category_submit').at(0).simulate('click');
        enzymeWrapper.find('form').simulate('submit');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call onClose on click of cancel button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_category_cancel').at(0).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });
});
