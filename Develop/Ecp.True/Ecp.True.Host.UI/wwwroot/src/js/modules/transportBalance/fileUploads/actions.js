import { apiService } from '../../../common/services/apiService';
import { receiveGridData } from '../../../common/components/grid/actions';

export const FILE_REGISTRATION_ADD_FILE = 'FILE_REGISTRATION_ADD_FILE';
export const REQUEST_FILEREGISTRATION_UPLOADFILE = 'REQUEST_FILEREGISTRATION_UPLOADFILE';
export const RECEIVE_FILE_REGISTRATION_UPLOAD_STATUS = 'RECEIVE_FILE_REGISTRATION_UPLOAD_STATUS';
export const REQUEST_FILEREGISTRATION_UPLOADACCESSINFO = 'REQUEST_FILEREGISTRATION_UPLOADACCESSINFO';
export const RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO = 'RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO';
export const ON_FILEREGISTRATION_VALIDATION = 'ON_FILEREGISTRATION_VALIDATION';
export const ON_FILEREGISTRATION_REINJECT = 'ON_FILEREGISTRATION_REINJECT';
export const RESET_FILEREGISTRATION_UPLOADPOPUP = 'RESET_FILEREGISTRATION_UPLOADPOPUP';
export const REQUEST_FILEREGISTRATION_GRID_DETAILS = 'REQUEST_FILEREGISTRATION_GRID_DETAILS';
export const SET_SELECTEDFILETYPE = 'SET_SELECTEDFILETYPE';
export const SET_GRID_FILTER = 'SET_GRID_FILTER';
export const CLEAR_GRID_FILTER = 'CLEAR_GRID_FILTER';
export const INITIALIZE_FILE_UPLOAD_FILTER = 'INITIALIZE_FILE_UPLOAD_FILTER';
export const START_FILE_UPLOAD = 'START_FILE_UPLOAD';
export const RECEIVE_FILE_REGISTRATION_FAILURE = 'RECEIVE_FILE_REGISTRATION_FAILURE';

export const fileRegistrationAddFile = file => {
    return {
        type: FILE_REGISTRATION_ADD_FILE,
        data: file
    };
};

export const onSaveFileRegistration = status => {
    return {
        type: RECEIVE_FILE_REGISTRATION_UPLOAD_STATUS,
        status
    };
};

const onSaveFileRegistrationFailure = () => {
    return {
        type: RECEIVE_FILE_REGISTRATION_FAILURE
    };
};


export const receiveFileRegistrationUploadAccessInfo = accessInfo => {
    return {
        type: RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO,
        accessInfo
    };
};

export const onFileRegistrationValidation = validationResult => {
    return {
        type: ON_FILEREGISTRATION_VALIDATION,
        validationResult
    };
};

export const onFileRegistrationReinject = reInjectFileInfo => {
    return {
        type: ON_FILEREGISTRATION_REINJECT,
        reInjectFileInfo
    };
};

export const resetFileregistrationUploadPopup = reInjectFileInfo => {
    return {
        type: RESET_FILEREGISTRATION_UPLOADPOPUP,
        reInjectFileInfo
    };
};

export const requestFileRegistrationUpload = file => {
    return {
        type: REQUEST_FILEREGISTRATION_UPLOADFILE,
        fetchConfig: {
            path: apiService.fileUpload.create(),
            body: file,
            success: onSaveFileRegistration,
            failure: onSaveFileRegistrationFailure

        }
    };
};

export const requestFileRegistrationUploadAccessInfo = (fileName, selectedFileType) => {
    return {
        type: REQUEST_FILEREGISTRATION_UPLOADACCESSINFO,
        fetchConfig: {
            path: apiService.fileUpload.getUploadAccessInfo(fileName, selectedFileType),
            success: receiveFileRegistrationUploadAccessInfo
        }
    };
};

export const requestFileRegistrationGridDetails = (path, name) => {
    return {
        type: REQUEST_FILEREGISTRATION_GRID_DETAILS,
        fetchConfig: {
            path,
            success: json => receiveGridData(json, name)
        }
    };
};

export const setSelectedFileType = function (selectedType) {
    return {
        type: SET_SELECTEDFILETYPE,
        selectedType
    };
};

export const updateGridFilter = () => {
    return {
        type: SET_GRID_FILTER
    };
};

export const clearGridFilter = () => {
    return {
        type: CLEAR_GRID_FILTER
    };
};

export const initializeFileUploadFilter = initialValues => {
    return {
        type: INITIALIZE_FILE_UPLOAD_FILTER,
        initialValues
    };
};

export const setFileUploadProgressStatus = status => {
    return {
        type: START_FILE_UPLOAD,
        status
    };
};
