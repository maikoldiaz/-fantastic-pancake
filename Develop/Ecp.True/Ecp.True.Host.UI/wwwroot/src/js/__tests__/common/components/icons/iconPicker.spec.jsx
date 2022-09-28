import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { mount } from 'enzyme';
import React from 'react';
// eslint-disable-next-line no-unused-vars
import setup from '../../../areas/setup';
import { IconPicker as IconPickerComponent } from '../../../../common/components/icons/iconPicker';

function mountWithRealStore() {
    const icons = [
        {
            name: 'someIconName',
            content: 'someIconContent'
        }
    ];

    const icon = {
        name: 'someIconName'
    };

    const reducers = {
        icons: jest.fn(() => icons)
    };
    const store = createStore(combineReducers(reducers));
    const props = {
        icons: icons,
        icon: icon,
        setIconId: jest.fn(() => Promise.resolve)
    };

    const enzymeWrapper = mount(<Provider store={store}><IconPickerComponent {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('icon picker', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should handle icon change', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        enzymeWrapper.find(`#icon_picker`).equals(true);
        enzymeWrapper.find(`#icon_picker`).simulate('change');
        expect(enzymeWrapper).toHaveLength(1);
    });
});
