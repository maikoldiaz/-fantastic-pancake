import React from 'react';
import Enzyme, { mount } from 'enzyme';
import EnzymeAdapter from 'enzyme-adapter-react-16';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import { navigationService } from '../../../common/services/navigationService';
import { scenarioService } from '../../../common/services/scenarioService';
import { resourceProvider } from '../../../common/services/resourceProvider';
import { bootstrapService } from '../../../common/services/bootstrapService';
import AppMenu from '../../../common/router/appMenu';

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
        <AppMenu {...props} />
    </Provider>);
    return { store, enzymeWrapper };
}

it('should render app menu page', () => {
    navigationService.getModulePath = jest.fn(() => 'somePath');
    scenarioService.getFeature = jest.fn(module => 'node');
    resourceProvider.read = jest.fn(someName => 'name');
    bootstrapService.getRoute = jest.fn(path => 'someRoute');
    const wrapper = render();
    expect(wrapper).toBeTruthy();
    expect(wrapper.enzymeWrapper).toHaveLength(1);
    expect(wrapper.enzymeWrapper.find(AppMenu).length).toEqual(1);
});
