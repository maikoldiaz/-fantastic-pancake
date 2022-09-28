import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import setup from '../../../areas/setup';
import { Error } from '../../../../common/components/errors/error.jsx';


let props = { errorCode: '404'};

function mountWithRealStore() {
    const reducers = jest.fn();
    const store = createStore(reducers);
    const enzymeWrapper = mount(<Provider store={store}><Error {...props} /></Provider>);
    return { enzymeWrapper, props, store };
}

describe('Error', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });
});
