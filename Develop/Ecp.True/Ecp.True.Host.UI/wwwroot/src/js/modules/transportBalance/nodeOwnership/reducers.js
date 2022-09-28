import {
    REQUEST_OWNERSHIP_NODE_ERROR_DETAIL,
    RECEIVE_LAST_OPERATIONAL_TICKET,
    RECEIVE_NODE_OWNERSHIP_DETAILS,
    INIT_OWNERSHIP_DETAILS,
    RECEIVE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA,
    RECEIVE_OWNERS_FOR_MOVEMENT,
    ADD_COMMENT_REOPEN_TICKET,
    RECEIVE_TICKET_REOPEN_SUCCESS,
    INIT_UPDATE_FILTERS,
    START_EDIT,
    END_EDIT,
    UNLOCK_NODE,
    SET_MOVEMENT_INVENTORY_OWNERSHIP_DATA,
    UPDATE_MOVEMENT_OWNERSHIP_DATA,
    UPDATE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA,
    RECEIVE_OWNERS_FOR_INVENTORY,
    RECEIVE_PUBLISH_OWNERSHIP_SUCCESS,
    EDITOR_INFORMATION,
    RECEIVE_SOURCE_NODE,
    SET_SOURCE_NODE,
    RECEIVE_DESTINATION_NODE,
    SET_DESTINATION_NODE,
    RECEIVE_SOURCE_PRODUCT,
    RECEIVE_DESTINATION_PRODUCT,
    CLEAR_SELECTED_DATA,
    SET_DESTINATION_PRODUCT,
    SET_SOURCE_PRODUCT,
    CLEAR_OWNERSHIP_DATA,
    ON_PUBLISH_NODE_OWNERSHIP,
    UPDATE_CURRENT_VOLUME_CONTROL,
    SET_DATE,
    PUBLISHING_NODE,
    RECEIVE_CONTRACT_DATA,
    GET_CONTRACT_DATA,
    DISPLAY_DATA_DROPDOWN,
    CLEAR_SELECTED_CONTRACT,
    CLEAR_MOVEMENT_INVENTORY_FILTER,
    RECEIVE_SEND_OWNERSHIP_NODE_FOR_APPROVAL,
    RECEIVE_OWNERSHIPNODE_FOR_APPROVAL_EXCEPTION,
    NODE_PUBLISH_SUCCESS,
    NODE_PUBLISH_FAILURE,
    RESET_NODE_IS_PUBLISHING,
    SET_NODE_IS_PUBLISHING,
    RECEIVE_CONCILIATION_NODE,
    FAILURE_CONCILIATION_NODE
} from './actions.js';

import { dateService } from '../../../common/services/dateService';
import { utilities } from '../../../common/services/utilities.js';
import { constants } from '../../../common/services/constants.js';
import { nodeOwnershipStateService } from './services/nodeOwnershipStateService';

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
    selectedField: null,
    lastTicketPerSegment: [],
    initialDate: null,
    nodeOwnershipPublishSuccessToggler: false,
    isPublishing: false
};

function moreOwnershipNodeCases(action, state) {
    switch (action.type) {
    case SET_DESTINATION_PRODUCT:
        return { destinationProducts: action.product };
    case SET_SOURCE_PRODUCT:
        return { sourceProducts: action.product };
    case CLEAR_OWNERSHIP_DATA:
        return { movementInventoryOwnershipData: [], updateOwnershipDataToggler: !state.updateOwnershipDataToggler };
    case UPDATE_CURRENT_VOLUME_CONTROL:
        return { totalVolumeControl: action.totalVolume };
    case SET_DATE:
        return { initialDate: { movementDate: dateService.format(state.nodeDetails.ticket.endDate) } };
    case RECEIVE_CONTRACT_DATA:
        return { contracts: action.contracts };
    default:
        return state;
    }
}

