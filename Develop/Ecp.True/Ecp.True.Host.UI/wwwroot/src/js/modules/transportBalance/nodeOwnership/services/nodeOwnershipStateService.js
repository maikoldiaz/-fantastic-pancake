import { utilities } from '../../../../common/services/utilities';
import { constants } from '../../../../common/services/constants';

const nodeOwnershipStateService = (function () {
    const buildSelectedData = (field, payload, selectedData) => {
        return Object.assign({}, selectedData, {
            [field]: payload
        });
    };

    function modifyInput(state, newState, config) {
        return Object.assign({}, state, newState, {
            sourceProducts: [],
            destinationNodes: config.destinationNode,
            selectedData: Object.assign({}, state.selectedData, newState.selectedData, {
                destinationNodes: Object.assign({}, state.destinationNodes, {
                    destinationNodeId: config.nodeId
                })
            })
        });
    }

    function modifyOutput(state, newState, config) {
        return Object.assign({}, state, newState, {
            sourceNodes: config.sourceNode,
            destinationProducts: [],
            selectedData: Object.assign({}, state.selectedData, newState.selectedData, {
                sourceNodes: Object.assign({}, state.sourceNodes, {
                    sourceNodeId: config.nodeId
                })
            })
        });
    }

    function modifyIdentifiedLoss(state, newState, config) {
        return Object.assign({}, state, newState, {
            sourceNodes: config.sourceNode,
            destinationNodes: config.destinationNode,
            destinationProducts: [],
            selectedData: Object.assign({}, state.selectedData, newState.selectedData, {
                sourceNodes: Object.assign({}, state.sourceNodes, {
                    sourceNodeId: config.nodeId
                })
            })
        });
    }

    function modifyInterface(state, newState, config) {
        return Object.assign({}, state, newState, {
            sourceNodes: config.sourceNode,
            destinationNodes: config.destinationNode,
            selectedData: Object.assign({}, state.selectedData, newState.selectedData, {
                sourceNodes: Object.assign({}, state.sourceNodes, {
                    sourceNodeId: config.nodeId
                }),
                destinationNodes: Object.assign({}, state.destinationNodes, {
                    destinationNodeId: config.nodeId
                })
            })
        });
    }

    function modifyToleranceOrUnidentifiedLoss(state, newState, config) {
        return Object.assign({}, state, newState, {
            sourceNodes: config.sourceNode,
            destinationNodes: config.destinationNode,
            sourceProducts: [],
            destinationProducts: []
        });
    }

    const fieldVariableTypeModifierFactory = {
        [constants.VariableType.Input]: modifyInput,
        [constants.VariableType.Output]: modifyOutput,
        [constants.VariableType.IdentifiedLoss]: modifyIdentifiedLoss,
        [constants.VariableType.Interface]: modifyInterface,
        [constants.VariableType.Tolerance]: modifyToleranceOrUnidentifiedLoss,
        [constants.VariableType.UnidentifiedLoss]: modifyToleranceOrUnidentifiedLoss
    };
    function modifyNetVolume(state, field, payload) {
        const movementInventoryOwnershipData = state.movementInventoryOwnershipData;
        const val = payload;
        const obj = Object.assign({}, state, {
            selectedData: buildSelectedData(field, payload, state.selectedData)
        });
        if (!utilities.isNullOrUndefined(movementInventoryOwnershipData) && movementInventoryOwnershipData.length > 0) {
            movementInventoryOwnershipData.forEach(x => {
                x.netVolume = val;
                x.ownershipVolume = (x.ownershipPercentage * val / 100);
            });
            return Object.assign({}, state, obj, {
                movementInventoryOwnershipData: movementInventoryOwnershipData,
                updateOwnershipDataToggler: !state.updateOwnershipDataToggler,
                displayData: false
            });
        }
        return obj;
    }
    function modifyVariable(state, field, payload) {
        if (!utilities.isNullOrUndefined(state.selectedData.variable) && payload.variableTypeId === state.selectedData.variable.variableTypeId) {
            return state;
        }
        // common obj
        const nodeId = state.nodeDetails.nodeId;
        const sourceNode = [{
            sourceNode: {
                nodeId,
                name: state.nodeDetails.node.name
            }
        }];
        const destinationNode = [{
            destinationNode: {
                nodeId,
                name: state.nodeDetails.node.name
            }
        }];
        const newState = Object.assign({}, state,
            {
                selectedData: Object.assign({}, buildSelectedData(field, payload, state.selectedData), {
                    destinationNodes: null,
                    sourceNodes: null,
                    sourceProduct: null,
                    destinationProduct: null,
                    contract: null,
                    movementType: null
                }),
                selectedDataToggler: !state.selectedDataToggler,
                contracts: [],
                movementInventoryOwnershipData: [],
                updateOwnershipDataToggler: !state.updateOwnershipDataToggler,
                selectedField: field
            });
        return fieldVariableTypeModifierFactory[payload.variableTypeId](state, newState, { nodeId, sourceNode, destinationNode });
    }

    function modifySourceOrDestinationNodes(state, field, payload) {
        const obj = Object.assign({}, state, {
            selectedData: field === 'sourceNodes' ?
                buildSelectedData(field, payload.sourceNode, state.selectedData) :
                buildSelectedData(field, payload.destinationNode, state.selectedData),
            selectedField: field,
            selectedDataToggler: !state.selectedDataToggler
        });
        if (!utilities.isNullOrUndefined(state.selectedData.variable) &&
        state.selectedData.variable.variableTypeId !== constants.VariableType.Tolerance
        && state.selectedData.variable.variableTypeId !== constants.VariableType.UnidentifiedLoss) {
            return Object.assign({}, state, obj, {
                movementInventoryOwnershipData: [],
                updateOwnershipDataToggler: !state.updateOwnershipDataToggler
            });
        }
        return obj;
    }

    function modifyProductOrMovementTypeOrContract(state, field, payload) {
        return Object.assign({}, state, {
            selectedData: (field === 'movementType' || field === 'contract') ?
                buildSelectedData(field, payload, state.selectedData) :
                buildSelectedData(field, payload.product, state.selectedData),
            selectedDataToggler: field !== 'contract' ? !state.selectedDataToggler : state.selectedDataToggler
        });
    }

    const fieldModifierFactory = {
        netVolume: modifyNetVolume,
        variable: modifyVariable,
        sourceNodes: modifySourceOrDestinationNodes,
        destinationNodes: modifySourceOrDestinationNodes,
        sourceProduct: modifyProductOrMovementTypeOrContract,
        destinationProduct: modifyProductOrMovementTypeOrContract,
        movementType: modifyProductOrMovementTypeOrContract,
        contract: modifyProductOrMovementTypeOrContract
    };

    return {
        updateState: (state, field, payload) => {
            if (utilities.isNullOrUndefined(field) || utilities.isNullOrUndefined(state)) {
                return state;
            }
            if (utilities.isNullOrUndefined(payload)) {
                return Object.assign({}, state, {
                    selectedData: buildSelectedData(field, payload, state.selectedData)
                });
            }
            return fieldModifierFactory[field](state, field, payload);
        }
    };
}());
export { nodeOwnershipStateService };
