import * as actions from '../../../../modules/administration/transferPoints/operative/actions';
import { apiService } from '../../../../common/services/apiService';

it('should initialize update transfer point row', () => {
    const transferPointRow = {};
    const isUpdate = true;
    const action = actions.initTransferPointRow(transferPointRow, isUpdate);
    const INIT_UPDATE_TRANSFER_POINT_ROW = 'INIT_UPDATE_TRANSFER_POINT_ROW';
    const mock_action = {
        type: INIT_UPDATE_TRANSFER_POINT_ROW,
        row: transferPointRow,
        isUpdate
    };
    expect(action).toEqual(mock_action);
});

it('should initialize delete transfer point row', () => {
    const transferPointRow = {};
    const isUpdate = false;
    const action = actions.initTransferPointRow(transferPointRow, isUpdate);
    const INIT_DELETE_TRANSFER_POINT_ROW = 'INIT_DELETE_TRANSFER_POINT_ROW';
    const mock_action = {
        type: INIT_DELETE_TRANSFER_POINT_ROW,
        row: transferPointRow,
        isUpdate
    };
    expect(action).toEqual(mock_action);
});

it('should request create transfer point', () => {
    const transferPointRow = {};
    const method = 'POST';
    const REQUEST_UPDATE_TRANSFER_POINT = 'REQUEST_UPDATE_TRANSFER_POINT';
    const action = actions.requestUpdateTransferPoint(transferPointRow, method);

    expect(action.type).toEqual(REQUEST_UPDATE_TRANSFER_POINT);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.nodeRelationship.update('operatives'));
    expect(action.fetchConfig.body).toEqual(transferPointRow);

    expect(action.fetchConfig.method).toEqual(method);
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_CREATE_TRANSFER_POINT = 'RECEIVE_CREATE_TRANSFER_POINT';
    expect(receiveAction.type).toEqual(RECEIVE_CREATE_TRANSFER_POINT);
    expect(receiveAction.success).toEqual(true);
});

it('should request delete transfer point', () => {
    const transferPointRow = {};
    const method = 'DELETE';
    const REQUEST_UPDATE_TRANSFER_POINT = 'REQUEST_UPDATE_TRANSFER_POINT';
    const action = actions.requestUpdateTransferPoint(transferPointRow, method);

    expect(action.type).toEqual(REQUEST_UPDATE_TRANSFER_POINT);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.nodeRelationship.update('operatives'));
    expect(action.fetchConfig.body).toEqual(transferPointRow);

    expect(action.fetchConfig.method).toEqual(method);
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_DELETE_TRANSFER_POINT = 'RECEIVE_DELETE_TRANSFER_POINT';
    expect(receiveAction.type).toEqual(RECEIVE_DELETE_TRANSFER_POINT);
    expect(receiveAction.success).toEqual(true);
});

it('should request update transfer point', () => {
    const transferPointRow = {};
    const method = 'PUT';
    const REQUEST_UPDATE_TRANSFER_POINT = 'REQUEST_UPDATE_TRANSFER_POINT';
    const action = actions.requestUpdateTransferPoint(transferPointRow, method);

    expect(action.type).toEqual(REQUEST_UPDATE_TRANSFER_POINT);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.nodeRelationship.update('operatives'));
    expect(action.fetchConfig.body).toEqual(transferPointRow);

    expect(action.fetchConfig.method).toEqual(method);
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_UPDATE_TRANSFER_POINT = 'RECEIVE_UPDATE_TRANSFER_POINT';
    expect(receiveAction.type).toEqual(RECEIVE_UPDATE_TRANSFER_POINT);
    expect(receiveAction.success).toEqual(true);
});

it('should request transfer source nodes', () => {
    const GET_TRANSFER_SOURCE_NODES = 'GET_TRANSFER_SOURCE_NODES';
    const action = actions.getTransferSourceNodes();

    expect(action.type).toEqual(GET_TRANSFER_SOURCE_NODES);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.nodeRelationship.queryTransferSourceNodes(false));

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_TRANSFER_SOURCE_NODES = 'RECEIVE_TRANSFER_SOURCE_NODES';
    expect(receiveAction.type).toEqual(RECEIVE_TRANSFER_SOURCE_NODES);
    expect(receiveAction.nodes).toEqual(true);
});

