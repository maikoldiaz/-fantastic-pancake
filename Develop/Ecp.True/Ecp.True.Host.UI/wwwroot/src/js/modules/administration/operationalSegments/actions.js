import { apiService } from './../../../common/services/apiService';
export const SAVE_SON_SEGMENTS = 'SAVE_SON_SEGMENTS';
export const UPDATE_SON_SEGMENTS_SUCCESS = 'UPDATE_SON_SEGMENTS_SUCCESS';
export const UPDATE_SON_SEGMENTS_FAILURE = 'UPDATE_SON_SEGMENTS_FAILURE';
export const UPDATE_SEGMENTS = 'UPDATE_SON_SEGMENTS';

export const updateSegmentsSuccess = data => {
    return {
        type: UPDATE_SON_SEGMENTS_SUCCESS,
        data
    };
};

export const updateSegmentsFailure = () => {
    return {
        type: UPDATE_SON_SEGMENTS_FAILURE
    };
};

export const saveSegments = segments => {
    return {
        type: SAVE_SON_SEGMENTS,
        fetchConfig: {
            path: apiService.category.updateCategoryElements(),
            method: 'PUT',
            body: segments,
            success: updateSegmentsSuccess,
            failure: updateSegmentsFailure
        }
    };
};

export const updateSegments = (operational, others) => {
    return {
        type: UPDATE_SEGMENTS,
        operational,
        others
    };
};

