import setup from '../../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import { navigationService } from '../../../../../common/services/navigationService';
import ConnectionDetails, {ConnectionDetails as ConnectionDetailsComponent} from '../../../../../modules/administration/nodeConnection/attributes/components/connectionDetails';
import Grid from '../../../../../common/components/grid/grid.jsx';

function mountWithRealStore() {
    const dataGrid = {
        categories: {
            config: {
                apiUrl: ''
            },
            pageFilters: {}
        },
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
        nodeConnection: {
            attributes: {
                connection: {
                    controlLimit: 'test',
                    isTransfer: true,
                    sourceNode: {},
                    destinationNode: {}
                }
            }
        }
    };

    const initialProps = {
        onEdit: jest.fn(() => Promise.resolve()),
        hideEdit: false
    };

    const grid = { connectionProducts: dataGrid.categories, ownershipRules: {} };

    const reducers = {
        categoryElementFilter: jest.fn(() => dataGrid.categoryElementFilter),
        shared: jest.fn(() => dataGrid.shared),
        flyout: jest.fn(() => dataGrid.flyout),
        nodeConnection: jest.fn(() => dataGrid.nodeConnection),
        grid: jest.fn(() => grid),
        onClose: jest.fn(() => Promise.resolve()),
        ownershipRules: jest.fn(() => grid.ownershipRules)
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialProps, { hideEdit: true });
    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <ConnectionDetails name="categoriesGrid" />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('ConnectionDetails', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.getParamByName = jest.fn(() => 'nodeConnectionId');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should pass the grid name for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('name')).toEqual('connectionProducts');
    });

    it('should build columns for the grid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find(Grid).at(0).prop('columns')).toHaveLength(7);
    });

    it('should have connection and controlLimit controls', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#lbl_connAttributes_name')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_connAttributes_controlLimit')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_connAttributes_isTransfer')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_connAttributes_algorithm')).toBe(true);
        expect(enzymeWrapper.find('.ep-card__lbl').at(0).text()).toEqual('connection');
        expect(enzymeWrapper.find('.ep-card__lbl').at(1).text()).toEqual('controlLimit');
        expect(enzymeWrapper.find('.ep-card__lbl').at(2).text()).toEqual('isTransferValue');
        expect(enzymeWrapper.find('.ep-card__lbl').at(3).text()).toEqual('analyticalModelTitle');
    });
    it('should handle form onchange', () => {
        const props = {
            value:true,
            initialValues: {
                    isActive: undefined,
                    rowVersion: undefined,
                    sourceNodeId: undefined,
                    destinationNodeId: undefined
            
            },
            isUpdated: false,
            connection: {
                "nodeConnectionId": 38024,
                "sourceNodeId": 87730,
                "destinationNodeId": 87736,
                "isActive": false,
                "controlLimit": null,
                "isTransfer": true,
                "algorithmId": null,
                "rowVersion": "AAAAAAACg1M=",
                "sourceNode": {
                    "name": "Automation_a4xp8",
                    "isActive": true
                },
                "destinationNode": {
                    "name": "Automation_ib7rd",
                    "isActive": true
                },
                "algorithm": null
            },
            getConnection: jest.fn(() => Promise.resolve()),
            setControlLimitSource: jest.fn(() => Promise.resolve()),
            changeStatus: jest.fn(),
        };

        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('#tog_category_active').hostNodes().simulate('change', true);
        expect(enzymeWrapper).toHaveLength(1);
    });
    
    it('should do nothing component did update', () => {
        const props = {

            isUpdated: false,
            connection: {

                isTransfer: true,
                sourceNode: {
                    isActive: true
                },
                destinationNode: {
                    isActive: true
                },
            },
            getConnection: jest.fn(() => Promise.resolve()),
            setControlLimitSource: jest.fn(() => Promise.resolve()),
            changeStatusFlag: jest.fn()
        };

        const wrapper = shallow(<ConnectionDetailsComponent {...props} />);
        wrapper.setState({ isUpdated: true });
        expect(wrapper.instance().props.changeStatusFlag.mock.calls).toHaveLength(0);
    });

    it('should get connection component did update', () => {
        const props = {

            isUpdated: true,
            connection: {

                isTransfer: true,
                sourceNode: {
                    isActive: true
                },
                destinationNode: {
                    isActive: true
                },
            },
            getConnection: jest.fn(() => Promise.resolve()),
            setControlLimitSource: jest.fn(() => Promise.resolve()),
            changeStatusFlag: jest.fn()
        };

        const wrapper = shallow(<ConnectionDetailsComponent {...props} />);
        wrapper.setState({ isUpdated: false });
        expect(wrapper.instance().props.changeStatusFlag).toBeCalled();
    });

});
