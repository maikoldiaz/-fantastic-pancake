import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import InitOfficialDeltaTicket, { InitOfficialDeltaTicket as InitOfficialDeltaTicketComponent } from '../../../../modules/dailyBalance/officialDelta/components/initialize.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { ticketValidator } from '../../../../modules/dailyBalance/officialDelta/ticketValidationService';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';

const initialValues = {
    shared: {
        groupedCategoryElements: [{
            categoryId: 1,
            elementId: 2,
            isActive: true,
            name: 'UpdatedCategory4gbx1'
        }]
    },
    officialDelta: {
        dateRange: []
    }
};

function mountWithRealStore() {
    const reducers = {
        closeModal: jest.fn(() => Promise.resolve),
        officialDelta: jest.fn(() => initialValues.officialDelta),
        shared: jest.fn(() => initialValues.shared)
    };

    const props = {
        showError: jest.fn(),
        showLoader: jest.fn(),
        hideLoader: jest.fn(),
        setOfficialDeltaTicketInfo: jest.fn()
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} ><InitOfficialDeltaTicket /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('InitOfficialDeltaTicket', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        systemConfigService.getDefaultOfficialDeltaTicketDateRange = jest.fn(() => {
            return 5;
        });
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('find Control', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#dd_initOfficialDeltaTicket_segment')).toBe(true);
        expect(enzymeWrapper.exists('#dd_initOfficialDeltaTicket_period')).toBe(true);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_initOfficialDeltaTicket_submit').text()).toEqual('deltaProcess');
    });

    it('should call component did update on segment change', () => {
        const props = {
            segmentChangeToggler: false,
            resetOfficialPeriods: jest.fn(),
            resetField: jest.fn(),
            getOfficialPeriods: jest.fn(),
            handleSubmit: jest.fn(),
            initOfficialDelta: jest.fn(),
            getCategoryElements: jest.fn(),
            hideError: jest.fn(),
            segment: { elementId: 10 },
            setIsValid: jest.fn()
        };

        const newProps = {
            segmentChangeToggler: true,
            segment: { elementId: 11 }
        };
        const wrapper = shallow(<InitOfficialDeltaTicketComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, newProps));
        expect(props.resetField.mock.calls).toHaveLength(2);
        expect(props.resetOfficialPeriods.mock.calls).toHaveLength(2);
        expect(props.getOfficialPeriods.mock.calls).toHaveLength(1);
        expect(props.hideError.mock.calls).toHaveLength(2);
    });

    it('should reset module name on unmount', () => {
        const props = {
            segmentChangeToggler: false,
            resetOfficialPeriods: jest.fn(),
            resetField: jest.fn(),
            getOfficialPeriods: jest.fn(),
            handleSubmit: jest.fn(),
            initOfficialDelta: jest.fn(),
            getCategoryElements: jest.fn(),
            resetModuleName: jest.fn(),
            hideError: jest.fn(),
            segment: { elementId: 10 },
            setIsValid: jest.fn()
        };
        const wrapper = shallow(<InitOfficialDeltaTicketComponent {...props} />);
        wrapper.unmount();
        expect(props.resetModuleName.mock.calls).toHaveLength(1);
    });

    it('should handle form submit', () => {
        ticketValidator.validateOfficialDeltaInProgress = jest.fn(() => {
            return;
        });
        ticketValidator.validatePreviousOfficialPeriod = jest.fn(() => {
            return;
        });
        const props = {
            segmentChangeToggler: false,
            resetOfficialPeriods: jest.fn(),
            resetField: jest.fn(),
            getOfficialPeriods: jest.fn(),
            handleSubmit: jest.fn(),
            initOfficialDelta: jest.fn(),
            getCategoryElements: jest.fn(),
            hideError: jest.fn(),
            setOfficialDeltaTicketInfo: jest.fn(),
            onNext: jest.fn(),
            segment: { elementId: 10 },
            setIsValid: jest.fn()
        };

        const wrapper = shallow(<InitOfficialDeltaTicketComponent {...props} />);
        wrapper.instance().onSubmit({ segment: { name: 'test', elementId: 10 } });
        expect(props.hideError.mock.calls).toHaveLength(1);
        expect(props.setOfficialDeltaTicketInfo.mock.calls).toHaveLength(0);
        expect(props.onNext.mock.calls).toHaveLength(0);
    });

    it('should show error when disable is called with true', () => {
        const props = {
            segmentChangeToggler: false,
            resetOfficialPeriods: jest.fn(),
            resetField: jest.fn(),
            getOfficialPeriods: jest.fn(),
            handleSubmit: jest.fn(),
            initOfficialDelta: jest.fn(),
            getCategoryElements: jest.fn(),
            resetModuleName: jest.fn(),
            showError: jest.fn(),
            setIsValid: jest.fn(),
            periodChange: jest.fn(),
            hideError: jest.fn(),
            segment: { elementId: 10 }
        };
        const wrapper = shallow(<InitOfficialDeltaTicketComponent {...props} />);
        const noMonths = {
            selectedYear: '2020',
            selectedMonths: []
        };
        wrapper.instance().validate(noMonths);
        expect(props.showError.mock.calls).toHaveLength(1);
        expect(props.setIsValid.mock.calls).toHaveLength(2);
    });

    it('should hideError error when disable is called with false', () => {
        const props = {
            segmentChangeToggler: false,
            resetOfficialPeriods: jest.fn(),
            resetField: jest.fn(),
            getOfficialPeriods: jest.fn(),
            handleSubmit: jest.fn(),
            initOfficialDelta: jest.fn(),
            getCategoryElements: jest.fn(),
            resetModuleName: jest.fn(),
            periodChange: jest.fn(),
            hideError: jest.fn(),
            setIsValid: jest.fn(),
            segment: { elementId: 10 },
            showError: jest.fn()
        };
        const noMonths = {
            selectedYear: '2020',
            selectedMonths: ['JUN']
        };
        const wrapper = shallow(<InitOfficialDeltaTicketComponent {...props} />);
        wrapper.instance().validate(noMonths);
        expect(props.hideError.mock.calls).toHaveLength(2);
        expect(props.setIsValid.mock.calls).toHaveLength(3);
    });
    it('should initialize on mount', () => {
        const props = {
            resetOfficialPeriods: jest.fn(),
            resetField: jest.fn(),
            getOfficialPeriods: jest.fn(),
            handleSubmit: jest.fn(),
            initOfficialDelta: jest.fn(),
            getCategoryElements: jest.fn(),
            resetModuleName: jest.fn(),
            periodChange: jest.fn(),
            hideError: jest.fn(),
            setIsValid: jest.fn(),
            segment: { elementId: 10 },
            showError: jest.fn()
        };
        shallow(<InitOfficialDeltaTicketComponent {...props} />);
        expect(props.initOfficialDelta.mock.calls).toHaveLength(1);
        expect(props.getCategoryElements.mock.calls).toHaveLength(1);
        expect(props.resetOfficialPeriods.mock.calls).toHaveLength(1);
        expect(props.resetField.mock.calls).toHaveLength(1);
        expect(props.hideError.mock.calls).toHaveLength(1);
        expect(props.setIsValid.mock.calls).toHaveLength(1);
    });
});

