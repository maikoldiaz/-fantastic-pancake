import React from 'react';
import { mount } from 'enzyme';
import { reducer as formReducer } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { navigationService } from '../../../../common/services/navigationService';
import { scenarioService } from '../../../../common/services/scenarioService';
import setup from '../../../areas/setup';
import { Breadcrumbs } from '../../../../common/components/breadcrumbs/breadcrumbs.jsx';

const feature = {
    scenario: 'Transport Balance', name: 'Cut off'
};
let props = { currentModule: null, route: { bcrumbsKey: 'someKey' } };

function mountWithRealStore() {
    const reducers = {
        form: formReducer
    };
    const store = createStore(combineReducers(reducers));
    const enzymeWrapper = mount(<Provider store={store}><Breadcrumbs {...props} /></Provider>);
    return { enzymeWrapper, props, store };
}

describe('Breadcrumbs', () => {
    it('should mount successfully', () => {
        navigationService.getModulePath = jest.fn(() => 'cutoff');
        scenarioService.getFeature = jest.fn(() => feature);
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should mount successfully without current module state', () => {
        props = { route: {} };
        navigationService.getModulePath = jest.fn(() => 'cutoff');
        scenarioService.getFeature = jest.fn(() => feature);
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find('.ep-bcrumbs')).toHaveLength(1);

        const elements = enzymeWrapper.find('.ep-bcrumbs__lst-lnk');
        expect(elements).toHaveLength(2);
        expect(elements.get(1).props.children).toEqual('Cut off');
    });

    it('should mount successfully with current module state', () => {
        props = { currentModule: 'currentModule', route: {} };
        navigationService.getModulePath = jest.fn(() => 'cutoff');
        scenarioService.getFeature = jest.fn(() => feature);
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.find('.ep-bcrumbs')).toHaveLength(1);

        const elements = enzymeWrapper.find('.ep-bcrumbs__lst-lnk');
        expect(elements).toHaveLength(2);
        expect(elements.get(0).props.children).toEqual('Transport Balance');
    });

    it('should call navigation service on feature click', () => {
        props = { currentModule: 'currentModule', route: {} };
        const navigationServiceMock = navigationService.navigateToModule = jest.fn(() => 'cutoff');
        scenarioService.getFeature = jest.fn(() => feature);
        const wrapper = mount(<Breadcrumbs {...props} />);
        wrapper.find('.ep-bcrumbs__lst-lnk').at(1).simulate('click');
        expect(navigationServiceMock.mock.calls).toHaveLength(1);
    });

    it('should call navigation service on feature click even if current module is empty', () => {
        props = { currentModule: ' ', route: {} };
        const navigationServiceMock = navigationService.navigateToModule = jest.fn(() => 'cutoff');
        scenarioService.getFeature = jest.fn(() => feature);
        const wrapper = mount(<Breadcrumbs {...props} />);
        wrapper.find('.ep-bcrumbs__lst-lnk').at(1).simulate('click');
        expect(navigationServiceMock.mock.calls).toHaveLength(1);
    });
});
