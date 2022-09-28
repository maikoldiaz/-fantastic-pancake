import {
    RECEIVE_OWNERSHIP_CALCULATION_DATES,
    RECEIVE_OWNERSHIP_CALCULATION_VALIDATIONS,
    SET_OWNERSHIP_CALCULATION_INFO,
    RECEIVE_CREATE_OWNERSHIP_CALCULATION, RECEIVE_OWNERSHIP_FAILURE,
    CLEAR_SEGMENT_AND_DATE,
    RECEIVE_CONCILIATION_TICKET,
    FAILURE_CONCILIATION_TICKET,
    RECEIVE_OWNERSHIPNODE_TICKET_DATA,
    FAILURE_OWNERSHIPNODE_TICKET_DATA,
    SAVE_SELECTED_OWNERSHIP_TICKET,
    RECEIVE_LAST_OPERATIONAL_CONCILIATION_TICKET
} from './actions';
import { RECEIVE_GRID_DATA } from '../../../common/components/grid/actions.js';

import { dateService } from '../../../common/services/dateService';

const ownershipInitialState = {
    segment: null,
    initialDate: null,
    lastCutoffDate: null,
    ticket: null,
    validations: null
};

export const ownership = (state = ownershipInitialState, action = {}) => {
    switch (action.type) {
    case '@@redux-form/CHANGE': {
        if (action.meta.form === 'ownershipCalculationCriteria' && action.meta.field === 'segment') {
            return Object.assign({}, state, { segment: action.payload });
        }
        return state;
    }
    case RECEIVE_OWNERSHIP_CALCULATION_DATES: {
        return Object.assign({}, state,
            {
                initialDate: action.dates.ownership ? dateService.format(action.dates.ownership) : action.dates.ownership,
                lastCutoffDate: action.dates.cutoff ? dateService.format(action.dates.cutoff) : action.dates.cutoff,
                refreshDateToggler: !state.refreshDateToggler
            });
    }
    case RECEIVE_OWNERSHIP_CALCULATION_VALIDATIONS: {
        return Object.assign({}, state,
            {
                validations: action.validations
            });
    }
    case SET_OWNERSHIP_CALCULATION_INFO: {
        return Object.assign({}, state,
            {
                ticket: action.ticket
            });
    }
    case RECEIVE_CREATE_OWNERSHIP_CALCULATION: {
        return Object.assign({}, state,
            {
                status: action.status,
                refreshToggler: !state.refreshToggler
            });
    }
    case CLEAR_SEGMENT_AND_DATE: {
        return Object.assign({}, state, {
            segment: null,
            initialDate: null
        });
    }
    case RECEIVE_OWNERSHIP_FAILURE: {
        return Object.assign({}, state, { failureToggler: !state.failureToggler });
    }
    case RECEIVE_CONCILIATION_TICKET: {
        return Object.assign({}, state, {
            conciliationSuccessToggler: !state.conciliationSuccessToggler
        });
    }
    case FAILURE_CONCILIATION_TICKET: {
        return Object.assign({}, state, {
            conciliationErrorToggler: !state.conciliationErrorToggler
        });
    }
    case RECEIVE_OWNERSHIPNODE_TICKET_DATA: {
        return Object.assign({}, state, {
            ownershipNodesSuccessToggler: !state.ownershipNodesSuccessToggler,
            ownershipNodesData: action.ownershipNodesData.value
        });
    }
    case FAILURE_OWNERSHIPNODE_TICKET_DATA: {
        return Object.assign({}, state, {
            ownershipNodesErrorToggler: !state.ownershipNodesErrorToggler
        });
    }
    case SAVE_SELECTED_OWNERSHIP_TICKET: {
        return Object.assign({}, state, {
            selectedTicket: action.ticket
        });
    }
    case RECEIVE_LAST_OPERATIONAL_CONCILIATION_TICKET: {
        return Object.assign({}, state, {
            lastOperationalConciliationTicket: action.ticket.value
        });
    }
    case RECEIVE_GRID_DATA: {
        return Object.assign({}, state, {
            refreshedGridData: !state.refreshedGridData
        });
    }
    default:
        return state;
    }
};
