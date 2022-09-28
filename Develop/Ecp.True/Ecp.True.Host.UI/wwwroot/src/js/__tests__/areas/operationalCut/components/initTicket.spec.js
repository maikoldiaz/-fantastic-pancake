
import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import InitTicket, { InitTicket as InitTicketComponent } from '../../../../modules/transportBalance/cutOff/components/initTicket.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import { dateService } from '../../../../common/services/dateService';
import { ticketValidator } from '../../../../modules/transportBalance/cutOff/services/ticketValidationService';

const initialValues = {
    root: {
        systemConfig: { controlLimit: 1, standardUncertaintyPercentage: 40, acceptableBalancePercentage: 2 }
    },
    shared: {
        categoryElements: [{
            categoryId: 1,
            elementId: 2,
            isActive: true,
            name: 'UpdatedCategory4gbx1'
        }]
    },
    cutoff: {
        operationalCut: {
            pendingTransactionErrors: [],
            ticket: { initialDate: '2019-10-01T00:00:00Z', finalDate: '2019-10-03T00:00:00Z' },
            unbalances: []
        } } };

function mountWithRealStore() {
    const reducers = {
        closeModal: jest.fn(() => Promise.resolve),
        cutoff: jest.fn(() => initialValues.cutoff),
        shared: jest.fn(() => initialValues.shared),
        root: jest.fn(()=> initialValues.root)
    };

    const props = {
        showError: jest.fn(),
        setTicketInfo: jest.fn(),
        systemConfig: jest.fn(() => initialValues.root.systemConfig),
        showLoader: jest.fn(),
        hideLoader: jest.fn()
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <InitTicket {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}


describe('InitTicket', () => {
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
        expect(enzymeWrapper.exists('#dd_initTicket_segment')).toBe(true);
        expect(enzymeWrapper.exists('#dt_initTicket_initialDate')).toBe(true);
        expect(enzymeWrapper.exists('#dt_initTicket_finalDate')).toBe(true);
        expect(enzymeWrapper.find('#btn_initTicket_submit').text()).toEqual('next');
    });

    it('should call request last ticket on prop update', () => {
        const props = {
            initialValues: {},
            categoryElements: [{
                categoryId: 1,
                elementId: 2,
                isActive: true,
                name: 'UpdatedCategory4gbx1'
            }],
            segmentChangeToggler: true,
            segment: true,
            requestLastTicket: jest.fn(),
            handleSubmit: jest.fn(),
            initCutOff: jest.fn(),
            getCategoryElements: jest.fn()
        };
        dateService.getDiff = jest.fn();
        const wrapper = shallow(<InitTicketComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { segmentChangeToggler: false }));
        expect(props.requestLastTicket.mock.calls).toHaveLength(1);
    });

    it('should handle form submit', () => {
        ticketValidator.validateAsync = jest.fn();
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find('form').simulate('submit');
        expect(props.setTicketInfo.mock.calls).toHaveLength(0);
        expect(props.showLoader.mock.calls).toHaveLength(0);
        expect(props.hideLoader.mock.calls).toHaveLength(0);
    });

    it('should reset fields on prop update', () => {
        const props = {
            initialValues: {},
            categoryElements: [{
                categoryId: 1,
                elementId: 2,
                isActive: true,
                name: 'UpdatedCategory4gbx1'
            }],
            segmentChangeToggler: true,
            segment: false,
            resetFields: jest.fn(),
            handleSubmit: jest.fn(),
            initCutOff: jest.fn(),
            getCategoryElements: jest.fn()
        };
        dateService.getDiff = jest.fn();
        const wrapper = shallow(<InitTicketComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { segmentChangeToggler: false }));
        expect(props.resetFields.mock.calls).toHaveLength(1);
    });
});
