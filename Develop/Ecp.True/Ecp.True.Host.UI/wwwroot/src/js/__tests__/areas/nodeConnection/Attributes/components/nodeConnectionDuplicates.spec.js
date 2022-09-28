import setup from "../../../setup";
import React from "react";
import { Provider } from "react-redux";
import { combineReducers, createStore } from "redux";
import NodeConnectionDuplicates from "../../../../../modules/administration/nodeConnection/attributes/components/nodeConnectionDuplicates.jsx";
import { mount } from "enzyme";

function mountWithRealStore() {
    const data = {
        nodeConnection: {
            nodeCostCenters: {
                totalToSaved: 0,
                totalUnSaved: 0
            }
        }
    };

    const reducers = {
        nodeConnection: jest.fn(() => data.nodeConnection)
    };

    const props = {};

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <NodeConnectionDuplicates {...props} />
    </Provider>);

    return { store, enzymeWrapper, props }
}

describe('node connection duplicates - modal', () => {

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();

        expect(enzymeWrapper).toHaveLength(1);

    });

});