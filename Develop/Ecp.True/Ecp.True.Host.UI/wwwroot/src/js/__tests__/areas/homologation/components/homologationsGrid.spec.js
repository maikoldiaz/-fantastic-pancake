import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import HomologationsGrid, { HomologationsGrid as HomologationsGridComponent } from '../../../../modules/administration/homologation/components/homologationsGrid.jsx';
import Grid from '../../../../common/components/grid/grid.jsx';

function mountWithRealStore() {
    const initialState = {
        homologations: {
            refreshToggler: false,
            conflictToggler: false,
            config: {
                apiUrl: '',
                name: 'homologations',
                idField: 'homologationGroupId'
            },
            items: []
        }
    };

    const initialProps = {
        onEdit: jest.fn(() => Promise.resolve()),
        hideEdit: false
    };

    const grid = { homologationsGrid: initialState.homologations };

    const reducers = {
        homologations: jest.fn(() => initialState.homologations),
        grid: jest.fn(() => grid),
        onClose: jest.fn(() => Promise.resolve())
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true });
    const enzymeWrapper = mount(<Provider store={store} {...props}><HomologationsGrid name="homologationsGrid" /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('homologationsGrid', () => {
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
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('homologationsGrid');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(4);
    });
    it('should refresh grid on refresh toggler update', () => {
        const props = {
            refreshGrid: jest.fn(),
            closeModal: jest.fn()
        };

        const wrapper = shallow(<HomologationsGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { refreshToggler: true }));
        expect(props.refreshGrid.mock.calls).toHaveLength(1);
        expect(props.closeModal.mock.calls).toHaveLength(1);
    });
    it('should show conflict on conflict toggler update', () => {
        const props = {
            showConflict: jest.fn()
        };

        const wrapper = shallow(<HomologationsGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { conflictToggler: true }));
        expect(props.showConflict.mock.calls).toHaveLength(1);
    });
    it('should open confirm modal on delete', () => {
        const props = {
            openConfirmModal: jest.fn(),
            openCreateHomologationModal: jest.fn()
        };

        const wrapper = shallow(<HomologationsGridComponent {...props} />);
        wrapper.instance().onDelete();
        expect(props.openConfirmModal.mock.calls).toHaveLength(1);
    });
});
