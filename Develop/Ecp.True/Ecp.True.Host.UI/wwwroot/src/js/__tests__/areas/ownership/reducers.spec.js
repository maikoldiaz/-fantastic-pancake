import * as actions from '../../../modules/transportBalance/ownership/actions';
import { ownership } from '../../../modules/transportBalance/ownership/reducers';

describe('Reducer for ownership balance', () => {
    const initialState = {
        segment: null,
        initialDate: null,
        lastCutoffDate: null,
        ticket: null,
        validations: {}
    };

    it('@@redux-form/CHANGE', () => {
        const action = {
            type: '@@redux-form/CHANGE', meta: { form: 'ownershipCalculationCriteria', field: 'segment' },
            payload: 1
        };
        const state = Object.assign({}, initialState, { segment: 1 });
        const newState = Object.assign({}, initialState, { segment: action.payload });

        expect(ownership(state, action)).toEqual(newState);
    });

    it('@@redux-form/CHANGE when meta form is not ownershipCalculationCriteria', () => {
        const action = {
            type: '@@redux-form/CHANGE', meta: {},
            payload: 1
        };
        const state = Object.assign({}, initialState, { segment: 1 });
        expect(ownership(state, action)).toEqual(state);
    });


    it('should receive ownership calculation dates', () => {
        const action = {
            type: actions.RECEIVE_OWNERSHIP_CALCULATION_DATES,
            dates: { ownership: '21-Nov-19', cutoff: '23-Nov-19' }
        };
        const newState = Object.assign({}, initialState, { initialDate: '2019-11-21T00:00:00Z', lastCutoffDate: '2019-11-23T00:00:00Z', refreshDateToggler: true });
        expect(ownership(initialState, action)).toEqual(newState);
    });

    it('should receive ownership calculation validation', () => {
        const action = {
            type: actions.RECEIVE_OWNERSHIP_CALCULATION_VALIDATIONS,
            validations: { errorMessage: 'nodeOwnershipCouldBeFound', result: 'true' }
        };
        const newState = Object.assign({}, initialState, { validations: { errorMessage: 'nodeOwnershipCouldBeFound', result: 'true' } });
        expect(ownership(initialState, action)).toEqual(newState);
    });

    it('should set ownership calculation info', () => {
        const action = {
            type: actions.SET_OWNERSHIP_CALCULATION_INFO,
            ticket: { startDate: '03-dec-19', endDate: '05-dec-19', categoryElementId: 1 }
        };
        const newState = Object.assign({}, initialState, { ticket: { startDate: '03-dec-19', endDate: '05-dec-19', categoryElementId: 1 } });
        expect(ownership(initialState, action)).toEqual(newState);
    });

    it('should receive and create ownership calculation ticket', () => {
        const action = {
            type: actions.RECEIVE_CREATE_OWNERSHIP_CALCULATION,
            status: 'true',
            refreshToggler: 'true'
        };
        const newState = Object.assign({}, initialState, { status: 'true', refreshToggler: true });
        expect(ownership(initialState, action)).toEqual(newState);
    });

    it('should return default state', () => {
        const action = {
            type: 'SET_OWNERSHIP_CALCULATION_INFO1'
        };
        const state = Object.assign({}, initialState, { step: 0 });

        expect(ownership(state, action)).toEqual(state);
    });

    it('should clear segment and data', () => {
        const action = {
            type: actions.CLEAR_SEGMENT_AND_DATE,
            segment: null,
            initialDate: null
        };
        const newState = Object.assign({}, initialState, { segment: null, initialDate: null });
        expect(ownership(initialState, action)).toEqual(newState);
    });

    it('should receive ownership failure', () => {
        const action = {
            type: actions.RECEIVE_OWNERSHIP_FAILURE,
            failureToggler: 'true'
        };
        const newState = Object.assign({}, initialState, { failureToggler: true });
        expect(ownership(initialState, action)).toEqual(newState);
    });

    it('should receive conciliation successful', () => {
        const action = {
            type: actions.RECEIVE_CONCILIATION_TICKET
        };
        const originalState = Object.assign({}, initialState, { conciliationSuccessToggler: false });
        const newState = Object.assign({}, initialState, { conciliationSuccessToggler: true });
        expect(ownership(originalState, action)).toEqual(newState);
    });

    it('should receive conciliation failure', () => {
        const action = {
            type: actions.FAILURE_CONCILIATION_TICKET,
            response: {}
        };
        const originalState = Object.assign({}, initialState, { conciliationErrorToggler: false });
        const newState = Object.assign({}, initialState, {
            conciliationErrorToggler: true
        });
        expect(ownership(originalState, action)).toEqual(newState);
    });

    it('should receive ownership node data', () => {
        const action = {
            type: actions.RECEIVE_OWNERSHIPNODE_TICKET_DATA,
            ownershipNodesData: {
                value: []
            }
        };
        const originalState = Object.assign({}, initialState, { ownershipNodesSuccessToggler: false });
        const newState = Object.assign({}, initialState, {
            ownershipNodesSuccessToggler: true,
            ownershipNodesData: []
        });
        expect(ownership(originalState, action)).toEqual(newState);
    });

    it('should failure ownership node data', () => {
        const action = {
            type: actions.FAILURE_OWNERSHIPNODE_TICKET_DATA,
        };
        const originalState = Object.assign({}, initialState, { ownershipNodesErrorToggler: false });
        const newState = Object.assign({}, initialState, {
            ownershipNodesErrorToggler: true
        });
        expect(ownership(originalState, action)).toEqual(newState);
    });

    it('should failure ownership node data', () => {
        const action = {
            type: actions.SAVE_SELECTED_OWNERSHIP_TICKET,
            ticket: { ticketId: 123 }
        };
        const originalState = Object.assign({}, initialState, { selectedTicket: null });
        const newState = Object.assign({}, initialState, {
            selectedTicket: { ticketId: 123 }
        });
        expect(ownership(originalState, action)).toEqual(newState);
    });

    it('should receive the last operation conciliation ticket data', () => {
        const action = {
            type: actions.RECEIVE_LAST_OPERATIONAL_CONCILIATION_TICKET,
            ticket: { value: [] }
        };
        const originalState = Object.assign({}, initialState, { lastOperationalConciliationTicket: null });
        const newState = Object.assign({}, initialState, {
            lastOperationalConciliationTicket: []
        });
        expect(ownership(originalState, action)).toEqual(newState);
    });
});
