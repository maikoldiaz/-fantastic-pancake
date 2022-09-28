import setup from '../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { reduxForm } from 'redux-form';
import TransformationSection from '../../../../modules/administration/transformSettings/components/transformationSection.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import { constants } from '../../../../common/services/constants';

const initialValues = {
    tabs: {
        transformSettingsPanel: { activeTab: 'movement' }
    },
    transformSettings: {
        infoToggler: true,
        origin: {
            sourceNodes: [{
                nodeId: 1,
                name: 'node 1'
            }],
            destinationNodes: [{
                nodeId: 1,
                name: 'node 1'
            }],
            sourceProducts: [{
                productId: 1,
                product: {
                    name: 'product 1'
                }
            }],
            destinationProducts: [{
                productId: 2,
                product: {
                    name: 'product 2'
                }
            }],
            units: [{
                elementId: 1,
                name: 'product 1'
            }],
            searchedSourceNodes: [{
                nodeId: 1
            }]
        },
        initialValues: {
            origin: {
                sourceNode: { nodeId: 1, name: 'node 1' },
                destinationNode: { nodeId: 1, name: 'node 1' }
            }
        }
    }
};

function mountWithRealStore() {
    const reducers = {
        onTransform: jest.fn(() => Promise.resolve),
        tabs: jest.fn(() => initialValues.tabs),
        transformSettings: jest.fn(() => initialValues.transformSettings),
        searchNodes: jest.fn(() => Promise.resolve),
        clearField: jest.fn(() => Promise.resolve),
        clearTransformationData: jest.fn(() => Promise.resolve),
        hideNotification: jest.fn(() => Promise.resolve)
    };
    const props = {
        name: 'origin',
        mode: constants.Modes.Update,
        sourceNode: { nodeId: 100 },
        activeTab: 'movements',
        selectSourceNode: jest.fn(() => Promise.resolve)
    };
    const store = createStore(combineReducers(reducers));
    const TransformationSectionForm = reduxForm({
        form: 'transformationForm'
    })(TransformationSection);
    const enzymeWrapper = mount(<Provider store={store} >
        <TransformationSectionForm initialValues={initialValues} {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Transformation Section', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should select source node', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find('#dd_origin_sourceNode').at(0).simulate('change', { target: { value: 'node 1' } });
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should be able to select source node', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_origin_sourceNode')).toBe(true);
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should be visible sourceProduct', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_origin_sourceProduct')).toBe(true);
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should be visible measurementUnit', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_origin_measurementUnit')).toBe(true);
        expect(enzymeWrapper).toHaveLength(1);
    });
});