import * as actions from '../../../../modules/administration/node/manageNode/actions';
import { apiService } from '../../../../common/services/apiService';
import { constants } from '../../../../common/services/constants';
import { systemConfigService } from '../../../../common/services/systemConfigService';

it('should initialize Node Storage Location Edit', () => {
    const nodeStorageLocation = 123;
    const action = actions.initializeNodeStorageLocationEdit(nodeStorageLocation);
    const INIT_EDIT_NODE_STORAGE_LOCATION = 'INIT_EDIT_NODE_STORAGE_LOCATION';
    const mock_action = {
        type: INIT_EDIT_NODE_STORAGE_LOCATION,
        nodeStorageLocation
    };
    expect(action).toEqual(mock_action);
});

it('should request Filtered Nodes', () => {
    const name = 'test';
    const path = 'path';
    const filter = 'filter';
    const action = actions.requestFilteredNodes(name, path, filter);
    const REQUEST_FILTERED_NODES = 'REQUEST_FILTERED_NODES';
    expect(action.type).toEqual(REQUEST_FILTERED_NODES);
});

it('should save Nodes Grid Filter', () => {
    const filterValues = {};
    const action = actions.saveNodesGridFilter(filterValues);
    const SAVE_NODES_GRID_FILTER = 'SAVE_NODES_GRID_FILTER';
    expect(action.type).toEqual(SAVE_NODES_GRID_FILTER);
});

it('should init Create Node', () => {
    const node = {};
    const action = actions.initCreateNode(node);
    const INIT_CREATE_NODE = 'INIT_CREATE_NODE';
    expect(action.type).toEqual(INIT_CREATE_NODE);
});

it('should init Update Node', () => {
    const node = {};
    const action = actions.initUpdateNode(node);
    const INIT_UPDATE_NODE = 'INIT_UPDATE_NODE';
    expect(action.type).toEqual(INIT_UPDATE_NODE);
});

it('should init Graphical Update Node', () => {
    const node = {};
    const action = actions.initGraphicalUpdateNode(node);
    const INIT_GRAPHICAL_UPDATE_NODE = 'INIT_GRAPHICAL_UPDATE_NODE';
    expect(action.type).toEqual(INIT_GRAPHICAL_UPDATE_NODE);
});

it('should initialize Node Storage Location Products', () => {
    const nodeStorageLocations = {};
    const action = actions.initializeNodeStorageLocationProducts(nodeStorageLocations);
    const INIT_NODE_STORAGE_LOCATION_PRODUCTS = 'INIT_NODE_STORAGE_LOCATION_PRODUCTS';
    expect(action.type).toEqual(INIT_NODE_STORAGE_LOCATION_PRODUCTS);
});

it('should receiveNodeStorageLocations', () => {
    const nodeStorageLocations = {};
    const action = actions.receiveNodeStorageLocations(nodeStorageLocations);
    const RECEIVE_NODE_STORAGE_LOCATIONS = 'RECEIVE_NODE_STORAGE_LOCATIONS';
    expect(action.type).toEqual(RECEIVE_NODE_STORAGE_LOCATIONS);
});

it('should update Node Storage Locations', () => {
    const nodeStorageLocation = {};
    const nodeStorageLocations = {};
    const action = actions.updateNodeStorageLocations(nodeStorageLocation, nodeStorageLocations);
    const UPDATE_NODE_STORAGE_LOCATIONS = 'UPDATE_NODE_STORAGE_LOCATIONS';
    expect(action.type).toEqual(UPDATE_NODE_STORAGE_LOCATIONS);
});

it('should remove Node Storage Locations', () => {
    const nodeStorageLocation = {};
    const action = actions.removeNodeStorageLocations(nodeStorageLocation);
    const REMOVE_NODE_STORAGE_LOCATIONS = 'REMOVE_NODE_STORAGE_LOCATIONS';
    expect(action.type).toEqual(REMOVE_NODE_STORAGE_LOCATIONS);
});

