import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { reducer as formReducer } from 'redux-form';
import { mount } from 'enzyme';
import React from 'react';
// eslint-disable-next-line no-unused-vars
import setup from '../../../areas/setup';
import TransferPointConfigurationButton from '../../../../common/components/networkDiagram/transferPointConfigurationButton';

function mountWithRealStore() {
    const nodeGraphicalConnection = {
        graphicalNetwork: {
            sourceNode: {
                out_sourceNode:[
                    {
                        destinationNodeId: 'someId'
                    }
                ],
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

    const enzymeWrapper = mount(<Provider store={store}><TransferPointConfigurationButton {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('transferPointConfigurationButton', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should click the transfer point configuration button', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(`#btnTransferConnection_sourceNode-destinationNode`).exists()).toEqual(true);
        enzymeWrapper.find(`#btnTransferConnection_sourceNode-destinationNode`).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });
});
