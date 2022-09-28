import setup from '../../../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../../common/services/httpService';
import { navigationService } from '../../../../../../common/services/navigationService';
import NodeDetails from '../../../../../../modules/administration/node/attributes/components/nodeDetails.jsx';
import Grid from '../../../../../../common/components/grid/grid.jsx';

function mountWithRealStore() {
    const dataGrid = {
        categories: {
            config: {
                apiUrl: ''
            },
            pageFilters: {}
        },
        categoryElementFilter: {
            connAttributes: {}
        },
        shared: {
            categories: [{
                categoryId: 1,
                name: 'Segment',
                description: 'Segment',
                isActive: false,
                isGrouper: true
            }, {
                categoryId: 2,
                name: 'Operator',
                description: 'Operator',
                isActive: true,
                isGrouper: true
            }]
        },
        flyout: {
            connAttributes: {
                isOpen: false
            }
        },
        node: {
            attributes: {
                node: {
                    controlLimit: 'test',
                    isTransfer: true,
                    sourceNode: {},
                    destinationNode: {}
                }
            }
        }
    };

    const initialProps = {
        onEdit: jest.fn(() => Promise.resolve()),
        hideEdit: false
    };

    const grid = { nodeProducts: dataGrid.categories, ownershipRules: {} };

    const reducers = {
        categoryElementFilter: jest.fn(() => dataGrid.categoryElementFilter),
        shared: jest.fn(() => dataGrid.shared),
        flyout: jest.fn(() => dataGrid.flyout),
        node: jest.fn(() => dataGrid.node),
        grid: jest.fn(() => grid),
        onClose: jest.fn(() => Promise.resolve()),
        ownershipRules: jest.fn(() => grid.ownershipRules)
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true });
    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <NodeDetails name="nodeProducts" />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('NodeDetails', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.getParamByName = jest.fn(() => 'nodeId');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('nodeProducts');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(7);
    });

    it('should have connection and controlLimit controls', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#lbl_nodeAttributes_name')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_nodeAttributes_controlLimit')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_nodeAttributes_acceptableBalancePercentage')).toBe(true);
        expect(enzymeWrapper.exists('#lnk_nodeAttributes_acceptableBalance_edit')).toBe(true);
    });
});
