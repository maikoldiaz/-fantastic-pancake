import {
    FILE_REGISTRATION_ADD_FILE,
    RECEIVE_FILE_REGISTRATION_UPLOAD_STATUS,
    RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO,
    ON_FILEREGISTRATION_VALIDATION,
    ON_FILEREGISTRATION_REINJECT,
    RESET_FILEREGISTRATION_UPLOADPOPUP,
    SET_SELECTEDFILETYPE,
    SET_GRID_FILTER,
    CLEAR_GRID_FILTER,
    INITIALIZE_FILE_UPLOAD_FILTER,
    START_FILE_UPLOAD,
    RECEIVE_FILE_REGISTRATION_FAILURE
} from './actions.js';

const initialState = {
    reinjectActionTypeKey: '4',
    selectedActionType: 0,
    resetFormToggler: false,
    receiveStatusToggler: false,
    receiveAccessInfoToggler: false,
    browseFile: {},
    validationResult: { success: true },
    reInjectFileInfo: {},
    isValidSelection: false,
    gridFilter: false,
    fileUploadInProgress: false
};
export const fileRegistration = function (state = initialState, action = {}) {
    switch (action.type) {
    case FILE_REGISTRATION_ADD_FILE:
        return Object.assign({}, state, { isValidSelection: true, browseFile: action.data });
    case RECEIVE_FILE_REGISTRATION_UPLOAD_STATUS:
        return Object.assign({}, state, { receiveStatusToggler: action.status ? !state.receiveStatusToggler : state.receiveStatusToggler });
    case RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO:
        return Object.assign({}, state, { uploadAccessInfo: action.accessInfo, receiveAccessInfoToggler: !state.receiveAccessInfoToggler });
    case ON_FILEREGISTRATION_VALIDATION:
        return Object.assign({}, state, { validationResult: action.validationResult });
    case ON_FILEREGISTRATION_REINJECT:
        return Object.assign({}, state, { reInjectFileInfo: action.reInjectFileInfo });
    case RESET_FILEREGISTRATION_UPLOADPOPUP:
        return Object.assign({}, state, { reInjectFileInfo: {}, validationResult: {}, isValidSelection: false, browseFile: {}, selectedFileType: null });
    case SET_SELECTEDFILETYPE:
        return Object.assign({}, state, { selectedFileType: action.selectedType });
    case SET_GRID_FILTER:
        return Object.assign({}, state, { gridFilter: true });
    case CLEAR_GRID_FILTER:
        return Object.assign({}, state, { gridFilter: false });
    case INITIALIZE_FILE_UPLOAD_FILTER:
        return Object.assign({}, state, { initialValues: action.initialValues });
    case START_FILE_UPLOAD:
        return Object.assign({}, state, { fileUploadInProgress: action.status });
    case RECEIVE_FILE_REGISTRATION_FAILURE:
        return Object.assign({}, state, { failureToggler: !state.failureToggler });
    default:
        return state;
    }
};

