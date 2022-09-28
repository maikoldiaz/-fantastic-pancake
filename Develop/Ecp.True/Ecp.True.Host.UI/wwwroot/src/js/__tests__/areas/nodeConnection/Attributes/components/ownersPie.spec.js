import setup from '../../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import OwnersPie from '../../../../../modules/administration/nodeConnection/attributes/components/ownersPie';

function mountWithRealStore() {
    const data = {
        category: {
            manageCategory: {
                category: {},
                isActive: true,
                isGrouper: true,
                refreshToggler: false
            }
        },
        nodeConnection: {
            attributes: {
                connection: {
                    controlLimit: 'test'
                },
                connectionProduct: {
                    nodeConnectionProductId: '1',
                    owners: [{
                        ownerId: 56,
                        ownershipPercentage: 50,
                        owner: {
                            name: 'testName'
                        }
                    }]
                }
            }
        },
        dualSelect: {
            target: {}
        }
    };

    const reducers = {
        nodeConnection: jest.fn(() => data.nodeConnection),
        dualSelect: jest.fn(() => data.dualSelect),
        onClose: jest.fn(() => Promise.resolve())
    };

    const props = {
        handleSubmit: jest.fn()
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <OwnersPie {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('OwnersPie', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });
});
