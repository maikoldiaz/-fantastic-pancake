import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { reducer as formReducer } from 'redux-form';
import { mount, EnzymeAdapter } from 'enzyme';
import React from 'react';
// eslint-disable-next-line no-unused-vars
import setup from '../../../areas/setup';
import PopupFactory, { PopupFactory as PopupFactoryComponent } from '../../../../common/components/modal/popupFactory';

function mountWithRealStore() {
    const ownershipRules = {
        node: [
            {
                confirmToggler: true,
                updateToggler: true
            }
        ]
    }

    const state = { ownershipRules };

    const reducers = {
        ownershipRules: jest.fn(() => ownershipRules)
    };
    const store = createStore(combineReducers(reducers));
    const props = {
        openModal: jest.fn(() => {
            return 1;
        })
    };

    const enzymeWrapper = mount(<Provider store={store}><PopupFactory {...state } {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('pop up factory', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should open confirm modal successfully', () => {
        let callbackType;
        const props = {
            type: 'someType',
            openModal: jest.fn((type) => {
                callbackType = type;
            })
        };
        const wrapper = mount(<PopupFactoryComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { confirmToggler: false }));
        expect(props.openModal.mock.calls).toHaveLength(1);
        expect(callbackType).toEqual('someTypeConfirm');
    });

    it('should open bulk update modal successfully', () => {
        let callbackType;
        const props = {
            type: 'someType',
            openModal: jest.fn((type) => {
                callbackType = type;
            })
        };
        const wrapper = mount(<PopupFactoryComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { updateToggler: false }));
        expect(props.openModal.mock.calls).toHaveLength(1);
        expect(callbackType).toEqual('someTypeBulkUpdate');
    });
});
