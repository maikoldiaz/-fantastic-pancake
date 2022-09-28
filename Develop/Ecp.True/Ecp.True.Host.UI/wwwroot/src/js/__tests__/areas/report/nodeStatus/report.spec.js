import setup from '../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { dateService } from '../../../../common/services/dateService';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import NodeStatus, { NodeStatus as NodeStatusComponent } from '../../../../modules/report/nodeStatus/components/report.jsx';
import { navigationService } from '../../../../common/services/navigationService';
import { toPbiReport } from '../../../../common/components/reports/pbiReport.jsx';
import { constants } from '../../../../common/services/constants';
import * as nodeStatusReportFilterBuilder from '../../../../modules/report/nodeStatus/filterBuilder';

const initialValues = {
    nodeStatusReport: {
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

const nodeStatusConfig = {
    category: {
        hidden: true,
        label: resourceProvider.read('category')
    },
    categoryElement: {
        label: resourceProvider.read('segment'),
        filterCategoryElementsItem: jest.fn()
    },
    node: {
        hidden: true
    },
    initialDate: {},
    finalDate: {
        allowAfterNow: false
    },
    reportType: {
        hidden: true
    },
    getTicket: jest.fn(),
    submitText: resourceProvider.read('viewReport'),
    parentPage: 'nodeStatusReport',
    getStartDateProps: jest.fn(),
    getEndDateProps: jest.fn(),
    validateDateRange: jest.fn(),
    onSubmitFilter: jest.fn(),
    requiredMessage: resourceProvider.read('CONTROLS_REQUIREDVALIDATION'),
    getReportRequest: jest.fn()
};

function mountWithRealStore() {
    const reducers = {
        formValues: jest.fn(() => initialValues.nodeStatusReport),
        form: formReducer,
        shared: jest.fn(() => sharedInitialState),
        report: jest.fn(() => initialValues),
        nodeFilter: jest.fn(() => nodeFilter)
    };

    const store = createStore(combineReducers(reducers));

    const props = {
        config: nodeStatusConfig
    };

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <NodeStatus initialValues={initialValues} />
    </Provider>);


    return { store, enzymeWrapper };
}

describe('Node Status Report', () => {
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
                    category: {
                        hidden: true,
                        label: resourceProvider.read('category')
                    },
                    categoryElement: {
                        label: resourceProvider.read('segment'),
                        filterCategoryElementsItem: jest.fn()
                    },
                    node: {
                        hidden: true
                    },
                    initialDate: {},
                    finalDate: {
                        allowAfterNow: false
                    },
                    reportType: {
                        hidden: true
                    }
                }
            };
            const reducers = {
                formValues: jest.fn(() => initialValues.nodeStatusReport),
                form: formReducer,
                shared: jest.fn(() => sharedInitialState),
                report: jest.fn(() => initialValues),
                nodeFilter: jest.fn(() => nodeFilter)
            };
            const store = createStore(combineReducers(reducers));
            const pbiFilters = nodeStatusReportFilterBuilder.nodeStatusFilterBuilder.build(props.filters);
            const ReportComponent = toPbiReport(props.reportType, pbiFilters);
            const wrapper = shallow(<Provider store={store} {...props}>
                <ReportComponent {...props} />
            </Provider>);
            expect(wrapper).toHaveLength(1);
        });

    it('should navigate back to manage module', () => {
        const navigationServiceMock = navigationService.navigateTo = jest.fn(()=> 'manage');
        const props = {};
        const wrapper = shallow(<NodeStatusComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { reportToggler: false }));
        expect(navigationServiceMock.mock.calls).toHaveLength(2);
    });

    it('should mount component returned by toPbiReport',
        async () => {
            const props = {
                filters: {
                    category: {
                        hidden: true,
                        label: resourceProvider.read('category')
                    },
                    categoryElement: {
                        label: resourceProvider.read('segment'),
                        filterCategoryElementsItem: jest.fn()
                    },
                    node: {
                        hidden: true
                    },
                    initialDate: {},
                    finalDate: {
                        allowAfterNow: false
                    },
                    reportType: {
                        hidden: true
                    }
                }
            };
            const pbiFilters = nodeStatusReportFilterBuilder.nodeStatusFilterBuilder.build(props.filters);
            const ReportComponent = toPbiReport(props.reportType, pbiFilters);
            const wrapper = shallow(<NodeStatusComponent {...props} />);
            expect(wrapper).toHaveLength(1);
        });
});
