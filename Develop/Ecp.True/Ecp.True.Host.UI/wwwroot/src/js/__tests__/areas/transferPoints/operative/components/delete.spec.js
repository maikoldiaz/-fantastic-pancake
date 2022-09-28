import setup from '../../../setup.js';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import DeleteTransferPointOperational from '../../../../../modules/administration/transferPoints/operative/components/delete.jsx';

const initialValues = {
    transferPointsOperational: {
        transferPoint: {
            transferPoint: 'RELACION_TT',
            sourceField: 'TestCreateUI',
            fieldWaterProduction: 'TestCreateUI',
            relatedSourceField: 'TestCreateUI',
            destinationNode: 'Automation_hb9um',
            destinationNodeType: 'Automation_z965a',
            movementType: 'Venta',
            sourceNode: 'ARAGUANEY - OBC',
            sourceNodeType: 'Limite',
            sourceProduct: 'CRUDO CAMPO CUSUCO',
            sourceProductType: 'TEST Rule'
        },
        deleteSuccess: false
    },
    modal: {
        modalKey: ''
    }
};

function mountWithRealStore() {
    const reducers = {
        closeModal: jest.fn(() => Promise.resolve),
        transferPointsOperational: jest.fn(() => initialValues.transferPointsOperational),
        modal: jest.fn(() => initialValues.modal)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <DeleteTransferPointOperational initialValues={initialValues} />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('DeleteTransferPointOperational', () => {
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
        expect(enzymeWrapper.exists('#lbl_transferPointOperational_name')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointOperational_transferPoint')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointOperational_movementType')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointOperational_nodeOrigin')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointOperational_nodeDestination')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointOperational_nodeOriginType')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointOperational_nodeDestinationType')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointOperational_product')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointOperational_productType')).toBe(true);
        expect(enzymeWrapper.find('#btn_deleteOperative_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find('#btn_deleteOperative_submit').text()).toEqual('acceptTransferOperational');
    });

    it('should give error when click on save and name is not given', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_deleteOperative_submit').at(0).simulate('click');
        enzymeWrapper.find('form').find('#btn_deleteOperative_submit').simulate('submit');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call onClose on click of cancel button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_deleteOperative_cancel').at(0).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });
});
