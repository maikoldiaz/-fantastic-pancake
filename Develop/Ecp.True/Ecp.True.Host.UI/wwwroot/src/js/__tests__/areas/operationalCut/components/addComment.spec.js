import setup from './../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { httpService } from './../../../../common/services/httpService';
import AddComment from '../../../../modules/transportBalance/cutOff/components/addComment.jsx';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';

const initialValues = {
    cutoff: {
        operationalCut: {
            name: 'pendingTranscations',
            pendingTranscations: { count: 10, preText: 'Pre-Text', postText: 'Post-Text' }
        }
    },
    controlexception: {
        controlException: {
                pendingTransactionErrors: { count: 10, preText: 'Pre-Text', postText: 'Post-Text'  }
        }
    },
    grid: {
        pendingTransactionErrors: {}
    }
};

function mountWithRealStore(initialValue) {
    const reducers = {
        cutoff: jest.fn(() => initialValue.cutoff),
        controlexception: jest.fn(() => initialValue.controlexception),
        grid: jest.fn(() => initialValues.grid),
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

describe('InitAddComment', () => {
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
        expect(enzymeWrapper.exists('#span_records_count')).toBe(true);
        expect(enzymeWrapper.find('#btn_addComment_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find('#btn_addComment_submit').text()).toEqual('submit');
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
    it('should check when operational cut name not exists in detail', () => {
        const initialValue = Object.assign({}, initialValues,
            {
                cutoff: {
                    operationalCut: {
                        ...initialValues.cutoff.operationalCut,
                        name: 'pendingTranscations1'
                    }
                }
            });
        const { enzymeWrapper } = mountWithRealStore(initialValue);
        expect(enzymeWrapper.find('#span_records_count').text()).toEqual('10');
    });
});
