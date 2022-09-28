import setup from '../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import CreateMovement, { CreateMovement as CreateMovementComponent } from '../../../../modules/transportBalance/nodeOwnership/components/createMovement.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import { constants } from '../../../../common/services/constants';
import { dataService } from '.../../../../modules/transportBalance/nodeOwnership/services/dataService.js';
import { utilities } from '../../../../common/services/utilities.js';
import { movementValidator } from '../../../../modules/transportBalance/nodeOwnership/services/movementValidator.js';
import { ownershipNode } from '../../../../modules/transportBalance/nodeOwnership/reducers.js';

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

const grid = { ownershipMovementInventoryGrid: initialValues.nodeOwnership.ownershipNode };


function mountWithRealStore() {
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
                movementInventoryOwnershipData: [{ ownerId: 1, ownerName: 'ECOPETROL', ownershipVolume: 0, ownershipPercentage: 100 }],
                node: {
                    ownershipNodeId: 1,
                    node: {
                        name: 'Automation'
                    }
                },
                initialValues: {
                    movementDate: '01/01/2020'
                },
                selectedData: {
                    sourceNodes: { nodeId: 5 },
                    destinationNodes: { nodeId: 6 },
                    sourceProduct: { productId: 4 },
                    variable: { variableTypeId: 5 }
                },
                selectedField: 'test'
            }
        },
        shared: {
            variableTypes: [{ variableTypeId: 1, name: 'Test' }],
            groupedCategoryElements: {
                [constants.Category.UnitMeasurement]: [{
                    elementId: 31,
                    name: 'Bbl'
                }],
                [constants.Category.MovementType]: [{
                    elementId: 31,
                    name: 'Movement1'
                }],
                [constants.Category.ReasonForChange]: [{
                    elementId: 31,
                    name: 'Reason1'
                }]
            }
        },
        root: {
            context: {
                userId: 1
            }
        }
    };

    const initialProps = {
        handleSubmit: jest.fn(),
        closeModal: jest.fn(() => Promise.resolve)
    };

    const reducers = {
        movementInventoryOwnershipData: jest.fn(() => state.nodeOwnership.ownershipNode.movementInventoryOwnershipData),
        shared: jest.fn(() => state.shared),
        root: jest.fn(() => state.root),
        grid: jest.fn(() => grid),
        nodeOwnership: jest.fn(() => state.nodeOwnership)
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true, clearSelectedData: jest.fn(), clearOwnershipData: jest.fn() });

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <CreateMovement name="ownershipMovementInventoryGrid" initialValues={initialValues} {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('create movement', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        utilities.isArrayNotEmpty = jest.fn(() => true);
        dataService.parseToISOString = jest.fn(() => '01/01/2020');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });
    it('should call handleSubmit method when form submits', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find('form').simulate('submit');
        expect(props.handleSubmit.mock.calls).toHaveLength(1);
    });
    it('should unmount component', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.unmount();
        expect(props.clearSelectedData.mock.calls).toHaveLength(1);
        expect(props.clearOwnershipData.mock.calls).toHaveLength(1);
    });
    it('should mount relevant Fields', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#dt_createMovement_date')).toBe(true);
        expect(enzymeWrapper.exists('#txt_decimal_netVolume')).toBe(true);
        expect(enzymeWrapper.exists('#dd_createMovement_unit')).toBe(true);
        expect(enzymeWrapper.exists('#dd_createMovement_variable')).toBe(true);
        expect(enzymeWrapper.exists('#dd_createMovement_sourceNode')).toBe(true);
        expect(enzymeWrapper.exists('#dd_createMovement_destinationNode')).toBe(true);
        expect(enzymeWrapper.exists('#dd_createMovement_sourceProduct')).toBe(true);
        expect(enzymeWrapper.exists('#dd_createMovement_destinationProduct')).toBe(true);
        expect(enzymeWrapper.exists('#dd_createMovement_movementType')).toBe(true);
        expect(enzymeWrapper.exists('#dd_createMovement_contract')).toBe(true);
        expect(enzymeWrapper.exists('#dd_createMovement_reasonForChange')).toBe(true);
        expect(enzymeWrapper.exists('#txt_createMovement_comments')).toBe(true);
        expect(enzymeWrapper.find('#btn_createMovement_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find('#btn_createMovement_submit').text()).toEqual('accept');
    });

    it('should call component did update and getOwnersForMovement on change of props', () => {
        const props = {
            setContracts: jest.fn(),
            setDate: jest.fn(),
            handleSubmit: jest.fn(),
            getOwnersForMovement: jest.fn(),
            getContractData: jest.fn(),
            selectedData: {
                sourceNodes: { nodeId: 5 },
                destinationNodes: { nodeId: 6 },
                sourceProduct: { productId: 4 },
                variable: { variableTypeId: 5 },
                movementType: { elementId: constants.MovementType.Purchase }
            },
            selectedField: 'sourceNodes',
            ownershipNode: {
                selectedDataToggler: true,
                movementOwnersDataToggler: true,
                inventoryOwnersDataToggler: false,
                nodeDetails: {
                    ticket: {
                        endDate: '09/01/2020'
                    }
                }
            }
        };

        const newProps = {
            ownershipNode: {
                selectedDataToggler: false,
                movementOwnersDataToggler: false,
                inventoryOwnersDataToggler: false,
                nodeDetails: {
                    ticket: {
                        endDate: '09/01/2020'
                    }
                }
            }
        };
        const wrapper = shallow(<CreateMovementComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, newProps));
        expect(props.getOwnersForMovement.mock.calls).toHaveLength(1);
        expect(props.getContractData.mock.calls).toHaveLength(1);
    });

    it('should call component did update and dispatch a list of actions on change of selected dropdown fields value in props', () => {
        dataService.getDispatchActions = jest.fn(() => ['action1']);
        const props = {
            setContracts: jest.fn(),
            updateSelectedData: jest.fn(),
            setDate: jest.fn(),
            handleSubmit: jest.fn(),
            getOwnersForMovement: jest.fn(),
            getContractData: jest.fn(),
            dispatchActions: jest.fn(),
            clearOwnershipData: jest.fn(),
            selectedData: {
                sourceNodes: { nodeId: 5 },
                destinationNodes: { nodeId: 6 },
                sourceProduct: { productId: 4 },
                variable: { variableTypeId: 5 }
            },
            selectedField: 'sourceNodes',
            ownershipNode: {
                nodeDetails: {
                    nodeId: 1,
                    node: {
                        name: 'someName'
                    }
                },
                selectedDataToggler: true,
                movementOwnersDataToggler: true,
                inventoryOwnersDataToggler: false
            }
        };

        const newProps = {
            selectedData: Object.assign({}, props.selectedData,
                {
                    sourceNodes: Object.assign({}, props.selectedData.sourceNodes, {
                        nodeId: 7
                    })
                })
        };

        const wrapper = shallow(<CreateMovementComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, newProps));

        expect(props.dispatchActions.mock.calls).toHaveLength(1);
    });

    it('should submit successfully and updateNodeMovementInventoryData in the grid and closeModal if no errors', () => {
        const errorCheckObject = {
            sourceNodeId: 1,
            destinationNodeId: 1
        };
        dataService.buildErrorCheckObject = jest.fn(() => errorCheckObject);
        dataService.buildMovementInventoryOwnershipArray = jest.fn(() => [{ transactionId: 1 }]);
        movementValidator.validateMovement = jest.fn(() => true);
        const props = {
            setContracts: jest.fn(),
            setDate: jest.fn(),
            handleSubmit: jest.fn(),
            getOwnersForMovement: jest.fn(),
            getContractData: jest.fn(),
            showError: jest.fn(() => Promise.resolve()),
            setMovementInventoryOwnershipData: jest.fn(),
            updateNodeMovementInventoryData: jest.fn(),
            startEdit: jest.fn(),
            closeModal: jest.fn(),
            selectedData: {
                sourceNodes: { nodeId: 5 },
                destinationNodes: { nodeId: 6 },
                sourceProduct: { productId: 4 },
                variable: { variableTypeId: 5 },
                movementType: { elementId: constants.MovementType.Purchase }
            },
            selectedField: 'sourceNodes',
            movementInventoryOwnershipData: [
                {
                    ownershipVolume: 2345,
                    ownershipPercentage: 100,
                    netVolume: 2345
                }
            ],
            ownershipNode: {
                selectedDataToggler: true,
                movementOwnersDataToggler: true,
                inventoryOwnersDataToggler: false,
                nodeDetails: {
                    ticket: {
                        endDate: '09/01/2020'
                    },
                    ownershipStatus: 1
                }
            }
        };

        const wrapper = shallow(<CreateMovementComponent {...props} />);
        wrapper.instance().onSubmit({ variable: { variableTypeId: constants.VariableType.Input }, netVolume: '235' });
        expect(props.setMovementInventoryOwnershipData).toHaveBeenCalled();
        expect(props.updateNodeMovementInventoryData).toHaveBeenCalled();
        expect(props.startEdit).toHaveBeenCalled();
        expect(props.closeModal).toHaveBeenCalled();
    });

    it('should call getOwnersForInventory method when user only chooses source node and source product', () => {
        dataService.getDispatchActions = jest.fn(() => ['action1']);
        utilities.isArrayNotEmpty = jest.fn(input => input.every(a => !utilities.isNullOrUndefined(a)));
        const props = {
            setContracts: jest.fn(),
            updateSelectedData: jest.fn(),
            setDate: jest.fn(),
            handleSubmit: jest.fn(),
            getOwnersForMovement: jest.fn(),
            getOwnersForInventory: jest.fn(),
            selectContract: jest.fn(),
            getContractData: jest.fn(),
            dispatchActions: jest.fn(),
            clearOwnershipData: jest.fn(),
            selectedData: {
                sourceNodes: { nodeId: 5 },
                destinationNodes: { nodeId: 6 },
                sourceProduct: { productId: 4 },
                destinationProduct: { productId: 4 },
                variable: { variableTypeId: 5 }
            },
            selectedField: 'sourceNodes',
            ownershipNode: {
                nodeDetails: {
                    nodeId: 1,
                    node: {
                        name: 'someName'
                    }
                },
                selectedDataToggler: true,
                movementOwnersDataToggler: true,
                inventoryOwnersDataToggler: false
            }
        };

        const newProps = {
            selectedData: Object.assign({}, props.selectedData, {
                sourceNodes: null,
                sourceProduct: null
            }),
            ownershipNode: Object.assign({}, props.ownershipNode, {
                selectedDataToggler: false
            })
        };

        const wrapper = shallow(<CreateMovementComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, newProps));

        expect(props.getOwnersForInventory.mock.calls).toHaveLength(1);
    });

    it('should call getOwnersForInventory method when user only chooses destination node and destination product', () => {
        dataService.getDispatchActions = jest.fn(() => ['action1']);
        utilities.isArrayNotEmpty = jest.fn(input => input.every(a => !utilities.isNullOrUndefined(a)));
        const props = {
            setContracts: jest.fn(),
            updateSelectedData: jest.fn(),
            setDate: jest.fn(),
            handleSubmit: jest.fn(),
            getOwnersForMovement: jest.fn(),
            getOwnersForInventory: jest.fn(),
            selectContract: jest.fn(),
            getContractData: jest.fn(),
            dispatchActions: jest.fn(),
            clearOwnershipData: jest.fn(),
            selectedData: {
                sourceNodes: { nodeId: 5 },
                destinationNodes: { nodeId: 6 },
                sourceProduct: { productId: 4 },
                destinationProduct: { productId: 4 },
                variable: { variableTypeId: 6 }
            },
            selectedField: 'sourceNodes',
            ownershipNode: {
                nodeDetails: {
                    nodeId: 1,
                    node: {
                        name: 'someName'
                    }
                },
                selectedDataToggler: true,
                movementOwnersDataToggler: true,
                inventoryOwnersDataToggler: false
            }
        };

        const newProps = {
            selectedData: Object.assign({}, props.selectedData, {
                destinationNodes: null,
                destinationProduct: null
            }),
            ownershipNode: Object.assign({}, props.ownershipNode, {
                selectedDataToggler: false
            })
        };

        const wrapper = shallow(<CreateMovementComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, newProps));

        expect(props.getOwnersForInventory.mock.calls).toHaveLength(1);
    });
});
