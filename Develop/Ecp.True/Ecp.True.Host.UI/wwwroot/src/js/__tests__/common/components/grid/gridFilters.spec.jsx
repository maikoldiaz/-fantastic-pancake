import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { reducer as formReducer } from 'redux-form';
import { mount, shallow } from 'enzyme';
import React from 'react';
import { SelectFilter, DateFilter, TextboxFilter } from '../../../../common/components/grid/gridFilters';
// eslint-disable-next-line no-unused-vars
import setup from '../../../areas/setup.js';
function mountWithRealStore(isAddSelectFilter = true, isAddTextBoxFilter, hideFilterTextBoxFlagValue) {
    const reducers = {
        form: formReducer
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        options: [],
        name: 'categoriesGrid',
        history: { length: 6, action: 'PUSH', location: { pathname: '/category/manage', search: '', hash: '', key: 'test' } },
        location: { pathname: '/category/manage', search: '', hash: '', key: 'test' },
        match: { path: '/category', url: '/category', isExact: false, params: {} },
        routerConfig: {
            pageKey: 'category', config: {
                routes: {
                    category: {
                        routeKey: 'manage', title: 'manageCategories',
                        component: { compare: null, displayName: 'Connect(Connect(DataGrid))' },
                        actions: [{ title: 'createCategory', iconClass: 'fas fa-search', type: 'Button', actionType: 'modal', key: 'createCategory', mode: 'create' }]
                    }
                }
            }
        },
        edit: true,
        column: {
            Header: 'Activo', sortable: true, show: true, minWidth: 100, minResizeWidth: 11, className: '',
            style: {}, headerClassName: 'rt-sortable-header', headerStyle: {}, footerClassName: '', footerStyle: {}, filterAll: false, id: 'isActive', hideFilterTextBox: hideFilterTextBoxFlagValue
        }
    };
    const rowProps = {};
    let enzymeWrapper = {};
    if (isAddSelectFilter) {
        enzymeWrapper = mount(<Provider store={store}><SelectFilter {...props} {...rowProps} /></Provider>);
    }
    else if (isAddTextBoxFilter) {
        enzymeWrapper = mount(<Provider store={store}><TextboxFilter {...props} {...rowProps} /></Provider>);
    } else {
        enzymeWrapper = mount(<Provider store={store}><DateFilter {...props} {...rowProps} /></Provider>);
    }
    return { store, enzymeWrapper, props, rowProps };
}

describe('grid filters', () => {
    it('should mount select filter successfully', () => {
        const { enzymeWrapper } = mountWithRealStore(true, false, false);
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(SelectFilter).length).toEqual(1);
    });
    it('should mount DateFilter filter successfully', () => {
        const { enzymeWrapper } = mountWithRealStore(false, false, false);
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(DateFilter).length).toEqual(1);
    });

    it('should mount Textfilter successfully', () => {
        const { enzymeWrapper } = mountWithRealStore(false, true, false);
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(TextboxFilter).length).toEqual(1);
    });

    it('should mount Textfilter successfully where hideFilterTextBox is true and style is none ', () => {
        const { enzymeWrapper } = mountWithRealStore(false, true, true);
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find('#txt_categoriesGrid_isActive').prop('style')).toHaveProperty('display', 'none');
    });

    it('should mount Textfilter successfully where hideFilterTextBox is false and style is block ', () => {
        const { enzymeWrapper } = mountWithRealStore(false, true, false);
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find('#txt_categoriesGrid_isActive').prop('style')).toHaveProperty('display', 'block');
    });

    it('should trigger on change of date filter successfully', () => {
        const props = {
            column: {
                id: 'someId'
            },
            onChange: jest.fn(),
            name: 'someName'
        };
        const wrapper = shallow(<DateFilter {...props} />);
        wrapper.find('#dt_someName_someId').simulate('change');
        expect(props.onChange.mock.calls).toHaveLength(1);
    });

    it('should mount text box filter successfully and simulate on change', () => {
        const props = {
            column: {
                id: 'someId'
            },
            onChange: jest.fn(),
            name: 'someName'
        };
        const wrapper = shallow(<TextboxFilter {...props} />);
        wrapper.find('#txt_someName_someId').simulate('change', { target: { value: 'someValue' } });
        expect(wrapper).toBeDefined();
    });

    it('should mount text box filter successfully and simulate key down', () => {
        const props = {
            column: {
                id: 'someId',
                defaultValue: {
                    value: 'someValue'
                }
            },
            onChange: jest.fn(),
            name: 'someName'
        };
        const wrapper = shallow(<TextboxFilter {...props} />);
        wrapper.find('#txt_someName_someId').simulate('keyDown', { key: 'Enter' });
        expect(props.onChange.mock.calls).toHaveLength(1);
    });

    it('should call on filter when on filter is true', () => {
        const props = {
            column: {
                id: 'someId',
                onFilter: jest.fn(() => { return true; })
            },
            onChange: jest.fn(),
            name: 'someName',
            onFilter: jest.fn(() => { return true; })
        };
        const wrapper = shallow(<DateFilter {...props} />);
        wrapper.find('#dt_someName_someId').simulate('change');
        expect(props.column.onFilter.mock.calls).toHaveLength(1);
    });
});
