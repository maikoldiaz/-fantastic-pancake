import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import { navigationService } from '../../../../../common/services/navigationService';
import BalanceControl, { BalanceControl as BalanceControlComponent } from '../../../../../modules/report/balanceControl/components/report.jsx';
import * as balanceControlChartFilterBuilder from '../../../../../modules/report/balanceControl/filterBuilder.js';
import { constants } from '../../../../../common/services/constants';
import { toPbiReport } from '../../../../../common/components/reports/pbiReport.jsx';

const initialValues = {
    balanceControlChart: {
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
        formValues: jest.fn(() => initialValues.balanceControlChart),
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

    const enzymeWrapper = mount(<Provider store={store}{...props} >
        <BalanceControl initialValues={initialValues} />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('BalanceControlChart', () => {
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

    it('should mount and return component returned by toPbiReport',
        async () => {
            const props = {
                elementName: 'Estación',
                nodeName: 'SAN FERNANDO',
                initialDate: '12/09/2019',
                finalDate: '18/09/2019',
                reportType: constants.Report.BalanceControlChart,
                filters: {
                    elementName: 'Estación',
                    nodeName: 'SAN FERNANDO',
                    initialDate: '12/09/2019',
                    finalDate: '18/09/2019',
                    reportType: constants.Report.BalanceControlChart
                }
            };

            const result = balanceControlChartFilterBuilder.balanceControlChartFilterBuilder.build(props.filters);
            const ReportComponent = toPbiReport(props.reportType, result);
            const wrapper = shallow(<BalanceControlComponent  {...props} />)
            expect(wrapper).toHaveLength(1);
        });

    it('should navigate back to manage module', () => {
        const navigationServiceMock = navigationService.navigateTo = jest.fn(() => 'manage');
        const props = {};

        const wrapper = shallow(<BalanceControlComponent  {...props} />)
        expect(navigationServiceMock.mock.calls).toHaveLength(1);
    });

});