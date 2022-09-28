import setup from '../../../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../../common/services/httpService';
import ControlLimit from '../../../../../../modules/administration/node/attributes/components/controlLimit.jsx';

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
                node: {
                    name: 'Automation_ge1g0',
                    nodeId: 11700,
                    description: 'Nodeone',
                    isActive: true,
                    autoOrder: false,
                    order: 604527289,
                    sendToSap: false,
                    logisticCenterId: '4033',
                    controlLimit: '2.60',
                    acceptableBalancePercentage: '2.30',
                    nodeOwnershipRuleId: 1,

                }
            }
        }
    };

    var initialValues = {
        name: 'Automation_ge1g0',
        nodeId: 11700,
        description: 'Nodeone',
        isActive: true,
        autoOrder: false,
        order: 604527289,
        sendToSap: false,
        logisticCenterId: '4033',
        controlLimit: '2.60',
        acceptableBalancePercentage: '2.30',
        nodeOwnershipRuleId: 1,
    };

    const reducers = {
        node: jest.fn(() => data.node),
        refreshToggler: jest.fn(() => data.category.manageCategory.refreshToggler),
        onClose: jest.fn(() => Promise.resolve())
    };

    const props = {
        initialValues: initialValues,
        closeModal: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn()
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <ControlLimit {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}


describe('ControlLimit', () => {
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
        expect(enzymeWrapper.exists('#lbl_node_name')).toBe(true);
        expect(enzymeWrapper.exists('#txt_decimal_controlLimit')).toBe(true);
        expect(enzymeWrapper.exists('#txt_percentage_acceptableBalancePercentage')).toBe(true);
        expect(enzymeWrapper.find('#btn_nodeAttributes_controlLimit_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find('#btn_nodeAttributes_controlLimit_submit').text()).toEqual('submit');
    });

    it('should give error when click on save and Control Limit is not given', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_nodeAttributes_controlLimit_submit').at(0).simulate('click');
        enzymeWrapper.find('form').simulate('submit');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call onClose on click of cancel button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_nodeAttributes_controlLimit_cancel').at(0).simulate('click');
    });

    it('should call handleSubmit method when form submits and it should be called with correct values', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find('form').simulate('submit');
        expect(props.handleSubmit.mock.calls).toHaveLength(1);
    });
});
