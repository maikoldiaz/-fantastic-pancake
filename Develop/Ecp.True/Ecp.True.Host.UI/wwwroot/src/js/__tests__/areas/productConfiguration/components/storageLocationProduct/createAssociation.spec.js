import setup from '../../../setup';
import React from "react";
import { mount, shallow } from 'enzyme';
import { Provider } from 'react-redux';
import { combineReducers, createStore } from 'redux';
import CreateAssociation, { CreateAssociation as CreateAssociationComponent } from '../../../../../modules/administration/productConfiguration/components/storageLocationProduct/createAssociation.jsx';
import { navigationService } from '../../../../../common/services/navigationService';

function mountWithRealStore() {
    const data = {
        shared: {
            logisticCenters: []
        },
        products: {
            products: [],
            newAssociation: {},
            associationsCreated: []
        },
        root: {
            systemConfig: {
                maxProductStorageLocationMappingCreationEdition: 1
            }
        }
    };

    const reducers = {
        shared: jest.fn(() => data.shared),
        products: jest.fn(() => data.products),
        root: jest.fn(() => data.root)
    };

    const props = {
        handleSubmit: (callback) => callback({ associations: [] }),
        initialValues: {
            associations: [{ name: 'center one' }, { name: 'center two' }]
        },
        createAssociations: jest.fn()
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store}>
        <CreateAssociation {...props} />
    </Provider>);

    return { store, enzymeWrapper, props };
}


describe('create association storage - product', () => {
    it('should mount successfully', () => {
        const { enzymeWrapper } = mountWithRealStore();
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should handle form submit', () => {
        const onSubmit = jest.fn()
        const props = {
            handleSubmit: jest.fn(() => onSubmit),
            getLogisticsCenter: jest.fn(),
            getProducts: jest.fn(),
            changeTabPanel: jest.fn()
        };

        let wrapper = shallow(<CreateAssociationComponent {...props} />)
        wrapper.instance().onSubmit = onSubmit;
        wrapper.find('form').simulate('submit');
        expect(props.handleSubmit.mock.calls).toHaveLength(1);
        expect(wrapper.instance().onSubmit).toBeCalled();

    });

    it('should handle cancel button and redirect', () => {
        const props = {
            handleSubmit: jest.fn(),
            getLogisticsCenter: jest.fn(),
            getProducts: jest.fn(),
            changeTabPanel: jest.fn(),
            getFieldNameFromForm: jest.fn(() => null),
            maxAssociationCreationEdition: 1
        };
        const navigateMock = navigationService.navigateTo = jest.fn();

        const wrapper = shallow(<CreateAssociationComponent {...props} />)
        wrapper.find('#btn_associations_cancel').simulate('click');

        expect(navigateMock).toBeCalled();
    });

    it('should handle cancel button and open modal', () => {
        const props = {
            handleSubmit: jest.fn(),
            getLogisticsCenter: jest.fn(),
            getProducts: jest.fn(),
            changeTabPanel: jest.fn(),
            getFieldNameFromForm: jest.fn(() => [{
                logisticsCenter: 'test',
                storageLocation: 'test',
                product: 'test'
            }]),
            maxAssociationCreationEdition: 1,
            openModal: jest.fn()
        };

        const wrapper = shallow(<CreateAssociationComponent {...props} />)
        wrapper.find('#btn_associations_cancel').simulate('click');

        expect(wrapper.instance().props.openModal).toBeCalled();
    });

    it('should handle cancel button and open modal', () => {
        const props = {
            handleSubmit: jest.fn(),
            getLogisticsCenter: jest.fn(),
            getProducts: jest.fn(),
            changeTabPanel: jest.fn(),
            getFieldNameFromForm: jest.fn(() => [{
                storageLocation: 'test',
                product: 'test'
            }]),
            maxAssociationCreationEdition: 1,
            openModal: jest.fn()
        };

        const wrapper = shallow(<CreateAssociationComponent {...props} />)
        wrapper.find('#btn_associations_cancel').simulate('click');

        expect(wrapper.instance().props.openModal).toBeCalled();
    });

    it('should handle cancel button and open modal', () => {
        const props = {
            handleSubmit: jest.fn(),
            getLogisticsCenter: jest.fn(),
            getProducts: jest.fn(),
            changeTabPanel: jest.fn(),
            getFieldNameFromForm: jest.fn(() => [{
                product: 'test'
            }]),
            maxAssociationCreationEdition: 1,
            openModal: jest.fn()
        };

        const wrapper = shallow(<CreateAssociationComponent {...props} />)
        wrapper.find('#btn_associations_cancel').simulate('click');

        expect(wrapper.instance().props.openModal).toBeCalled();
    });

    it('should handle submit logistic center', () => {
        const props = {
            handleSubmit: callback => callback({ 
                associations: [{ name: 'center one' }, { name: 'center two' }]
            }),
            createAssociations: jest.fn(() => Promise.resolve())
        };

        const { enzymeWrapper } = mountWithRealStore(props);
        enzymeWrapper.find('form').simulate('submit');
        expect(props.createAssociations.mock.calls).toHaveLength(0);
    });

    it('should component update', () => {
        const props = {
            createToggler: false,
            openModalAssosiationsSaved: jest.fn(),
            handleSubmit: jest.fn(),
            getLogisticsCenter: jest.fn(),
            getProducts: jest.fn(),
            changeTabPanel: jest.fn()
        };
    
        const wrapper = shallow(<CreateAssociationComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, { createToggler: !props.createToggler }));
        expect(props.openModalAssosiationsSaved.mock.calls).toHaveLength(1);
      });

      it('should component unmount', () => {
        const props = {
            createToggler: false,
            clearAllStorageList: jest.fn(),
            handleSubmit: jest.fn(),
            getLogisticsCenter: jest.fn(),
            getProducts: jest.fn(),
            changeTabPanel: jest.fn()
        };
    
        const wrapper = shallow(<CreateAssociationComponent {...props} />);
        wrapper.unmount();
        expect(props.clearAllStorageList.mock.calls).toHaveLength(1);
      });
});