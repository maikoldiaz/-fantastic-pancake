import * as actions from '../../../../modules/administration/transferPoints/logistics/actions';
import { transferPointsLogistics } from '../../../../modules/administration/transferPoints/logistics/reducers';

describe('Reducers for Transfer Points Logistics', () => {
    const initialState = { transferPoint: {}, sourceNodes: {}, fieldChange: { fieldChangeToggler: false } };

    it('should handle action INIT_TRANSFER_POINT_ROW',
        function () {
            const action = {
                type: actions.INIT_TRANSFER_POINT_ROW,
                row: {
                    notes: null
                }
            };
            const newState = Object.assign({}, initialState, { transferPoint: action.row });
            expect(transferPointsLogistics(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_DELETE_TRANSFER_POINT',
        function () {
            const action = {
                type: actions.RECEIVE_DELETE_TRANSFER_POINT
            };
            const newState = Object.assign({}, initialState, { deleteToggler: true });
            expect(transferPointsLogistics(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_CREATE_TRANSFER_POINT',
        function () {
            const action = {
                type: actions.RECEIVE_CREATE_TRANSFER_POINT
            };
            const newState = Object.assign({}, initialState, { createToggler: true });
            expect(transferPointsLogistics(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_TRANSFER_SOURCE_NODES',
        function () {
            const action = {
                type: actions.RECEIVE_TRANSFER_SOURCE_NODES,
                nodes: {
                    value: []
                }
            };

            const newState = Object.assign({}, initialState, {
                sourceNodes: Array.from(new Set(action.nodes.value.map(node => node.sourceNodeId))).map(id => action.nodes.value.find(node => node.sourceNodeId === id))
            });
            expect(transferPointsLogistics(initialState, action)).toEqual(newState);
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
            expect(transferPointsLogistics(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_SOURCE_TRANSFER_LOCATIONS',
        function () {
            const action = {
                type: actions.RECEIVE_SOURCE_TRANSFER_LOCATIONS,
                storageLocations: {
                    value: {}
                }
            };
            const newState = Object.assign({}, initialState, { sourceStorageLocations: action.storageLocations.value });
            expect(transferPointsLogistics(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_DESTINATION_TRANSFER_LOCATIONS',
        function () {
            const action = {
                type: actions.RECEIVE_DESTINATION_TRANSFER_LOCATIONS,
                storageLocations: {
                    value: {}
                }
            };
            const newState = Object.assign({}, initialState, { destinationStorageLocations: action.storageLocations.value });
            expect(transferPointsLogistics(initialState, action)).toEqual(newState);
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
            expect(transferPointsLogistics(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_TRANSFER_DESTINATION_PRODUCTS',
        function () {
            const action = {
                type: actions.RECEIVE_TRANSFER_DESTINATION_PRODUCTS,
                products: {
                    value: {}
                }
            };
            const newState = Object.assign({}, initialState, { destinationProducts: action.products.value });
            expect(transferPointsLogistics(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_SOURCE_LOGISTIC_CENTER',
        function () {
            const action = {
                type: actions.RECEIVE_SOURCE_LOGISTIC_CENTER,
                logisticCenter: {
                    logisticCenter: {
                        name: 'test',
                        storageLocations: [{ name: '' }]
                    }
                }
            };
            const newState = Object.assign({}, initialState, { sourceLogisticCenter: action.logisticCenter.logisticCenter.storageLocations[0].name });
            expect(transferPointsLogistics(initialState, action)).toEqual(newState);
        });

    it('should handle action RECEIVE_DESTINATION_LOGISTIC_CENTER',
        function () {
            const action = {
                type: actions.RECEIVE_DESTINATION_LOGISTIC_CENTER,
                logisticCenter: {
                    logisticCenter: {
                        name: 'test',
                        storageLocations: [{ name: '' }]
                    }
                }
            };
            const newState = Object.assign({}, initialState, { destinationLogisticCenter: action.logisticCenter.logisticCenter.storageLocations[0].name });
            expect(transferPointsLogistics(initialState, action)).toEqual(newState);
        });

    it('should handle action SET_SOURCE_STORAGE_LOCATION',
        function () {
            const action = {
                type: actions.SET_SOURCE_STORAGE_LOCATION,
                storageLocation: {
                    name: {},
                    storageLocation: { name: 'name' }
                }
            };
            const newState = Object.assign({}, initialState, { sourceStorageLocation: action.storageLocation.storageLocation.name });
            expect(transferPointsLogistics(initialState, action)).toEqual(newState);
        });

    it('should handle action SET_DESTINATION_STORAGE_LOCATION',
        function () {
            const action = {
                type: actions.SET_DESTINATION_STORAGE_LOCATION,
                storageLocation: {
                    name: {},
                    storageLocation: { name: 'name' }
                }
            };
            const newState = Object.assign({}, initialState, { destinationStorageLocation: action.storageLocation.storageLocation.name });
            expect(transferPointsLogistics(initialState, action)).toEqual(newState);
        });

    it('should handle action RESET_ON_SOURCE_NODE_CHANGE',
        function () {
            const action = {
                type: actions.RESET_ON_SOURCE_NODE_CHANGE
            };
            const newState = Object.assign({}, initialState, {
                destinationStorageLocations: [],
                destinationNodes: [],
                sourceStorageLocations: [],
                sourceProducts: [],
                destinationProducts: [],
                sourceStorageLocation: null,
                sourceLogisticCenter: null,
                destinationStorageLocation: null,
                destinationLogisticCenter: null
            });
            expect(transferPointsLogistics(initialState, action)).toEqual(newState);
        });

    it('should handle form field change',
        function () {
            const action = {
                type: '@@redux-form/CHANGE',
                meta: {
                    form: 'createTransferPointLogistics',
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
            expect(transferPointsLogistics(initialState, action)).toEqual(newState);
        });
});
