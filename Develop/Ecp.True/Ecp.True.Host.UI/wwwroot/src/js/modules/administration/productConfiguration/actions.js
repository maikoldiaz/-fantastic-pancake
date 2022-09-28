import { apiService } from '../../../common/services/apiService';

export const SAVE_PRODUCT = 'SAVE_PRODUCT';
export const RECEIVE_SAVE_PRODUCT = 'RECEIVE_SAVE_PRODUCT';
export const INIT_PRODUCT = 'INIT_PRODUCT';
export const UPDATE_PRODUCT = 'UPDATE_PRODUCT';
export const RECEIVE_UPDATE_PRODUCT = 'RECEIVE_UPDATE_PRODUCT';
export const DELETE_PRODUCT = 'DELETE_PRODUCT';
export const RECEIVE_DELETE_PRODUCT = 'RECEIVE_DELETE_PRODUCT';
export const CLEAR_SUCCESS = 'CLEAR_SUCCESS';
export const REQUEST_PRODUCTS = 'REQUEST_PRODUCTS';
export const RECEIVE_PRODUCTS = 'RECEIVE_PRODUCTS';
export const REQUEST_STORAGE_BY_LOGISTIC_CENTER = 'REQUEST_STORAGE_BY_LOGISTIC_CENTER';
export const RECEIVE_STORAGE_BY_LOGISTIC_CENTER = 'RECEIVE_STORAGE_BY_LOGISTIC_CENTER';
export const REQUEST_DELETE_ASSOCIATION = 'REQUEST_DELETE_ASSOCIATION';
export const RECEIVE_DELETE_ASSOCIATION = 'RECEIVE_DELETE_ASSOCIATION';
export const CLEAR_STORAGE_LIST_BY_POSITION = 'CLEAR_STORAGE_LIST_BY_POSITION';
export const CLEAR_ALL_STORAGE_LIST = 'CLEAR_ALL_STORAGE_LIST';
export const REQUEST_CREATE_ASSOCIATION = 'REQUEST_CREATE_ASSOCIATION';
export const RECEIVE_CREATE_ASSOCIATION = 'RECEIVE_CREATE_ASSOCIATION';

export const receiveSaveProduct = response => {
    return {
        type: RECEIVE_SAVE_PRODUCT,
        status: !response.hasError,
        isActive: response.isActive
    };
};

export const clearSuccess = () => {
    return {
        type: CLEAR_SUCCESS,
        saveSuccess: undefined,
        updateSuccess: undefined,
        deleteSuccess: undefined,
        isActive: undefined
    };
};


export const saveProduct = body => {
    return {
        type: SAVE_PRODUCT,
        fetchConfig: {
            path: apiService.product.createProduct(),
            method: 'POST',
            body,
            success: receiveSaveProduct
        }
    };
};

export const receiveProducts = data => {
    return {
        type: RECEIVE_PRODUCTS,
        data
    };
};

export const getProducts = () => {
    return {
        type: REQUEST_PRODUCTS,
        fetchConfig: {
            path: apiService.product.getProducts(),
            success: receiveProducts
        }
    };
};

export const receiveStorageLocation = (data, logisticCenterId, position) => {
    return {
        type: RECEIVE_STORAGE_BY_LOGISTIC_CENTER,
        storages: data,
        logisticCenterId,
        position
    };
};

export const getStorageLocationsByLogisticCenter = (logisticCenterId, position) => {
    return {
        type: REQUEST_STORAGE_BY_LOGISTIC_CENTER,
        fetchConfig: {
            path: apiService.getStorageLocationsByLogisticCenter(logisticCenterId),
            success: response => receiveStorageLocation(response.value, logisticCenterId, position)
        }
    };
};

export const receiveDeleteAssociationCenterStorageProductSuccess = () => {
    return {
        type: RECEIVE_DELETE_ASSOCIATION,
        deleted: true
    };
};

const receiveDeleteAssociationCenterStorageProductFailed = () => {
    return {
        type: RECEIVE_DELETE_ASSOCIATION,
        deleted: false
    };
};

export const deleteAssociationCenterStorageProduct = associationId => {
    return {
        type: REQUEST_DELETE_ASSOCIATION,
        fetchConfig: {
            path: apiService.deleteAssociationCenterStorageProduct(associationId),
            method: 'DELETE',
            success: receiveDeleteAssociationCenterStorageProductSuccess,
            failure: receiveDeleteAssociationCenterStorageProductFailed
        }
    };
};

export const clearStorageListByPosition = position => {
    return {
        type: CLEAR_STORAGE_LIST_BY_POSITION,
        position
    };
};

export const clearAllStorageList = () => {
    return {
        type: CLEAR_ALL_STORAGE_LIST
    };
};

export const receiveCreateAssociationCenterStorageProductSuccess = response => {
    return {
        type: RECEIVE_CREATE_ASSOCIATION,
        response
    };
};

export const createAssociations = json => {
    return {
        type: REQUEST_CREATE_ASSOCIATION,
        fetchConfig: {
            path: apiService.createAssociationCenterStorageProduct(),
            method: 'POST',
            body: json,
            success: receiveCreateAssociationCenterStorageProductSuccess
        }
    };
};

export const receiveUpdateProduct = () => {
    return {
        type: RECEIVE_UPDATE_PRODUCT,
        status: true
    };
};

export const failReceiveUpdateProduct = () => {
    return {
        type: RECEIVE_UPDATE_PRODUCT,
        status: false
    };
};

export const updateProduct = (body, productSapId) => {
    return {
        type: UPDATE_PRODUCT,
        fetchConfig: {
            path: apiService.product.updateProduct(productSapId),
            method: 'PUT',
            body,
            success: receiveUpdateProduct,
            failure: failReceiveUpdateProduct
        }
    };
};

export const initProduct = initialValues => {
    return {
        type: INIT_PRODUCT,
        initialValues
    };
};

export const receiveDeleteProduct = () => {
    return {
        type: RECEIVE_DELETE_PRODUCT,
        status: true
    };
};

export const failReceiveDeleteProduct = () => {
    return {
        type: RECEIVE_DELETE_PRODUCT,
        status: false
    };
};

export const deleteProduct = productSapId => {
    return {
        type: DELETE_PRODUCT,
        fetchConfig: {
            path: apiService.product.deleteProduct(productSapId),
            method: 'DELETE',
            success: receiveDeleteProduct,
            failure: failReceiveDeleteProduct
        }
    };
};
