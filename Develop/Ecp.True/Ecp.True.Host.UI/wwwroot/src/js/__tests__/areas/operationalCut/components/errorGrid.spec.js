import React from 'react';
import setup from '../../setup';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import ErrorsGrid, { ErrorsGrid as ErrorGridComponent } from '../../../../modules/transportBalance/cutOff/components/errorsGrid.jsx';

import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import Grid from '../../../../common/components/grid/grid.jsx';
import { gridUtils } from '../../../../common/components/grid/gridUtils';

function mountWithRealStore() {
    const grid = {
        addComment: {
            comments: []
        },
        cutoff: {
            operationalCut: {
                ticket: {}
            }
        },
        pendingTransactionsErrorsGrid: {
            selection: [],
            config: { name: 'pendingTransactionsErrorsGrid', idField: 'errorId', selectable: true, odata: false },
            refreshToggler: true,
            items: [{
                errorId: 1, transactionId: 1, errorMessage: 'err', comment: 'test Comment : 1',
                pendingTransaction: {
                    actionTypeId: 'Insert',
                    blobName: '/true/sinoper/json/inventory/intnew1.json',
                    createdBy: 'System',
                    createdDate: '2019-10-14T11:57:14.74',
                    destinationNode: 'source node B',
                    destinationProduct: 'source product B',
                    endDate: '2019-10-14T12:57:08.147',
                    messageId: 't67c57b6-test-426e-912a-22707fb75r55',
                    messageTypeId: 'Inventory',
                    sourceNode: 'source node A',
                    sourceProduct: 'source product A',
                    startDate: '2019-10-14T12:57:08.147',
                    systemTypeId: 'TRUE',
                    ticket: null,
                    ticketId: 23687,
                    transactionId: 1,
                    units: 'Gallon',
                    volume: 100
                }
            }]
        },
        shared: {
            categoryElements: []
        },
        config: { wizardName: 'name' }
    };

    const initialProps = {
        ticket: jest.fn(() => Promise.resolve()),
        selection: false,
        comments: []
    };

    const reducers = {
        cutoff: jest.fn(() => grid.cutoff),
        grid: jest.fn(() => grid),
        addComment: jest.fn(() => grid.addComment),
        shared: jest.fn(() => grid.shared)
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true });

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <ErrorsGrid config={grid.config} name="pendingTransactionsErrorsGrid" />
    </Provider>);
    return { store, enzymeWrapper, props };
}


describe('errorGrid', () => {
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
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('pendingTransactionsErrorsGrid');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(11);
    });

    it('should verify column names for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(1).find('span').at(0).text()).toEqual('errorMessage');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(2).find('span').at(0).text()).toEqual('source');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(3).find('span').at(0).text()).toEqual('classification');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(4).find('span').at(0).text()).toEqual('actionType');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(5).find('span').at(0).text()).toEqual('sourceNode');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(6).find('span').at(0).text()).toEqual('destinationNode');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(7).find('span').at(0).text()).toEqual('sourceProduct');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(8).find('span').at(0).text()).toEqual('destinationProduct');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(9).find('span').at(0).text()).toEqual('date');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(10).find('span').at(0).text()).toEqual('netQuantity');
        expect(enzymeWrapper.find('.rt-tr').at(0).find('.rt-th').at(11).find('span').at(0).text()).toEqual('units');
    });


    it('should read all Column', () => {
        gridUtils.buildTextColumn = jest.fn();
        gridUtils.buildDecimalColumn = jest.fn();
        gridUtils.buildDateColumn = jest.fn();
        const props = {
            selection: [],
            comment: 'comment',
            items: [{
                errorId: 1, transactionId: 1, errorMessage: 'err', comment: 'test Comment : 1',
                pendingTransaction: {
                    actionTypeId: 'Insert',
                    blobName: '/true/sinoper/json/inventory/intnew1.json',
                    createdBy: 'System',
                    createdDate: '2019-10-14T11:57:14.74',
                    destinationNode: 'source node B',
                    destinationProduct: 'source product B',
                    endDate: '2019-10-14T12:57:08.147',
                    messageId: 't67c57b6-test-426e-912a-22707fb75r55',
                    messageTypeId: 'Inventory',
                    sourceNode: 'source node A',
                    sourceProduct: 'source product A',
                    startDate: '2019-10-14T12:57:08.147',
                    systemTypeId: 'TRUE',
                    ticket: null,
                    ticketId: 23687,
                    transactionId: 1,
                    units: 'Gallon',
                    volume: 100
                }
            }],
            operationalCut: {
                ticket: {
                    startDate: '2019-10-14T11:57:14.74',
                    endDate: '2019-10-14T11:57:14.74',
                    categoryElementName: 'categoryname'
                }
            },
            step: 0,
            getErrors: jest.fn(),
            incrementCutOff: jest.fn(),
            getCategoryElements: jest.fn(),
            config: { wizardName: 'name' }
        };

        const wrapper = shallow(<ErrorGridComponent {...props} />);
        expect(wrapper.instance().getColumns()).toHaveLength(11);

        wrapper.instance().getErrors('someurl');

        expect(props.getErrors.mock.calls).toHaveLength(1);
        expect(props.incrementCutOff.mock.calls).toHaveLength(1);
    });
});


