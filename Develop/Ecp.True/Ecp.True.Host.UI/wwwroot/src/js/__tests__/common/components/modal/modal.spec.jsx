import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { reducer as formReducer } from 'redux-form';
import { mount } from 'enzyme';
import React from 'react';
// eslint-disable-next-line no-unused-vars
import setup from '../../../areas/setup';
import { Modal } from '../../../../common/components/modal/modal';
import { modalService } from '../../../../common/services/modalService.js';

function mountWithRealStore() {
    const modalProps = {
        modal: 
            {
                className: 'someClassName',
                mode: 'someMode',
                message: 'someMessage',
                isOpen: true,
                modalKey: 'some key',
                title: 'some title',
                messageOnly: true,
                canCancel: true
            }
    }

    const state = { modalProps };

    const reducers = {
        modal: jest.fn(() => modalProps.modal)
    };
    const store = createStore(combineReducers(reducers));
    const props = {
        modalState: modalProps.modal,
        closeModal: jest.fn()
    };

    const enzymeWrapper = mount(<Provider store={store}><Modal {...state } {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('modal', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should facilitate cancel', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find(`#btn_confirm_cancel`).simulate('click');
        expect(props.closeModal.mock.calls).toHaveLength(1);
    });

    it('should facilitate accept', () => {
        const modalProps = {
            modal: 
                {
                    className: 'someClassName',
                    mode: 'someMode',
                    message: 'someMessage',
                    isOpen: true,
                    modalKey: 'some key',
                    title: 'some title',
                    messageOnly: true,
                    canCancel: false,
                    acceptAction: true
                }
        }
    
        const state = { modalProps };
    
        const reducers = {
            modal: jest.fn(() => modalProps.modal)
        };
        const store = createStore(combineReducers(reducers));
        let acceptAction;
        const props = {
            modalState: modalProps.modal,
            closeModal: jest.fn(),
            fireAction: jest.fn((action) => { acceptAction = action; })
        };
        const wrapper = mount(<Provider store={store}><Modal {...state } {...props} /></Provider>);
        wrapper.find(`#btn_confirm_accept`).simulate('click');
        expect(props.fireAction.mock.calls).toHaveLength(1);
        expect(acceptAction).toBe(true);
    });

    it('should facilitate close modal', () => {
        const modalProps = {
            modal: 
                {
                    className: 'someClassName',
                    mode: 'someMode',
                    message: 'someMessage',
                    isOpen: true,
                    modalKey: 'some key',
                    title: 'some title',
                    messageOnly: true,
                    canCancel: false,
                    acceptAction: true,
                    closeAction: true
                }
        }
    
        const state = { modalProps };
    
        const reducers = {
            modal: jest.fn(() => modalProps.modal)
        };
        const store = createStore(combineReducers(reducers));
        let closeAction;
        const props = {
            modalState: modalProps.modal,
            closeModal: jest.fn(),
            fireAction: jest.fn((action) => { closeAction = action; })
        };
        const wrapper = mount(<Provider store={store}><Modal {...state } {...props} /></Provider>);
        wrapper.find(`#lbl_modal_close`).simulate('click');
        expect(props.fireAction.mock.calls).toHaveLength(1);
        expect(props.closeModal.mock.calls).toHaveLength(1);
        expect(closeAction).toBe(true);
    });
});
