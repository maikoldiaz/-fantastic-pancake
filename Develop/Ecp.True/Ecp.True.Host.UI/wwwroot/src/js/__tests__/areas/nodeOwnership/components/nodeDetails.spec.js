import setup from '../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import NodeDetails, { NodeDetails as NodeDetailsComponent } from '../../../../modules/transportBalance/nodeOwnership/components/nodeDetails.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import { constants } from '../../../../common/services/constants';
import { dataService } from '../../../../modules/transportBalance/nodeOwnership/services/dataService.js';

const initialValues = {
    nodeOwnership: {
        ownershipNode: {
            movementInventoryOwnershipData: [],
            updateOwnershipDataToggler: true,
            refreshToggler: false,
            config: {
                apiUrl: ''
            },
            pageFilters: {}
        }
    }
};

const state = {
    nodeOwnership: {
        ownershipNode: {
            nodeDetails: {
                nodeId: 1,
                ownershipStatus: 1,
                node: {
                    name: 'Automation'
                },
                ticket: {
                    endDate: '01/01/2020'
                }
            },
            nodeMovementInventoryData: [],
            node: {
                ownershipNodeId: 1,
                node: {
                    name: 'Automation'
                }
            }
        },
        ownershipNodeDetails: {
            movementInventoryfilters: {
                product: {},
                owner: {},
                variableType: {}
            },
            ownershipStatus: 1
        }
    },
    shared: {
        variableTypes: [{ variableTypeId: 1, name: 'Test' }, { variableTypeId: constants.VariableType.InitialInventory, name: 'Test1' }],
        categoryElements: [{
            categoryId: constants.Category.Owner,
            name: 'ECOPETROL'
        },
        {
            categoryId: constants.Category.Owner,
            name: 'EQUION'
        }
        ]
    },
    root: {
        context: {
            userId: 1
        }
    },
    grid: {
        ownershipNodeBalance: {
            items: [{
                productId: 1,
                product: {
                    productId: 1,
                    name: 'Test'
                }
            }]
        },
        ownershipNodeData: initialValues.nodeOwnership.ownershipNode
    }
};

