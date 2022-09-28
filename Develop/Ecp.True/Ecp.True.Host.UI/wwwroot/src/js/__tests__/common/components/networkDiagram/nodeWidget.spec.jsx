import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { reducer as formReducer } from 'redux-form';
import { shallow } from 'enzyme';
import React from 'react';
// eslint-disable-next-line no-unused-vars
import setup from '../../../areas/setup';
import { networkBuilderService } from '../../../../common/services/networkBuilderService.js';
import NetworkDiagramNodeWidget, { NetworkDiagramNodeWidget as NetworkDiagramNodeWidgetComponent } from '../../../../common/components/NetworkDiagram/nodeWidget.jsx';

function mountWithRealStore() {
    const nodeGraphicalConnection = {
        graphicalNetwork: {
            sourceNode: {
                out_sourceNode: [
                    {
                        destinationNodeId: 'someId'
                    }
                ],
                nodeName: 'sourceNodeName'
            },
            destinationNode: {
                nodeName: 'destinationNodeName'
            },
            someId: {
                name: 'someName'
            }
        }
    };

    const linkId = 'sourceNode-destinationNode';
    const reducers = {
        form: formReducer,
        nodeGraphicalConnection: jest.fn(() => nodeGraphicalConnection)
    };
    const store = createStore(combineReducers(reducers));
    const props = {
        createConnectionToDelete: jest.fn(),
        deleteNodeConnection: jest.fn(),
        openConfirmModal: jest.fn(),
        linkId: linkId,
        nodeGraphicalNetwork: nodeGraphicalConnection.graphicalNetwork,
        node: 'someId',
        getPort: jest.fn(),
        nodeDetails: 'some details',
        requestUpdateNode: jest.fn(),
        initGraphicalUpdateNode: jest.fn(),
        requestNodeStorageLocations: jest.fn(),
        showNodePanel: jest.fn()
    };

    const enzymeWrapper = shallow(<Provider store={store}><NetworkDiagramNodeWidget {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('networkDiagramNodeWidget', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should trigger component did update for prop changes', () => {
        const props = {
            node: {
                getPort: jest.fn()
            },
            getErrorNodeDetailsToggler: true,
            confirm: jest.fn()
        };
        const wrapper = shallow(<NetworkDiagramNodeWidgetComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { getErrorNodeDetailsToggler: false }));
        expect(props.confirm.mock.calls).toHaveLength(1);
    });

    it('should trigger onRepaintAndUpdateNetworkModel', () => {
        const props = {
            node: {
                getPort: jest.fn()
            },
            nodeDetails: {
                nodeId: 'id'
            },
            selectedNodeId: 'id',
            receiveGraphicalNetworkDataToggler: true,
            confirm: jest.fn(),
            updateNetworkModel: jest.fn(),
            onInputAndOutputClicked: jest.fn()
        };
        const networkBuilderServiceMock = networkBuilderService.onRepaintCanvas = jest.fn();
        const wrapper = shallow(<NetworkDiagramNodeWidgetComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { receiveGraphicalNetworkDataToggler: false }));
        expect(networkBuilderServiceMock.mock.calls).toHaveLength(1);
    });

    it('should trigger selectNodeAndConnections', () => {
        const props = {
            node: {
                getPort: jest.fn()
            },
            nodeDetails: {
                nodeId: 'id'
            },
            selectedNodeId: 'id',
            inputOutputClickedToggler: true,
            confirm: jest.fn(),
            updateNetworkModel: jest.fn(),
            onInputAndOutputClicked: jest.fn(),
            selectNode: jest.fn(),
            updateModelConnections: jest.fn(),
            modelConnections: [
                {
                    name: 'someName'
                }
            ]
        };
        const networkBuilderServiceMock = networkBuilderService.selectNodeAndConnection = jest.fn();
        const wrapper = shallow(<NetworkDiagramNodeWidgetComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { inputOutputClickedToggler: false }));
        expect(props.selectNode.mock.calls).toHaveLength(1);
        expect(props.updateModelConnections.mock.calls).toHaveLength(1);
        expect(networkBuilderServiceMock.mock.calls).toHaveLength(1);
    });

    it('should facilitate on mouse over', () => {
        const props = {
            node: {
                getPort: jest.fn()
            },
            nodeDetails: {
                nodeId: 'id',
                inputConnections: [
                    {
                        id: 'someId'
                    }
                ]
            },
            selectedNodeId: 'id',
            inputOutputClickedToggler: true,
            confirm: jest.fn(),
            updateNetworkModel: jest.fn(),
            onInputAndOutputClicked: jest.fn(),
            selectNode: jest.fn(),
            updateModelConnections: jest.fn(),
            modelConnections: [
                {
                    name: 'someName'
                }
            ],
            hideAllSourceNodeDetails: jest.fn()
        };
        const networkBuilderServiceMock = networkBuilderService.onRepaintCanvas = jest.fn();
        const wrapper = shallow(<NetworkDiagramNodeWidgetComponent {...props} />);
        wrapper.find('#ep-diagram__node-port').simulate('click', { stopPropagation: jest.fn(), event: { target: '', stopPropagation: jest.fn() } });
        expect(networkBuilderServiceMock.mock.calls).toHaveLength(0);
    });
});
