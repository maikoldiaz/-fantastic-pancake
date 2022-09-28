import setup from '../../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import CreateElement from '../../../../../modules/administration/category/manageElement/components/createElement.jsx';

const initialValues = {
    category: {
        manageElement: {
            categoryElement: {
                name: '',
                description: '',
                category: {
                    name: '',
                    categoryId: 1
                }
            },
            isActive: true,
            refreshToggler: false
        },
        manageCategory: {
            categories: {}
        },
        refreshToggler: false
    }
};

const picker = {
    colorPicker: {
        elementColor:{
            isOpen: true,
            color: {
                hex: "#000"
            }
        }
    },
    iconPicker: {}
}

function mountWithRealStore() {
    const reducers = {
        closeModal: jest.fn(() => Promise.resolve),
        category: jest.fn(() => initialValues.category),
        colorPicker: jest.fn(() => picker.colorPicker),
        iconPicker: jest.fn(() => picker.iconPicker)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <CreateElement initialValues={initialValues} />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('CreateElement', () => {
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
        expect(enzymeWrapper.exists('#dd_category')).toBe(true);
        expect(enzymeWrapper.exists('#txt_element_name')).toBe(true);
        expect(enzymeWrapper.exists('#txtarea_element_description')).toBe(true);
        expect(enzymeWrapper.exists('#tog_element_active')).toBe(true);
        expect(enzymeWrapper.exists('#txt_icon')).toBe(true);
        expect(enzymeWrapper.exists('#color_picker')).toBe(true);
        expect(enzymeWrapper.find('#btn_element_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find('#btn_element_submit').text()).toEqual('submit');
    });

    it('should give error when click on save and name is not given', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_element_submit').at(0).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call onClose on click of cancel button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_element_cancel').at(0).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });
});
