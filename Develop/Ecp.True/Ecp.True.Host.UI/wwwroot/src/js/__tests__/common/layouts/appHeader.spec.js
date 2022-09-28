import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { reducer as formReducer } from 'redux-form';
import { mount } from 'enzyme';
import React from 'react';
// eslint-disable-next-line no-unused-vars
import setup from '../../areas/setup';
import AppHeader from '../../../common/layouts/appHeader.jsx';
import { dateService } from '../../../common/services/dateService';

function mountWithRealStore(userId = 'test') {
    const appReady = {
        appReady: true,
        appReadyToggler: false,
        context: {
            name: 'test',
            userId
        },
        scenarios: {}
    };
    const reducers = {
        form: formReducer,
        root: jest.fn(() => appReady)
    };
    const store = createStore(combineReducers(reducers));

    const props = {
        root: appReady
    };

    const enzymeWrapper = mount(<Provider store={store}><AppHeader {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('app Header', () => {
    beforeAll(() => {
        dateService.wishMe = jest.fn(() => 'good morning');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should render user name when context is valid', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find('#spn_user_name')).toHaveLength(1);
        expect(enzymeWrapper.find('#spn_user_name').text()).toEqual('good morning, test');
    });

    it('should not render user name when context is not valid', () => {
        const { enzymeWrapper } = mountWithRealStore(null);
        expect(enzymeWrapper.find('#spn_user_name')).toHaveLength(1);
        expect(enzymeWrapper.find('#spn_user_name').text()).toEqual('good morning');
    });

    it('should logout onClick of Logout', () =>{
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find('#lnk_user_logout').exists()).toEqual(true);
        enzymeWrapper.find('#lnk_user_logout').simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });
});
