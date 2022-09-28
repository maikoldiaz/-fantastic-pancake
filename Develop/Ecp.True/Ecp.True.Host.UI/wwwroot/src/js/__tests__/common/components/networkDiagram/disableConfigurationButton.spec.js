import setup from '../../../areas/setup';
import React from 'react';
import { mount } from 'enzyme';
import configureStore from 'redux-mock-store';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import DisableConfigurationButton from '../../../../common/components/NetworkDiagram/deleteConfigurationButton';
import { networkBuilderService } from '../../../../common/services/networkBuilderService.js';

const initialState = {
    linkId: "1-2-3",
    nodeGraphicalConnection: {
        graphicalNetwork: [
            {NodeOne: { "nodeName" : "Abc"}}
         ],
    }
};

function mountWithMockStore(defaultState) {
    const mockStore = configureStore();
    const store = mockStore(defaultState);
    const enzymeWrapper = mount(<Provider store={store}><DisableConfigurationButton/></Provider>);
    return { store, enzymeWrapper };
}

describe('Disable configuration button', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        networkBuilderService.getColor = jest.fn();
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithMockStore(initialState);
        expect(enzymeWrapper).toHaveLength(1);
    });
});