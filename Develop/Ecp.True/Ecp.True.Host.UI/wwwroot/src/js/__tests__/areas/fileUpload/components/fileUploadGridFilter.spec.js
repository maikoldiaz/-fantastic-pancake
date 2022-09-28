import setup from '../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import FileuploadGridFilter, { FileuploadGridFilter as FileuploadGridFilterComponent } from '../../../../modules/transportBalance/fileUploads/components/fileUploadGridFilter.jsx';
import { dateService } from '../../../../common/services/dateService';
import { systemConfigService } from '../../../../common/services/systemConfigService';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';
import { optionService } from '../../../../common/services/optionService.js';
import FlyoutFooter from '../../../../common/components/footer/flyoutFooter.jsx';

const shared = {
    fileTypes: [
        {
            createdBy: 'System',
            createdDate: '2020-02-15T02:40:56.307Z',
            isFileType: true,
            lastModifiedBy: null,
            lastModifiedDate: null,
            name: 'Pedidos',
            systemTypeId: 4
        },
        {
            createdBy: 'System',
            createdDate: '2020-02-15T02:40:56.307Z',
            isFileType: true,
            lastModifiedBy: null,
            lastModifiedDate: null,
            name: 'Planeación, Programación y Acuerdos',
            systemTypeId: 5
        }
    ],
    users: [
        {
            createdBy: 'trueadmin"',
            createdDate: '2020-03-31T05:42:01.3',
            email: 'trueadmin@ecopetrol.com.co',
            name: 'trueadmin',
            userId: 1
        }
    ]
};

const grid = {
    fileUploads: {
        filterValues: {
            initialDate: '05/02/2020',
            finalDate: '12/02/2020'
        }
    }
};
const flyout = {};
flyout.FileuploadGridFilter = true;

const initialState = {
    messages: {
        fileRegistration: {
            browseFile: {},
            gridFilter: false,
            initialValues: { initialDate: '02-jun-20', finalDate: '03-jun-20', username: null, state: null, action: null },
            isValidSelection: false,
            reInjectFileInfo: {},
            receiveAccessInfoToggler: false,
            receiveStatusToggler: false,
            reinjectActionTypeKey: '4',
            resetFormToggler: false,
            selectedActionType: 0,
            validationResult: { success: true }
        }
    }
};

const reducers = {
    messages: jest.fn(() => initialState.messages),
    shared: jest.fn(() => shared),
    flyout: jest.fn(() => flyout),
    grid: jest.fn(() => grid),
    name: 'FileuploadGridFilter'
};
const props = {
    closeFlyout: jest.fn(() => Promise.resolve),
    resetForm: jest.fn(() => Promise.resolve),
    saveUploadFileFilter: jest.fn(() => Promise.resolve)
};
function mountWithRealStore() {
    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <FileuploadGridFilter name="FileuploadGridFilter" {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('fileUploadGrid', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });
});

it('should mount successfully', () => {
    const { enzymeWrapper } = mountWithRealStore(initialState);
    expect(enzymeWrapper).toHaveLength(1);
});

it('should load cancel, restart and Aplicar filtros buttons', () => {
    const { enzymeWrapper } = mountWithRealStore(initialState);
    expect(enzymeWrapper.find('#btn_uploadFileFilter_cancel').text()).toEqual('Cancelar');
    expect(enzymeWrapper.find('#btn_uploadFileFilter_reset').text()).toEqual('Reiniciar');
    expect(enzymeWrapper.find(FlyoutFooter).find('#btn_fileUploadGridFilter_submit').text()).toEqual('Aplicar filtros');
});

it('should load fields', () => {
    const { enzymeWrapper } = mountWithRealStore(initialState);
    optionService.getCutoffStateTypes = jest.fn(() => [{ label: 'finalized', value: 'Finalizado' }, { label: 'processing', value: 'Procesando' }, { label: 'failed', value: 'Fallido' }]);
    optionService.getExcelActionTypes = jest.fn(() => [{ label: 'insert', value: 'Insertar' }, { label: 'update', value: 'Actualizar' }, { label: 'delete', value: 'Eliminar' }, { label: 'reInject', value: 'Re Inyectar' }]);

    expect(enzymeWrapper.find('#dt_uploadFileFilter_initialDate').at(0).prop('name')).toEqual('initialDate');
    expect(enzymeWrapper.find('#dt_uploadFileFilter_finalDate').at(0).prop('name')).toEqual('finalDate');
    expect(enzymeWrapper.find('#dd_uploadFileFilter_username')).not.toBe(null);
    expect(enzymeWrapper.find('#dd_uploadFileFilter_state')).not.toBe(null);
    expect(enzymeWrapper.find('#dd_uploadFileFilter_action')).not.toBe(null);
    expect(enzymeWrapper.find('#dd_uploadFileFilter_type')).not.toBe(null);
    expect(enzymeWrapper.find('#dd_uploadFileFilter_state_sel').at(0).prop('aria-autocomplete')).toEqual('list');
    expect(enzymeWrapper.find('#dd_uploadFileFilter_action_sel').at(0).prop('aria-autocomplete')).toEqual('list');
});

