import * as actions from '../../../modules/transportBalance/nodeOwnership/actions';
import { ownershipNode, ownershipNodeDetails } from '../../../modules/transportBalance/nodeOwnership/reducers';
import { constants } from '../../../../js/common/services/constants';

describe('Reducer for node ownership cut', () => {
    const initialState = {
        nodeDetails: {
            ticket: { categoryElement: {}, node: {} }
        },
        editorInfo: {},
        startEditToggler: false,
        endEditToggler: false,
        unlockNodeToggler: false,
        publishOwnershipToggler: false,
        nodeMovementInventoryData: [],
        movementInventoryOwnershipData: [],
        movementInventoryOwnershipUpdated: false,
        movementInventoryfilters: {
            product: null,
            variableType: null,
            owner: null
        },
        commentToggler: false,
        totalVolumeControl: 0,
        message: null,
        reopenSuccess: false,
        publishSuccess: false,
        selectedData: {
            sourceNodes: null,
            destinationNodes: null,
            variable: null,
            sourceProduct: null,
            destinationProduct: null,
            movementType: null,
            netVolume: 0,
            contract: null,
            unit: null
        },
        selectedField: null
    };

    it('should set node error detail', () => {
        const action = {
            type: actions.REQUEST_OWNERSHIP_NODE_ERROR_DETAIL,
            node: { id: 100 }
        };

        const newState = Object.assign({}, initialState, { node: action.node });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('receive last operational ticket', () => {
        const action = {
            type: actions.RECEIVE_LAST_OPERATIONAL_TICKET,
            ticket: { value: 23 }
        };

        const newState = Object.assign({}, initialState, { lastTicketPerSegment: action.ticket.value });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('receive ownership node details', () => {
        const action = {
            type: actions.RECEIVE_NODE_OWNERSHIP_DETAILS,
            nodeDetails: {
                ownershipNodeId: 1,
                ticketId: 101,
                nodeId: 100,
                status: 'OWNERSHIP'
            },
            reopenSuccess: false
        };

        const newState = Object.assign({}, initialState, { nodeDetails: action.nodeDetails, reopenSuccess: false });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('receive ownership node movement and inventory data', () => {
        const action = {
            type: actions.RECEIVE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA,
            movementInventoryData: [{
                ownershipNodeId: 1,
                ticketId: 101,
                nodeId: 100,
                status: 'OWNERSHIP'
            }],
            nodeMovementInventoryDataToggler: false,
            movementInventoryOwnershipUpdated: false
        };

        const newState = Object.assign({}, initialState, { nodeMovementInventoryData: action.movementInventoryData, nodeMovementInventoryDataToggler: true, movementInventoryOwnershipUpdated: false });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('receive owners for movement', () => {
        const action = {
            type: actions.RECEIVE_OWNERS_FOR_MOVEMENT,
            movementOwners: [{
                ownerID: 1,
                ownerName: 'ECOPETROL',
                ownershipPercentage: 100,
                ownershipVolume: 1500
            }],
            movementOwnersDataToggler: true
        };

        const newState = Object.assign({}, initialState, { movementOwners: action.movementOwners, movementOwnersDataToggler: true });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should set movement inventory ownership data', () => {
        const action = {
            type: actions.SET_MOVEMENT_INVENTORY_OWNERSHIP_DATA,
            movementInventoryOwnershipData: { ownerId: 28 }
        };
        const newState = Object.assign({}, initialState, { movementInventoryOwnershipData: action.movementInventoryOwnershipData, updateOwnershipDataToggler: true, displayData: false });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should update movement ownership data', () => {
        const action = {
            type: actions.UPDATE_MOVEMENT_OWNERSHIP_DATA,
            updatedMovementOwnershipData: { ownerId: 28 },
            movementInventoryOwnershipUpdated: true
        };
        const newState = Object.assign({}, initialState, { movementInventoryOwnershipData: action.updatedMovementOwnershipData, movementInventoryOwnershipUpdated: true });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should add comment reopen ticket', () => {
        const action = {
            type: actions.ADD_COMMENT_REOPEN_TICKET,
            comment: 'Sample comment'
        };
        const newState = Object.assign({}, initialState, { reopenComment: Object.assign({}, initialState, { message: action.comment, commentToggler: !initialState.commentToggler }) });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should receive ticket reopen on success', () => {
        const action = {
            type: actions.RECEIVE_TICKET_REOPEN_SUCCESS,
            success: 'true'
        };
        const newState = Object.assign({}, initialState, { reopenSuccess: action.success, nodeDetails: { ticket: { categoryElement: {}, node: {} }, ownershipStatus: 'REOPENED' } });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should initialize filter updates', () => {
        const action = {
            type: actions.INIT_UPDATE_FILTERS,
            filter: {
                product: 1,
                owner: 'ECOPETROL',
                variableType: 'TOLERANCE'
            },
            movementInventoryfilterToggler: true
        };
        const newState = Object.assign({}, initialState, { movementInventoryfilters: action.filter, movementInventoryfilterToggler: true });
        expect(ownershipNodeDetails(initialState, action)).toEqual(newState);
    });

    it('should start edit', () => {
        const action = {
            type: actions.START_EDIT,
            startEditToggler: true
        };
        const newState = Object.assign({}, initialState, { startEditToggler: !action.startEditToggler });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should end edit', () => {
        const action = {
            type: actions.END_EDIT,
            endEditToggler: true
        };
        const newState = Object.assign({}, initialState, { endEditToggler: !action.endEditToggler });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should unlock node', () => {
        const action = {
            type: actions.UNLOCK_NODE,
            unlockNodeToggler: true
        };
        const newState = Object.assign({}, initialState, { unlockNodeToggler: !action.unlockNodeToggler });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should set node status to publishing', () => {
        const action = {
            type: actions.PUBLISHING_NODE,
            publishingNodeToggler: true
        };
        const newState = Object.assign({}, initialState, { publishingNodeToggler: !action.publishingNodeToggler });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should receive owners for inventory', () => {
        const action = {
            type: actions.RECEIVE_OWNERS_FOR_INVENTORY,
            inventoryOwners: { owner: 'REFICAR' }
        };
        const newState = Object.assign({}, initialState, { inventoryOwners: action.inventoryOwners, inventoryOwnersDataToggler: true });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should update ownership node movement inventory data', () => {
        const action = {
            type: actions.UPDATE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA,
            movementOwnershipInventoryData: [{
                ownerId: 13,
                transactionId: 26
            }]
        };
        initialState.nodeMovementInventoryData =
            [{ ownerId: 12, transactionId: 24 }];
        const newState = Object.assign({}, initialState, {
            nodeMovementInventoryData: [{ ownerId: 12, transactionId: 24 }, {
                ownerId: 13, transactionId: 26 }], nodeMovementInventoryDataToggler: true
        });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should receive publish ownership success', () => {
        const action = {
            type: actions.RECEIVE_PUBLISH_OWNERSHIP_SUCCESS,
            success: 'true'
        };
        const newState = Object.assign({}, initialState, { publishSuccess: action.success, movementInventoryOwnershipUpdated: false });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should update editor information', () => {
        const action = {
            type: actions.EDITOR_INFORMATION,
            editorInfo: { name: 'test editor' }
        };
        const newState = Object.assign({}, initialState, { editorInfo: action.editorInfo });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should update on publish node ownership', () => {
        const action = {
            type: actions.ON_PUBLISH_NODE_OWNERSHIP,
            publishOwnershipToggler: true
        };
        const newState = Object.assign({}, initialState, { publishOwnershipToggler: !action.publishOwnershipToggler });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should receive source node', () => {
        const action = {
            type: actions.RECEIVE_SOURCE_NODE,
            nodes: [
                { nodeId: 1
                }]
        };
        const newState = Object.assign({}, initialState, { sourceNodes: action.nodes });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should receive destination node', () => {
        const action = {
            type: actions.RECEIVE_DESTINATION_NODE,
            nodes: [
                { nodeId: 1
                }]
        };
        const newState = Object.assign({}, initialState, { destinationNodes: action.nodes });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should set destination node', () => {
        const action = {
            type: actions.SET_DESTINATION_NODE,
            nodes: [
                { nodeId: 1
                }]
        };
        const newState = Object.assign({}, initialState, { destinationNodes: action.nodes });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });


    it('should receive source product', () => {
        const action = {
            type: actions.RECEIVE_SOURCE_PRODUCT,
            products: [
                { productId: 1
                }]
        };
        const newState = Object.assign({}, initialState, { sourceProducts: action.products });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });


    it('should receive destination product', () => {
        const action = {
            type: actions.RECEIVE_DESTINATION_PRODUCT,
            products: [
                { productId: 1
                }]
        };
        const newState = Object.assign({}, initialState, { destinationProducts: action.products });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should set destination product', () => {
        const action = {
            type: actions.SET_DESTINATION_PRODUCT,
            product: [
                { productId: 1
                }]
        };
        const newState = Object.assign({}, initialState, { destinationProducts: action.product });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should set source product', () => {
        const action = {
            type: actions.SET_SOURCE_PRODUCT,
            product: [
                { productId: 1
                }]
        };
        const newState = Object.assign({}, initialState, { sourceProducts: action.product });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should clear ownership data', () => {
        const action = {
            type: actions.CLEAR_OWNERSHIP_DATA,
            movementInventoryOwnershipData: [],
            updateOwnershipDataToggler: false
        };
        const newState = Object.assign({}, initialState, { movementInventoryOwnershipData: action.movementInventoryOwnershipData, updateOwnershipDataToggler: true });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should update current volume control', () => {
        const action = {
            type: actions.UPDATE_CURRENT_VOLUME_CONTROL,
            totalVolume: 100
        };
        const newState = Object.assign({}, initialState, { totalVolumeControl: action.totalVolume });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should handle default case', () => {
        const defaultCaseValue = 'Any_Value';
        const action = {
            type: defaultCaseValue,
            ticket: { value: 23 }
        };
        expect(ownershipNode(initialState, action)).toEqual(initialState);
    });

    it('should receive contract data', () => {
        const action = {
            type: actions.RECEIVE_CONTRACT_DATA,
            contracts: [
                { contractId: 1
                }]
        };
        const newState = Object.assign({}, initialState, { contracts: action.contracts });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should get contract data', () => {
        const action = {
            type: actions.GET_CONTRACT_DATA,
            data: {
                value: [
                    {
                        contractId: 1,
                        documentNumber: 101,
                        position: 1
                    }
                ]
            }
        };
        const newState = Object.assign({}, initialState, { contractData: action.data.value, dropdown: [{ id: 1, name: '101 - 1' }] });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should display dropdown', () => {
        const initialState = {
            contractData: [{
                contractId: 1
            }]
        };
        const action = {
            type: actions.DISPLAY_DATA_DROPDOWN,
            id: 1,
            contractData: {
                contractId: 1
            }
        };
        const newState = Object.assign({}, initialState, { displayData: true, selectedContract: action.contractData });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should set date', () => {
        const initialState = {
            nodeDetails: {
                ticket: {
                    endDate: '01-jan-20'
                }
            }
        };
        const action = {
            type: actions.SET_DATE,
            initialDate: {
                movementDate: '01-jan-20'
            }
        };
        const newState = Object.assign({}, initialState, { initialDate: { movementDate: '2020-01-01T00:00:00Z' } });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should clear selected data', () => {
        const initialState = {};
        const action = {
            type: actions.CLEAR_SELECTED_DATA
        };
        const newState = Object.assign({}, initialState, {
            selectedData: {
                sourceNodes: null,
                destinationNodes: null,
                variable: null,
                sourceProduct: null,
                destinationProduct: null,
                movementType: null,
                netVolume: 0,
                contract: null,
                unit: null
            },
            sourceNodes: [],
            destinationNodes: [],
            sourceProducts: [],
            destinationProducts: []
        });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should clear the selected contract', () => {
        const initialState = {};
        const action = {
            type: actions.CLEAR_SELECTED_CONTRACT
        };
        const newState = Object.assign({}, initialState, {
            selectedContract: null,
            displayData: false
        });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('@@redux-form/CHANGE for netVolume of createMovement form', () => {
        const initialState = {
            movementInventoryOwnershipData: [{
                netVolume: 10,
                ownershipVolume: 0,
                ownershipPercentage: 100
            }]
        };
        const action = {
            type: '@@redux-form/CHANGE',
            meta: { form: 'createMovement',
                field: 'netVolume' },
            payload: 100
        };
        const newState = Object.assign({}, initialState, {
            selectedData: {
                netVolume: 100
            },
            movementInventoryOwnershipData: [{
                netVolume: 100,
                ownershipVolume: 100,
                ownershipPercentage: 100
            }],
            updateOwnershipDataToggler: true,
            displayData: false
        });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('@@redux-form/CHANGE for variable of type input of create movement form', () => {
        const initialState = {
            nodeDetails: {
                nodeId: 10,
                node: {
                    name: 'test'
                }
            },
            destinationNodes: null,
            selectedDataToggler: false,
            updateOwnershipDataToggler: false,
            selectedData: {}
        };
        const action = {
            type: '@@redux-form/CHANGE',
            meta: { form: 'createMovement',
                field: 'variable' },
            payload: {
                variableTypeId: constants.VariableType.Input
            }
        };
        const sourceNode = [{
            sourceNode: {
                nodeId: 10,
                name: 'test'
            }
        }];
        const destinationNode = [{
            destinationNode: {
                nodeId: 10,
                name: 'test'
            }
        }];
        const newState = Object.assign({}, initialState, {
            selectedData: {
                variable: action.payload,
                destinationNodes: {
                    destinationNodeId: 10
                },
                sourceNodes: null,
                sourceProduct: null,
                destinationProduct: null,
                contract: null,
                movementType: null
            },
            selectedDataToggler: true,
            contracts: [],
            movementInventoryOwnershipData: [],
            updateOwnershipDataToggler: true,
            selectedField: action.meta.field,
            sourceProducts: [],
            destinationNodes: destinationNode
        });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('@@redux-form/CHANGE for variable of type output of create movement form', () => {
        const initialState = {
            nodeDetails: {
                nodeId: 10,
                node: {
                    name: 'test'
                }
            },
            destinationNodes: null,
            selectedDataToggler: false,
            updateOwnershipDataToggler: false,
            selectedData: {}
        };
        const action = {
            type: '@@redux-form/CHANGE',
            meta: { form: 'createMovement',
                field: 'variable' },
            payload: {
                variableTypeId: constants.VariableType.Output
            }
        };
        const sourceNode = [{
            sourceNode: {
                nodeId: 10,
                name: 'test'
            }
        }];
        const destinationNode = [{
            destinationNode: {
                nodeId: 10,
                name: 'test'
            }
        }];
        const newState = Object.assign({}, initialState, {
            selectedData: {
                variable: action.payload,
                destinationNodes: null,
                sourceNodes: {
                    sourceNodeId: 10
                },
                sourceProduct: null,
                destinationProduct: null,
                contract: null,
                movementType: null
            },
            selectedDataToggler: true,
            contracts: [],
            movementInventoryOwnershipData: [],
            updateOwnershipDataToggler: true,
            selectedField: action.meta.field,
            destinationProducts: [],
            sourceNodes: sourceNode
        });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('@@redux-form/CHANGE for variable of type IdentifiedLoss of create movement form', () => {
        const initialState = {
            nodeDetails: {
                nodeId: 10,
                node: {
                    name: 'test'
                }
            },
            destinationNodes: null,
            selectedDataToggler: false,
            updateOwnershipDataToggler: false,
            selectedData: {}
        };
        const action = {
            type: '@@redux-form/CHANGE',
            meta: { form: 'createMovement',
                field: 'variable' },
            payload: {
                variableTypeId: constants.VariableType.IdentifiedLoss
            }
        };
        const sourceNode = [{
            sourceNode: {
                nodeId: 10,
                name: 'test'
            }
        }];
        const destinationNode = [{
            destinationNode: {
                nodeId: 10,
                name: 'test'
            }
        }];
        const newState = Object.assign({}, initialState, {
            selectedData: {
                variable: action.payload,
                destinationNodes: null,
                sourceNodes: {
                    sourceNodeId: 10
                },
                sourceProduct: null,
                destinationProduct: null,
                contract: null,
                movementType: null
            },
            selectedDataToggler: true,
            contracts: [],
            movementInventoryOwnershipData: [],
            updateOwnershipDataToggler: true,
            selectedField: action.meta.field,
            sourceNodes: sourceNode,
            destinationNodes: destinationNode,
            destinationProducts: []
        });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('@@redux-form/CHANGE for variable of type Interface of create movement form', () => {
        const initialState = {
            nodeDetails: {
                nodeId: 10,
                node: {
                    name: 'test'
                }
            },
            destinationNodes: null,
            selectedDataToggler: false,
            updateOwnershipDataToggler: false,
            selectedData: {}
        };
        const action = {
            type: '@@redux-form/CHANGE',
            meta: { form: 'createMovement',
                field: 'variable' },
            payload: {
                variableTypeId: constants.VariableType.Interface
            }
        };
        const sourceNode = [{
            sourceNode: {
                nodeId: 10,
                name: 'test'
            }
        }];
        const destinationNode = [{
            destinationNode: {
                nodeId: 10,
                name: 'test'
            }
        }];
        const newState = Object.assign({}, initialState, {
            selectedData: {
                variable: action.payload,
                destinationNodes: {
                    destinationNodeId: 10
                },
                sourceNodes: {
                    sourceNodeId: 10
                },
                sourceProduct: null,
                destinationProduct: null,
                contract: null,
                movementType: null
            },
            selectedDataToggler: true,
            contracts: [],
            movementInventoryOwnershipData: [],
            updateOwnershipDataToggler: true,
            selectedField: action.meta.field,
            sourceNodes: sourceNode,
            destinationNodes: destinationNode
        });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('@@redux-form/CHANGE for variable of type Tolerance or UnidentifiedLoss of create movement form', () => {
        const initialState = {
            nodeDetails: {
                nodeId: 10,
                node: {
                    name: 'test'
                }
            },
            destinationNodes: null,
            selectedDataToggler: false,
            updateOwnershipDataToggler: false,
            selectedData: {}
        };
        const action = {
            type: '@@redux-form/CHANGE',
            meta: { form: 'createMovement',
                field: 'variable' },
            payload: {
                variableTypeId: constants.VariableType.UnidentifiedLoss
            }
        };
        const sourceNode = [{
            sourceNode: {
                nodeId: 10,
                name: 'test'
            }
        }];
        const destinationNode = [{
            destinationNode: {
                nodeId: 10,
                name: 'test'
            }
        }];
        const newState = Object.assign({}, initialState, {
            selectedData: {
                variable: action.payload,
                destinationNodes: null,
                sourceNodes: null,
                sourceProduct: null,
                destinationProduct: null,
                contract: null,
                movementType: null
            },
            selectedDataToggler: true,
            contracts: [],
            movementInventoryOwnershipData: [],
            updateOwnershipDataToggler: true,
            selectedField: action.meta.field,
            sourceNodes: sourceNode,
            destinationNodes: destinationNode,
            sourceProducts: [],
            destinationProducts: []
        });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('@@redux-form/CHANGE for sourceNodes of create movement form', () => {
        const initialState = {
            selectedData: {
                variable: {
                    variableTypeId: constants.VariableType.Interface
                }
            },
            selectedDataToggler: false,
            updateOwnershipDataToggler: false
        };
        const action = {
            type: '@@redux-form/CHANGE',
            meta: { form: 'createMovement',
                field: 'sourceNodes' },
            payload: {
                sourceNode: {}
            }
        };
        const newState = Object.assign({}, initialState, {
            selectedData: {
                sourceNodes: action.payload.sourceNode,
                variable: {
                    variableTypeId: constants.VariableType.Interface
                }
            },
            selectedField: action.meta.field,
            selectedDataToggler: true,
            movementInventoryOwnershipData: [],
            updateOwnershipDataToggler: true
        });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('@@redux-form/CHANGE for destinationNodes of create movement form', () => {
        const initialState = {
            selectedData: {
                variable: {
                    variableTypeId: constants.VariableType.Interface
                }
            },
            selectedDataToggler: false,
            updateOwnershipDataToggler: false
        };
        const action = {
            type: '@@redux-form/CHANGE',
            meta: { form: 'createMovement',
                field: 'destinationNodes' },
            payload: {
                destinationNode: {}
            }
        };
        const newState = Object.assign({}, initialState, {
            selectedData: {
                destinationNodes: action.payload.destinationNode,
                variable: {
                    variableTypeId: constants.VariableType.Interface
                }
            },
            selectedField: action.meta.field,
            selectedDataToggler: true,
            movementInventoryOwnershipData: [],
            updateOwnershipDataToggler: true
        });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('@@redux-form/CHANGE for sourceProduct of create movement form', () => {
        const initialState = {
            selectedDataToggler: false
        };
        const action = {
            type: '@@redux-form/CHANGE',
            meta: { form: 'createMovement',
                field: 'sourceProduct' },
            payload: {
                product: {}
            }
        };
        const newState = Object.assign({}, initialState, {
            selectedData: {
                sourceProduct: action.payload.product
            },
            selectedDataToggler: true
        });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('@@redux-form/CHANGE for destinationProduct of create movement form', () => {
        const initialState = {
            selectedDataToggler: false
        };
        const action = {
            type: '@@redux-form/CHANGE',
            meta: { form: 'createMovement',
                field: 'destinationProduct' },
            payload: {
                product: {}
            }
        };
        const newState = Object.assign({}, initialState, {
            selectedData: {
                destinationProduct: action.payload.product
            },
            selectedDataToggler: true
        });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('@@redux-form/CHANGE for movementType of create movement form', () => {
        const initialState = {
            selectedDataToggler: false
        };
        const action = {
            type: '@@redux-form/CHANGE',
            meta: { form: 'createMovement',
                field: 'movementType' },
            payload: 'test'
        };
        const newState = Object.assign({}, initialState, {
            selectedData: {
                movementType: action.payload
            },
            selectedDataToggler: true
        });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('@@redux-form/CHANGE for contract of create movement form', () => {
        const initialState = {};
        const action = {
            type: '@@redux-form/CHANGE',
            meta: { form: 'createMovement',
                field: 'contract' },
            payload: 'test'
        };
        const newState = Object.assign({}, initialState, {
            selectedData: {
                contract: action.payload
            }
        });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });

    it('should receive conciliation successful', () => {
        const action = {
            type: actions.RECEIVE_CONCILIATION_NODE
        };
        const originalState = Object.assign({}, initialState, { conciliationSuccessToggler: false });
        const newState = Object.assign({}, initialState, { conciliationSuccessToggler: true });
        expect(ownershipNode(originalState, action)).toEqual(newState);
    });

    it('should receive conciliation failure', () => {
        const action = {
            type: actions.FAILURE_CONCILIATION_NODE,
            response: {}
        };
        const originalState = Object.assign({}, initialState, { conciliationErrorToggler: false });
        const newState = Object.assign({}, initialState, {
            conciliationErrorToggler: true
        });
        expect(ownershipNode(originalState, action)).toEqual(newState);
    });
});

describe('Reducer for ownership node details', () => {
    it('should update init update filters', () => {
        const initialState = {
            movementInventoryfilters: null,
            movementInventoryfilterToggler: false
        };
        const action = {
            type: actions.INIT_UPDATE_FILTERS,
            filter: {}
        };
        const newState = Object.assign({}, initialState, {
            movementInventoryfilters: {},
            movementInventoryfilterToggler: true
        });
        expect(ownershipNodeDetails(initialState, action)).toEqual(newState);
    });

    it('should clear movement inventory filter', () => {
        const initialState = {
            movementInventoryfilters: {
                product: null,
                variableType: null,
                owner: null
            }
        };
        const action = {
            type: actions.CLEAR_MOVEMENT_INVENTORY_FILTER,
            movementInventoryfilters: {
                product: null,
                variableType: null,
                owner: null
            }
        };
        const newState = Object.assign({}, initialState, { movementInventoryfilters: action.movementInventoryfilters });
        expect(ownershipNode(initialState, action)).toEqual(newState);
    });
});
