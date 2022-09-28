import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { reducer as formReducer } from 'redux-form';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import OfficialDeltaFilter from '../../../../../modules/report/officialDeltaNode/components/filter.jsx';
import { navigationService } from '../../../../../common/services/navigationService';


const formValues = {
    element: {
        elementId: 0,
        name: 'name element'
    },
    node: { 
        nodeId: 0, 
        name: 'name node' 
    },
    periods: { start: '2020-08-01', end: '2020-08-31', officialPeriods: [] }
};

const initialValuesReport = {
    officialDeltaNode: {
        filterSettings: {},
        filters: null,
        manualToggler: false,
        formValues: formValues
    }
};

const sharedInitialState = {
    categoryElements: [{categoryId:1,isActive:false},{category:2,isActive:true}],
    allCategories: [],
    progressStatus: {},
    selectedCategory: [],
    selectedElement: [],
    searchedNodes: [],
    categoryElementsToggler: false
};

const nodeFilterInitialState = {
    dateRange: {},
    defaultYear: null,
    dateRangeToggler: null,
    viewReportButtonStatusToggler: false
};

const loaderInitialState = {
    counter: 0
};

const configState = {

}

function mountWithRealStore() {
    const reducers = {
        formValues: jest.fn(() => formValues),
        form: formReducer,
        nodeFilter: jest.fn(() => nodeFilterInitialState),
        loader: jest.fn(() => loaderInitialState),
        shared: jest.fn(() => sharedInitialState),
        report: jest.fn(() => initialValuesReport)
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        onSelectedElement: jest.fn(),
        resetFilter: jest.fn(),
        resetField: jest.fn(),
        resetDateRange: jest.fn(),
        handleSubmit: callback => callback(formValues)
    };

    const dispatchSpy = jest.spyOn(store, 'dispatch');

    const enzymeWrapper = mount(<Provider store={store} >
        <OfficialDeltaFilter {...props} />
    </Provider>);

    return { store, enzymeWrapper, props, dispatchSpy };
}


describe('Report OfficialDeltaNode OfficialDeltaFilter', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.navigateTo = jest.fn();
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });


    it('button submit should exists', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#btn_nodeFilter_submit')).toBe(true);
    });

    it('should process form submit', () => {
        const { enzymeWrapper, props, dispatchSpy } = mountWithRealStore();
        enzymeWrapper.find('form').simulate('submit');
        expect(dispatchSpy).toHaveBeenCalledWith(expect.objectContaining({ type: 'OFFICIAL_DELTA_SAVE_FILTER' }));
    });
});

