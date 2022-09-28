
import React from 'react';
import Enzyme, { mount, shallow } from 'enzyme';
import EnzymeAdapter from 'enzyme-adapter-react-16';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import { navigationService } from '../../../../../common/services/navigationService';
import EventContract, { EventContract as EventContractComponent } from '../../../../../modules/report/eventContract/components/report.jsx';
import * as eventContractFilterBuilder from '../../../../../modules/report/eventContract/filterBuilder.js';
import { constants } from '../../../../../common/services/constants';
import { toPbiReport } from '../../../../../common/components/reports/pbiReport.jsx';

Enzyme.configure({ adapter: new EnzymeAdapter() });
const initialValues = {
    eventContractReport: {
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

const filter = {
    selectedCategory: 'aa',
    selectedElement: 'cc',
    selectedNode: 'ss'
};


function mountWithRealStore() {
    const reducers = {
        formValues: jest.fn(() => initialValues.eventContractReport),
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
        <EventContract initialValues={initialValues} />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('EventContract', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.navigateTo = jest.fn();
    });
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });
    it('should not mount categories field for event contract ', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_nodeFilter_category')).toBe(false);
    });

    it('should mount component returned by toPbiReport',
        async () => {
            const props = {
                elementName: 'Estación',
                nodeName: 'SAN FERNANDO',
                initialDate: '12/09/2019',
                finalDate: '18/09/2019',
                reportType: constants.Report.EventContractReport,
                filters: {
                    elementName: 'Estación',
                    nodeName: 'SAN FERNANDO',
                    initialDate: '12/09/2019',
                    finalDate: '18/09/2019',
                    reportType: constants.Report.EventContractReport,
                }
            };

            const result = eventContractFilterBuilder.eventContractFilterBuilder.build(props.filters);
            const ReportComponent = toPbiReport(props.reportType, result);
            const wrapper = shallow(<EventContractComponent  {...props} />)
            expect(wrapper).toHaveLength(1);
        });

    it('should navigate back to manage module', () => {
        const navigationServiceMock = navigationService.navigateTo = jest.fn(() => 'manage');
        const props = {};


        const wrapper = shallow(<EventContractComponent  {...props} />)
        expect(navigationServiceMock.mock.calls).toHaveLength(1);
    });



});