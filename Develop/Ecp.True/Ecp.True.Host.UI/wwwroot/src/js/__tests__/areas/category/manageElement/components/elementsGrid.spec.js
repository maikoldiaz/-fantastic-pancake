import setup from '../../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import ElementsGrid from '../../../../../modules/administration/category/manageElement/components/elementsGrid.jsx';
import Grid from '../../../../../common/components/grid/grid.jsx';

function mountWithRealStore() {
    const dataGrid = {
        elements: {
            config: {
                apiUrl: ''
            },
            pageFilters: {},
            items: [{
                category: {
                    categoryId: 1,
                    name: 'category',
                    description: 'Segment',
                    isActive: false,
                    isGrouper: false
                },
                categoryId: 1,
                name: 'element',
                description: 'Segment',
                isActive: false
            }, {
                category: {
                    categoryId: 2,
                    name: 'category2',
                    description: 'Segment',
                    isActive: false,
                    isGrouper: false
                },
                categoryId: 2,
                name: 'element2',
                description: 'Operator',
                isActive: true
            }]
        },
        categoryElement: {}
    };

    const initialProps = {
        onEdit: jest.fn(() => Promise.resolve()),
        hideEdit: false
    };

    const grid = { elementsGrid: dataGrid.elements };

    const reducers = {
        categoryElement: jest.fn(() => dataGrid.categoryElement),
        grid: jest.fn(() => grid),
        onClose: jest.fn(() => Promise.resolve())
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true });
    const enzymeWrapper = mount(<Provider store={store}{...props}>
        <ElementsGrid name="elementsGrid" />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('elementsGrid', () => {
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
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('elementsGrid');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(6);
    });

    it('should call edit actions on click of buttons', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find('[id^="lnk_elementsGrid_edit"]').at(1).simulate('click');
        expect(props.onEdit.mock.calls).toHaveLength(0);
    });
});
