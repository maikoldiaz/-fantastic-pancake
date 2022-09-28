import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import PendingMovementsGrid, { PendingMovementsGrid as PendingMovementsGridComponent } from './../../../../modules/transportBalance/operationalDelta/components/pendingMovementsGrid.jsx';
import Grid from '../../../../common/components/grid/grid.jsx';


function mountWithRealStore() {
    const initialState = {
        pendingMovements: {
            config: {
                name: 'pendingMovements',
                odata: false
            },
            items: [{
                movementId: '001',
                movementType: 'Tolerencia',
                sourceNode: '',
                destinationNode: '',
                sourceProduct: '',
                destinationProduct: '',
                amount: '34450.00',
                unit: 'Bbl',
                operationDate: '2020-06-14T00:00:00',
                action: 'Insert'
            } ]
        },
        pendingInventories: {
            config: {
                name: 'pendingInventories',
                odata: false
            },
            items: [{
                action: 'Insert',
                amount: '34450.00',
                inventoryDate: '2020-06-14T00:00:00',
                inventoryId: 'DEFECT 111011',
                node: 'Automation_test',
                product: 'CRUDO CAMPO MAMBO',
                unit: 'Bbl'
            } ]
        },
        operationalDelta: {
            deltaTicket: {
                startDate: null,
                endDate: null,
                segment: {}
            }
        }
    };

    const initialProps = {
        onEdit: jest.fn(() => Promise.resolve()),
        hideEdit: false
    };

    const grid = { pendingMovements: initialState.pendingMovements, pendingInventories: initialState.pendingInventories };

    const reducers = {
        grid: jest.fn(() => grid),
        onClose: jest.fn(() => Promise.resolve()),
        operationalDelta: jest.fn(() => initialState.operationalDelta)
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true });
    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <PendingMovementsGrid name="pendingMovements" />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('pendingMovementsGrid', () => {
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
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('pendingMovements');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(10);
    });

    it('should get pending movements', () => {
        const props = {
            getPendingMovements: jest.fn(),
            deltaTicket: {
                startDate: null,
                endDate: null,
                segment: {}
            },
            inventories: [],
            movements: []
        };
        const wrapper = shallow(<PendingMovementsGridComponent {...props} />);
        wrapper.instance().getPendingMovements('url');
        expect(props.getPendingMovements.mock.calls).toHaveLength(1);
    });

    it('should return false when no inventories or movements', () => {
        const props = {
            getPendingMovements: jest.fn(),
            deltaTicket: {
                startDate: null,
                endDate: null,
                segment: {}
            },
            inventories: [],
            movements: []
        };
        const wrapper = shallow(<PendingMovementsGridComponent {...props} />);
        expect(wrapper.instance().hasMovementsAndInventories()).toEqual(false);
    });

    it('should hide warning on unmount', () => {
        const props = {
            hideWarning: jest.fn(),
            deltaTicket: {
                startDate: null,
                endDate: null,
                segment: {}
            },
            inventories: [],
            movements: []
        };
        const wrapper = shallow(<PendingMovementsGridComponent {...props} />);
        wrapper.unmount();
        expect(props.hideWarning.mock.calls).toHaveLength(1);
    });

    it('should show warning on no inventory and movements', () => {
        const props = {
            showWarning: jest.fn(),
            deltaTicket: {
                startDate: null,
                endDate: null,
                segment: {}
            },
            inventories: [],
            movements: []
        };
        const wrapper = shallow(<PendingMovementsGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { inventories: [], movements: [] }));
        expect(props.showWarning.mock.calls).toHaveLength(1);
    });

    it('should not show warning on no inventory and movements', () => {
        const props = {
            showWarning: jest.fn(),
            deltaTicket: {
                startDate: null,
                endDate: null,
                segment: {}
            },
            inventories: [],
            movements: []
        };
        const wrapper = shallow(<PendingMovementsGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { inventories: [{ id: 1 }], movements: [] }));
        expect(props.showWarning.mock.calls).toHaveLength(0);
    });
});
