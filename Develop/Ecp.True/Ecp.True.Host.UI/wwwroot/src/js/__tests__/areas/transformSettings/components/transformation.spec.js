import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import Transformation, { Transformation as TransformationComponent } from '../../../../modules/administration/transformSettings/components/transformation.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
const initialValues = {
    tabs: {
        transformSettingsPanel: { activeTab: 'movement' }
    },
    transformSettings: {
        ready: true,
        transformation: {},
        initialValues: {},
        refreshToggler: true,
        nodesReadyToggler: true,
        destination: {
            sourceNodes: [{
                nodeId: 1,
                name: 'node 1'
            }],
            destinationNodes: [],
            sourceProducts: [],
            destinationProducts: [],
            units: [],
            searchedSourceNodes: [{
                nodeId: 1
            }]
        },
        origin: {
            sourceNodes: [{
                nodeId: 1,
                name: 'node 1'
            }],
            destinationNodes: [],
            sourceProducts: [],
            destinationProducts: [],
            units: [],
            searchedSourceNodes: [{
                nodeId: 1
            }]
        },
        initialValues: {
            origin: {
                sourceNode: { nodeId: 1, name: 'node 1' }
            },
            destination: {
                sourceNode: { nodeId: 1, name: 'node 1' }
            }
        }
    },
    shared: {
        groupedCategoryElements: []
    }
};

function mountWithRealStore() {
    const reducers = {
        onTransform: jest.fn(() => Promise.resolve),
        tabs: jest.fn(() => initialValues.tabs),
        transformSettings: jest.fn(() => initialValues.transformSettings),
        shared: jest.fn(() => initialValues.shared)
    };
    const props = {
        currentTab: 'movement',
        transformation: {
            transformationId: 10
        },
        mode: 'create',
        closeModal: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn(),
        refreshToggler: false,
        nodesReadyToggler: false,
        refreshGrid: jest.fn(()=> Promise.resolve)
    };
    const store = createStore(combineReducers(reducers));
    const enzymeWrapper = mount(<Provider store={store} >
        <Transformation initialValues={initialValues} {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Transformation Section', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });
    it('should call handleSubmit method when form submits and it should be called with correct values', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find('form').simulate('submit');
        expect(props.handleSubmit.mock.calls).toHaveLength(1);
    });
    it('should unmount component', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.unmount();
        expect(props.closeModal.mock.calls).toHaveLength(0);
    });
    it('should update component', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        const newProps = Object.assign({}, initialValues, {
            transformSettings: {
                ...initialValues.transformSettings,
                nodesReadyToggler: !initialValues.transformSettings.nodesReadyToggler,
                refreshToggler: !initialValues.transformSettings.refreshToggler
            }
        });
        enzymeWrapper.instance().componentDidUpdate(newProps);
        expect(props.closeModal.mock.calls).toHaveLength(0);
    });

    it('should update component for prop updates', () => {
        const props = {
            refreshToggler: true,
            refreshGrid: jest.fn(),
            closeModal: jest.fn(),
            handleSubmit: jest.fn(),
            getCategoryElements: jest.fn()
        };

        const wrapper = shallow(<TransformationComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { refreshToggler: false }));
        expect(props.refreshGrid.mock.calls).toHaveLength(1);
        expect(props.closeModal.mock.calls).toHaveLength(1);
    });

    it('should call get transformation info on prop updates', () => {
        const props = {
            nodesReadyToggler: true,
            refreshGrid: jest.fn(),
            closeModal: jest.fn(),
            mode: 'update',
            getTransformationInfo: jest.fn(),
            handleSubmit: jest.fn(),
            getCategoryElements: jest.fn()
        };

        const wrapper = shallow(<TransformationComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { nodesReadyToggler: false }));
        expect(props.getTransformationInfo.mock.calls).toHaveLength(1);
    });

    it('should call get source node on prop updates', () => {
        const props = {
            categoryElementsDataToggler: true,
            refreshGrid: jest.fn(),
            closeModal: jest.fn(),
            mode: 'update',
            getSourceNodes: jest.fn(),
            handleSubmit: jest.fn(),
            getCategoryElements: jest.fn()
        };

        const wrapper = shallow(<TransformationComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { categoryElementsDataToggler: false }));
        expect(props.getSourceNodes.mock.calls).toHaveLength(1);
    });
});
