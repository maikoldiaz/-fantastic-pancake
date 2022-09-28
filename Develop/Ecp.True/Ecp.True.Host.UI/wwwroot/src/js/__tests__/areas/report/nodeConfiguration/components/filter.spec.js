import setup from '../../../setup.js';
import React from 'react';
import { mount } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import NodeConfigurationReport from '../../../../../modules/report/nodeConfiguration/components/filter.jsx';

const initialValues = {
    cutOffReport: {
        selectedCategory: {},
        selectedElement: {},
        searchedNodes: []
    }
};

const sharedInitialState = {
    categoryElements: [],
    allCategories: []
};


function mountWithRealStore() {
    const reducers = {
        formValues: jest.fn(() => initialValues.cutOffReport),
        nodeFilter: jest.fn(() => 'filters'),
        form: formReducer,
        shared: jest.fn(() => sharedInitialState),
        report: jest.fn(() => initialValues)
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        showError: jest.fn(),
        handleSubmit: jest.fn()
    };

    const enzymeWrapper = mount(<Provider store={store} >
        <NodeConfigurationReport {...props} />
    </Provider>);

    return { store, enzymeWrapper, props };
}


describe('CutoffReportFilter', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('radio-button-group should not exists', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#r_reportFilter_type')).toBe(false);
    });

    it('should not mount nodes field for node status report ', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#txt_nodeFilter_node')).toBe(false);
    });

    it('should mount categories fields for node status report ', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_nodeFilter_category')).toBe(true);
    });
});

