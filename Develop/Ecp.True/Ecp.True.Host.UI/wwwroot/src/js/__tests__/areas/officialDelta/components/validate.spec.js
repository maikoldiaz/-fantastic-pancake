import setup from './../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { httpService } from './../../../../common/services/httpService';
import ValidateOfficialDeltaTicket, { ValidateOfficialDeltaTicket as ValidateOfficialDeltaTicketComponent } from '../../../../modules/dailyBalance/officialDelta/components/validate.jsx';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { openModal } from '../../../../common/actions';

const initialValues = {
    officialDelta: {
        ticket: {
            startDate: '2019-01-01',
            endDate: '2019-11-12',
            name: 'Segmento'
        },
        unapprovedNodes: []
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


    const enzymeWrapper = mount(<Provider store={store}><ValidateOfficialDeltaTicket goToStep={jest.fn()} {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('ValidateOfficialDelta', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });
    it('find controls', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_validateOfficialDeltaTicket_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find(ModalFooter).find('#btn_validateOfficialDeltaTicket_submit').text()).toEqual('next');
    });
    it('should disable next button on records', () => {
        const newProps = {
            ticket: {
                startDate: '2019-01-01',
                endDate: '2019-11-12',
                name: 'Segmento'
            },
            unapprovedNodes: [{ segmentName: 'name', nodeName: 'name', operationalDate: 'date', nodeStatus: 'status' }]
        };
        const { enzymeWrapper } = mountWithRealStore(newProps);
        expect(enzymeWrapper.find(SectionFooter).find('#btn_validateOfficialDeltaTicket_submit').prop('disabled')).toEqual(true);
        expect(enzymeWrapper.find('#tbl_validate_unapprovedNodes')).toHaveLength(1);
    });
    it('should get records for son segment on mount', () => {
        const props = {
            ticket: {
                startDate: '2019-01-01',
                endDate: '2019-11-12',
                name: 'Segmento',
                isSon: true
            },
            unapprovedNodes: [{ segmentName: 'name', nodeName: 'name', operationalDate: 'date', nodeStatus: 'status' }],
            getOfficialDeltaValidationData: jest.fn()
        };
        const wrapper = shallow(<ValidateOfficialDeltaTicketComponent {...props} />);
        expect(props.getOfficialDeltaValidationData.mock.calls).toHaveLength(1);
    });
    it('should enable next button on no records', () => {
        const newProps = {
            ticket: {
                startDate: '2019-01-01',
                endDate: '2019-11-12',
                name: 'Segmento'
            },
            unapprovedNodes: []
        };
        const { enzymeWrapper } = mountWithRealStore(newProps);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_validateOfficialDeltaTicket_submit').prop('disabled')).toEqual(false);
    });
    it('should confirm on click of next', () => {
        const newProps = {
            ticket: {
                startDate: '2019-01-01',
                endDate: '2019-11-12',
                name: 'Segmento'
            },
            unapprovedNodes: [],
            confirm: jest.fn()
        };
        const { enzymeWrapper } = mountWithRealStore(newProps);
        enzymeWrapper.find(ModalFooter).find('#btn_validateOfficialDeltaTicket_submit').simulate('click');
        expect(openModal).toHaveLength(1);
    });
    it('should cancel on click of cancel', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find(ModalFooter).find('#btn_validateOfficialDeltaTicket_cancel').simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });
    it('should disable next button if validation failed', () => {
        const newProps = {
            ticket: {
                startDate: '2019-01-01',
                endDate: '2019-11-12',
                name: 'Segmento'
            },
            unapprovedNodes: [],
            isValid: true
        };
        const { enzymeWrapper } = mountWithRealStore(newProps);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_validateOfficialDeltaTicket_submit').prop('disabled')).toEqual(true);
    });
});
