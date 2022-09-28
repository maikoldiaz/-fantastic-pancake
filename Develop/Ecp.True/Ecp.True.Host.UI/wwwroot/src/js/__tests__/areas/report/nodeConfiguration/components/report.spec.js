import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { dateService } from '../../../../../common/services/dateService';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import NodeConfiguration, { NodeConfiguration as NodeConfigurationComponent } from '../../../../../modules/report/nodeConfiguration/components/report.jsx';
import { navigationService } from '../../../../../common/services/navigationService';
import { toPbiReport } from '../../../../../common/components/reports/pbiReport.jsx';
import { constants } from '../../../../../common/services/constants';
import * as nodeConfigurationReportFilterBuilder from '../../../../../modules/report/nodeConfiguration/filterBuilder';

const initialValues = {
    nodeConfigurationReport: {
        selectedCategory: {},
        selectedElement: {},
        searchedNodes: []
    }
};
const sharedInitialState = {
    categoryElements: [],
    allCategories: []
};

const nodeFilter = {
    selectedCategory: '',
    selectedElement: '',
    selectedNode: ''
};

function mountWithRealStore() {
    const reducers = {
        formValues: jest.fn(() => initialValues.nodeConfigurationReport),
        form: formReducer,
        shared: jest.fn(() => sharedInitialState),
        report: jest.fn(() => initialValues),
        nodeFilter: jest.fn(() => nodeFilter)
    };

    const store = createStore(combineReducers(reducers));

    const props = {
        config: jest.fn(() => config)
    };

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <NodeConfiguration initialValues={initialValues} />
    </Provider>);


    return { store, enzymeWrapper };
}

describe('Node Configuration Report', () => {
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
                filters: {
                    elementName: 'Estación',
                    nodeName: 'SAN FERNANDO',
                    reportType: constants.Report.NodeConfigurationReport
                }
            };
            const reducers = {
                formValues: jest.fn(() => initialValues.nodeConfigurationReport),
                form: formReducer,
                shared: jest.fn(() => sharedInitialState),
                report: jest.fn(() => initialValues),
                nodeFilter: jest.fn(() => nodeFilter)
            };
            const store = createStore(combineReducers(reducers));
            const pbiFilters = nodeConfigurationReportFilterBuilder.nodeConfigurationFilterBuilder.build(props.filters);
            const ReportComponent = toPbiReport(props.reportType, pbiFilters);
            const wrapper = shallow(<Provider store={store} {...props}>
                <ReportComponent {...props} />
            </Provider>);
            expect(wrapper).toHaveLength(1);
        });

    it('should navigate back to manage module', () => {
        const navigationServiceMock = navigationService.navigateTo = jest.fn(()=> 'manage');
        const props = {};
        const wrapper = shallow(<NodeConfigurationComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { reportToggler: false }));
        expect(navigationServiceMock.mock.calls).toHaveLength(2);
    });

    it('should mount component returned by toPbiReport',
        async () => {
            const props = {
                filters: {
                    elementName: 'Estación',
                    nodeName: 'SAN FERNANDO',
                    reportType: constants.Report.NodeConfigurationReport
                }
            };
            const pbiFilters = nodeConfigurationReportFilterBuilder.nodeConfigurationFilterBuilder.build(props.filters);
            const ReportComponent = toPbiReport(props.reportType, pbiFilters);
            const wrapper = shallow(<NodeConfigurationComponent {...props} />);
            expect(wrapper).toHaveLength(1);
        });
});
