import setup from '../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import OwnershipActions, { OwnershipActions as OwnershipActionsComponent } from '../../../../modules/transportBalance/nodeOwnership/components/ownershipActions.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import { constants } from '../../../../common/services/constants';
import { dataService } from '../../../../modules/transportBalance/nodeOwnership/services/dataService';
import { navigationService } from '../../../../common/services/navigationService';

const state = {
    nodeOwnership: {
        ownershipNode: {
            nodeDetails: {
                nodeId: 1,
                ownershipStatus: 1,
                node: {
                    name: 'Automation'
                },
                ticket: {
                    startDate: '01/01/2020',
                    endDate: '01/01/2020',
                    categoryElement: {}
                },
                ticketId: 1,
                ownershipNodeId: 1,
                editor: 'testUser'
            },
            publishSuccess: false,
            publishOwnershipToggler: false,
            unlockNodeToggler: false,
            nodeMovementInventoryData: [{ status: constants.Modes.Delete, transactionId: 1 }]
        }
    },
    root: {
        context: {
            userId: 1
        }
    },
    confirmModal: {},
    grid: {
        ownershipNodeBalance: {
            items: [{
                productId: 1,
                product: {
                    productId: 1,
                    name: 'Test'
                }
            }]
        }
    }
};

const shallowMountProps = {
    unlockNodeAndResetEditChanges: jest.fn(),
    showError: jest.fn(),
    publishOwnerships: jest.fn(),
    onNodePublishing: jest.fn(),
    ownershipNode: state.nodeOwnership.ownershipNode,
    ownershipNodeBalance: state.grid.ownershipNodeBalance,
    nodeDetails: state.nodeOwnership.ownershipNode.nodeDetails,
    currentUser: 'testUser',
    showConcurrencyError: jest.fn()
};

