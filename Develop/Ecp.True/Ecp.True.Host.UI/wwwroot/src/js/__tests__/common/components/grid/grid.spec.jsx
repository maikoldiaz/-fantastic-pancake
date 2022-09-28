import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { reducer as formReducer } from 'redux-form';
import { mount, shallow } from 'enzyme';
import React from 'react';
// eslint-disable-next-line no-unused-vars
import setup from '../../../areas/setup.js';
import { Grid } from '../../../../common/components/grid/grid';
import { Grid as GridComponent } from '../../../../common/components/grid/grid';
import GridService  from '../../../../common/components/grid/gridService.js';

function mountWithRealStore() {
    const reducers = {
        form: formReducer
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        name: 'categoriesGrid',
        fetchGridData: jest.fn(),
        columns: [
            { Header: 'Category Id', accessor: 'categoryId', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: 'Name', accessor: 'name', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: 'Description', accessor: 'description', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: 'Created Date', accessor: 'createdDate', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: 'Active', accessor: 'isActive', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: 'Grouper', accessor: 'isGrouper', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
            { Header: '', accessor: '', sortable: false, filterable: false }
        ],
        config: { name: 'categoriesGrid', idField: 'categoryId', apiUrl: 'http://localhost:23062/v1/odata/categories', selectable: false },
        items: [], totalItems: 0, selection: [], selectAll: false,
        pageFilters: {}
    };

    const enzymeWrapper = mount(<Provider store={store}><Grid {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}
describe('add operator', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        // Check for the React Select Table if selectable is false
        expect(enzymeWrapper.find('ReactTable').length).toEqual(1);
    });

    it('should call fetchGridDataSilent on refreshSilentToggler change', () => {
        GridService.onPageReset = jest.fn();
        const props = {
            resetPageIndexToggler: false,
            name: 'categoriesGrid',
            fetchGridData: jest.fn(),
            fetchGridDataSilent: jest.fn(),
            columns: [
                { Header: 'Category Id', accessor: 'categoryId', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Name', accessor: 'name', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Description', accessor: 'description', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Created Date', accessor: 'createdDate', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Active', accessor: 'isActive', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Grouper', accessor: 'isGrouper', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: '', accessor: '', sortable: false, filterable: false }
            ],
            config: { name: 'categoriesGrid', idField: 'categoryId', apiUrl: 'http://localhost:23062/v1/odata/categories', selectable: false },
            items: [], totalItems: 0, selection: [], selectAll: false,
            pageFilters: {}
        }
        const wrapper = shallow(<GridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { refreshSilentToggler: true }));
        expect(props.fetchGridDataSilent.mock.calls).toHaveLength(1);
    });

    it('should call fetchGridData on refreshToggler change', () => {
        GridService.onPageReset = jest.fn();
        const props = {
            resetPageIndexToggler: false,
            name: 'categoriesGrid',
            fetchGridData: jest.fn(),
            fetchGridDataSilent: jest.fn(),
            columns: [
                { Header: 'Category Id', accessor: 'categoryId', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Name', accessor: 'name', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Description', accessor: 'description', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Created Date', accessor: 'createdDate', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Active', accessor: 'isActive', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Grouper', accessor: 'isGrouper', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: '', accessor: '', sortable: false, filterable: false }
            ],
            config: { name: 'categoriesGrid', idField: 'categoryId', apiUrl: 'http://localhost:23062/v1/odata/categories', selectable: false },
            items: [], totalItems: 0, selection: [], selectAll: false,
            pageFilters: {}
        }
        const wrapper = shallow(<GridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { refreshToggler: true }));
        expect(props.fetchGridData.mock.calls).toHaveLength(2);
    });

    it('should call fetchGridData on resetToggler change for startEmpty false', () => {
        GridService.onPageReset = jest.fn();
        const props = {
            resetPageIndexToggler: false,
            name: 'categoriesGrid',
            fetchGridData: jest.fn(),
            fetchGridDataSilent: jest.fn(),
            columns: [
                { Header: 'Category Id', accessor: 'categoryId', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Name', accessor: 'name', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Description', accessor: 'description', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Created Date', accessor: 'createdDate', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Active', accessor: 'isActive', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Grouper', accessor: 'isGrouper', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: '', accessor: '', sortable: false, filterable: false }
            ],
            config: { name: 'categoriesGrid', idField: 'categoryId', apiUrl: 'http://localhost:23062/v1/odata/categories', selectable: false },
            items: [], totalItems: 0, selection: [], selectAll: false,
            pageFilters: {}
        }
        const wrapper = shallow(<GridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { resetToggler: true }));
        expect(props.fetchGridData.mock.calls).toHaveLength(2);
    });

    it('should call clearGrid on resetToggler change for startEmpty true', () => {
        GridService.onPageReset = jest.fn();
        const props = {
            resetPageIndexToggler: false,
            name: 'categoriesGrid',
            fetchGridData: jest.fn(),
            fetchGridDataSilent: jest.fn(),
            clearGrid: jest.fn(),
            columns: [
                { Header: 'Category Id', accessor: 'categoryId', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Name', accessor: 'name', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Description', accessor: 'description', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Created Date', accessor: 'createdDate', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Active', accessor: 'isActive', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Grouper', accessor: 'isGrouper', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: '', accessor: '', sortable: false, filterable: false }
            ],
            config: { name: 'categoriesGrid', idField: 'categoryId', apiUrl: 'http://localhost:23062/v1/odata/categories', selectable: false, startEmpty: true},
            items: [], totalItems: 0, selection: [], selectAll: false,
            pageFilters: {}
        }
        const wrapper = shallow(<GridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { resetToggler: true }));
        expect(props.clearGrid.mock.calls).toHaveLength(1);
    });

    it('should call onReceiveData on receiveDataToggler change', () => {
        GridService.onPageReset = jest.fn();
        const props = {
            resetPageIndexToggler: false,
            name: 'categoriesGrid',
            fetchGridData: jest.fn(),
            fetchGridDataSilent: jest.fn(),
            clearGrid: jest.fn(),
            onReceiveData: jest.fn(),
            columns: [
                { Header: 'Category Id', accessor: 'categoryId', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Name', accessor: 'name', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Description', accessor: 'description', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Created Date', accessor: 'createdDate', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Active', accessor: 'isActive', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: 'Grouper', accessor: 'isGrouper', sortable: true, headerClassName: 'rt-sortable-header', filterable: true },
                { Header: '', accessor: '', sortable: false, filterable: false }
            ],
            config: { name: 'categoriesGrid', idField: 'categoryId', apiUrl: 'http://localhost:23062/v1/odata/categories', selectable: false, startEmpty: true},
            items: [], totalItems: 0, selection: [], selectAll: false,
            pageFilters: {}
        }
        const wrapper = shallow(<GridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { receiveDataToggler: true }));
        expect(props.onReceiveData.mock.calls).toHaveLength(1);
    });
});
