import * as actions from '../../../../modules/administration/nodeConnection/network/actions.js';
import { apiService } from '../../../../common/services/apiService';
import { resourceProvider } from '../../../../common/services/resourceProvider.js';

describe('Actions for graphical configuration network', () => {
    it('should add unsaved connection on create', () => {
        const CREATE_UNSAVED_CONNECTION = 'CREATE_UNSAVED_CONNECTION';
        const unsavedConnection = {
            sourcePortId: 'out_2',
            targetPortId: 'in_1'
        };
        const action = actions.createUnsavedConnection(unsavedConnection);
        expect(action.type).toEqual(CREATE_UNSAVED_CONNECTION);
        expect(action.unsavedConnection).toEqual(unsavedConnection);
    });
    it('should get graphical network', () => {
        const REQUEST_GET_GRAPHICALNETWORK = 'REQUEST_GET_GRAPHICALNETWORK';
        const action = actions.getGraphicalNetwork(1, 1);

        expect(action.type).toEqual(REQUEST_GET_GRAPHICALNETWORK);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.node.getGraphicalNetwork(1, 1));
    });
    it('should receive graphical network', () => {
        const RECEIVE_GET_GRAPHICALNETWORK = 'RECEIVE_GET_GRAPHICALNETWORK';
        const nodeGraphicalNetwork = {
            1: {
                nodeName: 'Test 1',
                segment: 'Transporte',
                operator: 'Test 1',
                controlLimit: 0.35,
                acceptableBalancePercentage: 0.2,
                inputConnections: 1,
                outputConnections: 1,
                in_1: [{ sourceNodeId: 2, destinationNodeId: 1, state: 'Active' }]
            },
            2: {
                nodeName: 'Test 2',
                segment: 'Transporte',
                operator: 'Test 2',
                controlLimit: 0.35,
                acceptableBalancePercentage: 0.2,
                inputConnections: 0,
                outputConnections: 1,
                in_2: []
            }
        };
        const action = actions.receiveNodeNetwork(nodeGraphicalNetwork);
        expect(action.type).toEqual(RECEIVE_GET_GRAPHICALNETWORK);
        expect(action.graphicalNetwork).toEqual(nodeGraphicalNetwork);
    });
    it('should create a new connection', () => {
        const CREATE_NODE_CONNECTION = 'CREATE_NODE_CONNECTION';
        const connection = {
            sourceNodeId: 1,
            destinationNodeId: 2,
            description: 'Node 1-Node 2',
            isActive: true
        };
        const action = actions.createNodeConnection(connection);
        expect(action.type).toEqual(CREATE_NODE_CONNECTION);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.nodeConnection.createOrUpdate());
    });
    it('should receive on new connection creation', () => {
        const RECEIVE_CREATE_CONNECTION = 'RECEIVE_CREATE_CONNECTION';
        const action = actions.receiveCreateConnection(true);
        expect(action.type).toEqual(RECEIVE_CREATE_CONNECTION);
    });
    it('should fail create connection on failure', () => {
        const FAIL_CREATE_CONNECTION = 'FAIL_CREATE_CONNECTION';
        const json = {
            errorCodes: [{ message: resourceProvider.read('systemErrorMessage') }]
        };
        const action = actions.failedCreateConnection(json);
        expect(action.type).toEqual(FAIL_CREATE_CONNECTION);
        expect(action.errorMessage).toEqual(resourceProvider.read('systemErrorMessage'));
    });
    it('should remove connection', () => {
        const REMOVE_CONNECTION = 'REMOVE_CONNECTION';
        const connection = {
            sourcePortId: 'out_2',
            targetPortId: 'in_1'
        };
        const action = actions.removeConnection(connection);
        expect(action.type).toEqual(REMOVE_CONNECTION);
        expect(action.connection).toEqual(connection);
    });
    it('should update model connections', () => {
        const UPDATE_MODEL_CONNECTIONS = 'UPDATE_MODEL_CONNECTIONS';
        const modelConnections = [{
            sourcePortId: 'out_2',
            targetPortId: 'in_1'
        }];
        const action = actions.updateModelConnections(modelConnections);
        expect(action.type).toEqual(UPDATE_MODEL_CONNECTIONS);
        expect(action.modelConnections).toEqual(modelConnections);
    });
    it('should update model nodes', () => {
        const UPDATE_MODEL_NODES = 'UPDATE_MODEL_NODES';
        const modelNodes = [{
            nodeId: 1,
            nodeName: 'Test Node'
        }];
        const action = actions.updateModelNodes(modelNodes);
        expect(action.type).toEqual(UPDATE_MODEL_NODES);
        expect(action.modelNodes).toEqual(modelNodes);
    });
    it('should clear unsaved connection', () => {
        const CLEAR_UNSAVED_CONNECTION = 'CLEAR_UNSAVED_CONNECTION';
        const action = actions.clearUnsavedConnection();
        expect(action.type).toEqual(CLEAR_UNSAVED_CONNECTION);
    });
    it('should clear error message', () => {
        const CLEAR_ERROR_MESSAGE = 'CLEAR_ERROR_MESSAGE';
        const action = actions.clearErrorMessage();
        expect(action.type).toEqual(CLEAR_ERROR_MESSAGE);
    });
    it('should reset state', () => {
        const RESET_STATE = 'RESET_STATE';
        const action = actions.resetState();
        expect(action.type).toEqual(RESET_STATE);
    });
    it('should persist current graphical network', () => {
        const PERSIST_CURRENT_GRAPHICAL_NETWORK = 'PERSIST_CURRENT_GRAPHICAL_NETWORK';
        const action = actions.persistCurrentGraphicalNetwork();
        expect(action.type).toEqual(PERSIST_CURRENT_GRAPHICAL_NETWORK);
    });
    it('should set unsaved node', () => {
        const SET_UNSAVED_NODE = 'SET_UNSAVED_NODE';
        const action = actions.setUnsavedNode();
        expect(action.type).toEqual(SET_UNSAVED_NODE);
    });
    it('should remove unsaved node', () => {
        const REMOVE_UNSAVED_NODE = 'REMOVE_UNSAVED_NODE';
        const action = actions.removeUnsavedNode();
        expect(action.type).toEqual(REMOVE_UNSAVED_NODE);
    });
    it('should get graphical node', () => {
        const REQUEST_GET_GRAPHICALNODE = 'REQUEST_GET_GRAPHICALNODE';
        const action = actions.getGraphicalNode(1, 1);

        expect(action.type).toEqual(REQUEST_GET_GRAPHICALNODE);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.node.getGraphicalNetwork(1, 1));
    });
    it('should receive graphical networks', () => {
        const RECEIVE_GET_GRAPHICALNODE = 'RECEIVE_GET_GRAPHICALNODE';
        const nodeGraphicalNetwork = {
            1: {
                nodeName: 'Test 1',
                segment: 'Transporte',
                operator: 'Test 1',
                controlLimit: 0.35,
                acceptableBalancePercentage: 0.2,
                inputConnections: 1,
                outputConnections: 1,
                in_1: [{ sourceNodeId: 2, destinationNodeId: 1, state: 'Active' }]
            }
        };
        const action = actions.receiveNode(nodeGraphicalNetwork);
        expect(action.type).toEqual(RECEIVE_GET_GRAPHICALNODE);
        expect(action.graphicalNetwork).toEqual(nodeGraphicalNetwork);
    });
    it('should get all source node details', () => {
        const nodeGraphicalNetwork = {
            1: {
                nodeName: 'Test 1',
                segment: 'Transporte',
                operator: 'Test 1',
                controlLimit: 0.35,
                acceptableBalancePercentage: 0.2,
                inputConnections: 1,
                outputConnections: 1,
                in_1: [{ sourceNodeId: 2, destinationNodeId: 1, state: 'Active' }],
                out_1: [{ sourceNodeId: 1, destinationNodeId: 2, state: 'Active' }]
            }
        };
        const REQUEST_GET_ALLSOURCENODESDETAILS_GRAPHICALNETWORK = 'REQUEST_GET_ALLSOURCENODESDETAILS_GRAPHICALNETWORK';
        const action = actions.showAllSourceNodesDetails(1, nodeGraphicalNetwork);

        expect(action.type).toEqual(REQUEST_GET_ALLSOURCENODESDETAILS_GRAPHICALNETWORK);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.node.showAllSourceNodesDetails(1));
    });
    it('should get all destination node details', () => {
        const nodeGraphicalNetwork = {
            1: {
                nodeName: 'Test 1',
                segment: 'Transporte',
                operator: 'Test 1',
                controlLimit: 0.35,
                acceptableBalancePercentage: 0.2,
                inputConnections: 1,
                outputConnections: 1,
                in_1: [{ sourceNodeId: 2, destinationNodeId: 1, state: 'Active' }],
                out_1: [{ sourceNodeId: 1, destinationNodeId: 2, state: 'Active' }]
            }
        };
        const REQUEST_GET_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK = 'REQUEST_GET_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK';
        const action = actions.showAllDestinationNodesDetails(1, nodeGraphicalNetwork);

        expect(action.type).toEqual(REQUEST_GET_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.node.showAllDestinationNodesDetails(1));
    });
    it('should hide all source node details', () => {
        const nodeGraphicalNetwork = {
            1: {
                nodeName: 'Test 1',
                segment: 'Transporte',
                operator: 'Test 1',
                controlLimit: 0.35,
                acceptableBalancePercentage: 0.2,
                inputConnections: 1,
                outputConnections: 1,
                in_1: [{ sourceNodeId: 2, destinationNodeId: 1, state: 'Active' }],
                out_1: [{ sourceNodeId: 1, destinationNodeId: 2, state: 'Active' }]
            }
        };
        const HIDE_ALLSOURCENODESDETAILS_GRAPHICALNETWORK = 'HIDE_ALLSOURCENODESDETAILS_GRAPHICALNETWORK';
        const action = actions.hideAllSourceNodeDetails(1, nodeGraphicalNetwork);

        expect(action.type).toEqual(HIDE_ALLSOURCENODESDETAILS_GRAPHICALNETWORK);
    });
    it('should hide all destination node details', () => {
        const nodeGraphicalNetwork = {
            1: {
                nodeName: 'Test 1',
                segment: 'Transporte',
                operator: 'Test 1',
                controlLimit: 0.35,
                acceptableBalancePercentage: 0.2,
                inputConnections: 1,
                outputConnections: 1,
                in_1: [{ sourceNodeId: 2, destinationNodeId: 1, state: 'Active' }],
                out_1: [{ sourceNodeId: 1, destinationNodeId: 2, state: 'Active' }]
            }
        };
        const HIDE_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK = 'HIDE_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK';
        const action = actions.hideAllDestinationNodeDetails(1, nodeGraphicalNetwork);

        expect(action.type).toEqual(HIDE_ALLDESTINATIONNODESDETAILS_GRAPHICALNETWORK);
    });
    it('should fail update connection on failure', () => {
        const FAIL_UPDATE_CONNECTION_DETAIL = 'FAIL_UPDATE_CONNECTION_DETAIL';
        const json = {
            errorCodes: [{ message: resourceProvider.read('systemErrorMessage') }]
        };
        const action = actions.failedUpdateConnection(json);
        expect(action.type).toEqual(FAIL_UPDATE_CONNECTION_DETAIL);
        expect(action.errorMessage).toEqual(resourceProvider.read('systemErrorMessage'));
    });
    it('should clear connection to Delete', () => {
        const CLEAR_CONNECTION_TO_DELETE = 'CLEAR_CONNECTION_TO_DELETE';
        const action = actions.clearConnectionToDelete();
        expect(action.type).toEqual(CLEAR_CONNECTION_TO_DELETE);
    });
    it('should delete node connection', () => {
        const REQUEST_DELETE_NODECONNECTION = 'REQUEST_DELETE_NODECONNECTION';
        const sourceNodeId = 1;
        const destinationNodeId = 2;
        const action = actions.deleteNodeConnection(sourceNodeId, destinationNodeId);
        expect(action.type).toEqual(REQUEST_DELETE_NODECONNECTION);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.nodeConnection.deleteNodeConnection(sourceNodeId, destinationNodeId));
    });

    it('should update node connection detail', () => {
        const UPDATE_NODE_CONNECTION_DETAIL = 'UPDATE_NODE_CONNECTION_DETAIL';
        const connection = { connectionId: 1 };

        const action = actions.updateNodeConnectionDetail(connection);
        expect(action.type).toEqual(UPDATE_NODE_CONNECTION_DETAIL);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.nodeConnection.createOrUpdate());
        expect(action.fetchConfig.body).toEqual(connection);
    });

    it('should set node connection', () => {
        const SET_NODE_CONNECTION = 'SET_NODE_CONNECTION';
        const connectionToActive = { connectionId: 1 };
        const action = actions.setNodeConnection(connectionToActive);
        expect(action.type).toEqual(SET_NODE_CONNECTION);
        expect(action.connectionToActive).toEqual(connectionToActive);
    });

    it('should create connection to delete', () => {
        const CREATE_CONNECTION_TO_DELETE = 'CREATE_CONNECTION_TO_DELETE';
        const connection = { connectionId: 1 };
        const sourceNode = 1;
        const targetNode = 1;
        const action = actions.createConnectionToDelete(connection, sourceNode, targetNode);
        expect(action.type).toEqual(CREATE_CONNECTION_TO_DELETE);
        expect(action.sourceNode).toEqual(sourceNode);
        expect(action.targetNode).toEqual(targetNode);
    });

    it('should remove deleted connection', () => {
        const REMOVE_DELETED_CONNECTION = 'REMOVE_DELETED_CONNECTION';
        const action = actions.removeDeletedConnection();
        expect(action.type).toEqual(REMOVE_DELETED_CONNECTION);
    });
    it('should reset connection to delete', () => {
        const RESET_CONNECTION_TO_DELETE = 'RESET_CONNECTION_TO_DELETE';
        const action = actions.resetConnectionToDelete();
        expect(action.type).toEqual(RESET_CONNECTION_TO_DELETE);
    });
    it('should update connection state', () => {
        const status = 'status';
        const UPDATE_CONNECTION_STATE = 'UPDATE_CONNECTION_STATE';
        const action = actions.updateConnectionState(status);
        expect(action.state).toEqual(status);
        expect(action.type).toEqual(UPDATE_CONNECTION_STATE);
    });
    it('should fail deleted connection', () => {
        const FAIL_DELETE_CONNECTION = 'FAIL_DELETE_CONNECTION';
        const json = {
            errorCodes: [{ message: resourceProvider.read('systemErrorMessage') }]
        };
        const action = actions.failedDeleteConnection(json);
        expect(action.type).toEqual(FAIL_DELETE_CONNECTION);
        expect(action.errorMessage).toEqual(resourceProvider.read('systemErrorMessage'));
    });
    it('should receive enable connection', () => {
        const RECEIVE_ENABLE_CONNECTION = 'RECEIVE_ENABLE_CONNECTION';
        const action = actions.receiveEnableConnection();
        expect(action.type).toEqual(RECEIVE_ENABLE_CONNECTION);
    });
    it('should receive connection after update', () => {
        const RECEIVE_CONNECTION_AFTER_UPDATE = 'RECEIVE_CONNECTION_AFTER_UPDATE';
        const connection = { connectionId: 1 };
        const action = actions.receiveConnectionAfterUpdate(connection);
        expect(action.type).toEqual(RECEIVE_CONNECTION_AFTER_UPDATE);
        expect(action.connection).toEqual(connection);
    });
    it('should update rowversion for connection', () => {
        const UPDATE_ROW_VERSION_FOR_CONNECTION = 'UPDATE_ROW_VERSION_FOR_CONNECTION';
        const connection = { connectionId: 1 };
        const action = actions.updateRowVersionForConnectionUpdate(connection);
        expect(action.type).toEqual(UPDATE_ROW_VERSION_FOR_CONNECTION);
        expect(action.connection).toEqual(connection);
    });
    it('should confirm delete', () => {
        const CONFIRM_DELETE = 'CONFIRM_DELETE';
        const action = actions.confirmDelete();
        expect(action.type).toEqual(CONFIRM_DELETE);
    });
    it('should confirm enable', () => {
        const CONFIRM_ENABLE = 'CONFIRM_ENABLE';
        const action = actions.confirmEnable();
        expect(action.type).toEqual(CONFIRM_ENABLE);
    });
    it('should confirm delete', () => {
        const CONFIRM_DISABLE = 'CONFIRM_DISABLE';
        const action = actions.confirmDisable();
        expect(action.type).toEqual(CONFIRM_DISABLE);
    });
    it('should save graphical filter', () => {
        const SAVE_GRAPHICAL_FILTER = 'SAVE_GRAPHICAL_FILTER';
        const filters = { };
        const action = actions.saveGraphicalFilter(filters);
        expect(action.type).toEqual(SAVE_GRAPHICAL_FILTER);
        expect(action.filters).toEqual(filters);
    });
});
