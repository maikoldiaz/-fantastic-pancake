import setup from '../../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { ConfirmModal } from '../../../../../common/components/modal/confirmModal';
import StorageLocationProductsGrid, { StorageLocationProductsGrid as StorageLocationProductsGridComponent } from './../../../../../modules/administration/productConfiguration/components/storageLocationProduct/storageLocationProductsGrid';

function mountWithRealStore() {
    const dataGrid = {
        pageActions: {
            hiddenActions: [],
            disabledActions: []
        },
        associations: {
            config: {
                apiUrl: ''
            },
            pageFilters: {},
            items: [{
                storageLocation: {
                    logisticCenter: {
                        name: 'center'
                    },
                    name: 'storageLocation'
                },
                product: {
                    name: 'product'
                }
            }, {
                storageLocation: {
                    logisticCenter: {
                        name: 'center'
                    },
                    name: 'storageLocation'
                },
                product: {
                    name: 'product'
                }
            }]
        },
        products: {
            deleted: false,
            deleteToggler: false
        }
    };

    const initialProps = {

    };

    const grid = { associationsGrid: dataGrid.associations };

    const reducers = {
        pageActions: jest.fn(() => dataGrid.pageActions),
        grid: jest.fn(() => grid),
        onClose: jest.fn(() => Promise.resolve()),
        products: jest.fn(() => dataGrid.products),
        openModal: jest.fn(() => Promise.resolve()),
        closeModal: jest.fn(() => Promise.resolve())
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, {});
    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <StorageLocationProductsGrid name="associationsGrid" />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('StorageLocationProducts', () => {
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
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('associationsGrid');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(4);
    });

    it('should open confirm modal on delete', () => {
        const props = {
            openModal: jest.fn(),
        };

        const wrapper = shallow(<StorageLocationProductsGridComponent {...props} />);
        wrapper.instance().onDelete();
        expect(props.openModal.mock.calls).toHaveLength(1);
    });

    it('should do show modal component did update', () => {
        const props = {
            openModal: jest.fn(() => Promise.resolve()),
            closeModal: jest.fn(() => Promise.resolve()),
            deleteToggler: false,
            isDeleted: false
        };

        const wrapper = shallow(<StorageLocationProductsGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { deleteToggler: !props.deleteToggler }));
        expect(wrapper.instance().props.openModal.mock.calls).toHaveLength(1);
    });

    it('should do show modal component did update when deleted is true', () => {
        const props = {
            openModal: jest.fn(() => Promise.resolve()),
            closeModal: jest.fn(() => Promise.resolve()),
            refreshGrid: jest.fn(() => Promise.resolve()),
            deleteToggler: false,
            isDeleted: true
        };

        const wrapper = shallow(<StorageLocationProductsGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { deleteToggler: !props.deleteToggler }));
        expect(wrapper.instance().props.openModal.mock.calls).toHaveLength(0);
    });

    it('should do show modal component did update when deletedToggles is equal', () => {
        const props = {
            openModal: jest.fn(() => Promise.resolve()),
            closeModal: jest.fn(() => Promise.resolve()),
            deleteToggler: false,
            isDeleted: true
        };

        const wrapper = shallow(<StorageLocationProductsGridComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { deleteToggler: props.deleteToggler }));
        expect(wrapper.instance().props.openModal.mock.calls).toHaveLength(0);
    });
});