it('should reset form filters', () => {
    const newProps = {
        closeFlyout: jest.fn(() => Promise.resolve),
        resetForm: jest.fn(() => Promise.resolve),
        saveUploadFileFilter: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn(() => Promise.resolve),
        getUsers: jest.fn(() => Promise.resolve),
        getSystemTypes: jest.fn()
    };

    const enzymeWrapper = shallow(<FileuploadGridFilterComponent {...newProps} />);

    enzymeWrapper.instance().onReset(false);
    expect(newProps.resetForm.mock.calls).toHaveLength(1);
});

it('should reset form', () => {
    const newProps = {
        closeFlyout: jest.fn(() => Promise.resolve),
        resetForm: jest.fn(() => Promise.resolve),
        saveUploadFileFilter: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn(() => Promise.resolve),
        getUsers: jest.fn(() => Promise.resolve),
        getSystemTypes: jest.fn()
    };

    const enzymeWrapper = shallow(<FileuploadGridFilterComponent {...newProps} />);
    enzymeWrapper.instance().onReset(false);

    expect(newProps.resetForm.mock.calls).toHaveLength(1);
});

it('should close flyout', () => {
    const newProps = {
        closeFlyout: jest.fn(() => Promise.resolve),
        resetForm: jest.fn(() => Promise.resolve),
        saveUploadFileFilter: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn(() => Promise.resolve),
        getUsers: jest.fn(() => Promise.resolve),
        getSystemTypes: jest.fn()
    };

    const enzymeWrapper = shallow(<FileuploadGridFilterComponent {...newProps} />);
    enzymeWrapper.instance().onReset(true);
    expect(newProps.closeFlyout.mock.calls).toHaveLength(1);
});


it('should submit form', () => {
    const newProps = {
        closeFlyout: jest.fn(() => Promise.resolve),
        resetForm: jest.fn(() => Promise.resolve),
        saveUploadFileFilter: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn(() => Promise.resolve),
        getUsers: jest.fn(() => Promise.resolve),
        getSystemTypes: jest.fn()
    };

    systemConfigService.getDefaultTransportFileUploadDateRange = jest.fn(() => 10);
    dateService.parseToDate = jest.fn();
    dateService.getDiff = jest.fn();
    const formValues = {
        initialDate: dateService.now(),
        finalDate: dateService.add(dateService.now(), 1, 'day'),
        saveUploadFileFilter: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn(() => Promise.resolve),
        getUsers: jest.fn(() => Promise.resolve),
        getSystemTypes: jest.fn()
    };

    const enzymeWrapper = shallow(<FileuploadGridFilterComponent {...newProps} />);
    enzymeWrapper.instance().onSubmit(formValues);
    expect(newProps.saveUploadFileFilter.mock.calls).toHaveLength(1);
});

it('should throw inconsistent date error', () => {
    const newProps = {
        handleSubmit: jest.fn(),
        saveUploadFileFilter: jest.fn(() => Promise.resolve),
        getUsers: jest.fn(() => Promise.resolve),
        closeFlyout: jest.fn(),
        showError: jest.fn(() => Promise.resolve),
        getSystemTypes: jest.fn()
    };

    resourceProvider.read = jest.fn(() => 'inconsistentDates');
    systemConfigService.getDefaultTransportFileUploadDateRange = jest.fn(() => 10);
    dateService.parseToDate = jest.fn();
    dateService.getDiff = jest.fn();
    const formValues = {
        initialDate: dateService.add(dateService.now(), 20, 'day').toDate(),
        finalDate: dateService.now()
    };
    const enzymeWrapper = shallow(<FileuploadGridFilterComponent {...newProps} />);
    enzymeWrapper.setProps(Object.assign({}, props));
    enzymeWrapper.instance().onSubmit(formValues);
});

it('should throw date range error', () => {
    const newProps = {
        handleSubmit: jest.fn(),
        saveUploadFileFilter: jest.fn(() => Promise.resolve),
        getUsers: jest.fn(() => Promise.resolve),
        closeFlyout: jest.fn(),
        showError: jest.fn(() => Promise.resolve),
        systemType: constants.SystemType.CONTRACT,
        getSystemTypes: jest.fn()
    };
    const formValues = {
        initialDate: dateService.now().toDate(),
        finalDate: dateService.add(dateService.now(), 5, 'day').toDate()
    };

    dateService.parseToDate = jest.fn().mockReturnValueOnce(formValues.initialDate).mockReturnValueOnce(formValues.finalDate);
    dateService.getDiff = jest.fn(() => 5);

    resourceProvider.readFormat = jest.fn(() => 'invalidContractsAndEventsDateRange');
    systemConfigService.getDefaultTransportFileUploadDateRange = jest.fn(() => 2);

    const enzymeWrapper = shallow(<FileuploadGridFilterComponent {...newProps} />);
    expect(newProps.getUsers.mock.calls).toHaveLength(1);
});


