import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { dateService } from '../../../../../common/services/dateService';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import SettingsAudit, { SettingsAudit as SettingsAuditComponent } from '../../../../../modules/report/settingsAudit/components/report.jsx';
import { navigationService } from '../../../../../common/services/navigationService';
import { toPbiReport } from '../../../../../common/components/reports/pbiReport.jsx';
import { constants } from '../../../../../common/services/constants';
import * as settingsAuditReportFilterBuilder from '../../../../../modules/report/settingsAudit/filterBuilder';

const initialValues = {
    settingsAuditReport: {
        initialDate: dateService.now(),
        finalDate: dateService.now()
    }
};
const sharedInitialState = {
    categoryElements: [],
    allCategories: []
};

const nodeFilter = {};

function mountWithRealStore() {
    const reducers = {
        formValues: jest.fn(() => initialValues.settingsAuditReport),
        form: formReducer,
        shared: jest.fn(() => sharedInitialState),
        report: jest.fn(() => initialValues),
        nodeFilter: jest.fn(() => nodeFilter)
    };

    const store = createStore(combineReducers(reducers));

    const props = {
        config: jest.fn(() => config),
        onSubmit: jest.fn()
    };

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <SettingsAudit initialValues={initialValues} />
    </Provider>);

    return { store, enzymeWrapper };
}

describe('Settings Audit Report', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.navigateTo = jest.fn();
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should not mount categories field for balance control chart ', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_nodeFilter_category')).toBe(false);
    });

    it('should return filters for settings audit report',
        async () => {
            const values = {
                initialDate: '12/09/2019',
                finalDate: '18/09/2019',
                reportType: constants.Report.SettingsAuditReport
            };
            const result = settingsAuditReportFilterBuilder.settingsAuditReportFilterBuilder.build(values);
            await expect(result.length).toBe(1);
            await expect(result[0].target.table).toBe('DimDate');
            await expect(result[0].target.column).toBe('Date');
        });

    it('should mount the component returned by toPbiReport',
        async () => {
            const props = {
                initialDate: '12/09/2019',
                finalDate: '18/09/2019',
                reportType: constants.Report.SettingsAuditReport
            };
            const reducers = {
                formValues: jest.fn(() => initialValues.settingsAuditReport),
                form: formReducer,
                shared: jest.fn(() => sharedInitialState),
                report: jest.fn(() => initialValues),
                nodeFilter: jest.fn(() => nodeFilter)
            };
            const store = createStore(combineReducers(reducers));
            const pbiFilters = settingsAuditReportFilterBuilder.settingsAuditReportFilterBuilder.build(props);
            const ReportComponent = toPbiReport(props.reportType, pbiFilters);
            const wrapper = shallow(<Provider store={store} {...props}>
                <ReportComponent {...props} />
            </Provider>);
            expect(wrapper).toHaveLength(1);
        });

    it('should navigate back to manage module', () => {
        const navigationServiceMock = navigationService.navigateTo = jest.fn(()=> 'manage');
        const props = {};
        const wrapper = shallow(<SettingsAuditComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { reportToggler: false }));
        expect(navigationServiceMock.mock.calls).toHaveLength(2);
    });

    it('should mount settings audit returned by toPbiReport',
        async () => {
            const props = {
                initialDate: '12/09/2019',
                finalDate: '18/09/2019',
                reportType: constants.Report.SettingsAuditReport,
                filters: {
                    initialDate: '12/09/2019',
                    finalDate: '18/09/2019',
                    reportType: constants.Report.SettingsAuditReport
                }
            };
            const result = settingsAuditReportFilterBuilder.settingsAuditReportFilterBuilder.build(props.filters);
            const ReportComponent = toPbiReport(props.reportType, result);
            const wrapper = shallow(<SettingsAuditComponent {...props} />);
            expect(wrapper).toHaveLength(1);
        });

    it('should call back actions on click of buttons', () => {
        const props = {
            config: jest.fn(() => config),
            onSubmit: jest.fn(),
            onReturn: jest.fn(),
            setBackNavigation: jest.fn(),
            initialDate: '12/09/2019',
            finalDate: '18/09/2019',
            reportType: constants.Report.SettingsAuditReport,
            filters: {
                initialDate: '12/09/2019',
                finalDate: '18/09/2019',
                reportType: constants.Report.SettingsAuditReport
            }
        };
        const enzymeWrapper = shallow(<SettingsAuditComponent {...props} />);
        enzymeWrapper.instance().onReturn();
        expect(props.setBackNavigation.mock.calls).toHaveLength(1);
    });
});
