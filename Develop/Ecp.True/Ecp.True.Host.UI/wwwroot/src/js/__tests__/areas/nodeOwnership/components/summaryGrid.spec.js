import setup from '../../setup.js';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { navigationService } from '../../../../common/services/navigationService';
import Grid from '../../../../common/components/grid/grid.jsx';
import SummaryGrid from '../../../../modules/transportBalance/nodeOwnership/components/summaryGrid.jsx';

const initialValues = {
    nodeOwnership: {
        config: {
            apiUrl: ''
        },
        ownershipNode: {
            nodeDetails: {},
            refreshToggler: false,
            pageFilters: {}
        },
        ownershipNodeDetails: {
            movementInventoryfilters: {
                product: null,
                variableType: null,
                owner: null
            },
            movementInventoryfilterToggler: false
        }
    }
};

const grid = { ownershipNodeBalance: initialValues.nodeOwnership };

function mountWithRealStore() {
    const reducers = {
        nodeOwnership: jest.fn(() => initialValues.nodeOwnership),
        grid: jest.fn(() => grid)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <SummaryGrid initialValues={initialValues} />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('SummaryGrid', () => {
    beforeAll(() => {
        navigationService.getParamByName = jest.fn(() => 'ownershipNodeId');
    });

    it('should mount with real store', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('ownershipNodeBalance');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(13);
    });
});
