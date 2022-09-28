import {
    RECEIVE_FILTER_CATEGORY_ELEMENTS,
    RECEIVE_UPDATE_DEVIATION
} from './actions.js';
import { utilities } from '../../../common/services/utilities';

const registryDeviationInitialState = {
    categoryElements: []
};

export const registryDeviation = (state = registryDeviationInitialState, action = {}) => {
    switch (action.type) {
    case RECEIVE_FILTER_CATEGORY_ELEMENTS: {
        return Object.assign({},
            state,
            {
                categoryElements: action.items.value.map(item => ({
                    ...item,
                    deviationPercentage: utilities.isNullOrUndefined(item.deviationPercentage) ? '0.00' : item.deviationPercentage
                }))
            });
    }
    case RECEIVE_UPDATE_DEVIATION: {
        return Object.assign({},
            state,
            {
                status: action.status,
                updatedDeviationToggler: !state.updatedDeviationToggler
            });
    }
    default:
        return state;
    }
};
