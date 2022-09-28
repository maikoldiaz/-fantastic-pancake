import setup from '../../../setup';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { constants } from '../../../../../common/services/constants';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import CreateNode from '../../../../../modules/administration/node/manageNode/components/createNode';

function mountWithRealStore() {
    const data = {
        initialValues: {
            name: "test"
        },
        node: {
            manageNode: {
                node: {},
                existingNode: {
                    name: "someName"
                }
            }
        }
    };

    const sharedInitialState = {
        groupedCategoryElements: [],
        logisticCenters: [],
        units: []
    };

    const reducers = {
        node: jest.fn(() => data.node),
        shared: jest.fn(() => sharedInitialState),
    };

    const props = {
        mode: constants.Modes.Create
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <CreateNode initialValues={data.initialValues} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Create Node', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should mount Create Node Fields',() =>{
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#txt_createNode_name')).toBe(true);
        expect(enzymeWrapper.exists('#dd_createNode_type')).toBe(true);
        expect(enzymeWrapper.exists('#dd_createNode_operator')).toBe(true);
        expect(enzymeWrapper.exists('#dd_createNode_segment')).toBe(true);
        expect(enzymeWrapper.exists('#dd_createNode_logisticCenter')).toBe(true);
        expect(enzymeWrapper.exists('#txt_decimal_capacity')).toBe(true);
        expect(enzymeWrapper.exists('#dd_createNode_unit')).toBe(true);
        expect(enzymeWrapper.exists('#txt_decimal_order')).toBe(true);
    });
    it('should mount Create Node labels', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.find('.ep-label').at(7).text()).toEqual('unit');
        expect(enzymeWrapper.find('.ep-label').at(6).text()).toEqual('capacity-optional');
    });
    it('should not throw any error', () => {
        const { enzymeWrapper } = mountWithRealStore();
        const input = enzymeWrapper.find('form').find('#txt_decimal_order').at(5);
        input.simulate('change', { target: { value: 9999999 } });
        expect(enzymeWrapper.find('.ep-control__error-txt').exists()).toBeFalsy();
    });
});