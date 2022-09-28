import setup from '../../../../../js/__tests__/areas/setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import ConfirmModal, { ConfirmModal as ConfirmModalComponent } from '../../../../common/components/modal/confirmModal.jsx';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';
import { closeConfirm, closeModal } from '../../../../common/actions';

const initialValues = {
    confirmModal: {
        isOpen: true,
        cancelButtonText: 'cancel',
        shouldShowCancelButton: true,
        title: 'TestTitle'
    }
};

function mountWithRealStore(newProps = {}) {
    const reducers = {
        confirmModal: jest.fn(() => Object.assign({}, initialValues.confirmModal, newProps))
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        handleClose: jest.fn(),
        onClose: jest.fn(),
        onCancel: jest.fn()
    };


    const enzymeWrapper = mount(<Provider store={store}>
        <ConfirmModal onConfirm={jest.fn()} {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('ConfirmModal', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore(initialValues);
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('find controls', () => {
        const { enzymeWrapper } = mountWithRealStore(initialValues);
        expect(enzymeWrapper.find('#btn_confirm_cancel').exists()).toEqual(true);
        expect(enzymeWrapper.find('#h1_confirm_title').exists()).toEqual(true);
        expect(enzymeWrapper.find('#lbl_confirm_close').exists()).toEqual(true);
        expect(enzymeWrapper.find('#cont_confirm_message').exists()).toEqual(true);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_confirm_submit').exists()).toEqual(true);
        expect(enzymeWrapper.find('#cont_confirm_modal').exists()).toEqual(true);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_confirm_submit').text()).toEqual('accept');
        expect(enzymeWrapper.find(ModalFooter).find('#btn_confirm_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find('#h1_confirm_title').text()).toEqual('TestTitle');
    });

    it('should call onClose on click of title', () => {
        const { enzymeWrapper } = mountWithRealStore(initialValues);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_confirm_cancel').exists()).toEqual(true);
        enzymeWrapper.find(ModalFooter).find('#btn_confirm_cancel').simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call onCancel on click of cancel button', () => {
        const props = {
            handleClose: jest.fn(),
            onClose: jest.fn(),
            onCancel: jest.fn(),
            confirmModal: {
                isOpen: true,
                cancelButtonText: 'cancel',
                shouldShowCancelButton: true,
                title: 'TestTitle'
            }
        };
        const { enzymeWrapper } = mountWithRealStore(props);
        enzymeWrapper.find(ModalFooter).find('#btn_confirm_cancel').simulate('click');
    });

    it('should call onClose and onCancel on click of accept button', () => {
        const props = {
            handleClose: jest.fn(),
            onClose: jest.fn(),
            onCancel: jest.fn(),
            onConfirm: jest.fn(() => Promise.resolve()),
            confirmModal: {
                isOpen: true,
                cancelButtonText: 'Cancel',
                shouldShowCancelButton: true,
                title: 'testTitle',
                data: 'testData'
            }
        };
        const { enzymeWrapper } = mountWithRealStore(props);
        enzymeWrapper.find(ModalFooter).find('#btn_confirm_submit').simulate('click');
    });
});
