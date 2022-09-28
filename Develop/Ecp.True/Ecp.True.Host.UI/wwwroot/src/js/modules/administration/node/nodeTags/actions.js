import { apiService } from '../../../../common/services/apiService';


export const REQUEST_NODE_CATEGORY = 'GROUP_NODE_CATEGORY';
export const RECEIVE_GROUP_NODE_CATEGORY = 'RECEIVE_GROUP_NODE_CATEGORY';
export const FAILED_NODE_TAG = 'FAILED_NODE_TAG';
export const INIT_FAILED_NODE_TAGS = 'INIT_FAILED_NODE_TAGS';
export const INIT_EXPIRE_NODE_TAGS = 'INIT_EXPIRE_NODE_TAGS';

export const receiveTagSuccess = data => {
    return {
        type: RECEIVE_GROUP_NODE_CATEGORY,
        data
    };
};

export const receiveTagFailure = data => {
    return {
        type: FAILED_NODE_TAG,
        data
    };
};

export const requestGroupNodeCategory = data => {
    return {
        type: REQUEST_NODE_CATEGORY,
        fetchConfig: {
            path: apiService.node.tagNode(),
            body: data,
            success: receiveTagSuccess,
            failure: receiveTagFailure
        }
    };
};

export const initErrorNodes = nodes => {
    return {
        type: INIT_FAILED_NODE_TAGS,
        nodes
    };
};

export const initExpireNode = nodes => {
    return {
        type: INIT_EXPIRE_NODE_TAGS,
        nodes
    };
};

