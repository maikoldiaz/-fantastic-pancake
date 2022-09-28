import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import { navigationService } from '../../../../../common/services/navigationService';
import CutOffReport, { CutOffReport as CutOffReportComponent } from '../../../../../modules/report/cutOff/components/report.jsx';
import * as cutoffReportFilterBuilder from '../../../../../modules/report/cutOff/filterBuilder.js';
import { constants } from '../../../../../common/services/constants';
import { toPbiReport } from '../../../../../common/components/reports/pbiReport.jsx';
import { dateService } from '../../../../../common/services/dateService.js';
const initialValues = {
    cutOffReport: {
        selectedCategory: {},
        selectedElement: {},
        searchedNodes: [],
        filters: {
            executionId: 123,
            initialDate: dateService.today(),
            finalDate: dateService.today()
        }
    }
};
const sharedInitialState = {
    categoryElements: [],
    allCategories: []
};

function mountWithRealStore() {
    const reducers = {
        formValues: jest.fn(() => initialValues.cutOffReport),
        saveCutOffReportFilter: jest.fn(() => 'filters'),
        form: formReducer,
        shared: jest.fn(() => sharedInitialState),
        report: jest.fn(() => initialValues)
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        showError: jest.fn(),
        handleSubmit: jest.fn()
    };

    const enzymeWrapper = mount(<Provider store={store} {...props} >
        <CutOffReport initialValues={initialValues} />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('CutoffReport', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.navigateTo = jest.fn();
        navigationService.getParamByName = jest.fn();
    });

    it('should mount component returned by toPbiReport',
        async () => {
            const props = {
                elementName: 'Estación',
                nodeName: 'SAN FERNANDO',
                initialDate: '12/09/2019',
                finalDate: '18/09/2019',
                reportType: constants.Report.WithoutCutoff,
                getOwnershipNode: jest.fn(),
                filters: {
                    elementName: 'Estación',
                    nodeName: 'SAN FERNANDO',
                    initialDate: '12/09/2019',
                    finalDate: '18/09/2019',
                    reportType: constants.Report.WithoutCutoff,
                    getOwnershipNode: jest.fn(),
                    executionId: 146
                }
            };
            const result = cutoffReportFilterBuilder.cutoffReportFilterBuilder.build(props.filters);
            const ReportComponent = toPbiReport(props.reportType, result);
            const wrapper = shallow(<CutOffReportComponent  {...props} />)
            expect(wrapper).toHaveLength(1);

        });

    it('should mount component when parameter ownershipNodeId',
        async () => {
            navigationService.getParamByName = jest.fn(() => 'ownershipNodeId');
            const navigationServiceMock = navigationService.navigateTo = jest.fn(() => 'manage');
            const props = {
                getOwnershipNode: jest.fn()
            };
            const enzymeWrapper = shallow(<CutOffReportComponent {...props} />)
            expect(navigationServiceMock.mock.calls).toHaveLength(0);
        });

    it('should mount component when parameter view',
        async () => {
            navigationService.getParamByName = jest.fn(() => 'view');
            const navigationServiceMock = navigationService.navigateTo = jest.fn(() => 'manage');
            const props = {

            };
            const enzymeWrapper = shallow(<CutOffReportComponent {...props} />)

            expect(navigationServiceMock.mock.calls).toHaveLength(1);
        });
});