import setup from '../../../setup.js';
import React from 'react';
import { mount } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import OfficialPendingBalanceReportFilter from '../../../../../modules/report/officialPendingBalance/components/filter.jsx';

const initialValues = {
    pendingBalance: {    }
};

const sharedInitialState = {
    categoryElements: [],
    allCategories: []
};

const nodeFilterInitialState = {
    selectedCategory: {},
    selectedElement: {},
};

function mountWithRealStore() {
    const reducers = {
        formValues: jest.fn(() => initialValues.pendingBalance),
        savePendingReportFilter: jest.fn(() => 'filters'),
        form: formReducer,
        shared: jest.fn(() => sharedInitialState),
        nodeFilter: jest.fn(() => nodeFilterInitialState),
        report: jest.fn(() => initialValues)
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        saveFilter: jest.fn(),
        onSelectedElement: jest.fn(),
        resetFilter: jest.fn(),
        handleSubmit: jest.fn()
    };

    const enzymeWrapper = mount(<Provider store={store} >
        <OfficialPendingBalanceReportFilter {...props} />
    </Provider>);

    return { store, enzymeWrapper, props };
}


describe('OfficialPendingBalanceReportFilter', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('view report button should exists', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#btn_nodeFilter_submit')).toBe(true);
    });

    it('should process form submit', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find('form').simulate('submit');
        expect(props.handleSubmit.mock.calls).toHaveLength(2);
    });
});
