import setup from '../../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import NodeTagsGrid, { NodeTagsGrid as NodeTagsGridComponent } from '../../../../../modules/administration/node/nodeTags/components/nodeTagsGrid.jsx';
import Grid from '../../../../../common/components/grid/grid.jsx';
import { fetchGridDataSilent } from '../../../../../common/actions';


function mountWithRealStore() {
    const dataGrid = {
        nodeTags: {
            config: {
                apiUrl: ''
            },
            items: [{
                nodeTagId: 1
            }]
        },
        categoryElementFilter: {
            nodeTagsGrid: {
                values: {
                    categoryElements: [
                    ]
                }
            }
        }
    };

    const state = {
        shared: {
            categories: {},
            categoryElements: {
            }
        },
        flyout: {
        }
    };

    const initialProps = {
        togglePageActions: jest.fn(() => Promise.resolve()),
        disablePageActions: jest.fn(() => Promise.resolve()),
        toggleGrid: jest.fn(() => Promise.resolve()),
        onExpire: jest.fn(() => Promise.resolve()),
        updateGridItems: jest.fn(() => Promise.resolve())
    };

    const grid = { nodeTagsGrid: dataGrid.nodeTags };

    const reducers = {
        categoryElementFilter: jest.fn(() => dataGrid.categoryElementFilter),
        shared: jest.fn(() => state.shared),
        flyout: jest.fn(() => state.flyout),
        grid: jest.fn(() => grid),
        togglePageActions: jest.fn(() => Promise.resolve()),
        disablePageActions: jest.fn(() => Promise.resolve()),
        toggleGrid: jest.fn(() => Promise.resolve()),
        onExpire: jest.fn(() => Promise.resolve()),
        updateGridItems: jest.fn(() => Promise.resolve())
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps);
    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <NodeTagsGrid name="nodeTagsGrid" />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('nodeTagsGrid', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('node tag grid should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it(' node tag gridshould pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        const gridName = enzymeWrapper.find(Grid).at(0).prop('name');
        expect(gridName).toEqual('nodeTagsGrid');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        const columns = enzymeWrapper.find(Grid).at(0).prop('columns');
        expect(columns).toHaveLength(6);
    });

    it('should disable page actions on filter update', () => {
        const props = {
            filters: [],
            disablePageActions: jest.fn(),
            selection: [],
            togglePageActions: jest.fn()
        };
        const wrapper = shallow(<NodeTagsGridComponent {...props} />);
        const newProps = {
            filters: [{ element: {} }]
        };
        wrapper.setProps(Object.assign({}, props, newProps));
        expect(props.disablePageActions.mock.calls).toHaveLength(1);
        expect(props.togglePageActions).toHaveBeenCalledWith(false);
        expect(props.togglePageActions.mock.calls).toHaveLength(1);
    });

    it('should disable page actions and toggle page actions on filter update', () => {
        const props = {
            filters: [],
            disablePageActions: jest.fn(),
            selection: [{
                nodeTagId: 1
            }],
            togglePageActions: jest.fn()
        };
        const wrapper = shallow(<NodeTagsGridComponent {...props} />);
        const newProps = {
            filters: [{ element: {} }]
        };
        wrapper.setProps(Object.assign({}, props, newProps));
        expect(props.disablePageActions.mock.calls).toHaveLength(1);
        expect(props.togglePageActions).toHaveBeenCalledWith(true);
        expect(props.togglePageActions.mock.calls).toHaveLength(1);
    });
});