it('should request transfer destination nodes', () => {
    const sourceNodeId = 55881;
    const GET_TRANSFER_DESTINATION_NODES = 'GET_TRANSFER_DESTINATION_NODES';
    const action = actions.getTransferDestinationNodes(sourceNodeId);

    expect(action.type).toEqual(GET_TRANSFER_DESTINATION_NODES);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.nodeRelationship.queryTransferDestinationNodes(sourceNodeId, false));

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_TRANSFER_DESTINATION_NODES = 'RECEIVE_TRANSFER_DESTINATION_NODES';
    expect(receiveAction.type).toEqual(RECEIVE_TRANSFER_DESTINATION_NODES);
    expect(receiveAction.nodes).toEqual(true);
});

it('should request products', () => {
    const nodeId = 55881;
    const GET_TRANSFER_SOURCE_PRODUCTS = 'GET_TRANSFER_SOURCE_PRODUCTS';
    const action = actions.getTransferSourceProducts(nodeId);

    expect(action.type).toEqual(GET_TRANSFER_SOURCE_PRODUCTS);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.node.queryNodeProducts(nodeId));

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_TRANSFER_SOURCE_PRODUCTS = 'RECEIVE_TRANSFER_SOURCE_PRODUCTS';
    expect(receiveAction.type).toEqual(RECEIVE_TRANSFER_SOURCE_PRODUCTS);
    expect(receiveAction.products).toEqual(true);
});

it('should request source node type', () => {
    const nodeId = 55881;
    const isSource = true;
    const GET_NODE_TYPE = 'GET_NODE_TYPE';
    const action = actions.getNodeType(nodeId, isSource);

    expect(action.type).toEqual(GET_NODE_TYPE);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.nodeRelationship.getNodeType(nodeId));

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const receiveFailureAction = action.fetchConfig.failure(true);

    const RECEIVE_SOURCE_NODE_TYPE = 'RECEIVE_SOURCE_NODE_TYPE';
    expect(receiveAction.type).toEqual(RECEIVE_SOURCE_NODE_TYPE);
    expect(receiveAction.nodeType).toEqual(true);

    const RECEIVE_NODE_TYPE_NOT_FOUND = 'RECEIVE_NODE_TYPE_NOT_FOUND';
    expect(receiveFailureAction.type).toEqual(RECEIVE_NODE_TYPE_NOT_FOUND);
    expect(receiveFailureAction.value).toEqual(true);
});

it('should request destination node type', () => {
    const nodeId = 55881;
    const isSource = false;
    const GET_NODE_TYPE = 'GET_NODE_TYPE';
    const action = actions.getNodeType(nodeId, isSource);

    expect(action.type).toEqual(GET_NODE_TYPE);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.nodeRelationship.getNodeType(nodeId));

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);
    const receiveFailureAction = action.fetchConfig.failure(true);

    const RECEIVE_DESTINATION_NODE_TYPE = 'RECEIVE_DESTINATION_NODE_TYPE';
    expect(receiveAction.type).toEqual(RECEIVE_DESTINATION_NODE_TYPE);
    expect(receiveAction.nodeType).toEqual(true);

    const RECEIVE_NODE_TYPE_NOT_FOUND = 'RECEIVE_NODE_TYPE_NOT_FOUND';
    expect(receiveFailureAction.type).toEqual(RECEIVE_NODE_TYPE_NOT_FOUND);
    expect(receiveFailureAction.value).toEqual(true);
});

it('should refresh node type', () => {
    const action = actions.resetNodeType();
    const RESET_NODE_TYPE = 'RESET_NODE_TYPE';
    const mock_action = {
        type: RESET_NODE_TYPE
    };
    expect(action).toEqual(mock_action);
});

it('should refresh create success', () => {
    const action = actions.refreshCreateSuccess();
    const REFRESH_CREATE_SUCCESS = 'REFRESH_CREATE_SUCCESS';
    const mock_action = {
        type: REFRESH_CREATE_SUCCESS
    };
    expect(action).toEqual(mock_action);
});
