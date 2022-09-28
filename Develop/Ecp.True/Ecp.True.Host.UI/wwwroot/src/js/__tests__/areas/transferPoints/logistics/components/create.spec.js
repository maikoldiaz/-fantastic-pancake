import setup from '../../../setup.js';
import { change } from 'redux-form';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import CreateTransferPointLogistic, { CreateTransferPointLogistic as CreateTransferPointLogisticComponent } from '../../../../../modules/administration/transferPoints/logistics/components/create.jsx';
import { actionProvider } from '../../../../../modules/administration/transferPoints/actionProvider';
import {
    getNodeStorageLocations,
    getTransferProducts,
    getLogisticCenter,
    resetOnSourceNodeChange,
    getTransferDestinationNodes,
    resetOnDestinationNodeChange
} from '../../../../../modules/administration/transferPoints/logistics/actions';
import { ModalFooter } from '../../../../../common/components/footer/modalFooter.jsx';

const initialValues = {
    transferPointsLogistics: {
        transferPoint: {
            operativeNodeRelationshipWithOwnershipId: 55881,
            sourceProduct: 'GASOLINA MOTOR REGULAR IMPORTADA',
            sourceStorageLocation: null,
            destinationProduct: 'CRUDO CAÑO LIMON',
            destinationStorageLocation: null,
            transferPoint: 'na',
            sourceSystem: 'CSV',
            loadDate: '2020-03-05T23:12:09.483Z',
            executionID: '669c338f-918f-42cb-884d-8e19f74840be',
            sourceNode: null,
            destinationNode: null,
            logisticSourceCenter: 'TR POZOS COLORADOS-GALAN 14:INV TRANSITO',
            logisticDestinationCenter: 'TR AYACUCHO:MATERIA PRIMA',
            notes: 'notesnotes',
            isDeleted: true
        },
        createToggler: false,
        fieldChangeToggler: false
    },
    modal: {
        modalKey: ''
    },
    sourceNodes: {},
    destinationNodes: {},
    sourceStorageLocations: {},
    destinationStorageLocations: {},
    sourceProducts: {},
    destinationProducts: {},
    sourceLogisticCenter: {},
    destinationLogisticCenter: {},
    sourceStorageLocation: null,
    destinationStorageLocation: null
};

const shared = {
    groupedCategoryElements: [],
    categoryElements: []
};

