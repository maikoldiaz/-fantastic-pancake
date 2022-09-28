import setup from '../../../setup.js';
import React from 'react';
import { mount } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import DeleteTransferPointLogistic from '../../../../../modules/administration/transferPoints/logistics/components/delete.jsx';

const initialValues = {
    transferPointsLogistics: {
        transferPoint: {
            toperativeNodeRelationshipWithOwnershipId: 55881,
            sourceProduct: "GASOLINA MOTOR REGULAR IMPORTADA",
            sourceStorageLocation: null,
            destinationProduct: "CRUDO CAÑO LIMON",
            destinationStorageLocation: null,
            transferPoint: "na",
            sourceSystem: "CSV",
            loadDate: "2020-03-05T23:12:09.483Z",
            executionID: "669c338f-918f-42cb-884d-8e19f74840be",
            sourceNode: null,
            destinationNode: null,
            logisticSourceCenter: "TR POZOS COLORADOS-GALAN 14:INV TRANSITO",
            logisticDestinationCenter: "TR AYACUCHO:MATERIA PRIMA",
            notes: "notesnotes",
            isDeleted: true
        },
        deleteToggler: false
    },
    modal: {
        modalKey: ""
    }
};

function mountWithRealStore() {
    const reducers = {
        closeModal: jest.fn(() => Promise.resolve),
        transferPointsLogistics: jest.fn(() => initialValues.transferPointsLogistics),
        modal: jest.fn(() => initialValues.modal)
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <DeleteTransferPointLogistic initialValues={initialValues} />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('DeleteTransferPointLogistic', () => {
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
        expect(enzymeWrapper.exists('#lbl_transferPointLogistics_name')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointLogistics_transferPoint')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointLogistics_logisticSourceCenter')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointLogistics_logisticDestinationCenter')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointLogistics_sourceProduct')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointLogistics_destinationProduct')).toBe(true);
        expect(enzymeWrapper.exists('#txtarea_transferPointLogistics_notes')).toBe(true);
        expect(enzymeWrapper.find('#btn_transferPointLogisticsDelete_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find('#btn_transferPointLogisticsDelete_submit').text()).toEqual('acceptTransferOperational');
    });

    it('should give error when click on save and name is not given', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_transferPointLogisticsDelete_submit').at(0).simulate('click');
        enzymeWrapper.find('form').find('#btn_transferPointLogisticsDelete_submit').simulate('submit');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should call onClose on click of cancel button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find('footer').find('#btn_transferPointLogisticsDelete_cancel').at(0).simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });
});