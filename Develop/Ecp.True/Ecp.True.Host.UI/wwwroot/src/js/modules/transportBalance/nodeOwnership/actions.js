import { apiService } from '../../../common/services/apiService.js';
export const REQUEST_LAST_OPERATIONAL_TICKET = 'REQUEST_LAST_OPERATIONAL_TICKET';
export const RECEIVE_LAST_OPERATIONAL_TICKET = 'RECEIVE_LAST_OPERATIONAL_TICKET';
export const REQUEST_OWNERSHIP_NODE_ERROR_DETAIL = 'REQUEST_OWNERSHIP_NODE_ERROR_DETAIL';
export const RECEIVE_NODE_OWNERSHIP_DETAILS = 'RECEIVE_NODE_OWNERSHIP_DETAILS';
export const REQUEST_NODE_OWNERSHIP_DETAILS = 'REQUEST_NODE_OWNERSHIP_DETAILS';
export const INIT_OWNERSHIP_DETAILS = 'INIT_OWNERSHIP_DETAILS';
export const REQUEST_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA = 'REQUEST_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA';
export const RECEIVE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA = 'RECEIVE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA';

export const REQUEST_OWNERS_FOR_MOVEMENT = 'REQUEST_OWNERS_FOR_MOVEMENT';
export const RECEIVE_OWNERS_FOR_MOVEMENT = 'RECEIVE_OWNERS_FOR_MOVEMENT';
export const ADD_COMMENT_REOPEN_TICKET = 'ADD_COMMENT_REOPEN_TICKET';
const REQUEST_TICKET_REOPEN = 'REQUEST_TICKET_REOPEN';
export const RECEIVE_TICKET_REOPEN_SUCCESS = 'RECEIVE_TICKET_REOPEN_SUCCESS';
export const SET_MOVEMENT_INVENTORY_OWNERSHIP_DATA = 'SET_MOVEMENT_INVENTORY_OWNERSHIP_DATA';
export const UPDATE_MOVEMENT_OWNERSHIP_DATA = 'UPDATE_MOVEMENT_OWNERSHIP_DATA';
export const UPDATE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA = 'UPDATE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA';

export const REQUEST_OWNERS_FOR_INVENTORY = 'REQUEST_OWNERS_FOR_INVENTORY';
export const RECEIVE_OWNERS_FOR_INVENTORY = 'RECEIVE_OWNERS_FOR_INVENTORY';
export const INIT_UPDATE_FILTERS = 'INIT_UPDATE_FILTERS';
const REQUEST_PUBLISH_OWNERSHIP = 'REQUEST_PUBLISH_OWNERSHIP';
export const RECEIVE_PUBLISH_OWNERSHIP_SUCCESS = 'RECEIVE_PUBLISH_OWNERSHIP_SUCCESS';

