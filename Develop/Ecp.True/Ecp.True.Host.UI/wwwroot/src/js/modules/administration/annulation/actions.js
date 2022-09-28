import { apiService } from '../../../common/services/apiService';
import { constants } from '../../../common/services/constants';

export const INIT_ANNULATION_TYPES = 'INIT_ANNULATION_TYPES';
export const UPDATE_ANNULATION_TYPES = 'UPDATE_ANNULATION_TYPES';
export const RESET_ANNULATION_TYPES = 'RESET_ANNULATION_TYPES';
export const SAVE_ANNULATION = 'SAVE_ANNULATION';
export const RECEIVE_SAVE_ANNULATION = 'RECEIVE_SAVE_ANNULATION';
export const INIT_ANNULATION = 'INIT_ANNULATION';

export const initTypes = () => {
    return {
        type: INIT_ANNULATION_TYPES
    };
};

export const updateTypes = (name, value, field) => {
    return {
        type: UPDATE_ANNULATION_TYPES,
        name,
        value,
        field
    };
};

export const resetTypes = (name, field) => {
    return {
        type: RESET_ANNULATION_TYPES,
        name,
        field
    };
};

export const receiveSaveAnnulation = status => {
    return {
        type: RECEIVE_SAVE_ANNULATION,
        status
    };
};

export const saveAnnulation = (body, mode) => {
    return {
        type: SAVE_ANNULATION,
        fetchConfig: {
            path: apiService.annulation.create(),
            method: mode === constants.Modes.Update ? 'PUT' : 'POST',
            body,
            success: receiveSaveAnnulation
        }
    };
};

export const initAnnulation = initialValues => {
    return {
        type: INIT_ANNULATION,
        initialValues
    };
};
