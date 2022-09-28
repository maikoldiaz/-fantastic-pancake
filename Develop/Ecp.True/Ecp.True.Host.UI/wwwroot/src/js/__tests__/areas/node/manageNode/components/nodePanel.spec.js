import setup from '../../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { constants } from '../../../../../common/services/constants';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import NodePanel, { NodePanel as NodePanelComponent } from '../../../../../modules/administration/node/manageNode/components/nodePanel.jsx';
import { navigationService } from '../../../../../common/services/navigationService';
import { networkBuilderService } from '../../../../../common/services/networkBuilderService';

function mountWithRealStore() {
    const data = {
        initialValues: { name: 'test' },
        node: { manageNode: { node: {} } },
        tabs: { nodePanel: true },
        nodeGraphicalConnection: { showCreateNodePanel: true }
    };

    const sharedInitialState = {
        groupedCategoryElements: [],
        logisticCenters: [],
        units: []
    };

    const reducers = {
        node: jest.fn(() => data.node),
        shared: jest.fn(() => sharedInitialState),
        tabs: jest.fn(() => data.tabs),
        nodeGraphicalConnection: jest.fn(() => data.nodeGraphicalConnection)
    };

    const props = {
        mode: constants.Modes.Create
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <NodePanel route={{ mode: 'create' }} initialValues={data.initialValues} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Node Panel', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
        navigationService.getParamByName = jest.fn();
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should get node on mount if nodeid is in route', () => {
        const props = {
            mode: constants.Modes.Update,
            node: { },
            route: { mode: constants.Modes.Update },
            validateNode: jest.fn(),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn(),
            setFailureState: jest.fn(() => Promise.resolve()),
            resetState: jest.fn(),
            removeUnsavedNode: jest.fn(),
            showNodePanel: jest.fn(),
            getNode: jest.fn()
        };
        navigationService.getParamByName = jest.fn(() => '123');
        const wrapper = shallow(<NodePanelComponent {...props} />);
        expect(props.getNode.mock.calls).toHaveLength(1);
        expect(navigationService.getParamByName.mock.calls).toHaveLength(2);
    });

    it('should get node on mount if nodeid is not in route', () => {
        const props = {
            mode: constants.Modes.Update,
            filterValues: {},
            node: { name: 'name' },
            route: { mode: constants.Modes.Update },
            validateNode: jest.fn(() => Promise.resolve()),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn(),
            getNode: jest.fn()
        };
        navigationService.getParamByName = jest.fn();
        const wrapper = shallow(<NodePanelComponent {...props} />);
        expect(props.validateNode.mock.calls).toHaveLength(1);
        expect(props.getLogisticCenters.mock.calls).toHaveLength(1);
        expect(props.getCategoryElements.mock.calls).toHaveLength(1);
        expect(navigationService.getParamByName.mock.calls).toHaveLength(0);
    });

    it('should cancel in create mode', () => {
        const props = {
            mode: constants.Modes.Create,
            node: { name: 'name' },
            route: { mode: constants.Modes.Create },
            validateNode: jest.fn(),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn(),
            setFailureState: jest.fn(() => Promise.resolve()),
            resetState: jest.fn(),
            removeUnsavedNode: jest.fn(),
            showNodePanel: jest.fn(),
            getNode: jest.fn()
        };
        jest.useFakeTimers();
        const wrapper = shallow(<NodePanelComponent {...props} />);
        wrapper.instance().onCancel();
        expect(props.setFailureState.mock.calls).toHaveLength(1);
        expect(props.resetState.mock.calls).toHaveLength(1);
        expect(setTimeout).toHaveBeenLastCalledWith(expect.any(Function), 100);
    });

    it('should cancel in update mode', () => {
        const props = {
            mode: constants.Modes.Update,
            node: { name: 'name' },
            route: { mode: constants.Modes.Update },
            validateNode: jest.fn(),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn(),
            setFailureState: jest.fn(() => Promise.resolve()),
            resetState: jest.fn(),
            removeUnsavedNode: jest.fn(),
            showNodePanel: jest.fn(),
            getNode: jest.fn()
        };
        jest.useFakeTimers();
        const wrapper = shallow(<NodePanelComponent {...props} />);
        wrapper.instance().onCancel();
        expect(props.showNodePanel.mock.calls).toHaveLength(1);
    });

    it('should go back', () => {
        const props = {
            mode: constants.Modes.Update,
            node: { name: 'name' },
            route: { mode: constants.Modes.Update },
            validateNode: jest.fn(),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn(),
            persist: jest.fn(() => Promise.resolve()),
            getNode: jest.fn()
        };
        const wrapper = shallow(<NodePanelComponent {...props} />);
        wrapper.instance().onBack();
        expect(props.persist.mock.calls).toHaveLength(1);
    });

    it('should enable disable button on isvalid', () => {
        const props = {
            mode: constants.Modes.Update,
            node: { name: 'name' },
            route: { mode: constants.Modes.Update },
            validateNode: jest.fn(),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn(),
            enableDisableSubmit: jest.fn(() => Promise.resolve()),
            getNode: jest.fn()
        };
        const wrapper = shallow(<NodePanelComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { isValid: true }));
        expect(props.enableDisableSubmit.mock.calls).toHaveLength(1);
    });

    it('should navigate to manage on refresh', () => {
        const props = {
            mode: constants.Modes.Update,
            node: { name: 'name' },
            validateNode: jest.fn(),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn(),
            route: { mode: constants.Modes.Update },
            enableDisableSubmit: jest.fn(() => Promise.resolve()),
            getNode: jest.fn()
        };
        navigationService.navigateTo = jest.fn();
        const wrapper = shallow(<NodePanelComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { refreshToggler: true }));
        expect(navigationService.navigateTo.mock.calls).toHaveLength(1);
    });

    it('should show error in popup on failure', () => {
        const props = {
            mode: constants.Modes.Update,
            node: { name: 'name' },
            validateNode: jest.fn(),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn(),
            route: { mode: constants.Modes.Update },
            enableDisableSubmit: jest.fn(() => Promise.resolve()),
            confirm: jest.fn(),
            showNodePanel: jest.fn()
        };
        const wrapper = shallow(<NodePanelComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { failureToggler: true }));
        expect(props.confirm.mock.calls).toHaveLength(1);
        expect(props.showNodePanel.mock.calls).toHaveLength(1);
    });

    it('should get graphical network', () => {
        const props = {
            mode: constants.Modes.Update,
            node: { name: 'name', segment: {} },
            resetState: jest.fn(),
            getGraphicalNetwork: jest.fn(),
            route: { mode: constants.Modes.Update },
            validateNode: jest.fn(),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn(),
            graphicalNodeFilters: { elementId: 'id' }
        };
        networkBuilderService.initialize = jest.fn();
        const wrapper = shallow(<NodePanelComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { nodeSavedToggler: true }));
        expect(props.resetState.mock.calls).toHaveLength(1);
        expect(props.getGraphicalNetwork.mock.calls).toHaveLength(1);
    });

    it('should get graphical node', () => {
        const props = {
            mode: constants.Modes.Create,
            node: { name: 'name', segment: {} },
            resetState: jest.fn(),
            getGraphicalNode: jest.fn(),
            route: { mode: constants.Modes.Update },
            validateNode: jest.fn(),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn()
        };
        networkBuilderService.initialize = jest.fn();
        const wrapper = shallow(<NodePanelComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { nodeSavedToggler: true }));
        expect(props.resetState.mock.calls).toHaveLength(1);
        expect(props.getGraphicalNode.mock.calls).toHaveLength(1);
    });

    it('should validate node and hide error in case of update', () => {
        const props = {
            mode: constants.Modes.Update,
            node: { name: 'name', segment: {} },
            validNode: true,
            hideError: jest.fn(),
            showError: jest.fn(),
            route: { mode: constants.Modes.Update },
            validateNode: jest.fn(),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn()
        };
        const wrapper = shallow(<NodePanelComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { validNodeToggler: true }));
        expect(props.hideError.mock.calls).toHaveLength(1);
    });

    it('should validate node and show error in case of update', () => {
        const props = {
            mode: constants.Modes.Update,
            node: { name: 'name', segment: {} },
            validNode: false,
            hideError: jest.fn(),
            showError: jest.fn(),
            route: { mode: constants.Modes.Update },
            validateNode: jest.fn(),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn()
        };
        const wrapper = shallow(<NodePanelComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { validNodeToggler: true }));
        expect(props.showError.mock.calls).toHaveLength(1);
    });

    it('should request node storage locations on toggle of update', () => {
        const props = {
            mode: constants.Modes.Update,
            node: { name: 'name', segment: {} },
            requestNodeStorageLocations: jest.fn(),
            route: { mode: constants.Modes.Update },
            validateNode: jest.fn(),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn()
        };
        const wrapper = shallow(<NodePanelComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { updateToggler: true }));
        expect(props.requestNodeStorageLocations.mock.calls).toHaveLength(1);
        expect(props.validateNode.mock.calls).toHaveLength(2);
        expect(props.getLogisticCenters.mock.calls).toHaveLength(2);
        expect(props.getCategoryElements.mock.calls).toHaveLength(2);
    });

    it('should save node on save', () => {
        const props = {
            mode: constants.Modes.Update,
            node: { name: 'name', segment: {}, operator: {}, nodeType: {} },
            nodeStorageLocations: [{ products: [] }],
            requestCreateUpdateNode: jest.fn(),
            route: { mode: constants.Modes.Update },
            validateNode: jest.fn(),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn()
        };
        const wrapper = shallow(<NodePanelComponent {...props} />);
        wrapper.instance().onSave(props.node);
        expect(props.requestCreateUpdateNode.mock.calls).toHaveLength(1);
    });

    it('should process submit on nodeGeneralInfo tab', () => {
        const props = {
            mode: constants.Modes.Update,
            node: { name: 'name', segment: {}, operator: {}, nodeType: {} },
            nodeStorageLocations: [{ products: [] }],
            requestCreateUpdateNode: jest.fn(),
            route: { mode: constants.Modes.Update },
            validateNode: jest.fn(),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn(),
            submit: jest.fn(),
            tabs: { activeTab: 'nodeGeneralInfo' }
        };
        const wrapper = shallow(<NodePanelComponent {...props} />);
        wrapper.instance().processSubmit();
        expect(props.submit.mock.calls).toHaveLength(1);
        expect(props.requestCreateUpdateNode.mock.calls).toHaveLength(0);
    });

    it('should process submit on warehouse tab', () => {
        const props = {
            mode: constants.Modes.Update,
            node: { name: 'name', segment: {}, operator: {}, nodeType: {} },
            nodeStorageLocations: [{ products: [] }],
            requestCreateUpdateNode: jest.fn(),
            route: { mode: constants.Modes.Update },
            validateNode: jest.fn(),
            getLogisticCenters: jest.fn(),
            getCategoryElements: jest.fn(),
            tabs: { activeTab: 'warehouse' }
        };
        const wrapper = shallow(<NodePanelComponent {...props} />);
        wrapper.instance().processSubmit();
        expect(props.requestCreateUpdateNode.mock.calls).toHaveLength(1);
    });
});
