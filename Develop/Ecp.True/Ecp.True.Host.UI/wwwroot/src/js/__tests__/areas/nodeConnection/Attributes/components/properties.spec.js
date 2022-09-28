import setup from '../../../setup';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import Properties, { Properties as PropertiesComponent } from '../../../../../modules/administration/nodeConnection/attributes/components/properties.jsx';
import { ModalFooter } from '../../../../../common/components/footer/modalFooter.jsx';

function mountWithRealStore(newState = {}) {
    const data = {
        initialValues: {
            connectionProduct: {
                nodeConnectionId: 2,
                nodeConnectionProductId: 2,
                owners: [],
                priority: 4,
                product: { name: 'CRUDOS IMPORTADOS' },
                productId: '10000002049',
                uncertaintyPercentage: 1.28
            }
        },
        nodeConnection: {
            attributes: {
                connectionProduct: {
                }
            }
        },
        shared: {}
    };

    const reducers = {
        nodeConnection: jest.fn(() => Object.assign({}, data.nodeConnection, newState)),
        shared: jest.fn(() => data.shared),
        onClose: jest.fn(() => Promise.resolve())
    };

    const props = {
        closeModal: jest.fn(() => Promise.resolve),
        handleSubmit: jest.fn({ values: {} })
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <Properties initialValues={data.initialValues.connectionProduct} />
    </Provider>);
    return { store, enzymeWrapper, props };
}

describe('Node Connection Properties', () => {
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
        expect(enzymeWrapper.exists('#txt_decimal_priority')).toBe(true);
        expect(enzymeWrapper.find('#btn_connAttributes_info_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find('#btn_connAttributes_info_submit').text()).toEqual('submit');
    });

    it('should give error when form submitted and values are not given', () => {
        const props = {
            refreshGrid: jest.fn(),
            closeModal: jest.fn(),
            productToggler: true,
            handleSubmit: jest.fn()
        };

        const wrapper = shallow(<PropertiesComponent {...props} />);
        wrapper.find('form').at(0).simulate('submit');
        expect(props.handleSubmit.mock.calls).toHaveLength(1);
    });

    it('should call onClose on click of cancel button', () => {
        const props = {
            refreshGrid: jest.fn(),
            closeModal: jest.fn(),
            productToggler: true,
            handleSubmit: jest.fn()
        };

        const { enzymeWrapper } = mountWithRealStore(props);
        enzymeWrapper.find(ModalFooter).find('#btn_connAttributes_info_cancel').at(0).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call component did update when props get updated', () => {
        const props = {
            refreshGrid: jest.fn(),
            closeModal: jest.fn(),
            productToggler: true,
            handleSubmit: jest.fn()
        };

        const wrapper = shallow(<PropertiesComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { productToggler: false }));
        expect(props.refreshGrid.mock.calls).toHaveLength(1);
        expect(props.closeModal.mock.calls).toHaveLength(1);
    });
});

