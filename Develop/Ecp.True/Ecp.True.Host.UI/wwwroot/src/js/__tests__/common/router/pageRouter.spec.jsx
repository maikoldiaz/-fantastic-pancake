import React from 'react';
import Enzyme, { mount, shallow } from 'enzyme';
import EnzymeAdapter from 'enzyme-adapter-react-16';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import PageRouter from '../../../common/router/pageRouter';
import ModulePage from '../../../common/router/pageRouter';
import { authService } from '../../../common/services/authService';
import setup from '../../areas/setup';
import { navigationService } from '../../../common/services/navigationService';

Enzyme.configure({ adapter: new EnzymeAdapter() });
function render(args) {
    const configureMockStore = configureStore();
    const defaultProps = {
        history: 'history',
        location: 'location',
        match: {},
        route: {
            actions: [],
            routeKey: 'route'
        },
        routerConfig: {
            getPageRoles: () => true,
            getPageNames: () => []
        }
    };

    const props = { ...defaultProps, ...args };
    const store = configureMockStore(defaultProps);
    store.dispatch = jest.fn();
    const enzymeWrapper = mount(<Provider store={store} >
        <PageRouter {...props} />
    </Provider>);

    return { store, enzymeWrapper };
}

it('should render page router page', () => {
    authService.isAuthorized = jest.fn(() => false);
    navigationService.handleError = jest.fn();
    const wrapper = render();
    expect(wrapper).toBeTruthy();
    expect(wrapper.enzymeWrapper).toHaveLength(1);
    expect(wrapper.enzymeWrapper.find(PageRouter).length).toEqual(1);
    expect(wrapper.enzymeWrapper.find(ModulePage).length).toEqual(1);
});

it('should call initialize of navigation service', () => {
    const props = {
        history: 'history',
        location: 'location',
        match: {},
        route: {
            actions: [],
            routeKey: 'route'
        },
        routerConfig: {
            getPageRoles: () => true,
            getPageNames: jest.fn(() => ['page1', 'page2']),
            getPageDetails: jest.fn(() => { return { id: 'someId' }; }),
            buildNavPath: jest.fn(),
            getPageRoute: jest.fn(() => { return 'someRoute'; })
        }
    };

    const authServiceMock = authService.isAuthorized = jest.fn(() => true);
    const navigationServiceMock = navigationService.initialize = jest.fn();
    const wrapper = shallow(<PageRouter {...props} />);
    expect(wrapper).toBeTruthy();
    expect(authServiceMock.mock.calls).toHaveLength(1);
});
