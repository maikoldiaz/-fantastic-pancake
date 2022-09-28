import * as actions from '../../../modules/administration/transformSettings/actions';
import { transformSettings } from '../../../modules/administration/transformSettings/reducers';
import { constants } from '../../../common/services/constants';

describe('Reducer for transform settings', () => {
    const initialState = {
        ready: false, initialValues: null,
        nodesReadyToggler: false, receiveNodes: 'receiveNodes', refreshToggler: false,
        deleteToggler: false, editToggler: false,
        origin: {
            units: [
                { elementId: 4 }
            ],
            sourceNodes: [
                {
                    nodeId: 5
                }
            ]
        },
        destination: {
            sourceNodes: [
                {
                    nodeId: 10
                }
            ]
        },
        mode: constants.Modes.Create
    };

    it('should receive source nodes', () => {
        const action = {
            type: actions.RECEIVE_GET_SOURCENODES,
            mode: constants.Modes.Create,
            sourceNodes: [],
            units: []
        };

        const newState = Object.assign({}, initialState, {
            ready: true, mode: constants.Modes.Create, nodesReadyToggler: true,
            origin: { sourceNodes: action.sourceNodes, units: action.units },
            destination: { sourceNodes: action.sourceNodes, units: action.units }
        });
        expect(transformSettings(initialState, action)).toEqual(newState);
    });

    it('should receive destination nodes', () => {
        const action = {
            name: 'receiveNodes',
            destinationNodes: [],
            type: actions.RECEIVE_GET_DESTINATIONNODES
        };
        const newState = Object.assign({}, initialState, {
            receiveNodes:
                Object.assign({}, action.name, {
                    destinationNodes: action.destinationNodes
                })
        });
        expect(transformSettings(initialState, action)).toEqual(newState);
    });

    it('should receive source products', () => {
        const action = {
            name: 'receiveNodes',
            sourceProducts: [],
            type: actions.RECEIVE_GET_SOURCEPRODUCTS
        };

        const newState = Object.assign({}, initialState, {
            receiveNodes: Object.assign({}, action.name, {
                sourceProducts: action.sourceProducts
            })
        });
        expect(transformSettings(initialState, action)).toEqual(newState);
    });

    it('should receive destination products', () => {
        const action = {
            name: 'receiveNodes',
            destinationProducts: [],
            type: actions.RECEIVE_GET_DESTINATIONPRODUCTS
        };

        const newState = Object.assign({}, initialState, {
            receiveNodes: Object.assign({}, action.name, {
                destinationProducts: action.destinationProducts
            })
        });
        expect(transformSettings(initialState, action)).toEqual(newState);
    });
    it('should search source nodes', () => {
        const action = {
            name: 'receiveNodes',
            searchedSourceNodes: [],
            type: actions.RECEIVE_SEARCH_SOURCENODES
        };

        const newState = Object.assign({}, initialState, {
            receiveNodes: Object.assign({}, action.name, {
                searchedSourceNodes: action.searchedSourceNodes
            })
        });
        expect(transformSettings(initialState, action)).toEqual(newState);
    });

    it('should clear search nodes', () => {
        const action = {
            type: actions.CLEAR_SEARCH_SOURCENODES,
            name: 'receiveNodes'
        };

        const newState = Object.assign({}, initialState, {
            receiveNodes: Object.assign({}, action.name, {
                searchedSourceNodes: []
            })
        });

        expect(transformSettings(initialState, action)).toEqual(newState);
    });

    it('should create transformation', () => {
        const action = {
            type: actions.RECEIVE_CREATE_TRANSFORMATION,
            status: 'create'
        };

        const newState = Object.assign({}, initialState, {
            status: action.status,
            refreshToggler: !initialState.refreshToggler
        });

        expect(transformSettings(initialState, action)).toEqual(newState);
    });
    it('should receive update transformation', () => {
        const action = {
            type: actions.RECEIVE_UPDATE_TRANSFORMATION,
            status: 'update'
        };

        const newState = Object.assign({}, initialState, {
            status: action.status,
            refreshToggler: !initialState.refreshToggler
        });

        expect(transformSettings(initialState, action)).toEqual(newState);
    });
    it('should reset transformation', () => {
        const action = {
            type: actions.RESET_TRANSFORMATION_POPUP
        };

        const newState = Object.assign({},
            initialState,
            {
                origin: {},
                destination: {},
                ready: false,
                initialValues: null,
                transformation: null
            });
        expect(transformSettings(initialState, action)).toEqual(newState);
    });

    it('should delete transformation', () => {
        const action = {
            type: actions.RECEIVE_DELETE_TRANSFORMATION,
            status: 'delete'
        };

        const newState = Object.assign({},
            initialState,
            {
                status: action.status,
                deleteToggler: !initialState.deleteToggler
            });

        expect(transformSettings(initialState, action)).toEqual(newState);
    });

    it('should clear transformation', () => {
        const action = {
            type: actions.CLEAR_TRANSFORMATION_DATA,
            name: 'receiveNodes'
        };

        const newState = Object.assign({}, initialState, {
            receiveNodes: Object.assign({}, action.name, {
                destinationNodes: [],
                sourceProducts: [],
                destinationProducts: []
            })
        });

        expect(transformSettings(initialState, action)).toEqual(newState);
    });

    it('should initialize edit transformation', () => {
        const action = {
            type: actions.REQUEST_EDIT_INITIALIZE_TRANSFORMATION,
            transformation: {}
        };

        const newState = Object.assign({},
            initialState,
            {
                editToggler: !initialState.editToggler,
                transformation: action.transformation
            });

        expect(transformSettings(initialState, action)).toEqual(newState);
    });

    it('check for default case', () => {
        const action = {
            type: 'anyValue'
        };
        expect(transformSettings(initialState, action)).toEqual(initialState);
    });

    it('should get transformation info', () => {
        const action = {
            type: actions.RECEIVE_GET_TRANSFORMATION_INFO,
            transformation: {
                originDestinationNodeId: 1,
                originSourceProductId: 2,
                originDestinationProductId: 3,
                originMeasurementId: 4,
                originSourceNodeId: 5,
                destinationDestinationNodeId: 6,
                destinationSourceProductId: 7,
                destinationDestinationProductId: 8,
                destinationMeasurementId: 9,
                destinationSourceNodeId: 10,
                messageTypeId: 1
            },
            origin: {
                destinationNodes: [
                    {
                        nodeId: 1
                    }
                ],
                sourceProducts: [
                    { productId: 2 }
                ],
                destinationProducts: [
                    { productId: 3 }
                ]
            },
            destination: {
                destinationNodes: [
                    {
                        nodeId: 1
                    }
                ],
                sourceProducts: [
                    { productId: 2 }
                ],
                destinationProducts: [
                    { productId: 3 }
                ]
            }
        };

        const actionResult = transformSettings(initialState, action);
        const newState = Object.assign({}, initialState, {
            ready: initialState.mode === constants.Modes.Update,
            origin: Object.assign({}, initialState.origin, {
                destinationNodes: action.origin.destinationNodes,
                sourceProducts: action.origin.sourceProducts,
                destinationProducts: action.origin.destinationProducts
            }),
            destination: Object.assign({}, initialState.destination, {
                destinationNodes: action.destination.destinationNodes,
                sourceProducts: action.destination.sourceProducts,
                destinationProducts: action.destination.destinationProducts
            })
        });
        expect(actionResult.ready).toEqual(newState.ready);
        expect(actionResult.origin).toEqual(newState.origin);
        expect(actionResult.destination).toEqual(newState.destination);
    });

    it('should get transformation info for second message type', () => {
        const action = {
            type: actions.RECEIVE_GET_TRANSFORMATION_INFO,
            transformation: {
                originDestinationNodeId: 1,
                originSourceProductId: 2,
                originDestinationProductId: 3,
                originMeasurementId: 4,
                originSourceNodeId: 5,
                destinationDestinationNodeId: 6,
                destinationSourceProductId: 7,
                destinationDestinationProductId: 8,
                destinationMeasurementId: 9,
                destinationSourceNodeId: 10,
                messageTypeId: 2
            },
            origin: {
                destinationNodes: [
                    {
                        nodeId: 1
                    }
                ],
                sourceProducts: [
                    { productId: 2 }
                ],
                destinationProducts: [
                    { productId: 3 }
                ]
            },
            destination: {
                destinationNodes: [
                    {
                        nodeId: 1
                    }
                ],
                sourceProducts: [
                    { productId: 2 }
                ],
                destinationProducts: [
                    { productId: 3 }
                ]
            }
        };

        const actionResult = transformSettings(initialState, action);
        const newState = Object.assign({}, initialState, {
            ready: initialState.mode === constants.Modes.Update,
            origin: Object.assign({}, initialState.origin, {
                destinationNodes: action.origin.destinationNodes,
                sourceProducts: action.origin.sourceProducts,
                destinationProducts: action.origin.destinationProducts
            }),
            destination: Object.assign({}, initialState.destination, {
                destinationNodes: action.destination.destinationNodes,
                sourceProducts: action.destination.sourceProducts,
                destinationProducts: action.destination.destinationProducts
            })
        });
        expect(actionResult.ready).toEqual(newState.ready);
        expect(actionResult.origin).toEqual(newState.origin);
        expect(actionResult.destination).toEqual(newState.destination);
    });
});
