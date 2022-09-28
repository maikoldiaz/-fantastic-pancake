import setup from '../../../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../../common/services/httpService';
import { navigationService } from '../../../../../../common/services/navigationService';
import NodeProducts from '../../../../../../modules/administration/node/attributes/components/nodeProducts.jsx';
import Grid from '../../../../../../common/components/grid/grid.jsx';

function mountWithRealStore() {
    const dataGrid = {
        categories: {
            config: {
                apiUrl: ''
            },
            pageFilters: {}
        },
        category: {}
    };

    const grid = { nodeProducts: dataGrid.categories, ownershipRules: {} };

    const reducers = {
        onEdit: jest.fn(() => Promise.resolve()),
        category: jest.fn(() => dataGrid.category),
        grid: jest.fn(() => grid),
        onClose: jest.fn(() => Promise.resolve()),
        ownershipRules: jest.fn(() => grid.ownershipRules)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <NodeProducts name="nodeProducts" />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('NodeProducts', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.getParamByName = jest.fn();
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
});
