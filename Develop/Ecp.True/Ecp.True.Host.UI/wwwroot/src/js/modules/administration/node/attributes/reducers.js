import {
    RECEIVE_GET_NODE,
    RECEIVE_UPDATE_NODE,
    INIT_NODE_PRODUCT,
    RECEIVE_UPDATE_NODE_PRODUCT,
    RECEIVE_UPDATE_NODE_OWNERS,
    CLEAR_NODE_ATTRIBUTES_OWNERS
} from './actions';

export const attributes = (state = { node: {}, nodeProduct: { owners: [] } }, action = {}) => {
    switch (action.type) {
    case RECEIVE_GET_NODE: {
        return Object.assign({}, state, { node: action.node.value[0] });
    }
    case RECEIVE_UPDATE_NODE: {
        return Object.assign({}, state, { nodeToggler: !state.nodeToggler });
    }
    case INIT_NODE_PRODUCT: {
        return Object.assign({}, state, { nodeProduct: action.nodeProduct });
    }
    case RECEIVE_UPDATE_NODE_PRODUCT: {
        return Object.assign({}, state, { productToggler: !state.productToggler });
    }
    case RECEIVE_UPDATE_NODE_OWNERS: {
        return Object.assign({}, state, { ownersUpdateToggler: !state.ownersUpdateToggler });
    }
    case CLEAR_NODE_ATTRIBUTES_OWNERS: {
        return Object.assign({}, state, {
            owners: []
        });
    }
    default:
        return state;
    }
};
