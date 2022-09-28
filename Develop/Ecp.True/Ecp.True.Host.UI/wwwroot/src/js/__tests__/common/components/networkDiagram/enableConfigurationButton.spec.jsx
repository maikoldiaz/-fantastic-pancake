import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { reducer as formReducer } from 'redux-form';
import { mount } from 'enzyme';
import React from 'react';
// eslint-disable-next-line no-unused-vars
import setup from '../../../areas/setup';
import EnableConfigurationButton from '../../../../common/components/networkDiagram/enableConfigurationButton';

function mountWithRealStore() {
    const nodeGraphicalConnection = {
        graphicalNetwork: {
            sourceNode: {
                    out_sourceNode:[
                        {
                            destinationNodeId: 'someId'
                        }
                    ],
                    nodeName: 'sourceNodeName'
                },
            destinationNode: {
                nodeName: 'destinationNodeName'
            }
        }
    };
    
    const linkId = "sourceNode-destinationNode";
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
        nodeGraphicalNetwork: nodeGraphicalConnection.graphicalNetwork
    };

    const enzymeWrapper = mount(<Provider store={store}><EnableConfigurationButton {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('enableConfigurationButton', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should click to enable configuration', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(`#btnEnableConnection_sourceNode-destinationNode`).exists()).toEqual(true);
        enzymeWrapper.find(`#btnEnableConnection_sourceNode-destinationNode`).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });
});
