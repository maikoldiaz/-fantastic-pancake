import setup from '../../setup.js';
import React from 'react';
import { mount } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import NodeStatusReport from '../../../../modules/report/nodeStatus/components/filter';

const initialValues = {
    nodeStatusReport: {
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

const nodeStatusConfig = {
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
    getTicket: jest.fn(),
    submitText: resourceProvider.read('viewReport'),
    parentPage: 'nodeStatusReport',
    getStartDateProps: jest.fn(),
    getEndDateProps: jest.fn(),
    validateDateRange: jest.fn(),
    onSubmitFilter: jest.fn(),
    requiredMessage: resourceProvider.read('CONTROLS_REQUIREDVALIDATION'),
    getReportRequest: jest.fn()
}

function mountWithRealStore() {
    const reducers = {
        formValues: jest.fn(() => initialValues.nodeStatusReport),
        form: formReducer,
        shared: jest.fn(() => sharedInitialState),
        report: jest.fn(() => initialValues),
        nodeFilter: jest.fn(() => nodeFilter)
    };

    const store = createStore(combineReducers(reducers));

    const props = {
        config: nodeStatusConfig
    };

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <NodeStatusReport initialValues={initialValues} />
    </Provider>);


    return { store, enzymeWrapper };
}

describe('NodeStatusReport', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should mount node status fields', () =>{
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_nodeFilter_element')).toBe(true);
        expect(enzymeWrapper.exists('#dt_nodeFilter_initialDate')).toBe(true);
        expect(enzymeWrapper.exists('#dt_nodeFilter_finalDate')).toBe(true);
        expect(enzymeWrapper.find('#btn_nodeFilter_submit').text()).toEqual('viewReport');
    });
    it('should not mount nodes field for node status report ', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#txt_nodeFilter_node')).toBe(false);
    });
    it('should not mount categories fields for node status report ', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_nodeFilter_category')).toBe(false);
    });
    it('should give error when click on view reports and date is not given', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_nodeFilter_submit').at(0).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });
    it('should pass props', () => {
        const { enzymeWrapper } = mountWithRealStore();
        const wrapperProps = enzymeWrapper.instance().props;
        expect(wrapperProps.config.node.hidden).toBe(true);
        expect(wrapperProps.config.parentPage).toEqual('nodeStatusReport');
    });
});
