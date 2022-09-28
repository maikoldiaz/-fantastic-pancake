import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import InitDeltaTicket, { InitDeltaTicket as InitDeltaTicketComponent } from '../../../../modules/transportBalance/operationalDelta/components/initDeltaTicket.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { ticketValidator } from '../../../../modules/transportBalance/operationalDelta/ticketValidationService';
import { dateService } from '../../../../common/services/dateService';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';

const initialValues = {
    shared: {
        operationalSegments: [{
            categoryId: 1,
            elementId: 2,
            isActive: true,
            name: 'UpdatedCategory4gbx1'
        }]
    },
    operationalDelta: {
        deltaTicket: {
            startDate: '2019-10-01T00:00:00Z',
            endDate: '2019-10-03T00:00:00Z',
            segment: {}
        }
    }
};

function mountWithRealStore() {
    const reducers = {
        closeModal: jest.fn(() => Promise.resolve),
        operationalDelta: jest.fn(() => initialValues.operationalDelta),
        shared: jest.fn(() => initialValues.shared)
    };

    const props = {
        showError: jest.fn(),
        showLoader: jest.fn(),
        hideLoader: jest.fn(),
        setDeltaTicketInfo: jest.fn()
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <InitDeltaTicket />
    </Provider>);
    return { store, enzymeWrapper, props };
}


describe('InitDeltaTicket', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        systemConfigService.getCurrentMonthValidDays = jest.fn(() => {
            return 40;
        });
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('find Control', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#dd_initDeltaTicket_segment')).toBe(true);
        expect(enzymeWrapper.exists('#dt_initDeltaTicket_statDate')).toBe(true);
        expect(enzymeWrapper.exists('#dt_initDeltaTicket_endDate')).toBe(true);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_initDeltaTicket_submit').text()).toEqual('deltaProcess');
    });

    it('should handle form submit', () => {
        ticketValidator.validateCutoffDeltaCalculation = jest.fn();
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find('form').simulate('submit');
        expect(props.setDeltaTicketInfo.mock.calls).toHaveLength(0);
        expect(props.showLoader.mock.calls).toHaveLength(0);
        expect(props.hideLoader.mock.calls).toHaveLength(0);
        expect(props.showError.mock.calls).toHaveLength(0);
    });

    it('should initialize delta ticket on mount', () => {
        const props = {
            initDeltaTicket: jest.fn(),
            getCategoryElements: jest.fn(),
            handleSubmit: jest.fn(),
            initDeltaStartDate: jest.fn(),
            deltaTicket: {
                startDate: null,
                endDate: null,
                segment: {}
            }
        };
        const wrapper = shallow(<InitDeltaTicketComponent {...props} />);
        expect(props.initDeltaTicket.mock.calls).toHaveLength(1);
        expect(props.getCategoryElements.mock.calls).toHaveLength(1);
    });

    it('should show error on update', () => {
        const props = {
            initDeltaTicket: jest.fn(),
            getCategoryElements: jest.fn(),
            showError: jest.fn(),
            changeField: jest.fn(),
            setIsReady: jest.fn(),
            handleSubmit: jest.fn(),
            initDeltaStartDate: jest.fn(),
            deltaTicket: {
                startDate: null,
                endDate: dateService.format('01-Jan-01'),
                segment: {}
            }
        };
        const wrapper = shallow(<InitDeltaTicketComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { isFinalDateReceivedToggler: true }));
        expect(props.showError.mock.calls).toHaveLength(1);
        expect(props.changeField.mock.calls).toHaveLength(1);
        expect(props.setIsReady.mock.calls).toHaveLength(1);
    });

    it('should initialize segment on segment change', () => {
        const props = {
            initDeltaTicket: jest.fn(),
            getCategoryElements: jest.fn(),
            showError: jest.fn(),
            changeField: jest.fn(),
            setIsReady: jest.fn(),
            handleSubmit: jest.fn(),
            initDeltaStartDate: jest.fn(),
            initDeltaSegment: jest.fn(),
            getEndDate: jest.fn(),
            hideError: jest.fn(),
            deltaTicket: {
                startDate: null,
                endDate: dateService.format('01-Jan-01'),
                segment: {}
            }
        };
        const wrapper = shallow(<InitDeltaTicketComponent {...props} />);
        wrapper.instance().onSegmentSelection({ elementId: 10 });
        expect(props.initDeltaSegment.mock.calls).toHaveLength(1);
        expect(props.getEndDate.mock.calls).toHaveLength(1);
        expect(props.hideError.mock.calls).toHaveLength(1);
    });

    it('should clear end date on clearing segment', () => {
        const props = {
            initDeltaTicket: jest.fn(),
            getCategoryElements: jest.fn(),
            showError: jest.fn(),
            changeField: jest.fn(),
            setIsReady: jest.fn(),
            handleSubmit: jest.fn(),
            initDeltaStartDate: jest.fn(),
            clearEndDate: jest.fn(),
            deltaTicket: {
                startDate: null,
                endDate: dateService.format('01-Jan-01'),
                segment: {}
            }
        };
        const wrapper = shallow(<InitDeltaTicketComponent {...props} />);
        wrapper.instance().onSegmentSelection(null);
        expect(props.clearEndDate.mock.calls).toHaveLength(1);
    });
});
