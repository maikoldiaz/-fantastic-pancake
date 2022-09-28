import setup from '../../../setup.js';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import TransferPointsLogisticGrid from '../../../../../modules/administration/transferPoints/logistics/components/grid.jsx';

const initialValues = {
    transferPointsLogistics: {
        config: {
            apiUrl: ''
        },
        pageFilters: {}
    }
};

const grid = { transferPointsLogistics: initialValues.transferPointsLogistics };

function mountWithRealStore() {
    const reducers = {
        grid: jest.fn(() => grid)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <TransferPointsLogisticGrid initialValues={initialValues} />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('TransferPointsLogisticGrid', () => {
    it('should mount with real store', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('transferPointsLogistics');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(6);
    });
});