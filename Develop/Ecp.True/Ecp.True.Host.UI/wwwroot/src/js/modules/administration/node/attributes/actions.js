import { apiService } from '../../../../common/services/apiService';

export const GET_NODE = 'GET_NODE';
export const RECEIVE_GET_NODE = 'RECEIVE_GET_NODE';

export const UPDATE_NODE = 'UPDATE_NODE';
export const RECEIVE_UPDATE_NODE = 'RECEIVE_UPDATE_NODE';

export const INIT_NODE_PRODUCT = 'INIT_NODE_PRODUCT';
export const UPDATE_NODE_PRODUCT = 'UPDATE_NODE_PRODUCT';
export const RECEIVE_UPDATE_NODE_PRODUCT = 'RECEIVE_UPDATE_NODE_PRODUCT';

export const UPDATE_NODE_OWNERS = 'UPDATE_NODE_OWNERS';
export const RECEIVE_UPDATE_NODE_OWNERS = 'RECEIVE_UPDATE_NODE_OWNERS';
export const CLEAR_NODE_ATTRIBUTES_OWNERS = 'CLEAR_NODE_ATTRIBUTES_OWNERS';

export const receiveGetNode = node => {
    return {
        type: RECEIVE_GET_NODE,
        node
    };
};

export const getNode = nodeId => {
    return {
        type: GET_NODE,
        fetchConfig: {
            path: apiService.node.getById(nodeId),
            success: receiveGetNode,
            notFound: true
        }
    };
};

export const receiveUpdateNode = () => {
    return {
        type: RECEIVE_UPDATE_NODE
    };
};

export const updateNode = node => {
    return {
        type: UPDATE_NODE,
        fetchConfig: {
            path: apiService.node.update(),
            method: 'PUT',
            success: receiveUpdateNode,
            body: node
        }
    };
};

export const initNodeProduct = nodeProduct => {
    return {
        type: INIT_NODE_PRODUCT,
        nodeProduct
    };
};

export const receiveUpdateProduct = () => {
    return {
        type: RECEIVE_UPDATE_NODE_PRODUCT
    };
};

export const updateProduct = product => {
    return {
        type: UPDATE_NODE_PRODUCT,
        fetchConfig: {
            path: apiService.node.updateProduct(),
            method: 'PUT',
            success: receiveUpdateProduct,
            body: product
        }
    };
};

export const receiveUpdateOwners = () => {
    return {
        type: RECEIVE_UPDATE_NODE_OWNERS
    };
};

export const updateOwners = data => {
    return {
        type: UPDATE_NODE_OWNERS,
        fetchConfig: {
            path: apiService.node.updateOwners(),
            method: 'PUT',
            success: receiveUpdateOwners,
            body: data
        }
    };
};

export const clearOwners = () => {
    return {
        type: CLEAR_NODE_ATTRIBUTES_OWNERS
    };
};
