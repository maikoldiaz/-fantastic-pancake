import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import { dateService } from '../../../../../common/services/dateService';
import { constants } from '../../../../../common/services/constants';
import TransactionsAuditReport, { TransactionsAuditReport as TransactionsAuditReportComponent } from '../../../../../modules/report/transactionsAudit/components/filter.jsx';

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
        <TransactionsAuditReport initialValues={initialValues} />
    </Provider>);

    return { store, enzymeWrapper };
}

describe('TransactionsAuditReport', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should mount movement inventory report fields', () =>{
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#lbl_nodeFilter_element')).toBe(true);
        expect(enzymeWrapper.exists('#dd_nodeFilter_element')).toBe(true);
        expect(enzymeWrapper.exists('#dt_nodeFilter_initialDate')).toBe(true);
        expect(enzymeWrapper.exists('#dt_nodeFilter_finalDate')).toBe(true);
        expect(enzymeWrapper.find('#btn_nodeFilter_submit').text()).toEqual('viewReport');
    });

    it('should not mount node field for movement inventory audit report ', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#txt_nodeFilter_node')).toBe(false);
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

        const wrapper = shallow(<TransactionsAuditReportComponent {...props} />);
        expect(props.resetFilter.mock.calls).toHaveLength(1);
    });
    it('should call reset back navigation on mount if back navigation is true', () => {
        const props = {
            backNavigation: true,
            resetBackNavigation: jest.fn(),
            resetFilter: jest.fn()
        };

        const wrapper = shallow(<TransactionsAuditReportComponent {...props} />);
        expect(props.resetBackNavigation.mock.calls).toHaveLength(1);
    });
});
