import * as actions from '../../../../modules/administration/transferPoints/logistics/actions';
import { apiService } from '../../../../common/services/apiService';
import { constants } from '../../../../common/services/constants';

it('should initialize transfer point row', () => {
    const transferPointRow =  {}
    const action = actions.initTransferPointRow(transferPointRow);
    const INIT_TRANSFER_POINT_ROW = 'INIT_TRANSFER_POINT_ROW';
    const mock_action = {
        type: INIT_TRANSFER_POINT_ROW,
        row: transferPointRow
    };
    expect(action).toEqual(mock_action);
});

it('should request create transfer point', () => {
    const transferPointRow = {}
    const method = 'POST'
    const REQUEST_UPDATE_TRANSFER_POINT = 'REQUEST_UPDATE_TRANSFER_POINT';
    const action = actions.requestUpdateTransferPoint(transferPointRow, method);

    expect(action.type).toEqual(REQUEST_UPDATE_TRANSFER_POINT);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.nodeRelationship.update(constants.NodeRelationship.Logistics));
    expect(action.fetchConfig.body).toEqual(transferPointRow);

    expect(action.fetchConfig.method).toEqual(method);
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_CREATE_TRANSFER_POINT = 'RECEIVE_CREATE_TRANSFER_POINT';
    expect(receiveAction.type).toEqual(RECEIVE_CREATE_TRANSFER_POINT);
    expect(receiveAction.success).toEqual(true);
});

it('should request delete transfer point', () => {
    const transferPointRow = {}
    const method = 'DELETE'
    const REQUEST_UPDATE_TRANSFER_POINT = 'REQUEST_UPDATE_TRANSFER_POINT';
    const action = actions.requestUpdateTransferPoint(transferPointRow, method);

    expect(action.type).toEqual(REQUEST_UPDATE_TRANSFER_POINT);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.nodeRelationship.update(constants.NodeRelationship.Logistics));
    expect(action.fetchConfig.body).toEqual(transferPointRow);

    expect(action.fetchConfig.method).toEqual(method);
    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_DELETE_TRANSFER_POINT = 'RECEIVE_DELETE_TRANSFER_POINT';
    expect(receiveAction.type).toEqual(RECEIVE_DELETE_TRANSFER_POINT);
    expect(receiveAction.success).toEqual(true);
});

it('should request transfer source nodes', () => {
    const GET_TRANSFER_SOURCE_NODES = 'GET_TRANSFER_SOURCE_NODES';
    const action = actions.getTransferSourceNodes();

    expect(action.type).toEqual(GET_TRANSFER_SOURCE_NODES);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.nodeRelationship.queryTransferSourceNodes(true));

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

    expect(action.fetchConfig.path).toEqual(apiService.nodeRelationship.queryTransferDestinationNodes(sourceNodeId, true));

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_TRANSFER_DESTINATION_NODES = 'RECEIVE_TRANSFER_DESTINATION_NODES';
    expect(receiveAction.type).toEqual(RECEIVE_TRANSFER_DESTINATION_NODES);
    expect(receiveAction.nodes).toEqual(true);
});

it('should request source storage locations', () => {
    const nodeId = 55881;
    const isSource = true;
    const GET_TRANSFER_LOCATIONS = 'GET_TRANSFER_LOCATIONS';
    const action = actions.getNodeStorageLocations(nodeId, isSource);

    expect(action.type).toEqual(GET_TRANSFER_LOCATIONS);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.node.getNodeStorageLocations(nodeId));

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_SOURCE_TRANSFER_LOCATIONS = 'RECEIVE_SOURCE_TRANSFER_LOCATIONS';
    expect(receiveAction.type).toEqual(RECEIVE_SOURCE_TRANSFER_LOCATIONS);
    expect(receiveAction.storageLocations).toEqual(true);
});

