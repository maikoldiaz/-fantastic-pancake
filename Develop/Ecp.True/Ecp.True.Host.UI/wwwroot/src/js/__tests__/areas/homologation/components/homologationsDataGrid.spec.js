import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import HomologationsDataGrid, { HomologationsDataGrid as HomologationsDataGridComponent } from '../../../../modules/administration/homologation/components/homologationsDataGrid.jsx';
import Grid from '../../../../common/components/grid/grid.jsx';

function mountWithRealStore() {
    const initialState = {
        homologations: {
            dataMappingsToggler: false,
            config: {
                apiUrl: '',
                name: 'homologations',
                idField: 'homologationGroupId'
            },
            items: [],
            homologationGroup: { sourceSystem: { systemTypeId: 2 } },
            homologationGroupDataMappings: []
        }
    };

    const initialProps = {
        onEdit: jest.fn(() => Promise.resolve()),
        hideEdit: false
    };

    const grid = { homologationsDataGrid: initialState.homologations };

    const reducers = {
        homologations: jest.fn(() => initialState.homologations),
        grid: jest.fn(() => grid),
        onClose: jest.fn(() => Promise.resolve())
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true });
    const enzymeWrapper = mount(<Provider store={store} {...props}><HomologationsDataGrid name="homologationsDataGrid" /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('homologationsDataGrid', () => {
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
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('homologationsDataGrid');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(3);
    });

    it('should load columns for source system id of TRUE', () => {
        const props = {
            loadDataMappings: jest.fn(),
            homologationGroupDataMappings: [],
            homologationGroup: { sourceSystem: { systemTypeId: 1 } }
        };

        const wrapper = shallow(<HomologationsDataGridComponent {...props} />);
        expect(wrapper.find(Grid).at(0).prop('columns')).toHaveLength(3);
    });
    it('should load datamappings on dataMappingsToggler update', () => {
        const props = {
            loadDataMappings: jest.fn(),
            homologationGroupDataMappings: [],
            homologationGroup: { sourceSystem: { systemTypeId: 2 } }
        };

        const wrapper = shallow(<HomologationsDataGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { dataMappingsToggler: true }));
        expect(props.loadDataMappings.mock.calls).toHaveLength(1);
    });
});
