import Wizard from './../../../../common/components/wizard/wizard';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { mount } from 'enzyme';
import React from 'react';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import { httpService } from './../../../../common/services/httpService';
import setup from '../../../areas/setup.js';

class WizardComponentA extends React.Component {
    render() {
        return (
            <div>Wizard Component A</div>
        );
    }
}

class WizardComponentB extends React.Component {
    render() {
        return (
            <div>Wizard Component B</div>
        );
    }
}

const data = { TestWizard: { activeStep: 1, totalSteps: 2 } };

function mountWithRealStore() {
    const reducers = {
        wizard: jest.fn(() => data)
    };

    const store = createStore(combineReducers(reducers));
    const props = {
        wizardName: 'TestWizard',
        config: {
            wizardName: 'TestWizard',
            activeStep: 1,
            isPage: true,
            clickable: false,
            wizardSteps: [
                {
                    title: 'Component 1',
                    component: WizardComponentA
                },
                {
                    title: 'Component 2',
                    component: WizardComponentB
                }
            ],
            disabledWizardStepsClick: true
        }
    };

    const enzymeWrapper = mount(<Provider store={store}><Wizard {...props} /></Provider>);
    return { store, enzymeWrapper, props };
}

describe('wizard', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount wizard', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should mount wizard component', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find('.ep-wizard__body').children().length).toEqual(1);
    });
});
