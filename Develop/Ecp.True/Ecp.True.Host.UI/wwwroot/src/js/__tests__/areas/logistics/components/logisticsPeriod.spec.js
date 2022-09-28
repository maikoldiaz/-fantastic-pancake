import setup from '../../setup.js';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import LogisticsPeriod from '../../../../modules/transportBalance/logistics/components/logisticsPeriod.jsx';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';

const logistics = {
    operational: {
        refreshToggler: true,
        lastOwnershipDate: {},
        initialDate: {},
        refreshDateToggler: true
    }
};

const shared = {
    groupedCategoryElements: [],
    categoryElements: []
};

const props = {
    componentType: 3
};

function mountWithRealStore() {
    const reducers = {
        closeModal: jest.fn(() => Promise.resolve),
        logistics: jest.fn(() => logistics),
        shared: jest.fn(() => shared)
    };

    const store = createStore(combineReducers(reducers));
    const enzymeWrapper = mount(<Provider store={store} >
        <LogisticsPeriod {...props}/>
    </Provider>);
    return { store, enzymeWrapper };
}

describe('LogisticsPeriod', () => {
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
        expect(enzymeWrapper.exists('#dt_logisticsPeriod_initialDate')).toBe(true);
        expect(enzymeWrapper.exists('#dt_logisticsPeriod_finalDate')).toBe(true);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_logisticsPeriod_cancel').text()).toEqual(resourceProvider.read('cancel'));
        expect(enzymeWrapper.find(ModalFooter).find('#btn_logisticsPeriod_submit').text()).toEqual(resourceProvider.read('next'));
    });

    it('should call onClose on click of cancel button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('ModalFooter').find('#btn_logisticsPeriod_cancel').at(0).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });
});
