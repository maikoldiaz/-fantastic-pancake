import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import { navigationService } from '../../../../../common/services/navigationService';
import OfficialPendingBalanceReport from '../../../../../modules/report/officialPendingBalance/components/report.jsx';
import * as reportsFilterBuilder from '../../../../../common/components/filterBuilder/reportsFilterBuilder';
import { toPbiReport } from '../../../../../common/components/reports/pbiReport.jsx';

const initialValues = {
    pendingBalance: {
        selectedCategory: {},
        selectedElement: {}
    }
};
const sharedInitialState = {
    categoryElements: [],
    allCategories: []
};

const nodeFilter = {
    selectedCategory: '',
    selectedElement: ''
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
    node: { hidden: true },
    submitText: resourceProvider.read('viewReport'),
    parentPage: 'officialBalanceLoaded',
    onSubmitFilter: jest.fn()
};

function mountWithRealStore() {
    const reducers = {
        formValues: jest.fn(() => initialValues.pendingBalance),
        form: formReducer,
        shared: jest.fn(() => sharedInitialState),
        report: jest.fn(() => initialValues),
        nodeFilter: jest.fn(() => nodeFilter)
    };

    const store = createStore(combineReducers(reducers));

    const props = {
        config: officialInitialConfig
    };

    const enzymeWrapper = mount(<Provider store={store}{...props} >
        <OfficialPendingBalanceReport initialValues={initialValues} />
    </Provider>);

    return { store, enzymeWrapper };
}

describe('Official Pending Balance Report', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.navigateTo = jest.fn();
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should not mount categories field for pending ', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_nodeFilter_category')).toBe(false);
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
                formValues: jest.fn(() => initialValues.pendingBalance),
                form: formReducer,
                shared: jest.fn(() => sharedInitialState),
                report: jest.fn(() => initialValues),
                nodeFilter: jest.fn(() => nodeFilter)
            };
            const store = createStore(combineReducers(reducers));
            const pbiFilters = reportsFilterBuilder.reportsFilterBuilder.build(props.filters);
            const ReportComponent = toPbiReport(props.reportType, pbiFilters);
            const wrapper = shallow(<Provider store={store} {...props}>
                <OfficialPendingBalanceReport {...props} />
            </Provider>);
            expect(wrapper).toHaveLength(1);
        });
});
