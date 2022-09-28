import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { reducer as formReducer } from 'redux-form';
import { mount } from 'enzyme';
import React from 'react';
// eslint-disable-next-line no-unused-vars
import setup from '../../../areas/setup';
import GridSelect from '../../../../common/components/grid/gridSelect';

function mountWithRealStore() {
    const reducers = {
        form: formReducer
    };
    const store = createStore(combineReducers(reducers));

    const props = {
        id: 1,
        selectType: 'checkbox',
        row: {},
        onClick: jest.fn(() => Promise.resolve)
    };

    const enzymeWrapper = mount(<Provider store={store}><GridSelect {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}
describe('gridSelect', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should click checkbox', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find(`#chk_1`).exists()).toEqual(true);
        enzymeWrapper.find(`#chk_1`).simulate('click');
        expect(props.onClick.mock.calls).toHaveLength(1);
    });
});
