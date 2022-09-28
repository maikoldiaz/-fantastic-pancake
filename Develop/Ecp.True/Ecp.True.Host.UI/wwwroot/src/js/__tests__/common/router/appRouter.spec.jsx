import React from 'react';
import Enzyme, { mount } from 'enzyme';
import EnzymeAdapter from 'enzyme-adapter-react-16';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import AppRouter from '../../../common/router/appRouter';

Enzyme.configure({ adapter: new EnzymeAdapter() });
function render(args) {
    const configureMockStore = configureStore();
    const defaultProps = {
        history: 'history',
        location: 'location',
        match: {},
        isOpen: 'false',
        scenarios: {},
        root: {
            isOpen: 'false',
            scenarios: {}
        },
        notification: {
            message: 'someMessage',
            state: 'someState',
            show: 'someShow'
        },
        notificationButton: {
            invokeNotificationButtonToggler: 'false'
        },
        route: {
            actions: [],
            routeKey: 'route',
            component: 'TicketDetail',
            isOpen: 'false'
        }
    };

    const props = { ...defaultProps, ...args };
    const store = configureMockStore(defaultProps);
    store.dispatch = jest.fn();
    const enzymeWrapper = mount(<Provider store={store} >
        <AppRouter {...props} />
    </Provider>);
    return { store, enzymeWrapper };
}

it('should render app router page', () => {
    const wrapper = render();
    expect(wrapper).toBeTruthy();
    expect(wrapper.enzymeWrapper).toHaveLength(1);
    expect(wrapper.enzymeWrapper.find(AppRouter).length).toEqual(1);
});
