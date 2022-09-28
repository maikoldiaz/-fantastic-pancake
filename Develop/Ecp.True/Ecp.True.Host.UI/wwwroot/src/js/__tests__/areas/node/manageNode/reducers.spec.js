import * as actions from '../../../../modules/administration/node/manageNode/actions.js';
import { manageNode } from '../../../../modules/administration/node/manageNode/reducers.js';
import { constants } from '../../../../common/services/constants.js';
import { resourceProvider } from '../../../../common/services/resourceProvider.js';
import { utilities } from '../../../../common/services/utilities.js';

describe('Reducer for graphical configuration network', () => {
    const nodesGridFilterInitialState = {
        defaultFilterValues: {
            segment: null, nodeTypes: [], operators: []
        },
        node: { isActive: true },
        nodeStorageLocations: [],
        filterValues: undefined,
        mode: constants.Modes.Update,
        isActive: false,
        searchedProducts: [],
        isAsyncValid: true,
        persist: false,
        autoOrderNode: false
    };

    it('should INIT_UPDATE_NODE', () => {
        const node =
        {
            name: 'NodeName',
            logisticCenter: {},
            nodeStorageLocations: [],
            capacity: 10,
            unit: 'Bbl',
            existingName: 'NodeName',
            existingLogisticCenter: {}
        };

        const action = {
            type: actions.INIT_UPDATE_NODE,
            node
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node:
            {
                name: node.name,
                logisticCenter: {},
                nodeStorageLocations: [],
                capacity: 10,
                unit: node.unit,
                existingName: node.existingName,
                existingLogisticCenter: {},
                existingLogisticCenterId: undefined
            },
            nodeStorageLocations: [],
            isActive: false,
            searchedProducts: [],
            mode: 'update',
            isAsyncValid: true,
            persist: false,
            autoOrderNode: false,
            filterValues: undefined,
            unit: node.unit,
            capacity: 10,
            updateToggler: true,
            validNode: true,
            graphicalNodeUpdate: false
        });
        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });

    it('should INIT_GRAPHICAL_UPDATE_NODE', () => {
        const node =
        {
            name: 'NodeName',
            logisticCenter: {},
            nodeStorageLocations: [],
            capacity: 10,
            unit: 'Bbl',
            existingName: 'NodeName',
            existingLogisticCenter: {}
        };

        const action = {
            type: actions.INIT_GRAPHICAL_UPDATE_NODE,
            node
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node:
            {
                name: 'NodeName',
                logisticCenter: {},
                nodeStorageLocations: [],
                capacity: 10,
                unit: 'Bbl',
                existingName: undefined,
                existingLogisticCenter: {},
                existingLogisticCenterId: undefined
            },
            nodeStorageLocations: [],
            filterValues: undefined,
            mode: 'update',
            isActive: false,
            searchedProducts: [],
            isAsyncValid: true,
            persist: false,
            autoOrderNode: false,
            unit: 'Bbl',
            capacity: 10,
            graphicalNodeUpdate: true
        });

        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });

    it('should INIT_CREATE_NODE', () => {
        const action = {
            type: actions.INIT_CREATE_NODE
        };
        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node: { isActive: true },
            nodeStorageLocations: [],
            isActive: false,
            searchedProducts: [],
            mode: 'create',
            isAsyncValid: true,
            persist: false,
            autoOrderNode: false,
            filterValues: undefined,
            validNode: true,
            graphicalNodeUpdate: false
        });
        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });

    it('should INIT_EDIT_NODE_STORAGE_LOCATION', () => {
        const action = {
            type: actions.INIT_EDIT_NODE_STORAGE_LOCATION,
            nodeStorageLocation: []
        };
        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node: { isActive: true },
            nodeStorageLocations: [],
            filterValues: undefined,
            mode: 'update',
            isActive: false,
            searchedProducts: [],
            isAsyncValid: true,
            persist: false,
            autoOrderNode: false,
            nodeStorageLocation: []
        });
        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });

    it('should SAVE_NODES_GRID_FILTER', () => {
        const action = {
            type: actions.SAVE_NODES_GRID_FILTER,
            filterValues: {}
        };
        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node: { isActive: true },
            nodeStorageLocations: [],
            filterValues: {},
            mode: 'update',
            isActive: false,
            searchedProducts: [],
            isAsyncValid: true,
            persist: false,
            autoOrderNode: false
        });

        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });


    it('should INIT_NODE_STORAGE_LOCATION_PRODUCTS', () => {
        const action = {
            type: actions.INIT_NODE_STORAGE_LOCATION_PRODUCTS,
            productNodeStorageLocation: {}
        };
        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node: { isActive: true },
            nodeStorageLocations: [],
            filterValues: undefined,
            mode: 'update',
            isActive: false,
            searchedProducts: [],
            isAsyncValid: true,
            persist: false,
            autoOrderNode: false,
            productNodeStorageLocation: {}
        });

        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });

    it('should RECEIVE_SEARCH_PRODUCTS', () => {
        const action = {
            type: actions.RECEIVE_SEARCH_PRODUCTS,
            productNodeStorageLocation: {}
        };
        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node: { isActive: true },
            nodeStorageLocations: [],
            filterValues: undefined,
            mode: 'update',
            isActive: false,
            searchedProducts: undefined,
            isAsyncValid: true,
            persist: false,
            autoOrderNode: false
        });

        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });

    it('should CLEAR_SEARCH_PRODUCTS', () => {
        const action = {
            type: actions.CLEAR_SEARCH_PRODUCTS
        };
        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node: { isActive: true },
            nodeStorageLocations: [],
            filterValues: undefined,
            mode: 'update',
            isActive: false,
            searchedProducts: [],
            isAsyncValid: true,
            persist: false,
            autoOrderNode: false
        });
        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });

    it('should UPDATE_NODE_STORAGE_LOCATIONS', () => {
        const action = {
            type: actions.UPDATE_NODE_STORAGE_LOCATIONS,
            nodeStorageLocations: []
        };
        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node: { isActive: true },
            nodeStorageLocations: [],
            filterValues: undefined,
            mode: 'update',
            isActive: false,
            searchedProducts: [],
            isAsyncValid: true,
            persist: false,
            autoOrderNode: false,
            isValid: false
        });

        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });

    it('should RECEIVE_NODE_STORAGE_LOCATIONS', () => {
        const action = {
            type: actions.RECEIVE_NODE_STORAGE_LOCATIONS,
            nodeStorageLocations: []
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node: { isActive: true },
            nodeStorageLocations: [],
            filterValues: undefined,
            mode: 'update',
            isActive: false,
            searchedProducts: [],
            isAsyncValid: true,
            persist: false,
            autoOrderNode: false,
            isValid: false
        });

        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });

    it('should REMOVE_NODE_STORAGE_LOCATIONS', () => {
        const action = {
            type: actions.REMOVE_NODE_STORAGE_LOCATIONS,
            nodeStorageLocation: {}
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node: { isActive: true },
            nodeStorageLocations: [],
            filterValues: undefined,
            mode: 'update',
            isActive: false,
            searchedProducts: [],
            isAsyncValid: true,
            persist: false,
            autoOrderNode: false,
            isValid: false
        });

        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });

    it('should RECEIVE_CREATE_UPDATE_NODE', () => {
        const action = {
            type: actions.RECEIVE_CREATE_UPDATE_NODE,
            nodeStorageLocation: {}
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node: { isActive: true },
            nodeStorageLocations: [],
            filterValues: undefined,
            mode: 'update',
            isActive: false,
            searchedProducts: [],
            isAsyncValid: true,
            persist: true,
            autoOrderNode: false,
            refreshToggler: true,
            existingNode: null
        });
        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });

    it('should RECEIVE_CREATE_UPDATE_GRAPHICAL_NODE', () => {
        const action = {
            type: actions.RECEIVE_CREATE_UPDATE_GRAPHICAL_NODE,
            status: {
                message: 'message'
            }
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node: { isActive: true },
            nodeStorageLocations: [],
            filterValues: undefined,
            mode: 'update',
            isActive: false,
            searchedProducts: [],
            isAsyncValid: true,
            persist: true,
            autoOrderNode: false,
            nodeSavedToggler: true,
            savedNodeId: NaN
        });

        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });


    it('should SUBMIT_WITH_AUTO_REORDER', () => {
        const action = {
            type: actions.SUBMIT_WITH_AUTO_REORDER
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            autoOrder: true
        });

        expect(manageNode(nodesGridFilterInitialState, action).autoOrder).toEqual(newState.autoOrder);
    });

    it('should RESET_AUTO_REORDER', () => {
        const action = {
            type: actions.RESET_AUTO_REORDER
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            autoOrderNode: false
        });

        expect(manageNode(nodesGridFilterInitialState, action).autoOrderNode).toEqual(newState.autoOrderNode);
    });

    it('should CHANGE_NODES_FILTER_PERSISTANCE', () => {
        const action = {
            type: actions.CHANGE_NODES_FILTER_PERSISTANCE,
            persist: true
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            persist: true
        });

        expect(manageNode(nodesGridFilterInitialState, action).persist).toEqual(newState.persist);
    });

    it('should RESET_NODES_FILTER', () => {
        const action = {
            type: actions.RESET_NODES_FILTER
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] }
        });

        expect(manageNode(nodesGridFilterInitialState, action).defaultFilterValues).toEqual(newState.defaultFilterValues);
    });

    it('should RECEIVE_SAME_ORDER_NODE', () => {
        const action = {
            type: actions.RECEIVE_SAME_ORDER_NODE,
            existingNode: {}
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            existingNode: {}
        });

        expect(manageNode(nodesGridFilterInitialState, action).existingNode).toEqual(newState.existingNode);
    });

    it('should RECEIVE_FAILURE_CREATE_UPDATE_NODE', () => {
        const action = {
            type: actions.RECEIVE_FAILURE_CREATE_UPDATE_NODE
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            saveNodeFailed: true
        });

        expect(manageNode(nodesGridFilterInitialState, action).saveNodeFailed).toEqual(newState.saveNodeFailed);
    });

    it('should SET_FAILURE_STATE', () => {
        const action = {
            type: actions.SET_FAILURE_STATE
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            status: false
        });
        expect(manageNode(nodesGridFilterInitialState, action).saveNodeFailed).toEqual(newState.saveNodeFailed);
    });

    it('should VALIDATE_NODE', () => {
        const action = {
            type: actions.VALIDATE_NODE,
            node: {
                operador: 'operador',
                segment: 'Seg',
                order: 1,
                nodeType: '1'
            }
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node: { isActive: true },
            nodeStorageLocations: [],
            filterValues: undefined,
            mode: 'update',
            isActive: false,
            searchedProducts: [],
            isAsyncValid: true,
            persist: false,
            autoOrderNode: false,
            validNode: false,
            validNodeToggler: true
        });

        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });

    it('should @@redux-form/CHANGE', () => {
        const action = {
            type: '@@redux-form/CHANGE',
            meta: {
                form: 'createNode',
                field: 'logisticCenter'
            },
            payload: {}
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node: { isActive: true, logisticCenter: {} },
            nodeStorageLocations: [],
            filterValues: undefined,
            mode: 'update',
            isActive: false,
            searchedProducts: [],
            isAsyncValid: true,
            persist: false,
            autoOrderNode: false,
            isValid: false
        });
        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });

    it('@@redux-form/STOP_ASYNC_VALIDATION', () => {
        const action = {
            type: '@@redux-form/STOP_ASYNC_VALIDATION',
            meta: {
                form: 'createNode'
            }
        };

        const newState = Object.assign({}, nodesGridFilterInitialState, {
            defaultFilterValues: { segment: null, nodeTypes: [], operators: [] },
            node: { isActive: true },
            nodeStorageLocations: [],
            filterValues: undefined,
            mode: 'update',
            isActive: false,
            searchedProducts: [],
            isAsyncValid: true,
            persist: false,
            autoOrderNode: false
        });
        expect(manageNode(nodesGridFilterInitialState, action)).toEqual(newState);
    });
});
