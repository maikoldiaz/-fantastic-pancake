import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { httpService } from '../../../../common/services/httpService';
import BlockRangeGrid, { BlockRangeGrid as BlockRangeGridComponent } from '../../../../modules/administration/blockchain/components/blockRangeGrid.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import Grid from '../../../../common/components/grid/grid.jsx';

const initialState = {
    blockRangeEvents: [{ type: 3 }, { type: 3 }],
    blockRange: {
        headBlock: '1',
        tailBlock: '2',
        event: 3
    },
    blockchain: {
        blockRangeEvents: [{ type: 3 }, { type: 3 }],
        blockRange: {
            headBlock: '1',
            tailBlock: '2',
            event: 3
        },
        blockRangeEventsToggler: false,
        blockRangeEventsFailureToggler: false
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
            blockRangeEvents: [{ type: 3 }, { type: 3 }],
            blockRange: {
                headBlock: '1',
                tailBlock: '2',
                event: 3
            },
            blockRangeEventsToggler: false,
            blockRangeEventsFailureToggler: false
        }
    };
    const initialProps = {
        blockRangeEvents: [{ type: 3 }, { type: 3 }],
        blockRange: {
            headBlock: '1',
            tailBlock: '2',
            event: 3
        },
        blockRangeEventsToggler: false,
        blockRangeEventsFailureToggler: false,
        receiveGridData: jest.fn(() => Promise.resolve)
    };

    const grid = { blockchainRangeEvents: dataGrid.transactions };

    const reducers = {
        blockchain: jest.fn(() => initialState.blockchain),
        blockRangeEventsToggler: jest.fn(() => false),
        grid: jest.fn(() => grid)
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true });
    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <BlockRangeGrid name="blockchainRangeEvents" initialValues={initialProps} />
    </Provider>);
    return { store, enzymeWrapper, props, initialProps };
}

describe('blockchainRangeEvents', () => {
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
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('blockchainRangeEvents');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(5);
    });
    it('should send data to grid on receive', () => {
        const props = {
            blockRange: {
                headBlock: '1',
                tailBlock: '2',
                event: 3
            },
            blockRangeEventsToggler: false,
            receiveGridData: jest.fn(() => Promise.resolve),
            blockRangeEvents: [{ type: 3 }, { type: 3 }]
        };

        const wrapper = shallow(<BlockRangeGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { blockRangeEventsToggler: true }));
        expect(props.receiveGridData.mock.calls).toHaveLength(1);
    });
    it('should show error on data receive failure', () => {
        const props = {
            blockRange: {
                headBlock: '1',
                tailBlock: '2',
                event: 3
            },
            blockRangeEventsFailureToggler: false,
            receiveGridData: jest.fn(() => Promise.resolve),
            showError: jest.fn(() => Promise.resolve),
            blockRangeEvents: [{ type: 3 }, { type: 3 }]
        };

        const wrapper = shallow(<BlockRangeGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { blockRangeEventsFailureToggler: true }));
        expect(props.receiveGridData.mock.calls).toHaveLength(1);
        expect(props.showError.mock.calls).toHaveLength(1);
    });
});
