import * as actions from '../../../../modules/administration/node/attributes/actions';
import { apiService } from '../../../../common/services/apiService';

describe('node actions', () => {
    it('should receive Get Connection Action', () => {
        const RECEIVE_GET_NODE = 'RECEIVE_GET_NODE';
        const node = {};
        const action = actions.receiveGetNode(node);
        const m_action = {
            type: RECEIVE_GET_NODE,
            node
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.node).toBeDefined();
        expect(action.node).toEqual(m_action.node);
    });

    it('should get node Action', () => {
        const GET_NODE = 'GET_NODE';
        const nodeId = 'Test Node Id';
        const action = actions.getNode(nodeId);
        const m_action = {
            type: GET_NODE,
            fetchConfig: {
                path: apiService.node.getById(nodeId),
                success: actions.receiveGetNode
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

    it('should receive Update Node Action', () => {
        const RECEIVE_UPDATE_NODE = 'RECEIVE_UPDATE_NODE';
        const action = actions.receiveUpdateNode();
        const m_action = {
            type: RECEIVE_UPDATE_NODE
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
    });

    it('should Update Node Action', () => {
        const UPDATE_NODE = 'UPDATE_NODE';
        const node = 'Test node';
        const action = actions.updateNode(node);
        const m_action = {
            type: UPDATE_NODE,
            fetchConfig: {
                path: apiService.node.update(),
                method: 'PUT',
                success: actions.receiveUpdateNode,
                body: node
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
        const INIT_NODE_PRODUCT = 'INIT_NODE_PRODUCT';
        const nodeProduct = 'Test Connection Product';
        const action = actions.initNodeProduct(nodeProduct);
        const m_action = {
            type: INIT_NODE_PRODUCT,
            nodeProduct
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.nodeProduct).toBeDefined();
        expect(action.nodeProduct).toEqual(m_action.nodeProduct);
    });

    it('should receive Update Product Action', () => {
        const RECEIVE_UPDATE_NODE_PRODUCT = 'RECEIVE_UPDATE_NODE_PRODUCT';
        const action = actions.receiveUpdateProduct();
        const m_action = {
            type: RECEIVE_UPDATE_NODE_PRODUCT
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
    });

    it('should Update Product Action', () => {
        const UPDATE_NODE_PRODUCT = 'UPDATE_NODE_PRODUCT';
        const product = 'Test Product';
        const action = actions.updateProduct(product);
        const m_action = {
            type: UPDATE_NODE_PRODUCT,
            fetchConfig: {
                path: apiService.node.updateProduct(),
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

    it('should receive Update Owners Action', () => {
        const RECEIVE_UPDATE_NODE_OWNERS = 'RECEIVE_UPDATE_NODE_OWNERS';
        const action = actions.receiveUpdateOwners();
        const m_action = {
            type: RECEIVE_UPDATE_NODE_OWNERS
        };

        expect(action).toBeDefined();
        expect(action.type).toBeDefined();
        expect(action.type).toEqual(m_action.type);
    });

    it('should Update Owners Action', () => {
        const UPDATE_NODE_OWNERS = 'UPDATE_NODE_OWNERS';
        const productId = 'Test Product Id';
        const owners = [{ownerId: 10}];
        const data = {
            owners: owners,
            productId: productId,
            rowVersion: "AAAAAAdw="
        };
        const action = actions.updateOwners(data);
        const m_action = {
            type: UPDATE_NODE_OWNERS,
            fetchConfig: {
                path: apiService.node.updateOwners(),
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

    it('should clear owners', () =>{
        const CLEAR_NODE_ATTRIBUTES_OWNERS = 'CLEAR_NODE_ATTRIBUTES_OWNERS';
        const action = actions.clearOwners();
        expect(action.type).toEqual(CLEAR_NODE_ATTRIBUTES_OWNERS);
    })
});
