import React from 'react';
import Enzyme, { mount } from 'enzyme';
import EnzymeAdapter from 'enzyme-adapter-react-16';
import ModulePage from '../../../common/router/modulePage';
import { Provider } from 'react-redux';
import { navigationService } from '../../../common/services/navigationService';
import { scenarioService } from '../../../common/services/scenarioService';
import { resourceProvider } from '../../../common/services/resourceProvider';
import configureStore from 'redux-mock-store';
import { dispatcher } from '../../../common/store/dispatcher';

Enzyme.configure({ adapter: new EnzymeAdapter() });

function TestComponent() {
    return (<div key="first_component" />);
}

function render(args) {
    const configureMockStore = configureStore();
    const props = {
        history: 'history',
        location: 'location',
        match: {},
        route: {
            routeKey: 'route',
            component: TestComponent,
            bcrumbsKey: 'someKey'
        },
        root: {
            currentModule: 'categories'
        }
    };

    const reducers = {
        closeModal: jest.fn(() => Promise.resolve)
    };
    const store = configureMockStore(props);

    const enzymeWrapper = mount(<Provider store={store}>
        <ModulePage {...props} />
    </Provider>);
    return { store, enzymeWrapper };
}

it('should render Module page', () => {
    dispatcher.dispatch = jest.fn();
    navigationService.getModulePath = jest.fn(() => 'somePath');
    scenarioService.getFeature = jest.fn(module => 'node');
    resourceProvider.read = jest.fn(someName => 'name');
    const wrapper = render();
    expect(wrapper).toBeTruthy();
    expect(wrapper.enzymeWrapper).toHaveLength(1);
    expect(wrapper.enzymeWrapper.find(ModulePage).length).toEqual(1);
});
