import setup from '../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import OwnershipCalculationValidations
    from '../../../../modules/transportBalance/ownership/components/ownershipCalculationValidations.jsx';
import { constants } from '../../../../common/services/constants';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';
import { wizardNextStep } from '../../../../common/actions';

const initialValue = {
    ownership: {
        ticket: {
            categoryElementId: 1,
            startDate: '01-01-2020',
            endDate: '01-02-2020',
            ticketTypeId: constants.TicketType.Ownership,
            categoryElementName: 'categoryElementName'
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
        onNext: jest.fn(() => Promise.resolve),
        wizardNextStep: jest.fn(() => Promise.resolve),
        executeOwnershipCalculation: jest.fn(()=> Promise.resolve),
        refreshToggler: false
    };

    const store = createStore(combineReducers(reducers));
    const enzymeWrapper = mount(<Provider store={store} >
        <OwnershipCalculationValidations config={{ wizardName: 'name' }} {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Ownership Calculation Validations', () => {
    beforeAll(() => {

    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should facilitate button click', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find(ModalFooter).find('#btn_ownershipCalculationValidations_submit').simulate('click');
        expect(wizardNextStep).toHaveLength(1);
    });

    it('should close modal on cancel button click', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find(ModalFooter).find('#btn_ownershipCalculationValidations_cancel').simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should find all the fields', () =>{
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('.ep-modal__content')).toBe(true);
        expect(enzymeWrapper.exists('.ep-validation')).toBe(true);
        expect(enzymeWrapper.exists('.ep-modal__footer')).toBe(true);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_ownershipCalculationValidations_cancel').text()).toEqual('Cancelar');
        expect(enzymeWrapper.find(ModalFooter).find('#btn_ownershipCalculationValidations_submit').text()).toEqual('Siguiente');
    });

    it('should give the section name', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('.ep-modal__content')).toBe(true);
        expect(enzymeWrapper.find('.ep-modal__content').text()).
            toEqual('Balance Volumétrico con Propiedad para el Segmento categoryElementName Período: 2020-01-01T00:00:00Z al 2020-01-02T00:00:00ZElementos para Ejecutar el Balance');
    });

    it('should give the header name', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find('.ep-validation__header').text()).toEqual('Elementos para Ejecutar el Balance');
    });
});
