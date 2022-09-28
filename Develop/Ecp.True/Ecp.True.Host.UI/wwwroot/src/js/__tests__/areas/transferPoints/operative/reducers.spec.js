import * as actions from '../../../../modules/administration/transferPoints/operative/actions';
import { transferPointsOperational } from '../../../../modules/administration/transferPoints/operative/reducers';

describe('Reducers for Transfer Points Operational', () => {
    const initialState = {
        transferPoint: {},
        sourceNodes: {},
        fieldChange: {
            fieldChangeToggler: false
        }
    };

    it('should handle action INIT_DELETE_TRANSFER_POINT_ROW',
        function () {
            const action = {
                type: actions.INIT_DELETE_TRANSFER_POINT_ROW,
                row: {}
            };
            const newState = Object.assign({}, initialState, { transferPoint: action.row });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_UPDATE_TRANSFER_POINT',
        function () {
            const action = {
                type: actions.RECEIVE_UPDATE_TRANSFER_POINT
            };
            const newState = Object.assign({}, initialState, { updateSuccess: true });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_DELETE_TRANSFER_POINT',
        function () {
            const action = {
                type: actions.RECEIVE_DELETE_TRANSFER_POINT
            };
            const newState = Object.assign({}, initialState, { deleteSuccess: true });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_CREATE_TRANSFER_POINT',
        function () {
            const action = {
                type: actions.RECEIVE_CREATE_TRANSFER_POINT
            };
            const newState = Object.assign({}, initialState, { createSuccess: true });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_NODE_TYPE_NOT_FOUND',
        function () {
            const action = {
                type: actions.RECEIVE_NODE_TYPE_NOT_FOUND
            };
            const newState = Object.assign({}, initialState, { nodeTypeFailure: true });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_TRANSFER_SOURCE_NODES',
        function () {
            const action = {
                type: actions.RECEIVE_TRANSFER_SOURCE_NODES,
                nodes: {
                    value: []
                }
            };

            const newState = Object.assign({}, initialState, { sourceNodes: action.nodes.value });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_TRANSFER_DESTINATION_NODES',
        function () {
            const action = {
                type: actions.RECEIVE_TRANSFER_DESTINATION_NODES,
                nodes: {
                    value: {}
                }
            };
            const newState = Object.assign({}, initialState, { destinationNodes: action.nodes.value });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_TRANSFER_SOURCE_PRODUCTS',
        function () {
            const action = {
                type: actions.RECEIVE_TRANSFER_SOURCE_PRODUCTS,
                products: {
                    value: {}
                }
            };
            const newState = Object.assign({}, initialState, { sourceProducts: action.products.value });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_SOURCE_NODE_TYPE',
        function () {
            const action = {
                type: actions.RECEIVE_SOURCE_NODE_TYPE,
                nodeType: {
                    name: 'name'
                }
            };
            const newState = Object.assign({}, initialState, { sourceNodeType: action.nodeType.name });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_DESTINATION_NODE_TYPE',
        function () {
            const action = {
                type: actions.RECEIVE_DESTINATION_NODE_TYPE,
                nodeType: {
                    name: 'name'
                }
            };
            const newState = Object.assign({}, initialState, { destinationNodeType: action.nodeType.name });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });

    it('should handle action INIT_UPDATE_TRANSFER_POINT_ROW',
        function () {
            const action = {
                type: actions.INIT_UPDATE_TRANSFER_POINT_ROW,
                row: {
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
                }
            };
            const newState = Object.assign({}, initialState, {
                initialValues: {
                    operativeNodeRelationshipId: action.row.operativeNodeRelationshipId,
                    transferPoint: { name: action.row.transferPoint },
                    movementType: { name: action.row.movementType },
                    sourceProduct: { product: { name: action.row.sourceProduct } },
                    sourceProductType: { name: action.row.sourceProductType },
                    sourceNode: { sourceNode: { name: action.row.sourceNode } },
                    destinationNode: { destinationNode: { name: action.row.destinationNode } },
                    sourceField: action.row.sourceField,
                    fieldWaterProduction: action.row.fieldWaterProduction,
                    relatedSourceField: action.row.relatedSourceField,
                    sourceNodeType: action.row.sourceNodeType,
                    destinationNodeType: action.row.destinationNodeType,
                    rowVersion: action.row.rowVersion
                }
            });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });

    it('should handle action REFRESH_TRANSFER_POINT_ROW',
        function () {
            const action = {
                type: actions.REFRESH_TRANSFER_POINT_ROW
            };
            const newState = Object.assign({}, initialState, { transferPoint: {} });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });

    it('should handle action REFRESH_CREATE_SUCCESS',
        function () {
            const action = {
                type: actions.REFRESH_CREATE_SUCCESS
            };
            const newState = Object.assign({}, initialState, { createSuccess: {} });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });

    it('should handle action RESET_NODE_TYPE',
        function () {
            const action = {
                type: actions.RESET_NODE_TYPE
            };
            const newState = Object.assign({}, initialState, {
                destinationNodeType: null,
                sourceNodeType: null,
                destinationNodes: [],
                sourceProducts: [],
                nodeTypeFailure: false
            });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });
    it('should handle form field change of source node',
        function () {
            const action = {
                type: '@@redux-form/CHANGE',
                meta: {
                    form: 'createTransferPointOperational',
                    field: 'sourceNode',
                    payload: {}
                }
            };
            const newState = Object.assign({}, initialState, {
                destinationNodeType: null,
                sourceNodeType: null,
                destinationNodes: [],
                sourceProducts: [],
                nodeTypeFailure: false,
                fieldChange: {
                    fieldChangeToggler: !initialState.fieldChange.fieldChangeToggler,
                    currentModifiedField: action.meta.field,
                    currentModifiedValue: action.payload
                }
            });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });

    it('should handle form field change of destination node',
        function () {
            const action = {
                type: '@@redux-form/CHANGE',
                meta: {
                    form: 'createTransferPointOperational',
                    field: 'destinationNode',
                    payload: {}
                }
            };
            const newState = Object.assign({}, initialState, {
                fieldChange: {
                    fieldChangeToggler: !initialState.fieldChange.fieldChangeToggler,
                    currentModifiedField: action.meta.field,
                    currentModifiedValue: action.payload
                }
            });
            expect(transferPointsOperational(initialState, action)).toEqual(newState);
        });
});
