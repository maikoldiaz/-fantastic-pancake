import setup from '../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { httpService } from '../../../../common/services/httpService';
import LogisticsCriteria, { LogisticsCriteria as LogisticsCriteriaComponent } from '../../../../modules/transportBalance/logistics/components/logisticsCriteria.jsx';
import { ModalFooter } from '../../../../common/components/footer/modalFooter.jsx';

const logistics = {
    operational: {
        refreshToggler: true,
        lastOwnershipDate: {},
        initialDate: {},
        refreshDateToggler: true
    }
};

const shared = {
    groupedCategoryElements: [],
    categoryElements: []
};

const props = {
    onSelectNode: jest.fn(),
    componentType: 3
};

function mountWithRealStore(newState = {}) {
    const reducers = {
        closeModal: jest.fn(() => Promise.resolve),
        logistics: jest.fn(() => Object.assign({}, logistics, newState)),
        shared: jest.fn(() => shared)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <LogisticsCriteria {...props} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('LogisticsCriteria', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('find Control', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#dd_logisticsCriteria_segment')).toBe(true);
        expect(enzymeWrapper.exists('#txt_logisticsCriteria_node')).toBe(true);
        expect(enzymeWrapper.exists('#r_logisticsCriteria_owner')).toBe(true);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_logisticsCriteria_cancel').text()).toEqual(resourceProvider.read('cancel'));
        expect(enzymeWrapper.find(ModalFooter).find('#btn_logisticsCriteria_submit').text()).toEqual(resourceProvider.read('next'));
    });

    it('should call onClose on click of cancel button', () => {
        const newProps = {
            onSelectNode: jest.fn(),
            handleSubmit: jest.fn(),
            owners: [{
                elementId: 'someId',
                name: 'name'
            }],
            getCategoryElements: jest.fn(),
            closeModal: jest.fn()
        };
        const { enzymeWrapper } = mountWithRealStore(newProps);
        enzymeWrapper.find('#btn_logisticsCriteria_cancel').at(0).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should submit form on submit button click', () => {
        const props = {
            onSelectNode: jest.fn(),
            handleSubmit: jest.fn(),
            owners: [{
                elementId: 'someId',
                name: 'name'
            }],
            getCategoryElements: jest.fn(),
            closeModal: jest.fn(),
            hideError: jest.fn(),
            clearSearchedNodes: jest.fn()
        };
        const wrapper = shallow(<LogisticsCriteriaComponent {...props} />);
        wrapper.find('#dd_logisticsCriteria_segment').at(0).simulate('change');
        expect(props.hideError.mock.calls).toHaveLength(2);
        expect(props.clearSearchedNodes.mock.calls).toHaveLength(1);
    });

    it('should request search node', () => {
        const props = {
            onSelectNode: jest.fn(),
            handleSubmit: jest.fn(),
            owners: [{
                elementId: 'someId',
                name: 'name'
            }],
            getCategoryElements: jest.fn(),
            hideError: jest.fn()
        };
        const wrapper = shallow(<LogisticsCriteriaComponent {...props} />);
        wrapper.find('#txt_logisticsCriteria_node').at(0).simulate('select');
        expect(props.onSelectNode.mock.calls).toHaveLength(1);
    });

    it('should select node', () => {
        const props = {
            onSelectNode: jest.fn(),
            handleSubmit: jest.fn(),
            owners: [{
                elementId: 'someId',
                name: 'name'
            }],
            getCategoryElements: jest.fn(),
            hideError: jest.fn()
        };
        const wrapper = shallow(<LogisticsCriteriaComponent {...props} />);
        wrapper.find('#txt_logisticsCriteria_node').at(0).simulate('select');
        expect(props.onSelectNode.mock.calls).toHaveLength(1);
    });
});
