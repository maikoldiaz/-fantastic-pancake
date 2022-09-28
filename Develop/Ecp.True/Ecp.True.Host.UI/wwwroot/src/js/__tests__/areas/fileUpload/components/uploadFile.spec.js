import setup from '../../setup.js';
import React from 'react';
import { mount } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import configureStore from 'redux-mock-store';
import { Provider } from 'react-redux';
import { UploadFileComponent } from '../../../../modules/transportBalance/fileUploads/components/uploadFile.jsx';
import { excelService } from '../../../../common/services/excelService';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants.js';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';

const uploadActionTypes = {
    1: 'Insertar',
    2: 'Actualizar',
    3: 'Elimina',
    4: 'Reinyectar'
};

const initialState = {
    messages: {
        fileRegistration: {
            browseFile: { name: 'movement.xlsx' },
            isValidSelection: false,
            receiveStatusToggler: true,
            receiveAccessInfoToggler: true,
            uploadAccessInfo: 'uploadToken',
            excelFileInfo: {
                Columns: ['MovementId', 'StartDate'],
                mandatorySheets: ['Movement']
            },
            validationResult: { status: true },
            reinjectActionTypeKey: 'reject_key'
        }
    },
    shared: {
        registrationActionTypes: uploadActionTypes,
        categoryElements: [
            {
                isActive: true,
                categoryId: 1
            },
            {
                isActive: true,
                categoryId: 2
            },
            {
                isActive: true,
                categoryId: 2
            }
        ]
    },
    addBrowsedFile: jest.fn(() => Promise.resolve),
    closeModal: jest.fn(() => Promise.resolve)
};

