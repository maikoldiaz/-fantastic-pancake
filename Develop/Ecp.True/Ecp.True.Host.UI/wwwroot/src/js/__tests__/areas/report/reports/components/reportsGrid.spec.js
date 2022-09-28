import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider.js';
import { httpService } from '../../../../../common/services/httpService.js';
import { scenarioService } from '../../../../../common/services/scenarioService';
import { navigationService } from '../../../../../common/services/navigationService';
import ReportsGrid, { ReportsGrid as ReportsGridComponent } from '../../../../../modules/report/reports/components/reportsGrid.jsx';
import Grid from '../../../../../common/components/grid/grid.jsx';

const dataGrid = {
    reportExecutions: {
        config: {
            apiUrl: ''
        },
        items: [
            {
                "executionId": 21,
                "nodeId": 42281,
                "nodeName": "testnode",
                "categoryId": 2,
                "segment": "segment",
                "elementId": 176797,
                "system": "system",
                "startDate": "2020-06-01T00:00:00Z",
                "endDate": "2020-06-30T00:00:00Z",
                "reportType": "OfficialInitialBalance",
                "statusType": 'FAILED',
                "scenarioType": 'Officer',
                "createdBy": "trueadmin",
                "reportName": "testreport",
                "createdDate": "2020-07-31T05:45:58.483Z"
            },
            {
                "executionId": 31,
                "nodeId": 42291,
                "nodeName": "testnode",
                "categoryId": 2,
                "segment": "segment",
                "elementId": 176754,
                "system": "system",
                "startDate": "2020-06-01T00:00:00Z",
                "endDate": "2020-06-30T00:00:00Z",
                "reportType": "SapBalance",
                "statusType": 'Finalizado',
                "scenarioType": 'Officer',
                "createdBy": "trueadmin",
                "reportName": "SapBalance",
                "createdDate": "2020-07-31T05:45:58.483Z"
            },
            {
                "executionId": 32,
                "nodeId": 42292,
                "nodeName": "testnode",
                "categoryId": 2,
                "segment": "segment",
                "elementId": 176754,
                "system": "system",
                "startDate": "2020-06-01T00:00:00Z",
                "endDate": "2020-06-30T00:00:00Z",
                "reportType": "UserRolesAndPermissions",
                "statusType": 'Finalizado',
                "scenarioType": 'Officer',
                "createdBy": "trueadmin",
                "reportName": "UserRolesAndPermissions",
                "createdDate": "2020-07-31T05:45:58.483Z"
            },
        ],
        refreshToggler: true,
        receiveDataToggler: false,
        pageFilters: {}
    }
};

const grid = {
    reportExecutions: dataGrid.reportExecutions,
};

const props = {
    onDownload: jest.fn(() => Promise.resolve()),
};

const reducers = {
    grid: jest.fn(() => grid),
};

const store = createStore(combineReducers(reducers));

function mountWithRealStore(componentProps = {}) {
    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <ReportsGrid name="reportExecutions" {...componentProps} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('ReportsGrid', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        scenarioService.getReportTypes = jest.fn(() => ['ReportName']);
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('reportExecutions');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(9);
    });

    it('should render column headers for the grid', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(0).text()).toEqual('reportName');
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(1).text()).toEqual('segment');
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(2).text()).toEqual('system');
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(3).text()).toEqual('nodeName');
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(4).text()).toEqual('initialDate');
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(5).text()).toEqual('finalDate');
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(6).text()).toEqual('executionDateOC');
        expect(enzymeWrapper.find('.rt-thead').at(0).find('.ep-tooltip__trigger').at(7).text()).toEqual('fileUploadStatus');
    });

    it('should render column data for the grid', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(0).text()).toEqual('testreport');
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(1).text()).toEqual('segment');
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(2).text()).toEqual('system');
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(3).text()).toEqual('testnode');
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(4).text()).toEqual('2020-06-01T00:00:00Z');
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(5).text()).toEqual('2020-06-30T00:00:00Z');
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(6).text()).toEqual('31-Jul-20 05:45');
        expect(enzymeWrapper.find('.rt-tr-group').at(0).find('.rt-td').at(7).text()).toEqual('FAILED');
    });

    it('should redirect to sendToSap report', () => {
        navigationService.navigateToModule = jest.fn();
        const componentProps = {
            generalReportTypes: ['ReportName']
        };
        const { enzymeWrapper } = mountWithRealStore(componentProps);
        enzymeWrapper.find('[id^="lnk_reportExecutions_download_31"]').at(0).simulate('click');
        
        expect(navigationService.navigateToModule.mock.calls).toHaveLength(1);
    });

    it('should redirect to userRolesAndPermissions report', () => {
        navigationService.navigateToModule = jest.fn();
        const componentProps = {
            generalReportTypes: ['ReportName']
        };
        const { enzymeWrapper } = mountWithRealStore(componentProps);
        enzymeWrapper.find('[id^="lnk_reportExecutions_download_32"]').at(0).simulate('click');
        
        expect(navigationService.navigateToModule.mock.calls).toHaveLength(1);
    });
});
