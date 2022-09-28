import setup from '../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import OwnershipMovementInventoryGrid,
{ OwnershipMovementInventoryGrid as OwnershipMovementInventoryGridComponent } from '../../../../modules/transportBalance/nodeOwnership/components/ownershipMovementInventoryGrid.jsx';
const initialValues = {
    nodeOwnership: {
        config: {
            apiUrl: ''
        },
        ownershipNode: {
            movementInventoryOwnershipData: [],
            updateOwnershipDataToggler: true,
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

const grid = { ownershipMovementInventoryGrid: initialValues.nodeOwnership };

function mountWithRealStore() {
    const reducers = {
        nodeOwnership: jest.fn(() => initialValues.nodeOwnership),
        grid: jest.fn(() => grid)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <OwnershipMovementInventoryGrid initialValues={initialValues} />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('OwnershipMovementInventoryGrid', () => {
    it('should mount with real store', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('ownershipMovementInventoryGrid');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(4);
    });

    it('should call component did update on change of props', () => {
        const props = {
            movementInventoryOwnershipData: [],
            updateOwnershipDataToggler: true,
            loadGridData: jest.fn()
        };
        const newProps = {
            movementInventoryOwnershipData: [],
            updateOwnershipDataToggler: false
        };
        const wrapper = shallow(<OwnershipMovementInventoryGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, newProps));
        expect(props.loadGridData.mock.calls).toHaveLength(2);
    });
});
