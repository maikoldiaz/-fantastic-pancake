import * as actions from '../../../modules/transportBalance/fileUploads/actions';
import { apiService } from '../../../common/services/apiService';

describe('Actions for file uploads', () => {
    it('should add file on file registration', () => {
        const FILE_REGISTRATION_ADD_FILE = 'FILE_REGISTRATION_ADD_FILE';
        const file = 'testFile';
        const action = actions.fileRegistrationAddFile(file);
        expect(action.type).toEqual(FILE_REGISTRATION_ADD_FILE);
        expect(action.data).toEqual(file);
    });
    it('should receive file registration upload status', () => {
        const RECEIVE_FILE_REGISTRATION_UPLOAD_STATUS = 'RECEIVE_FILE_REGISTRATION_UPLOAD_STATUS';
        const status = 'processing';
        const action = actions.onSaveFileRegistration(status);
        expect(action.type).toEqual(RECEIVE_FILE_REGISTRATION_UPLOAD_STATUS);
        expect(action.status).toEqual(status);
    });
    it('should receive file registration upload access info', () => {
        const RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO = 'RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO';
        const accessInfo = 'test access info';
        const action = actions.receiveFileRegistrationUploadAccessInfo(accessInfo);
        expect(action.type).toEqual(RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO);
        expect(action.accessInfo).toEqual(accessInfo);
    });
    it('should perform on file registration validation', () => {
        const ON_FILEREGISTRATION_VALIDATION = 'ON_FILEREGISTRATION_VALIDATION';
        const validationResult = true;
        const action = actions.onFileRegistrationValidation(validationResult);
        expect(action.type).toEqual(ON_FILEREGISTRATION_VALIDATION);
        expect(action.validationResult).toEqual(validationResult);
    });
    it('should perform on file registration reinject', () => {
        const ON_FILEREGISTRATION_REINJECT = 'ON_FILEREGISTRATION_REINJECT';
        const reInjectFileInfo = true;
        const action = actions.onFileRegistrationReinject(reInjectFileInfo);
        expect(action.type).toEqual(ON_FILEREGISTRATION_REINJECT);
        expect(action.reInjectFileInfo).toEqual(reInjectFileInfo)
    });
    it('should reinject file registration upload pop up', () => {
        const RESET_FILEREGISTRATION_UPLOADPOPUP = 'RESET_FILEREGISTRATION_UPLOADPOPUP';
        const reInjectFileInfo = true;
        const action = actions.resetFileregistrationUploadPopup(reInjectFileInfo);
        expect(action.type).toEqual(RESET_FILEREGISTRATION_UPLOADPOPUP);
        expect(action.reInjectFileInfo).toEqual(reInjectFileInfo)
    });
    it('should set selected file', () => {
        const SET_SELECTEDFILETYPE = 'SET_SELECTEDFILETYPE';
        const selectedType = 'Contract';
        const action = actions.setSelectedFileType(selectedType);
        expect(action.type).toEqual(SET_SELECTEDFILETYPE);
        expect(action.selectedType).toEqual(selectedType);
    });
    it('should set grid filter', () =>{
        const SET_GRID_FILTER = 'SET_GRID_FILTER';
        const action = actions.updateGridFilter();

        expect(action.type).toEqual(SET_GRID_FILTER);
    });
    it('should clear grid filter', () =>{
        const CLEAR_GRID_FILTER = 'CLEAR_GRID_FILTER';
        const action = actions.clearGridFilter();

        expect(action.type).toEqual(CLEAR_GRID_FILTER);
    });
    it('should set the upload progress variable', () =>{
        const START_FILE_UPLOAD = 'START_FILE_UPLOAD';
        const action = actions.setFileUploadProgressStatus(true);

        expect(action.type).toEqual(START_FILE_UPLOAD);
    });
});
