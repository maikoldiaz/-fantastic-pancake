import { apiService } from '../../../common/services/apiService';

export const FILTER_CATEGORY_ELEMENTS = 'FILTER_CATEGORY_ELEMENTS';
export const RECEIVE_FILTER_CATEGORY_ELEMENTS = 'RECEIVE_FILTER_CATEGORY_ELEMENTS';
export const REQUEST_UPDATE_DEVIATION = 'REQUEST_UPDATE_DEVIATION';
export const RECEIVE_UPDATE_DEVIATION = 'RECEIVE_UPDATE_DEVIATION';

export const receiveFilterCategoryElements = function (json) {
    return {
        type: RECEIVE_FILTER_CATEGORY_ELEMENTS,
        items: json
    };
};

export const filterCategoryElements = filter => {
    return {
        type: FILTER_CATEGORY_ELEMENTS,
        fetchConfig: {
            path: apiService.getFilterCategoryElements(filter),
            method: 'GET',
            success: json => receiveFilterCategoryElements(json)
        }
    };
};

const receiveUpdateDeviation = status => {
    return {
        type: RECEIVE_UPDATE_DEVIATION,
        status
    };
};

export const updateDeviation = segments => {
    return {
        type: REQUEST_UPDATE_DEVIATION,
        fetchConfig: {
            path: apiService.updateDeviationPercentage(),
            method: 'PUT',
            success: receiveUpdateDeviation,
            body: segments
        }
    };
};
