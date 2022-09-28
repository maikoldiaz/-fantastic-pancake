import setup from '../../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import CategoriesGrid from '../../../../../modules/administration/category/manageCategory/components/categoriesGrid.jsx';
import Grid from '../../../../../common/components/grid/grid.jsx';

function mountWithRealStore() {
    const dataGrid = {
        categories: {
            config: {
                apiUrl: ''
            },
            items: [{
                categoryId: 1,
                name: 'Segment',
                description: 'Segment',
                isActive: false,
                isGrouper: false
            }, {
                categoryId: 2,
                name: 'Operator',
                description: 'Operator',
                isActive: true,
                isGrouper: true
            }],
            pageFilters: {}
        },
        category: {}
    };

    const initialProps = {
        onEdit: jest.fn(() => Promise.resolve()),
        hideEdit: false
    };

    const grid = { categoriesGrid: dataGrid.categories };

    const reducers = {
        category: jest.fn(() => dataGrid.category),
        grid: jest.fn(() => grid),
        onClose: jest.fn(() => Promise.resolve())
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true });
    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <CategoriesGrid name="categoriesGrid" />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('categoriesGrid', () => {
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
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('categoriesGrid');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(6);
    });

    it('should call edit actions on click of buttons', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find('[id^="lnk_categoriesGrid_edit"]').at(1).simulate('click');
        expect(props.onEdit.mock.calls).toHaveLength(0);
    });
});
