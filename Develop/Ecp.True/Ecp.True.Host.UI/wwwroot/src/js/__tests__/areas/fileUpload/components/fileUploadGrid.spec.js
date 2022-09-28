import setup from '../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider.js';
import { httpService } from '../../../../common/services/httpService.js';
import FileUploadGrid, { FileUploadGrid as FileUploadGridComponent } from '../../../../modules/transportBalance/fileUploads/components/fileUploadGrid.jsx';
import blobService from '../../../../common/services/blobService';
import { utilities } from '../../../../common/services/utilities';
import { constants } from '../../../../common/services/constants';
import { navigationService } from '../../../../common/services/navigationService';
import Grid from '../../../../common/components/grid/grid.jsx';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { dateService } from '../../../../common/services/dateService';

const dataGrid = {
    FileuploadGrid: {
        config: {
            apiUrl: ''
        },
        items: [
            {
                uploadId: '830e8066-cd4a-4b6a-9306-f3d420b70366',
                errorCount: 1,
                status: 'Fallido',
                actionType: 'Insertar',
                segmentName: 'Automation_iu38c',
                name: 'b5da9cf4-9247-43bd-b6e5-040722fbd0f4.xlsx',
                systemTypeId: 'EXCEL',
                recordsProcessed: 1,
                isParsed: true,
                createdBy: 'trueadmin',
                createdDate: '2020-06-03T07:00:31.33Z',
                lastModifiedBy: null,
                lastModifiedDate: null
            },
            {
                uploadId: 'dfd16d74-e78e-4728-b189-0cc71954859e',
                errorCount: 1,
                status: 'Fallido',
                actionType: 'Insertar',
                segmentName: 'Automation_iu38c',
                name: 'b5da9cf4-9247-43bd-b6e5-040722fbd0f4.xlsx',
                systemTypeId: 'EXCEL',
                recordsProcessed: 1,
                isParsed: true,
                createdBy: 'trueadmin',
                createdDate: '2020-06-03T05:07:44.923Z',
                lastModifiedBy: null,
                lastModifiedDate: null
            },
            {
                uploadId: 'edc18248-9da0-402a-97fa-a6d0e614d9dd',
                errorCount: 0,
                status: 'Finalizado',
                actionType: 'Insertar',
                segmentName: 'Automation_iu38c',
                name: 'b5da9cf4-9247-43bd-b6e5-040722fbd0f4.xlsx',
                systemTypeId: 'EXCEL',
                recordsProcessed: 1,
                isParsed: true,
                createdBy: 'trueadmin',
                createdDate: '2020-06-03T05:05:52.803Z',
                lastModifiedBy: null,
                lastModifiedDate: null
            },
            {
                uploadId: '7212b1fb-8a0a-43b9-9b6b-cb46a7d7f996',
                errorCount: 0,
                status: 'Finalizado',
                actionType: 'Insertar',
                segmentName: 'Automation_iu38c',
                name: 'b5da9cf4-9247-43bd-b6e5-040722fbd0f4.xlsx',
                systemTypeId: 'EXCEL',
                recordsProcessed: 1,
                isParsed: true,
                createdBy: 'trueadmin',
                createdDate: '2020-06-03T05:01:29.397Z',
                lastModifiedBy: null,
                lastModifiedDate: null
            },
            {
                uploadId: '3cea6b8a-e455-4b63-8f1d-df771e09db78',
                errorCount: 0,
                status: 'Finalizado',
                actionType: 'Insertar',
                segmentName: 'Automation_iu38c',
                name: 'b5da9cf4-9247-43bd-b6e5-040722fbd0f4.xlsx',
                systemTypeId: 'EXCEL',
                recordsProcessed: 1,
                isParsed: true,
                createdBy: 'trueadmin',
                createdDate: '2020-06-03T04:59:41.687Z',
                lastModifiedBy: null,
                lastModifiedDate: null
            },
            {
                uploadId: 'b139f611-b428-4bf7-9e0c-54edd13963b2',
                errorCount: 1,
                status: 'Fallido',
                actionType: 'Insertar',
                segmentName: 'Automation_h1muc',
                name: 'b5da9cf4-9247-43bd-b6e5-040722fbd0f4.xlsx',
                systemTypeId: 'EXCEL',
                recordsProcessed: 1,
                isParsed: true,
                createdBy: 'trueadmin',
                createdDate: '2020-06-03T04:07:31.057Z',
                lastModifiedBy: null,
                lastModifiedDate: null
            },
            {
                uploadId: 'f8b456d4-7e95-43c8-b3e8-2faad3f857d9',
                errorCount: 1,
                status: 'Fallido',
                actionType: 'Insertar',
                segmentName: 'Automation_h1muc',
                name: 'b5da9cf4-9247-43bd-b6e5-040722fbd0f4.xlsx',
                systemTypeId: 'EXCEL',
                recordsProcessed: 1,
                isParsed: true,
                createdBy: 'trueadmin',
                createdDate: '2020-06-03T04:05:41.407Z',
                lastModifiedBy: null,
                lastModifiedDate: null
            },
            {
                uploadId: 'b8b0bfda-355e-4a5b-bd61-406231ece8fb',
                errorCount: 0,
                status: 'Finalizado',
                actionType: 'Insertar',
                segmentName: 'Automation_iu38c',
                name: 'TestData_InitialCutOff.xlsx',
                systemTypeId: 'EXCEL',
                recordsProcessed: 3,
                isParsed: true,
                createdBy: 'trueprofessional',
                createdDate: '2020-06-03T04:03:01.013Z',
                lastModifiedBy: null,
                lastModifiedDate: null
            },
            {
                uploadId: '9979c563-a990-4122-bf2c-e5db03019ade',
                errorCount: 1,
                status: 'Fallido',
                actionType: 'Insertar',
                segmentName: 'Automation_h1muc',
                name: 'ExcelOperationsRegistryExampleMVP2 (2) (1) (1).xlsx',
                systemTypeId: 'EXCEL',
                recordsProcessed: 1,
                isParsed: true,
                createdBy: 'trueadmin',
                createdDate: '2020-06-03T03:51:04.62Z',
                lastModifiedBy: null,
                lastModifiedDate: null
            },
            {
                uploadId: '34150605-a1b0-4d4c-857e-93d55178cc97',
                errorCount: 3,
                status: 'Procesando',
                actionType: 'Insertar',
                segmentName: 'Automation_h1muc',
                name: 'ExcelOperationsRegistryExampleMVP2 (2) (1) (1).xlsx',
                systemTypeId: 'EXCEL',
                recordsProcessed: 3,
                isParsed: true,
                createdBy: 'trueadmin',
                createdDate: '2020-06-03T03:41:00.467Z',
                lastModifiedBy: null,
                lastModifiedDate: null
            }

        ],
        refreshToggler: true,
        receiveDataToggler: false,
        pageFilters: {}
    }
};

