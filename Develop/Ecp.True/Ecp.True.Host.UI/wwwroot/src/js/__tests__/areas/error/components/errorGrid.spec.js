import React from 'react';
import setup from '../../setup';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import ErrorsGrid from '../../../../modules/administration/exceptions/components/errorsGrid.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { Provider } from 'react-redux';
import { systemConfigService } from './../../../../common/services/systemConfigService';
import { httpService } from '../../../../common/services/httpService';
import Grid from '../../../../common/components/grid/grid.jsx';
import { navigationService } from './../../../../common/services/navigationService';


function mountWithRealStore() {
    const dataGrid = {
        pendingTransactionErrors: {
            selection: [],
            config: { name: 'pendingTransactionErrors', idField: 'errorId', selectable: true, odata: false },
            refreshToggler: true,
            items: [{
                errorId: 1,
                transactionId: 1,
                errorMessage: 'err',
                comment: 'test Comment : 1',
                pendingTransaction: {
                    systemName: 'System Name',
                    messageTypeId: 'Movement'
                }
            }],
            pageFilters: {},
            categoryElements: []
        }
    };

    const grid = { pendingTransactionErrors: dataGrid.pendingTransactionErrors };
    const controlException = { controlException: { pageFilters: {}, retryToggler: false } };
    const shared = { categoryElements: [] };

    const props = {
        onDelete: jest.fn(() => Promise.resolve()),
        onDiscardException: jest.fn(() => Promise.resolve()),
        enableDisableDiscard: jest.fn(() => Promise.resolve())
    };

    const reducers = {
        grid: jest.fn(() => grid),
        controlexception: jest.fn(() => controlException),
        shared: jest.fn(() => shared)
    };
    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <ErrorsGrid name="pendingTransactionErrors" />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('control error grid', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        systemConfigService.getDefaultErrorLastDays = jest.fn(() => {
            return 10;
        });
        navigationService.navigateTo = jest.fn(() => Promise.resolve());
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('pendingTransactionErrors');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(6);
    });
    it('should call edit actions on click of buttons', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find('[id^="lnk_pendingTransactionErrors_discardException"]').at(0).simulate('click');
        expect(props.onDelete.mock.calls).toHaveLength(0);
    });
});
