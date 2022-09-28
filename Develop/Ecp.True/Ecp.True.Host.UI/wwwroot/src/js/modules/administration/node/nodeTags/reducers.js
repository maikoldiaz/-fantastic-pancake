import { RECEIVE_GROUP_NODE_CATEGORY, FAILED_NODE_TAG, INIT_FAILED_NODE_TAGS, INIT_EXPIRE_NODE_TAGS } from './actions';
import { utilities } from '../../../../common/services/utilities';

export const nodeTags = function (state = { defaultValues: {} }, action = {}) {
    switch (action.type) {
    case RECEIVE_GROUP_NODE_CATEGORY:
        return Object.assign({},
            state,
            {
                refreshToggler: !utilities.isNullOrUndefined(action.data) ? !state.refreshToggler : state.refreshToggler
            });
    case FAILED_NODE_TAG:
        return Object.assign({},
            state,
            {
                failureToggler: !state.failureToggler,
                errorResponse: action.data
            });
    case INIT_FAILED_NODE_TAGS:
        return Object.assign({},
            state,
            {
                errorNodes: action.nodes
            });
    case INIT_EXPIRE_NODE_TAGS:
        return Object.assign({},
            state,
            {
                expireNode: action.nodes
            });
    default:
        return state;
    }
};
