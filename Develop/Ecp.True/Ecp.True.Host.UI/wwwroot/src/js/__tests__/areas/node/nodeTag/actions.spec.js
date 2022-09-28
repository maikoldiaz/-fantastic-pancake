import * as actions from '../../../../modules/administration/node/nodeTags/actions';
import * as gridActions from '../../../../common/components/grid/actions';
import { apiService } from '../../../../common/services/apiService';

describe('Actions for NodeTags', () => {
    it('should tag a node', () => {
        const typeOfAction = 1;
        const taggedNodesData = [];
        taggedNodesData.push({
            NodeTagId: 1,
            nodeId: 1
        });

        const taggedNodeInfo = {
            operationalType: typeOfAction,
            elementId: 1,
            InputDate: new Date(),
            taggedNodes: taggedNodesData
        };
        const REQUEST_NODE_CATEGORY = 'GROUP_NODE_CATEGORY';
        const action = actions.requestGroupNodeCategory(taggedNodeInfo);

        expect(action.type).toEqual(REQUEST_NODE_CATEGORY);
        expect(action.fetchConfig).toBeDefined();

        expect(action.fetchConfig.path).toEqual(apiService.node.tagNode());
        expect(action.fetchConfig.body).toEqual(taggedNodeInfo);

        expect(action.fetchConfig.success).toBeDefined();
        const receiveAction = action.fetchConfig.success(true);
        const RECEIVE_GROUP_NODE_CATEGORY = 'RECEIVE_GROUP_NODE_CATEGORY';
        expect(receiveAction.type).toEqual(RECEIVE_GROUP_NODE_CATEGORY);

        expect(action.fetchConfig.failure).toBeDefined();
        const receiveFailureAction = action.fetchConfig.failure(true);
        const FAILED_NODE_TAG = 'FAILED_NODE_TAG';
        expect(receiveFailureAction.type).toEqual(FAILED_NODE_TAG);
        expect(receiveFailureAction.data).toBeDefined();
    });

    it('should disable Items on grid', () => {
        const DISABLE_ITEMS_GRID = 'DISABLE_ITEMS_GRID';

        const disableGridItems = {
            type: DISABLE_ITEMS_GRID,
            name: 'nodeTagGird',
            keyField: 'field',
            keyValues: 'values'
        };
        const action = gridActions.disableGridItems(disableGridItems);

        expect(action.type).toEqual(DISABLE_ITEMS_GRID);
    });

    it('should initialize failed nodes', () => {
        const INIT_FAILED_NODE_TAGS = 'INIT_FAILED_NODE_TAGS';

        const initFailedNodes = {
            type: INIT_FAILED_NODE_TAGS,
            nodes: {}
        };
        const action = actions.initErrorNodes(initFailedNodes);

        expect(action.type).toEqual(INIT_FAILED_NODE_TAGS);
    });

    it('should initialize expired nodes', () => {
        const INIT_EXPIRE_NODE_TAGS = 'INIT_EXPIRE_NODE_TAGS';

        const initExpiredNodes = {
            type: INIT_EXPIRE_NODE_TAGS,
            nodes: {}
        };
        const action = actions.initExpireNode(initExpiredNodes);

        expect(action.type).toEqual(INIT_EXPIRE_NODE_TAGS);
    });
});
