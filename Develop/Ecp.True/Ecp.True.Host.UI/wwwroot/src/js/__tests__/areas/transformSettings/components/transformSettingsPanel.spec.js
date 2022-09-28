import setup from '../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import TransformSettingsPanel from '../../../../modules/administration/transformSettings/components/transformSettingsPanel.jsx';
import { routerActions } from '../../../../common/router/routerActions';
const initialValues = {
    tabs: {
        transformSettingsPanel: { activeTab: 'movement' }
    }
};

function mountWithRealStore() {
    const reducers = {
        onTransform: jest.fn(() => Promise.resolve),
        tabs: jest.fn(() => initialValues.tabs)
    };
    const props = {
        route: {
            mode: 'create'
        }
    };
    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <TransformSettingsPanel initialValues={initialValues} {...props} />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('Transform Settings Panel', () => {
    beforeAll(() => {
        routerActions.configure = jest.fn(() => Promise.resolve);
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });
});
