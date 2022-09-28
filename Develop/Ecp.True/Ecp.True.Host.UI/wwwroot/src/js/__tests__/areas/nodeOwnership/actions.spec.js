import * as actions from '../../../modules/transportBalance/nodeOwnership/actions';
import { apiService } from '../../../common/services/apiService';

describe('Actions for Operational Node', () => {
    it('should initialize node error detail', () => {
        const REQUEST_OWNERSHIP_NODE_ERROR_DETAIL = 'REQUEST_OWNERSHIP_NODE_ERROR_DETAIL';
        const node = { id: 100 };
        const action = actions.initializeNodeErrorDetail(node);

        expect(action.type).toEqual(REQUEST_OWNERSHIP_NODE_ERROR_DETAIL);
        expect(action.node).toEqual(node);
    });

    it('Should request last ownership ticket', () => {
        const actionType = 'REQUEST_LAST_OPERATIONAL_TICKET';
        const ticket = { ticketId: 1234 };

        const action = actions.requestLastOperationalTicket();

        expect(action.type).toEqual(actionType);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ticket.getLastOperationalTicketsPerSegment());
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(ticket);
        const RECEIVE_LAST_OPERATIONAL_TICKET = 'RECEIVE_LAST_OPERATIONAL_TICKET';
        expect(receiveAction.type).toEqual(RECEIVE_LAST_OPERATIONAL_TICKET);
    });

    it('should receive last ownership ticket.', () => {
        const RECEIVE_LAST_OPERATIONAL_TICKET = 'RECEIVE_LAST_OPERATIONAL_TICKET';
        const ticket = { ticketId: 1234 };
        const action = actions.receiveLastOperationalTicket(ticket);

        expect(action.type).toEqual(RECEIVE_LAST_OPERATIONAL_TICKET);
        expect(action.ticket).toEqual(ticket);
    });

    it('should request owners for movement.', () => {
        const REQUEST_OWNERS_FOR_MOVEMENT = 'REQUEST_OWNERS_FOR_MOVEMENT';
        const action = actions.requestOwnersForMovement(10, 20, '1000245');
        expect(action.type).toEqual(REQUEST_OWNERS_FOR_MOVEMENT);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownershipNode.getOwnersForMovement(10, 20, '1000245'));
        expect(action.fetchConfig.success).toBeDefined();
    });

    it('should receive owners for movement.', () => {
        const RECEIVE_OWNERS_FOR_MOVEMENT = 'RECEIVE_OWNERS_FOR_MOVEMENT';
        const movementOwners = { OwnerId: 27 };
        const action = actions.receiveOwnersForMovement(movementOwners);
        expect(action.type).toEqual(RECEIVE_OWNERS_FOR_MOVEMENT);
        expect(action.movementOwners).toEqual(movementOwners);
    });

    it('should request owners for inventory.', () => {
        const REQUEST_OWNERS_FOR_INVENTORY = 'REQUEST_OWNERS_FOR_INVENTORY';
        const action = actions.requestOwnersForInventory(10, '1000245');
        expect(action.type).toEqual(REQUEST_OWNERS_FOR_INVENTORY);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownershipNode.getOwnersForInventory(10, '1000245'));
        expect(action.fetchConfig.success).toBeDefined();
    });

    it('should receive owners for inventory.', () => {
        const RECEIVE_OWNERS_FOR_INVENTORY = 'RECEIVE_OWNERS_FOR_INVENTORY';
        const inventoryOwners = { OwnerId: 28 };
        const action = actions.receiveOwnersForInventory(inventoryOwners);
        expect(action.type).toEqual(RECEIVE_OWNERS_FOR_INVENTORY);
        expect(action.inventoryOwners).toEqual(inventoryOwners);
    });

    it('should set movement inventory ownership data.', () => {
        const SET_MOVEMENT_INVENTORY_OWNERSHIP_DATA = 'SET_MOVEMENT_INVENTORY_OWNERSHIP_DATA';
        const movementInventoryOwnershipData = { OwnerId: 29 };
        const action = actions.setMovementInventoryOwnershipData(movementInventoryOwnershipData);
        expect(action.type).toEqual(SET_MOVEMENT_INVENTORY_OWNERSHIP_DATA);
        expect(action.movementInventoryOwnershipData).toEqual(movementInventoryOwnershipData);
    });

    it('should update movement ownership data.', () => {
        const UPDATE_MOVEMENT_OWNERSHIP_DATA = 'UPDATE_MOVEMENT_OWNERSHIP_DATA';
        const updatedMovementOwnershipData = { ownerId: 30 };
        const action = actions.updateMovementOwnershipData(updatedMovementOwnershipData);
        expect(action.type).toEqual(UPDATE_MOVEMENT_OWNERSHIP_DATA);
        expect(action.updatedMovementOwnershipData).toEqual(updatedMovementOwnershipData);
    });

    it('should update node movement inventory data', () => {
        const UPDATE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA = 'UPDATE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA';
        const action = actions.updateNodeMovementInventoryData();
        expect(action.type).toEqual(UPDATE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA);
    });

    it('should request publish ownership', () => {
        const REQUEST_PUBLISH_OWNERSHIP = 'REQUEST_PUBLISH_OWNERSHIP';
        const movementInventoryData = 'test';
        const action = actions.requestPublishOwnership(movementInventoryData);
        expect(action.type).toEqual(REQUEST_PUBLISH_OWNERSHIP);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownershipNode.publishOwnerships());
        expect(action.fetchConfig.success).toBeDefined();
    });

    it('should receive editor info', () => {
        const EDITOR_INFORMATION = 'EDITOR_INFORMATION';
        const editorInfo = 'test editor info';
        const action = actions.receiveEditorInfo(editorInfo);
        expect(action.type).toEqual(EDITOR_INFORMATION);
        expect(action.editorInfo).toEqual(editorInfo);
    });

    it('should execute on ownership node publish', () => {
        const ON_PUBLISH_NODE_OWNERSHIP = 'ON_PUBLISH_NODE_OWNERSHIP';
        const publishOwnershipToggler = true;
        const action = actions.onOwnershipNodePublish(publishOwnershipToggler);
        expect(action.type).toEqual(ON_PUBLISH_NODE_OWNERSHIP);
        expect(action.publishOwnershipToggler).toEqual(publishOwnershipToggler);
    });

    it('should request ownership node details', () => {
        const REQUEST_NODE_OWNERSHIP_DETAILS = 'REQUEST_NODE_OWNERSHIP_DETAILS';
        const node = { id: 100 };
        const ownershipNodeId = 100;
        const action = actions.requestNodeOwnershipDetails(ownershipNodeId);

        expect(action.type).toEqual(REQUEST_NODE_OWNERSHIP_DETAILS);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownershipNode.getById(ownershipNodeId));
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(node);
        const RECEIVE_NODE_OWNERSHIP_DETAILS = 'RECEIVE_NODE_OWNERSHIP_DETAILS';
        expect(receiveAction.type).toEqual(RECEIVE_NODE_OWNERSHIP_DETAILS);
    });

    it('should request ownership node movement inventory data', () => {
        const REQUEST_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA = 'REQUEST_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA';
        const node = { id: 100 };
        const ownershipNodeId = 100;
        const action = actions.requestOwnershipNodeMovementInventoryData(ownershipNodeId);

        expect(action.type).toEqual(REQUEST_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownershipNode.getOwnershipMovementInventoryData(ownershipNodeId));
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(node);
        const RECEIVE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA = 'RECEIVE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA';
        expect(receiveAction.type).toEqual(RECEIVE_OWNERSHIP_NODE_MOVEMENT_INVENTORY_DATA);
    });

    it('should add comment on ticket reopen', () => {
        const ADD_COMMENT_REOPEN_TICKET = 'ADD_COMMENT_REOPEN_TICKET';
        const comment = 'test';
        const action = actions.addComment(comment);

        expect(action.type).toEqual(ADD_COMMENT_REOPEN_TICKET);
        expect(action.comment).toEqual(comment);
    });

    it('should request to reopen the ticket', () => {
        const REQUEST_TICKET_REOPEN = 'REQUEST_TICKET_REOPEN';
        const success = true;
        const ownershipNodeId = 100;
        const message = 'test';
        const status = 'REOPEN';
        const action = actions.requestTicketReopen(ownershipNodeId, message, status);

        expect(action.type).toEqual(REQUEST_TICKET_REOPEN);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownershipNode.reopenTicket());
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(success);
        const RECEIVE_TICKET_REOPEN_SUCCESS = 'RECEIVE_TICKET_REOPEN_SUCCESS';
        expect(receiveAction.type).toEqual(RECEIVE_TICKET_REOPEN_SUCCESS);
    });

    it('should initialize filter update', () => {
        const INIT_UPDATE_FILTERS = 'INIT_UPDATE_FILTERS';
        const filter = {
            prodcut: 'CRUSCO',
            variableType: 'TOLERANCE',
            owner: 'ECOPETROL'
        };
        const action = actions.initUpdateFilters(filter);

        expect(action.type).toEqual(INIT_UPDATE_FILTERS);
        expect(action.filter).toEqual(filter);
    });

    it('should call start edit', () => {
        const START_EDIT = 'START_EDIT';
        const startEditToggler = true;
        const action = actions.startEdit(startEditToggler);

        expect(action.type).toEqual(START_EDIT);
        expect(action.startEditToggler).toEqual(startEditToggler);
    });

    it('should call end edit', () => {
        const END_EDIT = 'END_EDIT';
        const endEditToggler = true;
        const action = actions.endEdit(endEditToggler);

        expect(action.type).toEqual(END_EDIT);
        expect(action.endEditToggler).toEqual(endEditToggler);
    });

    it('should request node unlock', () => {
        const REQUEST_UNLOCK_NODE = 'REQUEST_UNLOCK_NODE';
        const action = actions.requestUnlockNode();

        expect(action.type).toEqual(REQUEST_UNLOCK_NODE);
    });

    it('should unlock node', () => {
        const UNLOCK_NODE = 'UNLOCK_NODE';
        const action = actions.acceptUnlockNode();

        expect(action.type).toEqual(UNLOCK_NODE);
    });

    it('should call on publishing node', () => {
        const PUBLISHING_NODE = 'PUBLISHING_NODE';
        const action = actions.onNodePublishing();

        expect(action.type).toEqual(PUBLISHING_NODE);
    });

    it('should set movement inventory ownership data', () => {
        const SET_MOVEMENT_INVENTORY_OWNERSHIP_DATA = 'SET_MOVEMENT_INVENTORY_OWNERSHIP_DATA';
        const movementInventoryOwnershipData = [{ ownerId: 1, ownershipPercentage: 100 }];
        const action = actions.setMovementInventoryOwnershipData(movementInventoryOwnershipData);

        expect(action.type).toEqual(SET_MOVEMENT_INVENTORY_OWNERSHIP_DATA);
        expect(action.movementInventoryOwnershipData).toEqual(movementInventoryOwnershipData);
    });

    it('should update movement inventory ownership data', () => {
        const SET_MOVEMENT_INVENTORY_OWNERSHIP_DATA = 'SET_MOVEMENT_INVENTORY_OWNERSHIP_DATA';
        const movementInventoryOwnershipData = [{ ownerId: 1, ownershipPercentage: 100 }];
        const action = actions.setMovementInventoryOwnershipData(movementInventoryOwnershipData);

        expect(action.type).toEqual(SET_MOVEMENT_INVENTORY_OWNERSHIP_DATA);
        expect(action.movementInventoryOwnershipData).toEqual(movementInventoryOwnershipData);
    });

    it('should request publish ownership', () => {
        const REQUEST_PUBLISH_OWNERSHIP = 'REQUEST_PUBLISH_OWNERSHIP';
        const movementInventoryData = [{
            movementTransactionId: 1,
            ownerId: 27,
            ownerName: 'ECOPETROL',
            ownershipPercentage: 100
        }];
        const action = actions.requestPublishOwnership(movementInventoryData);

        expect(action.type).toEqual(REQUEST_PUBLISH_OWNERSHIP);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownershipNode.publishOwnerships());
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(true);
        const RECEIVE_PUBLISH_OWNERSHIP_SUCCESS = 'RECEIVE_PUBLISH_OWNERSHIP_SUCCESS';
        expect(receiveAction.type).toEqual(RECEIVE_PUBLISH_OWNERSHIP_SUCCESS);
    });

    it('should receive editor information', () => {
        const EDITOR_INFORMATION = 'EDITOR_INFORMATION';
        const editorInfo = {};
        const action = actions.receiveEditorInfo(editorInfo);

        expect(action.type).toEqual(EDITOR_INFORMATION);
        expect(action.editorInfo).toEqual(editorInfo);
    });

    it('should call on ownership node publish', () => {
        const ON_PUBLISH_NODE_OWNERSHIP = 'ON_PUBLISH_NODE_OWNERSHIP';
        const publishOwnershipToggler = true;
        const action = actions.onOwnershipNodePublish(publishOwnershipToggler);

        expect(action.type).toEqual(ON_PUBLISH_NODE_OWNERSHIP);
        expect(action.publishOwnershipToggler).toEqual(publishOwnershipToggler);
    });

    it('should request source nodes', () => {
        const REQUEST_SOURCE_NODE = 'REQUEST_SOURCE_NODE';
        const destinationNodeId = 1;
        const nodes = { id: 100 };
        const action = actions.requestSourceNodes(destinationNodeId);

        expect(action.type).toEqual(REQUEST_SOURCE_NODE);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.nodeConnection.getSourceNodesByDestinationNode(destinationNodeId));
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(nodes);
        const RECEIVE_SOURCE_NODE = 'RECEIVE_SOURCE_NODE';
        expect(receiveAction.type).toEqual(RECEIVE_SOURCE_NODE);
    });

    it('should set source nodes', () => {
        const SET_SOURCE_NODE = 'SET_SOURCE_NODE';
        const nodes = { id: 100 };
        const action = actions.setSourceNodes(nodes);

        expect(action.type).toEqual(SET_SOURCE_NODE);
        expect(action.nodes).toEqual(nodes);
    });

    it('should request destination nodes', () => {
        const REQUEST_DESTINATION_NODE = 'REQUEST_DESTINATION_NODE';
        const sourceNodeId = 1;
        const nodes = { id: 100 };
        const action = actions.requestDestinationNodes(sourceNodeId);

        expect(action.type).toEqual(REQUEST_DESTINATION_NODE);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.nodeConnection.getDestinationNodesBySourceNode(sourceNodeId));
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(nodes);
        const RECEIVE_DESTINATION_NODE = 'RECEIVE_DESTINATION_NODE';
        expect(receiveAction.type).toEqual(RECEIVE_DESTINATION_NODE);
    });

    it('should set destination nodes', () => {
        const SET_DESTINATION_NODE = 'SET_DESTINATION_NODE';
        const nodes = { id: 100 };
        const action = actions.setDestinationNodes(nodes);

        expect(action.type).toEqual(SET_DESTINATION_NODE);
        expect(action.nodes).toEqual(nodes);
    });

    it('should request source products', () => {
        const REQUEST_SOURCE_PRODUCT = 'REQUEST_SOURCE_PRODUCT';
        const sourceNodeId = 1;
        const products = {
            value: {
                productId: 1,
                name: 'Test product'
            }
        };
        const action = actions.requestSourceProducts(sourceNodeId);

        expect(action.type).toEqual(REQUEST_SOURCE_PRODUCT);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.node.queryNodeProducts(sourceNodeId));
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(products);
        const RECEIVE_SOURCE_PRODUCT = 'RECEIVE_SOURCE_PRODUCT';
        expect(receiveAction.type).toEqual(RECEIVE_SOURCE_PRODUCT);
    });

    it('should request destination products', () => {
        const REQUEST_DESTINATION_PRODUCT = 'REQUEST_DESTINATION_PRODUCT';
        const destinationNodeId = 1;
        const products = {
            value: {
                productId: 1,
                name: 'Test product'
            }
        };
        const action = actions.requestDestinationProducts(destinationNodeId);

        expect(action.type).toEqual(REQUEST_DESTINATION_PRODUCT);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.node.queryNodeProducts(destinationNodeId));
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(products);
        const RECEIVE_DESTINATION_PRODUCT = 'RECEIVE_DESTINATION_PRODUCT';
        expect(receiveAction.type).toEqual(RECEIVE_DESTINATION_PRODUCT);
    });

    it('should set destination product', () => {
        const SET_DESTINATION_PRODUCT = 'SET_DESTINATION_PRODUCT';
        const product = 'test product';
        const action = actions.setDestinationProducts(product);

        expect(action.type).toEqual(SET_DESTINATION_PRODUCT);
        expect(action.product).toEqual(product);
    });

    it('should update current volume control', () => {
        const UPDATE_CURRENT_VOLUME_CONTROL = 'UPDATE_CURRENT_VOLUME_CONTROL';
        const totalVolume = 1000;
        const action = actions.updateCurrentVolumeControl(totalVolume);

        expect(action.type).toEqual(UPDATE_CURRENT_VOLUME_CONTROL);
        expect(action.totalVolume).toEqual(totalVolume);
    });


    it('should set source product', () => {
        const SET_SOURCE_PRODUCT = 'SET_SOURCE_PRODUCT';
        const product = 'test product';
        const action = actions.setSourceProducts(product);

        expect(action.type).toEqual(SET_SOURCE_PRODUCT);
        expect(action.product).toEqual(product);
    });

    it('should request contract data for new movement', () => {
        const REQUEST_CONTRACT_DATA = 'REQUEST_CONTRACT_DATA';
        const selectedData = {
            sourceProduct: {
                productId: 1
            },
            destinationProduct: {
                productId: 1
            },
            sourceNodes: {
                nodeId: 1
            },
            destinationNodes: {
                nodeId: 1
            },
            sourceProduct: {
                productId: '123'
            },
            movementType: {
                elementId: 123
            }
        };

        const contractData = {
            contractId: 1,
            documentNumber: 123,
            position: 1
        };

        const action = actions.requestContractData(selectedData, '05/05/2020 12:00:00.000z');

        expect(action.type).toEqual(REQUEST_CONTRACT_DATA);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownershipNode.getContractsForNewMovementsForNodeOwnership(selectedData, '05/05/2020 12:00:00.000z'));
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(contractData);
        const RECEIVE_CONTRACT_DATA = 'RECEIVE_CONTRACT_DATA';
        expect(receiveAction.type).toEqual(RECEIVE_CONTRACT_DATA);
    });

    it('should request contract data for edit movement', () => {
        const SET_CONTRACT_DATA = 'SET_CONTRACT_DATA';
        const movement = {
            sourceProductId: 1,
            sourceNodeId: 1,
            sourceProductId: '123' };

        const contractData = {
            contractId: 1,
            documentNumber: 123,
            position: 1
        };

        const action = actions.setContractData(movement);

        expect(action.type).toEqual(SET_CONTRACT_DATA);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownershipNode.editContracts(movement));
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(contractData);
        const GET_CONTRACT_DATA = 'GET_CONTRACT_DATA';
        expect(receiveAction.type).toEqual(GET_CONTRACT_DATA);
    });

    it('should display dropdown', () =>{
        const DISPLAY_DATA_DROPDOWN = 'DISPLAY_DATA_DROPDOWN';
        const id = 1;
        const action = actions.displayDataDropdown(id);

        expect(action.type).toEqual(DISPLAY_DATA_DROPDOWN);
        expect(action.id).toEqual(id);
    });

    it('should clear selected data', () =>{
        const CLEAR_SELECTED_DATA = 'CLEAR_SELECTED_DATA';
        const action = actions.clearSelectedData();

        expect(action.type).toEqual(CLEAR_SELECTED_DATA);
    });

    it('should clear ownership data', () =>{
        const CLEAR_OWNERSHIP_DATA = 'CLEAR_OWNERSHIP_DATA';
        const action = actions.clearOwnershipData();

        expect(action.type).toEqual(CLEAR_OWNERSHIP_DATA);
    });

    it('should set date', () =>{
        const SET_DATE = 'SET_DATE';
        const action = actions.setDate();

        expect(action.type).toEqual(SET_DATE);
    });

    it('should send ownership node for approval', () => {
        const REQUEST_SEND_OWNERSHIP_NODE_FOR_APPROVAL = 'REQUEST_SEND_OWNERSHIP_NODE_FOR_APPROVAL';
        const ownershipNodeId = 100;

        const contractData = {
            contractId: 1,
            documentNumber: 123,
            position: 1
        };

        const action = actions.requestSendOwnershipNodeForApproval(ownershipNodeId);

        expect(action.type).toEqual(REQUEST_SEND_OWNERSHIP_NODE_FOR_APPROVAL);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.ownershipNode.sendOwnershipNodeForApproval(ownershipNodeId));
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(true);
        const RECEIVE_SEND_OWNERSHIP_NODE_FOR_APPROVAL = 'RECEIVE_SEND_OWNERSHIP_NODE_FOR_APPROVAL';
        expect(receiveAction.type).toEqual(RECEIVE_SEND_OWNERSHIP_NODE_FOR_APPROVAL);
    });

    it('should clear the selected contract', () => {
        const CLEAR_SELECTED_CONTRACT = 'CLEAR_SELECTED_CONTRACT';
        const action = actions.clearSelectedContract();
        expect(action.type).toEqual(CLEAR_SELECTED_CONTRACT);
    });

    it('Clear movement inventory filter', () =>{
        const CLEAR_MOVEMENT_INVENTORY_FILTER = 'CLEAR_MOVEMENT_INVENTORY_FILTER';
        const action = actions.clearMovementInventoryFilter();

        expect(action.type).toEqual(CLEAR_MOVEMENT_INVENTORY_FILTER);
    });

    it('should request REQUEST_CONCILIATION_TICKET', () => {
        const ticketId = 123;
        const nodeId = 456;
        const action = actions.requestConciliationNode(ticketId, nodeId);

        expect(action.type).toEqual(actions.REQUEST_CONCILIATION_NODE);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.conciliation.requestConciliation(ticketId, nodeId));
        expect(action.fetchConfig.success).toBeDefined();
        expect(action.fetchConfig.failure).toBeDefined();

        const receiveSuccessAction = action.fetchConfig.success();
        expect(receiveSuccessAction.type).toEqual(actions.RECEIVE_CONCILIATION_NODE);

        const receiveFailAction = action.fetchConfig.failure({});
        expect(receiveFailAction.type).toEqual(actions.FAILURE_CONCILIATION_NODE);
        expect(receiveFailAction.response).toEqual({});
    });
});
