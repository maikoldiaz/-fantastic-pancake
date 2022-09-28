import setup from "../../../setup";
import React from "react";
import { Provider } from "react-redux";
import { combineReducers, createStore } from "redux";
import AssociationSavedModal from "../../../../../modules/administration/productConfiguration/components/storageLocationProduct/associationSavedModal.jsx";
import { mount } from "enzyme";

function mountWithRealStore() {
    const data = {
        products: {
            associationsCreated: [{}]
        }
    };

    const reducers = {
        products: jest.fn(() => data.products)
    };

    const props = {
        closeModal: jest.fn()
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <AssociationSavedModal {...props} />
    </Provider>);

    return { store, enzymeWrapper, props }
}

describe('association saved modal', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });
});