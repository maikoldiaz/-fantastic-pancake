import setup from '../../../../../js/__tests__/areas/setup'
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { httpService } from './../../../../common/services/httpService';
import AddComment from '../../../../common/components/modal/addComment.jsx' ;
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';

const initialValues = {
    addComment: {
        name: ''
    }
};

function mountWithRealStore(initialValues) {
    const reducers = {
        addComment: jest.fn(() => Promise.resolve)
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        closeModal: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn()
    };


    const enzymeWrapper = mount(<Provider store={store}>
        <AddComment {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('AddComment', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore(initialValues);
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('find controls', () => {
        const { enzymeWrapper } = mountWithRealStore(initialValues);
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#txt_addComment_comment')).toBe(true);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_addComment_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find(ModalFooter).find('#btn_addComment_submit').text()).toEqual('submit');
    });

    it('should cancel add comment when cancel button is clicked ', () => {
        const { enzymeWrapper } = mountWithRealStore(initialValues);
        enzymeWrapper.find(ModalFooter).find('#btn_addComment_cancel').simulate('click');
    });

    it('should call handleSubmit when submit button is clicked', () => {
        const { enzymeWrapper, props } = mountWithRealStore(initialValues);
        enzymeWrapper.find('form').simulate('submit');
        expect(props.handleSubmit.mock.calls).toHaveLength(1);
    });

    it('should unmount component', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.unmount();
        expect(props.closeModal.mock.calls).toHaveLength(0);
    });
});