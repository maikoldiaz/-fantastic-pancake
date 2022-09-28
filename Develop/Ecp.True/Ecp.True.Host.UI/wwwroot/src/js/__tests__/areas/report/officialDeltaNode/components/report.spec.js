import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import { navigationService } from '../../../../../common/services/navigationService';
import * as reportsFilterBuilder from '../../../../../common/components/filterBuilder/reportsFilterBuilder';
import { toPbiReport } from '../../../../../common/components/reports/pbiReport.jsx';
import { constants } from '../../../../../common/services/constants';
import OfficialDeltaReport, { OfficialDeltaReport as OfficialDeltaReportComponent } from '../../../../../modules/report/officialDeltaNode/components/report.jsx';
import { dateService } from '../../../../../common/services/dateService.js';

const initialValues = {
    initialBalance: {
        selectedCategory: {},
        selectedElement: {},
        searchedNodes: []
    },
    reportExecution: {
        execution: {}
    },
    officialDeltaNode: {
        filterSettings: {},
        filters: null,
        manualToggler: false,
        formValues: null,
        source: 'grid'
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

const officialInitialConfig = {
    category: {
        hidden: true,
        label: resourceProvider.read('category')
    },
    categoryElement: {
        label: resourceProvider.read('segment'),
        filterCategoryElementsItem: jest.fn()
    },
    initialDate: { hidden: true },
    finalDate: {
        hidden: true
    },
    reportType: {
        hidden: true
    },
    node: {},
    submitText: resourceProvider.read('viewReport'),
    parentPage: 'officialBalanceLoaded',
    onSubmitFilter: jest.fn()
};

const modalValues = {
    isOpen: false
};

function mountWithRealStore() {
    const reducers = {
        formValues: jest.fn(() => initialValues.initialBalance),
        form: formReducer,
        shared: jest.fn(() => sharedInitialState),
        report: jest.fn(() => initialValues),
        nodeFilter: jest.fn(() => nodeFilter),
        modal: jest.fn(() => modalValues)
    };

    const store = createStore(combineReducers(reducers));

    const props = {
        config: officialInitialConfig
    };

    const dispatchSpy = jest.spyOn(store, 'dispatch');

    const enzymeWrapper = mount(<Provider store={store} {...props} >
        <OfficialDeltaReport initialValues={initialValues} {...props} />
    </Provider>);

    return { store, enzymeWrapper, props, dispatchSpy };
}

describe('Report OfficialDeltaNode OfficialDeltaReport', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        httpService.getQueryString = jest.fn(() => '');
        navigationService.navigateTo = jest.fn();
        navigationService.getParamByName = jest.fn(() => '123');
    });

    it('should mount successfully with param id', () => {
        navigationService.getParamByName = jest.fn(() => '123');
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should mount successfully with param null', () => {
        navigationService.getParamByName = jest.fn(() => null);
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should mount successfully with param view', () => {
        navigationService.getParamByName = jest.fn(() => 'view');
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call componentDidUpdate', () => {
        const props = {
            showPageAction: jest.fn(),
            enableDisablePageAction: jest.fn(),
            filters: { nodeStatus: constants.OwnershipNodeStatusType.APPROVED, element: { elementId: 0, name: 'name element' } }
        }
        dateService.parseToPBIString = jest.fn();
        const enzymeWrapper = shallow(<OfficialDeltaReportComponent {...props} />)
        enzymeWrapper.setProps(Object.assign({}, props, { reportToggler: true }));
        expect(props.showPageAction.mock.calls).toHaveLength(1);
    });

    it('should call componentDidUpdate and exit with filters is null', () => {
        const props = {
            showPageAction: jest.fn(),
            enableDisablePageAction: jest.fn(),
            filters: null
        }
        dateService.parseToPBIString = jest.fn();
        const enzymeWrapper = shallow(<OfficialDeltaReportComponent {...props} />)
        enzymeWrapper.setProps(Object.assign({}, props, { reportToggler: true }));
        expect(props.showPageAction.mock.calls).toHaveLength(0);
    });

    it('should call componentDidUpdate and exit with filters that don\'t have nodestatus', () => {
        const props = {
            showPageAction: jest.fn(),
            enableDisablePageAction: jest.fn(),
            filters: { element: { elementId: 0, name: 'name element' } }
        }
        dateService.parseToPBIString = jest.fn();
        const enzymeWrapper = shallow(<OfficialDeltaReportComponent {...props} />)
        enzymeWrapper.setProps(Object.assign({}, props, { reportToggler: true }));
        expect(props.showPageAction.mock.calls).toHaveLength(0);
    });

    it('should call componentDidUpdate and execute nodestatus have nodestatus equal SUBMITFORAPPROVAL', () => {
        const props = {
            showPageAction: jest.fn(),
            enableDisablePageAction: jest.fn(),
            filters: { nodeStatus: constants.OwnershipNodeStatusType.SUBMITFORAPPROVAL, element: { elementId: 0, name: 'name element' } }
        }
        dateService.parseToPBIString = jest.fn();
        const enzymeWrapper = shallow(<OfficialDeltaReportComponent {...props} />)
        enzymeWrapper.setProps(Object.assign({}, props, { reportToggler: true }));
        expect(props.showPageAction.mock.calls).toHaveLength(1);
    });

    it('should call componentDidUpdate and execute nodestatus have nodestatus equal REJECTED', () => {
        const props = {
            showPageAction: jest.fn(),
            enableDisablePageAction: jest.fn(),
            filters: { nodeStatus: constants.OwnershipNodeStatusType.REJECTED, element: { elementId: 0, name: 'name element' } }
        }
        dateService.parseToPBIString = jest.fn();
        const enzymeWrapper = shallow(<OfficialDeltaReportComponent {...props} />)
        enzymeWrapper.setProps(Object.assign({}, props, { reportToggler: true }));
        expect(props.showPageAction.mock.calls).toHaveLength(2);
    });

    it('should call componentDidUpdate and execute nodestatus have nodestatus equal REOPENED', () => {
        const props = {
            showPageAction: jest.fn(),
            enableDisablePageAction: jest.fn(),
            filters: { nodeStatus: constants.OwnershipNodeStatusType.REOPENED, element: { elementId: 0, name: 'name element' } }
        }
        dateService.parseToPBIString = jest.fn();
        const enzymeWrapper = shallow(<OfficialDeltaReportComponent {...props} />)
        enzymeWrapper.setProps(Object.assign({}, props, { reportToggler: true }));
        expect(props.showPageAction.mock.calls).toHaveLength(2);
    });

    it('should call componentDidUpdate and execute nodestatus have nodestatus equal DELTA', () => {
        const props = {
            showPageAction: jest.fn(),
            enableDisablePageAction: jest.fn(),
            filters: { nodeStatus: constants.OwnershipNodeStatusType.DELTA, element: { elementId: 0, name: 'name element' } }
        }
        dateService.parseToPBIString = jest.fn();
        const enzymeWrapper = shallow(<OfficialDeltaReportComponent {...props} />)
        enzymeWrapper.setProps(Object.assign({}, props, { reportToggler: true }));
        expect(props.showPageAction.mock.calls).toHaveLength(2);
    });

    it('component is unmounted and modal is open', () => {
        const props = {
            showPageAction: jest.fn(),
            enableDisablePageAction: jest.fn(),
            filters: { nodeStatus: constants.OwnershipNodeStatusType.DELTA, element: { elementId: 0, name: 'name element' } },
            resetFilter: jest.fn(),
            resetFromGrid: jest.fn(),
            isOpenModal: true,
            closeModal: jest.fn()
        }
        const enzymeWrapper = shallow(<OfficialDeltaReportComponent {...props} />)
        enzymeWrapper.unmount();
        expect(props.closeModal.mock.calls).toHaveLength(1);
    });

    it('component is unmounted and modal is closed', () => {
        const props = {
            showPageAction: jest.fn(),
            enableDisablePageAction: jest.fn(),
            filters: { nodeStatus: constants.OwnershipNodeStatusType.DELTA, element: { elementId: 0, name: 'name element' } },
            resetFilter: jest.fn(),
            resetFromGrid: jest.fn(),
            isOpenModal: false,
            closeModal: jest.fn()
        }
        const enzymeWrapper = shallow(<OfficialDeltaReportComponent {...props} />)
        enzymeWrapper.unmount();
        expect(props.closeModal.mock.calls).toHaveLength(0);
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
                    },
                    element: {}
                }
            };
            const reducers = {
                formValues: jest.fn(() => initialValues.initialBalance),
                form: formReducer,
                shared: jest.fn(() => sharedInitialState),
                report: jest.fn(() => initialValues),
                nodeFilter: jest.fn(() => nodeFilter)
            };
            const store = createStore(combineReducers(reducers));
            const pbiFilters = reportsFilterBuilder.reportsFilterBuilder.build(props.filters);
            const ReportComponent = toPbiReport(props.reportType, pbiFilters);
            const wrapper = shallow(<Provider store={store} {...props}>
                <OfficialDeltaReport {...props} />
            </Provider>);
            expect(wrapper).toHaveLength(1);
        });
});
