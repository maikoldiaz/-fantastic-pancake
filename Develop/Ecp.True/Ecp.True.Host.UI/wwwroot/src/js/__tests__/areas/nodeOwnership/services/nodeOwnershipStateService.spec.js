import { nodeOwnershipStateService } from '../../../../modules/transportBalance/nodeOwnership/services/nodeOwnershipStateService';
import { constants } from '../../../../common/services/constants';

describe('action provider tests',
    () => {
        it('should return state for no change',
            () => {
                const state = {};
                const field = null;
                const payload = { product: { productId: 1 } };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(state);
            });

        it('should build for sourceProduct change',
            () => {
                const state = {};
                const field = 'sourceProduct';
                const payload = { product: { productId: 1 } };
                const outputState = {
                    selectedData: {
                        sourceProduct: {
                            productId: 1
                        }
                    },
                    selectedDataToggler: true
                };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(outputState);
            });

        it('should build for destinationProduct change',
            () => {
                const state = {};
                const field = 'destinationProduct';
                const payload = { product: { productId: 1 } };
                const outputState = {
                    selectedData: {
                        destinationProduct: {
                            productId: 1
                        }
                    },
                    selectedDataToggler: true
                };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(outputState);
            });

        it('should build for movementType change',
            () => {
                const state = {};
                const field = 'movementType';
                const payload = { movementType: 1 };
                const outputState = {
                    selectedData: {
                        movementType: {
                            movementType: 1
                        }
                    },
                    selectedDataToggler: true
                };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(outputState);
            });

        it('should build for contract change',
            () => {
                const state = {};
                const field = 'contract';
                const payload = { contractType: 1 };
                const outputState = {
                    selectedData: {
                        contract: {
                            contractType: 1
                        }
                    }
                };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(outputState);
            });

        it('should build for sourceNodes change without variable',
            () => {
                const state = { selectedData: { variable: null } };
                const field = 'sourceNodes';
                const payload = { sourceNode: { nodeId: 1 } };
                const outputState = {
                    selectedData: {
                        sourceNodes: { nodeId: 1 },
                        variable: null
                    },
                    selectedField: field,
                    selectedDataToggler: true
                };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(outputState);
            });

        it('should build for destinationNodes change without variable',
            () => {
                const state = { selectedData: { variable: null } };
                const field = 'destinationNodes';
                const payload = { destinationNode: { nodeId: 1 } };
                const outputState = {
                    selectedData: {
                        destinationNodes: { nodeId: 1 },
                        variable: null
                    },
                    selectedField: field,
                    selectedDataToggler: true
                };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(outputState);
            });

        it('should build for sourceNodes change with variable',
            () => {
                const state = { selectedData: { variable: { variableTypeId: constants.VariableType.Input } } };
                const field = 'sourceNodes';
                const payload = { sourceNode: { nodeId: 1 } };
                const outputState = {
                    selectedData: {
                        sourceNodes: { nodeId: 1 },
                        variable: { variableTypeId: constants.VariableType.Input }
                    },
                    selectedField: field,
                    selectedDataToggler: true,
                    movementInventoryOwnershipData: [],
                    updateOwnershipDataToggler: true
                };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(outputState);
            });

        it('should build for destinationNodes change with variable',
            () => {
                const state = { selectedData: { variable: { variableTypeId: constants.VariableType.Input } } };
                const field = 'destinationNodes';
                const payload = { destinationNode: { nodeId: 1 } };
                const outputState = {
                    selectedData: {
                        destinationNodes: { nodeId: 1 },
                        variable: { variableTypeId: constants.VariableType.Input }
                    },
                    selectedField: field,
                    selectedDataToggler: true,
                    movementInventoryOwnershipData: [],
                    updateOwnershipDataToggler: true
                };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(outputState);
            });

        it('should build for netVolume change',
            () => {
                const state = { movementInventoryOwnershipData: [ { ownershipPercentage: 33.33 } ] };
                const field = 'netVolume';
                const payload = 100;
                const updatedMovementInventoryOwnershipData = [ { ownershipVolume: (33.33 * 100 / 100), netVolume: payload, ownershipPercentage: 33.33 } ];
                const outputState = {
                    selectedData: {
                        netVolume: 100
                    },
                    movementInventoryOwnershipData: updatedMovementInventoryOwnershipData,
                    updateOwnershipDataToggler: !state.updateOwnershipDataToggler,
                    displayData: false
                };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(outputState);
            });

        it('should build for netVolume change without ownership data',
            () => {
                const state = { movementInventoryOwnershipData: null };
                const field = 'netVolume';
                const payload = 100;
                const outputState = {
                    selectedData: { netVolume: 100 },
                    movementInventoryOwnershipData: null
                };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(outputState);
            });

        it('should build for input variable change',
            () => {
                const state = { selectedData: { variable: { variableTypeId: constants.VariableType.Output } }, nodeDetails: { nodeId: 1, node: { name: 'name' } } };
                const config = {
                    nodeId: state.nodeDetails.nodeId,
                    sourceNode: [{ sourceNode: { nodeId: state.nodeDetails.nodeId, name: state.nodeDetails.node.name } }],
                    destinationNode: [{ destinationNode: { nodeId: state.nodeDetails.nodeId, name: state.nodeDetails.node.name } }]
                };
                const field = 'variable';
                const payload = { variableTypeId: constants.VariableType.Input };
                const newState = {
                    selectedData: {
                        variable: payload,
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
                    selectedField: field
                };
                const outputState = {
                    contracts: [],
                    sourceProducts: [],
                    destinationNodes: config.destinationNode,
                    selectedData: Object.assign({}, state.selectedData, newState.selectedData, {
                        destinationNodes: Object.assign({}, state.destinationNodes, {
                            destinationNodeId: config.nodeId
                        })
                    }),
                    movementInventoryOwnershipData: [],
                    nodeDetails: { nodeId: 1, node: { name: 'name' } },
                    selectedDataToggler: true,
                    updateOwnershipDataToggler: true,
                    selectedField: field
                };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(outputState);
            });

        it('should build for no variable change',
            () => {
                const state = { selectedData: { variable: { variableTypeId: constants.VariableType.Input } }, nodeDetails: { nodeId: 1, node: { name: 'name' } } };
                const field = 'variable';
                const payload = { variableTypeId: constants.VariableType.Input };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(state);
            });

        it('should build for output variable change',
            () => {
                const state = { selectedData: { variable: { variableTypeId: constants.VariableType.Input } }, nodeDetails: { nodeId: 1, node: { name: 'name' } } };
                const config = {
                    nodeId: state.nodeDetails.nodeId,
                    sourceNode: [{ sourceNode: { nodeId: state.nodeDetails.nodeId, name: state.nodeDetails.node.name } }],
                    destinationNode: [{ destinationNode: { nodeId: state.nodeDetails.nodeId, name: state.nodeDetails.node.name } }]
                };
                const field = 'variable';
                const payload = { variableTypeId: constants.VariableType.Output };
                const newState = {
                    selectedData: {
                        variable: payload,
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
                    selectedField: field
                };
                const outputState = {
                    contracts: [],
                    sourceNodes: config.sourceNode,
                    destinationProducts: [],
                    selectedData: Object.assign({}, state.selectedData, newState.selectedData, {
                        sourceNodes: Object.assign({}, state.sourceNodes, {
                            sourceNodeId: config.nodeId
                        })
                    }),
                    movementInventoryOwnershipData: [],
                    nodeDetails: { nodeId: 1, node: { name: 'name' } },
                    selectedDataToggler: true,
                    updateOwnershipDataToggler: true,
                    selectedField: field
                };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(outputState);
            });

        it('should build for IdentifiedLoss variable change',
            () => {
                const state = { selectedData: { variable: { variableTypeId: constants.VariableType.Input } }, nodeDetails: { nodeId: 1, node: { name: 'name' } } };
                const config = {
                    nodeId: state.nodeDetails.nodeId,
                    sourceNode: [{ sourceNode: { nodeId: state.nodeDetails.nodeId, name: state.nodeDetails.node.name } }],
                    destinationNode: [{ destinationNode: { nodeId: state.nodeDetails.nodeId, name: state.nodeDetails.node.name } }]
                };
                const field = 'variable';
                const payload = { variableTypeId: constants.VariableType.IdentifiedLoss };
                const newState = {
                    selectedData: {
                        variable: payload,
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
                    selectedField: field
                };
                const outputState = {
                    contracts: [],
                    sourceNodes: config.sourceNode,
                    destinationNodes: config.destinationNode,
                    destinationProducts: [],
                    selectedData: Object.assign({}, state.selectedData, newState.selectedData, {
                        sourceNodes: Object.assign({}, state.sourceNodes, {
                            sourceNodeId: config.nodeId
                        })
                    }),
                    movementInventoryOwnershipData: [],
                    nodeDetails: { nodeId: 1, node: { name: 'name' } },
                    selectedDataToggler: true,
                    updateOwnershipDataToggler: true,
                    selectedField: field
                };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(outputState);
            });

        it('should build for Interface variable change',
            () => {
                const state = { selectedData: { variable: { variableTypeId: constants.VariableType.Input } }, nodeDetails: { nodeId: 1, node: { name: 'name' } } };
                const config = {
                    nodeId: state.nodeDetails.nodeId,
                    sourceNode: [{ sourceNode: { nodeId: state.nodeDetails.nodeId, name: state.nodeDetails.node.name } }],
                    destinationNode: [{ destinationNode: { nodeId: state.nodeDetails.nodeId, name: state.nodeDetails.node.name } }]
                };
                const field = 'variable';
                const payload = { variableTypeId: constants.VariableType.Interface };
                const newState = {
                    selectedData: {
                        variable: payload,
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
                    selectedField: field
                };
                const outputState = {
                    contracts: [],
                    sourceNodes: config.sourceNode,
                    destinationNodes: config.destinationNode,
                    selectedData: Object.assign({}, state.selectedData, newState.selectedData, {
                        sourceNodes: Object.assign({}, state.sourceNodes, {
                            sourceNodeId: config.nodeId
                        }),
                        destinationNodes: Object.assign({}, state.destinationNodes, {
                            destinationNodeId: config.nodeId
                        })
                    }),
                    movementInventoryOwnershipData: [],
                    nodeDetails: { nodeId: 1, node: { name: 'name' } },
                    selectedDataToggler: true,
                    updateOwnershipDataToggler: true,
                    selectedField: field
                };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(outputState);
            });

        it('should build for Tolerance variable change',
            () => {
                const state = { selectedData: { variable: { variableTypeId: constants.VariableType.Input } }, nodeDetails: { nodeId: 1, node: { name: 'name' } } };
                const config = {
                    nodeId: state.nodeDetails.nodeId,
                    sourceNode: [{ sourceNode: { nodeId: state.nodeDetails.nodeId, name: state.nodeDetails.node.name } }],
                    destinationNode: [{ destinationNode: { nodeId: state.nodeDetails.nodeId, name: state.nodeDetails.node.name } }]
                };
                const field = 'variable';
                const payload = { variableTypeId: constants.VariableType.Tolerance };
                const newState = {
                    selectedData: {
                        variable: payload,
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
                    selectedField: field
                };
                const outputState = {
                    contracts: [],
                    sourceNodes: config.sourceNode,
                    destinationNodes: config.destinationNode,
                    sourceProducts: [],
                    destinationProducts: [],
                    movementInventoryOwnershipData: [],
                    nodeDetails: { nodeId: 1, node: { name: 'name' } },
                    selectedDataToggler: true,
                    updateOwnershipDataToggler: true,
                    selectedField: field,
                    selectedData: newState.selectedData
                };
                expect(nodeOwnershipStateService.updateState(state, field, payload)).toEqual(outputState);
            });
    });