function mountWithReduxStore(defaultState) {
    const mockStore = configureStore();
    const store = mockStore(defaultState);
    const props = {
        componentType: constants.SystemType.EXCEL,
        closeModal: jest.fn(() => Promise.resolve),
        addBrowsedFile: jest.fn(() => Promise.resolve),
        onFileRegistrationValidation: jest.fn(() => Promise.resolve),
        requestFileRegistrationUploadAccess: jest.fn(() => Promise.resolve),
        onSubmit: jest.fn(() => Promise.resolve()),
        browseFile: defaultState.messages.fileRegistration.browseFile,
        fileName: defaultState.messages.fileRegistration.browseFile.name,
        isValidSelection: defaultState.messages.fileRegistration.isValidSelection,
        actionTypes: defaultState.shared.registrationActionTypes,
        receiveStatusToggler: defaultState.messages.fileRegistration.receiveStatusToggler,
        receiveAccessInfoToggler: defaultState.messages.fileRegistration.receiveAccessInfoToggler,
        uploadAccessInfo: defaultState.messages.fileRegistration.uploadAccessInfo,
        excelFileInfo: defaultState.messages.fileRegistration.excelFileInfo,
        validationResult: defaultState.messages.fileRegistration.validationResult,
        categoryElements: defaultState.shared.categoryElements
    };
    const enzymeWrapper = mount(<Provider store={store}><UploadFileComponent {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

function mountWithRealStore(defaultState, shared, componentType = constants.SystemType.EXCEL) {
    const reducers = {
        form: formReducer,
        messages: jest.fn(() => defaultState.messages),
        shared: jest.fn(() => shared)
    };

    const props = {
        componentType,
        closeModal: jest.fn(() => Promise.resolve),
        addBrowsedFile: jest.fn(() => Promise.resolve),
        onFileRegistrationValidation: jest.fn(() => Promise.resolve),
        requestFileRegistrationUploadAccess: jest.fn(() => Promise.resolve),
        onSubmit: jest.fn(() => Promise.resolve()),
        browseFile: defaultState.messages.fileRegistration.browseFile,
        fileName: defaultState.messages.fileRegistration.browseFile.name,
        isValidSelection: defaultState.messages.fileRegistration.isValidSelection,
        actionTypes: defaultState.shared.registrationActionTypes,
        receiveStatusToggler: defaultState.messages.fileRegistration.receiveStatusToggler,
        receiveAccessInfoToggler: defaultState.messages.fileRegistration.receiveAccessInfoToggler,
        uploadAccessInfo: defaultState.messages.fileRegistration.uploadAccessInfo,
        excelFileInfo: defaultState.messages.fileRegistration.excelFileInfo,
        validationResult: defaultState.messages.fileRegistration.validationResult,
        categoryElements: defaultState.shared.categoryElements
    };
    const store = createStore(combineReducers(reducers), { messages: defaultState, shared: shared });
    const enzymeWrapper1 = mount(<Provider store={store}><UploadFileComponent {...props} /></Provider>);

    return { store, enzymeWrapper1, props };
}

describe('upload File', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
    });

    it('should mount successfully', () => {
        const shared = { registrationActionTypes: uploadActionTypes, categoryElements: initialState.shared.categoryElements };
        const { enzymeWrapper1 } = mountWithRealStore(initialState, shared);
        expect(enzymeWrapper1).toHaveLength(1);
    });

    it('should count number of options in reinject upload case ', () => {
        const { enzymeWrapper } = mountWithReduxStore(Object.assign({}, initialState,
            {
                messages: {
                    fileRegistration: {
                        ...initialState.messages.fileRegistration,
                        reinjectActionTypeKey: 4,
                        reInjectFileInfo: { value: 'Reinyectar' }
                    }
                }
            }));
        expect(enzymeWrapper.find('#dd_fileUpload_actions')).toHaveLength(11);
    });

    it('should mount successfully and find Control', () => {
        const { enzymeWrapper } = mountWithReduxStore(initialState);
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#dd_fileUpload_actions')).toBe(true);
        expect(enzymeWrapper.exists('#input_FileUpload')).toBe(true);
        expect(enzymeWrapper.find(ModalFooter).exists('#btn_uploadFile_cancel')).toBe(true);
        expect(enzymeWrapper.find(ModalFooter).exists('#btn_uploadFile_submit')).toBe(true);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_uploadFile_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find(ModalFooter).find('#btn_uploadFile_submit').text()).toEqual('submit');
    });

    it('should show invalid file in case of invalid file or data ', () => {
        excelService.validateExcel = jest.fn();
        const shared = { registrationActionTypes: uploadActionTypes, categoryElements: initialState.shared.categoryElements };
        const state = initialState;
        state.messages.fileRegistration.validationResult.status = false;

        const { enzymeWrapper1 } = mountWithRealStore(state, shared);
        expect(enzymeWrapper1.find('#span_file_error_message').exists()).toBe(true);
    });

    it('should check validate excel not call when button is disabled', () => {
        excelService.validateExcel = jest.fn();
        const { enzymeWrapper } = mountWithReduxStore(Object.assign({}, initialState,
            {
                messages: {
                    fileRegistration: {
                        ...initialState.messages.fileRegistration,
                        browseFile: { ...initialState.messages.fileRegistration.browseFile, name: null }
                    }
                }
            }));
        enzymeWrapper.find(ModalFooter).find('#btn_uploadFile_submit').simulate('click');
        expect(excelService.validateExcel.mock.calls).toHaveLength(0);
    });

    it('should cancel file upload when cancel button is clicked ', () => {
        const shared = { registrationActionTypes: uploadActionTypes, categoryElements: initialState.shared.categoryElements };
        const { enzymeWrapper1 } = mountWithRealStore(initialState, shared);
        enzymeWrapper1.find(ModalFooter).find('#btn_uploadFile_cancel').simulate('click');
        expect(enzymeWrapper1).toHaveLength(1);
    });

    it('should check validate excel is called', () => {
        excelService.validateExcel = jest.fn();
        const { enzymeWrapper } = mountWithReduxStore(Object.assign({}, initialState,
            {
                messages: {
                    fileRegistration: {
                        ...initialState.messages.fileRegistration,
                        browseFile: { ...initialState.messages.fileRegistration.browseFile, name: 'Name' }
                    }
                }
            }));
        enzymeWrapper.find('form').simulate('submit');
        expect(excelService.validateExcel.mock.calls).toHaveLength(1);
    });

    it('should add file when file is browsed ', () => {
        const shared = { registrationActionTypes: uploadActionTypes };
        const { enzymeWrapper1, props } = mountWithRealStore(initialState, shared);
        const fileContents = 'file contents';
        const file = new Blob([fileContents], { type: 'excel/application' });
        enzymeWrapper1.find('#input_FileUpload').simulate('change', { target: { files: [file] } });
        expect(props.addBrowsedFile.mock.calls).toHaveLength(1);
    });

    it('should throw required error for segment on click of submit', () => {
        const shared = { registrationActionTypes: uploadActionTypes };
        const { enzymeWrapper1 } = mountWithRealStore(initialState, shared);
        enzymeWrapper1.find('form').simulate('submit');
        expect(enzymeWrapper1.find('input').find('#dd_fileUpload_segment').value).toBeUndefined();
        expect(enzymeWrapper1.find('.ep-control__error-txt').exists()).toBeTruthy();
        expect(enzymeWrapper1.find('.ep-control__error-txt').at(0).text()).toEqual('required');
    });

    it('should throw required error for action on click of submit', () => {
        const shared = { registrationActionTypes: uploadActionTypes };
        const { enzymeWrapper1 } = mountWithRealStore(initialState, shared, constants.SystemType.EXCEL);
        enzymeWrapper1.find('form').simulate('submit');
        expect(enzymeWrapper1.find('input').find('#dd_fileUpload_actions').value).toBeUndefined();
        expect(enzymeWrapper1.find('.ep-control__error-txt').exists()).toBeTruthy();
        expect(enzymeWrapper1.find('.ep-control__error-txt').at(0).text()).toEqual('required');
    });

    it('should render required only action when component type is CONTRACT', () => {
        const shared = { registrationActionTypes: uploadActionTypes };
        const { enzymeWrapper1 } = mountWithRealStore(initialState, shared, constants.SystemType.CONTRACT);
        enzymeWrapper1.find('form').simulate('submit');
        expect(enzymeWrapper1.find('input').find('#dd_fileUpload_actions').value).toBeUndefined();
        expect(enzymeWrapper1.find('.ep-control__error-txt').exists()).toBeTruthy();
        expect(enzymeWrapper1.find('.ep-control__error-txt').at(0).text()).toEqual('required');
        expect(enzymeWrapper1.find('.ep-control__error-txt')).toHaveLength(1);
    });
});
