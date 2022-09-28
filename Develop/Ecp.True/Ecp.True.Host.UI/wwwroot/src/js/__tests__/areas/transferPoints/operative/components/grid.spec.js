import setup from '../../../setup.js';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import Grid from '../../../../../common/components/grid/grid.jsx';
import TransferPointsOperationalGrid from '../../../../../modules/administration/transferPoints/operative/components/grid.jsx';

const initialValues = {
    transferPointsOperational: {
        updateOwnershipDataToggler: true,
        refreshToggler: false,
        config: {
            apiUrl: ''
        },
        pageFilters: {}
    }
};

const grid = { transferPointsOperational: initialValues.transferPointsOperational };

function mountWithRealStore() {
    const reducers = {
        grid: jest.fn(() => grid)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <TransferPointsOperationalGrid initialValues={initialValues} />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('TransferPointsOperationalGrid', () => {
    it('should mount with real store', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('transferPointsOperational');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(9);
    });
});