export const START_EDIT = 'START_EDIT';
export const END_EDIT = 'END_EDIT';
export const REQUEST_UNLOCK_NODE = 'REQUEST_UNLOCK_NODE';
export const UNLOCK_NODE = 'UNLOCK_NODE';
export const EDITOR_INFORMATION = 'EDITOR_INFORMATION';
export const ON_PUBLISH_NODE_OWNERSHIP = 'ON_PUBLISH_NODE_OWNERSHIP';
export const RECEIVE_SOURCE_NODE = 'RECEIVE_SOURCE_NODE';
export const REQUEST_SOURCE_NODE = 'REQUEST_SOURCE_NODE';
export const SET_SOURCE_NODE = 'SET_SOURCE_NODE';
export const RECEIVE_DESTINATION_NODE = 'RECEIVE_DESTINATION_NODE';
export const REQUEST_DESTINATION_NODE = 'REQUEST_DESTINATION_NODE';
export const SET_DESTINATION_NODE = 'SET_DESTINATION_NODE';
export const RECEIVE_SOURCE_PRODUCT = 'RECEIVE_SOURCE_PRODUCT';
export const REQUEST_SOURCE_PRODUCT = 'REQUEST_SOURCE_PRODUCT';
export const RECEIVE_DESTINATION_PRODUCT = 'RECEIVE_DESTINATION_PRODUCT';
export const REQUEST_DESTINATION_PRODUCT = 'REQUEST_DESTINATION_PRODUCT';
export const UPDATE_SELECTED_DATA = 'UPDATE_SELECTED_DATA';
export const CLEAR_SELECTED_DATA = 'CLEAR_SELECTED_DATA';
export const SET_DESTINATION_PRODUCT = 'SET_DESTINATION_PRODUCT';
export const CLEAR_OWNERSHIP_DATA = 'CLEAR_OWNERSHIP_DATA';
export const SET_SOURCE_PRODUCT = 'SET_SOURCE_PRODUCT';
export const UPDATE_CURRENT_VOLUME_CONTROL = 'UPDATE_CURRENT_VOLUME_CONTROL';
const REQUEST_SEND_OWNERSHIP_NODE_FOR_APPROVAL = 'REQUEST_SEND_OWNERSHIP_NODE_FOR_APPROVAL';
export const RECEIVE_SEND_OWNERSHIP_NODE_FOR_APPROVAL = 'RECEIVE_SEND_OWNERSHIP_NODE_FOR_APPROVAL';
export const RECEIVE_OWNERSHIPNODE_FOR_APPROVAL_EXCEPTION = 'RECEIVE_OWNERSHIPNODE_FOR_APPROVAL_EXCEPTION';
export const SET_DATE = 'SET_DATE';
export const PUBLISHING_NODE = 'PUBLISHING_NODE';
export const REQUEST_CONTRACT_DATA = 'REQUEST_CONTRACT_DATA';
export const RECEIVE_CONTRACT_DATA = 'RECEIVE_CONTRACT_DATA';
export const SET_CONTRACT_DATA = 'SET_CONTRACT_DATA';
export const GET_CONTRACT_DATA = 'GET_CONTRACT_DATA';
export const DISPLAY_DATA_DROPDOWN = 'DISPLAY_DATA_DROPDOWN';
export const CLEAR_SELECTED_CONTRACT = 'CLEAR_SELECTED_CONTRACT';
export const CLEAR_MOVEMENT_INVENTORY_FILTER = 'CLEAR_MOVEMENT_INVENTORY_FILTER';
export const NODE_PUBLISH_SUCCESS = 'NODE_PUBLISH_SUCCESS';
export const NODE_PUBLISH_FAILURE = 'NODE_PUBLISH_FAILURE';
export const RESET_NODE_IS_PUBLISHING = 'RESET_NODE_IS_PUBLISHING';
export const SET_NODE_IS_PUBLISHING = 'SET_NODE_IS_PUBLISHING';

export const RECEIVE_CONCILIATION_NODE = 'RECEIVE_CONCILIATION_NODE';
export const FAILURE_CONCILIATION_NODE = 'FAILURE_CONCILIATION_NODE';
export const REQUEST_CONCILIATION_NODE = 'REQUEST_CONCILIATION_NODE';

export const receiveLastOperationalTicket = ticket => {
    return {
        type: RECEIVE_LAST_OPERATIONAL_TICKET,
        ticket
    };
};
export const requestLastOperationalTicket = () => {
    return {
        type: REQUEST_LAST_OPERATIONAL_TICKET,
        fetchConfig: {
            path: apiService.ticket.getLastOperationalTicketsPerSegment(),
            success: receiveLastOperationalTicket
        }
    };
};

export const initializeNodeErrorDetail = node => {
    return {
        type: REQUEST_OWNERSHIP_NODE_ERROR_DETAIL,
        node
    };
};

export const receiveNodeOwnershipDetails = nodeDetails => {
    return {
        type: RECEIVE_NODE_OWNERSHIP_DETAILS,
        nodeDetails
    };
};

export const initOwnershipDetails = () => {
    return {
        type: INIT_OWNERSHIP_DETAILS
    };
};
export const requestNodeOwnershipDetails = ownershipNodeId => {
    return {
        type: REQUEST_NODE_OWNERSHIP_DETAILS,
        fetchConfig: {
            showProgress: false,
            path: apiService.ownershipNode.getById(ownershipNodeId),
            success: receiveNodeOwnershipDetails
        }
    };
};

export const receiveOwnershipNodeMovementInventoryData = movementInventoryData => {
    return {
        type: RECEIVE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA,
        movementInventoryData
    };
};

export const requestOwnershipNodeMovementInventoryData = ownershipNodeId => {
    return {
        type: REQUEST_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA,
        fetchConfig: {
            path: apiService.ownershipNode.getOwnershipMovementInventoryData(ownershipNodeId),
            success: receiveOwnershipNodeMovementInventoryData
        }
    };
};

export const receiveOwnersForMovement = movementOwners => {
    return {
        type: RECEIVE_OWNERS_FOR_MOVEMENT,
        movementOwners
    };
};

export const requestOwnersForMovement = (sourceNodeId, DestinationNodeId, productId) => {
    return {
        type: REQUEST_OWNERS_FOR_MOVEMENT,
        fetchConfig: {
            path: apiService.ownershipNode.getOwnersForMovement(sourceNodeId, DestinationNodeId, productId),
            success: receiveOwnersForMovement
        }
    };
};