it('should request destination storage locations', () => {
    const nodeId = 55881;
    const isSource = false;
    const GET_TRANSFER_LOCATIONS = 'GET_TRANSFER_LOCATIONS';
    const action = actions.getNodeStorageLocations(nodeId, isSource);

    expect(action.type).toEqual(GET_TRANSFER_LOCATIONS);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.node.getNodeStorageLocations(nodeId));

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_DESTINATION_TRANSFER_LOCATIONS = 'RECEIVE_DESTINATION_TRANSFER_LOCATIONS';
    expect(receiveAction.type).toEqual(RECEIVE_DESTINATION_TRANSFER_LOCATIONS);
    expect(receiveAction.storageLocations).toEqual(true);
});

it('should request source products', () => {
    const nodeId = 55881;
    const isSource = true;
    const GET_TRANSFER_PRODUCTS = 'GET_TRANSFER_PRODUCTS';
    const action = actions.getTransferProducts(nodeId, isSource);

    expect(action.type).toEqual(GET_TRANSFER_PRODUCTS);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.node.queryNodeProducts(nodeId));

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_TRANSFER_SOURCE_PRODUCTS = 'RECEIVE_TRANSFER_SOURCE_PRODUCTS';
    expect(receiveAction.type).toEqual(RECEIVE_TRANSFER_SOURCE_PRODUCTS);
    expect(receiveAction.products).toEqual(true);
});

it('should request destination products', () => {
    const nodeId = 55881;
    const isSource = false;
    const GET_TRANSFER_PRODUCTS = 'GET_TRANSFER_PRODUCTS';
    const action = actions.getTransferProducts(nodeId, isSource);

    expect(action.type).toEqual(GET_TRANSFER_PRODUCTS);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.node.queryNodeProducts(nodeId));

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_TRANSFER_DESTINATION_PRODUCTS = 'RECEIVE_TRANSFER_DESTINATION_PRODUCTS';
    expect(receiveAction.type).toEqual(RECEIVE_TRANSFER_DESTINATION_PRODUCTS);
    expect(receiveAction.products).toEqual(true);
});

it('should request source logistic center', () => {
    const nodeName = 'test';
    const isSource = true;
    const GET_LOGISTIC_CENTER = 'GET_LOGISTIC_CENTER';
    const action = actions.getLogisticCenter(nodeName, isSource);

    expect(action.type).toEqual(GET_LOGISTIC_CENTER);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.node.getLogisticCenter(nodeName));

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_SOURCE_LOGISTIC_CENTER = 'RECEIVE_SOURCE_LOGISTIC_CENTER';
    expect(receiveAction.type).toEqual(RECEIVE_SOURCE_LOGISTIC_CENTER);
    expect(receiveAction.logisticCenter).toEqual(true);
});

it('should request destination logistic center', () => {
    const nodeName = 'test';
    const isSource = false;
    const GET_LOGISTIC_CENTER = 'GET_LOGISTIC_CENTER';
    const action = actions.getLogisticCenter(nodeName, isSource);

    expect(action.type).toEqual(GET_LOGISTIC_CENTER);
    expect(action.fetchConfig).toBeDefined();

    expect(action.fetchConfig.path).toEqual(apiService.node.getLogisticCenter(nodeName));

    expect(action.fetchConfig.success).toBeDefined();
    const receiveAction = action.fetchConfig.success(true);

    const RECEIVE_DESTINATION_LOGISTIC_CENTER = 'RECEIVE_DESTINATION_LOGISTIC_CENTER';
    expect(receiveAction.type).toEqual(RECEIVE_DESTINATION_LOGISTIC_CENTER);
    expect(receiveAction.logisticCenter).toEqual(true);
});

it('should reset on destination node change', () => {
    const action = actions.resetOnDestinationNodeChange();
    const RESET_ON_DESTINATION_NODE_CHANGE = 'RESET_ON_DESTINATION_NODE_CHANGE';
    const mock_action = {
        type: RESET_ON_DESTINATION_NODE_CHANGE
    };
    expect(action).toEqual(mock_action);
});

it('should reset on source node change', () => {
    const action = actions.resetOnSourceNodeChange();
    const RESET_ON_SOURCE_NODE_CHANGE = 'RESET_ON_SOURCE_NODE_CHANGE';
    const mock_action = {
        type: RESET_ON_SOURCE_NODE_CHANGE
    };
    expect(action).toEqual(mock_action);
});