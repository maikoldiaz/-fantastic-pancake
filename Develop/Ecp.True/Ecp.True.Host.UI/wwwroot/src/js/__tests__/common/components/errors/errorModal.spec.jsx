import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import setup from '../../../areas/setup';
import ErrorModal from '../../../../common/components/errors/errorModal.jsx';
import { supportConfigService } from '../../../../common/services/supportConfigService';

let props = {};

function mountWithRealStore() {
    const reducers = jest.fn();
    const store = createStore(reducers);
    const enzymeWrapper = mount(<Provider store={store}><ErrorModal {...props} /></Provider>);
    return { enzymeWrapper, props, store };
}

describe('ErrorModal', () => {
    it('should mount successfully', () => {
        supportConfigService.getAutoServicePortalLink = jest.fn();
        supportConfigService.getChatbotServiceLink = jest.fn();
        supportConfigService.getAttentionLinePhoneNumber = jest.fn();
        supportConfigService.getAttentionLinePhoneNumberExtension = jest.fn();
        supportConfigService.getAttentionLineEmail = jest.fn();

        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });
});
