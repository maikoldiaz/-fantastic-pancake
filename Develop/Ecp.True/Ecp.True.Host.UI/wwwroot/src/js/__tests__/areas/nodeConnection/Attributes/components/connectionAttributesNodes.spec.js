import setup from '../../../setup';
import React from "react";
import { mount, shallow } from 'enzyme';
import { Provider } from 'react-redux';
import { combineReducers, createStore } from 'redux';
import ConnectionAttributesNodes, { ConnectionAttributesNodes as ConnectionAttributesNodesComponent } from '../../../../../modules/administration/nodeConnection/attributes/components/connectionAttributesNodes.jsx';
import { navigationService } from '../../../../../common/services/navigationService';


function mountWithRealStore() {
    const data = {
        shared: {
            groupedCategoryElements: []
        },
        nodeConnection: {
            attributes: {
                newConnections: {}
            },
            nodeCostCenters: {
                duplicates: []
            }
        },
        root: {
            systemConfig: {
                maxNodeConnectionCreationEdition: 1
            }
        }
    };

    const reducers = {
        shared: jest.fn(() => data.shared),
        nodeConnection: jest.fn(() => data.nodeConnection),
        root: jest.fn(() => data.root)
    };

    const props = {};

    const store = createStore(combineReducers(reducers));



    const enzymeWrapper = mount(<Provider store={store}>
        <ConnectionAttributesNodes {...props} />
    </Provider>);

    return { store, enzymeWrapper, props };
}


describe('Connection attributes nodes', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should handle form submit', () => {
        // ConnectionAttributesNodesComponent.onSubmit = jest.fn()
        const onSubmit = jest.fn()
        const props = {
            handleSubmit: jest.fn(() => onSubmit),
            getCategoryElements: jest.fn(),
            createNodeConnection: jest.fn()
        };

        let wrapper = shallow(<ConnectionAttributesNodesComponent {...props} />)
        wrapper.instance().onSubmit = onSubmit;
        wrapper.find('form').simulate('submit');
        expect(props.handleSubmit.mock.calls).toHaveLength(1);
        expect(wrapper.instance().onSubmit).toBeCalled();

    });


    it('should handle cancel button and redirect', () => {
        const props = {
            handleSubmit: jest.fn(),
            getCategoryElements: jest.fn(),
            getFieldNameFromForm: jest.fn(() => []),
            maxNodeConnectionCreationEdition: 1
        };
        const navigateMock = navigationService.navigateTo = jest.fn();

        const wrapper = shallow(<ConnectionAttributesNodesComponent {...props} />)
        wrapper.find('#btn_nodeConnections_cancel').simulate('click');

        expect(navigateMock).toBeCalled();
    });

    it('should handle cancel button and open modal', () => {
        const props = {
            handleSubmit: jest.fn(),
            getCategoryElements: jest.fn(),
            getFieldNameFromForm: jest.fn(() => [{
                sourceSegment: 'test',
                sourceNode: 'test',
                destinationSegment: 'test',
                destinationNode: 'test'
            }]),
            maxNodeConnectionCreationEdition: 1,
            openModal: jest.fn()
        };

        const wrapper = shallow(<ConnectionAttributesNodesComponent {...props} />)
        wrapper.find('#btn_nodeConnections_cancel').simulate('click');

        expect(wrapper.instance().props.openModal).toBeCalled();
    });

    it('should handle node connections duplicates - notification', () => {
        const props = {
            handleSubmit: jest.fn(),
            getCategoryElements: jest.fn(),
            getFieldNameFromForm: jest.fn(() => []),
            isNodeConnectionDuplicatesNotified: false,
            notifyNodeCostCenterDuplicates: jest.fn(),
            openNodeCostCenterDuplicatesModal: jest.fn()
        };

        const wrapper = shallow(<ConnectionAttributesNodesComponent {...props} />)
        wrapper.setProps(Object.assign({}, props, {
            nodeConnectionDuplicates: [{
                sourceSegment: 'test',
                sourceNode: 'test',
                destinationSegment: 'test',
                destinationNode: 'test'
            }]
        }));
        expect(wrapper.instance().props.notifyNodeCostCenterDuplicates).toBeCalled();
    });

});