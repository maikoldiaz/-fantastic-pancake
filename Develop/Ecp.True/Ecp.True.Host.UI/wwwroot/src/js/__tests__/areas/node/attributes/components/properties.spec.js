import setup from '../../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import Properties, { Properties as PropertiesComponent } from '../../../../../modules/administration/node/attributes/components/properties.jsx';
import { ModalFooter } from '../../../../../common/components/footer/modalFooter.jsx';

function mountWithRealStore() {
    const data = {
        initialValues: {
            nodeProduct: {
                isActive: true,
                nodeStorageLocation: { name: '' },
                nodeStorageLocationId: 336,
                owners: [],
                product: { name: '' },
                productId: '',
                storageLocationProductId: 3,
                uncertaintyPercentage: 99.93
            }
        },
        node: {
            attributes: {
                nodeProduct: {}
            }
        },
        shared: {
            rules: {
                3: [{ ruleType: 1 }]
            },
            groupedCategoryElements: {
                10: [{ name: 'some Name' }]
            }
        }
    };

    const reducers = {
        node: jest.fn(() => data.node),
        shared: jest.fn(() => data.shared),
        onClose: jest.fn(() => Promise.resolve())
    };

    const props = {
        closeModal: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn()
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <Properties initialValues={data.initialValues.nodeProduct}/>
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Node Properties', () => {
    beforeAll(() => {
        resourceProvider.read = jest.fn(key => key);
        httpService.getCurrentCulture = jest.fn(() => 'en-Us');
    });

    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('find Controls', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
        expect(enzymeWrapper.exists('#dd_nodeAttributes_to')).toBe(true);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_nodeAttributes_properties_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find(ModalFooter).find('#btn_nodeAttributes_properties_submit').text()).toEqual('submit');
    });

    it('should give error when form submitted and values are not given', () => {
        const props = {
            refreshGrid: jest.fn(),
            closeModal: jest.fn(),
            productToggler: true,
            handleSubmit: jest.fn(),
            groupedCategoryElements: {
                10: [{ name: 'some Name' }]
            },
            initialValues: {
                ownershipRule: { name: 'someName' }
            },
            getCategoryElements: jest.fn()
        };

        const wrapper = shallow(<PropertiesComponent {...props} />);
        wrapper.find('form').simulate('submit');
        expect(props.handleSubmit.mock.calls).toHaveLength(1);
    });

    it('should call onClose on click of cancel button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find(ModalFooter).find('#btn_nodeAttributes_properties_cancel').simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call component did update when props get updated', () => {
        const props = {
            refreshGrid: jest.fn(),
            closeModal: jest.fn(),
            productToggler: true,
            handleSubmit: jest.fn(),
            groupedCategoryElements: {
                10: [{ name: 'some Name' }]
            },
            initialValues: {
                ownershipRule: { name: 'someName' }
            },
            getCategoryElements: jest.fn()
        };

        const wrapper = shallow(<PropertiesComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { productToggler: false }));
        expect(props.refreshGrid.mock.calls).toHaveLength(1);
        expect(props.closeModal.mock.calls).toHaveLength(1);
    });
});