export const receiveOwnersForInventory = inventoryOwners => {
    return {
        type: RECEIVE_OWNERS_FOR_INVENTORY,
        inventoryOwners
    };
};

export const requestOwnersForInventory = (nodeId, productId) => {
    return {
        type: REQUEST_OWNERS_FOR_INVENTORY,
        fetchConfig: {
            path: apiService.ownershipNode.getOwnersForInventory(nodeId, productId),
            success: receiveOwnersForInventory
        }
    };
};

export const addComment = comment => {
    return {
        type: ADD_COMMENT_REOPEN_TICKET,
        comment
    };
};

const receiveTicketReopenSuccess = success => {
    return {
        type: RECEIVE_TICKET_REOPEN_SUCCESS,
        success
    };
};

export const requestTicketReopen = (ownershipNodeId, message, status) => {
    return {
        type: REQUEST_TICKET_REOPEN,
        fetchConfig: {
            path: apiService.ownershipNode.reopenTicket(),
            method: 'PUT',
            body: {
                ownershipNodeId: ownershipNodeId,
                message: message,
                status: status
            },
            success: receiveTicketReopenSuccess
        }
    };
};

export const initUpdateFilters = filter => {
    return {
        type: INIT_UPDATE_FILTERS,
        filter
    };
};

export const startEdit = startEditToggler => {
    return { type: START_EDIT, startEditToggler };
};

export const endEdit = endEditToggler => {
    return { type: END_EDIT, endEditToggler };
};

export const requestUnlockNode = requestUnlockToggler => {
    return { type: REQUEST_UNLOCK_NODE, requestUnlockToggler };
};

export const acceptUnlockNode = unlockNodeToggler => {
    return { type: UNLOCK_NODE, unlockNodeToggler };
};

export const onNodePublishing = publishingNodeToggler => {
    return { type: PUBLISHING_NODE, publishingNodeToggler };
};

export const setMovementInventoryOwnershipData = movementInventoryOwnershipData => {
    return {
        type: SET_MOVEMENT_INVENTORY_OWNERSHIP_DATA,
        movementInventoryOwnershipData
    };
};

export const getContractData = data => {
    return {
        type: GET_CONTRACT_DATA,
        data
    };
};
export const setContractData = movementInventoryOwnershipData => {
    return {
        type: SET_CONTRACT_DATA,
        fetchConfig: {
            path: apiService.ownershipNode.editContracts(movementInventoryOwnershipData),
            success: data => getContractData(data)
        }
    };
};
export const displayDataDropdown = id => {
    return {
        type: DISPLAY_DATA_DROPDOWN,
        id
    };
};
export const clearSelectedContract = () => {
    return {
        type: CLEAR_SELECTED_CONTRACT
    };
};
export const updateMovementOwnershipData = updatedMovementOwnershipData => {
    return {
        type: UPDATE_MOVEMENT_OWNERSHIP_DATA,
        updatedMovementOwnershipData
    };
};

export const updateNodeMovementInventoryData = movementOwnershipInventoryData => {
    return {
        type: UPDATE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA,
        movementOwnershipInventoryData
    };
};


const receivePublishOwnershipSuccess = success => {
    return {
        type: RECEIVE_PUBLISH_OWNERSHIP_SUCCESS,
        success
    };
};

export const requestPublishOwnership = movementInventoryData => {
    return {
        type: REQUEST_PUBLISH_OWNERSHIP,
        fetchConfig: {
            path: apiService.ownershipNode.publishOwnerships(),
            method: 'POST',
            body: movementInventoryData,
            success: receivePublishOwnershipSuccess
        }
    };
};

export const receiveEditorInfo = editorInfo => {
    return {
        type: EDITOR_INFORMATION,
        editorInfo
    };
};

export const onOwnershipNodePublish = publishOwnershipToggler => {
    return { type: ON_PUBLISH_NODE_OWNERSHIP, publishOwnershipToggler };
};

const receiveSourceNodes = nodes => {
    return {
        type: RECEIVE_SOURCE_NODE,
        nodes
    };
};

export const requestSourceNodes = destinationNodeId => {
    return {
        type: REQUEST_SOURCE_NODE,
        fetchConfig: {
            path: apiService.nodeConnection.getSourceNodesByDestinationNode(destinationNodeId),
            success: nodes => receiveSourceNodes(nodes.value)
        }
    };
};

export const setSourceNodes = nodes => {
    return {
        type: SET_SOURCE_NODE,
        nodes
    };
};