function mountWithRealStore() {
    const props = {
        unlockNodeAndResetEditChanges: jest.fn(),
        showError: jest.fn(),
        publishOwnerships: jest.fn(),
        onNodePublishing: jest.fn()
    };

    const reducers = {
        root: jest.fn(() => state.root),
        grid: jest.fn(() => state.grid),
        nodeOwnership: jest.fn(() => state.nodeOwnership),
        confirmModal: jest.fn(() => state.confirmModal)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <OwnershipActions {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('ownership actions', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.navigateTo = jest.fn();
        dataService.groupByArray = jest.fn(() => []);
        dataService.compareStatus = jest.fn(() => true);
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should mount relevant buttons/links', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#lnk_ownershipNodeDetails_viewReport')).toBe(true);
        expect(enzymeWrapper.find('#lnk_ownershipNodeDetails_publish').text()).toEqual('publish');
        expect(enzymeWrapper.find('#lnk_ownershipNodeDetails_submitToApproval').text()).toEqual('submitToApproval');
        expect(enzymeWrapper.find('#lnk_ownershipNodeDetails_unlock').text()).toEqual('unlock');
    });

    it('should enable publish and unlock button', () => {
        const newProps = {
            nodeDetails: Object.assign({}, shallowMountProps.nodeDetails, {
                ownershipStatus: constants.OwnershipNodeStatus.LOCKED
            })
        };
        const wrapper = shallow(<OwnershipActionsComponent {...shallowMountProps} />);
        wrapper.setProps(Object.assign({}, shallowMountProps, newProps));
        expect(wrapper.find('#lnk_ownershipNodeDetails_publish').prop('disabled')).toBe(false);
        expect(wrapper.find('#lnk_ownershipNodeDetails_unlock').prop('disabled')).toBe(false);
    });

    it('should disable all buttons', () => {
        const wrapper = shallow(<OwnershipActionsComponent {...shallowMountProps} />);
        expect(wrapper.find('#lnk_ownershipNodeDetails_viewReport').prop('disabled')).toBe(true);
        expect(wrapper.find('#lnk_ownershipNodeDetails_submitToApproval').prop('disabled')).toBe(true);
        expect(wrapper.find('#lnk_ownershipNodeDetails_publish').prop('disabled')).toBe(true);
        expect(wrapper.find('#lnk_ownershipNodeDetails_unlock').prop('disabled')).toBe(true);
    });

    it('should enable view report and disable submitToApproval button', () => {
        dataService.compareStatus = jest.fn(() => false);
        const wrapper = shallow(<OwnershipActionsComponent {...shallowMountProps} />);
        expect(wrapper.find('#lnk_ownershipNodeDetails_viewReport').prop('disabled')).toBe(false);
        expect(wrapper.find('#lnk_ownershipNodeDetails_submitToApproval').prop('disabled')).toBe(true);
    });

    it('should publish updated movements on click of publish button', () => {
        const movement = { movementSource: {}, period: {} };
        dataService.buildMovementObject = jest.fn(() => movement);
        const groupedByArray = [{
            values: [{
                status: constants.Modes.Update,
                isMovement: true
            }]
        }];
        dataService.groupByArray = jest.fn(() => groupedByArray);
        const newProps = {
            nodeDetails: Object.assign({}, shallowMountProps.nodeDetails, {
                ownershipStatus: constants.OwnershipNodeStatus.LOCKED
            })
        };
        const wrapper = shallow(<OwnershipActionsComponent {...shallowMountProps} />);
        wrapper.setProps(Object.assign({}, shallowMountProps, newProps));
        wrapper.find('#lnk_ownershipNodeDetails_publish').first().simulate('click');
        expect(shallowMountProps.publishOwnerships.mock.calls).toHaveLength(0);
        expect(shallowMountProps.onNodePublishing.mock.calls).toHaveLength(1);
    });

    it('should publish updated inventories on click of publish button', () => {
        const invOwnership = { ownershipvolume: 2345, ownershippercentage: 100 };
        dataService.buildInventoryObject = jest.fn(() => invOwnership);
        const groupedByArray = [{
            values: [{
                status: constants.Modes.Update,
                isMovement: false
            }]
        }];
        dataService.groupByArray = jest.fn(() => groupedByArray);
        const newProps = {
            nodeDetails: Object.assign({}, shallowMountProps.nodeDetails, {
                ownershipStatus: constants.OwnershipNodeStatus.LOCKED
            })
        };
        const wrapper = shallow(<OwnershipActionsComponent {...shallowMountProps} />);
        wrapper.setProps(Object.assign({}, shallowMountProps, newProps));
        wrapper.find('#lnk_ownershipNodeDetails_publish').first().simulate('click');
        expect(shallowMountProps.onNodePublishing).toHaveBeenCalled();
    });

    it('should call component did update and navigate on publish success', () => {
        const newProps = {
            ownershipNode: Object.assign({}, shallowMountProps.ownershipNode, {
                publishSuccess: true
            })
        };
        const wrapper = shallow(<OwnershipActionsComponent {...shallowMountProps} />);
        wrapper.setProps(Object.assign({}, shallowMountProps, newProps));
        expect(navigationService.navigateTo.mock.calls).toHaveLength(1);
    });

    it('should call component did update and publish on change of publishOwnershipToggler prop', () => {
        const newProps = {
            ownershipNode: Object.assign({}, shallowMountProps.ownershipNode, {
                publishOwnershipToggler: true
            })
        };
        const wrapper = shallow(<OwnershipActionsComponent {...shallowMountProps} />);
        wrapper.setProps(Object.assign({}, shallowMountProps, newProps));
    });

    it('should publish updated movements for operative scenario on click of publish button', () => {
        const movement = { movementSource: {}, period: {}, scenarioId: 1 };
        dataService.buildMovementObject = jest.fn(() => movement);
        const groupedByArray = [{
            values: [{
                status: constants.Modes.Update,
                isMovement: true
            }]
        }];
        dataService.groupByArray = jest.fn(() => groupedByArray);
        const newProps = {
            nodeDetails: Object.assign({}, shallowMountProps.nodeDetails, {
                ownershipStatus: constants.OwnershipNodeStatus.LOCKED
            })
        };
        const wrapper = shallow(<OwnershipActionsComponent {...shallowMountProps} />);
        wrapper.setProps(Object.assign({}, shallowMountProps, newProps));
        wrapper.find('#lnk_ownershipNodeDetails_publish').first().simulate('click');
        expect(shallowMountProps.publishOwnerships.mock.calls).toHaveLength(0);
    });

    it('should publish updated movements on publish success ', () => {
        const movement = { movementSource: {}, period: {}, scenarioId: 1 };
        dataService.buildMovementObject = jest.fn(() => movement);
        const groupedByArray = [{
            values: [{
                status: constants.Modes.Update,
                isMovement: true
            }]
        }];
        dataService.groupByArray = jest.fn(() => groupedByArray);
        const wrapper = shallow(<OwnershipActionsComponent {...shallowMountProps} />);
        wrapper.setProps(Object.assign({}, shallowMountProps.ownershipNode, { isPublishing: false, nodeOwnershipPublishSuccessToggler: !shallowMountProps.ownershipNode.nodeOwnershipPublishSuccessToggler }));
        expect(shallowMountProps.publishOwnerships.mock.calls).toHaveLength(1);
    });

    it('should show concurrency error on publish failure', () => {
        const movement = { movementSource: {}, period: {}, scenarioId: 1 };
        dataService.buildMovementObject = jest.fn(() => movement);
        const groupedByArray = [{
            values: [{
                status: constants.Modes.Update,
                isMovement: true
            }]
        }];
        dataService.groupByArray = jest.fn(() => groupedByArray);
        const wrapper = shallow(<OwnershipActionsComponent {...shallowMountProps} />);
        wrapper.setProps(Object.assign({}, shallowMountProps.ownershipNode, { nodeOwnershipPublishFailure: !shallowMountProps.ownershipNode.nodeOwnershipPublishFailure }));
        expect(shallowMountProps.showConcurrencyError.mock.calls).toHaveLength(1);
    });
});
