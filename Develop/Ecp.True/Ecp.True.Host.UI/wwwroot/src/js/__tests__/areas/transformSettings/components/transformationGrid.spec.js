import setup from '../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import TransformationGrid from './../../../../modules/administration/transformSettings/components/transformationGrid.jsx';
import { navigationService } from './../../../../common/services/navigationService';
import { httpService } from './../../../../common/services/httpService';
import { resourceProvider } from './../../../../common/services/resourceProvider';

function mountWithRealStore_Movement() {
    const dataGrid = {
        movements: {
            config: {
                apiUrl: ''
            },
            pageFilters: {}
        }
    };
    const transformSettings = {
        deleteToggler: true,
        editToggler: true
    };

    const grid = { movements: dataGrid.movements };

    const reducers = {
        onEdit: jest.fn(() => Promise.resolve()),
        grid: jest.fn(() => grid),
        transformSettings: jest.fn(() => transformSettings)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <TransformationGrid name="movements" />
    </Provider>);
    return { store, enzymeWrapper };
}

function mountWithRealStore_Inventory() {
    const dataGrid = {
        inventories: {
            config: {
                apiUrl: ''
            },
            pageFilters: {}
        }
    };
    const transformSettings = {
        deleteToggler: true,
        editToggler: true
    };

    const grid = { inventories: dataGrid.inventories };

    const reducers = {
        onEdit: jest.fn(() => Promise.resolve()),
        grid: jest.fn(() => grid),
        transformSettings: jest.fn(() => transformSettings)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <TransformationGrid name="inventories" />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('transformation Grid', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.getParamByName = jest.fn();
    });

    it('should mount successfully for movement grid', () => {
        const { enzymeWrapper } = mountWithRealStore_Movement();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should mount successfully for inventory grid', () => {
        const { enzymeWrapper } = mountWithRealStore_Inventory();
        expect(enzymeWrapper).toHaveLength(1);
    });
});
