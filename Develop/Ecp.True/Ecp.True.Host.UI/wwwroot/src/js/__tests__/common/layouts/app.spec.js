import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { reducer as formReducer } from 'redux-form';
import { mount, shallow } from 'enzyme';
import React from 'react';
// eslint-disable-next-line no-unused-vars
import setup from '../../areas/setup';
import { AppComponent, App as AppComponentTest } from '../../../common/layouts/app.jsx';
import { systemConfigService } from '../../../common/services/systemConfigService';
import { sessionService } from '../../../common/services/sessionService';
import { navigationService } from '../../../common/services/navigationService';
import { bootstrapService } from '../../../common/services/bootstrapService';

function mountWithRealStore() {
    const appReady = {
        appReady: true,
        appReadyToggler: false,
        context: {
            name: 'test',
            userId: 'Id1'
        },
        scenarios: {}
    };

    const notifications = {
        isShow: false
    };
    const notificationButton = {
        invokeNotificationButtonToggler: false
    };
    const modal = {};

    const loaderInitialState = {
        showLoader: false
    };

    const sharedInitialState = {
        categories: [],
        categoryElements: [],
        rules: {},
        progressStatus: {
            categoryElements: 0
        },
        registrationActionTypes: {
            1: 'insert',
            2: 'update',
            3: 'delete',
            4: 'reinject'
        }
    };

    const reducers = {
        form: formReducer,
        root: jest.fn(() => appReady),
        notification: jest.fn(() => notifications),
        notificationButton: jest.fn(() => notificationButton),
        modal: jest.fn(() => modal),
        loader: jest.fn(() => loaderInitialState),
        shared: jest.fn(() => sharedInitialState)
    };

    const store = createStore(combineReducers(reducers));

    const props = {
        root: appReady
    };

    const enzymeWrapper = mount(<Provider store={store}><AppComponent {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('app', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should bootstrap and set app ready', () => {
        navigationService.isNotAuthorized = jest.fn(() => false);
        const props = {
            appReady: jest.fn(),
            ready: false,
            appToggler: true,
            bootstrapApp: jest.fn()
        };
        bootstrapService.bootstrap = jest.fn();
        
        const wrapper = shallow(<AppComponentTest {...props} />);

        expect(props.appReady.mock.calls).toHaveLength(0);
        expect(props.bootstrapApp.mock.calls).toHaveLength(1);

        wrapper.setProps(Object.assign({}, props, { ready: false, appToggler: false }));
        expect(props.appReady.mock.calls).toHaveLength(1);        
        expect(bootstrapService.bootstrap.mock.calls.length).toBe(1);
    });

    it('should not bootstrap and set app ready when unauthorized', () => {
        navigationService.isNotAuthorized = jest.fn(() => true);
        const props = {
            appReady: jest.fn(),
            ready: false,
            appToggler: true,
            bootstrapApp: jest.fn()
        };
        bootstrapService.bootstrap = jest.fn();

        const wrapper = shallow(<AppComponentTest {...props} />);

        expect(props.appReady.mock.calls).toHaveLength(1);
        expect(props.bootstrapApp.mock.calls).toHaveLength(0);
    });
});
