import * as actions from '../../../modules/administration/transformSettings/actions';
import { apiService } from '../../../common/services/apiService';
import { systemConfigService } from '../../../common/services/systemConfigService';

describe('Actions for transport settings', () => {
    it('should get source nodes', () => {
        const REQUEST_GET_SOURCENODES = 'REQUEST_GET_SOURCENODES';
        const units = 'barrels';
        const mode = 'edit';
        const action = actions.getSourceNodes(units, mode);
        expect(action.type).toEqual(REQUEST_GET_SOURCENODES);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.node.queryActive());
        expect(action.fetchConfig.success).toBeDefined();
        const node = {
            value: { nodeId: 23 }
        };
        const receiveAction = action.fetchConfig.success(node, units, mode);
        expect(receiveAction.type).toEqual(actions.RECEIVE_GET_SOURCENODES);
        expect(receiveAction.mode).toEqual(mode);
        expect(receiveAction.units).toEqual(units);
        expect(receiveAction.sourceNodes).toEqual(node.value);
    });

    it('should get destination nodes', () => {
        const REQUEST_GET_DESTINATIONNODES = 'REQUEST_GET_DESTINATIONNODES';
        const sourceNodeId = 100;
        const name = 'nodeName';
        const action = actions.getDestinationNodes(sourceNodeId, name);
        expect(action.type).toEqual(REQUEST_GET_DESTINATIONNODES);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.nodeConnection.getDestinationNodes(sourceNodeId));
        expect(action.fetchConfig.success).toBeDefined();
        const json = '';
        const receiveAction = action.fetchConfig.success(json, name);
        expect(receiveAction.type).toEqual(actions.RECEIVE_GET_DESTINATIONNODES);
        expect(receiveAction.destinationNodes).toEqual(json);
        expect(receiveAction.name).toEqual(name);
    });

    it('should get source products', () => {
        const REQUEST_GET_SOURCEPRODUCTS = 'REQUEST_GET_SOURCEPRODUCTS';
        const sourceNodeId = 100;
        const name = 'nodeName';
        const action = actions.getSourceProducts(sourceNodeId, name);
        expect(action.type).toEqual(REQUEST_GET_SOURCEPRODUCTS);
        expect(action.name).toEqual(name);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.node.queryNodeProducts(sourceNodeId));
        expect(action.fetchConfig.success).toBeDefined();
        const json = {
            value: {
                id: 100
            }
        };
        const receiveAction = action.fetchConfig.success(json, name);
        expect(receiveAction.type).toEqual(actions.RECEIVE_GET_SOURCEPRODUCTS);
        expect(receiveAction.sourceProducts).toEqual(json.value);
        expect(receiveAction.name).toEqual(name);
    });

    it('should get destination products', () => {
        const REQUEST_GET_DESTINATIONPRODUCTS = 'REQUEST_GET_DESTINATIONPRODUCTS';
        const destinationNodeId = 100;
        const name = 'nodeName';
        const action = actions.getDestinationProducts(destinationNodeId, name);
        expect(action.type).toEqual(REQUEST_GET_DESTINATIONPRODUCTS);
        expect(action.name).toEqual(name);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.node.queryNodeProducts(destinationNodeId));
        expect(action.fetchConfig.success).toBeDefined();
        const json = {
            value: {
                id: 100
            }
        };
        const receiveAction = action.fetchConfig.success(json, name);
        expect(receiveAction.type).toEqual(actions.RECEIVE_GET_DESTINATIONPRODUCTS);
        expect(receiveAction.destinationProducts).toEqual(json.value);
        expect(receiveAction.name).toEqual(name);
    });

    it('should request search source nodes', () => {
        const REQUEST_SEARCH_SOURCENODES = 'REQUEST_SEARCH_SOURCENODES';
        const searchText = 'text';
        const name = 'nameValue';
        systemConfigService.getAutocompleteItemsCount = jest.fn(() => 5);
        const action = actions.requestSearchSourceNodes(searchText, name);
        expect(action.type).toEqual(REQUEST_SEARCH_SOURCENODES);
        expect(action.name).toEqual(name);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.node.searchNodes(searchText));
        expect(action.fetchConfig.success).toBeDefined();
        const json = {
            value: {
                id: 100
            }
        };
        const receiveAction = action.fetchConfig.success(json, name);
        expect(receiveAction.type).toEqual(actions.RECEIVE_SEARCH_SOURCENODES);
        expect(receiveAction.searchedSourceNodes).toEqual(json.value);
        expect(receiveAction.name).toEqual(name);
    });

    it('should clear search source nodes', () => {
        const CLEAR_SEARCH_SOURCENODES = 'CLEAR_SEARCH_SOURCENODES';
        const name = 'name';
        const action = actions.clearSearchSourceNodes(name);
        expect(action.type).toEqual(CLEAR_SEARCH_SOURCENODES);
        expect(action.name).toEqual(name);
    });

    it('should create transformation', () => {
        const REQUEST_CREATE_TRANSFORMATION = 'REQUEST_CREATE_TRANSFORMATION';
        const transformation = {};
        const action = actions.createTransformation(transformation);
        expect(action.type).toEqual(REQUEST_CREATE_TRANSFORMATION);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.transformation.create());
        expect(action.fetchConfig.success).toBeDefined();
        const status = 'PROCESSED';
        const receiveAction = action.fetchConfig.success(status);
        expect(receiveAction.type).toEqual(actions.RECEIVE_CREATE_TRANSFORMATION);
        expect(receiveAction.status).toEqual(status);
        expect(action.fetchConfig.body).toEqual(transformation);
    });

    it('should update transformation', () => {
        const REQUEST_UPDATE_TRANSFORMATION = 'REQUEST_UPDATE_TRANSFORMATION';
        const transformation = {};
        const action = actions.updateTransformation(transformation);
        expect(action.type).toEqual(REQUEST_UPDATE_TRANSFORMATION);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.transformation.create());
        expect(action.fetchConfig.method).toEqual('PUT');
        expect(action.fetchConfig.success).toBeDefined();
        const status = 'PROCESSED';
        const receiveAction = action.fetchConfig.success(status);
        expect(receiveAction.type).toEqual(actions.RECEIVE_UPDATE_TRANSFORMATION);
        expect(receiveAction.status).toEqual(status);
        expect(action.fetchConfig.body).toEqual(transformation);
    });

    it('should get transformation info', () => {
        const REQUEST_GET_TRANSFORMATION_INFO = 'REQUEST_GET_TRANSFORMATION_INFO';
        const transformation = {};
        const action = actions.getTransformationInfo(transformation);
        expect(action.type).toEqual(REQUEST_GET_TRANSFORMATION_INFO);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.transformation.getInfo(transformation.transformationId));
        expect(action.fetchConfig.success).toBeDefined();
        const json = {
            origin: 'origin',
            destination: 'destination'
        };
        const receiveAction = action.fetchConfig.success(json, transformation);
        expect(receiveAction.type).toEqual(actions.RECEIVE_GET_TRANSFORMATION_INFO);
        expect(receiveAction.transformation).toEqual(transformation);
        expect(receiveAction.origin).toEqual(json.origin);
        expect(receiveAction.destination).toEqual(json.destination);
    });

    it('should delete transformation', () => {
        const REQUEST_DELETE_TRANSFORMATION = 'REQUEST_DELETE_TRANSFORMATION';
        const transformation = {};
        const action = actions.deleteTransformation(transformation);
        expect(action.type).toEqual(REQUEST_DELETE_TRANSFORMATION);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.transformation.delete(transformation.transformationId));
        expect(action.fetchConfig.method).toEqual('DELETE');
        expect(action.fetchConfig.success).toBeDefined();
        const status = 'DELETED';
        const receiveAction = action.fetchConfig.success(status);
        expect(receiveAction.type).toEqual(actions.RECEIVE_DELETE_TRANSFORMATION);
        expect(receiveAction.status).toEqual(status);
    });

    it('should reset  transformation', () => {
        const RESET_TRANSFORMATION_POPUP = 'RESET_TRANSFORMATION_POPUP';
        const action = actions.resetTransformationPopup();
        expect(action.type).toEqual(RESET_TRANSFORMATION_POPUP);
    });

    it('should clear transformation data', () => {
        const CLEAR_TRANSFORMATION_DATA = 'CLEAR_TRANSFORMATION_DATA';
        const name = 'name';
        const action = actions.clearTransformationData(name);
        expect(action.type).toEqual(CLEAR_TRANSFORMATION_DATA);
        expect(action.name).toEqual(name);
    });

    it('should initialize edit', () => {
        const REQUEST_EDIT_INITIALIZE_TRANSFORMATION = 'REQUEST_EDIT_INITIALIZE_TRANSFORMATION';
        const transformation = {};
        const action = actions.initializeEdit(transformation);
        expect(action.type).toEqual(REQUEST_EDIT_INITIALIZE_TRANSFORMATION);
        expect(action.transformation).toEqual(transformation);
    });
});
