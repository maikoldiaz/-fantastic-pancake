import setup from '../../../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../../common/services/httpService';
import Uncertainty from '../../../../../../modules/administration/node/attributes/components/uncertainty.jsx';

function mountWithRealStore() {
    const data = {
        category: {
            manageCategory: {
                category: {},
                isActive: true,
                isGrouper: true,
                refreshToggler: false
            }
        },
        node: {
            attributes: {
                nodeProduct: {
                    rowVersion:1,
                    storageLocationProductId: 123,
                },
                productToggler: false
            }
        }
    };

    const reducers = {
        node: jest.fn(() => data.node),
        refreshToggler: jest.fn(() => data.category.manageCategory.refreshToggler),
        onClose: jest.fn(() => Promise.resolve())
    };

    const props = {
        closeModal: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn()
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <Uncertainty {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Uncertainty', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should have cancel and submit button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#lbl_nodeProducts_product_name')).toBe(true);
        expect(enzymeWrapper.exists('#txt_percentage_uncertaintyPercentage')).toBe(true);
        expect(enzymeWrapper.find('#btn_nodeProducts_uncertainty_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find('#btn_nodeProducts_uncertainty_submit').text()).toEqual('submit');
    });

    it('should give error when click on save and uncertainty is not given', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_nodeProducts_uncertainty_submit').at(0).simulate('click');
        enzymeWrapper.find('form').simulate('submit');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call onClose on click of cancel button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_nodeProducts_uncertainty_cancel').at(0).simulate('click');
    });
});
