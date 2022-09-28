import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { reducer as formReducer } from 'redux-form';
import { mount } from 'enzyme';
import React from 'react';
// eslint-disable-next-line no-unused-vars
import setup from '../../areas/setup';
import PageActions from '../../../common/router/pageActions';

function mountWithRealStore() {
    const defaultProps = {
        history: 'history',
        location: 'location',
        match: {},
        pageActions: [
            {
                title: 'title1',
                type: 'Button',
                actionType: 'modal'
            },
            {
                title: 'title2',
                type: 'Dropdown',
                actionType: 'modal',
                options: ['option1', 'option2']
            }
        ],
        showModal: jest.fn(() => Promise.resolve)
    };

    const controlState = {
            disabledActions: [
                {
                        title: 'title2',
                        type: 'Button',
                        actionType: 'flyout'
                }
            ],
            hiddenActions: [],
            showModal: jest.fn(() => Promise.resolve),
    }

    const reducers = {
        pageActions: jest.fn(() => controlState)
    };
    const store = createStore(combineReducers(reducers));
    const props = {
        onClose: jest.fn(() => {
            return 1;
        }),
        showErrors: jest.fn(() => {
            return 1;
        }),
        showModal: jest.fn(() => Promise.resolve),
        actions: defaultProps.pageActions,
        controlState: controlState
    };

    const enzymeWrapper = mount(<Provider store={store}><PageActions {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('page actions', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should open modal', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        props.showModal = jest.fn();
        enzymeWrapper.find(`#btn_title1`).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });
});