export const ownershipNode = (state = initialState, action = {}) => {
    switch (action.type) {
    case REQUEST_OWNERSHIP_NODE_ERROR_DETAIL:
        return Object.assign({},
            state,
            {
                node: action.node
            });
    case RECEIVE_LAST_OPERATIONAL_TICKET:
        return Object.assign({}, state, {
            lastTicketPerSegment: !utilities.isNullOrUndefined(action.ticket.value) ? action.ticket.value : []
        });
    case RECEIVE_NODE_OWNERSHIP_DETAILS:
        return Object.assign({}, state, {
            nodeDetails: action.nodeDetails,
            reopenSuccess: false
        });
    case INIT_OWNERSHIP_DETAILS:
        return Object.assign({}, state, {
            initialValues: {
                reasonForChange: state.movementInventoryOwnershipData[0].reason ? {
                    name: state.movementInventoryOwnershipData[0].reason,
                    elementId: state.movementInventoryOwnershipData[0].reasonId
                } : null,
                comment: state.movementInventoryOwnershipData[0].comment
            }
        });
    case RECEIVE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA:
        return Object.assign({}, state, {
            nodeMovementInventoryData: action.movementInventoryData.map(item => {
                item.color = utilities.isNullOrUndefined(item.color) ? constants.DefaultColorCode : item.color;
                return item;
            }),
            nodeMovementInventoryDataToggler: !state.nodeMovementInventoryDataToggler,
            movementInventoryOwnershipUpdated: false
        });
    case RECEIVE_OWNERS_FOR_MOVEMENT:
        return Object.assign({}, state, {
            movementOwners: action.movementOwners,
            movementOwnersDataToggler: !state.movementOwnersDataToggler

        });
    case SET_MOVEMENT_INVENTORY_OWNERSHIP_DATA:
        return Object.assign({}, state, {
            movementInventoryOwnershipData: action.movementInventoryOwnershipData,
            updateOwnershipDataToggler: !state.updateOwnershipDataToggler,
            displayData: false
        });
    case UPDATE_MOVEMENT_OWNERSHIP_DATA: {
        return Object.assign({}, state, {
            movementInventoryOwnershipData: action.updatedMovementOwnershipData,
            movementInventoryOwnershipUpdated: true
        });
    }
    case GET_CONTRACT_DATA: {
        return Object.assign({}, state, {
            contractData: Object.assign([], action.data.value),
            dropdown: action.data.value.map(item => {
                return {
                    id: item.contractId,
                    name: utilities.buildDocumentNumberPosition(item.documentNumber, item.position)
                };
            })
        });
    }
    case DISPLAY_DATA_DROPDOWN: {
        return Object.assign({}, state, {
            displayData: true,
            selectedContract: state.contractData.find(contract => contract.contractId === action.id)
        });
    }
    case CLEAR_SELECTED_CONTRACT:
        return Object.assign({}, state,
            {
                selectedContract: null,
                displayData: false
            });
    case ADD_COMMENT_REOPEN_TICKET: {
        return Object.assign({},
            state,
            {
                reopenComment: Object.assign({}, state, {
                    message: action.comment,
                    commentToggler: !state.commentToggler
                })
            });
    }
    case RECEIVE_TICKET_REOPEN_SUCCESS: {
        return Object.assign({}, state,
            {
                reopenSuccess: action.success,
                nodeDetails: {
                    ...state.nodeDetails,
                    ownershipStatus: 'REOPENED'
                }
            });
    }
    case START_EDIT:
        return Object.assign({}, state, { startEditToggler: !action.startEditToggler });
    case END_EDIT:
        return Object.assign({}, state, { endEditToggler: !action.endEditToggler });
    case UNLOCK_NODE:
        return Object.assign({}, state, { unlockNodeToggler: !action.unlockNodeToggler });
    case PUBLISHING_NODE:
        return Object.assign({}, state, {
            publishingNodeToggler: !action.publishingNodeToggler
        });
    case RECEIVE_OWNERS_FOR_INVENTORY:
        return Object.assign({}, state, {
            inventoryOwners: action.inventoryOwners,
            inventoryOwnersDataToggler: !state.inventoryOwnersDataToggler
        });
    case UPDATE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA: {
        const items = [...state.nodeMovementInventoryData];
        action.movementOwnershipInventoryData.forEach(item => {
            const index = state.nodeMovementInventoryData.findIndex(x => x.ownerId === item.ownerId && x.transactionId === item.transactionId);
            if (index < 0) {
                items.push(item);
            } else {
                items[index] = item;
            }
        });
        return Object.assign({}, state, {
            nodeMovementInventoryData: items,
            nodeMovementInventoryDataToggler: !state.nodeMovementInventoryDataToggler
        });
    }
    case RECEIVE_PUBLISH_OWNERSHIP_SUCCESS: {
        return Object.assign({}, state,
            {
                publishSuccess: action.success,
                movementInventoryOwnershipUpdated: false
            });
    }
    case EDITOR_INFORMATION: {
        return Object.assign({}, state,
            {
                editorInfo: action.editorInfo
            });
    }
    case ON_PUBLISH_NODE_OWNERSHIP: {
        return Object.assign({}, state,
            {
                publishOwnershipToggler: !action.publishOwnershipToggler
            });
    }
    case SET_SOURCE_NODE:
    case RECEIVE_SOURCE_NODE:
        return Object.assign({}, state,
            {
                sourceNodes: action.nodes
            });
    case SET_DESTINATION_NODE:
    case RECEIVE_DESTINATION_NODE:
        return Object.assign({}, state,
            {
                destinationNodes: action.nodes
            });
    case RECEIVE_SOURCE_PRODUCT:
        return Object.assign({}, state,
            {
                sourceProducts: action.products
            });
    case RECEIVE_DESTINATION_PRODUCT:
        return Object.assign({}, state,
            {
                destinationProducts: action.products
            });
    case CLEAR_SELECTED_DATA:
        return Object.assign({}, state,
            {
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
                // initialValues: []
            });
    case NODE_PUBLISH_SUCCESS:
        return Object.assign({}, state, {
            nodeOwnershipPublishSuccessToggler: !state.nodeOwnershipPublishSuccessToggler
        });
    case NODE_PUBLISH_FAILURE:
        return Object.assign({}, state, {
            isPublishing: false,
            nodeOwnershipPublishFailure: !state.nodeOwnershipPublishFailure
        });
    case RESET_NODE_IS_PUBLISHING:
        return Object.assign({}, state, {
            isPublishing: false
        });
    case SET_NODE_IS_PUBLISHING:
        return Object.assign({}, state, {
            isPublishing: true
        });
    case '@@redux-form/CHANGE':
        if (action.meta.form === 'createMovement') {
            if (action.meta.field === 'unit' || action.meta.field === 'reasonForChange' || action.meta.field === 'comment' || action.meta.field === 'movementDate') {
                return state;
            }
            return nodeOwnershipStateService.updateState(state, action.meta.field, action.payload);
        }
        return state;
    case RECEIVE_CONCILIATION_NODE: {
        return Object.assign({}, state,
            {
                conciliationSuccessToggler: !state.conciliationSuccessToggler
            });
    }
    case FAILURE_CONCILIATION_NODE: {
        return Object.assign({}, state,
            {
                conciliationErrorToggler: !state.conciliationErrorToggler
            });
    }
    default:
        return Object.assign({}, state, moreOwnershipNodeCases(action, state));
    }
};

export const ownershipNodeDetails = (state = initialState, action = {}) => {
    switch (action.type) {
    case INIT_UPDATE_FILTERS:
        return Object.assign({}, state, {
            movementInventoryfilters: Object.assign({}, state.movementInventoryfilters, action.filter),
            movementInventoryfilterToggler: !state.movementInventoryfilterToggler
        });
    case CLEAR_MOVEMENT_INVENTORY_FILTER:
        return Object.assign({}, state, {
            movementInventoryfilters: initialState.movementInventoryfilters
        });
    case RECEIVE_OWNERSHIPNODE_FOR_APPROVAL_EXCEPTION: {
        return Object.assign({}, state,
            {
                ownershipnodeApprovalErrorToggler: !state.ownershipnodeApprovalErrorToggler
            });
    }
    case RECEIVE_SEND_OWNERSHIP_NODE_FOR_APPROVAL: {
        return Object.assign({}, state,
            {
                ownershipnodeApprovalSuccessToggler: !state.ownershipnodeApprovalSuccessToggler
            });
    }
    default:
        return state;
    }
};