const grid = {
    FileuploadGrid: dataGrid.FileuploadGrid,
    fileUploads: {
        filterValues: {
            initialDate: '05/02/2020',
            finalDate: '12/02/2020'
        }
    }
};
const notification = {};
const messages = {
    fileRegistration: {
        state: 'true',
        readAccessInfo: { isValid: true }
    }
};
const shared = {
    registrationActionTypes: {
        1: 'Insertar',
        2: 'Actualizar',
        3: 'Elimina',
        4: 'Reinyectar'
    },
    readAccessInfo: true
};

const flyout = [{ FileuploadGrid: true }];
const props = {
    onDownload: jest.fn(() => Promise.resolve()),
    viewErrorDetails: jest.fn(() => Promise.resolve()),
    initialize: jest.fn(),
    requestFileRegistrationReadAccess: jest.fn()
};

const reducers = {
    onEdit: jest.fn(() => Promise.resolve()),
    grid: jest.fn(() => grid),
    notification: jest.fn(() => notification),
    onClose: jest.fn(() => Promise.resolve()),
    messages: jest.fn(() => messages),
    shared: jest.fn(() => shared),
    flyout: jest.fn(() => flyout)
};

const store = createStore(combineReducers(reducers));

function mountWithRealStore() {
    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <FileUploadGrid name="FileuploadGrid" />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('fileUploadGrid', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('FileuploadGrid');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(9);
    });

    it('should render column headers for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(0).text()).toEqual('uploadFileId');
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(1).text()).toEqual('createdDate');
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(2).text()).toEqual('fileName');
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(3).text()).toEqual('actionType');
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(4).text()).toEqual('createdBy');
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(5).text()).toEqual('fileUploadStatus');
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(6).text()).toEqual('recordsProcessed');
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(7).text()).toEqual('fileRegistrationErrors');
    });

    it('should render column data for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(0).text()).toEqual('830e8066-cd4a-4b6a-9306-f3d420b70366');
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(1).text()).toEqual('2020-06-03T07:00:31Z');
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(2).text()).toEqual('b5da9cf4-9247-43bd-b6e5-040722fbd0f4.xlsx');
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(3).text()).toEqual('Insertar');
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(4).text()).toEqual('trueadmin');
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(5).text()).toEqual('Fallido');
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(6).text()).toEqual('1');
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(7).text()).toEqual('1');
    });

    it('should build column action buttons for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find('[id^="li_FileuploadGrid_viewError"]').at(0).find('[id^="lnk_FileuploadGrid_viewError"]').at(0).prop('disabled')).toEqual(false);
        expect(enzymeWrapper.find('[id^="lnk_FileuploadGrid_download"]').at(0).find('em').at(0).html()).toEqual('<em class="fas fa-file-download" aria-hidden="true"></em>');
    });

    it('should download', () => {
        blobService.getDownloadLink = jest.fn();
        blobService.initialize = jest.fn();
        utilities.isNullOrUndefined = jest.fn(() => false);
        utilities.toLowerCase = jest.fn(() => 'id');
        utilities.isIE = jest.fn(() => false);

        const newProps = {
            readAccessInfo: {
                accountName: 'AccountName',
                sasToken: 'SasToken',
                containerName: 'ContainerName'
            },
            viewErrorDetails: jest.fn(() => Promise.resolve()),
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            getSystemTypes: jest.fn()
        };

        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);

        enzymeWrapper.instance().onDownload(8);
        expect(blobService.getDownloadLink.mock.calls).toHaveLength(1);
        expect(utilities.isNullOrUndefined.mock.calls).toHaveLength(6);
        expect(blobService.initialize.mock.calls).toHaveLength(1);
    });

    it('correct url is called', () => {
        blobService.getDownloadLink = jest.fn();
        blobService.initialize = jest.fn();
        utilities.isNullOrUndefined = jest.fn(() => false);
        utilities.toLowerCase = jest.fn(() => 'id');
        utilities.isIE = jest.fn(() => true);

        const newProps = {
            readAccessInfo: {
                accountName: 'AccountName',
                sasToken: 'SasToken',
                containerName: 'ContainerName'
            },
            viewErrorDetails: jest.fn(() => Promise.resolve()),
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            getSystemTypes: jest.fn()
        };

        window.open = jest.fn();
        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);

        enzymeWrapper.instance().onDownload(8);
        // statementService.openStatementsReport(111);
        expect(window.open).toHaveBeenCalled();
    });

    it('ReInject is called', () => {
        const newProps = {
            openModal: jest.fn(),
            onReInject: jest.fn(),
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            getSystemTypes: jest.fn()
        };

        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);
        enzymeWrapper.instance().onReInject();
        // statementService.openStatementsReport(111);
        expect(newProps.openModal.mock.calls).toHaveLength(1);
    });

    it('Finalized is called', () => {
        const newProps = {
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            getSystemTypes: jest.fn()
        };
        const row = {
            status: 'Procesando'
        };

        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);
        expect(enzymeWrapper.instance().isFinalized(row)).toEqual(false);
    });

    it('Error Count is greater than 1', () => {
        const newProps = {
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            getSystemTypes: jest.fn()
        };
        const row = {
            errorCount: 1
        };

        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);
        expect(enzymeWrapper.instance().isEnabled(row)).toEqual(true);
    });

    it('should Refresh', () => {
        utilities.isNullOrUndefined = jest.fn(() => false);
        utilities.toLowerCase = jest.fn(() => 'id');

        const newProps = {
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            files: { find: jest.fn() },
            getSystemTypes: jest.fn()
        };

        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);

        enzymeWrapper.instance().shouldRefresh();
        expect(utilities.isNullOrUndefined.mock.calls).toHaveLength(6);
    });

    it('should return Processed Record length 1', () => {
        blobService.initialize = jest.fn();
        utilities.isNullOrUndefined = jest.fn(() => false);
        utilities.toLowerCase = jest.fn(() => 'id');

        const newProps = {
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            files: { find: jest.fn() },
            getSystemTypes: jest.fn()
        };
        const row = { original: { fileRegistrationTransactions: [{ statusTypeId: constants.StatusType.PROCESSED }] } };

        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);
        expect(enzymeWrapper.instance().getProcessedRecord(row)).toEqual(1);
    });


    it('should return empty Processed Record', () => {
        blobService.initialize = jest.fn();
        utilities.isNullOrUndefined = jest.fn(() => false);
        utilities.toLowerCase = jest.fn(() => 'id');

        const newProps = {
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            files: { find: jest.fn() },
            getSystemTypes: jest.fn()
        };
        const row = { original: { fileRegistrationTransactions: [{ statusTypeId: constants.StatusType.PROCESSED }] } };

        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);
        expect(enzymeWrapper.instance().getProcessedRecord(row)).toEqual(1);
    });

    it('should show ErrorDetails', () => {
        navigationService.navigateToModule = jest.fn();
        const newProps = {
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            files: { find: jest.fn() },
            getSystemTypes: jest.fn()
        };
        const error = { uploadId: 1 };

        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);
        enzymeWrapper.instance().viewErrorDetails(error);
        expect(navigationService.navigateToModule.mock.calls).toHaveLength(1);
    });

    it('should return filters', () => {
        utilities.isNullOrUndefined = jest.fn(() => false);
        const newProps = {
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            fileTypes: [{ systemTypeId: 1 }],
            getSystemTypes: jest.fn()
        };

        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);
        enzymeWrapper.instance().getColumns();
        expect(enzymeWrapper.instance().getColumns()).not.toBe(null);
    });

    it('should return filters for CONTRACT OR EVENT', () => {
        utilities.getValueOrDefault = jest.fn(() => 10);
        systemConfigService.getDefaultTransportFileUploadLastDays = jest.fn(() => 10);
        const newProps = {
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            fileTypes: [],
            systemType: constants.SystemType.CONTRACT,
            getSystemTypes: jest.fn()
        };

        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);
        enzymeWrapper.instance().getColumns();
        expect(enzymeWrapper.instance().getColumns()).not.toBe(null);
    });

    it('should return filters for EXCEL', () => {
        utilities.getValueOrDefault = jest.fn(() => 10);
        systemConfigService.getDefaultTransportFileUploadLastDays = jest.fn(() => 10);
        const newProps = {
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            fileTypes: [],
            systemType: constants.SystemType.EXCEL,
            getSystemTypes: jest.fn()
        };

        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);
        enzymeWrapper.instance().getColumns();
        expect(enzymeWrapper.instance().getColumns()).not.toBe(null);
    });

    it('should filter the data for value null', () => {
        utilities.getValueOrDefault = jest.fn(() => 10);
        dateService.formatFromDate = jest.fn(() => '12/03/2020');
        systemConfigService.getDefaultTransportFileUploadLastDays = jest.fn(() => 10);
        const newProps = {
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            fileTypes: [],
            systemType: constants.SystemType.EXCEL,
            updateGridFilter: jest.fn(),
            clearGridFilter: jest.fn(),
            change: jest.fn(),
            applyFilter: jest.fn(),
            getSystemTypes: jest.fn()
        };

        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);
        enzymeWrapper.instance().onFiltered(null);
        expect(newProps.updateGridFilter.mock.calls).toHaveLength(1);
        expect(newProps.change.mock.calls).toHaveLength(2);
        expect(newProps.applyFilter.mock.calls).toHaveLength(1);
    });

    it('should filter the data for value not null', () => {
        utilities.getValueOrDefault = jest.fn(() => 10);
        dateService.formatFromDate = jest.fn(() => '12/03/2020');
        dateService.parseToFilterString = jest.fn(() => '12/03/2020');
        dateService.format = jest.fn(() => '12/03/2020');
        systemConfigService.getDefaultTransportFileUploadLastDays = jest.fn(() => 10);
        utilities.isNullOrUndefined = jest.fn(() => true);
        const newProps = {
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            fileTypes: [],
            systemType: constants.SystemType.CONTRACT,
            updateGridFilter: jest.fn(),
            clearGridFilter: jest.fn(),
            change: jest.fn(),
            applyFilter: jest.fn(),
            getSystemTypes: jest.fn()
        };

        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);
        enzymeWrapper.instance().onFiltered('abc');
        expect(newProps.updateGridFilter.mock.calls).toHaveLength(0);
        expect(newProps.clearGridFilter.mock.calls).toHaveLength(1);
        expect(newProps.change.mock.calls).toHaveLength(2);
        expect(newProps.applyFilter.mock.calls).toHaveLength(1);
    });

    it('should unmount component', () => {
        const newProps = {
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            getSystemTypes: jest.fn()
        };

        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);
        enzymeWrapper.unmount();
        expect(newProps.setModuleName.mock.calls).toHaveLength(2);
    });

    it('should return null', () => {
        utilities.isNullOrUndefined = jest.fn(() => false);
        const newProps = {
            initialize: jest.fn(),
            requestFileRegistrationReadAccess: jest.fn(),
            setModuleName: jest.fn(),
            fileTypes: undefined,
            getSystemTypes: jest.fn()
        };

        const enzymeWrapper = shallow(<FileUploadGridComponent {...newProps} />);
        enzymeWrapper.instance().getColumns();
    });
});
