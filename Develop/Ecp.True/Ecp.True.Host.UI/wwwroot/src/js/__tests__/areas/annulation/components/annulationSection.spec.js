import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { reduxForm } from 'redux-form';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import AnnulationSection, { AnnulationSection as AnnulationSectionComponent } from '../../../../modules/administration/annulation/components/annulationSection.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';

const initialValues = {
    annulations: {
        initialValues: {
            isActive: true
        },
        createSuccess: false,
        source: { movement: 0, node: 0, product: 0 },
        annulation: { movement: 0, node: 0, product: 0 },
        fieldChange: {}
    },
    modal: {
        modalKey: ''
    },
    shared: {
        groupedCategoryElements: [[], [], [], [], [], [], [], [], [], []],
        originTypes: []
    }
};

function mountWithRealStore() {
    const reducers = {
        saveReversal: jest.fn(() => Promise.resolve),
        annulations: jest.fn(() => initialValues.annulations),
        modal: jest.fn(() => initialValues.modal),
        shared: jest.fn(() => initialValues.shared)
    };
    const props = {
        closeModal: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn(),
        refreshToggler: false,
        refreshGrid: jest.fn(()=> Promise.resolve),
        name: 'source'
    };
    const store = createStore(combineReducers(reducers));

    const AnnulationSectionForm = reduxForm({
        form: 'annulationsForm'
    })(AnnulationSection);

    const enzymeWrapper = mount(<Provider store={store} >
        <AnnulationSectionForm initialValues={initialValues} {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('AnnulationSectionForm', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should be able to select movement', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_source_movement')).toBe(true);
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should be able to select node', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_source_node')).toBe(true);
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should be able to select product', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper.exists('#dd_source_product')).toBe(true);
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should handle field update', () => {
        const props = {
            saveSuccess: false,
            handleSubmit: jest.fn(() => Promise.resolve),
            refreshGrid: jest.fn(),
            closeModal: jest.fn(),
            initTypes: jest.fn(),
            saveReversal: jest.fn(),
            dispatchActions: jest.fn(() => Promise.resolve()),
            fieldChange: { fieldChangeToggler: false },
            movementTypes: [],
            originTypes: []
        };

        const wrapper = shallow(<AnnulationSectionComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { fieldChange: {
            fieldChangeToggler: true,
            currentModifiedField: 'source.movement',
            currentModifiedValue: { elementId: 1 }
        } }));
        expect(props.dispatchActions.mock.calls).toHaveLength(1);
    });

    it('should disable movement in update mode', () => {
        const props = {
            saveSuccess: false,
            mode: constants.Modes.Update,
            name: 'source',
            handleSubmit: jest.fn(() => Promise.resolve),
            refreshGrid: jest.fn(),
            closeModal: jest.fn(),
            initTypes: jest.fn(),
            saveReversal: jest.fn(),
            dispatchActions: jest.fn(() => Promise.resolve()),
            fieldChange: { fieldChangeToggler: false },
            movementTypes: [],
            originTypes: []
        };

        const wrapper = shallow(<AnnulationSectionComponent {...props} />);
        expect(wrapper.exists('#dd_source_movement')).toBe(true);
        expect(wrapper.find('#dd_source_movement').props()).toHaveProperty('isDisabled', true);
    });
    it('should enable movement in creat mode', () => {
        const props = {
            saveSuccess: false,
            mode: constants.Modes.Create,
            name: 'source',
            handleSubmit: jest.fn(() => Promise.resolve),
            refreshGrid: jest.fn(),
            closeModal: jest.fn(),
            initTypes: jest.fn(),
            saveReversal: jest.fn(),
            dispatchActions: jest.fn(() => Promise.resolve()),
            fieldChange: { fieldChangeToggler: false },
            movementTypes: [],
            originTypes: []
        };

        const wrapper = shallow(<AnnulationSectionComponent {...props} />);
        expect(wrapper.exists('#dd_source_movement')).toBe(true);
        expect(wrapper.find('#dd_source_movement').props()).toHaveProperty('isDisabled', false);
    });
});
