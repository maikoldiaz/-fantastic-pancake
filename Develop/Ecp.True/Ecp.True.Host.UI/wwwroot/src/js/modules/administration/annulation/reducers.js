import { INIT_ANNULATION_TYPES, UPDATE_ANNULATION_TYPES, RESET_ANNULATION_TYPES, RECEIVE_SAVE_ANNULATION, INIT_ANNULATION } from './actions.js';
import { constants } from '../../../common/services/constants';

const initialState = {
    fieldChange: { fieldChangeToggler: false },
    source: { movement: 0, node: 0, product: 0 },
    annulation: { movement: 0, node: 0, product: 0 }
};

export const annulations = function (state = initialState, action = {}) {
    const sectionState = state[action.name];
    switch (action.type) {
    case INIT_ANNULATION_TYPES: {
        return Object.assign({}, state, {
            source: Object.assign({}, state.source, {
                movement: 0, node: 0, product: 0
            }),
            annulation: Object.assign({}, state.annulation, {
                movement: 0, node: 0, product: 0
            }),
            initialValues: { isActive: true }
        });
    }
    case UPDATE_ANNULATION_TYPES: {
        let updateFieldValue;
        if (action.field === constants.Annulations.Fields.Movement) {
            updateFieldValue = action.value.elementId;
        } else {
            updateFieldValue = action.value.originTypeId === constants.OriginTypes.None ? constants.OriginTypes.None : 0;
        }
        return Object.assign({}, state, {
            [action.name]: Object.assign({}, sectionState, {
                [action.field]: updateFieldValue
            })
        });
    }
    case RESET_ANNULATION_TYPES: {
        return Object.assign({}, state, {
            [action.name]: Object.assign({}, sectionState, {
                [action.field]: 0
            })
        });
    }
    case RECEIVE_SAVE_ANNULATION: {
        return Object.assign({}, state, {
            saveSuccess: !state.saveSuccess
        });
    }
    case INIT_ANNULATION : {
        return Object.assign({}, state, {
            initialValues: action.initialValues,
            source: {
                movement: action.initialValues.annulation.movement.elementId,
                product: action.initialValues.annulation.product.originTypeId === constants.OriginTypes.None ? constants.OriginTypes.None : 0,
                node: action.initialValues.annulation.node.originTypeId === constants.OriginTypes.None ? constants.OriginTypes.None : 0
            },
            annulation: {
                movement: action.initialValues.source.movement.elementId,
                product: action.initialValues.source.product.originTypeId === constants.OriginTypes.None ? constants.OriginTypes.None : 0,
                node: action.initialValues.source.node.originTypeId === constants.OriginTypes.None ? constants.OriginTypes.None : 0
            }
        });
    }
    case '@@redux-form/CHANGE': {
        if (action.meta.form === 'annulation') {
            return Object.assign({}, state, {
                fieldChange: {
                    fieldChangeToggler: !state.fieldChange.fieldChangeToggler,
                    currentModifiedField: action.meta.field,
                    currentModifiedValue: action.payload
                }
            });
        }
        return state;
    }
    default:
        return state;
    }
};
