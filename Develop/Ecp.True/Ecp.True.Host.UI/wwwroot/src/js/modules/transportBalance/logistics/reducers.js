import {
    RECEIVE_ADD_LOGISTICS_TICKET,
    RECEIVE_DATE_FOR_SEGMENT,
    SET_INITIAL_DATE,
    CLEAR_LOGISTICS_OWNERSHIP_REQUEST_DATA,
    CLEAR_SEARCH_NODES,
    ON_SEGMENT_SELECT,
    RECEIVE_SEARCH_NODES,
    SET_LOGISTICS_INFO,
    RECEIVE_LOGISTICS_VALIDATION_DATA,
    RECEIVE_LOGISTICS_OFFICIAL_PERIODS,
    SET_IS_PERIOD_VALID,
    RECEIVE_LOGISTICS_FAILURE
} from './actions.js';

import { dateService } from '../../../common/services/dateService';

const logisticsInitialState = {
    operational: {
        lastOwnershipDate: null,
        refreshToggler: false,
        initialDate: null
    },
    official: {
        lastOwnershipDate: null,
        refreshToggler: false,
        initialDate: null
    }
};

export const logistics = (state = logisticsInitialState, action = {}) => {
    switch (action.type) {
    case RECEIVE_ADD_LOGISTICS_TICKET:
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    status: action.status,
                    refreshToggler: !state[action.name].refreshToggler
                })
            });
    case RECEIVE_DATE_FOR_SEGMENT:
        return Object.assign({}, state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    refreshDateToggler: !state[action.name].refreshDateToggler,
                    lastOwnershipDate: action.date ? dateService.format(action.date) : action.date
                })
            });
    case SET_INITIAL_DATE:
        return Object.assign({}, state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    initialDate: action.date
                })
            });
    case CLEAR_LOGISTICS_OWNERSHIP_REQUEST_DATA:
        return Object.assign({}, state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    lastOwnershipDate: null,
                    initialDate: null
                })
            });
    case CLEAR_SEARCH_NODES:
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    searchedNodes: []
                })
            });
    case ON_SEGMENT_SELECT:
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    selectedSegment: action.selectedSegment
                })
            });
    case RECEIVE_SEARCH_NODES:
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    searchedNodes: action.nodes
                })
            });
    case SET_LOGISTICS_INFO: {
        return Object.assign({}, state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    logisticsInfo: action.logisticsInfo
                })
            });
    }
    case RECEIVE_LOGISTICS_VALIDATION_DATA:
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    validationData: action.validationData,
                    validationDataToggler: !state[action.name].validationDataToggler
                })
            });
    case RECEIVE_LOGISTICS_OFFICIAL_PERIODS:
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    periods: action.periods
                })
            });
    case SET_IS_PERIOD_VALID:
        return Object.assign({},
            state,
            {
                [action.name]: Object.assign({}, state[action.name], {
                    isPeriodValid: action.isPeriodValid
                })
            });
    case RECEIVE_LOGISTICS_FAILURE:
        return Object.assign({}, state, { [action.name]: Object.assign({}, state[action.name], { failureToggler: !state.failureToggler }) });
    default:
        return state;
    }
};
