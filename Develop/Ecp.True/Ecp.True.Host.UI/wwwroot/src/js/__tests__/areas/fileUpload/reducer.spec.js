import * as actions from '../../../modules/transportBalance/fileUploads/actions';
import { fileRegistration } from '../../../modules/transportBalance/fileUploads/reducers';
describe('Reducer for file uploads', () => {
    const initialState = {
        isValidSelection: false,
        browseFile: '',
        receiveStatusToggler: false,
        receiveAccessInfoToggler: false,
        validationResult: '',
        selectedFileType: '',
        fileUploadInProgress: false
    };
    it('should add a new file', () => {
        const action = {
            type: actions.FILE_REGISTRATION_ADD_FILE,
            data: 'test data'
        };

        const newState = Object.assign({}, initialState, {
            browseFile: action.data,
            isValidSelection: true
        });
        expect(fileRegistration(initialState, action)).toEqual(newState);
    })
    it('should receive file registration upload status', () => {
        const action = {
            type: actions.RECEIVE_FILE_REGISTRATION_UPLOAD_STATUS,
            status: true
        }
        const newState = Object.assign({}, initialState, {
            receiveStatusToggler: true,
            isValidSelection: false
        });
        expect(fileRegistration(initialState, action)).toEqual(newState);
    })
    it('should receive file registration upload access info', () => {
        const action = {
            type: actions.RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO,
            accessInfo: 'test access info',
            status: true
        }
        const newState = Object.assign({}, initialState, {
            receiveStatusToggler: false,
            isValidSelection: false,
            uploadAccessInfo: 'test access info',
            receiveAccessInfoToggler: true
        });
        expect(fileRegistration(initialState, action)).toEqual(newState);
    })
    it('should perform on file registration validation', () => {
        const action = {
            type: actions.ON_FILEREGISTRATION_VALIDATION,
            validationResult: 'test validation result'
        }
        const newState = Object.assign({}, initialState, {
            validationResult: 'test validation result',
        });
        expect(fileRegistration(initialState, action)).toEqual(newState);
    })
    it('should perform on file registration reinject', () => {
        const action = {
            type: actions.ON_FILEREGISTRATION_REINJECT,
            reInjectFileInfo: 'test reinject info'
        }
        const newState = Object.assign({}, initialState, {
            reInjectFileInfo: 'test reinject info',
        });
        expect(fileRegistration(initialState, action)).toEqual(newState);
    })
    it('should perform on reset file registration upload pop up', () => {
        const action = {
            type: actions.RESET_FILEREGISTRATION_UPLOADPOPUP
        };
        const newState = Object.assign({}, initialState, {
            reInjectFileInfo: {},
            validationResult: {},
            isValidSelection: false,
            browseFile: {},
            selectedFileType: null
        });
        expect(fileRegistration(initialState, action)).toEqual(newState);
    });
    it('should set the selected file', () => {
        const action = {
            type: actions.SET_SELECTEDFILETYPE,
            selectedType: 'test'
        };
        const newState = Object.assign({}, initialState, {
            selectedFileType: 'test'
        });
        expect(fileRegistration(initialState, action)).toEqual(newState);
    });
    it('should set the grid filter', () => {
        const action = {
            type: actions.SET_GRID_FILTER
        };
        const newState = Object.assign({}, initialState, { gridFilter: true });
        expect(fileRegistration(initialState, action)).toEqual(newState);
    });
    it('should clear the grid filter', () => {
        const action = {
            type: actions.CLEAR_GRID_FILTER
        };
        const newState = Object.assign({}, initialState, { gridFilter: false });
        expect(fileRegistration(initialState, action)).toEqual(newState);
    });
    it('should set the upload progress variable', () => {
        const action = {
            type: actions.START_FILE_UPLOAD,
            status: true
        };
        const newState = Object.assign({}, initialState, { fileUploadInProgress: true });
        expect(fileRegistration(initialState, action)).toEqual(newState);
    });
});
