import setup from '../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import Annulation, { Annulation as AnnulationComponent } from '../../../../modules/administration/annulation/components/annulation.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';

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
        refreshGrid: jest.fn(() => Promise.resolve)
    };
    const store = createStore(combineReducers(reducers));
    const enzymeWrapper = mount(<Provider store={store} >
        <Annulation initialValues={initialValues} {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Annulation', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });
    it('should call handleSubmit method when form submits and it should be called with correct values', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.find(ModalFooter).find('#btn_annulation_submit').simulate('click');
        expect(props.handleSubmit.mock.calls).toHaveLength(1);
    });
    it('should unmount component', () => {
        const { enzymeWrapper, props } = mountWithRealStore();
        enzymeWrapper.unmount();
        expect(props.closeModal.mock.calls).toHaveLength(0);
    });
    it('should call onClose on click of cancel button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find(ModalFooter).find('#btn_annulation_cancel').simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });
    it('should close modal and refresh grid on create success', () => {
        const props = {
            groupedElements: [],
            saveSuccess: false,
            handleSubmit: jest.fn(() => Promise.resolve),
            refreshGrid: jest.fn(),
            closeModal: jest.fn(),
            initTypes: jest.fn()
        };

        const wrapper = shallow(<AnnulationComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { saveSuccess: true }));
        expect(props.refreshGrid.mock.calls).toHaveLength(1);
        expect(props.closeModal.mock.calls).toHaveLength(1);
    });
    it('should initialize types in create mode', () => {
        const props = {
            groupedElements: [],
            saveSuccess: false,
            mode: constants.Modes.Create,
            handleSubmit: jest.fn(() => Promise.resolve),
            refreshGrid: jest.fn(),
            closeModal: jest.fn(),
            initTypes: jest.fn()
        };

        const wrapper = shallow(<AnnulationComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { saveSuccess: true }));
        expect(props.initTypes.mock.calls).toHaveLength(1);
    });
    it('should not initialize types in update mode', () => {
        const props = {
            groupedElements: [],
            saveSuccess: false,
            mode: constants.Modes.Update,
            handleSubmit: jest.fn(() => Promise.resolve),
            refreshGrid: jest.fn(),
            closeModal: jest.fn(),
            initTypes: jest.fn()
        };

        const wrapper = shallow(<AnnulationComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { saveSuccess: true }));
        expect(props.initTypes.mock.calls).toHaveLength(0);
    });
    it('should call save annulation', () => {
        const props = {
            groupedElements: [],
            saveSuccess: false,
            mode: constants.Modes.Update,
            handleSubmit: jest.fn(() => Promise.resolve),
            refreshGrid: jest.fn(),
            closeModal: jest.fn(),
            initTypes: jest.fn(),
            saveAnnulation: jest.fn(),
            values: {
                source: { movement: { elementId: 0 }, node: 0, product: 0 },
                annulation: { movement: { elementId: 0 }, node: 0, product: 0 }
            },
            initialValues: {}
        };

        const wrapper = shallow(<AnnulationComponent {...props} />);
        wrapper.instance().saveAnnulation(props.values);
        expect(props.saveAnnulation.mock.calls).toHaveLength(1);
    });
});
