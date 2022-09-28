import setup from '../../../setup';
import React from 'react';
import { mount,shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import ConnectionAttributes, { ConnectionAttributes as ConnectionAttributesComponent } from '../../../../../modules/administration/nodeConnection/attributes/components/connectionAttributes';
import Grid from '../../../../../common/components/grid/grid.jsx';

function mountWithRealStore() {
    const dataGrid = {
        pageActions: {
            hiddenActions: [],
            disabledActions: []
        },
        categories: {
            config: {
                apiUrl: ''
            },
            pageFilters: {},
            items: [{
                categoryId: 1,
                name: 'Segment',
                description: 'Segment',
                isActive: false,
                isGrouper: true,
                sourceNode: {
                    name: 'SourceNodeName'
                },
                destinationNode: {
                    name: 'DestinationNodeName'
                },
                controlLimit: 'test',
                isTransfer: false,
                algorithmId: null
            }, {
                categoryId: 2,
                name: 'Operator',
                description: 'Operator',
                isActive: true,
                isGrouper: true,
                sourceNode: {
                    name: 'SourceNodeName'
                },
                destinationNode: {
                    name: 'DestinationNodeName'
                },
                controlLimit: 'test',
                isTransfer: false,
                algorithmId: null
            }]
        },
        category: {},
        categoryElementFilter: {
            connAttributes: {}
        },
        shared: {
            categories: [{
                categoryId: 1,
                name: 'Segment',
                description: 'Segment',
                isActive: false,
                isGrouper: true
            }, {
                categoryId: 2,
                name: 'Operator',
                description: 'Operator',
                isActive: true,
                isGrouper: true
            }]
        },
        flyout: {
            connAttributes: {
                isOpen: false
            }
        },
        nodeConnection:{
            attributes:{
                statusDelete:false
            }
        }
    };

    const initialProps = {
        onEdit: jest.fn(() => Promise.resolve()),
        hideEdit: false
    };

    const grid = { categoriesGrid: dataGrid.categories };

    const reducers = {
        pageActions: jest.fn(() => dataGrid.pageActions),
        categoryElementFilter: jest.fn(() => dataGrid.categoryElementFilter),
        shared: jest.fn(() => dataGrid.shared),
        flyout: jest.fn(() => dataGrid.flyout),
        category: jest.fn(() => dataGrid.category),
        grid: jest.fn(() => grid),
        onClose: jest.fn(() => Promise.resolve()),
        nodeConnection: jest.fn(() =>dataGrid.nodeConnection)
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true });
    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <ConnectionAttributes name="categoriesGrid" />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('ConnectionAttributes', () => {
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
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(7);
    });
    it('should open confirm modal on delete', () => {
        const props = {
            openConfirmModal: jest.fn(),
        };

        const wrapper = shallow(<ConnectionAttributesComponent {...props} />);
        wrapper.instance().onDelete();
        expect(props.openConfirmModal.mock.calls).toHaveLength(1);
    });
});
it('should do show error modal component did update', () => {
    const props = {
        openErrorModal: jest.fn(() => Promise.resolve()),
        clearStatus: jest.fn(() => Promise.resolve()),
        statusError: false
    };

    const wrapper = shallow(<ConnectionAttributesComponent {...props} />);
    wrapper.setState({ isUpdated: true });
    expect(wrapper.instance().props.openErrorModal.mock.calls).toHaveLength(1);
}); 