import {
    RECEIVE_GET_SOURCENODES,
    RECEIVE_GET_DESTINATIONNODES,
    RECEIVE_GET_SOURCEPRODUCTS,
    RECEIVE_GET_DESTINATIONPRODUCTS,
    RECEIVE_SEARCH_SOURCENODES,
    CLEAR_SEARCH_SOURCENODES,
    RECEIVE_CREATE_TRANSFORMATION,
    RECEIVE_UPDATE_TRANSFORMATION,
    RESET_TRANSFORMATION_POPUP,
    RECEIVE_DELETE_TRANSFORMATION,
    RECEIVE_GET_TRANSFORMATION_INFO,
    CLEAR_TRANSFORMATION_DATA,
    REQUEST_EDIT_INITIALIZE_TRANSFORMATION
} from './actions.js';
import { constants } from '../../../common/services/constants.js';

function buildInitialValues(action, state) {
    const transformation = action.transformation;
    const origin = action.origin;
    const destination = action.destination;
    const originUnits = state.origin.units;
    const destUnits = state.origin.units;
    if (transformation.messageTypeId === 1) {
        return {
            origin: {
                destinationNode: origin.destinationNodes.find(t => t.nodeId === transformation.originDestinationNodeId),
                sourceProduct: origin.sourceProducts.find(t => t.productId === transformation.originSourceProductId),
                destinationProduct: origin.destinationProducts.find(t => t.productId === transformation.originDestinationProductId),
                measurementUnit: originUnits.find(u => u.elementId === transformation.originMeasurementId),
                sourceNode: state.origin.sourceNodes.find(n => n.nodeId === transformation.originSourceNodeId)
            },
            destination: {
                destinationNode: destination.destinationNodes.find(t => t.nodeId === transformation.destinationDestinationNodeId),
                sourceProduct: destination.sourceProducts.find(t => t.productId === transformation.destinationSourceProductId),
                destinationProduct: destination.destinationProducts.find(t => t.productId === transformation.destinationDestinationProductId),
                measurementUnit: destUnits.find(u => u.elementId === transformation.destinationMeasurementId),
                sourceNode: state.destination.sourceNodes.find(n => n.nodeId === transformation.destinationSourceNodeId)
            },
            rowVersion: transformation.rowVersion
        };
    }

    return {
        origin: {
            sourceProduct: origin.sourceProducts.find(t => t.productId === transformation.originSourceProductId),
            measurementUnit: originUnits.find(u => u.elementId === transformation.originMeasurementId),
            sourceNode: state.origin.sourceNodes.find(n => n.nodeId === transformation.originSourceNodeId)
        },
        destination: {
            sourceProduct: destination.sourceProducts.find(t => t.productId === transformation.destinationSourceProductId),
            measurementUnit: destUnits.find(u => u.elementId === transformation.destinationMeasurementId),
            sourceNode: state.destination.sourceNodes.find(n => n.nodeId === transformation.destinationSourceNodeId)
        },
        rowVersion: transformation.rowVersion
    };
}
export const transformSettings = (state = { ready: false, origin: {}, destination: {}, initialValues: null }, action = {}) => {
    const sectionState = state[action.name];
    switch (action.type) {
    case RECEIVE_GET_SOURCENODES: {
        return Object.assign({},
            state,
            {
                ready: action.mode === constants.Modes.Create,
                mode: action.mode,
                nodesReadyToggler: !state.nodesReadyToggler,
                origin: Object.assign({}, state.origin, {
                    sourceNodes: action.sourceNodes.slice(),
                    units: action.units.slice()
                }),
                destination: Object.assign({}, state.destination, {
                    sourceNodes: action.sourceNodes.slice(),
                    units: action.units.slice()
                })
            });
    }
    case RECEIVE_GET_DESTINATIONNODES: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, sectionState, {
                    destinationNodes: action.destinationNodes
                })
            });
    }
    case RECEIVE_GET_SOURCEPRODUCTS: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, sectionState, {
                    sourceProducts: action.sourceProducts
                })
            });
    }
    case RECEIVE_GET_DESTINATIONPRODUCTS: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, sectionState, {
                    destinationProducts: action.destinationProducts
                })
            });
    }
    case RECEIVE_SEARCH_SOURCENODES: {
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, sectionState, {
                    searchedSourceNodes: action.searchedSourceNodes
                })
            });
    }
    case CLEAR_SEARCH_SOURCENODES:
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, sectionState, {
                    searchedSourceNodes: []
                })
            });
    case RECEIVE_CREATE_TRANSFORMATION:
    case RECEIVE_UPDATE_TRANSFORMATION:
        return Object.assign({},
            state,
            {
                status: action.status,
                refreshToggler: !state.refreshToggler
            });
    case RESET_TRANSFORMATION_POPUP:
        return Object.assign({},
            state,
            {
                origin: {},
                destination: {},
                ready: false,
                initialValues: null,
                transformation: null
            });
    case RECEIVE_DELETE_TRANSFORMATION:
        return Object.assign({},
            state,
            {
                status: action.status,
                deleteToggler: !state.deleteToggler
            });
    case RECEIVE_GET_TRANSFORMATION_INFO: {
        return Object.assign({},
            state,
            {
                initialValues: buildInitialValues(action, state),
                ready: state.mode === constants.Modes.Update,
                origin: Object.assign({}, state.origin, {
                    destinationNodes: action.origin.destinationNodes,
                    sourceProducts: action.origin.sourceProducts,
                    destinationProducts: action.origin.destinationProducts
                }),
                destination: Object.assign({}, state.destination, {
                    destinationNodes: action.destination.destinationNodes,
                    sourceProducts: action.destination.sourceProducts,
                    destinationProducts: action.destination.destinationProducts
                })
            });
    }
    case CLEAR_TRANSFORMATION_DATA:
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, sectionState, {
                    destinationNodes: [],
                    sourceProducts: [],
                    destinationProducts: []
                })
            });
    case REQUEST_EDIT_INITIALIZE_TRANSFORMATION:
        return Object.assign({},
            state,
            {
                editToggler: !state.editToggler,
                transformation: action.transformation
            });
    default:
        return state;
    }
};

