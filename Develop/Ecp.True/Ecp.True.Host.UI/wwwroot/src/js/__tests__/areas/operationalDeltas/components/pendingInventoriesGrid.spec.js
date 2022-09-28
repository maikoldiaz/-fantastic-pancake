import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import PendingInventoriesGrid, { PendingInventoriesGrid as PendingInventoriesGridComponent } from './../../../../modules/transportBalance/operationalDelta/components/pendingInventoriesGrid.jsx';
import Grid from '../../../../common/components/grid/grid.jsx';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { wizardNextStep, wizardSetStep } from '../../../../common/actions';

function mountWithRealStore(newState = {}) {
    const initialState = {
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

    const grid = { pendingInventories: initialState.pendingInventories };

    const reducers = {
        grid: jest.fn(() => grid),
        onClose: jest.fn(() => Promise.resolve()),
        operationalDelta: jest.fn(() => Object.assign({}, initialState.operationalDelta, newState))
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true });
    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <PendingInventoriesGrid name="pendingInventories" config={{ wizardName: 'name' }} goToStep={jest.fn()}/>
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('pendingInventoriesGrid', () => {
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
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('pendingInventories');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(7);
    });

    it('should get pending inventories', () => {
        const props = {
            getPendingInventories: jest.fn(),
            deltaTicket: {
                startDate: null,
                endDate: null,
                segment: {}
            },
            config: { wizardName: 'name' }
        };
        const wrapper = shallow(<PendingInventoriesGridComponent {...props} />);
        wrapper.instance().getPendingInventories('url');
        expect(props.getPendingInventories.mock.calls).toHaveLength(1);
    });

    it('should cancel Delta Calculation on click of cancel', () => {
        const props = {
            cancelDeltaCalculation: jest.fn(),
            deltaTicket: {
                startDate: null,
                endDate: null,
                segment: {}
            },
            config: { wizardName: 'name' }
        };
        const { enzymeWrapper } = mountWithRealStore(props);
        enzymeWrapper.find(SectionFooter).find('#btn_pendingInventoriesGrid_cancel').simulate('click');
        expect(wizardSetStep).toHaveLength(2);
    });

    it('should go to next step in wizard on click of next', () => {
        const props = {
            onNext: jest.fn(),
            deltaTicket: {
                startDate: null,
                endDate: null,
                segment: {}
            },
            config: { wizardName: 'name' }
        };
        const { enzymeWrapper } = mountWithRealStore(props);
        enzymeWrapper.find(SectionFooter).find('#btn_pendingInventoriesGrid_submit').simulate('click');
        expect(wizardNextStep).toHaveLength(1);
    });
});