it('should clear Search Products', () => {
    const nodeStorageLocation = {};
    const action = actions.clearSearchProducts(nodeStorageLocation);
    const CLEAR_SEARCH_PRODUCTS = 'CLEAR_SEARCH_PRODUCTS';
    expect(action.type).toEqual(CLEAR_SEARCH_PRODUCTS);
});

it('should request Search Products', () => {
    const searchText = '';
    const storageLocationId = 123;
    systemConfigService.getAutocompleteItemsCount = jest.fn(() => 5);
    const action = actions.requestSearchProducts(searchText, storageLocationId);
    const REQUEST_SEARCH_PRODUCTS = 'REQUEST_SEARCH_PRODUCTS';
    expect(action.type).toEqual(REQUEST_SEARCH_PRODUCTS);
});

it('should request Node Storage Locations', () => {
    const nodeId = 123;
    const action = actions.requestNodeStorageLocations(nodeId);
    const REQUEST_NODE_STORAGE_LOCATIONS = 'REQUEST_NODE_STORAGE_LOCATIONS';
    expect(action.type).toEqual(REQUEST_NODE_STORAGE_LOCATIONS);
});

it('should request Create Update Node: isCreate and isGraphical to be False', () => {
    const node = {};
    const isCreate = false;
    const isGraphical = false;
    const action = actions.requestCreateUpdateNode(node, isCreate, isGraphical);
    const REQUEST_CREATE_UPDATE_NODE = 'REQUEST_CREATE_UPDATE_NODE';
    expect(action.type).toEqual(REQUEST_CREATE_UPDATE_NODE);
});

it('should request Create Update Node: isCreate and isGraphical to be true', () => {
    const node = {};
    const isCreate = true;
    const isGraphical = true;
    const action = actions.requestCreateUpdateNode(node, isCreate, isGraphical);
    const REQUEST_CREATE_UPDATE_NODE = 'REQUEST_CREATE_UPDATE_NODE';
    expect(action.type).toEqual(REQUEST_CREATE_UPDATE_NODE);
});

it('should request Update Node', () => {
    const nodeId = 123;
    const action = actions.requestUpdateNode(nodeId);
    const REQUEST_UPDATE_NODE = 'REQUEST_UPDATE_NODE';
    expect(action.type).toEqual(REQUEST_UPDATE_NODE);
});

it('should set Failure State', () => {
    const status = true;
    const action = actions.setFailureState(status);
    const SET_FAILURE_STATE = 'SET_FAILURE_STATE';
    expect(action.type).toEqual(SET_FAILURE_STATE);
});

it('should submit With Auto ReOrder', () => {
    const status = true;
    const action = actions.submitWithAutoReOrder(status);
    const SUBMIT_WITH_AUTO_REORDER = 'SUBMIT_WITH_AUTO_REORDER';
    expect(action.type).toEqual(SUBMIT_WITH_AUTO_REORDER);
});

it('should reset Reorder', () => {
    const action = actions.resetReorder();
    const RESET_AUTO_REORDER = 'RESET_AUTO_REORDER';
    expect(action.type).toEqual(RESET_AUTO_REORDER);
});

it('should change Nodes Filter Persistance', () => {
    const persist = true;
    const action = actions.changeNodesFilterPersistance(persist);
    const CHANGE_NODES_FILTER_PERSISTANCE = 'CHANGE_NODES_FILTER_PERSISTANCE';
    expect(action.type).toEqual(CHANGE_NODES_FILTER_PERSISTANCE);
});

it('should reset Nodes Filters', () => {
    const action = actions.resetNodesFilters();
    const RESET_NODES_FILTER = 'RESET_NODES_FILTER';
    expect(action.type).toEqual(RESET_NODES_FILTER);
});

it('should update Same Order Node', () => {
    const existingNode = {};
    const action = actions.updateSameOrderNode(existingNode);
    const RECEIVE_SAME_ORDER_NODE = 'RECEIVE_SAME_ORDER_NODE';
    expect(action.type).toEqual(RECEIVE_SAME_ORDER_NODE);
});

it('should isValidNode', () => {
    const node = {};
    const action = actions.isValidNode(node);
    const VALIDATE_NODE = 'VALIDATE_NODE';
    expect(action.type).toEqual(VALIDATE_NODE);
});