const receiveDestinationNodes = nodes => {
    return {
        type: RECEIVE_DESTINATION_NODE,
        nodes
    };
};

export const requestDestinationNodes = sourceNodeId => {
    return {
        type: REQUEST_DESTINATION_NODE,
        fetchConfig: {
            path: apiService.nodeConnection.getDestinationNodesBySourceNode(sourceNodeId),
            success: nodes => receiveDestinationNodes(nodes.value)
        }
    };
};

export const setDestinationNodes = nodes => {
    return {
        type: SET_DESTINATION_NODE,
        nodes
    };
};

const receiveSourceProducts = products => {
    return {
        type: RECEIVE_SOURCE_PRODUCT,
        products
    };
};

export const requestSourceProducts = sourceNodeId => {
    return {
        type: REQUEST_SOURCE_PRODUCT,
        fetchConfig: {
            path: apiService.node.queryNodeProducts(sourceNodeId),
            success: products => receiveSourceProducts(products.value)
        }
    };
};

const receiveDestinationProducts = products => {
    return {
        type: RECEIVE_DESTINATION_PRODUCT,
        products
    };
};

export const requestDestinationProducts = destinationNodeId => {
    return {
        type: REQUEST_DESTINATION_PRODUCT,
        fetchConfig: {
            path: apiService.node.queryNodeProducts(destinationNodeId),
            success: products => receiveDestinationProducts(products.value)
        }
    };
};

export const clearSelectedData = () => {
    return {
        type: CLEAR_SELECTED_DATA
    };
};

export const setDestinationProducts = product => {
    return {
        type: SET_DESTINATION_PRODUCT,
        product
    };
};

export const updateCurrentVolumeControl = totalVolume => {
    return {
        type: UPDATE_CURRENT_VOLUME_CONTROL,
        totalVolume
    };
};

export const receiveSendOwnershipNodeForApproval = success => {
    return {
        type: RECEIVE_SEND_OWNERSHIP_NODE_FOR_APPROVAL,
        success
    };
};

export const receiveOwnershipNodeForApprovalException = () => {
    return {
        type: RECEIVE_OWNERSHIPNODE_FOR_APPROVAL_EXCEPTION
    };
};

export const requestSendOwnershipNodeForApproval = ownershipNodeId => {
    return {
        type: REQUEST_SEND_OWNERSHIP_NODE_FOR_APPROVAL,
        fetchConfig: {
            path: apiService.ownershipNode.sendOwnershipNodeForApproval(ownershipNodeId),
            method: 'POST',
            success: receiveSendOwnershipNodeForApproval,
            failure: receiveOwnershipNodeForApprovalException
        }
    };
};

export const setSourceProducts = product => {
    return {
        type: SET_SOURCE_PRODUCT,
        product
    };
};

export const clearOwnershipData = () => {
    return {
        type: CLEAR_OWNERSHIP_DATA
    };
};

export const setDate = () => {
    return {
        type: SET_DATE
    };
};

export const receiveContractData = contracts => {
    return {
        type: RECEIVE_CONTRACT_DATA,
        contracts
    };
};

export const requestContractData = (selectedData, date) => {
    return {
        type: REQUEST_CONTRACT_DATA,
        fetchConfig: {
            path: apiService.ownershipNode.getContractsForNewMovementsForNodeOwnership(selectedData, date),
            success: contracts => receiveContractData(contracts.value)
        }
    };
};

export const clearMovementInventoryFilter = () => {
    return {
        type: CLEAR_MOVEMENT_INVENTORY_FILTER
    };
};

export const nodePublishSuccess = () => {
    return {
        type: NODE_PUBLISH_SUCCESS
    };
};

export const nodePublishFailure = () => {
    return {
        type: NODE_PUBLISH_FAILURE
    };
};

export const resetNodeIsPublishing = () => {
    return {
        type: RESET_NODE_IS_PUBLISHING
    };
};

export const setIsPublishing = () => {
    return {
        type: SET_NODE_IS_PUBLISHING
    };
};

export const receiveConciliationNode = () => {
    return {
        type: RECEIVE_CONCILIATION_NODE
    };
};
export const failureConciliationNode = response => {
    return {
        type: FAILURE_CONCILIATION_NODE,
        response
    };
};
export const requestConciliationNode = data => {
    return {
        type: REQUEST_CONCILIATION_NODE,
        fetchConfig: {
            path: apiService.conciliation.requestConciliation(),
            method: 'POST',
            body: data,
            success: receiveConciliationNode,
            failure: failureConciliationNode
        }
    };
};
