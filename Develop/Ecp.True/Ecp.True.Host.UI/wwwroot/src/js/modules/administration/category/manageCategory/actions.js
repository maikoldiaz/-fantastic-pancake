import { apiService } from './../../../../common/services/apiService';

const REQUEST_GET_CATEGORIES = 'REQUEST_GET_CATEGORIES';
export const RECEIVE_GET_CATEGORIES = 'RECEIVE_GET_CATEGORIES';

const REQUEST_ADD_CATEGORY = 'REQUEST_ADD_CATEGORY';
export const RECEIVE_ADD_CATEGORY = 'RECEIVE_ADD_CATEGORY';

const REQUEST_UPDATE_CATEGORY = 'REQUEST_UPDATE_CATEGORY';
export const RECEIVE_UPDATE_CATEGORY = 'RECEIVE_UPDATE_CATEGORY';

export const REQUEST_EDIT_INITIALIZE_CATEGORY = 'REQUEST_EDIT_INITIALIZE_CATEGORY';

const receiveGetCategory = categories => {
    return {
        type: RECEIVE_GET_CATEGORIES,
        categories
    };
};

const receiveAddCategory = status => {
    return {
        type: RECEIVE_ADD_CATEGORY,
        status
    };
};

const receiveUpdateCategory = status => {
    return {
        type: RECEIVE_UPDATE_CATEGORY,
        status
    };
};

export const getCategories = () => {
    return {
        type: REQUEST_GET_CATEGORIES,
        fetchConfig: {
            path: apiService.category.getAll(true),
            success: receiveGetCategory
        }
    };
};

export const createCategory = category => {
    return {
        type: REQUEST_ADD_CATEGORY,
        fetchConfig: {
            path: apiService.category.createOrUpdate(),
            success: receiveAddCategory,
            body: category
        }
    };
};

export const updateCategory = category => {
    return {
        type: REQUEST_UPDATE_CATEGORY,
        fetchConfig: {
            path: apiService.category.createOrUpdate(),
            method: 'PUT',
            success: receiveUpdateCategory,
            body: category
        }
    };
};

export const initializeEdit = category => {
    return {
        type: REQUEST_EDIT_INITIALIZE_CATEGORY,
        category
    };
};
