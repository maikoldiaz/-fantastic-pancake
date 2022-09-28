import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { dateService } from '../../../../../common/services/dateService';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import SettingsAuditReport, { SettingsAuditReport as SettingsAuditReportComponent } from '../../../../../modules/report/settingsAudit/components/filter.jsx';
import { nodeFilterConfigService } from '../../../../../common/services/nodeFilterConfigService';

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

const configuration = {
    category: {
        hidden: true,
        label: resourceProvider.read('category')
    },
    categoryElement: {
        hidden: true,
        label: resourceProvider.read('segment')
    },
    node: { hidden: true },
    initialDate: {},
    finalDate: { allowAfterNow: false },
    reportType: {
        hidden: true
    },
    dateRange: { hidden: true },
    submitText: resourceProvider.read('viewReport'),
    parentPage: 'settingsAuditReport',
    validateDateRange: jest.fn(),
    onSubmitFilter: jest.fn(),
    getStartDateProps: jest.fn(),
    getEndDateProps: jest.fn(),
    setDateProps: true
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
       //// config: jest.fn(() => configuration),
        onSubmit: jest.fn()
    };

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <SettingsAuditReport initialValues={initialValues} />
    </Provider>);

    return { store, enzymeWrapper };
}

describe('SettingsAuditReport', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        nodeFilterConfigService.getSettingsAuditConfig = jest.fn(()=> configuration)
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should unmount component successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.unmount();
        expect(enzymeWrapper).toHaveLength(0);
    });

    it('should mount settings audit report fields', () =>{
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dt_nodeFilter_initialDate')).toBe(true);
        expect(enzymeWrapper.exists('#dt_nodeFilter_finalDate')).toBe(true);
        expect(enzymeWrapper.find('#btn_nodeFilter_submit').text()).toEqual('Ver reporte');
    });

    it('should not mount categories field for settings audit report', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_nodeFilter_category')).toBe(false);
    });
    it('should give error when click on view reports and date is not given', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_nodeFilter_submit').at(0).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call reset filter on mount if back navigation is false', () => {
        const props = {
            backNavigation: false,
            resetFilter: jest.fn()
        };

        const wrapper = shallow(<SettingsAuditReportComponent {...props} />);
        expect(props.resetFilter.mock.calls).toHaveLength(1);
    });

    it('should call reset back navigation on mount if back navigation is true', () => {
        const props = {
            backNavigation: true,
            resetBackNavigation: jest.fn(),
            resetFilter: jest.fn()
        };

        const wrapper = shallow(<SettingsAuditReportComponent {...props} />);
        expect(props.resetBackNavigation.mock.calls).toHaveLength(1);
    });
});
