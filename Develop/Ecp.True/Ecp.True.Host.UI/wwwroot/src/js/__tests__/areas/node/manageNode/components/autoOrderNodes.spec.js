import setup from '../../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import AutoOrderNodes from '../../../../../modules/administration/node/manageNode/components/autoOrderNodes';
import { ModalFooter } from '../../../../../common/components/footer/modalFooter.jsx';

function mountWithRealStore() {
    const data = {
        initialValues: {
            name: "test"
        },
        node: {
            manageNode: {
                existingNode: {
                    name: "someName"
                }
            }
        }
    };

    const nodeGraphicalConnection = {
        showCreateNodePanel: false
    };

    const reducers = {
        node: jest.fn(() => data.node),
        nodeGraphicalConnection: jest.fn(() => nodeGraphicalConnection),
        onClose: jest.fn(() => Promise.resolve())
    };

    const props = {
        closeModal: jest.fn(() => Promise.resolve),
        saveNode: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn()
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <AutoOrderNodes initialValues={data.initialValues} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Auto Order Nodes', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('find Controls', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_autoOrderNodes_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find(ModalFooter).find('#btn_autoOrderNodes_submit').text()).toEqual('accept');
    });

    it('should call onClose on click of cancel button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find(ModalFooter).find('#btn_autoOrderNodes_cancel').at(0).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });
});