function mountWithRealStore() {
    const reducers = {
        closeModal: jest.fn(() => Promise.resolve),
        transferPointsLogistics: jest.fn(() => initialValues.transferPointsLogistics),
        modal: jest.fn(() => initialValues.modal),
        sourceNodes: jest.fn(() => initialValues.sourceNodes),
        destinationNodes: jest.fn(() => initialValues.destinationNodes),
        sourceProducts: jest.fn(() => initialValues.sourceProducts),
        sourceStorageLocations: jest.fn(() => initialValues.sourceStorageLocations),
        destinationStorageLocations: jest.fn(() => initialValues.destinationStorageLocations),
        destinationProducts: jest.fn(() => initialValues.destinationProducts),
        sourceLogisticCenter: jest.fn(() => initialValues.sourceLogisticCenter),
        destinationLogisticCenter: jest.fn(() => initialValues.destinationLogisticCenter),
        sourceStorageLocation: jest.fn(() => initialValues.sourceStorageLocation),
        destinationStorageLocation: jest.fn(() => initialValues.destinationStorageLocation),
        shared: jest.fn(() => shared),
        saveTransferPointLogistics: jest.fn(() => Promise.resolve())
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <CreateTransferPointLogistic initialValues={initialValues} />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('CreateTransferPointLogistic', () => {
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
        expect(enzymeWrapper.exists('#dd_transferPointLogistics_transferPoint')).toBe(true);
        expect(enzymeWrapper.exists('#dd_transferPointLogistics_sourceNode')).toBe(true);
        expect(enzymeWrapper.exists('#dd_transferPointLogistics_destinationNode')).toBe(true);
        expect(enzymeWrapper.exists('#dd_transferPointLogistics_sourceStorageLocation')).toBe(true);
        expect(enzymeWrapper.exists('#dd_transferPointLogistics_destinationStorageLocation')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointLogistics_logisticSourceCenter')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointLogistics_logisticDestinationCenter')).toBe(true);
        expect(enzymeWrapper.exists('#dd_transferPointLogistics_sourceProduct')).toBe(true);
        expect(enzymeWrapper.exists('#dd_transferPointLogistics_destinationProduct')).toBe(true);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_transferPointLogisticsCreate_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find(ModalFooter).find('#btn_transferPointLogisticsCreate_submit').text()).toEqual('submit');
    });

    it('should call onClose on click of cancel button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find(ModalFooter).find('#btn_transferPointLogisticsCreate_cancel').simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should update component for source node update', () => {
        const props = {
            fieldChange: {
                fieldChangeToggler: true
            },
            dispatchActions: jest.fn(() => Promise.resolve()),
            handleSubmit: jest.fn(),
            resetOnSourceNodeChange: jest.fn(),
            getTransferSourceNodes: jest.fn()
        };
        const value = { sourceNodeId: 1, sourceNode: { name: 'name' } };
        const inputArray = [
            change('createTransferPointLogistics', 'destinationNode', null),
            change('createTransferPointLogistics', 'sourceProduct', null),
            change('createTransferPointLogistics', 'sourceStorageLocation', null),
            change('createTransferPointLogistics', 'destinationProduct', null),
            change('createTransferPointLogistics', 'destinationStorageLocation', null),
            resetOnSourceNodeChange(),
            getTransferDestinationNodes(value.sourceNodeId),
            getNodeStorageLocations(value.sourceNodeId, true),
            getTransferProducts(value.sourceNodeId, true),
            getLogisticCenter(value.sourceNode.name, true)
        ];
        actionProvider.getlogisticsActions = jest.fn(() => {
            return inputArray;
        });

        const wrapper = shallow(<CreateTransferPointLogisticComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, {
            fieldChange: {
                fieldChangeToggler: false,
                currentModifiedField: 'sourceNode',
                currentModifiedValue: value
            }
        }));
        expect(props.dispatchActions).toHaveBeenCalledWith(inputArray);
        expect(props.dispatchActions.mock.calls).toHaveLength(1);
    });

    it('should update component for destination node update', () => {
        const props = {
            fieldChange: {
                fieldChangeToggler: true
            },
            dispatchActions: jest.fn(),
            handleSubmit: jest.fn(),
            resetOnSourceNodeChange: jest.fn(),
            getTransferSourceNodes: jest.fn()
        };
        const value = { destinationNode: { nodeId: 1, name: 'name' } };
        const inputArray = [change('createTransferPointLogistics', 'destinationProduct', null),
            change('createTransferPointLogistics', 'destinationStorageLocation', null),
            resetOnDestinationNodeChange(),
            getNodeStorageLocations(value.destinationNode.nodeId, false),
            getTransferProducts(value.destinationNode.nodeId, false),
            getLogisticCenter(value.destinationNode.name, false)];

        actionProvider.getlogisticsActions = jest.fn(() => {
            return inputArray;
        });

        const wrapper = shallow(<CreateTransferPointLogisticComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, {
            fieldChange: {
                fieldChangeToggler: false,
                currentModifiedField: 'destinationNode',
                currentModifiedValue: value
            }
        }));
        expect(props.dispatchActions.mock.calls).toHaveLength(1);
    });

    it('should update component for source Product update', () => {
        const props = {
            fieldChange: {
                fieldChangeToggler: true
            },
            dispatchActions: jest.fn(),
            handleSubmit: jest.fn(),
            resetOnSourceNodeChange: jest.fn(),
            getTransferSourceNodes: jest.fn()
        };
        actionProvider.getlogisticsActions = jest.fn(() => {
            return [];
        });

        const wrapper = shallow(<CreateTransferPointLogisticComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, {
            fieldChange: {
                fieldChangeToggler: false,
                currentModifiedField: 'sourceProduct',
                currentModifiedValue: { sourceProduct: 'name' }
            }
        }));
        expect(props.dispatchActions.mock.calls).toHaveLength(0);
    });
});
