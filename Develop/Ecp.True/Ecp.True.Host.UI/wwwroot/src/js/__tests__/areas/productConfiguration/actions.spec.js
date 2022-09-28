import * as actions from '../../../modules/administration/productConfiguration/actions';
import { apiService } from '../../../common/services/apiService';

describe('Actions for registryDeviationConfiguration', () => {
    it('should initialize types', () => {
        const action = actions.initProduct();
        const INIT_PRODUCT = 'INIT_PRODUCT';
        const mock_action = {
            type: INIT_PRODUCT
        };
        expect(action).toEqual(mock_action);
    });
    
    it('should request save product', () => {
        const value = {};
        const SAVE_PRODUCT = 'SAVE_PRODUCT';
        const action = actions.saveProduct(value);
    
        expect(action.type).toEqual(SAVE_PRODUCT);
        expect(action.fetchConfig).toBeDefined();
    
        expect(action.fetchConfig.path).toEqual(apiService.product.createProduct());
        expect(action.fetchConfig.body).toEqual(value);
        expect(action.fetchConfig.success).toBeDefined();
        const receiveAction = action.fetchConfig.success(true);
    
        const RECEIVE_SAVE_PRODUCT = 'RECEIVE_SAVE_PRODUCT';
        expect(receiveAction.type).toEqual(RECEIVE_SAVE_PRODUCT);
        expect(receiveAction.status).toEqual(true);
    });
    
    it('should receive save product', () => {
        
        const response = {
            hasError:true,
            isActive: true
        }
        const action = actions.receiveSaveProduct(response);
        const RECEIVE_SAVE_PRODUCT = 'RECEIVE_SAVE_PRODUCT';
        const mock_action = {
            type: RECEIVE_SAVE_PRODUCT,
            status: !response.hasError,
            isActive: response.isActive
        };
        expect(action).toEqual(mock_action);
    });
    
    it('should request update product', () => {
        const productSapId = {};
        const value = {};
        const UPDATE_PRODUCT = 'UPDATE_PRODUCT';
        const action = actions.updateProduct(value, productSapId);
    
        expect(action.type).toEqual(UPDATE_PRODUCT);
        expect(action.fetchConfig).toBeDefined();
    
        expect(action.fetchConfig.path).toEqual(apiService.product.updateProduct(productSapId));
        expect(action.fetchConfig.body).toEqual(value);
        expect(action.fetchConfig.success).toBeDefined();
        const receiveAction = action.fetchConfig.success(true);
    
        const RECEIVE_UPDATE_PRODUCT = 'RECEIVE_UPDATE_PRODUCT';
        expect(receiveAction.type).toEqual(RECEIVE_UPDATE_PRODUCT);
        expect(receiveAction.status).toEqual(true);
    });
    
    it('should receive update product', () => {
        
        const action = actions.receiveUpdateProduct();
        const RECEIVE_UPDATE_PRODUCT = 'RECEIVE_UPDATE_PRODUCT';
        const mock_action = {
            type: RECEIVE_UPDATE_PRODUCT,
            status: true
        };
        expect(action).toEqual(mock_action);
    });
    
    it('should request update product', () => {
        const productSapId = {};
        const value = {};
        const UPDATE_PRODUCT = 'UPDATE_PRODUCT';
        const action = actions.updateProduct(value, productSapId);
    
        expect(action.type).toEqual(UPDATE_PRODUCT);
        expect(action.fetchConfig).toBeDefined();
    
        expect(action.fetchConfig.path).toEqual(apiService.product.updateProduct(productSapId));
        expect(action.fetchConfig.body).toEqual(value);
        expect(action.fetchConfig.failure).toBeDefined();
        const receiveAction = action.fetchConfig.failure(true);
    
        const RECEIVE_UPDATE_PRODUCT = 'RECEIVE_UPDATE_PRODUCT';
        expect(receiveAction.type).toEqual(RECEIVE_UPDATE_PRODUCT);
        expect(receiveAction.status).toEqual(false);
    });
    
    it('should fail receive save product', () => {
        
        const action = actions.failReceiveUpdateProduct();
        const RECEIVE_UPDATE_PRODUCT = 'RECEIVE_UPDATE_PRODUCT';
        const mock_action = {
            type: RECEIVE_UPDATE_PRODUCT,
            status: false
        };
        expect(action).toEqual(mock_action);
    });
    
    it('should clear status product', () => {
        const CLEAR_SUCCESS = 'CLEAR_SUCCESS';
        const action = actions.clearSuccess();
        const m_action = {
            type: CLEAR_SUCCESS,
            saveSuccess: undefined,
            updateSuccess: undefined,
            deleteSuccess: undefined
        };
    
        expect(action).toBeDefined();
        expect(action.type).toEqual(m_action.type);
        expect(action.saveSuccess).toEqual(m_action.saveSuccess);
        expect(action.updateSuccess).toEqual(m_action.updateSuccess);
        expect(action.deleteSuccess).toEqual(m_action.deleteSuccess);
    });
    
    it('should request delete product', () => {
        const productSapId = {};
        const DELETE_PRODUCT = 'DELETE_PRODUCT';
        const action = actions.deleteProduct(productSapId);
    
        expect(action.type).toEqual(DELETE_PRODUCT);
        expect(action.fetchConfig).toBeDefined();
    
        expect(action.fetchConfig.path).toEqual(apiService.product.deleteProduct(productSapId));
        expect(action.fetchConfig.success).toBeDefined();
        const receiveAction = action.fetchConfig.success(true);
    
        const RECEIVE_DELETE_PRODUCT = 'RECEIVE_DELETE_PRODUCT';
        expect(receiveAction.type).toEqual(RECEIVE_DELETE_PRODUCT);
        expect(receiveAction.status).toEqual(true);
    });
    
    it('should receive delete product', () => {
        
        const action = actions.receiveDeleteProduct();
        const RECEIVE_DELETE_PRODUCT = 'RECEIVE_DELETE_PRODUCT';
        const mock_action = {
            type: RECEIVE_DELETE_PRODUCT,
            status: true
        };
        expect(action).toEqual(mock_action);
    });
    
    it('should request delete product and fail', () => {
        const productSapId = {};
        const DELETE_PRODUCT = 'DELETE_PRODUCT';
        const action = actions.deleteProduct(productSapId);
    
        expect(action.type).toEqual(DELETE_PRODUCT);
        expect(action.fetchConfig).toBeDefined();
    
        expect(action.fetchConfig.path).toEqual(apiService.product.deleteProduct(productSapId));
        expect(action.fetchConfig.failure).toBeDefined();
        const receiveAction = action.fetchConfig.failure(true);
    
        const RECEIVE_DELETE_PRODUCT = 'RECEIVE_DELETE_PRODUCT';
        expect(receiveAction.type).toEqual(RECEIVE_DELETE_PRODUCT);
        expect(receiveAction.status).toEqual(false);
    });
    
    it('should fail receive delete product', () => {
        
        const action = actions.failReceiveDeleteProduct();
        const RECEIVE_DELETE_PRODUCT = 'RECEIVE_DELETE_PRODUCT';
        const mock_action = {
            type: RECEIVE_DELETE_PRODUCT,
            status: false
        };
        expect(action).toEqual(mock_action);
    });

    it('should get products', () => {
        const json = [{}];
        const action = actions.getProducts();

        expect(action.type).toEqual(actions.REQUEST_PRODUCTS);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(json);
        expect(receiveAction.type).toEqual(actions.RECEIVE_PRODUCTS);
        expect(receiveAction.data).toEqual(json);
    });

    it('should get Storage Locations By Logistic Center', () => {
        const json = [{
            logisticCenterId: 1,
            position: 1,
            storages: {}
        }];
        const action = actions.getStorageLocationsByLogisticCenter(1, 1);

        expect(action.type).toEqual(actions.REQUEST_STORAGE_BY_LOGISTIC_CENTER);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(json);
        expect(receiveAction.type).toEqual(actions.RECEIVE_STORAGE_BY_LOGISTIC_CENTER);
        expect(receiveAction.storages).toEqual(json.storages);
        expect(receiveAction.logisticCenterId).toEqual(1);
        expect(receiveAction.position).toEqual(1);
    });

    it('should delete Association Center Storage Product when is true', () => {
        const json = [{
            deleted: true
        }];
        const action = actions.deleteAssociationCenterStorageProduct(1);

        expect(action.type).toEqual(actions.REQUEST_DELETE_ASSOCIATION);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(json);
        expect(receiveAction.type).toEqual(actions.RECEIVE_DELETE_ASSOCIATION);
        expect(receiveAction.deleted).toEqual(true);
    });

    it('should delete Association Center Storage Product when is false', () => {
        const json = [{
            deleted: true
        }];
        const action = actions.deleteAssociationCenterStorageProduct(1);

        expect(action.type).toEqual(actions.REQUEST_DELETE_ASSOCIATION);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.failure).toBeDefined();

        const receiveAction = action.fetchConfig.failure(json);
        expect(receiveAction.type).toEqual(actions.RECEIVE_DELETE_ASSOCIATION);
        expect(receiveAction.deleted).toEqual(false);
    });

    it('should clear Storage List By Position', () => {
        const action = actions.clearStorageListByPosition(1);

        expect(action.type).toEqual(actions.CLEAR_STORAGE_LIST_BY_POSITION);
        expect(action.position).toEqual(1);
    });

    it('should clear All Storage List', () => {
        const action = actions.clearAllStorageList();
        expect(action.type).toEqual(actions.CLEAR_ALL_STORAGE_LIST);
    });

    it('should create Associations', () => {
        const json = [{}];

        const action = actions.createAssociations();

        expect(action.type).toEqual(actions.REQUEST_CREATE_ASSOCIATION);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.success).toBeDefined();

        const receiveAction = action.fetchConfig.success(json);
        expect(receiveAction.type).toEqual(actions.RECEIVE_CREATE_ASSOCIATION);
        expect(receiveAction.response).toEqual([{}]);
    });
});