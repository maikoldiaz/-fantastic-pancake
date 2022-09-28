import setup from './../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { httpService } from './../../../../common/services/httpService';
import ConfirmCutoff from '../../../../modules/transportBalance/cutOff/components/confirmCutoff.jsx';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';

const initialValues = {
    cutoff: {
        operationalCut: {
            ticket: {
                startDate: '2019-01-01',
                endDate: '2019-11-12',
                segment: {
                    name: 'Segmento',
                    category: {
                        name: 'Automation_first'
                    }
                }
            }
        }
    }
};

function mountWithRealStore() {
    const reducers = {
        cutoff: jest.fn(() => initialValues.cutoff)
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        closeModal: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn()
    };


    const enzymeWrapper = mount(<Provider store={store}>
        <ConfirmCutoff {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('ConfirmCutoff', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });
    it('find controls', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_confirmCutoff_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find(ModalFooter).find('#btn_confirmCutoff_submit').text()).toEqual('accept');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should cancel add comment when cancel button is clicked ', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find(ModalFooter).find('#btn_confirmCutoff_cancel').simulate('click');
    });

    it('should call handleSubmit when submit button is clicked', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find('form').simulate('submit');
        expect(props.handleSubmit.mock.calls).toHaveLength(1);
    });
});
