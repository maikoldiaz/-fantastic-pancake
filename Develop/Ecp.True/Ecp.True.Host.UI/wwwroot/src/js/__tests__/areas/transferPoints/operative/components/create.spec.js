import setup from '../../../setup.js';
import React from 'react';
import { mount, shallow } from 'enzyme';
import { createStore, combineReducers } from 'redux';
import { Provider } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { httpService } from '../../../../../common/services/httpService';
import CreateTransferPointOperational, { CreateTransferPointOperational as CreateTransferPointOperationalComponent }
    from '../../../../../modules/administration/transferPoints/operative/components/create.jsx';
import { ModalFooter } from '../../../../../common/components/footer/modalFooter.jsx';

const initialValues = {
    transferPointsOperational: {
        transferPoint: {
            name: 'RELACION_TT',
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
        initialValues: {
            sourceNodeType: 'Limite',
            destinationNodeType: 'Automation_z965a'
        },
        createSuccess: false,
        updateSuccess: false,
        fieldChangeToggler: false
    },
    modal: {
        modalKey: ''
    },
    sourceNodes: {},
    destinationNodes: {},
    sourceProducts: {},
    createSuccess: {},
    updateSuccess: {},
    sourceNodeType: '',
    destinationNodeType: ''
};

const shared = {
    groupedCategoryElements: [],
    categoryElements: []
};

function mountWithRealStore() {
    const reducers = {
        closeModal: jest.fn(() => Promise.resolve),
        transferPointsOperational: jest.fn(() => initialValues.transferPointsOperational),
        modal: jest.fn(() => initialValues.modal),
        sourceNodes: jest.fn(() => initialValues.sourceNodes),
        destinationNodes: jest.fn(() => initialValues.destinationNodes),
        sourceProducts: jest.fn(() => initialValues.sourceProducts),
        createSuccess: jest.fn(() => initialValues.createSuccess),
        updateSuccess: jest.fn(() => initialValues.updateSuccess),
        sourceNodeType: jest.fn(() => initialValues.sourceNodeType),
        destinationNodeType: jest.fn(() => initialValues.destinationNodeType),
        shared: jest.fn(() => shared),
        saveTransferPointOperational: jest.fn(() => Promise.resolve())
    };

    const store = createStore(combineReducers(reducers));

    const enzymeWrapper = mount(<Provider store={store} >
        <CreateTransferPointOperational initialValues={initialValues} />
    </Provider>);
    return { store, enzymeWrapper };
}

describe('Create Transfer Point Operational Form', () => {
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
        expect(enzymeWrapper.exists('#dd_transferPointOperational_transferPoint')).toBe(true);
        expect(enzymeWrapper.exists('#dd_transferPointOperational_movementType')).toBe(true);
        expect(enzymeWrapper.exists('#dd_transferPointOperational_type')).toBe(true);
        expect(enzymeWrapper.exists('#dd_transferPointOperational_destinationNode')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointOperational_sourceNodeType')).toBe(true);
        expect(enzymeWrapper.exists('#lbl_transferPointOperational_destinationNodeType')).toBe(true);
        expect(enzymeWrapper.exists('#dd_transferPointOperational_sourceProduct')).toBe(true);
        expect(enzymeWrapper.exists('#dd_transferPointOperational_sourceProductType')).toBe(true);
        expect(enzymeWrapper.exists('#txt_createTransferPointOperational_camp')).toBe(true);
        expect(enzymeWrapper.exists('#txt_createTransferPointOperational_waterCamp')).toBe(true);
        expect(enzymeWrapper.exists('#txt_createTransferPointOperational_correlatedCases')).toBe(true);
        expect(enzymeWrapper.find(ModalFooter).find('#btn_createOperative_cancel').text()).toEqual('cancel');
        expect(enzymeWrapper.find(ModalFooter).find('#btn_createOperative_submit').text()).toEqual('submit');
    });

    it('should call onClose on click of cancel button', () => {
        const { enzymeWrapper } = mountWithRealStore();
        enzymeWrapper.find(ModalFooter).find('#btn_createOperative_cancel').simulate('click');
        expect(enzymeWrapper).toHaveLength(1);
    });

    it('should update component for source node update', () => {
        const props = {
            fieldChange: {
                fieldChangeToggler: true
            },
            handleSubmit: jest.fn(),
            resetValue: jest.fn(),
            getTransferDestinationNodes: jest.fn(),
            getTransferSourceProducts: jest.fn(),
            getNodeType: jest.fn(),
            resetNodeType: jest.fn(),
            getTransferSourceNodes: jest.fn()
        };

        const wrapper = shallow(<CreateTransferPointOperationalComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, {
            fieldChange: {
                fieldChangeToggler: false,
                currentModifiedField: 'sourceNode',
                currentModifiedValue: { sourceNodeId: 1, sourceNode: { name: 'name' } }
            }
        }));
        expect(props.resetValue.mock.calls).toHaveLength(1);
        expect(props.getTransferDestinationNodes.mock.calls).toHaveLength(1);
        expect(props.getTransferSourceProducts.mock.calls).toHaveLength(1);
        expect(props.getNodeType.mock.calls).toHaveLength(1);
    });
    it('should update component for destination node update', () => {
        const props = {
            fieldChange: {
                fieldChangeToggler: true
            },
            handleSubmit: jest.fn(),
            getNodeType: jest.fn(),
            resetNodeType: jest.fn(),
            getTransferSourceNodes: jest.fn()
        };

        const wrapper = shallow(<CreateTransferPointOperationalComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, {
            fieldChange: {
                fieldChangeToggler: false,
                currentModifiedField: 'destinationNode',
                currentModifiedValue: { destinationNode: { nodeId: 1, name: 'name' } }
            }
        }));
        expect(props.getNodeType.mock.calls).toHaveLength(1);
    });
    it('should update component for null source node update', () => {
        const props = {
            fieldChange: {
                fieldChangeToggler: true
            },
            handleSubmit: jest.fn(),
            resetValue: jest.fn(),
            getTransferDestinationNodes: jest.fn(),
            getTransferSourceProducts: jest.fn(),
            getNodeType: jest.fn(),
            resetNodeType: jest.fn(),
            getTransferSourceNodes: jest.fn()
        };

        const wrapper = shallow(<CreateTransferPointOperationalComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, {
            fieldChange: {
                fieldChangeToggler: false,
                currentModifiedField: 'sourceNode',
                currentModifiedValue: null
            }
        }));
        expect(props.resetValue.mock.calls).toHaveLength(1);
        expect(props.getTransferDestinationNodes.mock.calls).toHaveLength(0);
        expect(props.getTransferSourceProducts.mock.calls).toHaveLength(0);
        expect(props.getNodeType.mock.calls).toHaveLength(0);
    });
    it('should update component for null destination node update', () => {
        const props = {
            fieldChange: {
                fieldChangeToggler: true
            },
            handleSubmit: jest.fn(),
            getNodeType: jest.fn(),
            resetNodeType: jest.fn(),
            getTransferSourceNodes: jest.fn()
        };

        const wrapper = shallow(<CreateTransferPointOperationalComponent {...props} />);
        wrapper.setProps(Object.assign({}, props, {
            fieldChange: {
                fieldChangeToggler: false,
                currentModifiedField: 'destinationNode',
                currentModifiedValue: null
            }
        }));
        expect(props.getNodeType.mock.calls).toHaveLength(0);
    });
});
