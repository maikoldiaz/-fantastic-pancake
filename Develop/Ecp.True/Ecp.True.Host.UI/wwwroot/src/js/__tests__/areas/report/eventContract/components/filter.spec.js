import setup from '../../../setup.js';
import React from 'react';
import { mount } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import EventContractReport from '../../../../../modules/report/eventContract/components/filter.jsx';
const initialValues = {
    reports: {
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
        formValues: jest.fn(() => initialValues.reports),
        form: formReducer,
        shared: jest.fn(() => sharedInitialState),
        balanceControlChart: jest.fn(() => initialValues),
        nodeFilter: jest.fn(() => nodeFilter)
    };

    const store = createStore(combineReducers(reducers));

    const props = {
        config: jest.fn(() => config)
    };

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <EventContractReport initialValues={initialValues} />
    </Provider>);


    return { store, enzymeWrapper };
}

describe('EventContractReport', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should mount Event Contract Report fields', () =>{
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_nodeFilter_element')).toBe(true);
        expect(enzymeWrapper.exists('#txt_nodeFilter_node')).toBe(true);
        expect(enzymeWrapper.exists('#dt_nodeFilter_initialDate')).toBe(true);
        expect(enzymeWrapper.exists('#dt_nodeFilter_finalDate')).toBe(true);
        expect(enzymeWrapper.find('#btn_nodeFilter_submit').text()).toEqual('viewReport');
    });
    it('should not mount categories field for Event Contract Report ', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_nodeFilter_category')).toBe(false);
    });
    it('should give error when click on view reports and date is not given', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_nodeFilter_submit').at(0).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });
});
