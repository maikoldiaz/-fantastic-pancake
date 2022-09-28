import setup from '../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';
import OwnershipNodeDataGrid, { OwnershipNodeDataGrid as OwnershipNodeDataGridComponent } from '../../../../modules/transportBalance/nodeOwnership/components/ownershipNodeDataGrid.jsx';
import { constants } from '../../../../common/services/constants';

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
                product: {},
                variableType: null,
                owner: null
            },
            movementInventoryfilterToggler: false
        }
    }
};

const grid = { ownershipNodeData: initialValues.nodeOwnership };

function mountWithRealStore() {
    const state = {
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
        },
        shared: {
            variableTypes: [{ variableTypeId: 1, name: 'Test' }],
            groupedCategoryElements: [
                [
                    {
                        elementId: 31,
                        name: 'Bbl'
                    }
                ]],
            movementTypes: [{}],
            reasonforChange: [{}]
        },
        root: {
            context: {
                userId: 1
            }
        },
        grid: {
            ownershipNodeBalance: {
                items: []
            }
        }
    };
    const reducers = {
        nodeOwnership: jest.fn(() => initialValues.nodeOwnership),
        grid: jest.fn(() => grid),
        root: jest.fn(() => state.root)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <OwnershipNodeDataGrid initialValues={initialValues} />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('OwnershipNodeDataGrid', () => {
    it('should mount with real store', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('ownershipNodeData');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(0);
    });

    it('should call component did update and load grid data on change of props', () => {
        const props = {
            nodeMovementInventoryDataToggler: false,
            movementInventoryfilters: {
                product: { productId: 1 },
                variableType: {},
                owner: {}
            },
            ownershipNodeDetails: {},
            nodeMovementInventoryData: [{ status: 'update' }],
            ownershipNodeBalance: [],
            updateOwnershipNodeBalance: jest.fn(),
            loadMovementInventoryGrid: jest.fn()
        };
        const newProps = {
            nodeMovementInventoryDataToggler: true,
            movementInventoryfilters: {
                product: {},
                variableType: {},
                owner: {}
            },
            ownershipNodeDetails: {},
            nodeMovementInventoryData: [],
            ownershipNodeBalance: []
        };
        const wrapper = shallow(<OwnershipNodeDataGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, newProps));
        expect(props.loadMovementInventoryGrid.mock.calls).toHaveLength(1);
    });
    it('should call component did update and load grid data on change of props for variable type entradas', () => {
        const props = {
            nodeMovementInventoryDataToggler: false,
            movementInventoryfilters: {
                product: { destinationProductId: 1, destinationNodeId: 3 },
                variableType: constants.VariableType.Input,
                owner: {}
            },
            ownershipNodeDetails: {},
            nodeMovementInventoryData: [{ status: 'update' }],
            ownershipNodeBalance: [],
            updateOwnershipNodeBalance: jest.fn(),
            loadMovementInventoryGrid: jest.fn()
        };
        const newProps = {
            nodeMovementInventoryDataToggler: true,
            movementInventoryfilters: {
                product: {},
                variableType: {},
                owner: {}
            },
            ownershipNodeDetails: {},
            nodeMovementInventoryData: [],
            ownershipNodeBalance: []
        };
        const wrapper = shallow(<OwnershipNodeDataGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, newProps));
        expect(props.loadMovementInventoryGrid.mock.calls).toHaveLength(1);
    });
    it('should call component did update and load grid data on change of props for variable type salida', () => {
        const props = {
            nodeMovementInventoryDataToggler: false,
            movementInventoryfilters: {
                product: { sourceProductId: 1, sourceNodeId: 3 },
                variableType: constants.VariableType.Output,
                owner: {}
            },
            ownershipNodeDetails: {},
            nodeMovementInventoryData: [{ status: 'update' }],
            ownershipNodeBalance: [],
            updateOwnershipNodeBalance: jest.fn(),
            loadMovementInventoryGrid: jest.fn()
        };
        const newProps = {
            nodeMovementInventoryDataToggler: true,
            movementInventoryfilters: {
                product: {},
                variableType: {},
                owner: {}
            },
            ownershipNodeDetails: {},
            nodeMovementInventoryData: [],
            ownershipNodeBalance: []
        };
        const wrapper = shallow(<OwnershipNodeDataGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, newProps));
        expect(props.loadMovementInventoryGrid.mock.calls).toHaveLength(1);
    });
});
