import * as actions from '../../../../modules/administration/node/nodeTags/actions';
import * as gridActions from '../../../../common/components/grid/actions';
import { nodeTags } from '../../../../modules/administration/node/nodeTags/reducers';
import { grid } from '../../../../common/components/grid/reducers';

describe('Reducers for Node Tags', () => {
    const initialState = {
        defaultValues: {
            operationDate: null,
            element: null
        },
        refreshToggler: false,
        failureToggler: false
    };

    it('should handle action RECEIVE_GROUP_NODE_CATEGORY',
        function () {
            const action = {
                type: actions.RECEIVE_GROUP_NODE_CATEGORY
            };
            const newState = Object.assign({}, initialState, { defaultValues: { operationDate: null, element: null }, refreshToggler: initialState.refreshToggler });
            const returnedValue = nodeTags(initialState, action);
            expect(returnedValue.refreshToggler).toEqual(newState.refreshToggler);
        });

    it('should handle action FAILED_NODE_TAG', () => {
        const action = {
            type: actions.FAILED_NODE_TAG,
            data: {}
        };
        const newState = Object.assign({}, initialState, { failureToggler: !initialState.failureToggler, errorResponse: action.data });
        const returnedValue = nodeTags(initialState, action);
        expect(returnedValue.failureToggler).toEqual(newState.failureToggler);
        expect(returnedValue.errorResponse).toEqual(newState.errorResponse);
    });

    it('should handle action INIT_FAILED_NODE_TAGS', () => {
        const action = {
            type: actions.INIT_FAILED_NODE_TAGS,
            nodes: {}
        };
        const newState = Object.assign({}, initialState, { errorNodes: action.nodes });
        const returnedValue = nodeTags(initialState, action);
        expect(returnedValue.nodes).toEqual(newState.nodes);
    });

    it('should handle action INIT_EXPIRE_NODE_TAGS', () => {
        const action = {
            type: actions.INIT_EXPIRE_NODE_TAGS,
            nodes: {}
        };
        const newState = Object.assign({}, initialState, { expireNode: action.nodes });
        const returnedValue = nodeTags(initialState, action);
        expect(returnedValue.nodes).toEqual(newState.nodes);
    });

    it('should handle action DISABLE_GRID_ITEMS',
        function () {
            const action = {
                type: gridActions.DISABLE_ITEMS_GRID,
                keyField: ['1', '2', '3'],
                keyValues: ['1', '2', '3'],
                name: 'nodeItemsGrid'
            };
            const state = {
                nodeItemsGrid: {
                    items: ['1', '2', '3']
                }
            };

            const returnedValue = grid(state, action);
            expect(returnedValue.nodeItemsGrid.items[0].disabled).toEqual(false);
            expect(returnedValue.nodeItemsGrid.items[0][0]).toEqual('1');
        });

    it('should handle faulty actions', () => {
        const action = {
            type: 'test'
        };
        const returnedValue = nodeTags(initialState, action);
        expect(returnedValue).toEqual(initialState);
    });
});
