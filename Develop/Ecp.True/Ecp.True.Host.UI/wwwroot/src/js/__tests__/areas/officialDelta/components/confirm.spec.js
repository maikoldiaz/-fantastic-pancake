import setup from './../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { httpService } from './../../../../common/services/httpService';
import ConfirmOfficialDelta, { ConfirmOfficialDelta as ConfirmOfficialDeltaComponent } from '../../../../modules/dailyBalance/officialDelta/components/confirm.jsx';
import { navigationService } from '../../../../common/services/navigationService';
import { ticketValidator } from '../../../../modules/dailyBalance/officialDelta/ticketValidationService';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';
import { saveOfficialDelta } from '../../../../modules/dailyBalance/officialDelta/actions';

const initialValues = {
    officialDelta: {
        ticket: {
            startDate: '2019-01-01',
            endDate: '2019-11-12',
            name: 'Segmento'
        }
    }
};

function mountWithRealStore(newState = {}) {
    const reducers = {
        officialDelta: jest.fn(() => Object.assign({}, initialValues.officialDelta, newState))
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        closeModal: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn()
    };


    const enzymeWrapper = mount(<Provider store={store}><ConfirmOfficialDelta {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('ConfirmOfficialDelta', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });
    it('find controls', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_confirmOfficialDeltaTicket_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find(ModalFooter).find('#btn_confirmOfficialDeltaTicket_submit').text()).toEqual('accept');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should cancel add comment when cancel button is clicked ', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find(ModalFooter).find('#btn_confirmOfficialDeltaTicket_cancel').simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call handleSubmit when submit button is clicked', () => {
        const props = {
            receiveToggler: false,
            closeModal: jest.fn(),
            save: jest.fn(),
            ticket: {
                startDate: '2019-01-01',
                endDate: '2019-11-12',
                name: 'Segmento'
            }
        };
        ticketValidator.validateOfficialDeltaInProgress = jest.fn();
        ticketValidator.validatePreviousOfficialPeriod = jest.fn();
        const { enzymeWrapper } = mountWithRealStore(props);
        enzymeWrapper.find('#btn_confirmOfficialDeltaTicket_submit').simulate('click');
        expect(saveOfficialDelta).toHaveLength(1);
    });

    it('should call component did update on save ticket', () => {
        const props = {
            receiveToggler: false,
            closeModal: jest.fn(),
            ticket: {
                startDate: '2019-01-01',
                endDate: '2019-11-12',
                name: 'Segmento'
            }
        };

        navigationService.navigateToModule = jest.fn();
        const newProps = {
            receiveToggler: true
        };
        const wrapper = shallow(<ConfirmOfficialDeltaComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, newProps));
        expect(props.closeModal.mock.calls).toHaveLength(1);
    });
});
