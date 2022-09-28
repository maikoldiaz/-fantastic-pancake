import setup from '../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import AnnulationsGrid from '../../../../modules/administration/annulation/components/annulationsGrid.jsx';
import Grid from '../../../../common/components/grid/grid.jsx';

function mountWithRealStore() {
    const initialState = {
        annulations: {
            refreshToggler: false,
            config: {
                apiUrl: '',
                name: 'annulations',
                idField: 'annulationId'
            },
            items: [{
                categoryId: 1,
                sourceCategoryElement: { name: 'Automation_test' },
                annulationCategoryElement: { name: 'Evacuación Salida' },
                sourceNodeOriginType: { name: 'Destino' },
                destinationNodeOriginType: { name: 'Origen' },
                sourceProductOriginType: { name: 'Ninguno' },
                destinationProductOriginType: { name: 'Origen' },
                isActive: 'Active'
            }, {
                categoryId: 1,
                sourceCategoryElement: { name: 'Automation_test' },
                annulationCategoryElement: { name: 'Evacuación Salida' },
                sourceNodeOriginType: { name: 'Destino' },
                destinationNodeOriginType: { name: 'Origen' },
                sourceProductOriginType: { name: 'Ninguno' },
                destinationProductOriginType: { name: 'Origen' },
                isActive: 'Active'
            }]
        },
        shared: {
            originTypes: []
        }
    };

    const initialProps = {
        onEdit: jest.fn(() => Promise.resolve()),
        hideEdit: false,
        shared: {}
    };

    const grid = { annulationsGrid: initialState.annulations };

    const reducers = {
        shared: jest.fn(() => initialState.shared),
        grid: jest.fn(() => grid),
        onClose: jest.fn(() => Promise.resolve())
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true });
    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <AnnulationsGrid name="annulationsGrid" />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('annulationsGrid', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('annulationsGrid');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(9);
    });
});
