import * as actions from '../../../../modules/administration/nodeConnection/attributes/actions';
import { apiService } from '../../../../common/services/apiService';

describe('node connection actions', () => {
    it('should receive Get Connection Action', () => {
        const RECEIVE_GET_CONNECTION = 'RECEIVE_GET_CONNECTION';
        const result = {
            value: 'Test Value'
        };
        const action = actions.receiveGetConnection(result);
        const m_action = {
            type: RECEIVE_GET_CONNECTION,
            connection: result.value
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.connection).toBeDefined();
        expect(action.connection).toEqual(m_action.connection);
    });

    it('should get Connection Action', () => {
        const GET_CONNECTION = 'GET_CONNECTION';
        const connectionId = 'Test Connection Id';
        const action = actions.getConnection(connectionId);
        const m_action = {
            type: GET_CONNECTION,
            fetchConfig: {
                path: apiService.nodeConnection.queryById(connectionId),
                success: actions.receiveGetConnection
            }
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toBeDefined();
        expect(action.fetchConfig.path).toEqual(m_action.fetchConfig.path);
        expect(action.fetchConfig.success).toBeDefined();
        expect(action.fetchConfig.success).toEqual(m_action.fetchConfig.success);
    });

    it('should receive Update Connection Action', () => {
        const RECEIVE_UPDATE_CONNECTION = 'RECEIVE_UPDATE_CONNECTION';
        const action = actions.receiveUpdateConnection();
        const m_action = {
            type: RECEIVE_UPDATE_CONNECTION
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
    });

    it('should Update Connection Action', () => {
        const UPDATE_CONNECTION = 'UPDATE_CONNECTION';
        const connection = 'Test Connection';
        const action = actions.updateConnection(connection);
        const m_action = {
            type: UPDATE_CONNECTION,
            fetchConfig: {
                path: apiService.nodeConnection.createOrUpdate(),
                method: 'PUT',
                success: actions.receiveUpdateConnection,
                body: connection
            }
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toBeDefined();
        expect(action.fetchConfig.path).toEqual(m_action.fetchConfig.path);
        expect(action.fetchConfig.method).toBeDefined();
        expect(action.fetchConfig.method).toEqual(m_action.fetchConfig.method);
        expect(action.fetchConfig.success).toBeDefined();
        expect(action.fetchConfig.success).toEqual(m_action.fetchConfig.success);
        expect(action.fetchConfig.body).toBeDefined();
        expect(action.fetchConfig.body).toEqual(m_action.fetchConfig.body);
    });

    it('should Init Product Action', () => {
        const INIT_PRODUCT = 'INIT_PRODUCT';
        const connectionProduct = 'Test Connection Product';
        const action = actions.initProduct(connectionProduct);
        const m_action = {
            type: INIT_PRODUCT,
            connectionProduct
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.connectionProduct).toBeDefined();
        expect(action.connectionProduct).toEqual(m_action.connectionProduct);
    });

    it('should receive Update Product Action', () => {
        const RECEIVE_UPDATE_PRODUCT = 'RECEIVE_UPDATE_PRODUCT';
        const action = actions.receiveUpdateProduct();
        const m_action = {
            type: RECEIVE_UPDATE_PRODUCT
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
    });

    it('should Update Product Action', () => {
        const UPDATE_PRODUCT = 'UPDATE_PRODUCT';
        const product = 'Test Product';
        const action = actions.updateProduct(product);
        const m_action = {
            type: UPDATE_PRODUCT,
            fetchConfig: {
                path: apiService.nodeConnection.updateProduct(),
                method: 'PUT',
                success: actions.receiveUpdateProduct,
                body: product
            }
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toBeDefined();
        expect(action.fetchConfig.path).toEqual(m_action.fetchConfig.path);
        expect(action.fetchConfig.method).toBeDefined();
        expect(action.fetchConfig.method).toEqual(m_action.fetchConfig.method);
        expect(action.fetchConfig.success).toBeDefined();
        expect(action.fetchConfig.success).toEqual(m_action.fetchConfig.success);
        expect(action.fetchConfig.body).toBeDefined();
        expect(action.fetchConfig.body).toEqual(m_action.fetchConfig.body);
    });

    it('should receive Get Owners Action', () => {
        const RECEIVE_GET_OWNERS = 'RECEIVE_GET_OWNERS';
        const action = actions.receiveGetOwners({ value: [] });

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(RECEIVE_GET_OWNERS);
        expect(action.owners).toBeDefined();
        expect(action.owners).toEqual([]);
    });

    it('should get Owners Action', () => {
        const GET_OWNERS = 'GET_OWNERS';
        const action = actions.getOwners();
        const m_action = {
            type: GET_OWNERS,
            fetchConfig: {
                path: apiService.nodeConnection.getOwners(),
                success: actions.receiveGetOwners
            }
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toBeDefined();
        expect(action.fetchConfig.path).toEqual(m_action.fetchConfig.path);
        expect(action.fetchConfig.success).toBeDefined();
        expect(action.fetchConfig.success).toEqual(m_action.fetchConfig.success);
    });

    it('should receive Update Owners Action', () => {
        const RECEIVE_UPDATE_NODE_CONNECTION_OWNERS = 'RECEIVE_UPDATE_NODE_CONNECTION_OWNERS';
        const action = actions.receiveUpdateOwners();
        const m_action = {
            type: RECEIVE_UPDATE_NODE_CONNECTION_OWNERS
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
    });

    it('should Update Owners Action', () => {
        const UPDATE_NODE_CONNECTION_OWNERS = 'UPDATE_NODE_CONNECTION_OWNERS';
        const productId = 'Test Product Id';
        const owners = [{ ownerId: 10 }];
        const data = {
            owners: owners,
            productId: productId,
            rowVersion: "AAAAAAdw="
        };
        const action = actions.updateOwners(data);
        const m_action = {
            type: UPDATE_NODE_CONNECTION_OWNERS,
            fetchConfig: {
                path: apiService.nodeConnection.updateOwners(),
                method: 'PUT',
                body: data,
                success: actions.receiveUpdateOwners,
                body: owners
            }
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toBeDefined();
        expect(action.fetchConfig.path).toEqual(m_action.fetchConfig.path);
        expect(action.fetchConfig.method).toBeDefined();
        expect(action.fetchConfig.method).toEqual(m_action.fetchConfig.method);
        expect(action.fetchConfig.success).toBeDefined();
        expect(action.fetchConfig.success).toEqual(m_action.fetchConfig.success);
        expect(action.fetchConfig.body).toBeDefined();
    });

    it('should set control limit source', () => {
        const SET_CONTROL_LIMIT_SOURCE = 'SET_CONTROL_LIMIT_SOURCE';
        const source = 'graphicalNetwork';
        const action = actions.setControlLimitSource(source);
        expect(action.type).toEqual(SET_CONTROL_LIMIT_SOURCE);
        expect(action.controlLimitSource).toEqual(source);
    });

    it('should query connection by source and destination node id', () => {
        const QUERY_CONNECTION_BY_SOURCE_DESTINATION_NODE = 'QUERY_CONNECTION_BY_SOURCE_DESTINATION_NODE';
        const sourceNodeId = 1;
        const destinationNodeId = 2;

        const action = actions.queryConnectionBySourceAndDestinationNodeId(sourceNodeId, destinationNodeId);
        const m_action = {
            type: QUERY_CONNECTION_BY_SOURCE_DESTINATION_NODE,
            fetchConfig: {
                path: apiService.nodeConnection.queryBySourceAndDestinationNodeId(sourceNodeId, destinationNodeId),
                success: actions.receiveConnectionByNodeId
            }
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toBeDefined();
        expect(action.fetchConfig.path).toEqual(m_action.fetchConfig.path);
        expect(action.fetchConfig.success).toBeDefined();
        expect(action.fetchConfig.success).toEqual(m_action.fetchConfig.success);
    });

    it('should receive connection by node id', () => {
        const RECEIVE_CONNECTION_BY_NODES = 'RECEIVE_CONNECTION_BY_NODES';
        const connection = { connectionId: 1 };
        const action = actions.receiveConnectionByNodeId(connection);
        expect(action.type).toEqual(RECEIVE_CONNECTION_BY_NODES);
        expect(action.connection).toEqual(connection);
    });

    it('should receive active nodes', () => {
        const RECEIVE_GET_NODES = 'RECEIVE_GET_NODES';
        const action = actions.receiveActiveNodes();
        const m_action = {
            type: RECEIVE_GET_NODES
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
    });

    it('should get active nodes', () => {
        const REQUEST_GET_NODES = 'REQUEST_GET_NODES';
        const action = actions.getActiveNodes();
        const m_action = {
            type: REQUEST_GET_NODES,
            fetchConfig: {
                path: apiService.node.queryActive(),
                success: actions.receiveActiveNodes
            }
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toBeDefined();
        expect(action.fetchConfig.path).toEqual(m_action.fetchConfig.path);
        expect(action.fetchConfig.success).toBeDefined();
        expect(action.fetchConfig.success).toEqual(m_action.fetchConfig.success);


    });

    it('should get destination nodes by node Id', () => {
        const REQUEST_GET_DESTINATION_NODES = 'REQUEST_GET_DESTINATION_NODES';
        const nodeId = 1;
        const action = actions.getDestinationNodeByNodeId(nodeId);
        const m_action = {
            type: REQUEST_GET_DESTINATION_NODES,
            fetchConfig: {
                path: apiService.nodeConnection.getDestinationNodesBySourceNode(nodeId),
                success: json => receiveDestinationNodesNyNodeId(json.value, nodeId)
            }
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toBeDefined();
        expect(action.fetchConfig.path).toEqual(m_action.fetchConfig.path);

    });

    it('should create node cost center', () => {
        const REQUEST_CREATE_NODE_COST_CENTER = 'REQUEST_CREATE_NODE_COST_CENTER';
        const requestBody = {};
        const requestMethod = 'POST';
        const action = actions.createNodeCostCenter(requestBody);

        expect(action.type).toEqual(REQUEST_CREATE_NODE_COST_CENTER);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.nodeConnection.createNodeCostCenter());
        expect(action.fetchConfig.body).toEqual(requestBody);
        expect(action.fetchConfig.method).toEqual(requestMethod);

        expect(action.fetchConfig.success).toEqual(actions.receiveCreatedNodeCostCenter);
        expect(action.fetchConfig.failure).toEqual(actions.receiveCreatedNodeCostCenterException);

    });

    it('should notify node cost center duplicates', () => {
        const NOTIFY_NODE_COST_CENTER_DUPLICATES = 'NOTIFY_NODE_COST_CENTER_DUPLICATES';
        const action = actions.notifyNodeCostCenterDuplicates();

        const m_action = {
            type: NOTIFY_NODE_COST_CENTER_DUPLICATES,
            isDuplicatedNotified: true
        };

        expect(action.type).toEqual(m_action.type);
        expect(action.isDuplicatedNotified).toEqual(m_action.isDuplicatedNotified);
    });

    it('should init cost center duplicates', () => {
        const INIT_NODE_COST_CENTER_DUPLICATES = 'INIT_NODE_COST_CENTER_DUPLICATES';
        const duplicates = [];
        const action = actions.initCostCenterDuplicates(duplicates);
        const m_action = {
            type: INIT_NODE_COST_CENTER_DUPLICATES,
            duplicates: []
        };
        expect(action.type).toEqual(m_action.type);
        expect(action.duplicates).toEqual(m_action.duplicates);

    });

    it('should blur source node', () => {
        const BLUR_NODE_COST_CENTER = 'BLUR_NODE_COST_CENTER';
        const action = actions.blurSourceNode();
        const m_action = {
            type: BLUR_NODE_COST_CENTER
        }

        expect(action).toBeDefined();
        expect(action.type).toEqual(m_action.type);
    });

    it('should save node cost center', () => {
        const SAVE_NODE_COST_CENTER = 'SAVE_NODE_COST_CENTER';
        const action = actions.saveNodeCostCenter();
        const m_action = {
            type: SAVE_NODE_COST_CENTER,
            fetchConfig: {
                path: apiService.nodeConnection.updateNodeCostCenter(),
                method: 'PUT',
                body: {},
                success: actions.receiveSaveNodeCostCenter
            }
        };

        expect(action).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.path).toEqual(m_action.path);
        expect(action.method).toEqual(m_action.method);
        expect(action.body).toEqual(m_action.body);
        expect(action.success).toEqual(m_action.success);

    });


    it('should receive save node cost center', () => {
        const RECEIVE_SAVE_NODE_COST_CENTER = 'RECEIVE_SAVE_NODE_COST_CENTER';
        const status = true;
        const action = actions.receiveSaveNodeCostCenter(status);
        const m_action = {
            type: RECEIVE_SAVE_NODE_COST_CENTER,
            status: true
        };

        expect(action).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.status).toEqual(m_action.status);
    });
    

    it('should init node cost center', () => {
        const INIT_NODE_COST_CENTER = 'INIT_NODE_COST_CENTER';
        const initialValues = {};
        const action = actions.initNodeCostCenter(initialValues);
        const m_action = {
            type: INIT_NODE_COST_CENTER,
            initialValues: {}
        };

        expect(action).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.initialValues).toEqual(m_action.initialValues);

    });

    it('should delete node cost center', () => {
        const REQUEST_DELETE_NODE_COST_CENTER = 'REQUEST_DELETE_NODE_COST_CENTER';
        const sourceNodeId = 1;
        const action = actions.deleteNodeCostCenter(1);
        const m_action = {
            type: REQUEST_DELETE_NODE_COST_CENTER,
            path: apiService.nodeConnection.deleteNodeConnection(sourceNodeId),
            method: 'DELETE',
            success: jest.fn(),
            failure: jest.fn()
        };

        expect(action).toBeDefined();
        expect(action.type).toEqual(action.type);
        expect(action.path).toEqual(action.path);
        expect(action.method).toEqual(action.method);
        expect(action.success).toEqual(action.success);
        expect(action.failure).toEqual(action.failure);
    });

    it('should delete connection', () => {
        const REQUEST_DELETE_CONNECTION_ATTRIBUTES = 'REQUEST_DREQUEST_DELETE_CONNECTION_ATTRIBUTESLETE_NODE_COST_CENTER';
        const sourceNodeId = 1;
        const destinationNodeId = 1;
        const action = actions.deleteConnectionAttributes(1,1);
        const m_action = {
            type: REQUEST_DELETE_CONNECTION_ATTRIBUTES,
            path: apiService.nodeConnection.deleteNodeConnection(sourceNodeId,destinationNodeId),
            method: 'DELETE',
            success: jest.fn(),
            failure: jest.fn()
        };

        expect(action).toBeDefined();
        expect(action.type).toEqual(action.type);
        expect(action.path).toEqual(action.path);
        expect(action.method).toEqual(action.method);
        expect(action.success).toEqual(action.success);
        expect(action.failure).toEqual(action.failure);
    });  

    it('should receive delete Connection', () => {
        const RECEIVE_DELETE_CONNECTION_ATTRIBUTES = 'RECEIVE_DELETE_CONNECTION_ATTRIBUTES';
        const statusDelete = true;
        const action = actions.receiveDeleteConnectionAttributes(statusDelete);
        const m_action = {
            type: RECEIVE_DELETE_CONNECTION_ATTRIBUTES,
            statusDelete: true
        };

        expect(action).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.statusDelete).toEqual(m_action.statusDelete);
    });

    it('should clear status Connection', () => {
        const CLEAR_STATUS_CONNECTION_ATTRIBUTES = 'CLEAR_STATUS_CONNECTION_ATTRIBUTES';
        const statusDelete = undefined;
        const action = actions.clearStatusConnectionAttributes();
        const m_action = {
            type: CLEAR_STATUS_CONNECTION_ATTRIBUTES,
            statusDelete: undefined
        };

        expect(action).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.statusDelete).toEqual(m_action.statusDelete);
    });

    it('should change status Connection', () => {
        const CHANGE_STATUS_NODE_CONNECTION_ATTRIBUTES = 'CHANGE_STATUS_NODE_CONNECTION_ATTRIBUTES';
        const status = undefined;
        const action = actions.changeStatusNodeConnectionAttributes(status);
        const m_action = {
            type: CHANGE_STATUS_NODE_CONNECTION_ATTRIBUTES,
            status: undefined
        };

        expect(action).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.status).toEqual(m_action.status);
    });
    
    it('should change status node Cost Center', () => {
        const CLEAR_STATUS_NODE_COST_CENTER = 'CLEAR_STATUS_NODE_COST_CENTER';
        const status = undefined;
        const action = actions.clearStatusNodeCostCenter(status);
        const m_action = {
            type: CLEAR_STATUS_NODE_COST_CENTER ,
            status: undefined
        };

        expect(action).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.status).toEqual(m_action.status);
    });

    it('should get nodes by segment', () => {
        const REQUEST_NODE_BY_SEGMENT_ID = 'REQUEST_NODE_BY_SEGMENT_ID';
        const segmentIdSelected = 1;
        const action = actions.getNodesBySegmentId(segmentIdSelected, 0);
        const m_action = {
            type: REQUEST_NODE_BY_SEGMENT_ID,
            fetchConfig: {
                path: apiService.nodeConnection.getNodesBySegmentId(segmentIdSelected),
                success: response => actions.receiveNodesBySegment(response.value, segmentIdSelected)
            }
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toBeDefined();
        expect(action.fetchConfig.path).toEqual(m_action.fetchConfig.path);
        expect(action.fetchConfig.success).toBeDefined();
    });


    it('should receive nodes by segment id', () => {
        const RECEIVE_NODES_BY_SEGMENT = 'RECEIVE_NODES_BY_SEGMENT';
        const data = {
            nodes: [],
            segmentIdSelected: 1,
            position: 3,
            isSource: false
        };

        const action = actions.receiveNodesBySegment(
            data.nodes,
            data.segmentIdSelected,
            data.position,
            data.isSource
        );

        const m_action = {
            type: RECEIVE_NODES_BY_SEGMENT,
            nodes: data.nodes,
            segmentIdSelected: data.segmentIdSelected,
            position: data.position,
            isSource: data.isSource
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.nodes).toEqual(m_action.nodes);
        expect(action.segmentIdSelected).toEqual(m_action.segmentIdSelected);
        expect(action.position).toEqual(m_action.position);
        expect(action.isSource).toEqual(m_action.isSource);
    });

    it('should create node connections', () => {
        const REQUEST_CREATE_NODE_CONNECTION = 'REQUEST_CREATE_NODE_CONNECTION';
        const body = {};
        const action = actions.createNodeConnection(body);
        const m_action = {
            type: REQUEST_CREATE_NODE_CONNECTION,
            fetchConfig: {
                path: apiService.nodeConnection.createUsingList(),
                method: 'POST',
                body,
                success: actions.receiveCreateNodeConnection
            }
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(m_action.fetchConfig.path);
        expect(action.fetchConfig.method).toEqual(m_action.fetchConfig.method);
        expect(action.fetchConfig.body).toEqual(m_action.fetchConfig.body);
        expect(action.fetchConfig.success).toEqual(m_action.fetchConfig.success);

    });


});
