import setup from '../../setup.js';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import LogisticsValidations from '../../../../modules/transportBalance/logistics/components/logisticsValidations.jsx';

const logistics = {
    operational: {
        refreshToggler: true,
        lastOwnershipDate: {},
        initialDate: {},
        refreshDateToggler: true,
        logisticsInfo: {
            segmentName: 'Test Segment',
            node: { name: 'Test Node' },
            owner: [{ name: 'Ecopetrol' }],
            startDate: '01/02/2020',
            endDate: '02/02/2020'
        },
        validationData: [{
            nodeName: 'Test Node',
            operationDate: '02/02/2020',
            nodeStatus: 'FAILED'
        }]
    }
};

const shared = {
    groupedCategoryElements: [],
    categoryElements: []
};

const dataGrid = {
    logistics: {
        validationData: [{
            nodeName: 'Test Node',
            operationDate: '02/02/2020',
            nodeStatus: 'FAILED'
        }],
        refreshToggler: false,
        config: {
            apiUrl: ''
        },
        pageFilters: {}
    }
};

const grid = { logisticsValidationGrid: dataGrid.logistics };

const props = {
    componentType: 3
};

function mountWithRealStore() {
    const reducers = {
        closeModal: jest.fn(() => Promise.resolve),
        logistics: jest.fn(() => logistics),
        shared: jest.fn(() => shared),
        grid: jest.fn(() => grid)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <LogisticsValidations {...props}/>
    </Provider>);
    return { store, enzymeWrapper };
}

describe('LogisticsValidations', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('find Control', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#span_logisticsValidation_segment')).toBe(true);
        expect(enzymeWrapper.exists('#span_logisticsValidation_node')).toBe(true);
        expect(enzymeWrapper.exists('#span_logisticsValidation_owner')).toBe(true);
        expect(enzymeWrapper.exists('#span_logisticsValidation_period')).toBe(true);
    });
});
