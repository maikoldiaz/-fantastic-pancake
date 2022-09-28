import * as actions from '../../modules/transportBalance/fileUploads/actions';
import { apiService } from '../../common/services/apiService.js';

describe('file upload actions', () => {
    it('should initiate Add File Action', () => {
        const INIT_FILE_REGISTRATION_ADD_FILE = 'FILE_REGISTRATION_ADD_FILE';
        const fileObject = {
            fileName: 'movement.xls'
        };
        const action = actions.fileRegistrationAddFile(fileObject);
        const m_action = {
            type: INIT_FILE_REGISTRATION_ADD_FILE,
            data: fileObject
        };

        expect(action.type).toEqual(m_action.type);
        expect(action.data).toEqual(m_action.data);
    });

    it('should initiate File Registration Action', () => {
        const INIT_FILE_REGISTRATION_UPLOAD_STATUS = 'RECEIVE_FILE_REGISTRATION_UPLOAD_STATUS';
        const fileObject = {
            fileName: 'movement.xls',
            isValid: true
        };
        const action = actions.onSaveFileRegistration(fileObject);
        const m_action = {
            type: INIT_FILE_REGISTRATION_UPLOAD_STATUS,
            status: fileObject
        };

        expect(action.type).toEqual(m_action.type);
        expect(action.status).toEqual(m_action.status);
    });

    it('should receive File Registration Upload Access Info', () => {
        const RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO = 'RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO';
        const accessInfo = {
            fileName: 'movement.xls',
            isAuthorized: true
        };
        const action = actions.receiveFileRegistrationUploadAccessInfo(accessInfo);
        const m_action = {
            type: RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO,
            accessInfo: accessInfo
        };

        expect(action.type).toEqual(m_action.type);
        expect(action.accessInfo).toEqual(m_action.accessInfo);
    });

    it('should validate File Registration', () => {
        const ON_FILEREGISTRATION_VALIDATION = 'ON_FILEREGISTRATION_VALIDATION';
        const validationResult = {
            isValid: true
        };
        const action = actions.onFileRegistrationValidation(validationResult);
        const m_action = {
            type: ON_FILEREGISTRATION_VALIDATION,
            validationResult: validationResult
        };

        expect(action.type).toEqual(m_action.type);
        expect(action.validationResult).toEqual(m_action.validationResult);
    });

    it('should Register new file upload', () => {
        const REQUEST_FILEREGISTRATION_UPLOADFILE = 'REQUEST_FILEREGISTRATION_UPLOADFILE';
        const INIT_FILEREGISTRATION_UPLOAD_STATUS = 'RECEIVE_FILE_REGISTRATION_UPLOAD_STATUS';
        const fileRegistrationObject = { fileName: 'movement.xls' };
        const fileObject = { key: '1' };

        const action = actions.requestFileRegistrationUpload(fileRegistrationObject);

        expect(action.type).toEqual(REQUEST_FILEREGISTRATION_UPLOADFILE);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.fileUpload.create());
        expect(action.fetchConfig.success).toBeDefined();
        const receiveAction = action.fetchConfig.success(fileObject);
        expect(receiveAction.type).toEqual(INIT_FILEREGISTRATION_UPLOAD_STATUS);
        expect(receiveAction.status).toEqual(fileObject);

        expect(action.fetchConfig.body).toBeDefined();
        expect(action.fetchConfig.body).toEqual(fileRegistrationObject);
    });

    it('should upload file Access Info', () => {
        const REQUEST_FILEREGISTRATION_UPLOADACCESSINFO = 'REQUEST_FILEREGISTRATION_UPLOADACCESSINFO';
        const RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO = 'RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO';
        const fileName = 'movement.xls';
        const fileObject = { key: '1' };

        const action = actions.requestFileRegistrationUploadAccessInfo(fileName);

        expect(action.type).toEqual(REQUEST_FILEREGISTRATION_UPLOADACCESSINFO);
        expect(action.fetchConfig).toBeDefined();
        expect(action.fetchConfig.path).toEqual(apiService.fileUpload.getUploadAccessInfo(fileName));
        expect(action.fetchConfig.success).toBeDefined();
        const receiveAction = action.fetchConfig.success(fileObject);
        expect(receiveAction.type).toEqual(RECEIVE_FILEREGISTRATION_UPLOADACCESSINFO);
        expect(receiveAction.accessInfo).toEqual(fileObject);
    });
});
