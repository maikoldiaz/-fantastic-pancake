import setup from '../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import OwnershipCalculationConfirmation from './../../../../modules/transportBalance/ownership/components/ownershipCalculationConfirmation.jsx';
import { constants } from '../../../../common/services/constants';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';

const initialValue = {
    ownership: {
        ticket: {
            categoryElementId: 1,
            startDate: '',
            endDate: '',
            ticketTypeId: constants.TicketType.Ownership
        }
    },
    refreshToggler: false
};

function mountWithRealStore() {
    const reducers = {
        ownership: jest.fn(() => initialValue.ownership),
        refreshToggler: false
    };

    const props = {
        closeModal: jest.fn(() => Promise.resolve),
        refreshGrid: jest.fn(()=> Promise.resolve),
        executeOwnershipCalculation: jest.fn(()=> Promise.resolve),
        refreshToggler: false,
        showLoader: jest.fn(),
        showError: jest.fn()
    };

    const store = createStore(combineReducers(reducers));
    const enzymeWrapper = mount(<Provider store={store} >
        <OwnershipCalculationConfirmation {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Ownership Calculation Confirmation', () => {
    beforeAll(() => {

    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should update successfully', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        const newProps = Object.assign({}, initialValue, {
            refreshToggler: false
        });
        enzymeWrapper.instance().componentDidUpdate(newProps);
        expect(props.closeModal.mock.calls).toHaveLength(0);
        expect(props.refreshGrid.mock.calls).toHaveLength(0);
    });

    it('should handle selected segment change', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper.exists('#btn_ownershipCalculationConfirmation_submit')).toBe(true);
        enzymeWrapper.find(ModalFooter).find('#btn_ownershipCalculationConfirmation_submit').simulate('click');
        expect(props.showLoader.mock.calls).toHaveLength(0);
        expect(props.showError.mock.calls).toHaveLength(0);
    });
});
