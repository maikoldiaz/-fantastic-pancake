import setup from '../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import OperationalDeltaBusinessError from '../../../../common/components/modal/deltaCalculationBusinessError.jsx';
import Grid from '../../../../common/components/grid/grid.jsx';

const initialValues = {
    cutoff: {
        ticketInfo: {
            ticket: {
                segment: 'segmentName'
            }
        }
    }

};

function mountWithRealStore(initialValue) {
    const dataGrid = {
        operationalDeltaBusinessError: {
            config: {
                apiUrl: ''
            },
            items: [{
                identifier: 1,
                type: 'Type',
                sourceNode: 'SourceNode',
                destinationNode: 'Destination',
                sourceProduct: 'SourceProduct',
                destinationProduct: 'DestinationProduce',
                quantity: 'Bel',
                unit: '100',
                date: '24/01/2020'
            }],
            pageFilters: {}
        }
    };

    const initialProps = {
    };

    const grid = { operationalDeltaBusinessError: dataGrid.operationalDeltaBusinessError };

    const reducers = {
        operationalDeltaBusinessError: jest.fn(() => dataGrid.operationalDeltaBusinessError),
        grid: jest.fn(() => grid),
        cutoff: jest.fn(() => initialValue.cutoff)
    };

    const store = createStore(combineReducers(reducers));
    // const props =
    const props = {
        closeModal: jest.fn(() => Promise.resolve)
    };
    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <OperationalDeltaBusinessError />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Operational Delta errors', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore(initialValues);
        expect(enzymeWrapper).toHaveLength(1);
    });
    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore(initialValues);
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('operationalDeltaBusinessError');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore(initialValues);
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(7);
    });

    it('should close pop up when accept button is clicked ', () => {
        const { enzymeWrapper, props } = mountWithRealStore(initialValues);
        enzymeWrapper.find('#btn_deltaCalculationBusinessError_submit').simulate('click');
        expect(props.closeModal.mock.calls).toHaveLength(0);
    });
});
