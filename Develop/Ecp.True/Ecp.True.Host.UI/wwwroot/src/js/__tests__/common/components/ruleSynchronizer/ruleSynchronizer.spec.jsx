import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { reducer as formReducer } from 'redux-form';
import { mount, shallow } from 'enzyme';
import React from 'react';
// eslint-disable-next-line no-unused-vars
import setup from '../../../areas/setup';
import RuleSynchronizer, { RuleSynchronizer as RuleSynchronizerComponent} from '../../../../common/components/ruleSynchronizer/ruleSynchronizer';
import { constants } from '../../../../common/services/constants';

function mountWithFailureStatus(state, enabled) {
    const ruleSynchronizerTest = {
        ruleSynchronizer:
        {
            node: {
                state: constants.SyncStatus.Failed,
                enabled: false
            }
        }
    };

    const reducers = {
        form: formReducer,
        ruleSynchronizer: jest.fn(() => ruleSynchronizerTest.ruleSynchronizer)
    };
    const store = createStore(combineReducers(reducers));
    const props = {
        name: 'node',
        initSyncRules: jest.fn(() => {

            return 1;
        }),
        requestSyncProgress: jest.fn(() => {
            return 1;
        }),
        syncRules: jest.fn(() => {
            return 1;
        }),
        showError: jest.fn(() => {
            return 1;
        }),

    };

    const enzymeWrapper = mount(<Provider store={store}><RuleSynchronizer {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

function mountWithSuccessStatus(state, enabled) {
    const ruleSynchronizerTest = {
        ruleSynchronizer:
        {
            node: {
                state: constants.SyncStatus.Success,
                enabled: true
            }
        }
    };

    const reducers = {
        form: formReducer,
        ruleSynchronizer: jest.fn(() => ruleSynchronizerTest.ruleSynchronizer)
    };
    const store = createStore(combineReducers(reducers));
    const props = {
        name: 'node',
        initSyncRules: jest.fn(() => {

            return 1;
        }),
        requestSyncProgress: jest.fn(() => {
            return 1;
        }),
        syncRules: jest.fn(() => {
            return 1;
        }),
        showError: jest.fn(() => {
            return 1;
        }),

    };

    const enzymeWrapper = mount(<Provider store={store}><RuleSynchronizer {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

function mountWithNotReadyStatus(state, enabled) {
    const ruleSynchronizerTest = {
        ruleSynchronizer:
        {
            node: {
                state: constants.SyncStatus.NotReady,
                enabled: true
            }
        }
    };

    const reducers = {
        form: formReducer,
        ruleSynchronizer: jest.fn(() => ruleSynchronizerTest.ruleSynchronizer)
    };
    const store = createStore(combineReducers(reducers));
    const props = {
        name: 'node',
        initSyncRules: jest.fn(() => {

            return 1;
        }),
        requestSyncProgress: jest.fn(() => {
            return 1;
        }),
        syncRules: jest.fn(() => {
            return 1;
        }),
        showError: jest.fn(() => {
            return 1;
        }),

    };

    const enzymeWrapper = mount(<Provider store={store}><RuleSynchronizer {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('ruleSynchronizer', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithFailureStatus();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should refresh cache with error', () => {
        const { enzymeWrapper } = mountWithFailureStatus();
        expect(enzymeWrapper.find(`#ruleSynchronizer_error`).exists()).toEqual(true);
        enzymeWrapper.find(`#ruleSynchronizer_error`).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should sync rules for success status', () => {
        const { enzymeWrapper, props } = mountWithSuccessStatus();
        expect(enzymeWrapper.find(`button`).exists()).toEqual(true);
        enzymeWrapper.find(`button`).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
        expect(props.syncRules.mock.calls).toHaveLength(1);
    });

    it('should sync rules for not ready status', () => {
        const { enzymeWrapper, props } = mountWithNotReadyStatus();
        expect(enzymeWrapper.find(`button`).exists()).toEqual(true);
        enzymeWrapper.find(`button`).simulate('click');
        expect(props.syncRules.mock.calls).toHaveLength(1);
    });

    it('should call component did update for changed props', () => {
        const props = {
            timerToggler: true,
            requestSyncProgress: jest.fn(),
            initSyncRules: jest.fn()
        }
        const wrapper = shallow(<RuleSynchronizerComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { timerToggler: false }));
        expect(props.requestSyncProgress.mock.calls).toHaveLength(0);
    });

    it('should call show error on error toggler update', () => {
        const props = {
            errorToggler: true,
            requestSyncProgress: jest.fn(),
            showError: jest.fn(),
            initSyncRules: jest.fn()
        }
        const wrapper = shallow(<RuleSynchronizerComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { errorToggler: false }));
        expect(props.showError.mock.calls).toHaveLength(1);
    });
});