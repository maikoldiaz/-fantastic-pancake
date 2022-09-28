import setup from '../../setup.js';
import { createStore, combineReducers } from 'redux';
import { mount, shallow } from 'enzyme';
import { Provider } from 'react-redux';
import React from 'react';
import { apiService } from '../../../../common/services/apiService.js';
import Grid from '../../../../common/components/grid/grid.jsx';
import IntegrationManagementGrid, { IntegrationManagementGrid as IntegrationManagementGridComponent } from '../../../../modules/administration/integrationManagement/components/integrationManagementGrid.jsx';
import blobService from '../../../../common/services/blobService.js';

const grid = {
    integrationManagementGrid: {
        config: {
            apiUrl: apiService.integration.getIntegrationManagement(),
        }
    }
};

const containers = {
    true: {},
    ownership: {},
    delta: {},
}

function mountWithRealStore(customProps) {
    const reducers = {
        shared: jest.fn(() => ({})),
        grid: jest.fn(() => grid),
    };

    const store = createStore(combineReducers(reducers));
    const props = customProps || {};

    const dispatchSpy = jest.spyOn(store, 'dispatch');

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <IntegrationManagementGrid />
    </Provider>);

    return { store, enzymeWrapper, props, dispatchSpy };
}

describe('Admin IntegrationManagement IntegrationManagementGrid', () => {
    beforeAll(() => {
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call componentDidMount successfully and call requestFileReadAccess', () => {
        const props = { requestFileReadAccess: jest.fn(), download: true };
        shallow(<IntegrationManagementGridComponent {...props} />);
        expect(props.requestFileReadAccess.mock.calls).toHaveLength(1);
    });

    it('should call componentDidMount successfully and don\'t call requestFileReadAccess', () => {
        const props = { requestFileReadAccess: jest.fn(), download: false };
        shallow(<IntegrationManagementGridComponent {...props} />);
        expect(props.requestFileReadAccess.mock.calls).toHaveLength(0);
    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual(
            'integrationManagementGrid'
        );
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(5);
    });

    it('should do nothing when call function onDownload and readAccessInfo is null', () => {
        blobService.initialize = jest.fn();
        const props = {
            readAccessInfo: { true: null },
            requestFileReadAccess: jest.fn(),
            download: true
        };
        const wrapper = shallow(<IntegrationManagementGridComponent {...props} />);
        wrapper.instance().onDownload({sourceTypeId: 'PURCHASE'});
        expect(blobService.initialize).not.toHaveBeenCalled();
    });

    it('should download file succesfully when promise return resolve', () => {
        blobService.initialize = jest.fn();
        blobService.downloadSecureFile = jest.fn().mockResolvedValue(true);
        const props = {
            readAccessInfo: containers,
            requestFileReadAccess: jest.fn(),
            download: true
        };
        const wrapper = shallow(<IntegrationManagementGridComponent {...props} />);
        wrapper.instance().onDownload({sourceTypeId: 'OPERATIVEDELTA'});
        expect(blobService.initialize).toHaveBeenCalled();
        expect(blobService.downloadSecureFile).toHaveBeenCalled();
        return expect(blobService.downloadSecureFile.mock.results[0].value).resolves.toEqual(true);
    });

    it('should see modal error when promise return reject', async () => {
        blobService.initialize = jest.fn();
        blobService.downloadSecureFile = jest.fn().mockRejectedValue({ message: 'error'});
        const props = {
            readAccessInfo: containers,
            requestFileReadAccess: jest.fn(),
            download: true,
            showTechnicalError: jest.fn()
        };
        const wrapper = shallow(<IntegrationManagementGridComponent {...props} />);
        wrapper.instance().onDownload({sourceTypeId: 'OWNERSHIP'});
        expect(blobService.initialize).toHaveBeenCalled();
        expect(blobService.downloadSecureFile).toHaveBeenCalled();
        await expect(blobService.downloadSecureFile.mock.results[0].value).rejects.toEqual({ message: 'error'});
        expect(props.showTechnicalError).toHaveBeenCalled();
    });

});
