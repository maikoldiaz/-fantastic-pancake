import { apiService } from './../../../../common/services/apiService';

const REQUEST_ADD_CATEGORYELEMENT = 'REQUEST_ADD_CATEGORYELEMENT';
export const RECEIVE_ADD_CATEGORYELEMENT = 'RECEIVE_ADD_CATEGORYELEMENT';

const REQUEST_UPDATE_CATEGORYELEMENT = 'REQUEST_UPDATE_CATEGORYELEMENT';
export const RECEIVE_UPDATE_CATEGORYELEMENT = 'RECEIVE_UPDATE_CATEGORYELEMENT';

export const REQUEST_EDIT_INITIALIZE_CATEGORYELEMENT = 'REQUEST_EDIT_INITIALIZE_CATEGORYELEMENT';

const REQUEST_ICONS = 'REQUEST_ICONS';
export const RECEIVE_ICONS = 'RECEIVE_ICONS';

export const OPEN_ICON_MODAL = 'OPEN_ICON_MODAL';
export const REFRESH_STATUS = 'REFRESH_STATUS';
export const INITIALIZE_VALUE = 'INITIALIZE_VALUE';

const receiveAddCategoryElement = status => {
    return {
        type: RECEIVE_ADD_CATEGORYELEMENT,
        status
    };
};

const receiveUpdateCategoryElement = status => {
    return {
        type: RECEIVE_UPDATE_CATEGORYELEMENT,
        status
    };
};

export const createElement = categoryElement => {
    return {
        type: REQUEST_ADD_CATEGORYELEMENT,
        fetchConfig: {
            path: apiService.category.createOrUpdateElements(),
            success: receiveAddCategoryElement,
            body: categoryElement
        }
    };
};

export const updateElement = categoryElement => {
    return {
        type: REQUEST_UPDATE_CATEGORYELEMENT,
        fetchConfig: {
            path: apiService.category.createOrUpdateElements(),
            method: 'PUT',
            success: receiveUpdateCategoryElement,
            body: categoryElement
        }
    };
};

export const initializeEdit = categoryElement => {
    return {
        type: REQUEST_EDIT_INITIALIZE_CATEGORYELEMENT,
        categoryElement
    };
};

const receiveIcons = icons => {
    return {
        type: RECEIVE_ICONS,
        icons
    };
};

export const getIcons = () => {
    return {
        type: REQUEST_ICONS,
        fetchConfig: {
            path: apiService.category.getIcons(),
            success: receiveIcons
        }
    };
};

export const openIconModal = () => {
    return {
        type: OPEN_ICON_MODAL
    };
};

export const refreshStatus = () => {
    return {
        type: REFRESH_STATUS
    };
};

export const initializeValues = categoryElement => {
    return {
        type: INITIALIZE_VALUE,
        categoryElement
    };
};
