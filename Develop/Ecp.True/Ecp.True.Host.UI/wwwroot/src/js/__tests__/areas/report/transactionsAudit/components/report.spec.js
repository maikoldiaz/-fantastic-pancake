import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { dateService } from '../../../../../common/services/dateService';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import TransactionsAudit, { TransactionsAudit as TransactionsAuditComponent } from '../../../../../modules/report/transactionsAudit/components/report.jsx';
import { navigationService } from '../../../../../common/services/navigationService';
import { toPbiReport } from '../../../../../common/components/reports/pbiReport.jsx';
import { constants } from '../../../../../common/services/constants';
import * as transactionsAuditReportFilterBuilder from '../../../../../modules/report/transactionsAudit/filterBuilder';

const initialValues = {
    transactionsAudit: {
        element: {},
        initialDate: dateService.now(),
        finalDate: dateService.now(),
        reportType: constants.Report.MovementAuditReport
    }
};
const sharedInitialState = {
    categoryElements: [],
    allCategories: []
};

const nodeFilter = {};

function mountWithRealStore() {
    const reducers = {
        formValues: jest.fn(() => initialValues.transactionsAudit),
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
        <TransactionsAudit initialValues={initialValues} />
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

    it('should mount the component returned by toPbiReport',
        async () => {
            const props = {
                element: {},
                initialDate: dateService.now(),
                finalDate: dateService.now(),
                reportType: constants.Report.MovementAuditReport
            };
            const reducers = {
                formValues: jest.fn(() => initialValues.transactionsAudit),
                form: formReducer,
                shared: jest.fn(() => sharedInitialState),
                report: jest.fn(() => initialValues),
                nodeFilter: jest.fn(() => nodeFilter)
            };
            const store = createStore(combineReducers(reducers));
            const pbiFilters = transactionsAuditReportFilterBuilder.transactionsAuditReportFilterBuilder.build(props);
            const ReportComponent = toPbiReport(props.reportType, pbiFilters);
            const wrapper = shallow(<Provider store={store} {...props}>
                <ReportComponent {...props} />
            </Provider>);
            expect(wrapper).toHaveLength(1);
        });

    it('should navigate back to manage module', () => {
        const navigationServiceMock = navigationService.navigateTo = jest.fn(()=> 'manage');
        const props = {};
        const wrapper = shallow(<TransactionsAuditComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { reportToggler: false }));
        expect(navigationServiceMock.mock.calls).toHaveLength(2);
    });

    it('should mount component returned by toPbiReport',
        async () => {
            const props = {
                element: {
                    elementId: 1
                },
                initialDate: dateService.now(),
                finalDate: dateService.now(),
                reportType: constants.Report.transactionsAuditReport,
                filters: {
                    initialDate: dateService.now(),
                    finalDate: dateService.now(),
                    reportType: constants.Report.transactionsAuditReport
                }
            };
            const pbiFilters = transactionsAuditReportFilterBuilder.transactionsAuditReportFilterBuilder.build(props.filters);
            const ReportComponent = toPbiReport(props.reportType, pbiFilters);
            const wrapper = shallow(<TransactionsAuditComponent {...props} />);
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
            reportType: constants.Report.TransactionsAuditReport,
            filters: {
                initialDate: '12/09/2019',
                finalDate: '18/09/2019',
                reportType: constants.Report.TransactionsAuditReport
            }
        };
        const enzymeWrapper = shallow(<TransactionsAuditComponent {...props} />);
        enzymeWrapper.instance().onReturn();
        expect(props.setBackNavigation.mock.calls).toHaveLength(1);
    });
});
