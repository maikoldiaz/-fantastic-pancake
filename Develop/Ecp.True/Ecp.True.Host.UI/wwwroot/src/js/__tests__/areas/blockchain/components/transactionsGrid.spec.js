import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { httpService } from '../../../../common/services/httpService';
import TransactionsGrid, { TransactionsGrid as TransactionsGridComponent } from '../../../../modules/administration/blockchain/components/transactionsGrid.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';

const initialState = {
    transaction: {},
    transactions: { headblock: 2, events: [{ type: 1 }] },
    nextPageHead: 1, previousHead: 1, nextHead: 1,
    blockchain: {
        nextPageHead: 1,
        currentPage: 1,
        transactions: { events: [] },
        transactionsToggler: false,
        previousHead: [{ page: 1, head: -1 }]
    }
};

function mountWithRealStore() {
    const dataGrid = {
        transactions: {
            config: {
                apiUrl: ''
            },
            items: [{
                type: 3,
                version: 1,
                id: 'c3937f2d-61d6-4c37-b441-5f5f75f7526c',
                data: ',15-Jul-20,Insert,153569,1,153557,164,,0.21,,34686,10000002372',
                blockNumber: 65384,
                transactionHash: '0xf7b6f2368d254815fee01e9bc50e193dbcd9103a8d53d8e31554e97ab69a1891',
                transactionTime: '20-Jul-20T12:51:00',
                address: 'address'
            }, {
                type: 3,
                version: 1,
                id: '880b7fc4-0efc-4b18-b16b-a5ae9dfb9073',
                data: ',15-Jul-20,Insert,153569,1,153557,164,,0.21,,34686,10000002318',
                blockNumber: 65384,
                transactionHash: '0x93b04f019466b12ad497127a2363aaf0d7f612fbd015977f764caaf044d320c1',
                transactionTime: '20-Jul-20T12:51:00',
                address: 'address'
            }],
            pageFilters: {}
        },

        transaction: {},
        blockchain: {
            previousHead: [{ page: 1, head: -1 }],
            nextPageHead: 1,
            transactionsToggler: false
        }
    };
    const initialProps = {
        onView: jest.fn(() => Promise.resolve()),
        hideEdit: false,
        nextPageHead: 1, previousHead: [], nextHead: 1,
        transactionsToggler: false,
        transactions: {},
        blockchain: { previousHead: [] },
        receiveGridData: jest.fn(() => Promise.resolve),
        receiveCurrentHead: jest.fn(() => Promise.resolve)
    };

    const grid = { blockchaintransactions: dataGrid.transactions };

    const reducers = {
        blockchain: jest.fn(() => initialState.blockchain),
        transactionsToggler: jest.fn(() => false),
        transaction: jest.fn(() => dataGrid.transaction),
        grid: jest.fn(() => grid)
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true }, { blockchain: { nextPageHead: 1, previousHead: [] } });
    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <TransactionsGrid name="blockchaintransactions" initialValues={initialProps} />
    </Provider>);
    return { store, enzymeWrapper, props, initialProps };
}

describe('transactionsGrid', () => {
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
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('blockchaintransactions');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(6);
    });
    it('should send data to grid on receive', () => {
        const props = {
            transactionsToggler: false,
            receiveGridData: jest.fn(() => Promise.resolve),
            transactions: { headblock: 2, events: [{ type: 1 }] }
        };

        const wrapper = shallow(<TransactionsGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { transactionsToggler: true }));
        expect(props.receiveGridData.mock.calls).toHaveLength(1);
    });
});