function mountWithRealStore() {
    const props = {
        initUpdateFilters: jest.fn(),
        createMovement: jest.fn(),
        movementInventoryfilters: {
            product: {},
            owner: {},
            variableType: {}
        }
    };

    const reducers = {
        shared: jest.fn(() => state.shared),
        root: jest.fn(() => state.root),
        grid: jest.fn(() => state.grid),
        nodeOwnership: jest.fn(() => state.nodeOwnership)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <NodeDetails ownershipNodeId="1" name="ownershipNodeData" initialValues={initialValues} {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('node details', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        dataService.buildNodeDetailsFilter = jest.fn(() => []);
        dataService.compareStatus = jest.fn(() => true);
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should create movement on button click', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find('#btn_editOwnershipNode_newMovement').first().simulate('click');
        expect(props.createMovement.mock.calls).toHaveLength(1);
    });
    it('should mount relevant Fields', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#lbl_editOwnershipNode_product')).toBe(true);
        expect(enzymeWrapper.exists('#dd_editOwnershipNode_product')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_editOwnershipNode_variable')).toBe(true);
        expect(enzymeWrapper.exists('#dd_editOwnershipNode_variable')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_editOwnershipNode_owner')).toBe(true);
        expect(enzymeWrapper.exists('#dd_editOwnershipNode_owner')).toBe(true);
        expect(enzymeWrapper.find('#btn_editOwnershipNode_newMovement').text()).toEqual('newMovement');
    });

    it('should call component did update and initUpdateFilters on change of variable type props to set the default variable type', () => {
        const props = {
            getCategoryElements: jest.fn(),
            getVariableTypes: jest.fn(),
            initUpdateFilters: jest.fn(),
            getMovementInventoryData: jest.fn(),
            clearMovementInventoryFilter: jest.fn(),
            resetPageIndex: jest.fn(),
            ownershipNode: {
                nodeDetails: {
                    ticket: {
                        endDate: '09/01/2020'
                    }
                }
            },
            variableTypes: [],
            categoryElements: state.shared.categoryElements,
            ownershipNodeBalance: state.grid.ownershipNodeBalance,
            ownershipNodeDetails: {
                ownershipStatus: 1
            },
            movementInventoryfilters: {}
        };

        const newProps = {
            variableTypes: state.shared.variableTypes
        };
        const wrapper = shallow(<NodeDetailsComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, newProps));
        expect(props.initUpdateFilters).toHaveBeenCalled();
        expect(props.resetPageIndex.mock.calls).toHaveLength(2);
    });

    it('should call component did update and initUpdateFilters on change of category element props to set the default owner category', () => {
        const props = {
            getCategoryElements: jest.fn(),
            getVariableTypes: jest.fn(),
            initUpdateFilters: jest.fn(),
            getMovementInventoryData: jest.fn(),
            clearMovementInventoryFilter: jest.fn(),
            resetPageIndex: jest.fn(),
            ownershipNodeDetails: {
                ownershipStatus: 1
            },
            ownershipNode: {
                nodeDetails: {
                    ticket: {
                        endDate: '09/01/2020'
                    }
                }
            },
            movementInventoryfilters: {},
            variableTypes: [],
            categoryElements: [],
            ownershipNodeBalance: state.grid.ownershipNodeBalance
        };

        const newProps = {
            categoryElements: state.shared.categoryElements
        };
        const wrapper = shallow(<NodeDetailsComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, newProps));
        expect(props.initUpdateFilters).toHaveBeenCalled();
        expect(props.resetPageIndex.mock.calls).toHaveLength(1);
    });

    it('should call component did update and initUpdateFilters on change of ownership node balance props to set the default product', () => {
        const props = {
            getCategoryElements: jest.fn(),
            getVariableTypes: jest.fn(),
            initUpdateFilters: jest.fn(),
            getMovementInventoryData: jest.fn(),
            clearMovementInventoryFilter: jest.fn(),
            resetPageIndex: jest.fn(),
            ownershipNodeDetails: {
                ownershipStatus: 1
            },
            ownershipNode: {
                nodeDetails: {
                    ticket: {
                        endDate: '09/01/2020'
                    }
                }
            },
            movementInventoryfilters: {},
            variableTypes: [],
            categoryElements: state.shared.categoryElements,
            ownershipNodeBalance: state.grid.ownershipNodeBalance.items
        };

        const newProps = {
            movementInventoryfilters: {}
        };
        const wrapper = shallow(<NodeDetailsComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, newProps));
        expect(props.initUpdateFilters).toHaveBeenCalled();
        expect(props.resetPageIndex.mock.calls).toHaveLength(2);
    });

    it('should enable create movement button according to specific ownership status', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find('#btn_editOwnershipNode_newMovement').prop('disabled')).toBe(false);
    });

    it('should disable create movement button according to specific ownership status', () => {
        dataService.compareStatus = jest.fn(() => false);
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find('#btn_editOwnershipNode_newMovement').prop('disabled')).toBe(true);
    });

    it('should get category elements, variable types and movement inventory data on componentDidMount', () => {
        const props = {
            getCategoryElements: jest.fn(),
            getVariableTypes: jest.fn(),
            initUpdateFilters: jest.fn(),
            getMovementInventoryData: jest.fn(),
            clearMovementInventoryFilter: jest.fn(),
            ownershipNodeDetails: {
                ownershipStatus: 1
            },
            ownershipNode: {
                nodeDetails: {
                    ticket: {
                        endDate: '09/01/2020'
                    }
                }
            },
            movementInventoryfilters: {},
            variableTypes: null,
            categoryElements: state.shared.categoryElements,
            ownershipNodeBalance: state.grid.ownershipNodeBalance
        };

        const wrapper = shallow(<NodeDetailsComponent {...props} />);
        wrapper.instance().componentDidMount();
        expect(props.getCategoryElements.mock.calls).toHaveLength(2);
        expect(props.getVariableTypes.mock.calls).toHaveLength(2);
        expect(props.getMovementInventoryData.mock.calls).toHaveLength(2);
    });

    it('should set the product dropdown to null if the ownership changes are reverted.', () => {
        const props = {
            getCategoryElements: jest.fn(),
            getVariableTypes: jest.fn(),
            initUpdateFilters: jest.fn(),
            getMovementInventoryData: jest.fn(),
            resetPageIndex: jest.fn(),
            ownershipNodeDetails: {
                ownershipStatus: 1
            },
            ownershipNode: {
                nodeDetails: {
                    ticket: {
                        endDate: '09/01/2020'
                    }
                }
            },
            movementInventoryfilters: {},
            variableTypes: [],
            ownershipNodeBalance: [
                {
                    productId: '123'
                }
            ],
            categoryElements: state.shared.categoryElements,
            ownershipNodeBalance: state.grid.ownershipNodeBalance.items
        };

        const newProps = {
            ownershipNodeBalance: []
        };
        const wrapper = shallow(<NodeDetailsComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, newProps));
        expect(props.initUpdateFilters.mock.calls).toHaveLength(1);
        expect(props.resetPageIndex.mock.calls).toHaveLength(1);
    });
});
