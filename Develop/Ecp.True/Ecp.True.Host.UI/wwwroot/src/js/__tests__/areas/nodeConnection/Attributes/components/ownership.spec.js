import setup from '../../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import Ownership from '../../../../../modules/administration/nodeConnection/attributes/components/ownership';

function mountWithRealStore() {
    const data = {
        nodeConnection: {
            attributes: {
                connection: {
                    controlLimit: 'test'
                },
                connectionProduct: {
                    nodeConnectionProductId: '1'
                }
            }
        },
        dualSelect: {
            connectionProducts_owners: {
                target: [
                    {
                        id: 27,
                        name: 'REFICAR',
                        value: 50.50
                    },
                    {
                        id: 30,
                        name: 'ECOPETROL',
                        value: 29.50
                    },
                    {
                        id: 100,
                        name: 'BP LATAM',
                        value: 20.00
                    }]
            }
        }
    };

    const reducers = {
        nodeConnection: jest.fn(() => data.nodeConnection),
        dualSelect: jest.fn(() => data.dualSelect),
        onClose: jest.fn(() => Promise.resolve())
    };

    const props = {
        closeModal: jest.fn(() => Promise.resolve),
        onWizardNext: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn()
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <Ownership {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Ownership', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should have find and cancel buttons', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find('#btn_connectionProducts_ownership_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find('#btn_connectionProducts_ownership_submit').text()).toEqual('submit');
    });

    it('should call onClose on click of cancel button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_connectionProducts_ownership_cancel').at(0).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });
});
