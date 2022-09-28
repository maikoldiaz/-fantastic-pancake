import setup from '../../setup.js';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { TooltipOverlay } from '../../../../modules/transportBalance/nodeOwnership/components/tooltipOverlay.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';

const initialValues = {
    data: {
        startDate: '01/01/2020',
        endDate: '05/01/2020',
        volume: 100,
        measurementUnitDetail: {
            name: 'Bbl'
        }
    }
};

function mountWithRealStore() {
    const state = {
        data: {
            startDate: '01/01/2020',
            endDate: '05/01/2020',
            volume: 100,
            measurementUnitDetail: {
                name: 'Bbl'
            }
        }
    };

    const reducers = {
        data: jest.fn(() => state.data)
    };

    const store = createStore(combineReducers(reducers));
    const props = Object.assign({}, initialValues);

    const enzymeWrapper = mount(<Provider store={store} {...props}>
        <TooltipOverlay name="tooltip" initialValues={initialValues} {...props}/>
    </Provider>);
    return { enzymeWrapper, props };
}

describe('tooltip overlay', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });
});
